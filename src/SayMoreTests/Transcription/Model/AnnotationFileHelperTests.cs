using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Transcription.Model;

namespace SayMoreTests.Transcription.Model
{
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
		private XElement _lastIdElement;
		private XAttribute _lastIdAttribute;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_folder = new TemporaryFolder("AnnotationFileHelperTests");
			_basicEafFileName = _folder.Combine("basic.eaf");
			_root = new XElement("ANNOTATION_DOCUMENT");
			_header = new XElement("HEADER");
			_mediaDescriptor = new XElement("MEDIA_DESCRIPTOR");
			_mediaUrl = new XAttribute("MEDIA_URL", "UninspiredMediaFileName.wav");
			_lastIdElement = new XElement("PROPERTY");
			_lastIdAttribute = new XAttribute("NAME", "lastUsedAnnotationId");
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_folder.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadEafFile()
		{
			LoadEafFile(true);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadEafFile(bool loadBasicEafFile)
		{
			if (!loadBasicEafFile)
				_helper = AnnotationFileHelper.Load(CreateTestEaf());
			else
			{
				_root.Save(_basicEafFileName);
				_helper = AnnotationFileHelper.Load(_basicEafFileName);
			}

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
			LoadEafFile();
			Assert.IsNull(_helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToMediaFile_NoMediaDescriptorElement_ReturnsNull()
		{
			_root.Add(_header);
			LoadEafFile();
			Assert.IsNull(_helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToMediaFile_NoMediaUrlAttribute_ReturnsNull()
		{
			_header.Add(_mediaDescriptor);
			_root.Add(_header);
			LoadEafFile();
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
			Assert.AreEqual(Path.Combine(_helper.GetAnnotationFolderPath(), "UninspiredMediaFileName.wav"), _helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetMediaFile_HeaderMissing_SetsMediaFileName()
		{
			LoadEafFile();
			Assert.IsNull(_helper.GetFullPathToMediaFile());
			_helper.SetMediaFile("BeaversAndDucks.mp3");
			Assert.AreEqual(Path.Combine(_helper.GetAnnotationFolderPath(), "BeaversAndDucks.mp3"), _helper.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetMediaFile_MediaDescriptorMissing_SetsMediaFileName()
		{
			_root.Add(_header);
			LoadEafFile();
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
			LoadEafFile();
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
			LoadEafFile();
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
			_header.Add(_lastIdElement);
			_root.Add(_header);
			LoadEafFile();
			Assert.AreEqual("a1", _helper.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextAvailableAnnotationIdAndIncrement_AllElementsAndAttributesPresent_ReturnsCorrectValue()
		{
			_lastIdElement.SetValue(5);
			_lastIdElement.Add(_lastIdAttribute);
			_header.Add(_lastIdElement);
			_root.Add(_header);
			LoadEafFile();
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
			Assert.AreEqual(4, list.Count);
			Assert.AreEqual(0.75f, list["ts1"]);
			Assert.AreEqual(1.25f, list["ts2"]);
			Assert.AreEqual(2.121f, list["ts3"]);
			Assert.AreEqual(3.131f, list["ts4"]);
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
		public void GetDependentTierAnnotationElements_DependentTierIsNull_ReturnsEmptyList()
		{
			LoadEafFile();
			Assert.IsEmpty(_helper.GetDependentTierAnnotationElements(null).ToList());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDependentTierAnnotationElements_DependentTierPresent_ReturnsSortedByTranscriptionAnnotationId()
		{
			LoadEafFile(false);
			var dependentTiers = _helper.GetDependentTiersElements();
			var list = _helper.GetDependentTierAnnotationElements(dependentTiers.ElementAt(0));
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("a1", list.Keys.ElementAt(0));
			Assert.AreEqual("a2", list.Keys.ElementAt(1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDependentTierAnnotationElements_DependentTierPresent_ReturnsCorrectElements()
		{
			LoadEafFile(false);
			var dependentTiers = _helper.GetDependentTiersElements();
			var list = _helper.GetDependentTierAnnotationElements(dependentTiers.ElementAt(0));
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("a4", list.Values.ElementAt(0).Attribute("ANNOTATION_ID").Value);
			Assert.AreEqual("a5", list.Values.ElementAt(1).Attribute("ANNOTATION_ID").Value);
			Assert.AreEqual("FreeTranslation1", list.Values.ElementAt(0).Element("ANNOTATION_VALUE").Value);
			Assert.AreEqual("FreeTranslation2", list.Values.ElementAt(1).Element("ANNOTATION_VALUE").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDependentTiersElements_NoDependentTiersExist_AddsEmptyFreeTranslationTier()
		{
			LoadEafFile();
			var tierElements = _helper.GetDependentTiersElements().ToList();
			Assert.AreEqual(1, tierElements.Count);
			Assert.AreEqual("Transcription", tierElements[0].Attribute("PARENT_REF").Value);
			Assert.AreEqual("Phrase Free Translation", tierElements[0].Attribute("TIER_ID").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDependentTiersElements_DependentTiersExist_ReturnsThem()
		{
			LoadEafFile(false);
			var list = _helper.GetDependentTiersElements().ToList();
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("Phrase Free Translation", list[0].Attribute("TIER_ID").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateDependentTextTiers_NoTranscriptionAnnotationIds_ReturnsEmptyList()
		{
			LoadEafFile(false);
			Assert.IsEmpty(_helper.CreateDependentTextTiers(new string[] { }).ToList());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateDependentTextTiers_MoreTranscriptionAnnotationsThanDependentAnnotations_ReturnsTierWithCorrectAnnotationCount()
		{
			LoadEafFile(false);
			Assert.AreEqual(3, _helper.CreateDependentTextTiers(
				new[] { "a1", "a2", "a3" }).ElementAt(0).GetAllSegments().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateDependentTextTiers_OneDependentTeir_ReturnsTextTierWithCorrectSegmentTexts()
		{
			LoadEafFile(false);
			var textTier = _helper.CreateDependentTextTiers(new[] { "a1", "a2", "a3" }).ElementAt(0);
			Assert.AreEqual("FreeTranslation1", ((ITextSegment)textTier.GetSegment(0)).GetText());
			Assert.AreEqual("FreeTranslation2", ((ITextSegment)textTier.GetSegment(1)).GetText());
			Assert.IsEmpty(((ITextSegment)textTier.GetSegment(2)).GetText());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateDependentTextTiers_OneDependentTeir_ReturnsTextTierWithCorrectAnnotationIds()
		{
			LoadEafFile(false);
			var textTier = _helper.CreateDependentTextTiers(new[] { "a1", "a2", "a3" }).ElementAt(0);
			Assert.AreEqual("a4", ((ITextSegment)textTier.GetSegment(0)).Id);
			Assert.AreEqual("a5", ((ITextSegment)textTier.GetSegment(1)).Id);
			Assert.IsNull(((ITextSegment)textTier.GetSegment(2)).Id);
		}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetDependentTierAnnotations_DependentTierPresent_ReturnsCorrectElements()
		//{
		//    LoadEafFile(false);
		//    var list = _eafFile.GetDependentTierAnnotations();
		//    Assert.AreEqual(2, list.Count);
		//    Assert.AreEqual("Transcription1", list["a1"].Value);
		//    Assert.AreEqual("Transcription2", list["a2"].Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetTimeSlotCollection_HasMediaTierWithNoSegments_ReturnsEmptyCollection()
		//{
		//    _mediaTier = new AudioTier("teetering tier", "Fleet Foxes.mp3");
		//    Assert.AreEqual(0, _eafFile.GetTimeSlotCollection(_mediaTier).Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetTimeSlotCollection_HasMediaTierWithSegments_ReturnsCorrectCount()
		//{
		//    Assert.AreEqual(4, _eafFile.GetTimeSlotCollection(_mediaTier).Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetTimeSlotCollection_NoDuplicateTimes_ReturnsCorrectCollection()
		//{
		//    Assert.AreEqual(2f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(0));
		//    Assert.AreEqual(6f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(1));
		//    Assert.AreEqual(10f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(2));
		//    Assert.AreEqual(18f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(3));
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetTimeSlotCollection_DuplicateTimes_ReturnsCorrectCollection()
		//{
		//    _mediaTier = new AudioTier("teetering tier", null);
		//    _mediaTier.AddSegment(2f, 4f);
		//    _mediaTier.AddSegment(6f, 8f);

		//    Assert.AreEqual(3, _eafFile.GetTimeSlotCollection(_mediaTier).Count());
		//    Assert.AreEqual(2f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(0));
		//    Assert.AreEqual(6f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(1));
		//    Assert.AreEqual(14f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(2));
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetTimeSlotCollection_OutOfOrderAndOverlappingSegments_ReturnsSortedCollection()
		//{
		//    _mediaTier = new AudioTier("teetering tier", null);
		//    _mediaTier.AddSegment(100f, 50f);
		//    _mediaTier.AddSegment(70f, 40f);

		//    Assert.AreEqual(70f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(0));
		//    Assert.AreEqual(100f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(1));
		//    Assert.AreEqual(110f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(2));
		//    Assert.AreEqual(150f, _eafFile.GetTimeSlotCollection(_mediaTier).ElementAt(3));
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
			LoadEafFile();
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
			LoadEafFile();
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
	}
}
