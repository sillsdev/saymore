using System.Collections.Generic;
using System.IO;
using System.Linq;
using Palaso.UI.WindowsForms.PortableSettingsProvider;
using SayMore.Model;
using SayMore.Properties;

namespace SayMore.UI.ProjectChoosingAndCreating
{
	/// <summary>
	/// This contains all easily testable logic/behavior for the WelcomeDialog,
	/// which lets you open projects and create new ones.
	/// </summary>
	public class WelcomeDialogViewModel
	{
		//private readonly Project.Factory _projectFactory;

		//public WelcomeDialogViewModel()//Project.Factory projectFactory)
		//{
		//	//_projectFactory = projectFactory;
		//}

		public string ProjectSettingsFilePath { get; set; }

		/// ------------------------------------------------------------------------------------
//		public bool CreateNewProject(string prjFolder)
//		{
//			int isep = prjFolder.LastIndexOf(Path.DirectorySeparatorChar);
//			var prjFile = (isep >= 0 ? prjFolder.Substring(isep + 1) : prjFolder) + ".sprj";
//			prjFile = prjFile.Replace(" ", string.Empty);
//
//			ProjectSettingsFilePath = Path.Combine(prjFolder, prjFile);
//
//			// Review: do we care if their is already an .sprj file
//			// in the folder having a different name?
//
//			if (!File.Exists(ProjectSettingsFilePath))
//				return false; // Project.CreateAtLocation(ProjectSettingsFilePath);
//
//			var msg = LocalizationManager.LocalizeString(
//				"WelcomeDialog.ProjectAlreadyExistsMsg",
//				"A project already exists in the folder '{0}'.", "Dialog Boxes");
//
//			Utils.MsgBox(string.Format(msg, prjFolder), MessageBoxIcon.Exclamation);
//			ProjectSettingsFilePath = null;
//			return false;
//		}

		/// ------------------------------------------------------------------------------------
		public WelcomeDialogViewModel()
		{
			if (!Settings.Default.FirstTimeRun)
				return;

			// If this is the first time the program has been run, then stuff
			// the sample project(s) into the MRU list.
			var path = Path.Combine(Program.AppDataFolder, "Samples");

			if (Directory.Exists(path))
			{
				foreach (var sampleProjectFile in Project.GetAllProjectSettingsFiles(path))
					MruFiles.AddNewPath(sampleProjectFile);

				Settings.Default.FirstTimeRun = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a list of recently used projects where each items key is the full path
		/// to the project file and the value is just the name of the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<KeyValuePair<string, string>> RecentlyUsedProjects
		{
			get
			{
				return MruFiles.Paths.Select(prjPath =>
					new KeyValuePair<string, string>(prjPath, Path.GetFileNameWithoutExtension(prjPath)));
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetRequestedPath(string parentFolderPath, string newProjectName)
		{
//			try
//			{
			//	var project = _projectFactory(parentFolderPath, newProjectName);
				ProjectSettingsFilePath = Project.ComputePathToSettings(parentFolderPath, newProjectName);
//			}
//			catch (Exception error)
//			{
//				ErrorReport.ReportNonFatalException(error);
//				return false;
//			}
//			return true;
		}
	}
}