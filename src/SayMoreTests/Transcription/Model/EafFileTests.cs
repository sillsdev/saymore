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
		private EafFile _eafFile;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_eafFile = new EafFile(null, null);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMediaSegments_DoesNotHaveSegments_ThrowsException()
		{
			Assert.Throws<NullReferenceException>(() => _eafFile.GetMediaSegments());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMediaSegments_HasSegments_ReturnsCorrectCount()
		{
			SetupWithAudioTier();
			Assert.AreEqual(2, _eafFile.GetMediaSegments().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMediaSegments_HasSegments_ReturnsCorrectSegmentMediaFiles()
		{
			SetupWithAudioTier();
			Assert.AreEqual("Fleet Foxes.mp3", _eafFile.GetMediaSegments().ElementAt(0).MediaFile);
			Assert.AreEqual("Fleet Foxes.mp3", _eafFile.GetMediaSegments().ElementAt(1).MediaFile);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMediaSegments_HasSegments_ReturnsCorrectStartTimes()
		{
			SetupWithAudioTier();
			Assert.AreEqual(2f, _eafFile.GetMediaSegments().ElementAt(0).MediaStart);
			Assert.AreEqual(10f, _eafFile.GetMediaSegments().ElementAt(1).MediaStart);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMediaSegments_HasSegments_ReturnsCorrectLengths()
		{
			SetupWithAudioTier();
			Assert.AreEqual(4f, _eafFile.GetMediaSegments().ElementAt(0).MediaLength);
			Assert.AreEqual(8f, _eafFile.GetMediaSegments().ElementAt(1).MediaLength);
		}

		/// ------------------------------------------------------------------------------------
		private void SetupWithAudioTier()
		{
			var tier = new AudioTier("teetering tier", "Fleet Foxes.mp3");
			tier.AddSegment(2f, 4f);
			tier.AddSegment(10f, 8f);
			_eafFile.SetMediaTier(tier);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotCollection_DoesNotHaveMediaTier_ThrowException()
		{
			Assert.Throws<NullReferenceException>(() => _eafFile.GetTimeSlotCollection());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotCollection_HasMediaTierWithNoSegments_ReturnsEmptyCollection()
		{
			var tier = new AudioTier("teetering tier", "Fleet Foxes.mp3");
			_eafFile.SetMediaTier(tier);
			Assert.AreEqual(0, _eafFile.GetTimeSlotCollection().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotCollection_HasMediaTierWithSegments_ReturnsCorrectCount()
		{
			SetupWithAudioTier();
			Assert.AreEqual(4, _eafFile.GetTimeSlotCollection().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotCollection_NoDuplicateTimes_ReturnsCorrectCollection()
		{
			SetupWithAudioTier();
			Assert.AreEqual(2f, _eafFile.GetTimeSlotCollection().ElementAt(0));
			Assert.AreEqual(6f, _eafFile.GetTimeSlotCollection().ElementAt(1));
			Assert.AreEqual(10f, _eafFile.GetTimeSlotCollection().ElementAt(2));
			Assert.AreEqual(18f, _eafFile.GetTimeSlotCollection().ElementAt(3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotCollection_DuplicateTimes_ReturnsCorrectCollection()
		{
			var tier = new AudioTier("teetering tier", null);
			tier.AddSegment(2f, 4f);
			tier.AddSegment(6f, 8f);
			_eafFile.SetMediaTier(tier);

			Assert.AreEqual(3, _eafFile.GetTimeSlotCollection().Count());
			Assert.AreEqual(2f, _eafFile.GetTimeSlotCollection().ElementAt(0));
			Assert.AreEqual(6f, _eafFile.GetTimeSlotCollection().ElementAt(1));
			Assert.AreEqual(14f, _eafFile.GetTimeSlotCollection().ElementAt(2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotCollection_OutOfOrderAndOverlappingSegments_ReturnsSortedCollection()
		{
			var tier = new AudioTier("teetering tier", null);
			tier.AddSegment(100f, 50f);
			tier.AddSegment(70f, 40f);
			_eafFile.SetMediaTier(tier);

			Assert.AreEqual(70f, _eafFile.GetTimeSlotCollection().ElementAt(0));
			Assert.AreEqual(100f, _eafFile.GetTimeSlotCollection().ElementAt(1));
			Assert.AreEqual(110f, _eafFile.GetTimeSlotCollection().ElementAt(2));
			Assert.AreEqual(150f, _eafFile.GetTimeSlotCollection().ElementAt(3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotId_NoSegments_ReturnsNull()
		{
			var tier = new AudioTier("teetering tier", null);
			_eafFile.SetMediaTier(tier);
			Assert.IsNull(_eafFile.GetTimeSlotId(4f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotId_ForNonExistantTime_ReturnsNull()
		{
			SetupWithAudioTier();
			Assert.IsNull(_eafFile.GetTimeSlotId(444f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlotId_ForExistantTime_ReturnsNull()
		{
			SetupWithAudioTier();
			Assert.AreEqual("ts1", _eafFile.GetTimeSlotId(2f));
			Assert.AreEqual("ts2", _eafFile.GetTimeSlotId(6f));
			Assert.AreEqual("ts3", _eafFile.GetTimeSlotId(10f));
			Assert.AreEqual("ts4", _eafFile.GetTimeSlotId(18f));
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
		public void CreateAnnotationElement_TextSegmentIsNull_ReturnsNull()
		{
			Assert.IsNull(_eafFile.CreateAnnotationElement(null, null, 0));
			Assert.IsNull(_eafFile.CreateAnnotationElement(null,
				new AudioSegment(null, null, 0f, 0f) as ITextSegment, 0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateAnnotationElement_TimeSlotsNotFound_ReturnsNull()
		{
			SetupWithAudioTier();
			var textSeg = new TextSegment(null, null);

			// Invalid stop time
			Assert.IsNull(_eafFile.CreateAnnotationElement(
				new AudioSegment(null, null, 10f, 111f), textSeg, 0));

			// Invalid start time
			Assert.IsNull(_eafFile.CreateAnnotationElement(
				new AudioSegment(null, null, 111f, 8f), textSeg, 0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateAnnotationElement_GoodData_ReturnsCorrectElements()
		{
			SetupWithAudioTier();
			var textSeg = new TextSegment(null, string.Empty);

			var element = _eafFile.CreateAnnotationElement(
				_eafFile.GetMediaSegments().ElementAt(0), textSeg, 5);

			Assert.AreEqual("ANNOTATION", element.Name.LocalName);
			Assert.IsNotNull(element.Element("ALIGNABLE_ANNOTATION"));
			Assert.IsNotNull(element.Element("ALIGNABLE_ANNOTATION").Element("ANNOTATION_VALUE"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateTierElement_TextTierHasNoTextSegments_ReturnsEmptyElement()
		{
			SetupWithAudioTier();
			var element = _eafFile.CreateTierElement(new TextTier("blah"));
			Assert.AreEqual("TIER", element.Name.LocalName);
			Assert.AreEqual(0, element.Elements().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateTierElement_TextTierHasSegments_ReturnsCorrectElements()
		{
			SetupWithAudioTier();
			var textTier = new TextTier("blah");
			textTier.AddSegment("blah one");
			textTier.AddSegment("blah two");

			var element = _eafFile.CreateTierElement(textTier);
			Assert.AreEqual("TIER", element.Name.LocalName);
			Assert.AreEqual(2, element.Elements().Count());
			Assert.AreEqual(2, element.Elements("ANNOTATION").Count());
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
		public void CreateTimeOrderElement_TwoTimes_ReturnsCorrectSubElements()
		{
			var element = _eafFile.CreateTimeOrderElement(new[] { 0f, 0f });
			Assert.AreEqual("TIME_ORDER", element.Name.LocalName);
			Assert.AreEqual("TIME_SLOT", element.Elements().ElementAt(0).Name.LocalName);
			Assert.AreEqual("TIME_SLOT", element.Elements().ElementAt(1).Name.LocalName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateTimeOrderElement_TwoTimes_ReturnsCorrectTimeSlotIds()
		{
			var element = _eafFile.CreateTimeOrderElement(new[] { 0f, 0f });
			Assert.AreEqual("ts1", element.Elements().ElementAt(0).Attribute("TIME_SLOT_ID").Value);
			Assert.AreEqual("ts2", element.Elements().ElementAt(1).Attribute("TIME_SLOT_ID").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateTimeOrderElement_TwoTimes_ReturnsCorrectTimeValues()
		{
			var element = _eafFile.CreateTimeOrderElement(new[] { 1.3f, 4.5f });
			Assert.AreEqual("1300", element.Elements().ElementAt(0).Attribute("TIME_VALUE").Value);
			Assert.AreEqual("4500", element.Elements().ElementAt(1).Attribute("TIME_VALUE").Value);
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
