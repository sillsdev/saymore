using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using SilUtils;

namespace SIL.Sponge.Model
{
	#region SessionFileInfoTemplateList class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Contains a collection of SessionFileInfoTemplate objects.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlRoot("sessionFileInfoTemplates")]
	public class SessionFileInfoTemplateList
	{
		private static SessionFileInfoTemplateList s_list;

		[XmlElement("template")]
		public List<SessionFileInfoTemplate> Templates { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionFileInfoTemplate"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SessionFileInfoTemplateList()
		{
			Templates = new List<SessionFileInfoTemplate>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the tempate for the specified id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SessionFileInfoTemplate GetTemplateById(string tempateId)
		{
			LoadList();
			return s_list.Templates.FirstOrDefault(x => x.Id == tempateId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the tempate for the specified file extension.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SessionFileInfoTemplate GetTemplateByExt(string ext)
		{
			LoadList();
			ext = ext.TrimStart('.').ToLower();
			return s_list.Templates.FirstOrDefault(x => x.FileExtensions.Contains(ext));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of templates if it hasn't already been loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void LoadList()
		{
			if (s_list == null)
			{
				string path = Path.GetDirectoryName(Application.ExecutablePath);
				path = Path.Combine(path, "SessionFileInfoTemplates.xml");

				// TODO: Do something when there's an exception returned.
				Exception e;
				s_list = XmlSerializationHelper.DeserializeFromFile<SessionFileInfoTemplateList>(path, out e);
			}
		}
	}

	#endregion

	#region SessionFileInfoTemplate class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class containing information about the fields to display for a single Session file type.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SessionFileInfoTemplate
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionFileInfoTemplate"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SessionFileInfoTemplate()
		{
			Fields = new List<SessionFileInfoField>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the template id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("id")]
		public string Id { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the valid file extensions for the files matching the template. This is a
		/// comma delimited list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("fileExtensions")]
		public string FileExtensions { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the template fields collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("field")]
		public List<SessionFileInfoField> Fields { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Id;
		}
	}

	#endregion

	#region SessionFileInfoField class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SessionFileInfoField
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the field id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("id")]
		public string Id { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the field's string id (for localization).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("textStringId")]
		public string TextStringId { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the field's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("defaultText")]
		public string DefaultText { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the field's type (e.g. Text, ComboBox, etc.).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("type")]
		public string FieldType { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return DefaultText;
		}
	}

	#endregion
}
