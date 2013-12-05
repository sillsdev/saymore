using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Palaso.UI.WindowsForms.ClearShare;
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
	public class AutoCompleteValueGatherer : BackgroundFileProcessor<Dictionary<string, string>> /* a list of the languages mentioned in this file*/, IMultiListDataProvider, IAutoCompleteValueProvider
	{
		protected Dictionary<string, string> _mappingOfFieldsToAutoCompleteKey = new Dictionary<string,string>();
		protected List<string> _multiValueFields = new List<string>();

		public delegate AutoCompleteValueGatherer Factory(string rootDirectoryPath);

		/// ------------------------------------------------------------------------------------
		public AutoCompleteValueGatherer(string rootDirectoryPath, IEnumerable<FileType> allFileTypes,
			Func<ProjectElement, string, ComponentFile> componentFileFactory)
			:	base(rootDirectoryPath, allFileTypes, path => ExtractValues(path, componentFileFactory))
		{
			_mappingOfFieldsToAutoCompleteKey.Add("primaryLanguage", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("fathersLanguage", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("mothersLanguage", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("otherLanguage0", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("otherLanguage1", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("otherLanguage2", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("otherLanguage3", "language");
			_mappingOfFieldsToAutoCompleteKey.Add("fullName", "person");
			_mappingOfFieldsToAutoCompleteKey.Add(SessionFileType.kParticipantsFieldName, "person");
			_mappingOfFieldsToAutoCompleteKey.Add("recordist", "person");
			_mappingOfFieldsToAutoCompleteKey.Add("contributions", "person");
			_mappingOfFieldsToAutoCompleteKey.Add("education", "education");

			_multiValueFields = new List<string>(new[] { SessionFileType.kParticipantsFieldName, "education", "contributions" });
		}

		/// ------------------------------------------------------------------------------------
		protected override bool GetDoIncludeFile(string path)
		{
			if (_typesOfFilesToProcess.Any(t => t.IsMatch(path)) ||
				_typesOfFilesToProcess.Any(t => t.IsMatch(path.Replace(".meta", string.Empty))))
			{
				var p = GetActualPath(path);
				return File.Exists(p);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Subclass can override this to, for example, use the path of a sidecar file
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string GetActualPath(string path)
		{
			return (path.EndsWith(".meta") ? path.Substring(0, path.Length - 5) : path);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Given a data file, make a dictionary of the values
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected static Dictionary<string, string> ExtractValues(string path,
			Func<ProjectElement, string, ComponentFile> componentFileFactory)
		{
			try
			{
				var file = componentFileFactory(null, path);
				var dictionary = new Dictionary<string, string>();

				foreach (var fieldInstance in file.MetaDataFieldValues)
				{
					var value = (fieldInstance.Value is IAutoCompleteValueProvider ?
						((IAutoCompleteValueProviderWeird)fieldInstance.Value).GetValueForKey(fieldInstance.FieldId) :
						fieldInstance.ValueAsString);

					if (!string.IsNullOrEmpty(value))
						dictionary[fieldInstance.FieldId] = value;
				}

				// something of a hack... the name of the file is the only place we currently keep
				// the person's name
				if (file.FileType.GetType() == typeof(PersonFileType))
				{
					dictionary.Add("person", Path.GetFileNameWithoutExtension(path));
				}

				return dictionary;
			}
			catch(Exception err)
			{
#if DEBUG
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(err, "Only seeing because you're in debug mode.");
#endif
				return new Dictionary<string, string>();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gives [key, (list of unique values)]
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual Dictionary<string, IEnumerable<string>> GetValueLists(bool includeUnattestedFactoryChoices)
		{
			var keyToValuesDictionary = new Dictionary<string, IEnumerable<string>>();

			if (includeUnattestedFactoryChoices)
				AddFactoryChoices(keyToValuesDictionary);

			// Go through each file's dictionary of field/value pairs.
			lock (((ICollection)_fileToDataDictionary).SyncRoot)
			{
				foreach (var fieldValuePairs in _fileToDataDictionary.Values)
				{
					// Go through each field/value pair.
					foreach (var field in fieldValuePairs)
					{
						var key = GetKeyFromFieldName(field.Key);

						// If a list for the specified key hasn't already been started,
						// then start a new one for the key.
						if (!keyToValuesDictionary.ContainsKey(key))
							keyToValuesDictionary.Add(key, new List<string>());

						// In most cases, this will just loop once because most fields do not
						// contain multiple values. However, some fields contain multiple
						// values delimited by commas or semicolons. In those cases, this
						// loop will iterate once for each delimited value found.
						foreach (var value in GetIndividualValues(field)
							.Where(value => !keyToValuesDictionary[key].Contains(value)))
						{
							((List<string>)keyToValuesDictionary[key]).Add(value);
						}
					}
				}
			}

			return keyToValuesDictionary;
		}

		/// ------------------------------------------------------------------------------------
		private static void AddFactoryChoices(Dictionary<string, IEnumerable<string>> keyToValuesDictionary)
		{
			keyToValuesDictionary.Add(SessionFileType.kGenreFieldName, GenreDefinition.FactoryGenreDefinitions.Select(d => d.Name).ToList());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Splits a list of values into individual values. Some fields in the UI can contain
		/// multiple items delimited by a comma or semi colon (e.g. participants could contain
		/// several people, "Fred; Barney; Wilma; Betty"). When that is true, then each value
		/// in a multivalue field is a separate value in the auto-complete list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual IEnumerable<string> GetIndividualValues(KeyValuePair<string, string> field)
		{
			if (!_multiValueFields.Contains(field.Key))
				yield return field.Value;
			else
			{
				foreach (var v in from l in field.Value.Split(',', ';')
								  where l.Trim().Length > 0
								  select l.Trim())
				{
					yield return v;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Collapses various field names into a single key. For example, in the program,
		/// people's names can be entered into several fields (e.g. "participants",
		/// "fullname", "recordist", etc.). These would collapse to a single key called
		/// "person" and that key would be used by any name field that wanted to take
		/// advantage of an auto-complete list of names.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string GetKeyFromFieldName(string fieldKey)
		{
			string key;

			return (_mappingOfFieldsToAutoCompleteKey.TryGetValue(fieldKey, out key) ?
				key : fieldKey);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetValuesForKey(string key)
		{
			return GetValuesForKey(key, false);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetValuesForKey(string key, bool includeUnattestedFactoryChoices)
		{
			var list = (from kvp in GetValueLists(includeUnattestedFactoryChoices)
						where kvp.Key == key
						select kvp.Value).FirstOrDefault();

			return (list ?? new List<string>(0));
		}
	}
}
