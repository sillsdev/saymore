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
	public class EafFileTests
	{
		private EafFile _eafFile;
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
			_folder = new TemporaryFolder("EafFileTests");

			_basicEafFileName = _folder.Combine("basic.eaf");

			_root = new XElement("ANNOTATION_DOCUMENT");
			_header = new XElement("HEADER");
			_mediaDescriptor = new XElement("MEDIA_DESCRIPTOR");
			_mediaUrl = new XAttribute("MEDIA_URL", "UninspiredMediaFileName.wav");
			_lastIdElement = new XElement("PROPERTY");
			_lastIdAttribute = new XAttribute("NAME", "lastUsedAnnotationId");


			//_mediaTier = new AudioTier("teetering tier", "Fleet Foxes.mp3");
			//_mediaTier.AddSegment(2f, 4f);
			//_mediaTier.AddSegment(10f, 8f);

			//_textTier = new TextTier("lead");
			//_textTier.AddSegment("brass");
			//_textTier.AddSegment("steel");
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
				_eafFile = EafFile.Load(CreateTestEaf());
			else
			{
				_root.Save(_basicEafFileName);
				_eafFile = EafFile.Load(_basicEafFileName);
			}

			Assert.IsNotNull(_eafFile);
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
			Assert.IsFalse(EafFile.GetIsElanFile(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsElanFile_FileNameEmpty_ReturnsFalse()
		{
			Assert.IsFalse(EafFile.GetIsElanFile(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsElanFile_InvalidXmlFile_ReturnsFalse()
		{
			var filename = _folder.Combine("bad.xml");
			File.CreateText(filename).Close();
			Assert.IsFalse(EafFile.GetIsElanFile(filename));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsElanFile_ValidXmlButNotEafFile_ReturnsFalse()
		{
			var filename = _folder.Combine("goodBadEaf.xml");
			var element = new XElement("root", "blah blah");
			element.Save(filename);
			Assert.IsFalse(EafFile.GetIsElanFile(filename));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsElanFile_ValidEafFile_ReturnsTrue()
		{
			Assert.IsTrue(EafFile.GetIsElanFile(CreateTestEaf()));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToMediaFile_NoHeaderElement_ReturnsNull()
		{
			LoadEafFile();
			Assert.IsNull(_eafFile.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToMediaFile_NoMediaDescriptorElement_ReturnsNull()
		{
			_root.Add(_header);
			LoadEafFile();
			Assert.IsNull(_eafFile.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToMediaFile_NoMediaUrlAttribute_ReturnsNull()
		{
			_header.Add(_mediaDescriptor);
			_root.Add(_header);
			LoadEafFile();
			Assert.IsNull(_eafFile.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToMediaFile_AllElementsAndAttributesPresent_ReturnsMediaFileName()
		{
			_mediaDescriptor.Add(_mediaUrl);
			_header.Add(_mediaDescriptor);
			_root.Add(_header);
			LoadEafFile();
			Assert.AreEqual(Path.Combine(_eafFile.GetAnnotationFolderPath(), "UninspiredMediaFileName.wav"), _eafFile.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetMediaFile_HeaderMissing_SetsMediaFileName()
		{
			LoadEafFile();
			Assert.IsNull(_eafFile.GetFullPathToMediaFile());
			_eafFile.SetMediaFile("BeaversAndDucks.mp3");
			Assert.AreEqual(Path.Combine(_eafFile.GetAnnotationFolderPath(), "BeaversAndDucks.mp3"), _eafFile.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetMediaFile_MediaDescriptorMissing_SetsMediaFileName()
		{
			_root.Add(_header);
			LoadEafFile();
			Assert.IsNull(_eafFile.GetFullPathToMediaFile());
			_eafFile.SetMediaFile("BeaversAndDucks.mp3");
			Assert.AreEqual(Path.Combine(_eafFile.GetAnnotationFolderPath(), "BeaversAndDucks.mp3"), _eafFile.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetMediaFile_MediaUrlMissing_SetsMediaFileName()
		{
			_header.Add(_mediaDescriptor);
			_root.Add(_header);
			LoadEafFile();
			Assert.IsNull(_eafFile.GetFullPathToMediaFile());
			_eafFile.SetMediaFile("BeaversAndDucks.mp3");
			Assert.AreEqual(Path.Combine(_eafFile.GetAnnotationFolderPath(), "BeaversAndDucks.mp3"), _eafFile.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetMediaFile_AllElementsAndAttributesPresent_SetsMediaFileName()
		{
			_mediaDescriptor.Add(_mediaUrl);
			_header.Add(_mediaDescriptor);
			_root.Add(_header);
			LoadEafFile();
			Assert.AreEqual(Path.Combine(_eafFile.GetAnnotationFolderPath(), "UninspiredMediaFileName.wav"), _eafFile.GetFullPathToMediaFile());
			_eafFile.SetMediaFile("BeaversAndDucks.mp3");
			Assert.AreEqual(Path.Combine(_eafFile.GetAnnotationFolderPath(), "BeaversAndDucks.mp3"), _eafFile.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeMediaFileName_ChangesMediaFileName()
		{
			var testEafFile = CreateTestEaf();
			_eafFile = EafFile.Load(testEafFile);
			Assert.AreEqual(Path.Combine(_eafFile.GetAnnotationFolderPath(), "AmazingGrace.wav"), _eafFile.GetFullPathToMediaFile());

			EafFile.ChangeMediaFileName(testEafFile, "PiratesAndDawgs.mpg");
			_eafFile = EafFile.Load(testEafFile);
			Assert.AreEqual(Path.Combine(_eafFile.GetAnnotationFolderPath(), "PiratesAndDawgs.mpg"), _eafFile.GetFullPathToMediaFile());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextAvailableAnnotationIdAndIncrement_HeaderMissing_ReturnsOne()
		{
			LoadEafFile();
			Assert.AreEqual("a1", _eafFile.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextAvailableAnnotationIdAndIncrement_PropertyMissing_ReturnsOne()
		{
			_root.Add(_header);
			LoadEafFile();
			Assert.AreEqual("a1", _eafFile.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextAvailableAnnotationIdAndIncrement_LastIdAttributeMissing_ReturnsOne()
		{
			_header.Add(_lastIdElement);
			_root.Add(_header);
			LoadEafFile();
			Assert.AreEqual("a1", _eafFile.GetNextAvailableAnnotationIdAndIncrement());
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
			Assert.AreEqual("a6", _eafFile.GetNextAvailableAnnotationIdAndIncrement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlots_MissingTimeOrderElement_ReturnsEmptyList()
		{
			LoadEafFile();
			Assert.IsEmpty(_eafFile.GetTimeSlots().ToList());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlots_EmptyTimeOrderElement_ReturnsEmptyList()
		{
			_root.Add(new XElement("TIME_ORDER"));
			LoadEafFile();
			Assert.IsEmpty(_eafFile.GetTimeSlots().ToList());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeSlots_TimeSlotsExist_ReturnsList()
		{
			LoadEafFile(false);

			var list = _eafFile.GetTimeSlots();
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
			Assert.IsEmpty(_eafFile.GetTranscriptionTierAnnotations().ToList());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTranscriptionTierAnnotations_TranscriptionTierPresent_ReturnsSortedByAnnotationId()
		{
			LoadEafFile(false);
			var list =  _eafFile.GetTranscriptionTierAnnotations();
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
			var list = _eafFile.GetTranscriptionTierAnnotations();
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
			Assert.IsEmpty(_eafFile.GetDependentTierAnnotationElements(null).ToList());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDependentTierAnnotationElements_DependentTierPresent_ReturnsSortedByTranscriptionAnnotationId()
		{
			LoadEafFile(false);
			var dependentTiers = _eafFile.GetDependentTiersElements();
			var list = _eafFile.GetDependentTierAnnotationElements(dependentTiers.ElementAt(0));
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("a1", list.Keys.ElementAt(0));
			Assert.AreEqual("a2", list.Keys.ElementAt(1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDependentTierAnnotationElements_DependentTierPresent_ReturnsCorrectElements()
		{
			LoadEafFile(false);
			var dependentTiers = _eafFile.GetDependentTiersElements();
			var list = _eafFile.GetDependentTierAnnotationElements(dependentTiers.ElementAt(0));
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("a4", list.Values.ElementAt(0).Attribute("ANNOTATION_ID").Value);
			Assert.AreEqual("a5", list.Values.ElementAt(1).Attribute("ANNOTATION_ID").Value);
			Assert.AreEqual("FreeTranslation1", list.Values.ElementAt(0).Element("ANNOTATION_VALUE").Value);
			Assert.AreEqual("FreeTranslation2", list.Values.ElementAt(1).Element("ANNOTATION_VALUE").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDependentTiersElements_NoDependentTiersExist_ReturnsEmptyList()
		{
			LoadEafFile();
			Assert.IsEmpty(_eafFile.GetDependentTiersElements().ToList());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDependentTiersElements_DependentTiersExist_ReturnsThem()
		{
			LoadEafFile(false);
			var list = _eafFile.GetDependentTiersElements().ToList();
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("Phrase Free Translation", list[0].Attribute("TIER_ID").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateDependentTextTiers_NoTranscriptionAnnotationIds_ReturnsEmptyList()
		{
			LoadEafFile(false);
			Assert.IsEmpty(_eafFile.CreateDependentTextTiers(new string[] { }).ToList());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateDependentTextTiers_MoreTranscriptionAnnotationsThanDependentAnnotations_ReturnsTierWithCorrectAnnotationCount()
		{
			LoadEafFile(false);
			Assert.AreEqual(3, _eafFile.CreateDependentTextTiers(
				new[] { "a1", "a2", "a3" }).ElementAt(0).GetAllSegments().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateDependentTextTiers_OneDependentTeir_ReturnsTextTierWithCorrectSegmentTexts()
		{
			LoadEafFile(false);
			var textTier = _eafFile.CreateDependentTextTiers(new[] { "a1", "a2", "a3" }).ElementAt(0);
			Assert.AreEqual("FreeTranslation1", ((ITextSegment)textTier.GetSegment(0)).GetText());
			Assert.AreEqual("FreeTranslation2", ((ITextSegment)textTier.GetSegment(1)).GetText());
			Assert.IsEmpty(((ITextSegment)textTier.GetSegment(2)).GetText());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateDependentTextTiers_OneDependentTeir_ReturnsTextTierWithCorrectAnnotationIds()
		{
			LoadEafFile(false);
			var textTier = _eafFile.CreateDependentTextTiers(new[] { "a1", "a2", "a3" }).ElementAt(0);
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
			var element = _eafFile.GetOrCreateHeader();
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
			var element = _eafFile.GetOrCreateHeader();
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
			var element = _eafFile.CreateMediaDescriptorElement();
			Assert.AreEqual("MEDIA_DESCRIPTOR", element.Name.LocalName);
			Assert.IsNull(element.Attribute("MEDIA_URL"));
			Assert.IsNull(element.Attribute("MIME_TYPE"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMediaDescriptorElement_ValidMediaFile_ReturnsCorrectElementContent()
		{
			var element = new EafFile(null, @"c:\My\Folk\Music\Alathea.wav").CreateMediaDescriptorElement();
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

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateHeaderElement_ReturnsCorrectElementContent()
		//{
		//    var element = new EafFile(null, "Great Lake Swimmers.wav").CreateHeaderElement();
		//    Assert.AreEqual("HEADER", element.Name.LocalName);
		//    Assert.AreEqual(string.Empty, element.Attribute("MEDIA_FILE").Value);
		//    Assert.AreEqual("milliseconds", element.Attribute("TIME_UNITS").Value);
		//    Assert.IsNotNull(element.Element("MEDIA_DESCRIPTOR"));
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateLastUsedAnnotationIdPropertyElement_ReturnsCorrectElementContent()
		//{
		//    var element = _eafFile.CreateLastUsedAnnotationIdPropertyElement(123);
		//    Assert.AreEqual("PROPERTY", element.Name.LocalName);
		//    Assert.AreEqual("lastUsedAnnotationId", element.Attribute("NAME").Value);
		//    Assert.AreEqual("123", element.Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateTierElement_ReturnsCorrectElementContent()
		//{
		//    var element = _eafFile.CreateTierElement("tippy tier");
		//    Assert.AreEqual("TIER", element.Name.LocalName);
		//    Assert.AreEqual("en", element.Attribute("DEFAULT_LOCALE").Value);
		//    Assert.AreEqual("default-lt", element.Attribute("LINGUISTIC_TYPE_REF").Value);
		//    Assert.AreEqual("tippy tier", element.Attribute("TIER_ID").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateTimeSlotElements_ReturnsElementsWithCorrectNames()
		//{
		//    var elements = _eafFile.CreateTimeSlotElements(new[] { 0f, 0f, 0f }).ToList();
		//    Assert.AreEqual("TIME_SLOT", elements[0].Name.LocalName);
		//    Assert.AreEqual("TIME_SLOT", elements[1].Name.LocalName);
		//    Assert.AreEqual("TIME_SLOT", elements[2].Name.LocalName);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateTimeSlotElements_ReturnsElementsWithCorrectIds()
		//{
		//    var elements = _eafFile.CreateTimeSlotElements(new[] { 0f, 0f, 0f }).ToList();
		//    Assert.AreEqual("ts1", elements[0].Attribute("TIME_SLOT_ID").Value);
		//    Assert.AreEqual("ts2", elements[1].Attribute("TIME_SLOT_ID").Value);
		//    Assert.AreEqual("ts3", elements[2].Attribute("TIME_SLOT_ID").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateTimeSlotElements_ReturnsElementsWithCorrectValues()
		//{
		//    var elements = _eafFile.CreateTimeSlotElements(new[] { 0.3f, 0.7f, 0.9f }).ToList();
		//    Assert.AreEqual("300", elements[0].Attribute("TIME_VALUE").Value);
		//    Assert.AreEqual("700", elements[1].Attribute("TIME_VALUE").Value);
		//    Assert.AreEqual("900", elements[2].Attribute("TIME_VALUE").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateAnnotationElement_NullTextTier_ReturnsEmptyList()
		//{
		//    Assert.AreEqual(0, _eafFile.CreateAnnotationElements(null,
		//        _mediaTier.GetAllSegments().Cast<IMediaSegment>(), null).Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateAnnotationElement_NullMediaSegments_ReturnsEmptyList()
		//{
		//    Assert.AreEqual(0, _eafFile.CreateAnnotationElements(new TextTier("iron"), null, null).Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateAnnotationElement_NoTextSegments_ReturnsEmptyList()
		//{
		//    Assert.AreEqual(0, _eafFile.CreateAnnotationElements(new TextTier("copper"),
		//        _mediaTier.GetAllSegments().Cast<IMediaSegment>(), null).Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateAnnotationElement_GoodData_ReturnsCorrectNumberOfElements()
		//{
		//    Assert.AreEqual(2, _eafFile.CreateAnnotationElements(_textTier,
		//        _mediaTier.GetAllSegments().Cast<IMediaSegment>(), f => "").Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateAnnotationElement_GoodData_ReturnsCorrectElementNames()
		//{
		//    var elements = _eafFile.CreateAnnotationElements(_textTier,
		//        _mediaTier.GetAllSegments().Cast<IMediaSegment>(), f => "").ToList();

		//    Assert.AreEqual("ANNOTATION", elements[0].Name.LocalName);
		//    Assert.AreEqual("ANNOTATION", elements[1].Name.LocalName);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateAnnotationElement_GoodData_ReturnsCorrectAnnotationIds()
		//{
		//    var elements = _eafFile.CreateAnnotationElements(_textTier,
		//        _mediaTier.GetAllSegments().Cast<IMediaSegment>(), f => "").ToList();

		//    Assert.AreEqual("a1", elements[0].Element("ALIGNABLE_ANNOTATION").Attribute("ANNOTATION_ID").Value);
		//    Assert.AreEqual("a2", elements[1].Element("ALIGNABLE_ANNOTATION").Attribute("ANNOTATION_ID").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateAnnotationElement_GoodData_ReturnsCorrectTimeSlotRefs()
		//{
		//    int i = 5;

		//    var elements = _eafFile.CreateAnnotationElements(_textTier,
		//        _mediaTier.GetAllSegments().Cast<IMediaSegment>(), f => "ts" + i++).ToList();

		//    Assert.AreEqual("ts5", elements[0].Element("ALIGNABLE_ANNOTATION").Attribute("TIME_SLOT_REF1").Value);
		//    Assert.AreEqual("ts6", elements[0].Element("ALIGNABLE_ANNOTATION").Attribute("TIME_SLOT_REF2").Value);
		//    Assert.AreEqual("ts7", elements[1].Element("ALIGNABLE_ANNOTATION").Attribute("TIME_SLOT_REF1").Value);
		//    Assert.AreEqual("ts8", elements[1].Element("ALIGNABLE_ANNOTATION").Attribute("TIME_SLOT_REF2").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateAnnotationElement_GoodData_ReturnsCorrectAnnotations()
		//{
		//    var elements = _eafFile.CreateAnnotationElements(_textTier,
		//        _mediaTier.GetAllSegments().Cast<IMediaSegment>(), f => "").ToList();

		//    Assert.AreEqual("brass", elements[0].Element("ALIGNABLE_ANNOTATION").Element("ANNOTATION_VALUE").Value);
		//    Assert.AreEqual("steel", elements[1].Element("ALIGNABLE_ANNOTATION").Element("ANNOTATION_VALUE").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateAlignableAnnotationElement_ReturnsCorrectElementContent()
		//{
		//    var element = _eafFile.CreateAlignableAnnotationElement(48, "ts33", "ts55", "some text");
		//    Assert.AreEqual("ALIGNABLE_ANNOTATION", element.Name.LocalName);
		//    Assert.AreEqual("a48", element.Attribute("ANNOTATION_ID").Value);
		//    Assert.AreEqual("ts33", element.Attribute("TIME_SLOT_REF1").Value);
		//    Assert.AreEqual("ts55", element.Attribute("TIME_SLOT_REF2").Value);
		//    Assert.AreEqual("some text", element.Element("ANNOTATION_VALUE").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateLinguisticTypeElement_ReturnsCorrectElementContent()
		//{
		//    var element = _eafFile.CreateLinguisticTypeElement();
		//    Assert.AreEqual("LINGUISTIC_TYPE", element.Name.LocalName);
		//    Assert.AreEqual("false", element.Attribute("GRAPHIC_REFERENCES").Value);
		//    Assert.AreEqual("default-lt", element.Attribute("LINGUISTIC_TYPE_ID").Value);
		//    Assert.AreEqual("true", element.Attribute("TIME_ALIGNABLE").Value);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CreateLocaleElement_ReturnsCorrectElementContent()
		//{
		//    var element = _eafFile.CreateLocaleElement();
		//    Assert.AreEqual("LOCALE", element.Name.LocalName);
		//    Assert.AreEqual("IPA Extended", element.Attribute("VARIANT").Value);
		//    Assert.AreEqual("ipa-ext", element.Attribute("LANGUAGE_CODE").Value);
		//}
	}
}
