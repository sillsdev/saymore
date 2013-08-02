using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Palaso.Reporting;
using SayMore.Model.Fields;
using Palaso.Xml;

namespace SayMore.Model.Files
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class lets us save and load objects which have a set of fields to/from simple XML files.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class XmlFileSerializer
	{
		public const string kCustomFieldIdPrefix = "custom_";
		private const string kCustomFieldsElement = "CustomFields";
		protected IDictionary<string, IXmlFieldSerializer> _xmlFieldSerializers;

		/// ------------------------------------------------------------------------------------
		public XmlFileSerializer(IDictionary<string, IXmlFieldSerializer> xmlFieldSerializers)
		{
			_xmlFieldSerializers = xmlFieldSerializers;
		}

		/// ------------------------------------------------------------------------------------
		public void Save(IEnumerable<FieldInstance> fields, string path, string rootElementName)
		{
			var root = new XElement(rootElementName); // TODO: could use actual name

			XElement customFieldsElement = null;
			foreach (var fieldInstance in fields)
			{
				bool custom;
				var element = GetElementFromField(fieldInstance, out custom);
				if (element == null)
					continue;
				if (custom)
				{
					if (customFieldsElement == null)
					{
						customFieldsElement = new XElement(kCustomFieldsElement);
						root.Add(customFieldsElement);
					}
					customFieldsElement.Add(element);
				}
				else
					root.Add(element);
			}

			var giveUpTime = DateTime.Now.AddSeconds(5);
			bool attemptedRetry = false;

			while (true)
			{
				try
				{
					root.Save(path);
					break;
				}
				catch (IOException)
				{
					// Guarantee that it retries at least once
					if (attemptedRetry && DateTime.Now >= giveUpTime)
						throw;

					attemptedRetry = true;
					Thread.Sleep(100);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public XElement GetElementFromField(FieldInstance fld, out bool custom)
		{
			XElement element = null;

			var id = fld.FieldId;
			custom = false;
			if (id.StartsWith(kCustomFieldIdPrefix))
			{
				id = id.Substring(kCustomFieldIdPrefix.Length);
				custom = true;
			}

			IXmlFieldSerializer fldSerializer;

			if (!custom && _xmlFieldSerializers != null && _xmlFieldSerializers.TryGetValue(id, out fldSerializer))
			{
				element = fldSerializer.Serialize(fld.Value);
			}
			else if (fld.Type == FieldInstance.kStringType)
			{
				var value = (fld.ValueAsString ?? string.Empty);
				if (value.Length > 0)
					element = new XElement(id, fld.ValueAsString);
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
		public void Load(/*TODO: ClearShare.Work work,*/ List<FieldInstance> fields, string path,
			string rootElementName, FileType fileType)
		{
			fields.Clear();

			var doc = new XmlDocument();

			var giveUpTime = DateTime.Now.AddSeconds(4);

			while (true)
			{
				try
				{
					doc.Load(path);
					break;
				}
				catch (IOException)
				{
					if (DateTime.Now >= giveUpTime)
					{
						Logger.WriteEvent("IO Exception path: " + path ?? "null");
						throw;
					}

					Thread.Sleep(100);
				}
			}

			var root = doc.ChildNodes[1];
			fields.AddRange(root.ChildNodes.Cast<XmlNode>()
				.Select(node => GetFieldFromNode(node, fileType.GetIsCustomFieldId))
				.Where(fieldInstance => fieldInstance != null));

			var customFieldList = root.SelectSingleNode(kCustomFieldsElement);
			if (customFieldList != null)
			{
				fields.AddRange(customFieldList.ChildNodes.Cast<XmlNode>()
					.Select(node => new FieldInstance(kCustomFieldIdPrefix + node.Name, FieldInstance.kStringType,
					CleanupLineBreaks(node.InnerText))));
			}
		}

		/// ------------------------------------------------------------------------------------
		public FieldInstance GetFieldFromNode(XmlNode node, Func<string, bool> isCustomField)
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

			// We don't store fullName separately from the file name, anymore
			if (fieldId == kCustomFieldsElement || fieldId == "fullName")
				return null;

			// In SayMore, the type attribute is not optional, but it was in Sponge.
			var type = node.GetOptionalStringAttribute("type", FieldInstance.kStringType);

			IXmlFieldSerializer fldSerializer;
			if (_xmlFieldSerializers != null && _xmlFieldSerializers.TryGetValue(fieldId, out fldSerializer))
			{
				var obj = fldSerializer.Deserialize(node.OuterXml);
				return obj == null ? null : new FieldInstance(fieldId, type, obj);
			}

			if (type == FieldInstance.kStringType)
			{
				if (isCustomField != null && isCustomField(fieldId))
					fieldId = kCustomFieldIdPrefix + fieldId;
				return new FieldInstance(fieldId, type, CleanupLineBreaks(node.InnerText));
			}

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
