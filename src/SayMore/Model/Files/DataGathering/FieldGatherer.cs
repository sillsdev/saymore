using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
		protected override ThreadPriority ThreadPriority
		{
			get { return ThreadPriority.AboveNormal; }
		}

		/// ------------------------------------------------------------------------------------
		public void GatherFieldsForFileNow(string path)
		{
			// REVIEW: I'm not sure this lock is necessary, but just to make sure...
			lock (new object())
			{
				CollectDataForFile(path);
			}
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

		/// ----------------------------------------------------------------------------------------
		public virtual IEnumerable<FieldDefinition> GetAllFieldsForFileType(FileType fileType)
		{
			var type = fileType.GetType();

			var fieldsForType = new List<FieldDefinition>();

			lock (((ICollection)_fileToDataDictionary).SyncRoot)
			{
				// Go through all the lists of fields found for the specified file type
				// and create a single list of unique field names for that file type.
				foreach (var listofKeys in from fileTypeFields in _fileToDataDictionary.Values
										   where fileTypeFields.FileType == type
										   select fileTypeFields.FieldKeys)
				{
					foreach (var key in listofKeys)
					{
						if (fieldsForType.All(f => f.Key != key))
							fieldsForType.Add(new FieldDefinition(key) { IsCustom = fileType.GetIsCustomFieldId(key) });
					}
				}
			}

			return fieldsForType;
		}
	}

	#region FileTypeFields class
	/// ----------------------------------------------------------------------------------------
	public class FileTypeFields
	{
		public delegate FileTypeFields Factory(string path);

		public Type FileType { get; private set; }
		public IEnumerable<string> FieldKeys { get; private set; }

		/// ------------------------------------------------------------------------------------
		public FileTypeFields(string path, Func<ProjectElement, string, ComponentFile> componentFileFactory)
		{
			var file = componentFileFactory(null, path);
			FileType = file.FileType.GetType();
			FieldKeys = file.AllFields.Select(field => field.FieldId);
		}
	}

	#endregion
}
