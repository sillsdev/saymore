using System;
using System.Windows.Forms;
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

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainWnd());
		}
	}
}
