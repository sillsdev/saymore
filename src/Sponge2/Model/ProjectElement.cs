using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Palaso.Code;
using Sponge2.Persistence;

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

		/// <summary>
		/// Use this for creating new or existing elements
		/// </summary>
		/// <param name="parentElementFolder">E.g. "c:/MyProject/Sessions"</param>
		/// <param name="id">e.g. "ETR007"</param>
		/// <param name="fileSerializer">used to load/save</param>
		protected ProjectElement(string parentElementFolder, string id,
			ComponentFile.Factory componentFileFactory, FileSerializer fileSerializer)
		{
			_componentFileFactory = componentFileFactory;
			_fileSerializer = fileSerializer;
			RequireThat.Directory(parentElementFolder).Exists();

			ParentFolderPath = parentElementFolder;
			Id = id;
			Fields = new List<FieldValue>();

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


		//REVIEW David (JH):  Can we move this back to an IEnumerable? IEnumerables are perferred for their better encapsulation.
		// if you want on the other end  (the caller) to work with an array, you can always convert it.
		// As an array, the caller might think that the can make changes to it, but they can't really.
		public ComponentFile[] GetComponentFiles()
		{
			// John: Should we cache this?
			// Ansr: if it proves slow, but then we have to complicate things to keep it up to date.
			return (from x in Directory.GetFiles(FolderPath, "*.*")
					where (
						//!x.EndsWith("." + Sponge.SessionMetaDataFileExtension) &&
						//!x.EndsWith("." + Sponge.SessionFileExtension) &&
						!x.ToLower().EndsWith("thumbs.db"))
					orderby x
					select _componentFileFactory(x)).ToArray();
		}


		protected internal string ParentFolderPath { get; set; }

		public string FolderPath
		{
			get
			{
				return Path.Combine(ParentFolderPath, Id);
			}
		}

		public string SettingsFilePath
		{
			get
			{
				return Path.Combine(FolderPath, Id + "." + ExtensionWithoutPeriod);
			}
		}

		protected abstract string ExtensionWithoutPeriod { get;}

		public List<FieldValue> Fields { get; set; }
		public abstract string RootElementName { get; }


		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			_fileSerializer.Save(Fields, SettingsFilePath, RootElementName);
		}

		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			_fileSerializer.Load(Fields, SettingsFilePath, RootElementName);
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Id;
		}
	}

}
