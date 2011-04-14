using System;
using System.IO;
using System.Windows.Forms;
using Localization;
using SayMore.Properties;

namespace SayMore.UI.ProjectChoosingAndCreating.NewProjectDialog
{
	/// ----------------------------------------------------------------------------------------
	public class NewProjectDlgViewModel
	{
		private readonly ToolTip _tooltip;
		private PathValidator _pathValidator;

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

			if (_pathValidator == null)
				_pathValidator = new PathValidator(newProjectPathLabel, _tooltip) { InvalidMessage = invalidPathMsg };

			var validPathMsg = LocalizationManager.GetString(newProjectPathLabel);

			NewProjectName = newName;

			return _pathValidator.IsPathValid(DefaultProjectsFolder,
				newName, validPathMsg, invalidPathMsg);
		}

		/// ------------------------------------------------------------------------------------
		protected string DefaultProjectsFolder
		{
			get
			{
				if (string.IsNullOrEmpty(Settings.Default.DefaultFolderForNewProjects) ||
					!Directory.Exists(Settings.Default.DefaultFolderForNewProjects))
				{
					return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SayMore");
				}

				return Settings.Default.DefaultFolderForNewProjects;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string ParentFolderPathForNewProject
		{
			get { return DefaultProjectsFolder; }
		}
	}
}