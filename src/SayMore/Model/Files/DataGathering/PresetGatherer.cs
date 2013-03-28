using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using L10NSharp;

namespace SayMore.Model.Files.DataGathering
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Gets all the metadata settings found in the whole project,
	/// for the purpose of automatically making presets
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PresetGatherer : BackgroundFileProcessor<PresetData>
	{
		public delegate PresetGatherer Factory(string rootDirectoryPath);

		/// ------------------------------------------------------------------------------------
		public PresetGatherer(string rootDirectoryPath, IEnumerable<FileType> allFileTypes,
			PresetData.Factory presetFactory)
			: base(rootDirectoryPath, allFileTypes.Where(t => t.IsAudioOrVideo),
				path => presetFactory(path))
		{
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
		public IEnumerable<KeyValuePair<string, Dictionary<string, string>>> GetPresets()
		{
			KeyValuePair<string, Dictionary<string, string>>[] suggestions;
			lock (((ICollection)_fileToDataDictionary).SyncRoot)
			{
				var suggestor = new UniqueCombinationsFinder(_fileToDataDictionary.Values.Select(d => d.Dictionary));
				suggestions = suggestor.GetSuggestions().ToArray();
			}

			if (suggestions.Length == 0)
			{
				var msg = LocalizationManager.GetString("Miscellaneous.PresetGathererNoPresetsYetMsg", "No presets yet");

				yield return new KeyValuePair<string, Dictionary<string, string>>(
					msg, new Dictionary<string, string>());
			}
			else
			{
				foreach (var keyValuePair in suggestions)
					yield return keyValuePair;
			}
		}

		///// ------------------------------------------------------------------------------------
		//public IEnumerable<Dictionary<string, string>> GetPresets()
		//{
		//    var suggestor = new UniqueCombinationsFinder(_fileToDataDictionary.Values.Select(d => d.Dictionary));
		//    return suggestor.GetSuggestions();
		//}
	}

	#region PresetData class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// The preset which would be derived from this file
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PresetData
	{
		public delegate PresetData FactoryForTest(string path, Func<string, Dictionary<string, string>> pathToDictionaryFunction);
		public delegate PresetData Factory(string path);

		public Dictionary<string, string> Dictionary { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Notice, it's up to the caller to give us files which make sense.
		/// E.g., media files have sidecars with data that makes sense as a presets.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PresetData(string path, Func<ProjectElement, string, ComponentFile> componentFileFactory)
		{
			var file = componentFileFactory(null, path);

			var writableFields = from field in file.MetaDataFieldValues
								 where file.FileType.GetShowInPresetOptions(field.FieldId)
								 select field;

			Dictionary = writableFields.ToDictionary(field => field.FieldId, field => field.ValueAsString);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// for test only... probably was a waste of time
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PresetData(string path, Func<string, Dictionary<string, string>> pathToDictionaryFunction)
		{
			Dictionary = pathToDictionaryFunction(path);
		}
	}

	#endregion
}
