using System.Collections.Generic;
using System.Xml.Serialization;
using Palaso.IO;
using SilTools;

namespace SayMore.Model.Fields
{
	/// ----------------------------------------------------------------------------------------
	[XmlType("type")]
	public class GenreDefinition
	{
		private static List<GenreDefinition> s_allTypes;
		private static GenreDefinition s_unknownType;

		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("comments")]
		public string Comments { get; set; }

		[XmlElement("definition")]
		public string Definition { get; set; }

		[XmlArray("examples"), XmlArrayItem("example")]
		public List<string> Examples { get; set; }

		/// ------------------------------------------------------------------------------------
		public static GenreDefinition UnknownType
		{
			get
			{
				if (s_unknownType == null)
				{
					s_unknownType = new GenreDefinition();
					s_unknownType.Id = "unknown";
					s_unknownType.Name = Program.GetString("EventsView.MetadataEditor.UnknownGenre", "<Unknown>",
						"Unknown genre displayed in the genre drop-down list.");
				}

				return s_unknownType;
			}
		}

		/// ------------------------------------------------------------------------------------
		public static IEnumerable<GenreDefinition> FactoryGenreDefinitions
		{
			get
			{
				if (s_allTypes == null)
				{
					var path = FileLocator.GetFileDistributedWithApplication("Genres.xml");
					s_allTypes = Load(path) ?? new List<GenreDefinition>();
					s_allTypes.Add(UnknownType);
				}

				return s_allTypes;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the application's genre definitions from the file specified by path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<GenreDefinition> Load(string path)
		{
			return XmlSerializationHelper.DeserializeFromFile<List<GenreDefinition>>(path, "genres", true);
		}

		/// ------------------------------------------------------------------------------------
		public string Tooltip
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}
	}
}
