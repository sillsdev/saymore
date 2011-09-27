using System;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Localization;
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

			//SetUpLocalization();

			Settings.Default.MRUList = MruFiles.Initialize(Settings.Default.MRUList, 4);
			_applicationContainer = new ApplicationContainer(false);

			SetUpErrorHandling();
			SetUpReporting();

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

			var msg = GetString("UI.ProjectWindow.LoadingProjectErrorMsg",
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
			SetUILanguage();

			// Copy the localization file to where the settings file is located.
			var localizationFilePath = Path.Combine(PortableSettingsProvider.SettingsFileFolder, "SayMore.tmx");
			if (!File.Exists(localizationFilePath))
			{
				var srcLocalizationFilePath = FileLocator.GetFileDistributedWithApplication("SayMore.tmx");
				File.Copy(srcLocalizationFilePath, localizationFilePath);
			}

			L10NMngr = LocalizationManager.Create("SayMore", "SayMore", PortableSettingsProvider.SettingsFileFolder);

			//LocalizeItemDlg.SaveDialogSplitterPosition += (pos =>
			//    Settings.Default.LocalizeDlgSplitterPos = pos);

			//LocalizeItemDlg.SetDialogSplitterPosition += (currPos =>
			//    (Settings.Default.LocalizeDlgSplitterPos > 0 ? Settings.Default.LocalizeDlgSplitterPos : currPos));

			//LocalizeItemDlg.SaveDialogBounds += (dlg =>
			//    Settings.Default.LocalizeDlgBounds = dlg.Bounds);

			//LocalizeItemDlg.SetDialogBounds += (dlg =>
			//{
			//    if (!Settings.Default.LocalizeDlgBounds.IsEmpty)
			//        dlg.Bounds = Settings.Default.LocalizeDlgBounds;
			//});

			//LocalizeItemDlg.StringsLocalized += (() =>
			//{
			//});
		}

		/// ------------------------------------------------------------------------------------
		private static void SetUILanguage()
		{
			string langId = Settings.Default.UserInterfaceLanguage;

			// Specifying the UI language on the command-line trumps the one in
			// the settings file (i.e. the one set in the options dialog box).
			foreach (var arg in Environment.GetCommandLineArgs()
				.Where(arg => arg.ToLower().StartsWith("/uilang:") || arg.ToLower().StartsWith("-uilang:")))
			{
				langId = arg.Substring(8);
				break;
			}

			LocalizationManager.UILanguageId = (string.IsNullOrEmpty(langId) ?
				LocalizationManager.kDefaultLang : langId);
		}

		/// ------------------------------------------------------------------------------------
		private static LocalizationManager L10NMngr { get; set; }

		/// ------------------------------------------------------------------------------------
		internal static void SaveOnTheFlyLocalizations()
		{
			if (L10NMngr != null)
				L10NMngr.SaveOnTheFlyLocalizations();
		}

		/// ------------------------------------------------------------------------------------
		internal static void ReapplyLocalizationsToAllObjects()
		{
			if (L10NMngr != null)
				L10NMngr.ReapplyLocalizationsToAllObjects();
		}

		/// ------------------------------------------------------------------------------------
		internal static void RefreshToolTipsOnLocalizationManager()
		{
			if (L10NMngr != null)
				L10NMngr.RefreshToolTips();
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetUILanguageId()
		{
			return LocalizationManager.UILanguageId;
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetString(string id, string defaultText)
		{
			return (L10NMngr == null ? defaultText : L10NMngr.LocalizeString(id, defaultText));
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetString(string id, string defaultText, string comment)
		{
			return (L10NMngr == null ? defaultText : L10NMngr.LocalizeString(id, defaultText, comment, null));
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetStringForObject(object obj)
		{
			return GetStringForObject(obj, "????");
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetStringForObject(object obj, string defaultText)
		{
			return (L10NMngr == null ? defaultText : (L10NMngr.GetString(obj) ?? defaultText));
		}

		/// ------------------------------------------------------------------------------------
		internal static void RegisterForLocalization(object obj, string id)
		{
			if (L10NMngr != null)
				L10NMngr.LocalizeObject(obj, id);
		}

		/// ------------------------------------------------------------------------------------
		internal static void RegisterForLocalization(object obj, string id, string defaultText)
		{
			if (L10NMngr == null)
				return;

			L10NMngr.LocalizeString(id, defaultText);
			L10NMngr.LocalizeObject(obj, id);
		}

		/// ------------------------------------------------------------------------------------
		internal static void RegisterForLocalization(object obj, string id,
			string defaultText, string comment, string group)
		{
			if (L10NMngr == null)
				return;

			L10NMngr.LocalizeObject(obj, id, defaultText, null, comment, group);
			L10NMngr.LocalizeObject(obj, id);
		}

		#endregion
	}
}
