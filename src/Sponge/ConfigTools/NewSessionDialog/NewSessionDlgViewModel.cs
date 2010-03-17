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
using System.Collections.Generic;
using System.Linq;
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
	public class NewSessionDlgViewModel
	{
		private readonly SpongeProject _project;
		private readonly HashSet<string> _sessionFiles = new HashSet<string>();
		private readonly ToolTip _tooltip;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionDlgViewModel(SpongeProject project)
		{
			_project = project;
			_tooltip = new ToolTip();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DefaultNewSessionId
		{
			get { return _project.IsoCode; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NewSessionId { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the files added to the session.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] SessionFiles
		{
			get { return _sessionFiles.ToArray(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsNewSessionIdValid(string newId, Label newSessionPathLabel)
		{
			var invalidPathMsg = LocalizationManager.LocalizeString(
				"NewSessionDlg.lblPath.InvalidPathMsg", "Unable to create a new session by that name.",
				"This text is displayed under the session name when it is invalid.", "Dialog Boxes");

			var validPathMsg = LocalizationManager.GetString(newSessionPathLabel);

			NewSessionId = newId;

			return PathValidator.ValidatePathEntry(_project.SessionsFolder,
				newId, newSessionPathLabel, validPathMsg, invalidPathMsg, _tooltip);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LetUserChooseSessionFiles()
		{
			var caption = LocalizationManager.LocalizeString(
				"NewSessionDlg.OpenFileDlgCaption", "Copy Files into Session", "Dialog Boxes");

			AddFilesToSession(Sponge.GetFilesOfAnyType(caption));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddFilesToSession(IEnumerable<string> fileNames)
		{
			if (fileNames != null)
			{
				foreach (string file in fileNames)
					_sessionFiles.Add(file);
			}
		}
	}
}
