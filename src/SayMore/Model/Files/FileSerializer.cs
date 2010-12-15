using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using SayMore.Model.Fields;
using Palaso.Xml;

namespace SayMore.Model.Files
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class lets us save and load objects which have a set of fields.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FileSerializer
	{
		protected IDictionary<string, IXmlFieldSerializer> _xmlFieldSerializers;

		/// ------------------------------------------------------------------------------------
		public FileSerializer(IDictionary<string, IXmlFieldSerializer> xmlFieldSerializers)
		{
			_xmlFieldSerializers = xmlFieldSerializers;
		}

		/// ------------------------------------------------------------------------------------
		public void Save(IEnumerable<FieldInstance> fields, string path, string rootElementName)
		{
			var child = new XElement(rootElementName);//todo could use actual name

			foreach (var element in fields.Select(f => GetElementFromField(f)).Where(e => e != null))
				child.Add(element);

			child.Save(path);
		}

		/// ------------------------------------------------------------------------------------
		public XElement GetElementFromField(FieldInstance fld)
		{
			XElement element = null;

			IXmlFieldSerializer fldSerializer;

			if (_xmlFieldSerializers != null && _xmlFieldSerializers.TryGetValue(fld.FieldId, out fldSerializer))
			{
				element = fldSerializer.Serialize(fld.Value);
			}
			else if (fld.Type == "string")
			{
				var value = (fld.ValueAsString ?? string.Empty);
				if (value.Length > 0)
					element = new XElement(fld.FieldId, fld.ValueAsString);
			}

			if (element != null)
				element.Add(new XAttribute("type", fld.Type));

			return element;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create an empty copy of the file if it isn't there.
		/// </summary>
		/// <returns>true if the file had to be created</returns>
		/// ------------------------------------------------------------------------------------
		public bool CreateIfMissing(string path, string rootElementName)
		{
			if (File.Exists(path))
				return false;

			Save(new FieldInstance[]{}, path, rootElementName);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public void Load(/*TODO: ClearShare.Work work,*/List<FieldInstance> fields, string path, string rootElementName)
		{
			fields.Clear();
			var doc = new XmlDocument();
			doc.Load(path);

			foreach (var fld in doc.ChildNodes[1].ChildNodes.Cast<XmlNode>()
				.Select(n => GetFieldFromNode(n)).Where(f => f != null))
			{
				fields.Add(fld);
			}
		}

		/// ------------------------------------------------------------------------------------
		public FieldInstance GetFieldFromNode(XmlNode node)
		{
			if (node.Name == "olac")//might not be quite right, it will be in a namespace
			{
				//TODO: load the clearshare work
				// var olac = new OlacSystem();
				// olac.LoadWorkFromXml(work, node.OuterXml);
				return null;
			}

			var fieldId = node.Name;
			fieldId = UpdateOldSpongeIds(fieldId);

			if (fieldId == "fullName")
				return null; //we don't store that separately from the file name, anymore

			// In SayMore, the type attribute is not optional, but it was in Sponge.
			var type = node.GetOptionalStringAttribute("type", "string");

			IXmlFieldSerializer fldSerializer;
			if (_xmlFieldSerializers != null && _xmlFieldSerializers.TryGetValue(fieldId, out fldSerializer))
				return new FieldInstance(fieldId, type, fldSerializer.Deserialize(node.OuterXml));

			if (type == "string")
				return new FieldInstance(fieldId, type, CleanupLineBreaks(node.InnerText));

			return null;
		}

		/// ------------------------------------------------------------------------------------
		private static string CleanupLineBreaks(string value)
		{
			value = value.Replace("\r\n", ">~<");
			value = value.Replace("\n", Environment.NewLine);
			return value.Replace(">~<", Environment.NewLine);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts some Sponge-era field ids to SayMore equivalents.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string UpdateOldSpongeIds(string fieldId)
		{
			fieldId = fieldId.Replace("eventType", "genre");
			fieldId = fieldId.Replace("Langauge", "Language");
			return fieldId.Replace("learnedLanguageIn", "primaryLanguageLearnedIn");
		}
	}
}
