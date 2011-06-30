using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Palaso.IO;

namespace SayMore.Transcription.Model
{
	public class EafFile
	{
		public string EafFileName { get; private set; }
		public XElement Root { get; private set; }

		private string _mediaFileName;

		#region Constructors and static methods for loading and verifying validity of file
		/// ------------------------------------------------------------------------------------
		private EafFile(string eafFileName)
		{
			EafFileName = eafFileName;
			Root = XElement.Load(EafFileName);

			if (Root.Name.LocalName != "ANNOTATION_DOCUMENT")
				Root = null;
		}

		/// ------------------------------------------------------------------------------------
		public EafFile(string eafFileName, string mediaFileName)
		{
			EafFileName = eafFileName;
			_mediaFileName = mediaFileName;
		}

		/// ------------------------------------------------------------------------------------
		public static EafFile Load(string eafFileName)
		{
			if (!File.Exists(eafFileName) || !GetIsElanFile(eafFileName))
				return null;

			return new EafFile(eafFileName);
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsElanFile(string fileName)
		{
			try
			{
				var root = XElement.Load(fileName);
				return root.Name.LocalName == "ANNOTATION_DOCUMENT";
			}
			catch { }

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the path of the folder that contains the annotation file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetAnnotationFolderPath()
		{
			return Path.GetDirectoryName(EafFileName);
		}

		#endregion

		#region Methods for reading/writing media file
		/// ------------------------------------------------------------------------------------
		public static void ChangeMediaFileName(string eafFileName, string mediaFileName)
		{
			var eafFile = Load(eafFileName);

			if (eafFile != null)
			{
				eafFile.SetMediaFile(mediaFileName);
				eafFile.Save();
			}
		}

		/// ------------------------------------------------------------------------------------
		public string GetFullPathToMediaFile()
		{
			if (_mediaFileName == null)
			{
				var element = Root.Element("HEADER");
				if (element == null)
					return null;

				element = element.Element("MEDIA_DESCRIPTOR");
				if (element == null)
					return null;

				var mediaUrl = element.Attribute("MEDIA_URL");
				if (mediaUrl != null)
					_mediaFileName = mediaUrl.Value;
			}

			return (_mediaFileName == null || Path.IsPathRooted(_mediaFileName) ?
				_mediaFileName :
				Path.Combine(GetAnnotationFolderPath(), Path.GetFileName(_mediaFileName)));
		}

		/// ------------------------------------------------------------------------------------
		public void SetMediaFile(string mediaFileName)
		{
			_mediaFileName = mediaFileName;

			var header = GetOrCreateHeader();

			if (header.Element("MEDIA_DESCRIPTOR") != null)
				header.Element("MEDIA_DESCRIPTOR").Remove();

			header.Add(CreateMediaDescriptorElement());
		}

		#endregion

		#region Methods for reading/writing lastUsedAnnotationId
		/// ------------------------------------------------------------------------------------
		public string GetNextAvailableAnnotationIdAndIncrement()
		{
			var header = GetOrCreateHeader();

			if (header.Element("PROPERTY") == null || header.Element("PROPERTY").Attribute("NAME") == null ||
				header.Element("PROPERTY").Attribute("NAME").Value != "lastUsedAnnotationId")
			{
				header.Add(new XElement("PROPERTY", new XAttribute("NAME", "lastUsedAnnotationId"), 0));
			}

			var element = header.Elements("PROPERTY").Where(e => e.Attribute("NAME") != null &&
				e.Attribute("NAME").Value == "lastUsedAnnotationId").Single();

			int id;
			if (!int.TryParse(element.Value, out id))
				id = 0;

			element.SetValue(++id);
			return string.Format("a{0}", id);
		}

		#endregion

		#region Methods for getting the time slots
		/// ------------------------------------------------------------------------------------
		public IDictionary<string, float> GetTimeSlots()
		{
			var element = Root.Element("TIME_ORDER");
			if (element == null)
				return new Dictionary<string, float>();

			return element.Elements("TIME_SLOT").ToDictionary(
				e => e.Attribute("TIME_SLOT_ID").Value,
				e => (float)(int.Parse(e.Attribute("TIME_VALUE").Value) / (decimal)1000));
		}

		#endregion

		#region Methods for getting/creating a header element (including media descriptor element)
		/// ------------------------------------------------------------------------------------
		public XElement GetOrCreateHeader()
		{
			var header = Root.Element("HEADER");
			if (header == null)
			{
				Root.Add(new XElement("HEADER",
					new XAttribute("MEDIA_FILE", string.Empty),
					new XAttribute("TIME_UNITS", "milliseconds"),
					CreateMediaDescriptorElement()));
			}

			return Root.Element("HEADER");
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateMediaDescriptorElement()
		{
			var element = new XElement("MEDIA_DESCRIPTOR");

			if (_mediaFileName != null)
			{
				element.Add(new XAttribute("MEDIA_URL", Path.GetFileName(_mediaFileName)),
					new XAttribute("MIME_TYPE", CreateMediaFileMimeType()));
			}

			return element;
		}

		/// ------------------------------------------------------------------------------------
		public string CreateMediaFileMimeType()
		{
			return "audio/" + (Path.GetExtension(_mediaFileName).ToLower() == ".wav" ? "x-wav" : "*");
		}

		#endregion

		#region Methods for creating ITier objects from annotations from the EAF file.
		/// ------------------------------------------------------------------------------------
		public IEnumerable<ITier> GetTiers()
		{
			var timeSlots = GetTimeSlots();

			var transcriptionAnnotations = GetTranscriptionTierAnnotations();

			if (transcriptionAnnotations.Count == 0)
				return new ITier[] { };

			var timeOrderTier = new TimeOrderTier(GetFullPathToMediaFile());
			var textTier = new TextTier("Transcription");

			foreach (var kvp in transcriptionAnnotations)
			{
				var start = timeSlots[kvp.Value.Attribute("TIME_SLOT_REF1").Value];
				var stop = timeSlots[kvp.Value.Attribute("TIME_SLOT_REF2").Value];
				timeOrderTier.AddSegment(start, stop);
				textTier.AddSegment(kvp.Key, kvp.Value.Value);
			}

			textTier.AddDependentTierRange(CreateDependentTextTiers(transcriptionAnnotations.Keys));

			return new[] { (ITier)timeOrderTier, textTier };
		}

		/// ------------------------------------------------------------------------------------
		public IDictionary<string, XElement> GetTranscriptionTierAnnotations()
		{
			var transcriptionTierElement = GetTranscriptionTierElement();

			if (transcriptionTierElement == null)
				return new Dictionary<string, XElement>();

			return transcriptionTierElement.Elements("ANNOTATION")
				.Select(e => e.Element("ALIGNABLE_ANNOTATION"))
				.ToDictionary(e => e.Attribute("ANNOTATION_ID").Value, e => e);
		}

		/// ------------------------------------------------------------------------------------
		public XElement GetTranscriptionTierElement()
		{
			var element = Root.Elements().FirstOrDefault(e => e.Name.LocalName == "TIER" &&
				e.Attribute("TIER_ID") != null && e.Attribute("TIER_ID").Value.ToLower() == "transcription");

			if (element == null)
			{
				element = new XElement("TIER",
					new XAttribute("DEFAULT_LOCALE", "ipa-ext"),
					new XAttribute("LINGUISTIC_TYPE_REF", "Transcription"),
					new XAttribute("TIER_ID", "Transcription"));

				Root.Add(element);
			}

			return element;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<XElement> GetDependentTiersElements()
		{
			// Create a list of all tiers that reference the transcription tier.
			return Root.Elements().Where(e => e.Name.LocalName == "TIER" &&
				e.Attribute("PARENT_REF") != null && e.Attribute("PARENT_REF").Value.ToLower() == "transcription");
		}

		/// ------------------------------------------------------------------------------------
		public IDictionary<string, XElement> GetDependentTierAnnotationElements(XElement dependentTierElement)
		{
			if (dependentTierElement == null)
				return new Dictionary<string, XElement>();

			return dependentTierElement.Elements("ANNOTATION")
				.Select(e => e.Element("REF_ANNOTATION"))
				.ToDictionary(e => e.Attribute("ANNOTATION_REF").Value, e => e);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ITier> CreateDependentTextTiers(IEnumerable<string> transcriptionAnnotationIds)
		{
			foreach (var dependentTierElement in GetDependentTiersElements())
			{
				var depAnnotations = GetDependentTierAnnotationElements(dependentTierElement);
				var dependentTier = new TextTier(dependentTierElement.Attribute("TIER_ID").Value);

				// Go through all the annotations in the transcription tier looking for
				// annotations in the dependent tier that reference the annotations in
				// the transcription tier.
				foreach (var id in transcriptionAnnotationIds)
				{
					XElement depElement;
					if (depAnnotations.TryGetValue(id, out depElement))
					{
						dependentTier.AddSegment(depElement.Attribute("ANNOTATION_ID").Value,
							depElement.Element("ANNOTATION_VALUE").Value);
					}
					else
					{
						dependentTier.AddSegment(null, string.Empty);
					}
				}

				if (dependentTier.GetAllSegments().Count() > 0)
					yield return dependentTier;
			}
		}

		#endregion

		#region Methods for saving an EAF file
		/// ------------------------------------------------------------------------------------
		public void Save(TextTier transcriptionTier)
		{
			var transcriptionSegments = transcriptionTier.GetAllSegments().Cast<ITextSegment>().ToList();

			for (int i = 0; i < transcriptionSegments.Count; i++)
			{
				SetTranscriptionTierAnnotationValue(transcriptionSegments[i].Id,
					transcriptionSegments[i].GetText());

				foreach (var dependentTier in transcriptionTier.DependentTiers.Where(t => t.DataType == TierType.Text))
				{
					var dependentSegment = dependentTier.GetSegment(i) as ITextSegment;

					SetDependentTierAnnotationValue(dependentTier.DisplayName,
						transcriptionSegments[i].Id, dependentSegment.Id,
						dependentSegment.GetText());
				}
			}

			Save();
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			var folder = Path.GetDirectoryName(EafFileName);
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			Root.Save(EafFileName);
		}

		/// ------------------------------------------------------------------------------------
		public void SetTranscriptionTierAnnotationValue(string transcriptionAnnotationId, string text)
		{
			var element = Root.Elements("TIER")
				.SingleOrDefault(e => e.Attribute("TIER_ID").Value.ToLower() == "transcription");

			if (element == null)
				return;

			element = element.Elements("ANNOTATION")
				.SingleOrDefault(e => e.Element("ALIGNABLE_ANNOTATION").Attribute("ANNOTATION_ID").Value == transcriptionAnnotationId);

			if (element == null)
				return;

			element.Element("ALIGNABLE_ANNOTATION").Element("ANNOTATION_VALUE").SetValue(text);
		}

		/// ------------------------------------------------------------------------------------
		public void SetDependentTierAnnotationValue(string dependentTierId,
			string transcriptionAnnotationId, string dependentAnnotationId, string text)
		{
			var tierElement = Root.Elements("TIER")
				.SingleOrDefault(e => e.Attribute("TIER_ID").Value.ToLower() == dependentTierId.ToLower());

			if (tierElement == null)
				return;

			var annElement = tierElement.Elements("ANNOTATION")
				.SingleOrDefault(e => e.Element("REF_ANNOTATION").Attribute("ANNOTATION_ID").Value == dependentAnnotationId);

			if (annElement != null)
				annElement.Element("REF_ANNOTATION").Element("ANNOTATION_VALUE").SetValue(text);
			else
			{
				tierElement.Add(new XElement("ANNOTATION",
					new XElement("REF_ANNOTATION",
					new XAttribute("ANNOTATION_ID", GetNextAvailableAnnotationIdAndIncrement()),
					new XAttribute("ANNOTATION_REF", transcriptionAnnotationId),
					new XElement("ANNOTATION_VALUE", text))));
			}
		}

		#endregion

		#region Methods for creating an annotation file.
		/// ------------------------------------------------------------------------------------
		public static string Create(string segmentFileName, string mediaFileName)
		{
			var newAnnotationFileName = mediaFileName + ".annotations.eaf";
			var isElanFile = GetIsElanFile(segmentFileName);

			File.Copy(isElanFile ? segmentFileName :
				FileLocator.GetFileDistributedWithApplication("annotationTemplate.etf"), newAnnotationFileName);

			ChangeMediaFileName(newAnnotationFileName, mediaFileName);

			if (!isElanFile)
			{
				var helper = new AudacityLabelHelper(File.ReadAllLines(segmentFileName), mediaFileName);
				CreateFromAudacityInfo(newAnnotationFileName, mediaFileName, helper.LabelInfo);
			}

			return newAnnotationFileName;
		}

		#region Methods for creating from Audacity label file
		/// ------------------------------------------------------------------------------------
		public static string CreateFromAudacityInfo(string newAnnotationFile,
			string mediaFileName, IEnumerable<AudacityLabelInfo> audacityLabels)
		{
			var eafFile = Load(newAnnotationFile);
			eafFile.SetMediaFile(mediaFileName);

			foreach (var label in audacityLabels)
				eafFile.AddNewTranscriptionAnnotationElement(label);

			eafFile.Save();
			return newAnnotationFile;
		}

		/// ------------------------------------------------------------------------------------
		public void AddNewTranscriptionAnnotationElement(AudacityLabelInfo labelInfo)
		{
			var timeSlotRef1 = CreateTimeOrderElementAndReturnId(labelInfo.Start);
			var timeSlotRef2 = CreateTimeOrderElementAndReturnId(labelInfo.Stop);

			GetTranscriptionTierElement().Add(new XElement("ANNOTATION",
				new XElement("ALIGNABLE_ANNOTATION",
					new XAttribute("ANNOTATION_ID", GetNextAvailableAnnotationIdAndIncrement()),
					new XAttribute("TIME_SLOT_REF1", timeSlotRef1),
					new XAttribute("TIME_SLOT_REF2", timeSlotRef2),
					new XElement("ANNOTATION_VALUE", labelInfo.Text))));
		}

		/// ------------------------------------------------------------------------------------
		public string CreateTimeOrderElementAndReturnId(float time)
		{
			var timeOrderElement = Root.Element("TIME_ORDER");

			var lastTimeSlotId = (timeOrderElement.LastNode == null ? 0 :
				int.Parse(((XElement)timeOrderElement.LastNode).Attribute("TIME_SLOT_ID").Value.Substring(2)));

			timeOrderElement.Add(new XElement("TIME_SLOT",
				new XAttribute("TIME_SLOT_ID", string.Format("ts{0}", ++lastTimeSlotId)),
				new XAttribute("TIME_VALUE", (int)(time * 1000))));

			return string.Format("ts{0}", lastTimeSlotId);
		}

		#endregion

		#endregion
	}
}
