using System.Collections.Generic;
using System.Linq;

namespace SayMore.Model.Files.DataGathering
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SessionPersonAutoCompleteValueGatherer : AutoCompleteValueGatherer
	{
		public SessionPersonAutoCompleteValueGatherer(string rootDirectoryPath, IEnumerable<FileType> allFileTypes,
			ComponentFile.Factory componentFileFactory)
			: base(rootDirectoryPath,
					allFileTypes.Where(t => t.GetType() == typeof(PersonFileType) || t.GetType() == typeof(SessionFileType)),
					componentFileFactory)
		{
			//NB: this stuff would move to field definitions, if/when we implement them

			_mappingOfFieldsToAutoCompleteKey.Add("primaryLanguage", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("fathersLanguage", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("mothersLanguage", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("otherLanguage0", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("otherLanguage1", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("otherLanguage2", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("otherLanguage3", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("fullName", "person");
			_mappingOfFieldsToAutoCompleteKey.Add("participants", "person");
			_mappingOfFieldsToAutoCompleteKey.Add("recordist", "person");
			_mappingOfFieldsToAutoCompleteKey.Add("education", "education");

			_multiValueFields = new List<string>(new[] { "participants", "education" });
		}
	}
}
