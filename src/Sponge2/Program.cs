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
		/// <summary>
		/// We have one project open at a time, and this helps us bootstrap the project and
		/// properly dispose of various things when the project is closed.
		/// </summary>
		private static ProjectContext _projectContext;

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
			if(_projectContext!=null)
			{
				_projectContext.Dispose();
			}
		}

		/// ------------------------------------------------------------------------------------
		static void StartUpShellBasedOnMostRecentUsedIfPossible()
		{
			if (MruProjects.Latest != null && File.Exists(MruProjects.Latest))
			{
				OpenProjectWindow(MruProjects.Latest);
			}
			else
			{
				//since the message pump hasn't started yet, show the UI for choosing when it is
				Application.Idle += ChooseAnotherProject;
			}
		}

		/// ------------------------------------------------------------------------------------
		private static void OpenProjectWindow(string projectPath)
		{
			_projectContext = new ProjectContext(projectPath);
			ProjectWindow projectWindow = _projectContext.CreateProjectWindow();
			projectWindow.Closed += OnProjectWindow_Closed;
			projectWindow.Show();
		}

		/// ------------------------------------------------------------------------------------
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
				OpenProjectWindow(welcomeModel.ProjectSettingsFilePath);
			}
		}

		/// ------------------------------------------------------------------------------------
		static void OnProjectWindow_Closed(object sender, EventArgs e)
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
		private static void SetUpErrorHandling()
		{
			Palaso.Reporting.ErrorReport.AddProperty("EmailAddress", "david_olson@sil.org");//hahah
			Palaso.Reporting.ErrorReport.AddStandardProperties();
			Palaso.Reporting.ExceptionHandler.Init();
		}
	}
}
