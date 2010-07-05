using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SayMore.Model.Fields;

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
		public override void Start()
		{
			// This will force the gathering of fields to be done at least once, before
			// the program is fully up and running. That way fields are available to views
			// right away.
			ProcessAllFiles();
			_restartRequested = false;
			base.Start();
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
			if (".session .person".Contains(Path.GetExtension(path)))
				return path;

			return path + (!path.Contains(".meta") ? ".meta" : string.Empty);
		}

		/// ------------------------------------------------------------------------------------
//		public virtual IEnumerable<FieldDefinition> GetCustomFieldsForFileType(FileType fileType)
//		{
//			var type = fileType.GetType();
//
//			var fieldsForType = new List<FieldDefinition>();
//
			// Go through all the lists of fields found for the specified file type
			// and create a single list of unique field names for that file type.
//			foreach (var listofKeys in from fileTypeFields in _fileToDataDictionary.Values
//										where fileTypeFields.FileType == type
//										select fileTypeFields.FieldKeys)
//			{
//				foreach (var key in listofKeys.Where(x=>fileType.GetIsCustomFieldId(x)))
//				{
//					if (!fieldsForType.Any(f=>f.Key==key))
//						fieldsForType.Add(new FieldDefinition(key,"string",new string[]{}){IsCustom =true});
//				}
//			}
//
//			return fieldsForType;
//		}
		public virtual IEnumerable<FieldDefinition> GetAllFieldsForFileType(FileType fileType)
		{
			var type = fileType.GetType();

			var fieldsForType = new List<FieldDefinition>();

			// Go through all the lists of fields found for the specified file type
			// and create a single list of unique field names for that file type.
			foreach (var listofKeys in from fileTypeFields in _fileToDataDictionary.Values
									   where fileTypeFields.FileType == type
									   select fileTypeFields.FieldKeys)
			{
				foreach (var key in listofKeys)
				{
					if (!fieldsForType.Any(f => f.Key == key))
						fieldsForType.Add(new FieldDefinition(key, "string", new string[] { }) { IsCustom = fileType.GetIsCustomFieldId(key) });
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
		public IEnumerable<string> FieldKeys { get; private set; }

		/// ------------------------------------------------------------------------------------
		public FileTypeFields(string path, ComponentFile.Factory componentFileFactory)
		{
			// As JohnH said in PresetData, this is "hacky" and he's right.
			var file = componentFileFactory(path.Replace(".meta", string.Empty));
			FileType = file.FileType.GetType();
			FieldKeys = file.MetaDataFieldValues.Select(field => field.FieldId);
		}
	}
}
