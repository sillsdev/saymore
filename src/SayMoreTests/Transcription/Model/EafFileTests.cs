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
		private EafFileHelper _eafFile;
		private AudioTier _mediaTier;
		private TextTier _textTier;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_eafFile = new EafFileHelper(null, null);

			_mediaTier = new AudioTier("teetering tier", "Fleet Foxes.mp3");
			_mediaTier.AddSegment(2f, 4f);
			_mediaTier.AddSegment(10f, 8f);

			_textTier = new TextTier("lead");
			_textTier.AddSegment("brass");
			_textTier.AddSegment("steel");
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotCollection_HasMediaTierWithNoSegments_ReturnsEmptyCollection()
		{
			_mediaTier = new AudioTier("teetering tier", "Fleet Foxes.mp3");
			Assert.AreEqual(0, _eafFile.GetTimeSlotCollection(_mediaTier).Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotCollection_HasMediaTierWithSegments_ReturnsCorrectCount()
		{
			Assert.AreEqual(4, _eafFile.GetTimeSlotCollection(_mediaTier).Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotCollection_NoDuplicateTimes_ReturnsCorrectCollection()
		{
			Assert.AreEqual(2f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(0));
			Assert.AreEqual(6f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(1));
			Assert.AreEqual(10f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(2));
			Assert.AreEqual(18f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotCollection_DuplicateTimes_ReturnsCorrectCollection()
		{
			_mediaTier = new AudioTier("teetering tier", null);
			_mediaTier.AddSegment(2f, 4f);
			_mediaTier.AddSegment(6f, 8f);

			Assert.AreEqual(3, _eafFile.GetTimeSlotCollection(_mediaTier).Count());
			Assert.AreEqual(2f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(0));
			Assert.AreEqual(6f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(1));
			Assert.AreEqual(14f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotCollection_OutOfOrderAndOverlappingSegments_ReturnsSortedCollection()
		{
			_mediaTier = new AudioTier("teetering tier", null);
			_mediaTier.AddSegment(100f, 50f);
			_mediaTier.AddSegment(70f, 40f);

			Assert.AreEqual(70f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(0));
			Assert.AreEqual(100f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(1));
			Assert.AreEqual(110f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(2));
			Assert.AreEqual(150f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateRootElement_ReturnsCorrectElementContent()
		{
			var element = _eafFile.CreateRootElement();

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
			var element = new EafFileHelper(null, "Alathea.wav").CreateMediaDescriptorElement();
			Assert.AreEqual("MEDIA_DESCRIPTOR", element.Name.LocalName);
			Assert.AreEqual("Alathea.wav", element.Attribute("MEDIA_URL").Value);
			Assert.AreEqual("audio/x-wav", element.Attribute("MIME_TYPE").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMediaFileMimeType_WaveFile_ReturnsProperMimeType()
		{
			Assert.AreEqual("audio/x-wav", new EafFileHelper(null, "Alathea.wav").CreateMediaFileMimeType());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMediaFileMimeType_Mp3File_ReturnsProperMimeType()
		{
			Assert.AreEqual("audio/*", new EafFileHelper(null, "Alathea.mp3").CreateMediaFileMimeType());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateHeaderElement_ReturnsCorrectElementContent()
		{
			var element = new EafFileHelper(null, "Great Lake Swimmers.wav").CreateHeaderElement();
			Assert.AreEqual("HEADER", element.Name.LocalName);
			Assert.AreEqual(string.Empty, element.Attribute("MEDIA_FILE").Value);
			Assert.AreEqual("milliseconds", element.Attribute("TIME_UNITS").Value);
			Assert.IsNotNull(element.Element("MEDIA_DESCRIPTOR"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateLastUsedAnnotationIdPropertyElement_ReturnsCorrectElementContent()
		{
			var element = _eafFile.CreateLastUsedAnnotationIdPropertyElement(123);
			Assert.AreEqual("PROPERTY", element.Name.LocalName);
			Assert.AreEqual("lastUsedAnnotationId", element.Attribute("NAME").Value);
			Assert.AreEqual("123", element.Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateTierElement_ReturnsCorrectElementContent()
		{
			var element = _eafFile.CreateTierElement("tippy tier");
			Assert.AreEqual("TIER", element.Name.LocalName);
			Assert.AreEqual("en", element.Attribute("DEFAULT_LOCALE").Value);
			Assert.AreEqual("default-lt", element.Attribute("LINGUISTIC_TYPE_REF").Value);
			Assert.AreEqual("tippy tier", element.Attribute("TIER_ID").Value);
		}

				/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateTimeSlotElements_ReturnsElementsWithCorrectNames()
		{
			var elements = _eafFile.CreateTimeSlotElements(new[] { 0f, 0f, 0f }).ToList();
			Assert.AreEqual("TIME_SLOT", elements[0].Name.LocalName);
			Assert.AreEqual("TIME_SLOT", elements[1].Name.LocalName);
			Assert.AreEqual("TIME_SLOT", elements[2].Name.LocalName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateTimeSlotElements_ReturnsElementsWithCorrectIds()
		{
			var elements = _eafFile.CreateTimeSlotElements(new[] { 0f, 0f, 0f }).ToList();
			Assert.AreEqual("ts1", elements[0].Attribute("TIME_SLOT_ID").Value);
			Assert.AreEqual("ts2", elements[1].Attribute("TIME_SLOT_ID").Value);
			Assert.AreEqual("ts3", elements[2].Attribute("TIME_SLOT_ID").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateTimeSlotElements_ReturnsElementsWithCorrectValues()
		{
			var elements = _eafFile.CreateTimeSlotElements(new[] { 0.3f, 0.7f, 0.9f }).ToList();
			Assert.AreEqual("300", elements[0].Attribute("TIME_VALUE").Value);
			Assert.AreEqual("700", elements[1].Attribute("TIME_VALUE").Value);
			Assert.AreEqual("900", elements[2].Attribute("TIME_VALUE").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateAnnotationElement_NullTextTier_ReturnsEmptyList()
		{
			Assert.AreEqual(0, _eafFile.CreateAnnotationElements(null,
				_mediaTier.GetAllSegments().Cast<IMediaSegment>(), null).Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateAnnotationElement_NullMediaSegments_ReturnsEmptyList()
		{
			Assert.AreEqual(0, _eafFile.CreateAnnotationElements(new TextTier("iron"), null, null).Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateAnnotationElement_NoTextSegments_ReturnsEmptyList()
		{
			Assert.AreEqual(0, _eafFile.CreateAnnotationElements(new TextTier("copper"),
				_mediaTier.GetAllSegments().Cast<IMediaSegment>(), null).Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateAnnotationElement_GoodData_ReturnsCorrectNumberOfElements()
		{
			Assert.AreEqual(2, _eafFile.CreateAnnotationElements(_textTier,
				_mediaTier.GetAllSegments().Cast<IMediaSegment>(), f => "").Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateAnnotationElement_GoodData_ReturnsCorrectElementNames()
		{
			var elements = _eafFile.CreateAnnotationElements(_textTier,
				_mediaTier.GetAllSegments().Cast<IMediaSegment>(), f => "").ToList();

			Assert.AreEqual("ANNOTATION", elements[0].Name.LocalName);
			Assert.AreEqual("ANNOTATION", elements[1].Name.LocalName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateAnnotationElement_GoodData_ReturnsCorrectAnnotationIds()
		{
			var elements = _eafFile.CreateAnnotationElements(_textTier,
				_mediaTier.GetAllSegments().Cast<IMediaSegment>(), f => "").ToList();

			Assert.AreEqual("a1", elements[0].Element("ALIGNABLE_ANNOTATION").Attribute("ANNOTATION_ID").Value);
			Assert.AreEqual("a2", elements[1].Element("ALIGNABLE_ANNOTATION").Attribute("ANNOTATION_ID").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateAnnotationElement_GoodData_ReturnsCorrectTimeSlotRefs()
		{
			int i = 5;

			var elements = _eafFile.CreateAnnotationElements(_textTier,
				_mediaTier.GetAllSegments().Cast<IMediaSegment>(), f => "ts" + i++).ToList();

			Assert.AreEqual("ts5", elements[0].Element("ALIGNABLE_ANNOTATION").Attribute("TIME_SLOT_REF1").Value);
			Assert.AreEqual("ts6", elements[0].Element("ALIGNABLE_ANNOTATION").Attribute("TIME_SLOT_REF2").Value);
			Assert.AreEqual("ts7", elements[1].Element("ALIGNABLE_ANNOTATION").Attribute("TIME_SLOT_REF1").Value);
			Assert.AreEqual("ts8", elements[1].Element("ALIGNABLE_ANNOTATION").Attribute("TIME_SLOT_REF2").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateAnnotationElement_GoodData_ReturnsCorrectAnnotations()
		{
			var elements = _eafFile.CreateAnnotationElements(_textTier,
				_mediaTier.GetAllSegments().Cast<IMediaSegment>(), f => "").ToList();

			Assert.AreEqual("brass", elements[0].Element("ALIGNABLE_ANNOTATION").Element("ANNOTATION_VALUE").Value);
			Assert.AreEqual("steel", elements[1].Element("ALIGNABLE_ANNOTATION").Element("ANNOTATION_VALUE").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateAlignableAnnotationElement_ReturnsCorrectElementContent()
		{
			var element = _eafFile.CreateAlignableAnnotationElement(48, "ts33", "ts55", "some text");
			Assert.AreEqual("ALIGNABLE_ANNOTATION", element.Name.LocalName);
			Assert.AreEqual("a48", element.Attribute("ANNOTATION_ID").Value);
			Assert.AreEqual("ts33", element.Attribute("TIME_SLOT_REF1").Value);
			Assert.AreEqual("ts55", element.Attribute("TIME_SLOT_REF2").Value);
			Assert.AreEqual("some text", element.Element("ANNOTATION_VALUE").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateLinguisticTypeElement_ReturnsCorrectElementContent()
		{
			var element = _eafFile.CreateLinguisticTypeElement();
			Assert.AreEqual("LINGUISTIC_TYPE", element.Name.LocalName);
			Assert.AreEqual("false", element.Attribute("GRAPHIC_REFERENCES").Value);
			Assert.AreEqual("default-lt", element.Attribute("LINGUISTIC_TYPE_ID").Value);
			Assert.AreEqual("true", element.Attribute("TIME_ALIGNABLE").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateLocaleElement_ReturnsCorrectElementContent()
		{
			var element = _eafFile.CreateLocaleElement();
			Assert.AreEqual("LOCALE", element.Name.LocalName);
			Assert.AreEqual("US", element.Attribute("COUNTRY_CODE").Value);
			Assert.AreEqual("en", element.Attribute("LANGUAGE_CODE").Value);
		}
	}
}
