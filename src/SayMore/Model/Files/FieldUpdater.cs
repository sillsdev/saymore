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
		/// <summary>
		/// Receives a list of key/value pairs where the key represents the name of the field
		/// that needs to change and the value is the new field name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RenameFields(ComponentFile file, IEnumerable<KeyValuePair<string, string>> fromToPairs)
		{
			FindAndUpdateFiles(file, metaDataFields =>
			{
				foreach (var kvp in fromToPairs)
				{
					var field = metaDataFields.Find(x => x.FieldId == kvp.Key);
					if (field != null)
						field.FieldId = kvp.Value;
				}
			});
		}

		/// ------------------------------------------------------------------------------------
		public void DeleteFields(ComponentFile file, IEnumerable<string> fieldsToDelete)
		{
			FindAndUpdateFiles(file, metaDataFields =>
			{
				foreach (var id in fieldsToDelete)
				{
					var field = metaDataFields.Find(x => x.FieldId == id);
					if (field != null)
						metaDataFields.Remove(field);
				}
			});
		}

		/// ------------------------------------------------------------------------------------
		public void AddFields(ComponentFile file, IEnumerable<string> fieldsToAdd)
		{
			FindAndUpdateFiles(file, metaDataFields =>
			{
				foreach (var newId in fieldsToAdd)
				{
					var field = metaDataFields.Find(x => x.FieldId == newId);
					if (field == null)
						metaDataFields.Add(new FieldValue(newId, string.Empty));
				}
			});
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetMatchingFiles(FileType fileType)
		{
			return from path in Directory.GetFiles(_rootProjectFolder, "*.*", SearchOption.AllDirectories)
				   where fileType.IsMatch(path)
				   select path;
		}

		/// ------------------------------------------------------------------------------------
		private void FindAndUpdateFiles(ComponentFile file, Action<List<FieldValue>> updatePredicate)
		{
			var matchingFiles = GetMatchingFiles(file.FileType);

			foreach (var path in matchingFiles)
			{
				if (file.PathToAnnotatedFile == path)
					continue;

				var sidecarFile = file.FileType.GetMetaFilePath(path);
				if (!File.Exists(sidecarFile))
					continue;

				var metaDataFieldValues = new List<FieldValue>();
				_fileSerializer.Load(metaDataFieldValues, sidecarFile, file.RootElementName);
				updatePredicate(metaDataFieldValues);
				_fileSerializer.Save(metaDataFieldValues, sidecarFile, file.RootElementName);
			}
		}
	}
}
