using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using L10NSharp;
using SIL.Reporting;
using SayMore.Model.Fields;
using SIL.Xml;
using SayMore.Utilities;

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
		public const string kAdditionalFieldIdPrefix = "additional_";
		private const string kAdditionalFieldsElement = "AdditionalFields";
		protected IDictionary<string, IXmlFieldSerializer> _xmlFieldSerializers;

		/// ------------------------------------------------------------------------------------
		public XmlFileSerializer(IDictionary<string, IXmlFieldSerializer> xmlFieldSerializers)
		{
			_xmlFieldSerializers = xmlFieldSerializers;
		}

		/// ------------------------------------------------------------------------------------
		public void Save(IEnumerable<FieldInstance> fields, string path, string rootElementName)
		{
			var giveUpTime = DateTime.Now.AddSeconds(15);

			// SP-872: Access to the path is denied
			if (File.Exists(path))
			{
				var result = FileSystemUtils.WaitForFileRelease(path, true);
				if (result != FileSystemUtils.WaitForReleaseResult.Free)
				{
					if (result == FileSystemUtils.WaitForReleaseResult.TimedOut)
					{
						var msg = LocalizationManager.GetString("CommonToMultipleViews.FileIsReadOnlyOrLocked",
							"SayMore is not able to write to the file \"{0}.\" It is read-only or locked.");
						ErrorReport.ReportNonFatalMessageWithStackTrace(msg, path);
					}
					return;
				}
			}
			else
			{
				// SP-692: Could not find a part of the path ... in mscorlib
				// This can happen if the object was renamed, resulting in the directory being renamed.
				var dir = Path.GetDirectoryName(path);
				if (dir == null) return;
				if (!Directory.Exists(dir))
				{
					// Do not throw an exception if the session or person was deleted, just return now.
					var parentDir = Path.GetDirectoryName(dir);
					if (parentDir == null)
						throw new DirectoryNotFoundException(dir);

					var dirName = new DirectoryInfo(parentDir).Name;
					if (Directory.Exists(parentDir) && 
						(dirName == Session.kFolderName ||
						 dirName == Person.kFolderName))
						return;

					throw new DirectoryNotFoundException(dir);
				}
			}

			var root = new XElement(rootElementName); // TODO: could use actual name

			XElement customFieldsElement = null;
			XElement additionalFieldsElement = null;
			foreach (var fieldInstance in fields)
			{
				bool custom;
				bool additional;
				var element = GetElementFromField(fieldInstance, out custom, out additional);
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
				else if (additional)
				{
					if (additionalFieldsElement == null)
					{
						additionalFieldsElement = new XElement(kAdditionalFieldsElement);
						root.Add(additionalFieldsElement);
					}
					additionalFieldsElement.Add(element);
				}
				else
					root.Add(element);
			}

			bool attemptedRetry = false;

			while (true)
			{
				try
				{
					GC.Collect();
					root.Save(path);
					break;
				}
				catch (IOException e)
				{
					if (e.GetType() != typeof(IOException))
						throw;

					// Guarantee that it retries at least once
					if (attemptedRetry && DateTime.Now >= giveUpTime)
					{
						// SP-896: Apparently the file that was just judged to be writable above has now been locked
						// by some other process and is no longer writable. Pretty annoying when you think about it.
						// I can't reproduce this error, but the only other thing I can think to do is make it non-
						// fatal (as it is above if it was not released within the 10-second wait time) and hope the
						// user can find some way to fix it or will have better luck next time.
						ErrorReport.ReportNonFatalException(e);
						return;
					}

					attemptedRetry = true;
					Thread.Sleep(100);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public XElement GetElementFromField(FieldInstance fld, out bool custom, out bool additional)
		{
			XElement element = null;

			var id = fld.FieldId;
			custom = false;
			additional = false;
			if (id.StartsWith(kCustomFieldIdPrefix))
			{
				id = id.Substring(kCustomFieldIdPrefix.Length);
				custom = true;
			}
			else if (id.StartsWith(kAdditionalFieldIdPrefix))
			{
				id = id.Substring(kAdditionalFieldIdPrefix.Length);
				additional = true;
			}

			IXmlFieldSerializer fldSerializer;

			if (!custom && !additional && _xmlFieldSerializers != null && _xmlFieldSerializers.TryGetValue(id, out fldSerializer))
			{
				element = fldSerializer.Serialize(fld.Value);
			}
			else if (fld.Type == FieldInstance.kStringType)
			{
				var value = (fld.ValueAsString ?? string.Empty);
				if (value.Length > 0)
				{
					// SP-775: SayMore crash when attempting to use a Custom Field name that starts with a non-alpha character
					if ((!Char.IsLetter(id[0])) && (id[0] != '_')) id = "_" + id;

					element = new XElement(id, fld.ValueAsString);
				}
			}

			if (element != null)
				element.Add(new XAttribute("type", fld.Type));

			return element;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create an "empty" (with just a root element) copy of the file if it isn't there.
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
				catch (IOException e)
				{
					if (e.GetType() != typeof(IOException))
						throw;

					if (DateTime.Now >= giveUpTime)
					{
						Logger.WriteEvent("IO Exception path: " + (path ?? "null"));
						throw;
					}

					Thread.Sleep(100);
				}
				catch (XmlException xmlException)
				{
					// By default XmlExceptions (or at least some of them) don't contain the path name.
					throw new XmlException("Failed to load XML file: " + (path ?? "null"), xmlException);
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

			var additionalFieldList = root.SelectSingleNode(kAdditionalFieldsElement);
			if (additionalFieldList != null)
			{
				fields.AddRange(additionalFieldList.ChildNodes.Cast<XmlNode>()
					.Select(node => new FieldInstance(kAdditionalFieldIdPrefix + node.Name, FieldInstance.kStringType,
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
			if (fieldId == kCustomFieldsElement || fieldId == kAdditionalFieldsElement || fieldId == "fullName")
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
			fieldId = fieldId.Replace("eventType", SessionFileType.kGenreFieldName);
			fieldId = fieldId.Replace("Langauge", "Language");
			return fieldId.Replace("learnedLanguageIn", "primaryLanguageLearnedIn");
		}
	}
}
