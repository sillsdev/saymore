using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using SIL.Sponge.Model.MetaData;
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
			VerifyListLoaded();
			return s_list.Templates.FirstOrDefault(x => x.Id == tempateId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the tempate for the specified file extension.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SessionFileInfoTemplate GetTemplateByExt(string ext)
		{
			VerifyListLoaded();
			ext = ext.TrimStart('.').ToLower();
			return s_list.Templates.FirstOrDefault(x => x.FileExtensions.Contains(ext));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that the templates list has been loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void VerifyListLoaded()
		{
			if (s_list == null)
			{
				var distFilesDirectory = Path.Combine(Palaso.IO.FileLocator.DirectoryOfApplicationOrSolution, "DistFiles");
				string path = Path.Combine(distFilesDirectory, "SessionFileInfoTemplates.xml");

				// TODO: Do something when there's an exception returned.
				Exception e;
				s_list = XmlSerializationHelper.DeserializeFromFile<SessionFileInfoTemplateList>(path, out e);

				if (e != null)
					throw e;
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
		private string m_fileExtensions;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionFileInfoTemplate"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SessionFileInfoTemplate()
		{
			Fields = new List<FieldDefinition>();
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
		public string FileExtensions
		{
			get { return m_fileExtensions; }
			set { m_fileExtensions = (value == null ? null : value.ToLower()); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the template fields collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("field")]
		public List<FieldDefinition> Fields { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the field definition for the specified field name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FieldDefinition GetFieldDefinition(string fieldName)
		{
			return Fields.FirstOrDefault(x => x.FieldName == fieldName);
		}

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

	//#region SessionFileInfoField class
	///// ----------------------------------------------------------------------------------------
	///// <summary>
	/////
	///// </summary>
	///// ----------------------------------------------------------------------------------------
	//public class SessionFileInfoField : IInfoPanelField
	//{
	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// Gets the field id.
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    [XmlAttribute("id")]
	//    public string Id { get; set; }

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// Gets the field's text.
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    [XmlElement("value")]
	//    public string Value { get; set; }

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// Gets the default (i.e. before localization) field's display text.
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    [XmlElement("defaultDisplayText")]
	//    public string DefaultDisplayText { get; set; }

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// Gets the field's type (e.g. Text, ComboBox, etc.).
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    [XmlElement("type")]
	//    public string FieldType { get; set; }

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// Gets the field's string id (for localization).
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    [XmlElement("textStringId")]
	//    public string TextStringId { get; set; }

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// Returns a <see cref="System.String"/> that represents this instance.
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    public override string ToString()
	//    {
	//        return DefaultDisplayText;
	//    }
	//}

	//#endregion
}
