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
		public FieldUpdater(FieldGatherer fieldGatherer,
			IDictionary<string, IXmlFieldSerializer> fieldSerializers)
		{
			_fileSerializer = new FileSerializer(fieldSerializers);
			_fieldGatherer = fieldGatherer;

			if (_fieldGatherer != null)
				_rootProjectFolder = _fieldGatherer.RootDirectoryPath;
		}

		/// ------------------------------------------------------------------------------------
		public static FieldUpdater CreateMinimalFieldUpdaterForTests(string rootProjectFolder)
		{
			return new FieldUpdater(null, null) { _rootProjectFolder = rootProjectFolder };
		}

		/// ------------------------------------------------------------------------------------
		public void RenameField(ComponentFile file, string oldId, string newId)
		{
			FindAndUpdateFiles(file, oldId, (metaDataFields, field) => field.FieldId = newId);
		}

		/// ------------------------------------------------------------------------------------
		public void DeleteField(ComponentFile file, string idOfFieldToDelete)
		{
			FindAndUpdateFiles(file, idOfFieldToDelete,
				(metaDataFields, field) => metaDataFields.Remove(field));
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetMatchingFiles(FileType fileType)
		{
			return from path in Directory.GetFiles(_rootProjectFolder, "*.*", SearchOption.AllDirectories)
				   where fileType.IsMatch(path)
				   select path;
		}

		/// ------------------------------------------------------------------------------------
		private void FindAndUpdateFiles(ComponentFile file, string idOfFieldToFind,
			Action<List<FieldInstance>, FieldInstance> updateAction)
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

				var metaDataFields = new List<FieldInstance>();
				_fileSerializer.Load(metaDataFields, sidecarFilePath, file.RootElementName);

				var field = metaDataFields.Find(x => x.FieldId == idOfFieldToFind);
				if (field != null)
				{
					updateAction(metaDataFields, field);
					_fileSerializer.Save(metaDataFields, sidecarFilePath, file.RootElementName);
					if (_fieldGatherer != null)
						_fieldGatherer.GatherFieldsForFileNow(sidecarFilePath);
				}
			}

			if (_fieldGatherer != null)
				_fieldGatherer.ResumeProcessing(false);
		}
	}
}
