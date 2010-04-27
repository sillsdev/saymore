using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Palaso.Code;
using SilUtils;
using Sponge2.Properties;

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

		protected ProjectChild(ComponentFile.Factory componentFileFactory)
		{
			_componentFileFactory = componentFileFactory;
		}

		[Obsolete("Don't use this. It is for serialization only")]
		protected ProjectChild()
		{
		}

		protected static ProjectChild InitializeAtLocation(ProjectChild child, string parentDirectoryPath, string id)
		{
			var childDirectory = Path.Combine(parentDirectoryPath, id);
			RequireThat.Directory(parentDirectoryPath).Exists();
			RequireThat.Directory(childDirectory).DoesNotExist();
			Directory.CreateDirectory(childDirectory);
			child.Id = id;
			child.ParentFolderPath = parentDirectoryPath;
			child.Save();
			return child;
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


		[XmlIgnore]
		protected internal string ParentFolderPath { get; set; }

		[XmlIgnore]
		private string FolderPath
		{
			get
			{
				return Path.Combine(ParentFolderPath, Id);
			}
		}

		[XmlIgnore]
		public string SettingsFilePath
		{
			get
			{
				return Path.Combine(FolderPath, Id +"." + ExtensionWithoutPeriod);
			}
		}

		protected abstract string ExtensionWithoutPeriod
		{ get;
		}

		public void Save()
		{
			var x = new XmlSerializerFactory();
			using (var file = File.OpenWrite(SettingsFilePath))
			{
				x.CreateSerializer(typeof (Session)).Serialize(file, this);
			}
			//was swallowing the message that explained why it failed: XmlSerializationHelper.SerializeToFile(SettingsFilePath, this);
		}
	}
}
