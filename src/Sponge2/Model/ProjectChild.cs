using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Palaso.Code;

namespace Sponge2.Model
{
	/// <summary>
	/// A project is made of sessions and people, each of which subclass from this simple class.
	///
	/// Better name welcome!
	/// </summary>
	public abstract class ProjectChild
	{
		/// <summary>
		/// This lets us make componentFile instances without knowing all the inputs they need
		/// </summary>
		private ComponentFile.Factory _componentFileFactory;

		protected ProjectChild(string desiredOrExistingFolder, ComponentFile.Factory componentFileFactory)
		{
			_componentFileFactory = componentFileFactory;

			ParentFolderPath = Path.GetDirectoryName(desiredOrExistingFolder);
			Id = Path.GetFileName(desiredOrExistingFolder);
			Fields = new List<FieldValue>();

			if (File.Exists(SettingsFilePath))
			{
				Load();
			}
			else
			{
				RequireThat.Directory(ParentFolderPath).Exists();

				//review: we might be tempted to recover from someone deleting the sponge project file
				//while leaving the other files in there... is that really worth recovering from?
				//maybe it's better to say "whoaaaaa!" else they'll think we lost their data when
				//all the fields are blank

				RequireThat.Directory(ParentFolderPath).DoesNotExist();
				Directory.CreateDirectory(desiredOrExistingFolder);

				Save();
			}
		}




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

		private string FolderPath
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
			XElement child = new XElement("ProjectChild");//todo could use actual name
			foreach(FieldValue v in Fields)
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
			XElement child = XElement.Load(SettingsFilePath);
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
