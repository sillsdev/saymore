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
		private List<FieldInstance> _fields;
		private FileSerializer _serializer;
		private TestFieldSerializer _testFieldSerializer;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_testFieldSerializer = new TestFieldSerializer();

			var fieldSerializers = new Dictionary<string, IXmlFieldSerializer>();
			fieldSerializers[_testFieldSerializer.ElementName] = _testFieldSerializer;

			_serializer = new FileSerializer(fieldSerializers);
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
			Assert.IsNull(_serializer.GetElementFromField(fld));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetElementFromField_FieldValueIsString_ReturnsCorrectElement()
		{
			var fld = new FieldInstance("a", "string", "blah");
			var e = _serializer.GetElementFromField(fld);

			Assert.IsNotNull("a", e.Name.ToString());
			Assert.AreEqual("string", e.Attribute("type").Value);
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
			node.SetAttribute("type", "string");
			node.InnerText = "element stuff";

			var fld = _serializer.GetFieldFromNode(node);

			Assert.AreEqual("a", fld.FieldId);
			Assert.AreEqual("string", fld.Type);
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
		public void SaveThenLoad_TwoStrings_RoundTripped()
		{
			var valueA = new FieldInstance("a", "string", "aaa");
			_fields.Add(valueA);
			var valueB = new FieldInstance("b", "string", "bbb");
			_fields.Add(valueB);

			DoRoundTrip();
			Assert.AreEqual(2, _fields.Count);
			Assert.IsTrue(_fields.Contains(valueA));
			Assert.IsTrue(_fields.Contains(valueB));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_StringsWithNewLines_RoundTripped()
		{
			var valueA = new FieldInstance("a", "string", "aaa" + Environment.NewLine + "second line");
			_fields.Add(valueA);

			DoRoundTrip();
			Assert.AreEqual(1, _fields.Count);
			Assert.AreEqual(valueA, _fields[0]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Load_LoadingMultipleTimes_DoesNotIntroduceDuplicates()
		{
			_fields.Add(new FieldInstance("a", "string", "aaa"));

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
			_fields.Add(new FieldInstance("a", "string", "<mess me up"));
			DoRoundTrip();
			Assert.AreEqual("<mess me up", _fields.First().ValueAsString);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveThenLoad_NoneStringType_RoundTripped()
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
			_serializer.Load(_fields, _parentFolder.Combine("test.txt"), "x");
		}
	}
}
