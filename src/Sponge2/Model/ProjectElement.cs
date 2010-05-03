using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Palaso.Code;
using Sponge2.Model.Files;

namespace Sponge2.Model
{
	/// <summary>
	/// A project is made of sessions and people, each of which subclass from this simple class.
	/// Here, we call those things "ProjectElemements"
	/// </summary>
	public abstract class ProjectElement
	{
		/// <summary>
		/// This lets us make componentFile instances without knowing all the inputs they need
		/// </summary>
		private ComponentFile.Factory _componentFileFactory;
		private FileSerializer _fileSerializer;
		public ComponentFile MetaDataFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Use this for creating new or existing elements
		/// </summary>
		/// <param name="parentElementFolder">E.g. "c:/MyProject/Sessions"</param>
		/// <param name="id">e.g. "ETR007"</param>
		/// <param name="fileSerializer">used to load/save</param>
		/// <param name="fileType"></param>
		/// ------------------------------------------------------------------------------------
		protected ProjectElement(string parentElementFolder, string id,
			ComponentFile.Factory componentFileFactory, FileSerializer fileSerializer, FileType fileType)
		{
			_componentFileFactory = componentFileFactory;
			_fileSerializer = fileSerializer;
			RequireThat.Directory(parentElementFolder).Exists();

			ParentFolderPath = parentElementFolder;
			Id = id;
			//Fields = new List<FieldValue>();
			MetaDataFile = new ComponentFile(SettingsFilePath, fileType, _fileSerializer, RootElementName);

			if (File.Exists(SettingsFilePath))
			{
				Load();
			}
			else
			{
				Directory.CreateDirectory(FolderPath);
				Save();
			}
		}

		public string Id { get; /*ideally only the factory and serializer should see this*/ set; }
		protected internal string ParentFolderPath { get; set; }
		protected abstract string ExtensionWithoutPeriod { get; }
		public abstract string RootElementName { get; }

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ComponentFile> GetComponentFiles()
		{
			// John: Should we cache this?
			// Ansr: if it proves slow, but then we have to complicate things to keep it up to date.

			//this is the actual person or session data
			yield return MetaDataFile;

			//these are the other files we find in the folder
			var otherFiles = from x in Directory.GetFiles(FolderPath, "*.*")
							 where (
								 !x.EndsWith("." + ExtensionWithoutPeriod) &&
								 //!x.EndsWith("." + Sponge.SessionFileExtension) &&
								 !x.ToLower().EndsWith("thumbs.db"))
							 orderby x
							 select _componentFileFactory(x);

			foreach (var file in otherFiles)
			{
				yield return file;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string FolderPath
		{
			get
			{
				return Path.Combine(ParentFolderPath, Id);
			}
		}

		/// ------------------------------------------------------------------------------------
		public string SettingsFilePath
		{
			get
			{
				return Path.Combine(FolderPath, Id + "." + ExtensionWithoutPeriod);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			MetaDataFile.Save();
		}

		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			MetaDataFile.Load();
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Id;
		}
	}
}
