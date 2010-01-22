using System;
using System.Windows.Forms;
using SIL.Sponge.ConfigTools;
using SIL.Sponge.Model;
using SilUtils;

//using Palaso.Reporting;

namespace SIL.Sponge
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// Can't get this working yet.
			//Logger.Init();
			//ErrorReport.EmailAddress = "david_olson@sil.org";
			//ErrorReport.AddStandardProperties();
			//ExceptionHandler.Init();

			PortableSettingsProvider.SettingsFilePath = SpongeProject.MainProjectsFolder;

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
