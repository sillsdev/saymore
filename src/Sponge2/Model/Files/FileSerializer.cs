using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Sponge2.Model.Files
{
	/// <summary>
	/// This class lets us save and load objects which have a set of fields.
	/// </summary>
	public class FileSerializer
	{
		public void Save(IEnumerable<FieldValue> fields, string path, string rootElementName)
		{
			var child = new XElement(rootElementName);//todo could use actual name
			foreach (var v in fields)
			{
				var element = new XElement(v.FieldDefinitionKey, v.Value);
				element.Add(new XAttribute("type", v.Type));
				child.Add(element);
			}
			child.Save(path);
		}

		public void Load(List<FieldValue> fields, string path, string rootElementName)
		{
			fields.Clear();
			var child = XElement.Load(path);
			foreach (var element in child.Descendants())
			{
				var type = element.Attribute("type").Value;

				//without this, \r\n was getting changed to \n (mostly a problem for unit tests)
				string s = element.Value.Replace("\n", Environment.NewLine);
				s = s.Replace("\r\n", Environment.NewLine);

				//Enhance: think about checking with existing field definitions
				//1)we would probably NOT want to lose a value just because it wasn't
				//defined on this computer.
				//2)someday we may want to check the type, too
				//Enhance: someday we may have other types
				fields.Add(new FieldValue(element.Name.LocalName, type, s));
			}
		}
	}
}
