using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Palaso.TestUtilities;
using System.Linq;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using Moq;

namespace SayMoreTests.Model.Files
{
	[TestFixture]
	public class FileSerializerTests
	{
		#region TestFieldSerializer
		internal class TestFieldSerializer : FieldSerializer
		{
			internal TestFieldSerializer() : base("dates")
			{
			}

			public override object Deserialize(string xmlBlob)
			{
				var element = GetElementFromXml(xmlBlob).Element(SessionFileType.kDateFieldName);
				return new DateTime(
					int.Parse(element.Element("year").Value),
					int.Parse(element.Element("month").Value),
					int.Parse(element.Element("day").Value));
			}

			public override XElement Serialize(object obj)
			{
				return InternalSerialize(obj, typeof(DateTime), element =>
				{
					var e = new XElement(SessionFileType.kDateFieldName);
					e.Add(new XElement("year", ((DateTime)obj).Year));
					e.Add(new XElement("month", ((DateTime)obj).Month));
					e.Add(new XElement("day", ((DateTime)obj).Day));
					element.Add(e);
					return element;
				});
			}
		}

		#endregion

		private TemporaryFolder _parentFolder;
		private List<FieldInstance> _fields;
		private XmlFileSerializer _serializer;
		private TestFieldSerializer _testFieldSerializer;
		private Mock<FileType> _fileType;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_testFieldSerializer = new TestFieldSerializer();
			_fileType = new Mock<FileType>("Mocked", null);
			_fileType.Setup(f => f.GetIsCustomFieldId(It.Is<string>(id => id != "a" && id != "dates" && id != "b"))).Returns(true);

			var fieldSerializers = new Dictionary<string, IXmlFieldSerializer>();
			fieldSerializers[_testFieldSerializer.ElementName] = _testFieldSerializer;

