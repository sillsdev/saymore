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

		string ILanguageDisambiguator.Disambiguate(string language, string personId,
			bool primaryLanguage, bool fathersLanguage, bool mothersLanguage)
		{
			return WritingSystemDlg.LookUpLanguage(_parentForm, language, personId,
				primaryLanguage, fathersLanguage, mothersLanguage)?.ToString();
		}
	}
}
