using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using SayMore.Transcription.Model;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class EafFileTests
	{
		private EafFile _file;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_file = new EafFile(null, null);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateRootElement_ReturnsCorrectElementContent()
		{
			var element = _file.CreateRootElement();

			Assert.AreEqual("ANNOTATION_DOCUMENT", element.Name.LocalName);
			Assert.AreEqual(string.Empty, element.Attribute("AUTHOR").Value);
			Assert.IsTrue(element.Attribute("DATE").Value.StartsWith(DateTime.Now.ToString("yyyy-MM-dd")));
			Assert.AreEqual("2.7", element.Attribute("FORMAT").Value);
			Assert.AreEqual("2.7", element.Attribute("VERSION").Value);
			Assert.IsTrue(element.Attribute(XNamespace.Xmlns + "xsi").IsNamespaceDeclaration);
			Assert.AreEqual("http://www.w3.org/2001/XMLSchema-instance", element.Attribute(XNamespace.Xmlns + "xsi").Value);
			Assert.IsNotNull(element.Attributes().SingleOrDefault(a =>
				a.Name.LocalName == "noNamespaceSchemaLocation" && a.Value == "http://www.mpi.nl/tools/elan/EAFv2.7.xsd"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMediaDescriptorElement_ReturnsCorrectElementContent()
		{
			var element = new EafFile(null, "Alathea.wav").CreateMediaDescriptorElement();
			Assert.AreEqual("MEDIA_DESCRIPTOR", element.Name.LocalName);
			Assert.AreEqual("Alathea.wav", element.Attribute("MEDIA_URL").Value);
			Assert.AreEqual("audio/x-wav", element.Attribute("MIME_TYPE").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMediaFileMimeType_WaveFile_ReturnsProperMimeType()
		{
			Assert.AreEqual("audio/x-wav", new EafFile(null, "Alathea.wav").CreateMediaFileMimeType());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMediaFileMimeType_Mp3File_ReturnsProperMimeType()
		{
			Assert.AreEqual("audio/*", new EafFile(null, "Alathea.mp3").CreateMediaFileMimeType());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateHeaderElement_ReturnsCorrectElementContent()
		{
			var element = new EafFile(null, "Great Lake Swimmers.wav").CreateHeaderElement();
			Assert.AreEqual("HEADER", element.Name.LocalName);
			Assert.AreEqual(string.Empty, element.Attribute("MEDIA_FILE").Value);
			Assert.AreEqual("milliseconds", element.Attribute("TIME_UNITS").Value);
			Assert.IsNotNull(element.Element("MEDIA_DESCRIPTOR"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateLastUsedAnnotationIdPropertyElement_ReturnsCorrectElementContent()
		{
			var element = _file.CreateLastUsedAnnotationIdPropertyElement(123);
			Assert.AreEqual(element.Name.LocalName, "PROPERTY");
			Assert.AreEqual(element.Attribute("NAME").Value, "lastUsedAnnotationId");
			Assert.AreEqual(element.Value, "123");
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateTimeOrderElement_TwoTimes_ReturnsCorrectSubElements()
		{
			var element = _file.CreateTimeOrderElement(new[] { 0f, 0f });
			Assert.AreEqual("TIME_ORDER", element.Name.LocalName);
			Assert.AreEqual("TIME_SLOT", element.Elements().ElementAt(0).Name.LocalName);
			Assert.AreEqual("TIME_SLOT", element.Elements().ElementAt(1).Name.LocalName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateTimeOrderElement_TwoTimes_ReturnsCorrectTimeSlotIds()
		{
			var element = _file.CreateTimeOrderElement(new[] { 0f, 0f });
			Assert.AreEqual("ts1", element.Elements().ElementAt(0).Attribute("TIME_SLOT_ID").Value);
			Assert.AreEqual("ts2", element.Elements().ElementAt(1).Attribute("TIME_SLOT_ID").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateTimeOrderElement_TwoTimes_ReturnsCorrectTimeValues()
		{
			var element = _file.CreateTimeOrderElement(new[] { 1.3f, 4.5f });
			Assert.AreEqual("1300", element.Elements().ElementAt(0).Attribute("TIME_VALUE").Value);
			Assert.AreEqual("4500", element.Elements().ElementAt(1).Attribute("TIME_VALUE").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateLinguisticTypeElement_ReturnsCorrectElementContent()
		{
			var element = _file.CreateLinguisticTypeElement();
			Assert.AreEqual("LINGUISTIC_TYPE", element.Name.LocalName);
			Assert.AreEqual("false", element.Attribute("GRAPHIC_REFERENCES").Value);
			Assert.AreEqual("default-lt", element.Attribute("LINGUISTIC_TYPE_ID").Value);
			Assert.AreEqual("true", element.Attribute("TIME_ALIGNABLE").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateLocaleElement_ReturnsCorrectElementContent()
		{
			var element = _file.CreateLocaleElement();
			Assert.AreEqual("LOCALE", element.Name.LocalName);
			Assert.AreEqual("US", element.Attribute("COUNTRY_CODE").Value);
			Assert.AreEqual("en", element.Attribute("LANGUAGE_CODE").Value);
		}
	}
}
