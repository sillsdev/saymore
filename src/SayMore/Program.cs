using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using SIL.Localization;
using SayMore.Properties;
using SayMore.UI.ProjectWindow;
using SilUtils;

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

			Settings.Default.MRUList = MruFiles.Initialize(Settings.Default.MRUList, 4);
			_applicationContainer = new ApplicationContainer();

			LocalizationManager.Enabled = true;
			LocalizationManager.Initialize();

			SetUpErrorHandling();

			var args = Environment.GetCommandLineArgs();
			var firstTimeArg = args.FirstOrDefault(x => x.ToLower().StartsWith("-i"));
			if (firstTimeArg != null)
			{
				using (var dlg = new FirstTimeRunDialog("put filename here"))
					dlg.ShowDialog();
			}

			StartUpShellBasedOnMostRecentUsedIfPossible();

			Application.Run();
			Settings.Default.Save();

			if (_projectContext != null)
				_projectContext.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		private static void StartUpShellBasedOnMostRecentUsedIfPossible()
		{
			if (MruFiles.Latest == null || !File.Exists(MruFiles.Latest) ||
				!OpenProjectWindow(MruFiles.Latest))
			{
				//since the message pump hasn't started yet, show the UI for choosing when it is
				Application.Idle += ChooseAnotherProject;
			}
		}

		/// ------------------------------------------------------------------------------------
		private static bool OpenProjectWindow(string projectPath)
		{
			Debug.Assert(_projectContext == null);

			try
			{
				_projectContext = _applicationContainer.CreateProjectContext(projectPath);
				_projectContext.ProjectWindow.Closed += HandleProjectWindowClosed;
				_projectContext.ProjectWindow.Show();
				return true;
			}
			catch (Exception e)
			{
				HandleErrorOpeningProjectWindow(e, projectPath);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		private static void HandleErrorOpeningProjectWindow(Exception error, string projectPath)
		{
			if (_projectContext != null)
			{
				if (_projectContext.ProjectWindow != null)
				{
					_projectContext.ProjectWindow.Closed -= HandleProjectWindowClosed;
					_projectContext.ProjectWindow.Close();
				}

				_projectContext.Dispose();
				_projectContext = null;
			}

			Palaso.Reporting.ErrorReport.NotifyUserOfProblem(
				new Palaso.Reporting.ShowAlwaysPolicy(), error,
				"{0} had a problem loading the {1} project. Please report this problem to the developers by clicking 'Details' below.",
				Application.ProductName, Path.GetFileNameWithoutExtension(projectPath));

			Settings.Default.MRUList.Remove(projectPath);
			MruFiles.Initialize(Settings.Default.MRUList);
		}

		/// ------------------------------------------------------------------------------------
		static void ChooseAnotherProject(object sender, EventArgs e)
		{
			Application.Idle -= ChooseAnotherProject;

			while (true)
			{
				using (var dlg = _applicationContainer.CreateWelcomeDialog())
				{
					if (dlg.ShowDialog() != DialogResult.OK)
					{
						Application.Exit();
						return;
					}

					if (OpenProjectWindow(dlg.Model.ProjectSettingsFilePath))
					{
						MruFiles.AddNewPath(dlg.Model.ProjectSettingsFilePath);
						return;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		static void HandleProjectWindowClosed(object sender, EventArgs e)
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
			Palaso.Reporting.ErrorReport.AddProperty("EmailAddress", "issues@saymore.palaso.org");
			Palaso.Reporting.ErrorReport.AddStandardProperties();
			Palaso.Reporting.ExceptionHandler.Init();
		}
	}
}
