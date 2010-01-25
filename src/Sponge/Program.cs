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
	static class Program
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

			PortableSettingsProvider.SettingsFilePath = SpongeProject.MainProjectsFolder;

			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr");

			LocalizationManager.Enabled = true;
			LocalizationManager.Initialize(Path.Combine(SpongeProject.MainProjectsFolder, "Localizations"));

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			string prj = null;

			using (var dlg = new WelcomeDlg())
			{
				if (dlg.ShowDialog() == DialogResult.OK)
					prj = string.Empty;
			}

			if (!string.IsNullOrEmpty(prj))
				Application.Run(new MainWnd());
		}
	}
}
