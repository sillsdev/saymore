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
	static class SpongeRunner
	{
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

			if (!Directory.Exists(MainAppSettingsFolder))
				Directory.CreateDirectory(MainAppSettingsFolder);

			if (!Directory.Exists(SpongeProject.ProjectsFolder))
				Directory.CreateDirectory(SpongeProject.ProjectsFolder);

			PortableSettingsProvider.SettingsFilePath = MainAppSettingsFolder;

			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr");

			LocalizationManager.Enabled = true;
			LocalizationManager.Initialize(Path.Combine(MainAppSettingsFolder, "Localizations"));

			LocalizeItemDlg.SetDialogBounds += LocalizeItemDlg_SetDialogBounds;
			LocalizeItemDlg.SetDialogSplitterPosition += LocalizeItemDlg_SetDialogSplitterPosition;
			LocalizeItemDlg.SaveDialogBounds += LocalizeItemDlg_SaveDialogBounds;
			LocalizeItemDlg.SaveDialogSplitterPosition += LocalizeItemDlg_SaveDialogSplitterPosition;

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
		public static string MainAppSettingsFolder
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(
					Environment.SpecialFolder.MyDocuments), "Sponge");
			}
		}
	}
}
