using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using SayMore.Model.Fields;
using Palaso.Xml;

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
			var doc = new XmlDocument();
			doc.Load(path);

			foreach (XmlNode node in doc.ChildNodes[1].ChildNodes)
			{
				var type = node.GetOptionalStringAttribute("type", "string");//sponge-era files didn't have this

				//without this, \r\n was getting changed to \n (mostly a problem for unit tests)
				string s = node.InnerText.Replace("\n", Environment.NewLine);
				s = s.Replace("\r\n", Environment.NewLine);

				//Enhance: think about checking with existing field definitions
				//1)we would probably NOT want to lose a value just because it wasn't
				//defined on this computer.
				//2)someday we may want to check the type, too
				//Enhance: someday we may have other types


				var localName = node.Name;
				localName = localName.Replace("eventType", "genre");//Sponge
				localName = localName.Replace("Langauge", "Language");//Sponge
				localName = localName.Replace("learnedLanguageIn", "primaryLanguageLearnedIn");//Sponge

				if (localName == "fullName")
					continue; //we don't store that separately from the file name, anymore

				fields.Add(new FieldInstance(localName, type, s));
			}
		}
	}
}
