// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2025, SIL Global. All Rights Reserved.
// <copyright from='2024' to='2025' company='SIL Global'>
//		Copyright (c) 2025, SIL Global. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (https://sil.mit-license.org/)
// </copyright>
#endregion
// --------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using SayMore.Model.Files;
using static SayMore.Model.Files.PersonFileType;

namespace SayMore.Model
{
	public class MetadataMigrator
	{
		private readonly Func<ILanguageDisambiguator> _getDisambiguator;
		private readonly bool _isInVietnam;
		private ILanguageDisambiguator _disambiguator;
		private readonly Dictionary<string, string> _cache = new Dictionary<string, string>();

		public enum Result
		{
			Cancelled,
			NotNeeded,
			Migrated,
		}

		public MetadataMigrator(Func<ILanguageDisambiguator> getDisambiguator, bool isInVietnam = false)
		{
			_getDisambiguator = getDisambiguator;
			_isInVietnam = isInVietnam;
		}

		/// <summary>
		/// If for any language, we get a different return value from LanguageList.FindByISO3Code
		/// and LanguageList.FindByEnglishName, have the user disambiguate.
		/// </summary>
		public Result MigrateAmbiguousLanguages(IStringValueCollection metadata)
		{
			var handledMother = false;
			var handledFather = false;
			var result = MigrateAmbiguousLanguage(metadata, kPrimaryLanguage, ref handledMother, ref handledFather);

			// otherLanguage0 - otherLanguage3
			for (var i = 0; i < 4 && result != Result.Cancelled; i++)
			{
				var resultForThisLanguage = MigrateAmbiguousLanguage(metadata, GetOtherLanguageKey(i),
					ref handledMother, ref handledFather);
				if (resultForThisLanguage != Result.NotNeeded)
					result = resultForThisLanguage;
			}

			return result;
		}

		private Result MigrateAmbiguousLanguage(IStringValueCollection metadata, string key, 
			ref bool handledMother, ref bool handledFather)
		{
			var languageStr = metadata.GetStringValue(key, null);
			if (languageStr == null)
				return Result.NotNeeded;


			if (LanguageHelper.IsAmbiguous(languageStr, _isInVietnam))
			{
				if (_disambiguator == null)
				{
					_disambiguator = _getDisambiguator();
					if (!_disambiguator.IsOkayToDisambiguate())
					{
						_disambiguator = null;
						return Result.Cancelled;
					}
				}

				var isLanguageOfMother = !handledMother && metadata.GetStringValue(kMothersLanguage, null) == languageStr;
				var isLanguageOfFather = !handledFather && metadata.GetStringValue(kFathersLanguage, null) == languageStr;

				if (!_cache.TryGetValue(languageStr, out var newValue))
				{
					newValue = _disambiguator.Disambiguate(languageStr, metadata.Id,
						key == kPrimaryLanguage, isLanguageOfFather, isLanguageOfMother);
				}

				if (newValue == null)
					return Result.Cancelled;

				_cache[languageStr] = newValue;
				metadata.SetStringValue(key, newValue);
				if (isLanguageOfMother)
				{
					metadata.SetStringValue(kMothersLanguage, newValue);
					handledMother = true;
				}

				if (isLanguageOfFather)
				{
					metadata.SetStringValue(kFathersLanguage, newValue);
					handledFather = true;
				}

				return Result.Migrated;
			}

			return Result.NotNeeded;
		}
	}
}
