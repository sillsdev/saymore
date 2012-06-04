using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using NUnit.Framework;
using Palaso.Reporting;
using Palaso.TestUtilities;
using SayMore.Transcription.Model;
using SayMoreTests.Model.Files;

namespace SayMoreTests.Transcription.Model
{
	// Methods still needing tests:
	//  - SaveAnnotations
	//  - CreateFileFromFile
	//  - CreateFileFromTimesAsString
	//  - CreateFileFromSegments

	[TestFixture]
	public class AnnotationFileHelperTests
	{
		private AnnotationFileHelper _helper;
		private TemporaryFolder _folder;
		private string _basicEafFileName;

		private XElement _root;
		private XElement _header;
		private XElement _mediaDescriptor;
		private XAttribute _mediaUrl;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			ErrorReport.IsOkToInteractWithUser = false;
			_folder = new TemporaryFolder("AnnotationFileHelperTests");
			_basicEafFileName = _folder.Combine("basic.eaf");
			_root = new XElement("ANNOTATION_DOCUMENT");
			_header = new XElement("HEADER");
			_mediaDescriptor = new XElement("MEDIA_DESCRIPTOR");
			_mediaUrl = new XAttribute("MEDIA_URL", "UninspiredMediaFileName.wav");
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_folder.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadEafFile(bool loadBasicEafFile = true, bool setMediaFile = true)
		{

			if (!loadBasicEafFile)
				_helper = AnnotationFileHelper.Load(CreateTestEaf());
			else
			{
				_root.Save(_basicEafFileName);
				_helper = AnnotationFileHelper.Load(_basicEafFileName);
			}

			if (setMediaFile)
			{
				var mediaFilePath = MediaFileInfoTests.GetShortTestAudioFile();
				File.Move(mediaFilePath, _folder.Combine("dummy.wav"));
				_helper.SetMediaFile(_folder.Combine("dummy.wav"));
			}

			_root = _helper.Root;
			Assert.IsNotNull(_helper);
		}

		/// ------------------------------------------------------------------------------------
		private string CreateTestEaf()
		{
			var path = _folder.Combine("test.eaf");
			CreateTestEaf(path);
			return path;
		}

