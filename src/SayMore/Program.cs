using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DesktopAnalytics;
using L10NSharp;
using SIL.Code;
using SIL.Extensions;
using SIL.IO;
using SIL.Reporting;
using SIL.Windows.Forms.Miscellaneous;
using SIL.Windows.Forms.PortableSettingsProvider;
using SayMore.Media;
using SayMore.Properties;
using SayMore.UI;
using SayMore.UI.Overview;
using SayMore.UI.ProjectWindow;
using SayMore.Model;
using SayMore.Utilities;
using SIL.Windows.Forms.Reporting;
using SIL.WritingSystems;
using static System.String;

namespace SayMore
{
	static class Program
	{
		public const string kCompanyAbbrev = "SIL";
		public const int kFileLoadError = 13;

		/// <summary>
		/// We have one project open at a time, and this helps us bootstrap the project and
		/// properly dispose of various things when the project is closed.
		/// </summary>
		private static ProjectContext _projectContext;
		private static Mutex _oneInstancePerProjectMutex;
		private static string _mutexId;

		private static string _pathOfLoadedProjectFile;
		private static ApplicationContainer _applicationContainer;
		private static Font _dialogFont;

		public delegate void PersonMetadataChangedHandler();
		public static event PersonMetadataChangedHandler PersonDataChanged;

		private static readonly List<Exception> _pendingExceptionsToReportToAnalytics = new List<Exception>();
        private static UserInfo s_userInfo;

