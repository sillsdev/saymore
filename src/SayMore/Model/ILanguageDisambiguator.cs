// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2025, SIL Global. All Rights Reserved.
// <copyright from='2024' to='2025' company='SIL Global'>
//		Copyright (c) 2025, SIL Global. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (https://sil.mit-license.org/)
// </copyright>
#endregion
// --------------------------------------------------------------------------------------------
namespace SayMore.Model
{
	/// <summary>
	/// Interface to allow different implementations of logic to disambiguate a language string
	/// that could be either a "code" (ISO-639-1, ISO-639-2, or BCP-47) or a language name (an
	/// autonym, or the name of the language in English or perhaps some other LWC). This allows
	/// for a non-interactive test implementation but could also be used to implement some kind
	/// of automated disambiguation logic as well.
	/// </summary>
	public interface ILanguageDisambiguator
	{
		/// <summary>
		/// This method will be called one time per "batch" of languages to be disambiguated. In
		/// an interactive implementation, this allows for the situation to be explained to the
		/// user.
		/// </summary>
		/// <returns><c>true</c> if it is okay to proceed with calls to <see cref="Disambiguate"/>;
		/// <c>false</c> otherwise.</returns>
		bool IsOkayToDisambiguate();

		/// <summary>
		/// Attempts to disambiguate a language represented by a user-entered string and return a
		/// string that the system can thereafter unambiguously recognize as a known language.
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
		/// <returns>Result of attempted disambiguation. Null if the operation was cancelled or
		/// otherwise unsuccessful</returns>
		string Disambiguate(string language, string personId, bool primaryLanguage,
			bool fathersLanguage, bool mothersLanguage);
	}
}
