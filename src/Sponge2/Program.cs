using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SIL.Sponge;
using SIL.Sponge.ConfigTools;
using Sponge2.ProjectChosingAndCreating;
using Sponge2.Properties;

namespace Sponge2
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

//			Properties.Settings.Default.Reset();
//			Settings.Default.Save();

			SetUpErrorHandling();


			MruProjects.Initialize(Properties.Settings.Default.MRUList);
			bool userWantsToOpenAnotherProject = true;

			if (MruProjects.Latest != null && File.Exists(MruProjects.Latest))
			{
				string path = MruProjects.Latest;
				userWantsToOpenAnotherProject = RunWindowForProject(path);
			}

			while(userWantsToOpenAnotherProject)
			{
				userWantsToOpenAnotherProject = OnOpenProject();
			}

			Properties.Settings.Default.Save();
		}

		private static bool RunWindowForProject(string path)
		{
			using (var bootStrapper = new BootStrapper(path))
			{
				Shell shell = bootStrapper.CreateShell();
				Application.Run(shell);
				return shell.UserWantsToOpenADifferentProject;
			}
		}

		private static bool OnOpenProject()
		{
			using (var dlg = new PretendWelcomeDialog())// WelcomeDialog())
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					MruProjects.AddNewPath(dlg.ProjectPath);
					MruProjects.Save();
					Properties.Settings.Default.Save();
					return RunWindowForProject(dlg.ProjectPath);
				}
				return false;
			}
		}

		private static void SetUpErrorHandling()
		{
			Palaso.Reporting.ErrorReport.AddProperty("EmailAddress", "david_olson@sil.org");//hahah
			Palaso.Reporting.ErrorReport.AddStandardProperties();
			Palaso.Reporting.ExceptionHandler.Init();
		}
	}
}
