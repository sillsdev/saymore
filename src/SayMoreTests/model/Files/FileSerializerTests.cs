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
				var element = GetElementFromXml(xmlBlob).Element("date");
				return new DateTime(
					int.Parse(element.Element("year").Value),
					int.Parse(element.Element("month").Value),
					int.Parse(element.Element("day").Value));
			}

			public override XElement Serialize(object obj)
			{
				return InternalSerialize(obj, typeof(DateTime), element =>
				{
					var e = new XElement("date");
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
		private List<FieldInstance> _standardFields;
		private List<FieldInstance> _customFields;
		private FileSerializer _serializer;
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

			_serializer = new FileSerializer(fieldSerializers);
			_parentFolder = new TemporaryFolder("fileTypeTest");
			_standardFields = new List<FieldInstance>();
			_customFields = new List<FieldInstance>();
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
			Assert.IsNull(_serializer.GetElementFromField(fld));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetElementFromField_FieldValueIsString_ReturnsCorrectElement()
		{
			var fld = new FieldInstance("a", FieldInstance.kStringType, "blah");
			var e = _serializer.GetElementFromField(fld);

			Assert.IsNotNull("a", e.Name.ToString());
			Assert.AreEqual(FieldInstance.kStringType, e.Attribute("type").Value);
			Assert.AreEqual("blah", e.Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetElementFromField_FieldValueIsNonString_ReturnsCorrectElements()
		{
			var elementName = _testFieldSerializer.ElementName;

			var fld = new FieldInstance(elementName, new DateTime(1963, 4, 19));
			var e = _serializer.GetElementFromField(fld);

			Assert.IsNotNull(elementName, e.Name.ToString());
			Assert.AreEqual("xml", e.Attribute("type").Value);
			var dateElement = e.Element("date");

			Assert.AreEqual(3, dateElement.Elements().Count());
			Assert.AreEqual("1963", dateElement.Element("year").Value);
			Assert.AreEqual("4", dateElement.Element("month").Value);
			Assert.AreEqual("19", dateElement.Element("day").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFieldFromNode_NodeContainsStringType_ReturnsField()
		{
			var doc = new XmlDocument();
			var node = doc.CreateElement("a");
			node.SetAttribute("type", FieldInstance.kStringType);
			node.InnerText = "element stuff";

			var fld = _serializer.GetFieldFromNode(node);

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

			var fld = _serializer.GetFieldFromNode(node);

			Assert.AreEqual("a", fld.FieldId);
			Assert.AreEqual(FieldInstance.kStringType, fld.Type);
			Assert.AreEqual("element stuff", fld.Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFieldFromNode_NodeContainsNonStringType_ReturnsField()
		{
			var doc = new XmlDocument();
			var node = doc.CreateElement("dates");
			node.SetAttribute("type", "xml");
			node.InnerXml = "<date><year>1963</year><month>4</month><day>19</day></date>";

			var fld = _serializer.GetFieldFromNode(node);

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

			Assert.IsNull(_serializer.GetFieldFromNode(node));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFieldFromNode_UnknownType_ReturnsNull()
		{
			var doc = new XmlDocument();
			var node = doc.CreateElement("a");
			node.SetAttribute("type", "flubber");
			node.InnerText = "element stuff";

			Assert.IsNull(_serializer.GetFieldFromNode(node));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFieldFromNode_CustomFieldsNode_ReturnsNull()
		{
			var doc = new XmlDocument();
			var node = doc.CreateElement("CustomFields");
			node.InnerText = "<stage_source type=\"string\">one hour</stage_source>";

			Assert.IsNull(_serializer.GetFieldFromNode(node));
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
				_serializer.Save(_standardFields, null, _parentFolder.Combine("notthere", "test.txt"), "x"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_NoFields_RoundTripped()
		{
			SaveToStandardPlace();
			LoadFromStandardPlace();
			Assert.AreEqual(0, _standardFields.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_TwoStrings_RoundTripped()
		{
			var valueA = new FieldInstance("a", FieldInstance.kStringType, "aaa");
			_standardFields.Add(valueA);
			var valueB = new FieldInstance("b", FieldInstance.kStringType, "bbb");
			_standardFields.Add(valueB);

			DoRoundTrip();
			Assert.AreEqual(2, _standardFields.Count);
			Assert.IsTrue(_standardFields.Contains(valueA));
			Assert.IsTrue(_standardFields.Contains(valueB));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_UnrecognizedStringField_LoadsAsCustom()
		{
			var standardField = new FieldInstance("a", FieldInstance.kStringType, "aaa");
			_standardFields.Add(standardField);
			var fieldThatShouldLoadAsCustom = new FieldInstance("unrecognized", FieldInstance.kStringType, "bbb");
			_standardFields.Add(fieldThatShouldLoadAsCustom);

			DoRoundTrip();
			Assert.AreEqual(1, _standardFields.Count);
			Assert.AreEqual(1, _customFields.Count);
			Assert.IsTrue(_standardFields.Contains(standardField));
			Assert.IsTrue(_customFields.Contains(fieldThatShouldLoadAsCustom));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_CustomStringField_RoundTripped()
		{
			var standardField = new FieldInstance("a", FieldInstance.kStringType, "aaa");
			_standardFields.Add(standardField);
			var customField = new FieldInstance("custom", FieldInstance.kStringType, "bbb");
			_customFields.Add(customField);

			DoRoundTrip();
			Assert.AreEqual(1, _standardFields.Count);
			Assert.AreEqual(1, _customFields.Count);
			Assert.IsTrue(_standardFields.Contains(standardField));
			Assert.IsTrue(_customFields.Contains(customField));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_CustomFieldWithSameNameAsFactoryField_RoundTrippedAsCustom()
		{
			var standardField = new FieldInstance("a", FieldInstance.kStringType, "aaa");
			_standardFields.Add(standardField);
			var customField = new FieldInstance("b", FieldInstance.kStringType, "bbb");
			_customFields.Add(customField);

			DoRoundTrip();
			Assert.AreEqual(1, _standardFields.Count);
			Assert.AreEqual(1, _customFields.Count);
			Assert.IsTrue(_standardFields.Contains(standardField));
			Assert.IsTrue(_customFields.Contains(customField));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_StringsWithNewLines_RoundTripped()
		{
			var valueA = new FieldInstance("a", FieldInstance.kStringType, "aaa" + Environment.NewLine + "second line");
			_standardFields.Add(valueA);

			DoRoundTrip();
			Assert.AreEqual(1, _standardFields.Count);
			Assert.AreEqual(valueA, _standardFields[0]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Load_LoadingMultipleTimes_DoesNotIntroduceDuplicates()
		{
			_standardFields.Add(new FieldInstance("a", FieldInstance.kStringType, "aaa"));

			SaveToStandardPlace();
			LoadFromStandardPlace();
			LoadFromStandardPlace();
			LoadFromStandardPlace();
			Assert.AreEqual(1, _standardFields.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_StringWithXmlSymbols_RoundTripped()
		{
			_standardFields.Add(new FieldInstance("a", FieldInstance.kStringType, "<mess me up"));
			DoRoundTrip();
			Assert.AreEqual("<mess me up", _standardFields.First().ValueAsString);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_NonStringType_RoundTripped()
		{
			var value = new FieldInstance(_testFieldSerializer.ElementName, new DateTime(2010, 12, 13));
			_standardFields.Add(value);

			DoRoundTrip();
			Assert.AreEqual(1, _standardFields.Count);
			Assert.AreEqual(new DateTime(2010, 12, 13), (DateTime)_standardFields.First().Value);
		}

		/// ------------------------------------------------------------------------------------
		private void DoRoundTrip()
		{
			SaveToStandardPlace();
			_standardFields.Clear();
			_customFields.Clear();
			LoadFromStandardPlace();
		}

		/// ------------------------------------------------------------------------------------
		private void SaveToStandardPlace()
		{
			_serializer.Save(_standardFields, _customFields, _parentFolder.Combine("test.txt"), "x");
		}

		/// ------------------------------------------------------------------------------------
		private void LoadFromStandardPlace()
		{
			_serializer.Load(_standardFields, _customFields, _parentFolder.Combine("test.txt"),
				"x", _fileType.Object);
		}
	}
}
