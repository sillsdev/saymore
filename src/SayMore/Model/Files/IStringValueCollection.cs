// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2025, SIL Global. All Rights Reserved.
// <copyright from='2024' to='2025' company='SIL Global'>
//		Copyright (c) 2025, SIL Global. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (https://sil.mit-license.org/)
// </copyright>
#endregion
// --------------------------------------------------------------------------------------------
namespace SayMore.Model.Files
{
	public interface IStringValueCollection
	{
		string Id { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>Sets the given string value or the specified default if not found</summary>
		/// ------------------------------------------------------------------------------------
		string GetStringValue(string key, string defaultValue);

		/// ------------------------------------------------------------------------------------
		/// <summary>Sets the given string value and returns whether it was set exactly as
		/// requested.</summary>
		/// <returns><c>false</c> if the value is changed or the set operation fails</returns>
		/// ------------------------------------------------------------------------------------
		bool TrySetStringValue(string key, string newValue);

		/// ------------------------------------------------------------------------------------
		/// <summary>Sets the given string value and returns the value as set (which might be
		/// different from the value passed in)</summary>
		/// ------------------------------------------------------------------------------------
		string SetStringValue(string key, string newValue);

	}
}