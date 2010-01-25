using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
using SIL.Sponge.ConfigTools;
using SIL.Sponge.Model;
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
