using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Palaso.Code;

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

		/// <summary>
		/// Use this for creating new or existing elements
		/// </summary>
		/// <param name="parentElementFolder">E.g. "c:/MyProject/Sessions"</param>
		/// <param name="id">e.g. "ETR007"</param>
		protected ProjectElement(string parentElementFolder, string id, ComponentFile.Factory componentFileFactory)
		{
			_componentFileFactory = componentFileFactory;
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

/*		/// <summary>
		/// Use this for creating new elements
		/// </summary>
		/// <param name="parentElementFolder">E.g. "c:/MyProject/Sessions"</param>
		/// <param name="id">e.g. "ETR007"</param>
		protected ProjectElement(string parentElementFolder, string id, ComponentFile.Factory componentFileFactory)
		{
			_componentFileFactory = componentFileFactory;
			RequireThat.Directory(parentElementFolder).Exists();

			ParentFolderPath = parentElementFolder;
			Id = id;
			Fields = new List<FieldValue>();
			Directory.CreateDirectory(FolderPath);
			Save();
		}

		/// <summary>
		/// Use this constructor for existing elements which just need to be read off disk
		/// </summary>
		/// <param name="existingElementFolder">E.g. "c:/MyProject/Sessions/ETR007"</param>
		protected ProjectElement(string existingElementFolder, ComponentFile.Factory componentFileFactory)
		{
			_componentFileFactory = componentFileFactory;
			RequireThat.Directory(existingElementFolder).Exists();

			ParentFolderPath = Path.GetDirectoryName(existingElementFolder);
			Id = Path.GetFileName(existingElementFolder);
			Fields = new List<FieldValue>();


			//review: we might be tempted to recover from someone deleting the sponge project file
			//while leaving the other files in there... is that really worth recovering from?
			//maybe it's better to say "whoaaaaa!" else they'll think we lost their data when
			//all the fields are blank
			Load();
		}
		*/


		public string Id { get; /*ideally only the factory and serializer should see this*/ set; }

		public IEnumerable<ComponentFile> GetComponentFiles()
		{
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
				return Path.Combine(FolderPath, Id +"." + ExtensionWithoutPeriod);
			}
		}

		protected abstract string ExtensionWithoutPeriod { get;}

		public List<FieldValue> Fields { get; set; }

		//TODO: consider extracting loading/saving to a class used for that purpose,
		//and inject it.  This would allow
		// 1) reuse for all persisted things: project, session, person, and meta data files
		// 2) disk-less unit testing (by replacing the normal persister with a memory one or a null one)

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			var child = new XElement("ProjectElement");//todo could use actual name
			foreach(var v in Fields)
			{
				var element = new XElement(v.FieldDefinitionKey, v.Value);
				element.Add(new XAttribute("type", v.Type));
				child.Add(element);
			}
			child.Save(SettingsFilePath);
		}
		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			Fields.Clear();
			var child = XElement.Load(SettingsFilePath);
			foreach (var element in child.Descendants())
			{
				var type = element.Attribute("type").Value;

				//Enhance: think about checking with existing field definitions
				//1)we would probably NOT want to lose a value just because it wasn't
				//defined on this computer.
				//2)someday we may want to check the type, too
				//Enhance: someday we may have other types
				Fields.Add(new FieldValue(element.Name.LocalName, type, element.Value));
			}
		}
	}
}
