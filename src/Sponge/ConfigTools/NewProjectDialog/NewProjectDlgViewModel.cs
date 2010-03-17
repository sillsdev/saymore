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
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
using SIL.Sponge.Model;

namespace SIL.Sponge.ConfigTools
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
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewProjectDlgViewModel()
		{
			_tooltip = new ToolTip();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NewProjectName { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsNewProjectNameValid(string newName, Label newProjectPathLabel)
		{
			var invalidPathMsg = LocalizationManager.LocalizeString(
				"NewProjectDlg.newProjectPathLabel.InvalidPathMsg", "Unable to create a new project by that name.",
				"This text is displayed under the project name when it is invalid.", "Dialog Boxes");

			var validPathMsg = LocalizationManager.GetString(newProjectPathLabel);

			NewProjectName = newName;

			return PathValidator.ValidatePathEntry(SpongeProject.ProjectsFolder,
				newName, newProjectPathLabel, validPathMsg, invalidPathMsg, _tooltip);
		}
	}
}
