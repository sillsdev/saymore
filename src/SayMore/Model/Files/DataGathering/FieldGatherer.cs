using System;
using System.Collections.Generic;
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
			ComponentFile.Factory componentFileFactory)
			: base(rootDirectoryPath, allFileTypes, path => ExtractFields(path, componentFileFactory))
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns an object containing the FileType of the specified file and a
		/// list of all the fields contained in the file's associated sidecar file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected static FileTypeFields ExtractFields(string path,
			ComponentFile.Factory componentFileFactory)
		{
			return new FileTypeFields(componentFileFactory(path));
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnNewDataAvailable(FileTypeFields fileTypeFields)
		{
			// Remove the list of fields already cached for the specified file type.
			// This will force the list to be rebuilt the next time it's requested.
			if (_fieldsByFileType.ContainsKey(fileTypeFields.FileType))
				_fieldsByFileType.Remove(fileTypeFields.FileType);

			base.OnNewDataAvailable(fileTypeFields);
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<string> GetFieldsForType(FileType fileType,
			IEnumerable<string> exclude)
		{
			var type = fileType.GetType();

			IEnumerable<string> fields;
			if (_fieldsByFileType.TryGetValue(type, out fields))
				return fields;

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

			_fieldsByFileType[type] = fieldsForType;
			return fieldsForType;
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class FileTypeFields
	{
		public Type FileType { get; private set; }
		public IEnumerable<string> Fields { get; private set; }

		public FileTypeFields(ComponentFile file)
		{
			FileType = file.FileType.GetType();
			Fields = file.MetaDataFieldValues.Select(field => field.FieldId);
		}
	}
}
