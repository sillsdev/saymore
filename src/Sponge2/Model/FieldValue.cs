using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sponge2.Model
{
	public class FieldValue
	{
		public FieldValue(string id, string type, string value)
		{
			FieldDefinitionKey = id;
			Type = type;
			Value = value;
		}

		/// ------------------------------------------------------------------------------------
		//review David (jh): how about "key" or "id" or something short like that?
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
