using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using SIL.TestUtilities;
using SayMore.Transcription.Model;
using SayMore.Transcription.Model.Exporters;
using SayMoreTests.MediaUtils;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class FLExTextExporterTests
	{
		FLExTextExporter _helper;
		private TemporaryFolder _tempFolder;
		private string _mediaFile;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_helper = new FLExTextExporter(null, "Homer", null, "en", "fr", "filename1", "filename2");

			var tempMediaPath = MediaFileInfoTests.GetLongerTestAudioFile();
			_tempFolder = new TemporaryFolder("TierCollectionTests");
			_mediaFile = Path.Combine(_tempFolder.Path, "mediaFile.wav");
			File.Move(tempMediaPath, _mediaFile);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			if (_tempFolder != null)
			{
				_tempFolder.Dispose();
				_tempFolder = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateRootElement_CreatesDocumentElementWithVersionAttrib()
		{
			var element = _helper.CreateRootElement();
			Assert.AreEqual("document", element.Name.LocalName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateRootElement_CreatesOuterTwoLevels()
		{
			var element = _helper.CreateRootElement();
			Assert.AreEqual(1, element.Elements().Count());
			Assert.IsNotNull(element.Element("interlinear-text"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateRootElement_CreatesTitleInRoot()
		{
			var element = _helper.CreateRootElement();
			Assert.IsNotNull(element.Element("interlinear-text").Element("item"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateRootElement_CreatesCorrectTitleAttributes()
		{
			var element = _helper.CreateRootElement().Element("interlinear-text").Element("item");
			Assert.IsNotNull(element.Attribute("type"));
			Assert.IsNotNull(element.Attribute("lang"));
			Assert.AreEqual("title", element.Attribute("type").Value);
			Assert.AreEqual("fr", element.Attribute("lang").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateRootElement_CreatesCorrectTitleContents()
		{
			var element = _helper.CreateRootElement();
			Assert.AreEqual("Homer", element.Element("interlinear-text").Element("item").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateLanguagesElement_NullInputList_ReturnsEmptyElement()
		{
			Assert.IsFalse(_helper.CreateLanguagesElement(null).HasElements);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateLanguagesElement_EmptyInputList_ReturnsEmptyElement()
		{
			Assert.IsFalse(_helper.CreateLanguagesElement(new string[] { }).HasElements);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateLanguagesElement_GoodInputList_ReturnsCorrectSubElementCount()
		{
			var element = _helper.CreateLanguagesElement(new[] { "en", "fr" });
			Assert.AreEqual(2, element.Elements().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateLanguagesElement_GoodInputList_ReturnsCorrectSubElements()
		{
			var element = _helper.CreateLanguagesElement(new[] { "en", "fr" });

			foreach (var subElement in element.Elements())
				Assert.AreEqual("language", subElement.Name.LocalName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateLanguagesElement_GoodInputList_ReturnsCorrectSubElementAttributes()
		{
			var element = _helper.CreateLanguagesElement(new[] { "en", "fr" });
			Assert.IsNotNull(element.Elements().Single(e => e.Attribute("lang").Value == "en"));
			Assert.IsNotNull(element.Elements().Single(e => e.Attribute("lang").Value == "fr"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateItemElement_CreatesCorrectElement()
		{
			var element = _helper.CreateItemElement("en", "blah", "boring text");
			Assert.AreEqual("item", element.Name.LocalName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateItemElement_CreatesCorrectElementLanguageAttribute()
		{
			var element = _helper.CreateItemElement("en", "blah", "boring text");
			Assert.AreEqual("en", element.Attribute("lang").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateItemElement_CreatesCorrectElementTypeAttribute()
		{
			var element = _helper.CreateItemElement("en", "blah", "boring text");
			Assert.AreEqual("blah", element.Attribute("type").Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateItemElement_CreatesCorrectElementContent()
		{
			var element = _helper.CreateItemElement("en", "blah", "boring text");
			Assert.AreEqual("boring text", element.Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleWordElement_CreatesCorrectElementName()
		{
			//var element = _helper.CreateSingleWordElement(string.Empty);
			//Assert.AreEqual("words", element.Name.LocalName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleWordElement_CreatesCorrectSubElementCount()
		{
			//var element = _helper.CreateSingleWordElement(string.Empty);
			//Assert.AreEqual(1, element.Elements().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleWordElement_CreatesCorrectSubElements()
		{
			//var element = _helper.CreateSingleWordElement(string.Empty);
			//Assert.IsNotNull(element.Element("word"));
			//Assert.IsNotNull(element.Element("word").Element("item"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleWordElement_CreatesCorrectSubElementContent()
		{
			//var element = _helper.CreateSingleWordElement("gopher").Element("word").Element("item");
			//Assert.AreEqual("txt", element.Attribute("type").Value);
			//Assert.AreEqual("en", element.Attribute("lang").Value);
			//Assert.AreEqual("gopher", element.Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleParagraphElement_NullFreeTranslation_CreatesCorrectElements()
		{
			CheckParagraphElement(_helper.CreateSingleParagraphElement("prairie dog", null, "10", "20"),
				"prairie dog", null, "10", "20", 1);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleParagraphElement_FreeTranslationPresent_CreatesCorrectElements()
		{
			CheckParagraphElement(_helper.CreateSingleParagraphElement("prairie dog", "squirrel", "10", "20"),
				"prairie dog", "squirrel", "10", "20", 1);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateParagraphElements_CreatesCorrectNumberOfElements()
		{
			CreateTestTier();
			Assert.AreEqual(3, _helper.CreateParagraphElements().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateParagraphElements_CreatesCorrectElements()
		{
			CreateTestTier();
			var elements = _helper.CreateParagraphElements().ToArray();

			CheckParagraphElement(elements[0], "up", "in", "10000", "20000", 1);
			CheckParagraphElement(elements[1], "down", "around", "20000", "30000", 2);
			CheckParagraphElement(elements[2], "over", "through", "30000", "40000", 3);
		}

		/// ------------------------------------------------------------------------------------
		private void CreateTestTier()
		{
			var tier = new TextTier(TextTier.ElanTranscriptionTierId);
			tier.AddSegment("up");
			tier.AddSegment("down");
			tier.AddSegment("over");

			var dependentTier = new TextTier(TextTier.ElanTranslationTierId);
			dependentTier.AddSegment("in");
			dependentTier.AddSegment("around");
			dependentTier.AddSegment("through");

			var timeTier = new TimeTier("test tier", _mediaFile);
			timeTier.AddSegment(10f, 20f);
			timeTier.AddSegment(20f, 30f);
			timeTier.AddSegment(30f, 40f);

			_helper = new FLExTextExporter(null, "Homer", new TierCollection { tier, dependentTier, timeTier }, "en", "fr", "filename1", "filename2");
		}

		/// ------------------------------------------------------------------------------------
		public void CheckParagraphElement(XElement element, string expectedTranscription,
			string expectedFreeTranslation, string expectedStartOffset, string expectedEndOffSet, int segmentNumber)
		{
			Assert.AreEqual("paragraph", element.Name.LocalName);

			var phrases = element.Element("phrases");
			Assert.IsNotNull(phrases);

			var phrase = phrases.Element("phrase");
			Assert.IsNotNull(phrase);

			// phrase
			Assert.IsNotNull(phrase.Element("item"));
			Assert.IsNotNull(phrase.Element("words"));
			Assert.AreEqual(expectedStartOffset, phrase.Attribute("begin-time-offset").Value);
			Assert.AreEqual(expectedEndOffSet, phrase.Attribute("end-time-offset").Value);

			var items = phrase.Elements().Where(e => e.Name.LocalName == "item").ToList();

			// segnum
			var segnumElement = items.FirstOrDefault(e => e.Attribute("type").Value == "segnum");
			Assert.IsNotNull(segnumElement);
			Assert.AreEqual("fr", segnumElement.Attribute("lang").Value);
			Assert.AreEqual(segmentNumber.ToString(CultureInfo.InvariantCulture), segnumElement.Value);

			// transcription
			var transcriptionElement = items.FirstOrDefault(e => e.Attribute("type").Value == "txt");
			Assert.IsNotNull(transcriptionElement);
			Assert.AreEqual("en", transcriptionElement.Attribute("lang").Value);
			Assert.AreEqual(expectedTranscription, transcriptionElement.Value);

			// translation
			var freeTransElement = items.FirstOrDefault(e => e.Attribute("type").Value == "gls");

			if (expectedFreeTranslation == null)
			{
				Assert.IsNull(freeTransElement);
				return;
			}

			Assert.IsNotNull(freeTransElement);
			Assert.AreEqual("fr", freeTransElement.Attribute("lang").Value);
			Assert.AreEqual(expectedFreeTranslation, freeTransElement.Value);
		}
	}
}
