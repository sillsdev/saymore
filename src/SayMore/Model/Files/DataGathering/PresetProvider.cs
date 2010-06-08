using System.Collections.Generic;
using System.Linq;

namespace SayMore.Model.Files.DataGathering
{
	/// <summary>
	/// Gets all the metadata settings found in the whole project,
	/// for the purpose of automatically making presets
	/// </summary>
	public class PresetProvider : BackgroundFileProcessor<PresetData>
	{
		public PresetProvider(string rootDirectoryPath, PresetData.Factory factoryMethod)
			:	base(rootDirectoryPath, path=>factoryMethod(path))
		{
		}

		public IEnumerable<KeyValuePair<string, Dictionary<string, string>>> GetSuggestions()
		{
			var suggestor = new UniqueCombinationsFinder(from d in _fileToDataDictionary.Values select d.Dictionary);
			return suggestor.GetSuggestions();
		}
	}

	/// <summary>
	/// The preset which would be derived from this file
	/// </summary>
	public class PresetData
	{
		public delegate PresetData Factory(string path);
		private readonly string _path;
		public Dictionary<string, string> Dictionary { get; private set; }
		public PresetData(string path, IEnumerable<FileType> fileTypes)
		{
			_path = path;
			Dictionary = new Dictionary<string, string>();
		}
	}
}
