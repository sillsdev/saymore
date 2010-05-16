using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Palaso.Reporting;
using SayMore.Model;

namespace SayMore.UI.ProjectChoosingAndCreating
{
	/// <summary>
	/// This contains all easily testable logic/behavior for the WelcomeDialog,
	/// which lets you open projects and create new ones.
	/// </summary>
	public class WelcomeDialogViewModel
	{
		//private readonly Project.Factory _projectFactory;

		public WelcomeDialogViewModel()//Project.Factory projectFactory)
		{
			//_projectFactory = projectFactory;
		}

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
		/// <summary>
		/// Returns a list of recently used projects where each items key is the full path
		/// to the project file and the value is just the name of the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<KeyValuePair<string, string>> RecentlyUsedProjects
		{
			get
			{
				foreach (string prjPath in MruProjects.Paths)
				{
					yield return new KeyValuePair<string, string>(prjPath,
																  Path.GetFileNameWithoutExtension(prjPath));
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public string GetVersionInfo(string fmt)
		{
			Version ver = Assembly.GetExecutingAssembly().GetName().Version;

			// The build number is just the number of days since 01/01/2000
			DateTime bldDate = new DateTime(2000, 1, 1).AddDays(ver.Build);

			return string.Format(fmt, ver.Major, ver.Minor, ver.Revision,
								 bldDate.ToString("dd-MMM-yyyy"));
		}

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