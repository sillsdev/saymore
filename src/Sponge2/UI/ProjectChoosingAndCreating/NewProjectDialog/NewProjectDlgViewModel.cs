// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: NewSessionDlgViewModel.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Windows.Forms;
using SIL.Localization;

namespace Sponge2.UI.ProjectChoosingAndCreating.NewProjectDialog
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

		protected string DefaultProjectsFolder
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Sponge");
			}
		}

		public string ParentFolderPathForNewProject
		{
			get { return DefaultProjectsFolder; }
		}
	}
}