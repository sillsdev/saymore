using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Palaso.IO;
using Palaso.Reporting;
using SilUtils;

namespace Sponge2.Model
{
	#region FieldTemplateList class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Contains a collection of all the FieldTemplate objects for the application.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlRoot("fieldTemplates")]
	public class FieldTemplateList
	{
		private static FieldTemplateList s_list;

		[XmlElement("templates")]
		public List<FieldTemplate> Templates { get; set; }

		/// ------------------------------------------------------------------------------------
		public FieldTemplateList()
		{
			Templates = new List<FieldTemplate>();
		}

		/// ------------------------------------------------------------------------------------
		public static FieldTemplate GetTemplateById(string tempateId)
		{
			VerifyListLoaded();
			return s_list.Templates.FirstOrDefault(x => x.Id == tempateId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the tempate for the specified file extension.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static FieldTemplate GetTemplateByExt(string ext)
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
				var distFilesDirectory = Path.Combine(FileLocator.DirectoryOfApplicationOrSolution, "DistFiles");
				string path = Path.Combine(distFilesDirectory, "FieldTemplatesTemplates.xml");

				// TODO: Do something when there's an exception returned.
				Exception e;
				s_list = XmlSerializationHelper.DeserializeFromFile<FieldTemplateList>(path, out e);

				if (e != null)
					ErrorReport.ReportNonFatalException(e);
			}
		}
	}

	#endregion

	#region FieldTemplate class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a template for fields associated with a single FileType.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FieldTemplate
	{
		private string _fileExtensions;

		/// ------------------------------------------------------------------------------------
		public FieldTemplate()
		{
			FieldDefinitions = new List<FieldDefinition>();
		}

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
			get { return _fileExtensions; }
			set { _fileExtensions = (value == null ? null : value.ToLower()); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlArray("fieldDefinitions"), XmlArrayItem("field")]
		public List<FieldDefinition> FieldDefinitions { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the field definition for the specified field name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FieldDefinition GetFieldDefinition(string fieldName)
		{
			return FieldDefinitions.FirstOrDefault(x => x.FieldName == fieldName);
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
