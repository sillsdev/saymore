using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Localization;
using SilUtils;

namespace SayMore.Model.Fields
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("type")]
	public class GenreDefinition
	{
		private static List<GenreDefinition> s_allTypes;

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

		public static GenreDefinition UnknownType { get; private set; }

		/// ------------------------------------------------------------------------------------
		public static IEnumerable<GenreDefinition> FactoryGenreDefinitions
		{
			get
			{
				if (s_allTypes == null)
				{
					var path = Application.ExecutablePath;
					path = Path.Combine(Path.GetDirectoryName(path), "Genres.xml");
					s_allTypes = Load(path) ?? new List<GenreDefinition>();

					UnknownType = new GenreDefinition();
					UnknownType.Id = "unknown";
					UnknownType.Name = LocalizationManager.LocalizeString(
						"UnknownEventType", "<Unknown>",
						"Unknown genre displayed in the genre drop-down list.",
						"Misc. Strings");

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
		/// <summary>
		/// Gets the tooltip.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Tooltip
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}
	}
}
