using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SayMore.Model.Fields;

namespace SayMore.Model.Files.DataGathering
{
	/// <summary>
	/// Gets all the languages mentioned in the whole project,
	/// for the purpose of type-ahead
	/// </summary>
	public class LanguageNameGatherer : BackgroundFileProcessor<List<string> /* a list of the languages mentioned in this file*/>
	{
		public delegate LanguageNameGatherer Factory(string rootDirectoryPath);

		public LanguageNameGatherer(string rootDirectoryPath, IEnumerable<FileType> allFileTypes,ComponentFile.Factory componentFileFactory)
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
				if (field.FieldDefinitionKey.Contains("lang"))
				{
					langs.AddRange(field.Value.Split(new char[] {',', ';'}));
				}
			}
			return langs;
		}


		public  IEnumerable<string> GetLanguages()
		{
			var uniqueOnes=new List<string>();
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
	}

}
