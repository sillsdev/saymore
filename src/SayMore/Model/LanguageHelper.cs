// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2025, SIL Global. All Rights Reserved.
// <copyright from='2024' to='2025' company='SIL Global'>
//		Copyright (c) 2025, SIL Global. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (https://sil.mit-license.org/)
// </copyright>
#endregion
// --------------------------------------------------------------------------------------------
using System.Collections.Generic;
using SIL.Archiving.IMDI.Lists;
using SIL.WritingSystems;
using System.Linq;
using SIL.Extensions;
using static System.Char;
using System;

namespace SayMore.Model
{
	public static class LanguageHelper
	{
		private static LanguageLookup _languageLookup;

		internal static LanguageLookup _LanguageLookup =>
			_languageLookup ?? (_languageLookup = new LanguageLookup());

		/// <summary>
		/// If the given code is a valid 2-letter "ISO 639-1" standard code, returns the
		/// corresponding 3-letter code from the "ISO 639-2" standard. If null, or some other
		/// length code is passed in, the value is simply returned. If a three-letter code is
		/// passed in, it is assumed to be valid and is simply returned without any checking.
		/// </summary>
		/// <param name="languageCode">A 2-letter or 3-letter code.</param>
		/// <returns>Typically, a valid ISO 639-2 3-letter code.</returns>
		public static string GetIso639ThreeCharCode(this string languageCode)
		{
			languageCode = languageCode?.Split('-')[0];
			if (languageCode?.Length == 2)
			{
				var l = _LanguageLookup.GetLanguageFromCode(languageCode);
				if (l != null)
					languageCode = l.ThreeLetterTag;
			}

			return languageCode;
		}

		public static string[] SplitOnColon(this string languageStr)
		{
			return languageStr.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>
		/// Analyzes the given string to determine whether it is ambiguous in specifying a
		/// language. Only strings that are exactly two or three characters in length and are
		/// either all uppercase or all lowercase will be treated as potentially ambiguous. If
		/// such strings are valid ISO-639-1 or ISO-639-2 language codes and also can be
		/// identified as a known language name with a different code, then they are considered
		/// ambiguous.
		/// </summary>
		/// <param name="language">A string representing a language</param>
		/// <param name="isInVietnam">A flag indicating whether the context is known to be in
		/// Vietnam. (There is an endangered language "En" in Vietnam, so "en" should be
		/// considered ambiguous if we know we're dealing with a project or session in Vietnam.)
		/// </param>
		/// <returns>Whether the language string could be interpreted as either a language name
		/// or a code (in which case it might be ambiguous).</returns>
		/// <remarks>Note that this method does NOT attempt to guarantee that a language specifier
		/// is actually valid. It merely tries to see if it could be ambiguous when treated as a
		/// code and a language name, resulting in two different possible languages. If the
		/// language-chooser UI was used to specify the language, it should have the form
		/// "code:name", where the code is a valid BCP-47 code. It is unlikely (though possible)
		/// that a user would specify a language in that form just by typing in a text field.
		/// Therefore, if we encounter that form, we will assume that the portion before the
		/// colon is a code (not a language name) and treat it as unambiguous.
		/// </remarks>
		public static bool IsAmbiguous(string language, bool isInVietnam = false)
		{
			language = language.Split(':')[0];
			switch (language.Length)
			{
				case 2:
					if (language == "en" || language == "EN")
						return isInVietnam;
					goto case 3;
				case 3:
					if (!language.All(IsUpper) && !language.All(IsLower))
						return false; // Camel/Title case - presumably a language name
					var byCode = LanguageList.FindByISO3Code(language);
					if (byCode == null || byCode.Iso3Code == "und")
						return false; // Not a known code; presumably a language name.
					var byName = LanguageList.FindByEnglishName(language);
					if (byName == null || byName.Iso3Code == "und")
						return false; // Also not a known language name, but not "ambiguous"
					return byCode.Iso3Code != byName.Iso3Code;
				default:
					return false;
			}
		}

		public static List<string> GetParts(string languageDesignator) => languageDesignator.SplitTrimmed(':');

		public static TwoPartLanguageSpecifier GetTwoPartLanguageSpecification(string languageDesignator)
		{
			var parts = GetParts(languageDesignator);
			return parts.Count == 2 ? new TwoPartLanguageSpecifier(parts[0], parts[1]) : null;
		}

		public static bool IsWellFormedTwoPartLanguageSpecification(string languageDesignator)
		{
			return GetTwoPartLanguageSpecification(languageDesignator)?.IsValid ?? false;
		}
	}
}
