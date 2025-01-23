// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2025, SIL Global. All Rights Reserved.
// <copyright from='2024' to='2025' company='SIL Global'>
//		Copyright (c) 2025, SIL Global. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (https://sil.mit-license.org/)
// </copyright>
#endregion
// --------------------------------------------------------------------------------------------
using System.Windows.Forms;
using L10NSharp;
using SayMore.Model;
using static SayMore.Program;

namespace SayMore.UI
{
	/// <summary>
	/// A user-interactive implementation that presents massage boxes/dialog boxes to assist the
	/// user in disambiguating ambiguous language strings (which were previously easy to enter).
	/// </summary>
	internal class InteractiveLanguageDisambiguator : ILanguageDisambiguator
	{
		private readonly Form _parentForm;

		public InteractiveLanguageDisambiguator(Form parentForm)
		{
			_parentForm = parentForm;
		}

		public bool IsOkayToDisambiguate()
		{
			return MessageBox.Show(_parentForm, LocalizationManager.GetString("General.NeedToDisambiguateLanguages",
				"One or more of the languages spoken by included participants are specified in a " +
				"way that could be ambiguous. For each one, you will be asked to identify the " +
				"intended language."),
				ProductName, MessageBoxButtons.OKCancel) == DialogResult.OK;
		}

		/// <summary>
		/// Presents the user with a dialog box to help them disambiguate a language represented
		/// by a user-entered string. If the user does not cancel the dialog, this will return a
		/// string that the system can thereafter unambiguously recognize as a known language (or
		/// "qaa:..." to represent an unknown language).
		/// </summary>
		/// <param name="language">String representing a possible language that cannot be
		/// unambiguously interpreted</param>
		/// <param name="personId">ID of the person who uses this language</param>
		/// <param name="primaryLanguage">Flag indicating whether this is the person's primary
		/// language</param>
		/// <param name="fathersLanguage">Flag indicating whether this is the person's father's
		/// language</param>
		/// <param name="mothersLanguage">Flag indicating whether this is the person's mother's
		/// language</param>
		/// <returns>Result of attempted disambiguation. This will be a string with a
		/// known BCP-47 language code, followed by a colon and a human-readable language name.
		/// If the user cancels out of the dialog, this will return <c>null</c>.</returns>
		string ILanguageDisambiguator.Disambiguate(string language, string personId,
			bool primaryLanguage, bool fathersLanguage, bool mothersLanguage)
		{
			return WritingSystemDlg.LookUpLanguage(_parentForm, language, personId,
				primaryLanguage, fathersLanguage, mothersLanguage)?.ToString();
		}
	}
}
