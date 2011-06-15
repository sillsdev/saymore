using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SayMore.Transcription.Model
{
	public class EafFile
	{
		public string MediaFileName { get; private set; }
		public string EafFileName { get; private set; }
		public IEnumerable<ITier> TextTiers { get; private set; }
		public ITier MediaTier { get; private set; }

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
		public void SetTextTiers(IEnumerable<ITier> tiers)
		{
			TextTiers = tiers;
		}

		/// ------------------------------------------------------------------------------------
		public void SetMediaTier(ITier tier)
		{
			MediaTier = tier;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<IMediaSegment> GetMediaSegments()
		{
			return MediaTier.GetAllSegments().Cast<IMediaSegment>().ToList();
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<float> GetTimeSlotCollection()
		{
			var mediaSegments = GetMediaSegments().ToList();

			return mediaSegments.Select(s => s.MediaStart)
				.Concat(mediaSegments.Select(s => s.MediaStart + s.MediaLength))
				.Distinct().OrderBy(s => s).ToList();
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			var timeSlotCollection = GetTimeSlotCollection();
			var timeOrderElement = CreateTimeOrderElement(timeSlotCollection);
			var headerElement = CreateHeaderElement();
			headerElement.Add(CreateLastUsedAnnotationIdPropertyElement(timeSlotCollection.Count()));

			var root = CreateRootElement();
			root.Add(headerElement, timeOrderElement);

			foreach (var tier in TextTiers.Where(t => t.DataType == TierType.Text))
				root.Add(CreateTierElement(tier));

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
					new XAttribute("TIME_VALUE", (int)Math.Round(value * 1000f)))));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateTierElement(ITier textTier)
		{
			var tierElement = CreateTierElement(textTier.DisplayName);

			var i = 0;
			var annotationId = 1;

			foreach (var mediaSegment in GetMediaSegments())
			{
				ISegment textSegment;
				if (!textTier.TryGetSegment(i++, out textSegment))
					continue;

				var annotationElement = CreateAnnotationElement(mediaSegment,
					textSegment as ITextSegment, annotationId);

				if (annotationElement == null)
					continue;

				tierElement.Add(annotationElement);
				annotationId++;
			}

			return tierElement;
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateAnnotationElement(IMediaSegment mediaSegment,
			ITextSegment textSegment, int annotationId)
		{
			if (textSegment == null)
				return null;

			var startTimeSlotId = GetTimeSlotId(mediaSegment.MediaStart);
			var stopTimeSlotId = GetTimeSlotId(mediaSegment.MediaStart + mediaSegment.MediaLength);

			if (startTimeSlotId == null || stopTimeSlotId == null)
				return null;

			return new XElement("ANNOTATION", CreateAlignableAnnotationElement(annotationId,
				startTimeSlotId, stopTimeSlotId, textSegment.GetText()));
		}

		/// ------------------------------------------------------------------------------------
		public string GetTimeSlotId(float time)
		{
			var index = GetTimeSlotCollection().ToList().IndexOf(time);
			return (index < 0 ? null : string.Format("ts{0}", index + 1));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateTierElement(string tierName)
		{
			// TODO: Fix the default locale.
			return new XElement("TIER",
				new XAttribute("DEFAULT_LOCALE", "en"),
				new XAttribute("LINGUISTIC_TYPE_REF", "default-lt"),
				new XAttribute("TIER_ID", tierName));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateAlignableAnnotationElement(int annotationId,
			string startTimeSlotId, string stopTimeSlotId, string annotationText)
		{
			return new XElement("ALIGNABLE_ANNOTATION",
					new XAttribute("ANNOTATION_ID", string.Format("a{0}", annotationId)),
					new XAttribute("TIME_SLOT_REF1", startTimeSlotId),
					new	XAttribute("TIME_SLOT_REF2", stopTimeSlotId),
					new XElement("ANNOTATION_VALUE", annotationText));
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