			_serializer = new XmlFileSerializer(fieldSerializers);
			_parentFolder = new TemporaryFolder("fileTypeTest");
			_fields = new List<FieldInstance>();
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetElementFromField_InvalidFieldType_ReturnsNull()
		{
			var fld = new FieldInstance("a", "invalid", "blah");
			bool custom;
			bool additional;
			Assert.IsNull(_serializer.GetElementFromField(fld, out custom, out additional));
			Assert.IsFalse(custom);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetElementFromField_FieldValueIsString_ReturnsCorrectElement()
		{
			var fld = new FieldInstance("a", FieldInstance.kStringType, "blah");
			bool custom;
			bool additional;
			var e = _serializer.GetElementFromField(fld, out custom, out additional);

			Assert.IsNotNull("a", e.Name.ToString());
			Assert.IsFalse(custom);
			Assert.AreEqual(FieldInstance.kStringType, e.Attribute("type").Value);
			Assert.AreEqual("blah", e.Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetElementFromField_FieldValueIsDatetime_ReturnsXmlDateElements()
		{
			var elementName = _testFieldSerializer.ElementName;

			var fld = new FieldInstance(elementName, new DateTime(1963, 4, 19));
			bool custom;
			bool additional;
			var e = _serializer.GetElementFromField(fld, out custom, out additional);

			Assert.IsNotNull(elementName, e.Name.ToString());
			Assert.IsFalse(custom);
			Assert.AreEqual("xml", e.Attribute("type").Value);
			var dateElement = e.Element(SessionFileType.kDateFieldName);

			Assert.AreEqual(3, dateElement.Elements().Count());
			Assert.AreEqual("1963", dateElement.Element("year").Value);
			Assert.AreEqual("4", dateElement.Element("month").Value);
			Assert.AreEqual("19", dateElement.Element("day").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFieldFromNode_ExplicitStringType_ReturnsField()
		{
			var doc = new XmlDocument();
			var node = doc.CreateElement("a");
			node.SetAttribute("type", FieldInstance.kStringType);
			node.InnerText = "element stuff";

			var fld = _serializer.GetFieldFromNode(node, _fileType.Object.GetIsCustomFieldId);

			Assert.AreEqual("a", fld.FieldId);
			Assert.AreEqual(FieldInstance.kStringType, fld.Type);
			Assert.AreEqual("element stuff", fld.Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFieldFromNode_ImplicitSpongeStringType_ReturnsField()
		{
			var doc = new XmlDocument();
			var node = doc.CreateElement("a");
			node.InnerText = "element stuff";

			var fld = _serializer.GetFieldFromNode(node, _fileType.Object.GetIsCustomFieldId);

			Assert.AreEqual("a", fld.FieldId);
			Assert.AreEqual(FieldInstance.kStringType, fld.Type);
			Assert.AreEqual("element stuff", fld.Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFieldFromNode_NodeContainsXmlDate_ReturnsField()
		{
			var doc = new XmlDocument();
			var node = doc.CreateElement("dates");
			node.SetAttribute("type", "xml");
			node.InnerXml = "<date><year>1963</year><month>4</month><day>19</day></date>";

			var fld = _serializer.GetFieldFromNode(node, _fileType.Object.GetIsCustomFieldId);

			Assert.AreEqual("dates", fld.FieldId);
			Assert.AreEqual("xml", fld.Type);
			Assert.AreEqual(new DateTime(1963, 4, 19), fld.Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFieldFromNode_fullName_ReturnsNull()
		{
			var doc = new XmlDocument();
			var node = doc.CreateElement("fullName");
			node.SetAttribute("type", FieldInstance.kStringType);
			node.InnerXml = "John Peter Bogle Jr The Third";

			Assert.IsNull(_serializer.GetFieldFromNode(node, _fileType.Object.GetIsCustomFieldId));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFieldFromNode_UnknownType_ReturnsNull()
		{
			var doc = new XmlDocument();
			var node = doc.CreateElement("a");
			node.SetAttribute("type", "flubber");
			node.InnerText = "element stuff";

			Assert.IsNull(_serializer.GetFieldFromNode(node, _fileType.Object.GetIsCustomFieldId));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFieldFromNode_NonFactoryFieldId_ReturnsCustomField()
		{
			var doc = new XmlDocument();
			var node = doc.CreateElement("stage_source");
			node.SetAttribute("type", FieldInstance.kStringType);
			node.InnerText = "one hour";

			var fld = _serializer.GetFieldFromNode(node, _fileType.Object.GetIsCustomFieldId);

			Assert.AreEqual("custom_stage_source", fld.FieldId);
			Assert.AreEqual(FieldInstance.kStringType, fld.Type);
			Assert.AreEqual("one hour", fld.Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Load_FileDoesNotExist_Throws()
		{
			Assert.Throws<FileNotFoundException>(LoadFromStandardPlace);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Save_CannotCreateFile_Throws()
		{
			Assert.Throws<DirectoryNotFoundException>(() =>
				_serializer.Save(_fields, _parentFolder.Combine("notthere", "test.txt"), "x"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_NoFields_RoundTripped()
		{
			SaveToStandardPlace();
			LoadFromStandardPlace();
			Assert.AreEqual(0, _fields.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_TwoFactorytrings_RoundTripped()
		{
			var valueA = new FieldInstance("a", FieldInstance.kStringType, "aaa");
			_fields.Add(valueA);
			var valueB = new FieldInstance("b", FieldInstance.kStringType, "bbb");
			_fields.Add(valueB);

			DoRoundTrip();
			Assert.AreEqual(2, _fields.Count);
			Assert.IsTrue(_fields.Contains(valueA));
			Assert.IsTrue(_fields.Contains(valueB));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_UnrecognizedStringField_LoadsAsCustom()
		{
			var standardField = new FieldInstance("a", FieldInstance.kStringType, "aaa");
			_fields.Add(standardField);
			var fieldThatShouldLoadAsCustom = new FieldInstance("unrecognized", FieldInstance.kStringType, "bbb");
			_fields.Add(fieldThatShouldLoadAsCustom);

			DoRoundTrip();
			Assert.AreEqual(2, _fields.Count);
			Assert.IsTrue(_fields.Contains(standardField));
			Assert.IsTrue(_fields.Contains(new FieldInstance("custom_unrecognized", FieldInstance.kStringType, "bbb")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_CustomStringField_RoundTripped()
		{
			var standardField = new FieldInstance("a", FieldInstance.kStringType, "aaa");
			_fields.Add(standardField);
			var customField = new FieldInstance("custom_gloop", FieldInstance.kStringType, "bbb");
			_fields.Add(customField);

			DoRoundTrip();
			Assert.AreEqual(2, _fields.Count);
			Assert.IsTrue(_fields.Contains(standardField));
			Assert.IsTrue(_fields.Contains(customField));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_CustomFieldWithSameBaseNameAsFactoryField_RoundTrippedAsCustom()
		{
			var standardField = new FieldInstance("a", FieldInstance.kStringType, "aaa");
			_fields.Add(standardField);
			var customField = new FieldInstance("custom_a", FieldInstance.kStringType, "bbb");
			_fields.Add(customField);

			DoRoundTrip();
			Assert.AreEqual(2, _fields.Count);
			Assert.IsTrue(_fields.Contains(standardField));
			Assert.IsTrue(_fields.Contains(customField));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_StringsWithNewLines_RoundTripped()
		{
			var valueA = new FieldInstance("a", FieldInstance.kStringType, "aaa" + Environment.NewLine + "second line");
			_fields.Add(valueA);

			DoRoundTrip();
			Assert.AreEqual(1, _fields.Count);
			Assert.AreEqual(valueA, _fields[0]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Load_LoadingMultipleTimes_DoesNotIntroduceDuplicates()
		{
			_fields.Add(new FieldInstance("a", FieldInstance.kStringType, "aaa"));

			SaveToStandardPlace();
			LoadFromStandardPlace();
			LoadFromStandardPlace();
			LoadFromStandardPlace();
			Assert.AreEqual(1, _fields.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_StringWithXmlSymbols_RoundTripped()
		{
			_fields.Add(new FieldInstance("a", FieldInstance.kStringType, "<mess me up"));
			DoRoundTrip();
			Assert.AreEqual("<mess me up", _fields.First().ValueAsString);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_NonStringType_RoundTripped()
		{
			var value = new FieldInstance(_testFieldSerializer.ElementName, new DateTime(2010, 12, 13));
			_fields.Add(value);

			DoRoundTrip();
			Assert.AreEqual(1, _fields.Count);
			Assert.AreEqual(new DateTime(2010, 12, 13), (DateTime)_fields.First().Value);
		}

		/// ------------------------------------------------------------------------------------
		private void DoRoundTrip()
		{
			SaveToStandardPlace();
			_fields.Clear();
			LoadFromStandardPlace();
		}

		/// ------------------------------------------------------------------------------------
		private void SaveToStandardPlace()
		{
			_serializer.Save(_fields, _parentFolder.Combine("test.txt"), "x");
		}

		/// ------------------------------------------------------------------------------------
		private void LoadFromStandardPlace()
		{
			_serializer.Load(_fields, _parentFolder.Combine("test.txt"),
				"x", _fileType.Object);
		}
	}
}
