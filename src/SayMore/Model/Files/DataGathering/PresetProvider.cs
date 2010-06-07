using System;
using System.Collections.Generic;
using System.IO;

namespace SayMore.Model.Files.DataGathering
{
	//Gets all the metadata settings found in the whole project, for the purpose of automatically making presets
	public class PresetProvider : BackgroundFileProcessor<PresetData>
	{
		private readonly List<Dictionary<string, string>> _instances;
		private UniqueCombinationsFinder _suggestor;

		public PresetProvider(string rootDirectoryPath)
			:	base(rootDirectoryPath, path=>new PresetData(path))

		{

		}
//		private PresetProvider(List<Dictionary<string, string>> instances)
//		{
//			_instances = instances;
//		}

//		public static PresetProvider CreateFromDirectory(string directory)
//		{
//			var allMetaDataSets = new List<Dictionary<string, string>>();
//			foreach (var sessionDirectoryPath in Directory.GetDirectories(directory))
//			{
//				foreach (var path in Directory.GetFiles(sessionDirectoryPath))
//				{
//					var file = SessionFile.Create(path);
//					Dictionary<string, string> dictionary = file.GetMetaDataDictionary();
//					if(dictionary.Count>0)
//						allMetaDataSets.Add(dictionary);
//				}
//			}
//			return new PresetProvider(allMetaDataSets);
//		}
//
//		public static PresetProvider CreateFromTestArray(string[] set)
//		{
//			var instances = new List<Dictionary<string, string>>();
//			foreach (var v in set)
//			{
//				if (v.Trim() == "")
//					continue;
//				var fields = new Dictionary<string, string>();
//				foreach (string field in v.Split(new char[] { ',' }))
//				{
//					if (field.Trim() == "")
//						continue;
//					var x = field.Split(new char[] { '=' });
//					fields.Add(x[0], x[1]);
//				}
//				instances.Add(fields);
//			}
//			return new PresetProvider(instances);
//		}
//
		public IEnumerable<KeyValuePair<string, Dictionary<string, string>>> GetSuggestions()
		{
			if (_suggestor == null)//this could take some time, so don't make it until we have to
				_suggestor = new UniqueCombinationsFinder(_instances);
			return _suggestor.GetSuggestions();
		}
	}

	/// <summary>
	/// The preset which would be derived from this file
	/// </summary>
	public class PresetData
	{
		private readonly string _path;

		public PresetData(string path)
		{
			_path = path;
		}
	}
}
