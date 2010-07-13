using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SayMore.Model.Fields;

namespace SayMore.Model.Files
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Scans a project folder for files of a specified type and updates fields in the
	/// sidecar files associated with those files. Updates include adding, renaming, and
	/// removing. When removing, the data for the field is lost.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FieldUpdater
	{
		private readonly string _rootProjectFolder;
		private readonly FileSerializer _fileSerializer;

		/// ------------------------------------------------------------------------------------
		public FieldUpdater(string rootProjectFolder)
		{
			_rootProjectFolder = rootProjectFolder;
			_fileSerializer = new FileSerializer();
		}

		/// ------------------------------------------------------------------------------------
		public void RenameField(ComponentFile file, string oldName, string newName)
		{
			FindAndUpdateFiles(file, metaDataFields =>
			{
				var field = metaDataFields.Find(x => x.FieldId == oldName);
				if (field != null)
					field.FieldId = newName;
			});
		}

		///// ------------------------------------------------------------------------------------
		//public void DeleteFields(ComponentFile file, IEnumerable<string> fieldsToDelete)
		//{
		//    FindAndUpdateFiles(file, metaDataFields =>
		//    {
		//        foreach (var key in fieldsToDelete)
		//        {
		//            var field = metaDataFields.Find(x => x.FieldKey == key);
		//            if (field != null && field.IsCustomField)
		//                metaDataFields.Remove(field);
		//        }
		//    });
		//}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetMatchingFiles(FileType fileType)
		{
			return from path in Directory.GetFiles(_rootProjectFolder, "*.*", SearchOption.AllDirectories)
				   where fileType.IsMatch(path)
				   select path;
		}

		/// ------------------------------------------------------------------------------------
		private void FindAndUpdateFiles(ComponentFile file, Action<List<FieldInstance>> updatePredicate)
		{
			var matchingFiles = GetMatchingFiles(file.FileType);

			foreach (var path in matchingFiles)
			{
				if (file.PathToAnnotatedFile == path)
					continue;

				var sidecarFile = file.FileType.GetMetaFilePath(path);
				if (!File.Exists(sidecarFile))
					continue;

				var metaDataFieldValues = new List<FieldInstance>();
				_fileSerializer.Load(metaDataFieldValues, sidecarFile, file.RootElementName);
				updatePredicate(metaDataFieldValues);
				_fileSerializer.Save(metaDataFieldValues, sidecarFile, file.RootElementName);
			}
		}
	}
}
