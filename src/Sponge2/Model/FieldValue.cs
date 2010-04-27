using System.Xml.Serialization;

namespace Sponge2.Model
{
	public class FieldValue
	{
		/// ------------------------------------------------------------------------------------
		[XmlElement("fieldDefinitionKey")]
		public string FieldDefinitionKey { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("Type")]
		public string Type { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("value")]
		public string Value { get; set; }
	}
}
