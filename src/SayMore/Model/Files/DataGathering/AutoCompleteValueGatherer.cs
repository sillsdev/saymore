using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
	public class AutoCompleteValueGatherer : BackgroundFileProcessor<Dictionary<string, string>> /* a list of the languages mentioned in this file*/, IMultiListDataProvider
	{
		protected Dictionary<string, string> _mappingOfFieldsToAutoCompleteKey = new Dictionary<string,string>();
		protected List<string> _multiValueFields = new List<string>();

		public delegate AutoCompleteValueGatherer Factory(string rootDirectoryPath);

		/// ------------------------------------------------------------------------------------
		public AutoCompleteValueGatherer(string rootDirectoryPath, IEnumerable<FileType> allFileTypes,
			ComponentFile.Factory componentFileFactory)
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
			_mappingOfFieldsToAutoCompleteKey.Add("participants", "person");
			_mappingOfFieldsToAutoCompleteKey.Add("recordist", "person");
			_mappingOfFieldsToAutoCompleteKey.Add("education", "education");

			_multiValueFields = new List<string>(new[] { "participants", "education" });
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
			ComponentFile.Factory componentFileFactory)
		{
			try
			{
				var file = componentFileFactory(path);


				//TODO there is an occasional problem here we need to track down, which
				//may be a case of the same key appearing twice. For now, added try catch
				//so we don't let that close down the whole gatherer for the life of the app

				var dictionary = file.MetaDataFieldValues.ToDictionary(
					field => field.FieldId, field => field.Value);

				// something of a hack... the name of the file is the only place we currently keep
				// the person's name
				if (file.FileType.GetType() == typeof (PersonFileType))
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
		public virtual Dictionary<string, IEnumerable<string>> GetValueLists()
		{
			var d = new Dictionary<string, IEnumerable<string>>();
			d.Add("eventType", new List<string>(){"aaaaa","bbbbb"});
			foreach (KeyValuePair<string, Dictionary<string, string>> fieldsOfFile in _fileToDataDictionary)
			{
				foreach (var field in fieldsOfFile.Value)
				{
					var key = GetKeyFromFieldName(field.Key);
					if (!d.ContainsKey(key))
					{
						d.Add(key, new List<string>());
					}

					foreach (var value in GetIndividualValues(field))
					{
						if (!d[key].Contains(value))
						{
							((List<string>)d[key]).Add(value);
						}
					}
				}
			}

			return d;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Split list of values into individual components. Some fields in the UI can contain
		/// multiple items separated by a comma or semi colon (e.g. participants could contain
		/// several people, "Fred; Barney; Wilma; Betty"). When that is true, then each value
		/// in a multivalue field is a separate value in the auto-complete list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual IEnumerable<string> GetIndividualValues(KeyValuePair<string, string> field)
		{
			if (_multiValueFields.Contains(field.Key))
			{
				foreach (var v in from l in field.Value.Split(',', ';')
								  where l.Trim().Length > 0
								  select l.Trim())
				{
					yield return v;
				}

			}

			yield return field.Value;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Collapses various field names, e.g. various language fields, into a single key,
		/// e.g. "language"
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string GetKeyFromFieldName(string fieldKey)
		{
			string key;

			return (_mappingOfFieldsToAutoCompleteKey.TryGetValue(fieldKey, out key) ?
				key : fieldKey);
		}
	}
}
