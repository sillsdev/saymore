using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using L10NSharp;
using Palaso.IO;
using Palaso.Xml;

namespace SayMore.Model.Fields
{
	/// ----------------------------------------------------------------------------------------
	[XmlType("type")]
	public class GenreDefinition
	{
		private static List<GenreDefinition> s_allTypes;
		private static GenreDefinition s_unknownType;
		private readonly HashSet<string> _namesInDisplayedUiLanguages = new HashSet<string>();

		[XmlAttribute("id")]
		public string Id { get; set; }

		private string _name;

		[XmlElement("name")]
		public string Name
		{
			get
			{
				if (Id == "unknown")
					return _name;
				string name;
				try
				{
					name = GetLocalizedName();
				}
				catch (ArgumentException)
				{
					// This can happen when running unit tests
					name = _name;
				}
				catch (InvalidOperationException)
				{
					// SP-865: This can happen if the the list is still loading, "Collection was modified; enumeration operation may not execute."
					// Let the other thread continue and try again.
					Application.DoEvents();
					Thread.Sleep(0);

					name = GetLocalizedName();
				}
				return name;
			}
			set { _name = value; }
		}

		private string GetLocalizedName()
		{
			var name = LocalizationManager.GetDynamicString("SayMore", "SessionsView.MetadataEditor.Genre." + Id, _name,
						Definition);
			_namesInDisplayedUiLanguages.Add(name);

			return name;
		}

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
					s_unknownType.Name = LocalizationManager.GetString("SessionsView.MetadataEditor.UnknownGenre", "<Unknown>",
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

		/// ------------------------------------------------------------------------------------
		public static string TranslateIdToName(string id)
		{
			// Previous versions made it possible for localized versions of these UI strings to
			// get erroneously persisted in the meta data
			if (id == "<Inconnu>" || id == "<Desconocido>" | id == "<Неизвестный>")
				return UnknownType.Name;
			var genreDefinition = FactoryGenreDefinitions.FirstOrDefault(d => d.Id == id);
			return genreDefinition != null ? genreDefinition.Name : id;
		}

		/// ------------------------------------------------------------------------------------
		public static string TranslateNameToId(string name)
		{
			var genreDefinition = FactoryGenreDefinitions.FirstOrDefault(d => d._namesInDisplayedUiLanguages.Contains(name));
			return genreDefinition != null ? genreDefinition.Id : name;
		}

		/// ------------------------------------------------------------------------------------
		/// Incoming list could be a mix of UI names and ids. This method makes sure all are
		/// actual IDs (though if they are user-defined, there is no distinction) and that there
		/// are no duplicates.
		/// ------------------------------------------------------------------------------------
		public static HashSet<string> GetGenreNameList(IEnumerable<string> list)
		{
			var nameList = new HashSet<string>();
			// Need to do a double-translation to ensure string is displayed in curreent UI language.
			foreach (var nameOrId in list)
				nameList.Add(TranslateIdToName(TranslateNameToId(nameOrId)));
			return nameList;
		}
	}
}
