using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SIL.Localization;
using SIL.Sponge;
using SIL.Sponge.ConfigTools;
using SIL.Sponge.Properties;

namespace Sponge2
{
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
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			LocalizationManager.Enabled = true;
			LocalizationManager.Initialize();

			//Settings.Default.Reset();
			//Settings.Default.Save();

			SetUpErrorHandling();

			MruProjects.Initialize(Settings.Default.MRUList);
			bool userWantsToOpenAnotherProject = true;

			if (MruProjects.Latest != null && File.Exists(MruProjects.Latest))
			{
				string path = MruProjects.Latest;
				userWantsToOpenAnotherProject = RunWindowForProject(path);
			}

			while (userWantsToOpenAnotherProject)
			{
				userWantsToOpenAnotherProject = OnOpenProject();
			}

			Settings.Default.Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool RunWindowForProject(string path)
		{
			using (var bootStrapper = new BootStrapper(path))
			{
				Shell shell = bootStrapper.CreateShell();
				Application.Run(shell);
				return shell.UserWantsToOpenADifferentProject;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool OnOpenProject()
		{
			using (var dlg = new WelcomeDialog())
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					MruProjects.AddNewPath(dlg.ProjectPath);
					MruProjects.Save();
					Settings.Default.Save();
					return RunWindowForProject(dlg.ProjectPath);
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void SetUpErrorHandling()
		{
			Palaso.Reporting.ErrorReport.AddProperty("EmailAddress", "david_olson@sil.org");//hahah
			Palaso.Reporting.ErrorReport.AddStandardProperties();
			Palaso.Reporting.ExceptionHandler.Init();
		}
	}
}
