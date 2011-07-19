using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SayMore.Transcription.Model
{
	public class InterlinearXmlHelper
	{
		private readonly string _wsTranscriptionId;
		private readonly string _wsFreeTranslationId;
		private readonly string _title;
		private readonly ITier _transcriptionTier;

		/// ------------------------------------------------------------------------------------
		public InterlinearXmlHelper(string title, ITier transcriptionTier,
			string wsTranscriptionId, string wsFreeTranslationId)
		{
			_title = title;
			_transcriptionTier = transcriptionTier;
			_wsTranscriptionId = wsTranscriptionId;
			_wsFreeTranslationId = wsFreeTranslationId;
		}

		/// ------------------------------------------------------------------------------------
		public XElement GetPopulatedRootElement()
		{
			var rootElement = CreateRootElement();

			rootElement.Element("interlinear-text").Element("paragraphs").Add(
				CreateParagraphElements(_transcriptionTier));

			rootElement.Element("interlinear-text").Add(CreateLanguagesElement(
				new[] { _wsTranscriptionId, _wsFreeTranslationId }));

			return rootElement;
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateRootElement()
		{
			return new XElement("document", new XElement("interlinear-text",
				CreateItemElement(_wsFreeTranslationId, "title", _title),
				new XElement("paragraphs")));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateLanguagesElement(IEnumerable<string> langIds)
		{
			var element = new XElement("languages");

			if (langIds != null)
			{
				foreach (var id in langIds)
					element.Add(new XElement("language", new XAttribute("lang", id)));
			}

			return element;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<XElement> CreateParagraphElements(ITier tier)
		{
			// TODO: This will need refactoring when display name is localizable.
			var translationTier =
				tier.DependentTiers.SingleOrDefault(t => t.DisplayName.ToLower() == TextTier.SayMoreFreeTranslationTierName.ToLower());

			var segmentList = tier.GetAllSegments().Cast<ITextSegment>().ToArray();

			for (int i = 0; i < segmentList.Length; i++)
			{
				ISegment freeTranslationSegment;
				translationTier.TryGetSegment(i, out freeTranslationSegment);
				var freeTranslation = freeTranslationSegment as ITextSegment;

				yield return CreateSingleParagraphElement(segmentList[i].GetText(),
					(freeTranslation != null ? freeTranslation.GetText() : null));
			}
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateSingleParagraphElement(string transcription, string freeTranslation)
		{
			var transcriptionElement = CreateSingleWordElement(transcription);
			var phraseElement = new XElement("phrase", transcriptionElement);

			if (freeTranslation != null)
				phraseElement.Add(CreateItemElement(_wsFreeTranslationId, "gls", freeTranslation));

			return new XElement("paragraph", new XElement("phrases", phraseElement));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateSingleWordElement(string text)
		{
			return new XElement("words", new XElement("word",
				CreateItemElement(_wsTranscriptionId, "txt", text)));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateItemElement(string langId, string type, string text)
		{
			return new XElement("item", new XAttribute("type", type),
				new XAttribute("lang", langId), text);
		}

		/// ------------------------------------------------------------------------------------
		public static void Save(string filename, string title, ITier transcriptionTier,
			string wsTranscriptionId, string wsFreeTranslationId)
		{
			var helper = new InterlinearXmlHelper(title,
				transcriptionTier, wsTranscriptionId, wsFreeTranslationId);

			helper.GetPopulatedRootElement().Save(filename);
		}
	}
}
