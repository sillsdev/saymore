using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	public class EafFile
	{
		public string MediaFileName { get; private set; }
		public string EafFileName { get; private set; }
		public IEnumerable<ISegment> Segments { get; private set; }

		/// ------------------------------------------------------------------------------------
		public EafFile(string eafFileName, string mediaFileName)
		{
			EafFileName = eafFileName;
			MediaFileName = mediaFileName;

			if (File.Exists(eafFileName))
			{
				// Load file;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Save(AudioTier origTier, IEnumerable<ITier> tiers)
		{
			var timeSlots = origTier.GetAllSegments().Cast<IMediaSegment>().Select(s => s .MediaStart)
				.Concat(origTier.GetAllSegments().Cast<IMediaSegment>().Select(s => s.MediaStart + s.MediaLength))
				.Distinct().OrderBy(s => s);

			var timeOrderElement = CreateTimeOrderElement(timeSlots);
			var headerElement = CreateHeaderElement();
			headerElement.Add(CreateLastUsedAnnotationIdPropertyElement(timeSlots.Count()));

			var root = CreateRootElement();
			root.Add(headerElement, timeOrderElement);

			foreach (var tier in tiers.Where(t => t.DataType == TierType.Text))
				root.Add(CreateTierElement(origTier, tier, timeSlots));

			root.Add(CreateLinguisticTypeElement());
			root.Add(CreateLocaleElement());

			var folder = Path.GetDirectoryName(EafFileName);
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			root.Save(EafFileName);
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateRootElement()
		{
			XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

			return new XElement("ANNOTATION_DOCUMENT",
				new XAttribute("AUTHOR", string.Empty),
				new XAttribute("DATE", XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Local)),
				new XAttribute("FORMAT", 2.7f),
				new XAttribute("VERSION", 2.7f),
				new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
				new XAttribute(xsi + "noNamespaceSchemaLocation", "http://www.mpi.nl/tools/elan/EAFv2.7.xsd"));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateHeaderElement()
		{
			return new XElement("HEADER",
				new XAttribute("MEDIA_FILE", string.Empty),
				new XAttribute("TIME_UNITS", "milliseconds"),
				CreateMediaDescriptorElement());
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateMediaDescriptorElement()
		{
			return new XElement("MEDIA_DESCRIPTOR",
				new XAttribute("MEDIA_URL", MediaFileName),
				new XAttribute("MIME_TYPE", CreateMediaFileMimeType()));
		}

		/// ------------------------------------------------------------------------------------
		public string CreateMediaFileMimeType()
		{
			return "audio/" + (Path.GetExtension(MediaFileName).ToLower() == ".wav" ? "x-wav" : "*");
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateLastUsedAnnotationIdPropertyElement(int lastUsedAnnotationId)
		{
			return new XElement("PROPERTY",
				new XAttribute("NAME", "lastUsedAnnotationId"), lastUsedAnnotationId);
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateTimeOrderElement(IEnumerable<float> timeSlots)
		{
			int id = 0;

			return new XElement("TIME_ORDER", timeSlots.Select(value => new XElement("TIME_SLOT",
					new XAttribute("TIME_SLOT_ID", string.Format("ts{0}", ++id)),
					new XAttribute("TIME_VALUE", (int)(value * 1000)))));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateTierElement(AudioTier origTier, ITier tier, IEnumerable<float> tSlots)
		{
			var timeSlots = tSlots.ToList();

			// TODO: Fix the default locale.
			var tierElement = new XElement("TIER",
				new XAttribute("DEFAULT_LOCALE", "en"),
				new XAttribute("LINGUISTIC_TYPE_REF", "default-lt"),
				new XAttribute("TIER_ID", tier.DisplayName));

			var i = 0;

			foreach (var mediaSegment in origTier.GetAllSegments().Cast<IMediaSegment>())
			{
				ISegment textSegment;
				if (!tier.TryGetSegment(i, out textSegment) || !(textSegment is ITextSegment))
					continue;

				var start = timeSlots.IndexOf(mediaSegment.MediaStart);
				var stop = timeSlots.IndexOf(mediaSegment.MediaStart + mediaSegment.MediaLength);

				var annotationElement = new XElement("ANNOTATION");
				annotationElement.Add(new XElement("ALIGNABLE_ANNOTATION",
					new XAttribute("ANNOTATION_ID", string.Format("a{0}", ++i)),
					new XAttribute("TIME_SLOT_REF1", string.Format("ts{0}", start + 1)),
					new	XAttribute("TIME_SLOT_REF2", string.Format("ts{0}", stop + 1)),
					new XElement("ANNOTATION_VALUE", ((ITextSegment)textSegment).GetText())));

				tierElement.Add(annotationElement);
			}

			return tierElement;
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateLinguisticTypeElement()
		{
			return new XElement("LINGUISTIC_TYPE",
				new XAttribute("GRAPHIC_REFERENCES", false),
				new XAttribute("LINGUISTIC_TYPE_ID", "default-lt"),
				new XAttribute("TIME_ALIGNABLE", true));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateLocaleElement()
		{
			// TODO: Fix this.
			return new XElement("LOCALE",
				new XAttribute("COUNTRY_CODE", "US"),
				new XAttribute("LANGUAGE_CODE", "en"));
		}
	}
}
