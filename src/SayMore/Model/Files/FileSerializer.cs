using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using SayMore.Model.Fields;

namespace SayMore.Model.Files
{
	/// <summary>
	/// This class lets us save and load objects which have a set of fields.
	/// </summary>
	public class FileSerializer
	{
		public void Save(IEnumerable<FieldInstance> fields, string path, string rootElementName)
		{
			var child = new XElement(rootElementName);//todo could use actual name
			foreach (var v in fields.Where(f => f.Value.Trim() != string.Empty))
			{
				var element = new XElement(v.FieldId, v.Value);
				element.Add(new XAttribute("type", v.Type));
				child.Add(element);
			}

			child.Save(path);
		}

		/// <summary>
		/// Create an empty copy of the file if it isn't there.
		/// </summary>
		/// <returns>true if the file had to be created</returns>
		public bool CreateIfMissing(string path, string rootElementName)
		{
			if(File.Exists(path))
				return false;
			Save(new FieldInstance[]{}, path, rootElementName);
			return true;
		}

		public void Load(List<FieldInstance> fields, string path, string rootElementName)
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
				var localName = element.Name.LocalName;
				if(localName=="eventType")
				{
					localName = "genre";
				}
				fields.Add(new FieldInstance(localName, type, s));
			}
		}
	}
}
