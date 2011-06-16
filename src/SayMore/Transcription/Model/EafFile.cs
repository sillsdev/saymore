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
		//public IEnumerable<ITier> TextTiers { get; private set; }
		//public ITier MediaTier { get; private set; }

		/// ------------------------------------------------------------------------------------
		public EafFile(string eafFileName, string mediaFileName)
		{
			EafFileName = eafFileName;
			MediaFileName = mediaFileName;
			//SetTextTiers(new List<ITier>());

			if (File.Exists(eafFileName))
				Load(XElement.Load(EafFileName));
		}

		///// ------------------------------------------------------------------------------------
		private float AddFloats(float x, float y)
		{
			return (float)((decimal)x + (decimal)y);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<float> GetTimeSlotCollection(ITier mediaTier)
		{
			var mediaSegments = mediaTier.GetAllSegments().Cast<IMediaSegment>();

			return mediaSegments.Select(s => s.MediaStart)
				.Concat(mediaSegments.Select(s => AddFloats(s.MediaStart, s.MediaLength)))
				.Distinct().OrderBy(s => s);
		}

		#region Methods for reading an EAF file
		/// ------------------------------------------------------------------------------------
		public IEnumerable<ITier> Load(XElement root)
		{
			var timeSlots = root.Element("TIME_ORDER").Elements("TIME_SLOT").ToDictionary(
				e => e.Attribute("TIME_SLOT_ID").Value,
				e => (float)(int.Parse(e.Attribute("TIME_VALUE").Value) / (decimal)1000));

			var	tierElement = root.Elements().First(e => e.Name.LocalName == "TIER");

			var mediaTier = new AudioTier("Original", MediaFileName);
			var textTier = new TextTier(tierElement.Attribute("TIER_ID").Value);

			foreach (var annElement in tierElement.Elements("ANNOTATION"))
				AddSegment(mediaTier, textTier, annElement.Element("ALIGNABLE_ANNOTATION"), timeSlots);

			return new[] { (ITier)mediaTier, textTier };
		}

		/// ------------------------------------------------------------------------------------
		public void AddSegment(AudioTier mediaTier, TextTier textTier, XElement element,
			IDictionary<string, float> timeSlots)
		{
			var start = timeSlots[element.Attribute("TIME_SLOT_REF1").Value];
			var stop = timeSlots[element.Attribute("TIME_SLOT_REF2").Value];

			mediaTier.AddSegment(start, (float)((decimal)stop - (decimal)start));
			textTier.AddSegment(element.Element("ANNOTATION_VALUE").Value);
		}

		#endregion

		#region Methods for writing an EAF file
		/// ------------------------------------------------------------------------------------
		public void Save(ITier mediaTier, IEnumerable<ITier> textTiers)
		{
			var timeSlotCollection = GetTimeSlotCollection(mediaTier).ToList();
			var timeOrderElement = new XElement("TIME_ORDER", CreateTimeSlotElements(timeSlotCollection));
			var headerElement = CreateHeaderElement();
			headerElement.Add(CreateLastUsedAnnotationIdPropertyElement(timeSlotCollection.Count()));

			var root = CreateRootElement();
			root.Add(headerElement, timeOrderElement);

			foreach (var tier in textTiers.Where(t => t.DataType == TierType.Text))
			{
				var tierElement = CreateTierElement(tier.DisplayName);
				tierElement.Add(CreateAnnotationElements(tier,
					mediaTier.GetAllSegments().Cast<IMediaSegment>(), time =>
					{
						var index = timeSlotCollection.IndexOf(time);
						return (index < 0 ? null : string.Format("ts{0}", index + 1));
					}));

				root.Add(tierElement);
			}

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
		public IEnumerable<XElement> CreateTimeSlotElements(IEnumerable<float> timeSlots)
		{
			int timeSlotId = 1;

			return timeSlots.Select(value => new XElement("TIME_SLOT",
				new XAttribute("TIME_SLOT_ID", string.Format("ts{0}", timeSlotId++)),
				new XAttribute("TIME_VALUE", (int)Math.Round(value * 1000f))));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateTierElement(string tierId)
		{
			// TODO: Fix the default locale.
			return new XElement("TIER",
				new XAttribute("DEFAULT_LOCALE", "en"),
				new XAttribute("LINGUISTIC_TYPE_REF", "default-lt"),
				new XAttribute("TIER_ID", tierId));
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<XElement> CreateAnnotationElements(ITier textTier,
			IEnumerable<IMediaSegment> mediaSegments, Func<float, string> getTimeSlotId)
		{
			if (textTier != null && mediaSegments != null)
			{
				var i = 0;
				var annotationId = 1;

				foreach (var mseg in mediaSegments)
				{
					ISegment tSeg;
					if (!textTier.TryGetSegment(i++, out tSeg))
						continue;

					var startTimeSlotId = getTimeSlotId(mseg.MediaStart);
					var stopTimeSlotId = getTimeSlotId(AddFloats(mseg.MediaStart, mseg.MediaLength));

					if (startTimeSlotId == null || stopTimeSlotId == null)
						continue;

					yield return new XElement("ANNOTATION", CreateAlignableAnnotationElement(annotationId++,
						startTimeSlotId, stopTimeSlotId, ((ITextSegment)tSeg).GetText()));
				}
			}
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

		#endregion
	}
}