		/// ------------------------------------------------------------------------------------
		public static void CreateTestEaf(string filename)
		{
			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream("SayMoreTests.Resources.test.eaf"))
			{
				var buffer = new byte[stream.Length];
				for (int i = 0; i < buffer.Length; i++)
					buffer[i] = (byte)stream.ReadByte();

				File.WriteAllBytes(filename, buffer);
				stream.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Save_ExpectedTiers_SavesAsExpected()
		{
			var mediaFilePath = MediaFileInfoTests.GetShortTestAudioFile();
			var origMediaFilename = Path.Combine(_folder.Path, "junk.wav");
			File.Move(mediaFilePath, origMediaFilename);

			var tiers = new TierCollection(origMediaFilename);
			tiers.GetTimeTier().AddSegment(0, 3);
			tiers.GetTimeTier().AddSegment(3, 23.8f);
			tiers.GetTranscriptionTier().AddSegment("yo momma");
			tiers.GetTranscriptionTier().AddSegment("yo dadda");
			tiers.GetFreeTranslationTier().AddSegment("your mother");
			tiers.GetFreeTranslationTier().AddSegment("your father");

			var eafFilename = AnnotationFileHelper.Save(origMediaFilename, tiers);

			Assert.AreEqual(Path.Combine(_folder.Path, "junk.wav.annotations.eaf"), eafFilename);
			var loadedTiers = AnnotationFileHelper.Load(eafFilename).GetTierCollection();

			CheckTiersAreEqual(tiers, loadedTiers);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Save_NoTextTiers_CreatesTextTiersWithEmptySegments()
		{
			var mediaFilePath = MediaFileInfoTests.GetShortTestAudioFile();
			var origMediaFilename = Path.Combine(_folder.Path, "junk.wav");
			File.Move(mediaFilePath, origMediaFilename);

			var tiers = new TierCollection(origMediaFilename);
			tiers.GetTimeTier().AddSegment(0, 3);
			tiers.GetTimeTier().AddSegment(3, 23.8f);
			tiers.GetTranscriptionTier().AddSegment(string.Empty);
			tiers.GetTranscriptionTier().AddSegment(string.Empty);
			tiers.GetFreeTranslationTier().AddSegment(string.Empty);
			tiers.GetFreeTranslationTier().AddSegment(string.Empty);
			var expectedCollection = tiers.Copy();
			tiers.RemoveAt(2);
			tiers.RemoveAt(1);

			var eafFilename = AnnotationFileHelper.Save(origMediaFilename, tiers);

			Assert.AreEqual(Path.Combine(_folder.Path, "junk.wav.annotations.eaf"), eafFilename);
			var loadedTiers = AnnotationFileHelper.Load(eafFilename).GetTierCollection();

			CheckTiersAreEqual(expectedCollection, loadedTiers);
		}

		/// ------------------------------------------------------------------------------------
		private void CheckTiersAreEqual(TierCollection expected, TierCollection actual)
		{
			Assert.AreEqual(expected.Count, actual.Count);
			for (int i = 0; i < expected.Count; i++)
				TierCollectionTests.CheckTier(expected[i], actual[i]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsElanFile_FileNameNull_ReturnsFalse()
		{
			Assert.IsFalse(AnnotationFileHelper.GetIsElanFile(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsElanFile_FileNameEmpty_ReturnsFalse()
		{
			Assert.IsFalse(AnnotationFileHelper.GetIsElanFile(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsElanFile_InvalidXmlFile_ReturnsFalse()
		{
			var filename = _folder.Combine("bad.xml");
			File.CreateText(filename).Close();
			Assert.IsFalse(AnnotationFileHelper.GetIsElanFile(filename));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsElanFile_ValidXmlButNotEafFile_ReturnsFalse()
		{
			var filename = _folder.Combine("goodBadEaf.xml");
			var element = new XElement("root", "blah blah");
			element.Save(filename);
			Assert.IsFalse(AnnotationFileHelper.GetIsElanFile(filename));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsElanFile_ValidEafFile_ReturnsTrue()
		{
			Assert.IsTrue(AnnotationFileHelper.GetIsElanFile(CreateTestEaf()));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToMediaFile_NoHeaderElement_ReturnsNull()
		{
			LoadEafFile(true, false);
			Assert.IsNull(_helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToMediaFile_NoMediaDescriptorElement_ReturnsNull()
		{
			_root.Add(_header);
			LoadEafFile(true, false);
			Assert.IsNull(_helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToMediaFile_NoMediaUrlAttribute_ReturnsNull()
		{
			_root.Add(_header);
			LoadEafFile(true, false);
			Assert.IsNull(_helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToMediaFile_AllElementsAndAttributesPresent_ReturnsMediaFileName()
		{
			_mediaDescriptor.Add(_mediaUrl);
			_header.Add(_mediaDescriptor);
			_root.Add(_header);
			LoadEafFile();
			Assert.AreEqual(Path.Combine(_helper.GetAnnotationFolderPath(),
				"dummy.wav"), _helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetMediaFile_HeaderMissing_SetsMediaFileName()
		{
			LoadEafFile(true, false);
			Assert.IsNull(_helper.GetFullPathToMediaFile());
			_helper.SetMediaFile("BeaversAndDucks.mp3");
			Assert.AreEqual(Path.Combine(_helper.GetAnnotationFolderPath(), "BeaversAndDucks.mp3"), _helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetMediaFile_MediaDescriptorMissing_SetsMediaFileName()
		{
			_root.Add(_header);
			LoadEafFile(true, false);
			Assert.IsNull(_helper.GetFullPathToMediaFile());
			_helper.SetMediaFile("BeaversAndDucks.mp3");
			Assert.AreEqual(Path.Combine(_helper.GetAnnotationFolderPath(), "BeaversAndDucks.mp3"), _helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetMediaFile_MediaUrlMissing_SetsMediaFileName()
		{
			_header.Add(_mediaDescriptor);
			_root.Add(_header);
			LoadEafFile(true, false);
			Assert.IsNull(_helper.GetFullPathToMediaFile());
			_helper.SetMediaFile("BeaversAndDucks.mp3");
			Assert.AreEqual(Path.Combine(_helper.GetAnnotationFolderPath(), "BeaversAndDucks.mp3"), _helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetMediaFile_AllElementsAndAttributesPresent_SetsMediaFileName()
		{
			_mediaDescriptor.Add(_mediaUrl);
			_header.Add(_mediaDescriptor);
			_root.Add(_header);
			LoadEafFile(true, false);
			Assert.AreEqual(Path.Combine(_helper.GetAnnotationFolderPath(), "UninspiredMediaFileName.wav"), _helper.GetFullPathToMediaFile());
			_helper.SetMediaFile("BeaversAndDucks.mp3");
			Assert.AreEqual(Path.Combine(_helper.GetAnnotationFolderPath(), "BeaversAndDucks.mp3"), _helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeMediaFileName_ChangesMediaFileName()
		{
			var testEafFile = CreateTestEaf();
			_helper = AnnotationFileHelper.Load(testEafFile);
			Assert.AreEqual(Path.Combine(_helper.GetAnnotationFolderPath(), "AmazingGrace.wav"), _helper.GetFullPathToMediaFile());

			AnnotationFileHelper.ChangeMediaFileName(testEafFile, "PiratesAndDawgs.mpg");
			_helper = AnnotationFileHelper.Load(testEafFile);
			Assert.AreEqual(Path.Combine(_helper.GetAnnotationFolderPath(), "PiratesAndDawgs.mpg"), _helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextAvailableAnnotationIdAndIncrement_HeaderMissing_ReturnsOne()
		{
			LoadEafFile();
			Assert.AreEqual("a1", _helper.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextAvailableAnnotationIdAndIncrement_PropertyMissing_ReturnsOne()
		{
			_root.Add(_header);
			LoadEafFile();
			Assert.AreEqual("a1", _helper.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextAvailableAnnotationIdAndIncrement_LastIdAttributeMissing_ReturnsOne()
		{
			LoadEafFile();
			Assert.AreEqual("a1", _helper.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextAvailableAnnotationIdAndIncrement_AllElementsAndAttributesPresent_ReturnsCorrectValue()
		{
			LoadEafFile();

			var element = _helper.Root.Element("HEADER").Elements("PROPERTY")
				.First(e => e.Attribute("NAME").Value == "lastUsedAnnotationId");

			element.SetValue(5);

			Assert.AreEqual("a6", _helper.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlots_MissingTimeOrderElement_ReturnsEmptyList()
		{
			LoadEafFile();
			Assert.IsEmpty(_helper.GetTimeSlots().ToList());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlots_MissingTimeOrderElement_CreatesTimeOrderElement()
		{
			LoadEafFile();
			Assert.IsNull(_helper.Root.Element("TIME_ORDER"));
			_helper.GetTimeSlots();
			Assert.IsNotNull(_helper.Root.Element("TIME_ORDER"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlots_EmptyTimeOrderElement_ReturnsEmptyList()
		{
			_root.Add(new XElement("TIME_ORDER"));
			LoadEafFile();
			Assert.IsEmpty(_helper.GetTimeSlots().ToList());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlots_TimeSlotsExist_ReturnsList()
		{
			LoadEafFile(false);

			var list = _helper.GetTimeSlots();
			Assert.AreEqual(6, list.Count);
			Assert.AreEqual(0f, list["ts1"]);
			Assert.AreEqual(0.75f, list["ts2"]);
			Assert.AreEqual(0.75f, list["ts3"]);
			Assert.AreEqual(1.25f, list["ts4"]);
			Assert.AreEqual(1.25f, list["ts5"]);
			Assert.AreEqual(2.121f, list["ts6"]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlots_UnspecifiedTimeValuesExist_CalculatesRegularIntervals()
		{
			LoadEafFile();
			var timeElement = new XElement("TIME_ORDER");
			AddTimeSlots(timeElement, new[] { 10, 20, -1, -1, 37 });
			_helper.Root.Add(timeElement);

			var timeList = _helper.GetTimeSlots();
			Assert.AreEqual(0.010f, timeList["ts1"]);
			Assert.AreEqual(0.020f, timeList["ts2"]);
			Assert.AreEqual((20 + (17f / 3)) / 1000f, timeList["ts3"]);
			Assert.AreEqual((20 + (17f / 3) + (17f / 3)) / 1000f, timeList["ts4"]);
			Assert.AreEqual(0.037f, timeList["ts5"]);
		}

		/// ------------------------------------------------------------------------------------
		private void AddTimeSlots(XElement timeElement, IEnumerable<int> values)
		{
			int id = 1;
			foreach (int val in values)
			{
				var slot = new XElement("TIME_SLOT", new XAttribute("TIME_SLOT_ID", string.Format("ts{0}", id++)));
				if (val >= 0)
					slot.Add(new XAttribute("TIME_VALUE", val));

				timeElement.Add(slot);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveTimeSlots_NoTimeSubdivisionDependentTiers_RemovesThem()
		{
			LoadEafFile(false);
			Assert.AreEqual(6, _helper.GetTimeSlots().Count);
			_helper.RemoveTimeSlots();
			Assert.AreEqual(0, _helper.GetTimeSlots().Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveTimeSlots_WithTimeSubdivisionDependentTiers_DoesNotRemoveThem()
		{
			LoadEafFile(false);

			_helper.Root.Add(new XElement("TIER",
				new XAttribute("PARENT_REF", "Transcription"),
				new XAttribute("TIER_ID", "junk"),
				new XElement("ANNOTATION", new XElement("ALIGNABLE_ANNOTATION"))));

			Assert.AreEqual(6, _helper.GetTimeSlots().Count);
			_helper.RemoveTimeSlots();
			Assert.AreEqual(6, _helper.GetTimeSlots().Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesTranscriptionTierHaveDepedentTimeSubdivisionTier_NoSubdivisionTier_ReturnsFalse()
		{
			LoadEafFile();
			Assert.IsFalse(_helper.GetDoesTranscriptionTierHaveDepedentTimeSubdivisionTier());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesTranscriptionTierHaveDepedentTimeSubdivisionTier_HasSubdivisionTier_ReturnsTrue()
		{
			LoadEafFile();

			_helper.Root.Add(new XElement("TIER",
				new XAttribute("PARENT_REF", "Transcription"),
				new XAttribute("TIER_ID", "junk"),
				new XElement("ANNOTATION", new XElement("ALIGNABLE_ANNOTATION"))));

			Assert.IsTrue(_helper.GetDoesTranscriptionTierHaveDepedentTimeSubdivisionTier());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTranscriptionTierAnnotations_TranscriptionTierMissing_ReturnsEmptyList()
		{
			LoadEafFile();
			Assert.IsEmpty(_helper.GetTranscriptionTierAnnotations().ToList());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTranscriptionTierAnnotations_TranscriptionTierPresent_ReturnsSortedByAnnotationId()
		{
			LoadEafFile(false);
			var list =  _helper.GetTranscriptionTierAnnotations();
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual("a1", list.Keys.ElementAt(0));
			Assert.AreEqual("a3", list.Keys.ElementAt(1));
			Assert.AreEqual("a2", list.Keys.ElementAt(2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTranscriptionTierAnnotations_TranscriptionTierPresent_ReturnsCorrectElements()
		{
			LoadEafFile(false);
			var list = _helper.GetTranscriptionTierAnnotations();
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual("Transcription1", list["a1"].Element("ANNOTATION_VALUE").Value);
			Assert.AreEqual("Transcription2", list["a2"].Element("ANNOTATION_VALUE").Value);
			Assert.AreEqual("Transcription3", list["a3"].Element("ANNOTATION_VALUE").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetOrCreateTranscriptionTierElement_TierIsNull_CreatesElementAndReturnsIt()
		{
			LoadEafFile();
			Assert.IsEmpty(_helper.Root.Elements("TIER").ToArray());
			var element = _helper.GetOrCreateTranscriptionTierElement();
			Assert.IsNotNull(element);
			Assert.AreEqual("TIER", element.Name.LocalName);
			Assert.AreEqual("ipa-ext", element.Attribute("DEFAULT_LOCALE").Value);
			Assert.AreEqual("Transcription", element.Attribute("LINGUISTIC_TYPE_REF").Value);
			Assert.AreEqual(TextTier.ElanTranscriptionTierId, element.Attribute("TIER_ID").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetOrCreateTranscriptionTierElement_TierExists_DoesNotCreatesSecondElement()
		{
			LoadEafFile();
			Assert.IsEmpty(_helper.Root.Elements("TIER").ToArray());

			var first = _helper.GetOrCreateTranscriptionTierElement();
			Assert.AreEqual(1, _helper.Root.Elements("TIER")
				.Count(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranscriptionTierId));

			var second = _helper.GetOrCreateTranscriptionTierElement();
			Assert.AreEqual(1, _helper.Root.Elements("TIER")
				.Count(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranscriptionTierId));

			Assert.AreSame(first, second);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetOrCreateFreeTranslationTierElement_TierIsNull_CreatesElementAndReturnsIt()
		{
			LoadEafFile();
			Assert.IsEmpty(_helper.Root.Elements("TIER").ToArray());
			var element = _helper.GetOrCreateFreeTranslationTierElement();
			Assert.IsNotNull(element);
			Assert.AreEqual("TIER", element.Name.LocalName);
			Assert.AreEqual(TextTier.ElanTranscriptionTierId, element.Attribute("PARENT_REF").Value);
			Assert.AreEqual("Transcription", element.Attribute("LINGUISTIC_TYPE_REF").Value);
			Assert.AreEqual(TextTier.ElanTranslationTierId, element.Attribute("TIER_ID").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetOrCreateFreeTranslationTierElement_TierExists_DoesNotCreatesSecondElement()
		{
			LoadEafFile();
			Assert.IsEmpty(_helper.Root.Elements("TIER").ToArray());

			var first = _helper.GetOrCreateFreeTranslationTierElement();
			Assert.AreEqual(1, _helper.Root.Elements("TIER")
				.Count(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranslationTierId));

			var second = _helper.GetOrCreateFreeTranslationTierElement();
			Assert.AreEqual(1, _helper.Root.Elements("TIER")
				.Count(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranslationTierId));

			Assert.AreSame(first, second);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFreeTranslationTierAnnotations_FreeTransTierMissing_CreatesAndReturnsEmptyList()
		{
			LoadEafFile();
			var annotations = _helper.GetFreeTranslationTierAnnotations();
			Assert.AreEqual(0, annotations.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFreeTranslationTierAnnotations_AnnotationsExist_ReturnsThemWithParentRefs()
		{
			LoadEafFile(false);

			var annotations = _helper.GetFreeTranslationTierAnnotations();
			Assert.AreEqual(2, annotations.Count);
			Assert.AreEqual("a1", annotations.Keys.ElementAt(0));
			Assert.AreEqual("a2", annotations.Keys.ElementAt(1));
			Assert.AreEqual("FreeTranslation1", annotations.Values.ElementAt(0));
			Assert.AreEqual("FreeTranslation2", annotations.Values.ElementAt(1));
		}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetDependentTierAnnotationElements_DependentTierPresent_ReturnsCorrectElements()
		//{
		//    LoadEafFile(false);
		//    var dependentTiers = _helper.GetDependentTiersElements();
		//    var list = _helper.GetDependentTierAnnotationElements(dependentTiers.ElementAt(0));
		//    Assert.AreEqual(2, list.Count);
		//    Assert.AreEqual("a4", list.Values.ElementAt(0).Attribute("ANNOTATION_ID").Value);
		//    Assert.AreEqual("a5", list.Values.ElementAt(1).Attribute("ANNOTATION_ID").Value);
		//    Assert.AreEqual("FreeTranslation1", list.Values.ElementAt(0).Element("ANNOTATION_VALUE").Value);
		//    Assert.AreEqual("FreeTranslation2", list.Values.ElementAt(1).Element("ANNOTATION_VALUE").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetDependentTiersElements_NoDependentTiersExist_AddsEmptyFreeTranslationTier()
		//{
		//    LoadEafFile();
		//    var tierElements = _helper.GetDependentTiersElements().ToList();
		//    Assert.AreEqual(1, tierElements.Count);
		//    Assert.AreEqual(TextTier.ElanTranscriptionTierId, tierElements[0].Attribute("PARENT_REF").Value);
		//    Assert.AreEqual(TextTier.ElanTranslationTierId, tierElements[0].Attribute("TIER_ID").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetDependentTiersElements_DependentTiersExist_ReturnsThem()
		//{
		//    LoadEafFile(false);
		//    var list = _helper.GetDependentTiersElements().ToList();
		//    Assert.AreEqual(2, list.Count);
		//    Assert.AreEqual(TextTier.ElanTranslationTierId, list[0].Attribute("TIER_ID").Value);
		//    Assert.AreEqual("User Defined Tier", list[1].Attribute("TIER_ID").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateDependentSayMoreTiers_NoTranscriptionAnnotationIds_ReturnsEmptyList()
		//{
		//    LoadEafFile(false);
		//    Assert.IsEmpty(_helper.CreateDependentSayMoreTiers(new string[] { }).ToList());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateDependentSayMoreTiers_MoreTranscriptionAnnotationsThanDependentAnnotations_ReturnsTierWithCorrectAnnotationCount()
		//{
		//    LoadEafFile(false);
		//    Assert.AreEqual(3, _helper.CreateDependentSayMoreTiers(
		//        new[] { "a1", "a2", "a3" }).ElementAt(0).Segments.Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateDependentSayMoreTiers_OneDependentTeir_ReturnsTextTierWithCorrectSegmentTexts()
		//{
		//    LoadEafFile(false);
		//    var textTier = _helper.CreateDependentSayMoreTiers(new[] { "a1", "a2", "a3" }).ElementAt(0);
		//    Assert.AreEqual("FreeTranslation1", textTier.Segments.ElementAt(0).Text);
		//    Assert.AreEqual("FreeTranslation2", textTier.Segments.ElementAt(1).Text);
		//    Assert.IsEmpty(textTier.Segments.ElementAt(2).Text);
		//}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetOrCreateHeader_HeaderMissing_ReturnsHeader()
		{
			LoadEafFile();
			var element = _helper.GetOrCreateHeader();
			Assert.AreEqual("HEADER", element.Name.LocalName);
			Assert.IsNotNull(element.Element("MEDIA_DESCRIPTOR"));
			Assert.IsNotNull(element.Attribute("MEDIA_FILE"));
			Assert.IsNotNull(element.Attribute("TIME_UNITS"));
			Assert.AreEqual(string.Empty, element.Attribute("MEDIA_FILE").Value);
			Assert.AreEqual("milliseconds", element.Attribute("TIME_UNITS").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetOrCreateHeader_HeaderPresent_ReturnsHeaderButDoesNotCreate()
		{
			_root.Add(_header);
			LoadEafFile(true, false);
			var element = _helper.GetOrCreateHeader();
			Assert.AreEqual("HEADER", element.Name.LocalName);
			Assert.IsNull(element.Element("MEDIA_DESCRIPTOR"));
			Assert.IsNull(element.Attribute("MEDIA_FILE"));
			Assert.IsNull(element.Attribute("TIME_UNITS"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMediaDescriptorElement_NullMediaFile_ReturnsBasicElement()
		{
			_root.Add(_header);
			LoadEafFile(true, false);
			var element = _helper.CreateMediaDescriptorElement();
			Assert.AreEqual("MEDIA_DESCRIPTOR", element.Name.LocalName);
			Assert.IsNull(element.Attribute("MEDIA_URL"));
			Assert.IsNull(element.Attribute("MIME_TYPE"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMediaDescriptorElement_ValidMediaFile_ReturnsCorrectElementContent()
		{
			var element = new AnnotationFileHelper(null, @"c:\My\Folk\Music\Alathea.wav").CreateMediaDescriptorElement();
			Assert.AreEqual("MEDIA_DESCRIPTOR", element.Name.LocalName);
			Assert.AreEqual("Alathea.wav", element.Attribute("MEDIA_URL").Value);
			Assert.AreEqual("audio/x-wav", element.Attribute("MIME_TYPE").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMediaFileMimeType_WaveFile_ReturnsProperMimeType()
		{
			Assert.AreEqual("audio/x-wav", new AnnotationFileHelper(null, "Alathea.wav").CreateMediaFileMimeType());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMediaFileMimeType_NonWaveAudioFile_ReturnsProperMimeType()
		{
			Assert.AreEqual("audio/*", new AnnotationFileHelper(null, "Alathea.mp3").CreateMediaFileMimeType());
			Assert.AreEqual("audio/*", new AnnotationFileHelper(null, "Alathea.wma").CreateMediaFileMimeType());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMediaFileMimeType_MpgFile_ReturnsProperMimeType()
		{
			Assert.AreEqual("video/mpeg", new AnnotationFileHelper(null, "Alathea.mpg").CreateMediaFileMimeType());
			Assert.AreEqual("video/mpeg", new AnnotationFileHelper(null, "Alathea.mpeg").CreateMediaFileMimeType());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMediaFileMimeType_NonMpgVideoFile_ReturnsProperMimeType()
		{
			Assert.AreEqual("video/*", new AnnotationFileHelper(null, "Alathea.wmv").CreateMediaFileMimeType());
			Assert.AreEqual("video/*", new AnnotationFileHelper(null, "Alathea.mov").CreateMediaFileMimeType());
			Assert.AreEqual("video/*", new AnnotationFileHelper(null, "Alathea.avi").CreateMediaFileMimeType());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveAnnotationsFromTier_TierDoesNotExist_DoesNothing()
		{
			LoadEafFile(false);

			foreach (var e in _helper.Root.Elements("TIER"))
				Assert.Greater(e.Elements().Count(), 0);

			_helper.RemoveAnnotationsFromTier("blahblah");

			foreach (var e in _helper.Root.Elements("TIER"))
				Assert.Greater(e.Elements().Count(), 0);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveAnnotationsFromTier_PassIdOfTranscriptionTier_RemovesItsAnnotations()
		{
			LoadEafFile(false);

			foreach (var e in _helper.Root.Elements("TIER"))
				Assert.Greater(e.Elements().Count(), 0);

			_helper.RemoveAnnotationsFromTier(TextTier.ElanTranscriptionTierId);

			Assert.IsFalse(_helper.Root.Elements("TIER")
				.First(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranscriptionTierId).HasElements);

			Assert.IsTrue(_helper.Root.Elements("TIER")
				.First(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranslationTierId).HasElements);

			Assert.IsTrue(_helper.Root.Elements("TIER")
				.First(e => e.Attribute("TIER_ID").Value == "User Defined Tier").HasElements);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveAnnotationsFromTier_PassIdOfFreeTranslationTier_RemovesItsAnnotations()
		{
			LoadEafFile(false);

			foreach (var e in _helper.Root.Elements("TIER"))
				Assert.Greater(e.Elements().Count(), 0);

			_helper.RemoveAnnotationsFromTier(TextTier.ElanTranslationTierId);

			Assert.IsTrue(_helper.Root.Elements("TIER")
				.First(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranscriptionTierId).HasElements);

			Assert.IsFalse(_helper.Root.Elements("TIER")
				.First(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranslationTierId).HasElements);

			Assert.IsTrue(_helper.Root.Elements("TIER")
				.First(e => e.Attribute("TIER_ID").Value == "User Defined Tier").HasElements);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveAnnotationsFromTier_PassIdOfUserDefinedTier_RemovesItsAnnotations()
		{
			LoadEafFile(false);

			foreach (var e in _helper.Root.Elements("TIER"))
				Assert.Greater(e.Elements().Count(), 0);

			_helper.RemoveAnnotationsFromTier("User Defined Tier");

			Assert.IsTrue(_helper.Root.Elements("TIER")
				.First(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranscriptionTierId).HasElements);

			Assert.IsTrue(_helper.Root.Elements("TIER")
				.First(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranslationTierId).HasElements);

			Assert.IsFalse(_helper.Root.Elements("TIER")
				.First(e => e.Attribute("TIER_ID").Value == "User Defined Tier").HasElements);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTranscriptionTierIds_NoTranscriptionAnnotations_ReturnsEmptyList()
		{
			LoadEafFile(false);
			_helper.RemoveAnnotationsFromTier(TextTier.ElanTranscriptionTierId);
			Assert.IsEmpty(_helper.GetTranscriptionTierAnnotationIds().ToArray());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTranscriptionTierIds_HasTranscriptionAnnotations_ReturnsIds()
		{
			LoadEafFile(false);
			var list = _helper.GetTranscriptionTierAnnotationIds().ToArray();
			Assert.AreEqual("a1", list[0]);
			Assert.AreEqual("a3", list[1]);
			Assert.AreEqual("a2", list[2]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetLastUsedAnnotationId_IdTooSmall_ThrowsException()
		{
			LoadEafFile();
			Assert.Throws<ArgumentOutOfRangeException>(() => _helper.SetLastUsedAnnotationId(-1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetLastUsedAnnotationId_NoHeaderExists_CreatesHeaderAndSetsIt()
		{
			LoadEafFile();
			_helper.SetLastUsedAnnotationId(4);
			Assert.AreEqual("a5", _helper.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetLastUsedAnnotationId_HeaderExists_SetsIt()
		{
			LoadEafFile();
			_helper.GetNextAvailableAnnotationIdAndIncrement();
			_helper.SetLastUsedAnnotationId(9);
			Assert.AreEqual("a10", _helper.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CorrectLastUsedAnnotationIdIfNecessary_NoNeedForCorrection_DoesNothing()
		{
			LoadEafFile(false);

			int count = _helper.Root.Elements("TIER").Sum(e => e.Elements().Count());
			_helper.SetLastUsedAnnotationId(count);
			_helper.CorrectLastUsedAnnotationIdIfNecessary();
			Assert.AreEqual(string.Format("a{0}", count + 1), _helper.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CorrectLastUsedAnnotationIdIfNecessary_CorrectionWhenValueIsWrong_MakesCorrection()
		{
			LoadEafFile(false);

			int count = _helper.Root.Elements("TIER").Sum(e => e.Elements().Count());
			_helper.SetLastUsedAnnotationId(count + 5);
			_helper.CorrectLastUsedAnnotationIdIfNecessary();
			Assert.AreEqual(string.Format("a{0}", count + 1), _helper.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CorrectLastUsedAnnotationIdIfNecessary_CorrectionAfterRemovingAnnotations_MakesCorrection()
		{
			LoadEafFile(false);

			Assert.AreEqual(7, _helper.Root.Elements("TIER").Sum(e => e.Elements().Count()));
			Assert.AreEqual("a8", _helper.GetNextAvailableAnnotationIdAndIncrement());

			_helper.RemoveAnnotationsFromTier("User Defined Tier");
			Assert.AreEqual(5, _helper.Root.Elements("TIER").Sum(e => e.Elements().Count()));
			Assert.AreEqual("a9", _helper.GetNextAvailableAnnotationIdAndIncrement());

			_helper.CorrectLastUsedAnnotationIdIfNecessary();

			Assert.AreEqual("a6", _helper.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetOrCreateTranscriptionTierElement_ElementDoesNotExist_CreatesIt()
		{
			LoadEafFile();
			_helper.RemoveAnnotationsFromTier(TextTier.ElanTranscriptionTierId);

			Assert.IsNull(_helper.Root.Elements("TIER")
				.FirstOrDefault(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranscriptionTierId));

			Assert.IsNotNull(_helper.GetOrCreateTranscriptionTierElement());

			Assert.IsNotNull(_helper.Root.Elements("TIER")
				.FirstOrDefault(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranscriptionTierId));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetOrCreateTranscriptionTierElement_ElementExists_DoesNotCreateIt()
		{
			LoadEafFile();

			_helper.RemoveAnnotationsFromTier(TextTier.ElanTranscriptionTierId);
			_helper.GetOrCreateTranscriptionTierElement();

			Assert.AreEqual(1, _helper.Root.Elements("TIER")
				.Count(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranscriptionTierId));

			_helper.GetOrCreateTranscriptionTierElement();

			Assert.AreEqual(1, _helper.Root.Elements("TIER")
				.Count(e => e.Attribute("TIER_ID").Value == TextTier.ElanTranscriptionTierId));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateTranscriptionAnnotationElement_CreatesTimeSlotAndAnnotationElements()
		{
			LoadEafFile(false);

			_helper.RemoveTimeSlots();
			_helper.RemoveAnnotationsFromTier(TextTier.ElanTranscriptionTierId);

			var elements = _helper.GetTranscriptionTierAnnotations().Select(kvp => kvp.Value).ToArray();
			Assert.AreEqual(0, elements.Length);
			Assert.AreEqual(0, _helper.GetTimeSlots().Count);

			_helper.CreateTranscriptionAnnotationElement(new Segment(null, 3f, 5.4f));

			elements = _helper.GetTranscriptionTierAnnotations().Select(kvp => kvp.Value).ToArray();
			Assert.AreEqual(1, elements.Length);

			var timeSlots = _helper.GetTimeSlots();
			Assert.AreEqual(2, timeSlots.Count);
			Assert.AreEqual("ts1", timeSlots.Keys.ElementAt(0));
			Assert.AreEqual("ts2", timeSlots.Keys.ElementAt(1));
			Assert.AreEqual(3f, timeSlots["ts1"]);
			Assert.AreEqual(5.4f, timeSlots["ts2"]);
		}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void SaveTranscriptionValue_PassNull_SetsValueToEmptyString()
		//{
		//    LoadEafFile(false);
		//    var elements = _helper.GetTranscriptionTierAnnotations().Select(kvp => kvp.Value).ToArray();
		//    Assert.AreEqual("Transcription3", elements[1].Element("ANNOTATION_VALUE").Value);
		//    _helper.SaveTranscriptionValue("a3", null);
		//    Assert.AreEqual(string.Empty, elements[1].Element("ANNOTATION_VALUE").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void SaveTranscriptionValue_PassValue_SetsValue()
		//{
		//    LoadEafFile(false);
		//    var elements = _helper.GetTranscriptionTierAnnotations().Select(kvp => kvp.Value).ToArray();
		//    Assert.AreEqual("Transcription3", elements[1].Element("ANNOTATION_VALUE").Value);
		//    _helper.SaveTranscriptionValue("a3", "sadie the dog");
		//    Assert.AreEqual("sadie the dog", elements[1].Element("ANNOTATION_VALUE").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void SaveDependentAnnotationValue_PassNull_SetsValueToEmptyString()
		//{
		//    LoadEafFile(false);

		//    _helper.RemoveAnnotationsFromTier(TextTier.ElanTranslationTierId);

		//    _helper.SaveFreeTranslationAnnotationValue(TextTier.ElanTranscriptionTierId,
		//        TextTier.ElanTranslationTierId, null);

		//    var dependents = _helper.GetDependentTiersElements().ToArray();
		//    var elements = _helper.GetDependentTierAnnotationElements(dependents[0]);
		//    Assert.AreEqual(string.Empty, elements.Values.ElementAt(0).Element("ANNOTATION_VALUE").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void SaveDependentAnnotationValue_PassValue_SetsValue()
		//{
		//    LoadEafFile(false);

		//    _helper.RemoveAnnotationsFromTier(TextTier.ElanTranslationTierId);

		//    _helper.SaveFreeTranslationAnnotationValue(TextTier.ElanTranscriptionTierId,
		//        TextTier.ElanTranslationTierId, "Bear the cat");

		//    var dependents = _helper.GetDependentTiersElements().ToArray();
		//    var elements = _helper.GetDependentTierAnnotationElements(dependents[0]);
		//    Assert.AreEqual("Bear the cat", elements.Values.ElementAt(0).Element("ANNOTATION_VALUE").Value);
		//}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetOrCreateTimeOrderElementAndReturnId_WhenTimeSlotsAlreadyExist_AddsAndReturnsId()
		{
			LoadEafFile(false);

			Assert.AreEqual(6, _helper.GetTimeSlots().Count);
			_helper.GetOrCreateTimeOrderElementAndReturnId(33.5f);
			var timeSlots = _helper.GetTimeSlots();
			Assert.AreEqual(7, timeSlots.Count);
			Assert.AreEqual(33.5, timeSlots["ts7"]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetOrCreateTimeOrderElementAndReturnId_WhenTimeSlotsDoNotExist_AddsAndReturnsId()
		{
			LoadEafFile(false);

			Assert.AreEqual(6, _helper.GetTimeSlots().Count);
			_helper.RemoveTimeSlots();
			Assert.AreEqual(0, _helper.GetTimeSlots().Count);

			_helper.GetOrCreateTimeOrderElementAndReturnId(33.5f);
			var timeSlots = _helper.GetTimeSlots();
			Assert.AreEqual(1, timeSlots.Count);
			Assert.AreEqual(33.5, timeSlots["ts1"]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTierCollection_ReturnsCorrectTiers()
		{
			LoadEafFile(false);

			var collection = _helper.GetTierCollection();
			Assert.AreEqual(3, collection.Count);

			Assert.IsInstanceOf<TimeTier>(collection[0]);
			Assert.IsInstanceOf<TextTier>(collection[1]);
			Assert.IsInstanceOf<TextTier>(collection[2]);

			Assert.AreEqual(TierType.Time, collection[0].TierType);
			Assert.AreEqual(TierType.Transcription, collection[1].TierType);
			Assert.AreEqual(TierType.FreeTranslation, collection[2].TierType);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTierCollection_ReturnsSameNumberSegmentsInEachTier()
		{
			LoadEafFile(false);

			var collection = _helper.GetTierCollection();

			Assert.AreEqual(3, collection[0].Segments.Count());
			Assert.AreEqual(3, collection[1].Segments.Count());
			Assert.AreEqual(3, collection[2].Segments.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTierCollection_ReturnsCorrectSegmentTimes()
		{
			LoadEafFile(false);

			var collection = _helper.GetTierCollection();
			var segs = collection[0].Segments.ToArray();

			Assert.AreEqual(0f, segs[0].Start);
			Assert.AreEqual(.75f, segs[0].End);
			Assert.AreEqual(.75f, segs[1].Start);
			Assert.AreEqual(1.25f, segs[1].End);
			Assert.AreEqual(1.25f, segs[2].Start);
			Assert.AreEqual(2.121f, segs[2].End);

			Assert.AreEqual(collection[0], segs[0].Tier);
			Assert.AreEqual(collection[0], segs[1].Tier);
			Assert.AreEqual(collection[0], segs[2].Tier);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTierCollection_ReturnsCorrectTranscriptionSegments()
		{
			LoadEafFile(false);

			var collection = _helper.GetTierCollection();
			var segs = collection[1].Segments.ToArray();

			Assert.AreEqual("Transcription1", segs[0].Text);
			Assert.AreEqual("Transcription3", segs[1].Text);
			Assert.AreEqual("Transcription2", segs[2].Text);

			Assert.AreEqual(collection[1], segs[0].Tier);
			Assert.AreEqual(collection[1], segs[1].Tier);
			Assert.AreEqual(collection[1], segs[2].Tier);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTierCollection_ReturnsCorrectFreeTranslationSegments()
		{
			LoadEafFile(false);

			var collection = _helper.GetTierCollection();
			var segs = collection[2].Segments.ToArray();

			Assert.AreEqual("FreeTranslation1", segs[0].Text);
			Assert.AreEqual(string.Empty, segs[1].Text);
			Assert.AreEqual("FreeTranslation2", segs[2].Text);

			Assert.AreEqual(collection[2], segs[0].Tier);
			Assert.AreEqual(collection[2], segs[1].Tier);
			Assert.AreEqual(collection[2], segs[2].Tier);
		}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetTierCollection_ReturnsCorrectUserDefSegments()
		//{
		//    LoadEafFile(false);

		//    var collection = _helper.GetTierCollection();
		//    var segs = collection[3].Segments.ToArray();

		//    Assert.AreEqual("UserAnnotationValue1", segs[0].Text);
		//    Assert.AreEqual("UserAnnotationValue2", segs[1].Text);
		//    Assert.AreEqual(string.Empty, segs[2].Text);

		//    Assert.AreEqual(collection[3], segs[0].Tier);
		//    Assert.AreEqual(collection[3], segs[1].Tier);
		//    Assert.AreEqual(collection[3], segs[2].Tier);
		//}
	}
}
