using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SIL.Localization;
using SayMore.Model;
using SayMore.Properties;
using SayMore.UI.ProjectChoosingAndCreating;
using SayMore.UI.ProjectWindow;

namespace SayMore
{
	static class Program
	{
		/// <summary>
		/// We have one project open at a time, and this helps us bootstrap the project and
		/// properly dispose of various things when the project is closed.
		/// </summary>
		private static ProjectContext _projectContext;

		private static ApplicationContainer _applicationContainer;

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

			_applicationContainer = new ApplicationContainer();

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
			Debug.Assert(_projectContext == null);
			_projectContext = _applicationContainer.CreateProjectContext(projectPath);
			_projectContext.ProjectWindow.Closed += OnProjectWindow_Closed;
			_projectContext.ProjectWindow.Show();
		}

		/// ------------------------------------------------------------------------------------
		static void ChooseAnotherProject(object sender, EventArgs e)
		{
			Application.Idle -= ChooseAnotherProject;

			using (var dlg = _applicationContainer.CreateWelcomeDialog())
			{
				if (dlg.ShowDialog() != DialogResult.OK)
				{
					Application.Exit();
					return;
				}

				MruProjects.AddNewPath(dlg.Model.ProjectSettingsFilePath);
				MruProjects.Save();
//				if (!File.Exists(dlg.Model.ProjectSettingsFilePath))
//				{
//					new Project(dlg.Model.ProjectSettingsFilePath);
//				}
				OpenProjectWindow(dlg.Model.ProjectSettingsFilePath);
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
