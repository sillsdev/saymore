using System;
using System.IO;
using System.Windows.Forms;
using Localization;
using SayMore.Properties;

namespace SayMore.UI.ProjectChoosingAndCreating.NewProjectDialog
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class NewProjectDlgViewModel
	{
		private readonly ToolTip _tooltip;

		/// ------------------------------------------------------------------------------------
		public NewProjectDlgViewModel()
		{
			_tooltip = new ToolTip();
		}

		/// ------------------------------------------------------------------------------------
		public string NewProjectName { get; private set; }

		/// ------------------------------------------------------------------------------------
		public bool IsNewProjectNameValid(string newName, Label newProjectPathLabel)
		{
			var invalidPathMsg = LocalizationManager.LocalizeString(
				"NewProjectDlg.newProjectPathLabel.InvalidPathMsg", "Unable to create a new project by that name.",
				"This text is displayed under the project name when it is invalid.", "Dialog Boxes");

			var validPathMsg = LocalizationManager.GetString(newProjectPathLabel);

			NewProjectName = newName;

			return PathValidator.ValidatePathEntry(DefaultProjectsFolder,
				newName, newProjectPathLabel, validPathMsg, invalidPathMsg, _tooltip);
		}

		/// ------------------------------------------------------------------------------------
		protected string DefaultProjectsFolder
		{
			get
			{
				if(string.IsNullOrEmpty(Settings.Default.DefaultFolderForNewProjects)
					|| !Directory.Exists(Settings.Default.DefaultFolderForNewProjects))
				{
					return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SayMore");
				}
				else
				{
					return Settings.Default.DefaultFolderForNewProjects;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public string ParentFolderPathForNewProject
		{
			get { return DefaultProjectsFolder; }
		}
	}
}