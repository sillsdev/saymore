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
		private readonly XmlFileSerializer _xmlFileSerializer;

		/// ------------------------------------------------------------------------------------
		public FieldUpdater(FieldGatherer fieldGatherer,
			IDictionary<string, IXmlFieldSerializer> fieldSerializers)
		{
			_xmlFileSerializer = new XmlFileSerializer(fieldSerializers);
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
		/// <summary>
		/// This method is used to update all files of the given type when a filed is being
		/// renamed or deleted. The ComponentFile supplied should be the one that was "active"
		/// when the delete or rename occurred (it will be skipped since it is assumed that the
		/// change was already done there).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FindAndUpdateFiles(ComponentFile file, string idOfFieldToFind,
			Action<List<FieldInstance>, FieldInstance> updateAction)
		{
			// Since the field being removed or renamed is *no longer* a factory field, it
			// will be loaded as a custom field (having prefix "custom_", so if the caller
			// passes the field name without that prefix, we need to add it.
			if (!idOfFieldToFind.StartsWith(XmlFileSerializer.kCustomFieldIdPrefix))
				idOfFieldToFind = XmlFileSerializer.kCustomFieldIdPrefix + idOfFieldToFind;

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
				_xmlFileSerializer.Load(metaDataFields, sidecarFilePath, file.RootElementName, file.FileType);

				var field = metaDataFields.Find(x => x.FieldId == idOfFieldToFind);
				if (field != null)
				{
					updateAction(metaDataFields, field);
					_xmlFileSerializer.Save(metaDataFields, sidecarFilePath, file.RootElementName);
					if (_fieldGatherer != null)
						_fieldGatherer.GatherFieldsForFileNow(sidecarFilePath);
				}
			}

			if (_fieldGatherer != null)
				_fieldGatherer.ResumeProcessing(false);
		}
	}
}
