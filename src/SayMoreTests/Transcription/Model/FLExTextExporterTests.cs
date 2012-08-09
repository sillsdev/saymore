using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using SayMore.Transcription.Model;
using SayMore.Transcription.Model.Exporters;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class FLExTextExporterTests
	{
		FLExTextExporter _helper;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_helper = new FLExTextExporter(null, "Homer", null, "en", "fr");
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
			var element = _helper.CreateSingleWordElement(string.Empty);
			Assert.AreEqual("words", element.Name.LocalName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleWordElement_CreatesCorrectSubElementCount()
		{
			var element = _helper.CreateSingleWordElement(string.Empty);
			Assert.AreEqual(1, element.Elements().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleWordElement_CreatesCorrectSubElements()
		{
			var element = _helper.CreateSingleWordElement(string.Empty);
			Assert.IsNotNull(element.Element("word"));
			Assert.IsNotNull(element.Element("word").Element("item"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleWordElement_CreatesCorrectSubElementContent()
		{
			var element = _helper.CreateSingleWordElement("gopher").Element("word").Element("item");
			Assert.AreEqual("txt", element.Attribute("type").Value);
			Assert.AreEqual("en", element.Attribute("lang").Value);
			Assert.AreEqual("gopher", element.Value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleParagraphElement_NullFreeTranslation_CreatesCorrectElements()
		{
			CheckParagraphElement(_helper.CreateSingleParagraphElement("prairie dog", null),
				"prairie dog", null);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleParagraphElement_FreeTranslationPresent_CreatesCorrectElements()
		{
			CheckParagraphElement(_helper.CreateSingleParagraphElement("prairie dog", "squirrel"),
				"prairie dog", "squirrel");
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

			CheckParagraphElement(elements[0], "up", "in");
			CheckParagraphElement(elements[1], "down", "around");
			CheckParagraphElement(elements[2], "over", "through");
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

			_helper = new FLExTextExporter(null, "Homer", new TierCollection { tier, dependentTier }, "en", "fr");
		}

		/// ------------------------------------------------------------------------------------
		public void CheckParagraphElement(XElement element, string expectedTranscription,
			string expectedFreeTranslation)
		{
			Assert.AreEqual("paragraph", element.Name.LocalName);
			Assert.IsNotNull(element.Element("phrases"));
			Assert.IsNotNull(element.Element("phrases").Element("phrase"));
			Assert.IsNotNull(element.Element("phrases").Element("phrase").Element("words"));
			Assert.IsNotNull(element.Element("phrases").Element("phrase").Element("words").Element("word"));
			Assert.IsNotNull(element.Element("phrases").Element("phrase").Element("words").Element("word").Element("item"));

			var transcriptionElement = element.Element("phrases").Element("phrase").Element("words").Element("word").Element("item");
			Assert.AreEqual("txt", transcriptionElement.Attribute("type").Value);
			Assert.AreEqual("en", transcriptionElement.Attribute("lang").Value);
			Assert.AreEqual(expectedTranscription, transcriptionElement.Value);

			if (expectedFreeTranslation == null)
			{
				Assert.IsNull(element.Element("phrases").Element("phrase").Element("item"));
				return;
			}

			Assert.IsNotNull(element.Element("phrases").Element("phrase").Element("item"));

			var freeTransElement = element.Element("phrases").Element("phrase").Element("item");
			Assert.AreEqual("gls", freeTransElement.Attribute("type").Value);
			Assert.AreEqual("fr", freeTransElement.Attribute("lang").Value);
			Assert.AreEqual(expectedFreeTranslation, freeTransElement.Value);
		}
	}
}
