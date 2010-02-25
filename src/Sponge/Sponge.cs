using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
using SIL.Sponge.ConfigTools;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;
using SilUtils;

//using Palaso.Reporting;

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public static class Sponge
	{
		public const string ApplicationFolderName = "Sponge";
		public const string ProjectFolderName = "Projects";
		public const string ProjectFileExtention = "sprj";
		public const string SessionFolderName = "Sessions";
		public const string SessionFileExtension = "session";
		public const string PeopleFolderName = "People";
		public const string PersonFileExtension = "person";

		private static string s_mainAppFldr;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[STAThread]
		static void Main()
		{
			// Can't get this working yet.
			//Logger.Init();
			//ErrorReport.EmailAddress = "david_olson@sil.org";
			//ErrorReport.AddStandardProperties();
			//ExceptionHandler.Init();

			if (!Directory.Exists(MainApplicationFolder))
				Directory.CreateDirectory(MainApplicationFolder);

			if (!Directory.Exists(SpongeProject.ProjectsFolder))
				Directory.CreateDirectory(SpongeProject.ProjectsFolder);

			PortableSettingsProvider.SettingsFilePath = MainApplicationFolder;

			//Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr");

			LocalizationManager.Enabled = true;
			LocalizationManager.Initialize(Path.Combine(MainApplicationFolder, "Localizations"));

			LocalizeItemDlg.SetDialogBounds += LocalizeItemDlg_SetDialogBounds;
			LocalizeItemDlg.SetDialogSplitterPosition += LocalizeItemDlg_SetDialogSplitterPosition;
			LocalizeItemDlg.SaveDialogBounds += LocalizeItemDlg_SaveDialogBounds;
			LocalizeItemDlg.SaveDialogSplitterPosition += LocalizeItemDlg_SaveDialogSplitterPosition;

			string path = Path.GetDirectoryName(Application.ExecutablePath);
			SessionFileInfoTemplateList.Initialize(Path.Combine(path, "SessionFileInfoTemplates.xml"));

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			SpongeProject prj = null;

			using (var dlg = new WelcomeDlg())
			{
				if (dlg.ShowDialog() == DialogResult.OK)
					prj = dlg.SpongeProject;
			}

			if (prj != null)
				Application.Run(new MainWnd(prj));
		}

		#region methods for saving and setting localization dialog settings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the location of the splitter on the localization dialog box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static int LocalizeItemDlg_SetDialogSplitterPosition()
		{
			return Settings.Default.LocalizationDlgSplitterPos;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the size and location of the localization dialog box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static void LocalizeItemDlg_SetDialogBounds(LocalizeItemDlg dlg)
		{
			var rc = Settings.Default.LocalizationDlgBounds;
			if (rc.Height < 0)
				dlg.StartPosition = FormStartPosition.CenterScreen;
			else
				dlg.Bounds = rc;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the location of the splitter on the localization dialog box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void LocalizeItemDlg_SaveDialogSplitterPosition(int pos)
		{
			Settings.Default.LocalizationDlgSplitterPos = pos;
			Settings.Default.Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the size and location of the localization dialog box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void LocalizeItemDlg_SaveDialogBounds(LocalizeItemDlg dlg)
		{
			Settings.Default.LocalizationDlgBounds = dlg.Bounds;
			Settings.Default.Save();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the parent folder in which all the Sponge settings and projects are stored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string MainApplicationFolder
		{
			get
			{
				// I would rather just return the Path.Combine result, but I'm getting a
				// strange test failure on the Palaso build machine either in this property
				// or in the SpongeProject.ProjectsFolder property where calling it once
				// works, but the second time, it seems to concatenate to the value returned
				// in a previous call. Grrr!
				if (s_mainAppFldr == null)
				{
					s_mainAppFldr = Path.Combine(Environment.GetFolderPath(
						Environment.SpecialFolder.MyDocuments), ApplicationFolderName);
				}

				return s_mainAppFldr;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the localized text for "All Files" displayed in open file dialog boxes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string OFDlgAllFileTypeText
		{
			get
			{
				return LocalizationManager.LocalizeString("AllFileType", "All Files (*.*)",
					"Text shown in file type box of open file dialog boxes.",
					"Miscellaneous Strings");
			}
		}
	}
}
