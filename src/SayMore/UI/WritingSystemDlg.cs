// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2025, SIL Global. All Rights Reserved.
// <copyright from='2025' to='2025' company='SIL Global'>
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

namespace SayMore.UI
{
	public static class WritingSystemDlg
	{
		public static TwoPartLanguageSpecifier LookUpLanguage(Form parentForm, string language, string personId,
			bool primaryLanguage, bool fathersLanguage, bool mothersLanguage)
		{
			using (var dlg = new LanguageLookupDialog())
			{
				string captionFmt;
				if (primaryLanguage)
				{
					captionFmt = LocalizationManager.GetString("WritingSystemDlg.PrimaryLanguage",
						"Primary language for {0}",
						"Param is id (probably name) of person");
				}
				else if (fathersLanguage)
				{
					captionFmt = mothersLanguage ? 
						LocalizationManager.GetString("WritingSystemDlg.ParentsLanguage",
							"Primary language used by the parents of {0}", 
							"Param is id (probably name) of person") :
						LocalizationManager.GetString("WritingSystemDlg.FathersLanguage",
							"Primary language used by the father of {0}", 
							"Param is id (probably name) of person");
				}
				else if (mothersLanguage)
				{
					captionFmt = LocalizationManager.GetString("WritingSystemDlg.MothersLanguage",
						"Primary language used by the mother of {0}",
						"Param is id (probably name) of person");
				}
				else
				{
					captionFmt = LocalizationManager.GetString("WritingSystemDlg.AnotherLanguage",
						"Another language used by {0}",
						"Param is id (probably name) of person");
				}

				dlg.Caption = Format(captionFmt, personId);
				var parts = LanguageHelper.GetParts(language);
				if (parts.Count > 1)
				{
					dlg.SearchText = parts[0];
					dlg.SetLanguageAlias(parts[0], parts[1]);
				}
				else
					dlg.SearchText = language;
				return dlg.ShowDialog(parentForm) == DialogResult.Cancel ? null :
					new TwoPartLanguageSpecifier(dlg.SelectedLanguage.LanguageTag, dlg.DesiredLanguageName);
			}
		}
	}
}
