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
using SIL.Windows.Forms.WritingSystems;
using static System.String;
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
			using (var dlg = new LanguageLookupDialog())
			{
				string caption;
				if (primaryLanguage)
				{
					caption = LocalizationManager.GetString("General.DisambiguatePrimaryLanguage",
						"Identify the primary language for {0}",
						"Param is id (probably name) of person");
				}
				else if (fathersLanguage)
				{
					caption = mothersLanguage ? 
						LocalizationManager.GetString("General.DisambiguateParentsLanguage",
							"Identify the primary language used by the parents of {0}", 
							"Param is id (probably name) of person") :
						LocalizationManager.GetString("General.DisambiguateFathersLanguage",
							"Identify the primary language used by the father of {0}", 
							"Param is id (probably name) of person");
				}
				else if (mothersLanguage)
				{
					caption = LocalizationManager.GetString("General.DisambiguateMothersLanguage",
							"Identify the primary language used by the mother of {0}",
							"Param is id (probably name) of person");
				}
				else
				{
					caption = LocalizationManager.GetString("General.DisambiguateMothersLanguage",
						"Identify the another language used by {0}",
						"Param is id (probably name) of person");
				}

				caption = Format(caption, personId);
				// TODO: uncomment this and remove MessageBox line when we get the updated DLL
				// dlg.Caption = caption;
				MessageBox.Show(_parentForm, caption, "Temporary!");
				dlg.SearchText = language;
				return dlg.ShowDialog(_parentForm) == DialogResult.Cancel ? null :
					$"{dlg.SelectedLanguage.LanguageTag}:{dlg.DesiredLanguageName}";
			}
		}
	}
}
