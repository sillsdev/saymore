using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SayMore.Model.Fields;
using SayMore.Model.Files.DataGathering;

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
		private string _rootProjectFolder;
		private readonly FieldGatherer _fieldGatherer;
		private readonly FileSerializer _fileSerializer;

		/// ------------------------------------------------------------------------------------
		public FieldUpdater(FieldGatherer fieldGatherer)
		{
			_fileSerializer = new FileSerializer();
			_fieldGatherer = fieldGatherer;

			if (_fieldGatherer != null)
				_rootProjectFolder = _fieldGatherer.RootDirectoryPath;
		}

		/// ------------------------------------------------------------------------------------
		public static FieldUpdater CreateMinimalFieldUpdaterForTests(string rootProjectFolder)
		{
			return new FieldUpdater(null) { _rootProjectFolder = rootProjectFolder };
		}

		/// ------------------------------------------------------------------------------------
		public void RenameField(ComponentFile file, string oldName, string newName)
		{
			FindAndUpdateFiles(file, metaDataFields =>
			{
				var field = metaDataFields.Find(x => x.FieldId == oldName);
				if (field != null)
				{
					field.FieldId = newName;
					return true;
				}

				return false;
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
		private void FindAndUpdateFiles(ComponentFile file, Func<List<FieldInstance>, bool> updatePredicate)
		{
			var matchingFiles = GetMatchingFiles(file.FileType);

			if (_fieldGatherer != null)
				_fieldGatherer.SuspendProcessing();

			foreach (var path in matchingFiles)
			{
				if (file.PathToAnnotatedFile == path)
					continue;

				var sidecarFilePath = file.FileType.GetMetaFilePath(path);
				if (!File.Exists(sidecarFilePath))
					continue;

				var metaDataFieldValues = new List<FieldInstance>();
				_fileSerializer.Load(metaDataFieldValues, sidecarFilePath, file.RootElementName);

				if (updatePredicate(metaDataFieldValues))
				{
					_fileSerializer.Save(metaDataFieldValues, sidecarFilePath, file.RootElementName);
					if (_fieldGatherer != null)
						_fieldGatherer.GatherFieldsForFile(sidecarFilePath);
				}
			}

			if (_fieldGatherer != null)
				_fieldGatherer.ResumeProcessing(false);
		}
	}
}
