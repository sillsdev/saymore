using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SayMore.Model.Fields;

namespace SayMore.Model.Files.DataGathering
{
	/// <summary>
	/// Gets values used in the whole project (e.g. language names),
	/// for the purpose of type-ahead.
	///
	/// The data model while we're gathering is:
	/// {filePath, {fieldKey, (list of values)}}
	/// Then when the data is needed for autocomplete, GetValueLists() distills it down to
	/// {fieldKey, (list of unique values)}
	/// </summary>
	public class AutoCompleteValueGatherer : BackgroundFileProcessor<Dictionary<string,string>> /* a list of the languages mentioned in this file*/, IMultiListDataProvider
	{
		private Dictionary<string, string> _mapping;

		public delegate AutoCompleteValueGatherer Factory(string rootDirectoryPath);

		public AutoCompleteValueGatherer(string rootDirectoryPath, IEnumerable<FileType> allFileTypes,
			ComponentFile.Factory componentFileFactory)
			:	base(rootDirectoryPath,
					allFileTypes.Where(t => t.GetType() == typeof(PersonFileType)),
					path => ExtractValues(path, componentFileFactory))
		{
			_mapping = new Dictionary<string, string>();
			_mapping.Add("primaryLanguage", "language");
			_mapping.Add("fathersLanguage", "language");
			_mapping.Add("mothersLanguage", "language");
			_mapping.Add("otherLanguage0", "language");
			_mapping.Add("otherLanguage1", "language");
			_mapping.Add("otherLanguage2", "language");
			_mapping.Add("otherLanguage3", "language");
			_mapping.Add("fullName", "person");

		}

		/// <summary>
		/// Given a data file, make a dictionary of the values
		/// </summary>
		private static Dictionary<string,string> ExtractValues(string path, ComponentFile.Factory componentFileFactory)
		{
			var f = componentFileFactory(path);

			return f.MetaDataFieldValues.ToDictionary(field => field.FieldKey,
														   field => field.Value);
			//TODO: split on commas and ;

/*			var langs = new List<string>();
			foreach (FieldValue field in f.MetaDataFieldValues)
			{
				if (field.FieldKey.ToLower().Contains("lang")
					&& !field.FieldKey.ToLower().Contains("learned")
					&& !string.IsNullOrEmpty(field.Value))
				{
					var langsInField = from l in field.Value.Split(',', ';')
									 where l.Trim().Length > 0
									 select l.Trim();

					Debug.WriteLine("AutoCompleteValueGatherer: " + field.FieldKey + ": " + langsInField.Aggregate((a, b) => a + ", " + b));
					langs.AddRange(langsInField);
				}
			}

			return langs;
*/		}

		/// <summary>
		/// Gives [key, (list of unique values)]
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, IEnumerable<string>> GetValueLists()
		{
			var d = new Dictionary<string, IEnumerable<string>>();
			foreach (KeyValuePair<string, Dictionary<string, string>> fieldsOfFile in _fileToDataDictionary)
			{
				foreach (var field in fieldsOfFile.Value)
				{
					var key = GetKeyFromFieldName(field.Key);
					if(!d.ContainsKey(key))
					{
						d.Add(key, new List<string>());
					}
					if(!d[key].Contains(field.Value))
					{
						((List<string>)d[key]).Add(field.Value);
					}
				}
			}
			return d;
		}

		/// <summary>
		/// Collapses various field names, e.g. various language fields,
		/// into a single key, e.g. "language"
		/// </summary>
		private string GetKeyFromFieldName(string fieldKey)
		{
			string key;
			if(_mapping.TryGetValue(fieldKey, out key))
			{
				return key;
			}
			return fieldKey;
		}
	}
}
