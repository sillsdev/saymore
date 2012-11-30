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
		private string _validPathMessage;

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
			var invalidPathMsg = LocalizationManager.GetString("DialogBoxes.NewProjectDlg.InvalidPathMsg",
				"Unable to create a new project by that name.",
				"This text is displayed under the project name when it is invalid.");

			if (_validPathMessage == null)
				_validPathMessage = newProjectPathLabel.Text;

			if (_pathValidator == null)
				_pathValidator = new PathValidator(newProjectPathLabel, _tooltip) { InvalidMessage = invalidPathMsg };

			NewProjectName = newName;

			return _pathValidator.IsPathValid(DefaultProjectsFolder,
				newName, _validPathMessage, invalidPathMsg);
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