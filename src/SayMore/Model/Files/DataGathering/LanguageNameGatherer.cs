using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SayMore.Model.Fields;

namespace SayMore.Model.Files.DataGathering
{
	/// <summary>
	/// Gets all the languages mentioned in the whole project,
	/// for the purpose of type-ahead
	/// </summary>
	public class LanguageNameGatherer : BackgroundFileProcessor<List<string> /* a list of the languages mentioned in this file*/>, IMultiListDataProvider
	{
		public delegate LanguageNameGatherer Factory(string rootDirectoryPath);

		public LanguageNameGatherer(string rootDirectoryPath, IEnumerable<FileType> allFileTypes,
			ComponentFile.Factory componentFileFactory)
			:	base(rootDirectoryPath,
					allFileTypes.Where(t => t.GetType() == typeof(PersonFileType)),
					path => ExtractLanguages(path, componentFileFactory))
		{
		}

		private static List<string> ExtractLanguages(string path, ComponentFile.Factory componentFileFactory)
		{
			var f = componentFileFactory(path);
			var langs = new List<string>();
			foreach (FieldValue field in f.MetaDataFieldValues)
			{
				if (field.FieldKey.ToLower().Contains("lang")
					&& !field.FieldKey.ToLower().Contains("learned")
					&& !string.IsNullOrEmpty(field.Value))
				{
					var langsInField = from l in field.Value.Split(',', ';')
									 where l.Trim().Length > 0
									 select l.Trim();

					Debug.WriteLine("LanguageNameGather: " + field.FieldKey + ": " + langsInField.Aggregate((a, b) => a + ", " + b));
					langs.AddRange(langsInField);
				}
			}

			return langs;
		}

		public IEnumerable<string> GetValues()
		{
			var uniqueOnes = new List<string>();

			foreach (List<string> languages in _fileToDataDictionary.Values)
			{
				foreach (string language in languages)
				{
					if (!uniqueOnes.Contains(language))
					{
						uniqueOnes.Add(language);
					}
				}
			}

			return uniqueOnes;
		}

		public Dictionary<string, IEnumerable<string>> GetValueLists()
		{
			var d = new Dictionary<string, IEnumerable<string>>();
			d.Add("language", GetValues());
			return d;
		}
	}
}
