using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SayMore.Model.Files.DataGathering
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Scans a project folder for all sidecar files and collects the fields found therein.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FieldGatherer : BackgroundFileProcessor<FileTypeFields>
	{
		protected Dictionary<Type, IEnumerable<string>> _fieldsByFileType =
			new Dictionary<Type, IEnumerable<string>>();

		/// ------------------------------------------------------------------------------------
		public FieldGatherer(string rootDirectoryPath, IEnumerable<FileType> allFileTypes,
			FileTypeFields.Factory fileTypeFieldsFactory)
			: base(rootDirectoryPath, allFileTypes, path => fileTypeFieldsFactory(path))
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override bool GetDoIncludeFile(string path)
		{
			if (_typesOfFilesToProcess.Any(t => t.IsMatch(path.Replace(".meta", string.Empty))))
			{
				var p = GetActualPath(path);
				return File.Exists(p);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetActualPath(string path)
		{
			return path + (!path.Contains(".meta") ? ".meta" : string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<string> GetFieldsForType(FileType fileType,
			IEnumerable<string> exclude)
		{
			var type = fileType.GetType();

			var fieldsForType = new List<string>();

			// Go through all the lists of fields found for the specified file type
			// and create a single list of unique field names for that file type.
			foreach (var listOfLists in from fileTypeFields in _fileToDataDictionary.Values
										where fileTypeFields.FileType == type
										select fileTypeFields.Fields)
			{
				foreach (var field in listOfLists.Except(exclude))
				{
					if (!fieldsForType.Contains(field))
						fieldsForType.Add(field);
				}
			}

			return fieldsForType;
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class FileTypeFields
	{
		public delegate FileTypeFields Factory(string path);

		public Type FileType { get; private set; }
		public IEnumerable<string> Fields { get; private set; }

		/// ------------------------------------------------------------------------------------
		public FileTypeFields(string path, ComponentFile.Factory componentFileFactory)
		{
			// As JohnH said in PresetData, this is "hacky" and he's right.
			var file = componentFileFactory(path.Replace(".meta", string.Empty));
			FileType = file.FileType.GetType();
			Fields = file.MetaDataFieldValues.Select(field => field.FieldId);
		}
	}
}
