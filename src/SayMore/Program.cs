using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Localization;
using Localization.UI;
using Palaso.IO;
using Palaso.Reporting;
using SayMore.Properties;
using SayMore.UI.MediaPlayer;
using SayMore.UI.ProjectWindow;
using SilTools;

namespace SayMore
{
	static class Program
	{
		/// <summary>
		/// We have one project open at a time, and this helps us bootstrap the project and
		/// properly dispose of various things when the project is closed.
		/// </summary>
		private static ProjectContext _projectContext;

		private static string _pathOfLoadedProjectFile;
		private static ApplicationContainer _applicationContainer;
		private static Font _dialogFont;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[STAThread]
		static void Main()
		{
			// This is pretty annoying: When because .Net doesn't have a font style of SemiBold
			// (e.g. Segoe UI SemiBold), fonts having that style are assumed to be bold, but
			// when some controls (e.g. Label) are set to a SemiBold font, the are displayed as
			// bold, so we'll create our own forcing the style to regular, which seems to work.
			// Don't use SystemFonts.DefaultFont because that always returns "Microsoft Sans Serif"
			// and SystemFonts.DialogFont always returns "Tahoma", regardless of OS.
			// See: http://benhollis.net/blog/2007/04/11/setting-the-correct-default-font-in-net-windows-forms-apps/
			_dialogFont = new Font(SystemFonts.MessageBoxFont, FontStyle.Regular);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			//bring in settings from any previous version
			if (Settings.Default.NeedUpgrade)
			{
				//see http://stackoverflow.com/questions/3498561/net-applicationsettingsbase-should-i-call-upgrade-every-time-i-load
				Settings.Default.Upgrade();
				Settings.Default.NeedUpgrade = false;
				Settings.Default.Save();
			}


			Settings.Default.MRUList = MruFiles.Initialize(Settings.Default.MRUList, 4);
			_applicationContainer = new ApplicationContainer(false);

			SetUpErrorHandling();
			SetUpReporting();
			SetUpLocalization();

			var args = Environment.GetCommandLineArgs();
			var firstTimeArg = args.FirstOrDefault(x => x.ToLower().StartsWith("-i"));
			if (firstTimeArg != null)
			{
				using (var dlg = new FirstTimeRunDialog("put filename here"))
					dlg.ShowDialog();
			}

			StartUpShellBasedOnMostRecentUsedIfPossible();

			Application.Run();
			Settings.Default.Save();

			if (_projectContext != null)
				_projectContext.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		public static Font DialogFont
		{
			get { return _dialogFont ?? SystemFonts.MessageBoxFont; }
		}

		/// ------------------------------------------------------------------------------------
		private static void StartUpShellBasedOnMostRecentUsedIfPossible()
		{
			if (MruFiles.Latest == null || !File.Exists(MruFiles.Latest) ||
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

			try
			{
				// Remove this call if we end only wanting to show the splash screen except
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
			_projectContext.ProjectWindow.Activated -= HandleProjectWindowActivated;
			_applicationContainer.CloseSplashScreen();

			// Sometimes after closing the splash screen the project window
			// looses focus, so do this.
			_projectContext.ProjectWindow.Activate();
		}

		/// ------------------------------------------------------------------------------------
		private static void HandleErrorOpeningProjectWindow(Exception error, string projectPath)
		{
			if (_projectContext != null)
			{
				if (_projectContext.ProjectWindow != null)
				{
					_projectContext.ProjectWindow.Closed -= HandleProjectWindowClosed;
					_projectContext.ProjectWindow.Close();
				}

				_projectContext.Dispose();
				_projectContext = null;
			}

			_applicationContainer.CloseSplashScreen();

			var msg = GetString("MainWindow.LoadingProjectErrorMsg",
				"{0} had a problem loading the {1} project. Please report this problem to the developers by clicking 'Details' below.");

			ErrorReport.NotifyUserOfProblem(new ShowAlwaysPolicy(), error, msg,
				Application.ProductName, Path.GetFileNameWithoutExtension(projectPath));

			Settings.Default.MRUList.Remove(projectPath);
			MruFiles.Initialize(Settings.Default.MRUList);
		}

		/// ------------------------------------------------------------------------------------
		static void ChooseAnotherProject(object sender, EventArgs e)
		{
			Application.Idle -= ChooseAnotherProject;
			_applicationContainer.CloseSplashScreen();

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
			_projectContext.Dispose();
			_projectContext = null;

			if (((ProjectWindow)sender).UserWantsToOpenADifferentProject)
			{
				Application.Idle += ChooseAnotherProject;
			}
			else
			{
				Application.Exit();
			}
		}

		/// ------------------------------------------------------------------------------------
		public static void ShowHelpTopic(string topicLink)
		{
			var path = FileLocator.GetFileDistributedWithApplication("SayMore.chm");
			Help.ShowHelp(new Label(), path, topicLink);
			UsageReporter.SendNavigationNotice("Help: "+topicLink);
		}

		/// ------------------------------------------------------------------------------------
		private static void SetUpErrorHandling()
		{
			Application.ApplicationExit += (sender, args) => MPlayerHelper.CleanUpMPlayerProcesses();
			Application.ThreadException += (sender, args) => MPlayerHelper.CleanUpMPlayerProcesses();
			AppDomain.CurrentDomain.UnhandledException += (sender, args) => MPlayerHelper.CleanUpMPlayerProcesses();

			ErrorReport.EmailAddress = "issues@saymore.palaso.org";
			ErrorReport.AddStandardProperties();
			ExceptionHandler.Init();
		}

		/// ------------------------------------------------------------------------------------
		private static void SetUpReporting()
		{
			if (Settings.Default.Reporting == null)
			{
				Settings.Default.Reporting = new ReportingSettings();
				Settings.Default.Save();
			}

			UsageReporter.Init(Settings.Default.Reporting, "saymore.palaso.org", "UA-22170471-3",
#if DEBUG
 true
#else
				false
#endif
);
		}

		#region Localization Manager Methods/Properties
		/// ------------------------------------------------------------------------------------
		public static void SetUpLocalization()
		{
			SetUILanguage(Environment.GetCommandLineArgs());

			// Copy the localization file to where the settings file is located.
			var localizationFileFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			localizationFileFolder = Path.Combine(localizationFileFolder, "SIL");
			localizationFileFolder = Path.Combine(localizationFileFolder, "SayMore");
			var localizationFilePath = Path.Combine(localizationFileFolder, "SayMore.tmx");

			if (!File.Exists(localizationFilePath))
			{
				if (!Directory.Exists(localizationFileFolder))
					Directory.CreateDirectory(localizationFileFolder);

				var srcLocalizationFilePath = FileLocator.GetFileDistributedWithApplication("SayMore.tmx");
				File.Copy(srcLocalizationFilePath, localizationFilePath);
			}

			L10NMngr = LocalizationManager.Create("SayMore", "SayMore", localizationFileFolder);

			LocalizeItemDlg.SetDialogSettings += (dlg =>
				Settings.Default.LocalizationDlgSettings);

			LocalizeItemDlg.SaveDialogSettings += ((dlg, settings) =>
				Settings.Default.LocalizationDlgSettings = settings);

			var ci = CultureInfo.GetCultureInfo(LocalizationManager.UILanguageId);
			if (!LocalizationManager.GetUILanguages(true).Contains(ci))
			{
				var defaultCultureInfo = CultureInfo.GetCultureInfo(LocalizationManager.kDefaultLang);
				var msg = "Your user interface language was previously set to {0} but there " +
					"are no localziations found for that language. Therefore, your user interface " +
					"language will revert to {1}. It's possible the file that contains your " +
					"localized strings is corrupt or missing.";

				ErrorReport.NotifyUserOfProblem(msg, ci.DisplayName, defaultCultureInfo.DisplayName);

				LocalizationManager.UILanguageId = LocalizationManager.kDefaultLang;
				Settings.Default.UserInterfaceLanguage = LocalizationManager.kDefaultLang;
			}
		}

		/// ------------------------------------------------------------------------------------
		public static void SetUILanguage(IEnumerable<string> commandLineArgs)
		{
			string langId = Settings.Default.UserInterfaceLanguage;

			if (commandLineArgs != null)
			{
				// Specifying the UI language on the command-line trumps the one in
				// the settings file (i.e. the one set in the options dialog box).
				foreach (var arg in commandLineArgs
					.Where(arg => arg.ToLower().StartsWith("/uilang:") || arg.ToLower().StartsWith("-uilang:")))
				{
					langId = arg.Substring(8);
					break;
				}
			}

			LocalizationManager.UILanguageId = (string.IsNullOrEmpty(langId) ?
				LocalizationManager.kDefaultLang : langId);
		}

		/// ------------------------------------------------------------------------------------
		private static LocalizationManager L10NMngr { get; set; }

		/// ------------------------------------------------------------------------------------
		internal static void ReapplyLocalizationsToAllObjects()
		{
			if (L10NMngr != null)
				L10NMngr.ReapplyLocalizationsToAllObjects();
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetUILanguageId()
		{
			return LocalizationManager.UILanguageId;
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetString(string id, string defaultText)
		{
			return (L10NMngr == null ? defaultText :
				L10NMngr.GetLocalizedString(id, defaultText, null));
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetString(string id, string defaultText, string comment)
		{
			return (L10NMngr == null ? defaultText :
				L10NMngr.GetLocalizedString(id, defaultText, comment));
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetString(string id, string defaultText, string comment, object obj)
		{
			if (L10NMngr != null)
			{
				L10NMngr.RegisterObjectForLocalizing(obj, id, defaultText, null, null, comment);
				return L10NMngr.GetLocalizedString(id, defaultText, comment);
			}

			return defaultText;
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetString(string id, string defaultText, string comment,
			string defaultToolTipText, string defaultShortcutKey, object obj)
		{
			if (L10NMngr != null)
			{
				L10NMngr.RegisterObjectForLocalizing(obj, id, defaultText,
					defaultToolTipText, defaultShortcutKey, comment);

				return L10NMngr.GetLocalizedString(id, defaultText, comment);
			}

			return defaultText;
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetStringForObject(object obj, string defaultText)
		{
			return (L10NMngr == null ? defaultText : L10NMngr.GetStringForObject(obj, defaultText));
		}

		#endregion
	}
}