		private static bool s_handlingFirstChanceExceptionThreadsafe = false;
		private static bool s_handlingFirstChanceExceptionUnsafe = false;
		private static int s_countOfContiguousFirstChanceOutOfMemoryExceptions = 0;
		private static string s_productName;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[STAThread]
		static void Main()
		{
			// I had put the following line of code in to fix SP-436 after reading this article:
			// https://stackoverflow.com/questions/4077822/net-4-0-and-the-dreaded-onuserpreferencechanged-hang
			// Microsoft.Win32.SystemEvents.UserPreferenceChanged += delegate { };
			// But then I found this page: https://www.aaronlerch.com/blog/2008/12/15/debugging-ui/
			// which describes how to find out where the actual problem is.
			Thread.CurrentThread.Name = "UI";

			// This is pretty annoying: When, because .Net doesn't have a font style of SemiBold
			// (e.g. Segoe UI SemiBold), fonts having that style are assumed to be bold, but
			// when some controls (e.g. Label) are set to a SemiBold font, they are displayed as
			// bold, so we'll create our own, forcing the style to regular, which seems to work.
			// Don't use SystemFonts.DefaultFont because that always returns "Microsoft Sans Serif"
			// and SystemFonts.DialogFont always returns "Tahoma", regardless of OS.
			// See: https://benhollis.net/blog/2007/04/11/setting-the-correct-default-font-in-net-windows-forms-apps/
			_dialogFont = new Font(SystemFonts.MessageBoxFont, FontStyle.Regular);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// The following not only get the location of the settings file used for the analytics stuff. It also
			// detects corruption and deletes it if needed so SayMore doesn't crash.
			var analyticsConfigFilePath = GetAnalyticsConfigFilePath(); // Analytics settings.

			if ((Control.ModifierKeys & Keys.Shift) > 0 && !IsNullOrEmpty(analyticsConfigFilePath))
			{
				var confirmationString = LocalizationManager.GetString("MainWindow.ConfirmDeleteUserSettingsFile",
					"Do you want to delete your user settings? (This will clear your most-recently-used project list, window positions, UI language settings, etc. It will not affect your SayMore project data.)");

				if (DialogResult.Yes ==
					MessageBox.Show(confirmationString, ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
				{
					File.Delete(analyticsConfigFilePath);
					File.Delete(new PortableSettingsProvider().GetFullSettingsFilePath());
				}
			}

			//bring in settings from any previous version
			//NB: this code doesn't actually work, because for some reason Saymore uses its own settings code,
			//(which emits a "settings" file rather than "user.config"),
			//and which apparently doesn't use the application version to trigger the following technique:
			// Insight from Tom: Looks like ALL user settings in SayMore use the PortableSettingsProvider. I think the
			// idea of this was to facilitate installing SayMore to a thumb drive or whatever, so it could be totally
			// portable. This provider does not attach version numbers to the settings files (or the cryptic GUIDs or
			// whatever to their containing folders), so once NeedUpgrade gets set to false (the very first time SayMore
			// is run), it will never again be true. It's easy enough to store NeedUpgrade in the normal
			// user.config file by removing the attribute in Settings.Designer.cs that causes it to be handled by the
			// custom provider. But PortableSettingsProvider would need to implement IApplicationSettingsProvider and
			// implement Upgrade in an appropriate way to handle this.
			if (Settings.Default.NeedUpgrade) //TODO: this doesn't get triggered with David's custom settings
			{
				//see https://stackoverflow.com/questions/3498561/net-applicationsettingsbase-should-i-call-upgrade-every-time-i-load
				Settings.Default.Upgrade();	//TODO: and this doesn't seem to actually do anything with David's custom settings
				Settings.Default.NeedUpgrade = false;
				Settings.Default.Save();
			}
			//so, as a hack because this is biting our users *now*.
			//this hack begins the damage control started above, when from 1.6.52 to 1.6.53, we changed the namespace
			//of the grid settings. It removes the old settings, talks to the user, and waits for the user to restart.
			else
			{
				try
				{
					// ReSharper disable once NotAccessedVariable
					var x = Settings.Default.SessionsListGrid; //we want this to throw if the last version used the SILGrid, and this one uses the BetterGrid
					// ReSharper disable once RedundantAssignment
					x = Settings.Default.PersonListGrid;
				}
				catch (Exception)
				{
					string path="";
					try
					{
						ErrorReport.NotifyUserOfProblem("We apologize for the inconvenience, but to complete this upgrade, SayMore needs to exit. Please run it again to complete the upgrade.");

						var s = Application.LocalUserAppDataPath;
						s = s.Substring(0, s.IndexOf("Local", StringComparison.InvariantCultureIgnoreCase) + 5);
						path = s.CombineForPath("SayMore", "SayMore.Settings");
						File.Delete(path);

						Settings.Default.MRUList = MruFiles.Initialize(Settings.Default.MRUList, 4);
						//leave this reminder to our post-restart self
						if(MruFiles.Latest!=null)
							File.WriteAllText(MRULatestReminderFilePath, MruFiles.Latest);

						//Application.Restart(); won't work, because the settings will still get saved

						Environment.FailFast("SayMore quitting hard to prevent old settings from being saved again.");
					}
					catch (Exception error)
					{
						ErrorReport.NotifyUserOfProblem(error,
							"SayMore was unable to find or delete the settings file from the old version of SayMore. Normally, this would be found at " + path);
						Application.Exit();
					}
				}

			}

			//this hack is a continuation of the damage control started above, when from 1.6.52 to 1.6.53, we changed the namespace
			//of the grid settings.
			if (File.Exists(MRULatestReminderFilePath))
			{
				var path = File.ReadAllText(MRULatestReminderFilePath).Trim();
				if(File.Exists(path))
				{
					Settings.Default.MRUList = new StringCollection();
					Settings.Default.MRUList.Add(path);
				}
				File.Delete(MRULatestReminderFilePath);
			}

			Settings.Default.MRUList = MruFiles.Initialize(Settings.Default.MRUList, 4);
			_applicationContainer = new ApplicationContainer(false);

			Logger.Init();
			AppDomain.CurrentDomain.FirstChanceException += FirstChanceHandler;
			Logger.WriteEvent(ApplicationContainer.GetVersionInfo("SayMore version {0}.{1}.{2} {3}    Built on {4}", BuildType.Current));
			Logger.WriteEvent("Visual Styles State: {0}", Application.VisualStyleState);
			SetUpErrorHandling();
			Sldr.Initialize();

            s_userInfo = new UserInfo {UILanguageCode = Settings.Default.UserInterfaceLanguage};

#if DEBUG
			// Always track if this is a debug build, but track to a different segment.io project
			using (new Analytics("twa75xkko9", s_userInfo))
#else
			// If this is a release build, then allow an environment variable to be set to false
			// so that testers aren't generating false analytics
			string feedbackSetting = System.Environment.GetEnvironmentVariable("FEEDBACK");

			var allowTracking = IsNullOrEmpty(feedbackSetting) || feedbackSetting.ToLower() == "yes" || feedbackSetting.ToLower() == "true";

			using (new Analytics("jtfe7dyef3", userInfo, allowTracking))
#endif
			{
				foreach (var exception in _pendingExceptionsToReportToAnalytics)
					Analytics.ReportException(exception);

				bool startedWithCommandLineProject = false;
				var args = Environment.GetCommandLineArgs();
				if (args.Length > 1)
				{
					var possibleProjFile = args[1];
					startedWithCommandLineProject =
						possibleProjFile.EndsWith(Settings.Default.ProjectFileExtension) &&
							File.Exists(possibleProjFile) &&
							OpenProjectWindow(possibleProjFile);
				}

				if (!startedWithCommandLineProject)
					StartUpShellBasedOnMostRecentUsedIfPossible();

				try
				{
					Application.Run();
					Settings.Default.Save();
					Logger.WriteEvent("SayMore shutting down");
					if (s_countOfContiguousFirstChanceOutOfMemoryExceptions > 1)
						Logger.WriteEvent("Total number of contiguous OutOfMemoryExceptions: {0}", s_countOfContiguousFirstChanceOutOfMemoryExceptions);
					Logger.ShutDown();

					SafelyDisposeProjectContext();
				}
				finally
				{
					ReleaseMutexForThisProject();
					Sldr.Cleanup();
					FileSyncHelper.RestartAllStoppedClients();
				}
			}
		}

		public static string GetAnalyticsConfigFilePath()
		{
			try
			{
				return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
			}
			catch (ConfigurationErrorsException e)
			{
				_pendingExceptionsToReportToAnalytics.Add(e);
				File.Delete(e.Filename);
				return e.Filename;
			}
		}

		/// ------------------------------------------------------------------------------------
		public static void ReleaseMutexForThisProject()
		{
			if (_oneInstancePerProjectMutex != null)
			{
				_oneInstancePerProjectMutex.ReleaseMutex();
				_oneInstancePerProjectMutex = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// First, we try to get the mutex quickly and quietly. If that fails and this request
		/// was an explicit user choice (as opposed to merely opening the last project), we put
		/// up a dialog and wait 10 seconds, while we wait for the mutex to come free.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool ObtainTokenForThisProject(string pathToSayMoreProjectFile)
		{
			ReleaseMutexForThisProject(); // Just to be sure.
			Guard.AgainstNull(pathToSayMoreProjectFile, "pathToSayMoreProjectFile");
			var mutexIdBldr = new StringBuilder(pathToSayMoreProjectFile);
			for (int i = 0; i < mutexIdBldr.Length; i++)
			{
				if (mutexIdBldr[i] == Path.DirectorySeparatorChar || mutexIdBldr[i] == Path.VolumeSeparatorChar)
					mutexIdBldr[i] = '-';
			}
			_mutexId = mutexIdBldr.ToString();
			try
			{
				_oneInstancePerProjectMutex = Mutex.OpenExisting(_mutexId);
				if (_oneInstancePerProjectMutex.WaitOne(1500, false))
					return true;
			}
			catch (Exception e)
			{
				if (e is WaitHandleCannotBeOpenedException || e is AbandonedMutexException)
				{
					bool thisThreadGrantedOwnership;
					_oneInstancePerProjectMutex = new Mutex(true, _mutexId, out thisThreadGrantedOwnership);
					if (thisThreadGrantedOwnership)
						return true;
				}
				else
					throw;
			}

			if (Application.MessageLoop)
			{
				using (var dlg = new LoadingDlg(LocalizationManager.GetString("MainWindow.WaitingForOtherSayMoreInstance",
						"Waiting for other instance of SayMore to finish...")))
				{
					var worker = new BackgroundWorker();
					worker.DoWork += WaitForProjectMutex;
					worker.WorkerSupportsCancellation = true;
					dlg.BackgroundWorker = worker;
					dlg.Show(null);

					if (dlg.DialogResult == DialogResult.OK)
						return true;
				}

				ErrorReport.NotifyUserOfProblem(Format(
					LocalizationManager.GetString("MainWindow.ProjectOpenInOtherSayMore",
					"Another instance of SayMore is already open with this project:\r\n{0}\r\n\r\nIf you cannot find that instance of SayMore, restart your computer."),
					pathToSayMoreProjectFile));
			}

			_oneInstancePerProjectMutex = null;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		private static void WaitForProjectMutex(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker worker = (BackgroundWorker)sender;
			var dlg = (LoadingDlg)e.Argument;

			int attempts = 0;
			while ((e.Result == null || ((bool)e.Result == false && attempts++ < 5)) && !worker.CancellationPending && !e.Cancel)
			try
			{
				Thread.Sleep(2000);
				dlg.Invoke(new Action(delegate {e.Result = _oneInstancePerProjectMutex.WaitOne(1, false); }));
			}
			catch (Exception error)
			{
				ErrorReport.NotifyUserOfProblem(error,
					LocalizationManager.GetString("MainWindow.ProblemOpeningSayMoreProject",
					"There was a problem opening the SayMore project which might require that you restart your computer."));
			}
		}

		/// ------------------------------------------------------------------------------------
		private static string MRULatestReminderFilePath
		{
			get
			{
				var s = Application.LocalUserAppDataPath;
				s = s.Substring(0, s.IndexOf("Local", StringComparison.InvariantCultureIgnoreCase) + 5);
				return Path.Combine(s, "SayMore", "lastFilePath.txt");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static void ArchiveProjectUsingIMDI(Form parentForm)
		{
			// SP-767: some project changes not being saved before archiving
			SaveProjectMetadata();
			_projectContext.Project.ArchiveProjectUsingIMDI(parentForm);
		}

		/// ------------------------------------------------------------------------------------
		public static void ArchiveProjectUsingRAMP(Form parentForm)
		{
			SaveProjectMetadata();
			_projectContext.Project.ArchiveProjectUsingRAMP(parentForm);
		}

		public static List<XmlException> FileLoadErrors
		{
			get 
			{
				if (_projectContext == null || _projectContext.Project == null) // This can happen during unit testing
					return new List<XmlException>(0);
				return _projectContext.Project.FileLoadErrors;
			}
		}

		/// ------------------------------------------------------------------------------------
		public delegate void SaveDelegate();

		/// ------------------------------------------------------------------------------------
		public static void SaveProjectMetadata()
		{
			// This can happen during unit testing
			if (_projectContext == null) return;

			var views = _projectContext.ProjectWindow.Views;
			foreach (var view in views)
			{
				var savable = view as ISaveable;
				if (savable == null) continue;

				// this can happen when creating new session from device
				if (view.InvokeRequired)
					view.Invoke(new SaveDelegate(savable.Save));
				else
					savable.Save();
			}
		}

		/// ------------------------------------------------------------------------------------
		private static void SafelyDisposeProjectContext()
		{
			var localCopy = _projectContext;
			if (localCopy != null)
			{
				lock (localCopy)
				{
					localCopy.Dispose();
					_projectContext = null;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string SilCommonDataFolder
		{
			get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), kCompanyAbbrev); }
		}

		/// ------------------------------------------------------------------------------------
		public static string CommonAppDataFolder
		{
			get { return Path.Combine(SilCommonDataFolder, Application.ProductName); }
		}

		/// ------------------------------------------------------------------------------------
		public static Font DialogFont
		{
			get { return _dialogFont ?? SystemFonts.MessageBoxFont; }
		}

		/// ------------------------------------------------------------------------------------
		private static void StartUpShellBasedOnMostRecentUsedIfPossible()
		{
			// In Windows 7, just holding down the shift key while starting the app. works to
			// prevent the last project from being loaded. However, I found on XP (at least
			// running XP mode) that holding the shift key while starting an app. does
			// nothing... as in the app. is not launched. Therefore, running SayMore with an
			// 'nl' command-line option will also suppress loading the last project.
			var noLoadArg = Environment.GetCommandLineArgs().FirstOrDefault(a => "-nl-NL/nl/NL".Contains(a));

			if (MruFiles.Latest == null || !File.Exists(MruFiles.Latest) ||
				(Control.ModifierKeys == Keys.Shift) || noLoadArg != null ||
				!OpenProjectWindow(MruFiles.Latest))
			{
				//since the message pump hasn't started yet, show the UI for choosing when it is
				Application.Idle += ChooseAnotherProject;
			}
		}

		/// ------------------------------------------------------------------------------------
		private static bool OpenProjectWindow(string projectPath)
		{
			Debug.Assert(_projectContext == null);

			Logger.WriteEvent("Attempting to open project {0}", projectPath);

			try
			{
				if (!ObtainTokenForThisProject(projectPath))
					return false;

				// Remove this call if we end only wanting to show the splash screen
				// at app. startup. Right now it's shown whenever a project is loaded.
				_applicationContainer.ShowSplashScreen();

				_applicationContainer.SetProjectNameOnSplashScreen(projectPath);
				_projectContext = _applicationContainer.CreateProjectContext(projectPath);
				_projectContext.ProjectWindow.Closed += HandleProjectWindowClosed;
				_projectContext.ProjectWindow.Activated += HandleProjectWindowActivated;
				_projectContext.ProjectWindow.Show();
				_pathOfLoadedProjectFile = projectPath;
				Application.Idle += SaveLastOpenedProjectInMRUList;
				return true;
			}
			catch (OutOfMemoryException oomex)
			{
				Logger.WriteEvent("Out of memory exception in Program.OpenProjectWindow:\r\n{0}", oomex.ToString());
				MessageBox.Show(oomex.ToString());
				Application.Exit();
			}
			catch (Exception e)
			{
				HandleErrorOpeningProjectWindow(e, projectPath);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// By the time we get here, we know the app. has settled down after loading a project.
		/// Now that the project has been loaded without crashing, save the project as the
		/// most recently opened project. xref: SP-186.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void SaveLastOpenedProjectInMRUList(object sender, EventArgs e)
		{
			Application.Idle -= SaveLastOpenedProjectInMRUList;
			MruFiles.AddNewPath(_pathOfLoadedProjectFile);
			Settings.Default.Save();
		}

		/// ------------------------------------------------------------------------------------
		private static void HandleProjectWindowActivated(object sender, EventArgs e)
		{
			Logger.WriteEvent("Project window activated for project {0}", _projectContext.Project.Name);

			_projectContext.ProjectWindow.Activated -= HandleProjectWindowActivated;
			_applicationContainer.CloseSplashScreen();

			// Sometimes after closing the splash screen the project window
			// looses focus, so do this.
			_projectContext.ProjectWindow.Activate();
		}

		/// ------------------------------------------------------------------------------------
		private static void HandleErrorOpeningProjectWindow(Exception error, string projectPath)
		{
			var localCopy = _projectContext;
			if (localCopy != null)
			{
				lock (localCopy)
				{
					localCopy.ProjectWindow.Closed -= HandleProjectWindowClosed;
					localCopy.ProjectWindow.Close();
					localCopy.Dispose();
					_projectContext = null;
				}
			}

			_applicationContainer.CloseSplashScreen();

			var msg = Format(LocalizationManager.GetString("MainWindow.LoadingProjectErrorMsg",
				"{0} had a problem loading the {1} project. Please report this problem " +
				"to the developers by clicking 'Details' below."),
				Application.ProductName, Path.GetFileNameWithoutExtension(projectPath));

			Logger.WriteEvent(msg);
			Logger.WriteEvent("Details:\r\n{0}", error);

			ErrorReport.NotifyUserOfProblem(new ShowAlwaysPolicy(), error, msg);

			Settings.Default.MRUList.Remove(projectPath);
			MruFiles.Initialize(Settings.Default.MRUList);
		}

		/// ------------------------------------------------------------------------------------
		static void ChooseAnotherProject(object sender, EventArgs e)
		{
			Application.Idle -= ChooseAnotherProject;

			while (true)
			{
				using (var dlg = _applicationContainer.CreateWelcomeDialog())
				{
					if (dlg.ShowDialog() != DialogResult.OK)
					{
						Application.Exit();
						return;
					}

					if (OpenProjectWindow(dlg.Model.ProjectSettingsFilePath))
						return;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		static void HandleProjectWindowClosed(object sender, EventArgs e)
		{
			SafelyDisposeProjectContext();
			ReleaseMutexForThisProject();

			if (((ProjectWindow)sender).UserWantsToOpenADifferentProject)
			{
				Application.Idle += ChooseAnotherProject;
			}
			else
			{
				try
				{
					Application.Exit();
				}
				catch (IndexOutOfRangeException ex)
				{
					// This is a kludge to prevent reporting a known (and apparently harmless) bug
					// in .Net. For more info, see SP-438, which has a link to the MS bug report.
					if (!ex.StackTrace.Contains("System.Windows.Forms.Application.ThreadContext.ExitCommon") ||
						!ex.StackTrace.Contains("System.Collections.Hashtable.ValueCollection.CopyTo"))
						throw;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public static void SuspendAudioVideoBackgroundProcesses()
		{
			var localCopy = _projectContext;
			if (localCopy == null)
				return;

			lock (localCopy)
			{
				localCopy.SuspendAudioVideoBackgroundProcesses();
			}
		}

		/// ------------------------------------------------------------------------------------
		public static void ResumeAudioVideoBackgroundProcesses(bool processAllPendingEventsNow)
		{
			var localCopy = _projectContext;
			if (localCopy == null)
				return;

			lock (localCopy)
			{
				if (processAllPendingEventsNow)
					WaitCursor.Show();

				localCopy.ResumeAudioVideoBackgroundProcesses(processAllPendingEventsNow);
			}
			if (processAllPendingEventsNow)
				WaitCursor.Hide();
		}

		/// ------------------------------------------------------------------------------------
		public static void SuspendBackgroundProcesses()
		{
			var localCopy = _projectContext;
			if (localCopy == null)
				return;

			lock (localCopy)
			{
				localCopy.SuspendBackgroundProcesses();
			}
		}

		/// ------------------------------------------------------------------------------------
		public static void ResumeBackgroundProcesses(bool processAllPendingEventsNow)
		{
			var localCopy = _projectContext;
			if (localCopy == null)
				return;

			lock (localCopy)
			{
				if (processAllPendingEventsNow)
					WaitCursor.Show();

				localCopy.ResumeBackgroundProcesses(processAllPendingEventsNow);
			}
			if (processAllPendingEventsNow)
				WaitCursor.Hide();
		}

		/// ------------------------------------------------------------------------------------
		public static void ShowHelpTopic(string topicLink)
		{
			var path = FileLocationUtilities.GetFileDistributedWithApplication("SayMore.chm");
			Help.ShowHelp(new Label(), path, topicLink);

			Analytics.Track("Show Help Topic", new Dictionary<string, string> {
				{"topicLink", topicLink}});
		}


		/// ------------------------------------------------------------------------------------
		private static void SetUpErrorHandling()
		{
			Application.ApplicationExit += (sender, args) => ExternalProcess.CleanUpAllProcesses();
			Application.ThreadException += (sender, args) => ExternalProcess.CleanUpAllProcesses();
			AppDomain.CurrentDomain.UnhandledException += (sender, args) => ExternalProcess.CleanUpAllProcesses();

			ErrorReport.EmailAddress = "saymore_issues@sil.org";
			ErrorReport.AddStandardProperties();
			ExceptionHandler.Init(new WinFormsExceptionHandler());
			ExceptionHandler.AddDelegate((w, e) => Analytics.ReportException(e.Exception));
		}

		/// ------------------------------------------------------------------------------------
		static void FirstChanceHandler(object source, FirstChanceExceptionEventArgs e)
		{
			// Never try to handle another one if we're already handling one. It will probably just make things worse.
			// This check is outside the lock, just in case the attempt to get the lock throws an exception. It's not
			// perfect, but it's better than nothing.
			if (s_handlingFirstChanceExceptionUnsafe)
				return;
			s_handlingFirstChanceExceptionUnsafe = true;

			lock (_applicationContainer)
			{
				// Never try to handle another one if we're already handling one. It will probably just make things worse.
				if (s_handlingFirstChanceExceptionThreadsafe)
					return;
				s_handlingFirstChanceExceptionThreadsafe = true;
				if (!((e.Exception is MissingMethodException) && e.Exception.Message.Contains(".ShortcutKeys")))
				{
					if (e.Exception is OutOfMemoryException)
					{
						if (s_countOfContiguousFirstChanceOutOfMemoryExceptions > 0)
							s_countOfContiguousFirstChanceOutOfMemoryExceptions++;
						else
						{
							Logger.WriteEvent("FirstChanceException event: {0}", e.Exception.ToString());
							s_countOfContiguousFirstChanceOutOfMemoryExceptions = 1;
						}
					}
					else
					{
						if (s_countOfContiguousFirstChanceOutOfMemoryExceptions > 1)
						{
							Logger.WriteEvent("Total number of contiguous OutOfMemoryExceptions: {0}", s_countOfContiguousFirstChanceOutOfMemoryExceptions);
							s_countOfContiguousFirstChanceOutOfMemoryExceptions = 0;
						}
						Logger.WriteEvent("FirstChanceException event: {0}", e.Exception.ToString());
					}
				}
				s_handlingFirstChanceExceptionThreadsafe = false;
			}

			s_handlingFirstChanceExceptionUnsafe = false;
		}

		public static string ProjectSettingsFile => _projectContext.Project.SettingsFilePath;
		public static Project CurrentProject => _projectContext?.Project;
		public static ProjectWindow ProjectWindow => _projectContext?.ProjectWindow;

		public static string ProductName
		{
			get
			{
				if (s_productName == null)
				{
					// Probably could just use Application.ProductName, but the fallback logic here is different.

					Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

					object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
					if (attributes.Length > 0)
						s_productName = ((AssemblyProductAttribute)attributes[0]).Product;

					if (IsNullOrEmpty(s_productName))
					{
						attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
						s_productName = (attributes.Length > 0) ? ((AssemblyTitleAttribute) attributes[0]).Title :
							"SayMore"; // hard-coded fallback
					}
				}
				return s_productName;
			}
		}

		/// <summary>Gets all controls of the desired type</summary>
		public static IEnumerable<T> GetControlsOfType<T>(Control root) where T : Control
		{
			if (root == null)
				yield break;

			if (root is T t)
				yield return t;

			if (!root.HasChildren)
				yield break;

			foreach (var i in from Control c in root.Controls from i in GetControlsOfType<T>(c) select i)
				yield return i;
		}

		/// ------------------------------------------------------------------------------------
		public static void OnPersonDataChanged()
		{
			var handler = PersonDataChanged;
			if (handler != null) handler();
		}

        public static void UpdateUiLanguageForUser(string languageId)
        {
            Analytics.Track("UI language chosen",
                new Dictionary<string, string> {
                    { "Previous", Settings.Default.UserInterfaceLanguage },
                    { "New", languageId } });
            s_userInfo.UILanguageCode = languageId;
            Analytics.IdentifyUpdate(s_userInfo);
            Settings.Default.UserInterfaceLanguage = languageId;
            Logger.WriteEvent("Changed UI Locale to: " + languageId);
            LocalizationManager.SetUILanguage(languageId, true);
        }
    }
}
