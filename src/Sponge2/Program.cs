using System;
using System.IO;
using System.Windows.Forms;
using SIL.Localization;
using SIL.Sponge;
using Sponge2.ProjectChoosingAndCreating;
using Sponge2.Properties;

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
			StartUpShellBasedOnMostRecentUsedIfPossible();
			Application.Run();
			Settings.Default.Save();
		}

		static void StartUpShellBasedOnMostRecentUsedIfPossible()
		{
			if (MruProjects.Latest != null && File.Exists(MruProjects.Latest))
			{
				//enhance: we could eventually have a single bootstrapper, if making it takes a long time
				//but for now, this is clean.
				using (var bootStrapper = new BootStrapper(MruProjects.Latest))
				{
					Shell shell = bootStrapper.CreateShell();
					shell.Closed += OnShell_Closed;
					shell.Show();
				}
			}
			else
			{
				//since the message pump hasn't started yet, show the UI for choosing when it is
				Application.Idle += ChooseAnotherProject;
			}
		}

		static void ChooseAnotherProject(object sender, EventArgs e)
		{
			Application.Idle -= ChooseAnotherProject;

			var welcomeModel = new WelcomeDialogViewModel();

			using (var dlg = new WelcomeDialog(welcomeModel))
			{
				if (dlg.ShowDialog() != DialogResult.OK)
				{
					Application.Exit();
					return;
				}

				MruProjects.AddNewPath(welcomeModel.ProjectSettingsFilePath);
				MruProjects.Save();
				using (var bootStrapper = new BootStrapper(welcomeModel.ProjectSettingsFilePath))
				{
					Shell shell = bootStrapper.CreateShell();
					shell.Closed += OnShell_Closed;
					shell.Show();
				}
			}
		}

		static void OnShell_Closed(object sender, EventArgs e)
		{
			if(((Shell) sender).UserWantsToOpenADifferentProject)
			{
				Application.Idle += ChooseAnotherProject;
			}
			else
			{
				Application.Exit();
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
