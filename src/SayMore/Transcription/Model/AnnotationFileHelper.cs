using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Localization;
using Palaso.IO;
using Palaso.Reporting;
using SayMore.Properties;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class for managing annotation files. SayMore annotation files are the same as
	/// ELAN eaf files.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AnnotationFileHelper
	{
		public string AnnotationFileName { get; private set; }
		public XElement Root { get; private set; }

		private string _mediaFileName;

		#region Constructors and static methods for loading and verifying validity of file
		/// ------------------------------------------------------------------------------------
		private AnnotationFileHelper(string annotationFileName)
		{
			AnnotationFileName = annotationFileName;
			Root = XElement.Load(AnnotationFileName);

			if (Root.Name.LocalName != "ANNOTATION_DOCUMENT")
				Root = null;
		}

		/// ------------------------------------------------------------------------------------
		public AnnotationFileHelper(string annotationFileName, string mediaFileName)
		{
			AnnotationFileName = annotationFileName;
			_mediaFileName = mediaFileName;
		}

		/// ------------------------------------------------------------------------------------
		public static AnnotationFileHelper Load(string annotationFileName)
		{
			if (!File.Exists(annotationFileName))
			{
				throw new FileNotFoundException(string.Format(LocalizationManager.GetString(
					"EventsView.Transcription.AnnotationFileNotFoundMsg", "File not found: '{0}'"),
					annotationFileName));
			}

			if (!GetIsElanFile(annotationFileName))
			{
				var msg = LocalizationManager.GetString("EventsView.Transcription.AnnotationFileHelper.BadAnnotationFileMsg",
					"File '{0}' is not a SayMore annotation file. It is possibly corrupt.");

				throw new Exception(string.Format(msg, annotationFileName));
			}

			var helper = new AnnotationFileHelper(annotationFileName);

			// Ensure there is a dependent free translation tier.
			var elements = helper.GetDependentTiersElements();
			if (elements.FirstOrDefault(e =>
				e.Attribute("TIER_ID").Value.ToLower() == TextTier.ElanFreeTranslationTierName.ToLower()) == null)
			{
				helper.Root.Add(new XElement("TIER",
					new XAttribute("DEFAULT_LOCALE", "en"),
					new XAttribute("LINGUISTIC_TYPE_REF", "Translation"),
					new XAttribute("PARENT_REF", TextTier.TranscriptionTierName),
					new XAttribute("TIER_ID", TextTier.ElanFreeTranslationTierName)));

				helper.Save();
				helper = new AnnotationFileHelper(annotationFileName);
			}

			return helper;
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
			return Path.GetDirectoryName(AnnotationFileName);
		}

		#endregion

		#region Methods for reading/writing media file
		/// ------------------------------------------------------------------------------------
		public static void ChangeMediaFileName(string annotationFileName, string mediaFileName)
		{
			var eafFile = Load(annotationFileName);

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

			var element = header.Elements("PROPERTY")
				.Single(e => e.Attribute("NAME") != null && e.Attribute("NAME").Value == "lastUsedAnnotationId");

			int id;
			if (!int.TryParse(element.Value, out id))
				id = 0;

			element.SetValue(++id);
			return string.Format("a{0}", id);
		}

		/// ------------------------------------------------------------------------------------
		public void SetLastUsedAnnotationId(int id)
		{
			if (id <= 0)
			{
				var msg = "{0} is an invalid value for the last used annotation id. Must be greater than zero.";
				throw new ArgumentOutOfRangeException(string.Format(msg, id));
			}

			// Forces the header and lastUsedAnnotationId elements to get created.
			GetNextAvailableAnnotationIdAndIncrement();
			var header = Root.Element("HEADER");

			var elements = (from element in header.Elements("PROPERTY")
							let attrib = element.Attributes("NAME").FirstOrDefault(a => a.Value == "lastUsedAnnotationId")
							where attrib != null
							select element).ToArray();

			elements[0].SetValue(id);
		}

		/// ------------------------------------------------------------------------------------
		public void CorrectLastUsedAnnotationIdIfNecessary()
		{
			int id = 0;

			foreach (var tier in Root.Elements("TIER"))
			{
				foreach (var annotation in tier.Elements("ANNOTATION"))
				{
					foreach (var element in annotation.Elements())
					{
						var idAttrib = element.Attribute("ANNOTATION_ID");
						if (idAttrib == null)
							continue;

						int value;
						if (int.TryParse(idAttrib.Value.Replace("a", string.Empty), out value))
							id = Math.Max(id, value);
					}
				}
			}

			SetLastUsedAnnotationId(id);
		}

		#endregion

		#region Methods for time Order and time-alignable annotation elements
		/// ------------------------------------------------------------------------------------
		public void CreateTranscriptionElement(Segment seg)
		{
			var timeSlotRef1 = CreateTimeOrderElementAndReturnId(seg.Start);
			var timeSlotRef2 = CreateTimeOrderElementAndReturnId(seg.End);

			GetTranscriptionTierElement().Add(new XElement("ANNOTATION",
				new XElement("ALIGNABLE_ANNOTATION",
					new XAttribute("ANNOTATION_ID", GetNextAvailableAnnotationIdAndIncrement()),
					new XAttribute("TIME_SLOT_REF1", timeSlotRef1),
					new XAttribute("TIME_SLOT_REF2", timeSlotRef2),
					new XElement("ANNOTATION_VALUE", seg.Text))));
		}

		/// ------------------------------------------------------------------------------------
		public string CreateTimeOrderElementAndReturnId(float time)
		{
			var timeOrderElement = Root.Element("TIME_ORDER");

			var lastTimeSlotId = (timeOrderElement.LastNode == null ? 0 :
				int.Parse(((XElement)timeOrderElement.LastNode).Attribute("TIME_SLOT_ID").Value.Substring(2)));

			timeOrderElement.Add(new XElement("TIME_SLOT",
				new XAttribute("TIME_SLOT_ID", string.Format("ts{0}", ++lastTimeSlotId)),
				new XAttribute("TIME_VALUE", (int)Math.Round(time * 1000))));

			return string.Format("ts{0}", lastTimeSlotId);
		}

		#endregion

		#region Methods for getting/removing the time slots
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

		/// ------------------------------------------------------------------------------------
		public void RemoveTimeSlots()
		{
			var element = Root.Element("TIME_ORDER");
			if (element != null)
				element.RemoveAll();
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
			var ext = Path.GetExtension(_mediaFileName).ToLower();

			switch (ext)
			{
				case ".wav" : return "audio/x-wav";
				case ".mpg":
				case ".mpeg": return "video/mpeg";
			}

			return (Settings.Default.AudioFileExtensions.Contains(ext) ? "audio" : "video") + "/*";
		}

		#endregion

		#region Methods for creating SayMore tiers from the EAF file.
		/// ------------------------------------------------------------------------------------
		public TierCollection GetTierCollection()
		{
			var collection = new TierCollection();

			var timeSlots = GetTimeSlots();

			var transcriptionAnnotations = GetTranscriptionTierAnnotations();

			if (transcriptionAnnotations.Count == 0)
				return collection;

			var timeOrderTier = new TimeTier(GetFullPathToMediaFile());
			var textTier = new TextTier(TextTier.TranscriptionTierName) { TierType = TierType.Transcription };

			foreach (var kvp in transcriptionAnnotations)
			{
				var start = timeSlots[kvp.Value.Attribute("TIME_SLOT_REF1").Value];
				var stop = timeSlots[kvp.Value.Attribute("TIME_SLOT_REF2").Value];
				timeOrderTier.AddSegment(start, stop);
				textTier.AddSegment(kvp.Value.Value);
			}

			collection.Add(timeOrderTier);
			collection.Add(textTier);

			foreach (var tier in CreateDependentSayMoreTiers(transcriptionAnnotations.Keys))
				collection.Add(tier);

			return collection;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<TierBase> CreateDependentSayMoreTiers(IEnumerable<string> ids)
		{
			var annotationIds = ids.ToArray();

			foreach (var dependentTierElement in GetDependentTiersElements())
			{
				var depAnnotations = GetDependentTierAnnotationElements(dependentTierElement);
				var dependentTierName = dependentTierElement.Attribute("TIER_ID").Value;
				var dependentTier = dependentTierName != TextTier.ElanFreeTranslationTierName ?
					new TextTier(dependentTierName) :
					new TextTier(TextTier.SayMoreFreeTranslationTierName) { TierType = TierType.FreeTranslation };

				// Go through all the annotations in the transcription tier looking for
				// annotations in the dependent tier that reference the annotations in
				// the transcription tier.
				foreach (var id in annotationIds)
				{
					XElement depElement;
					dependentTier.AddSegment(depAnnotations.TryGetValue(id, out depElement) ?
						depElement.Element("ANNOTATION_VALUE").Value : string.Empty);
				}

				if (dependentTier.Segments.Any())
					yield return dependentTier;
			}
		}

		#endregion

		#region Methods for getting information from the EAF file.
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
				e.Attribute("TIER_ID") != null &&
				e.Attribute("TIER_ID").Value.ToLower() == TextTier.TranscriptionTierName.ToLower());

			if (element == null)
			{
				element = new XElement("TIER",
					new XAttribute("DEFAULT_LOCALE", "ipa-ext"),
					new XAttribute("LINGUISTIC_TYPE_REF", "Transcription"),
					new XAttribute("TIER_ID", TextTier.TranscriptionTierName));

				Root.Add(element);
			}

			return element;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetTranscriptionTierIds()
		{
			var transcriptionTier = GetTranscriptionTierElement();

			return transcriptionTier.Elements("ANNOTATION")
				.Select(e => e.Element("ALIGNABLE_ANNOTATION"))
				.Where(e => e != null).Select(aae => aae.Attribute("ANNOTATION_ID").Value);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<XElement> GetDependentTiersElements()
		{
			// Create a list of all tiers that reference the transcription tier.
			return Root.Elements().Where(e => e.Name.LocalName == "TIER" &&
				e.Attribute("PARENT_REF") != null &&
				e.Attribute("PARENT_REF").Value.ToLower() == TextTier.TranscriptionTierName.ToLower());
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

		#endregion

		#region Methods for modifying/saving EAF file
		/// ------------------------------------------------------------------------------------
		public void RemoveTiersAnnotations(string tierId)
		{
			var element = Root.Elements("TIER")
				.SingleOrDefault(e => e.Attribute("TIER_ID").Value.ToLower() == tierId.ToLower());

			if (element != null)
				element.RemoveNodes();
		}

		/// ------------------------------------------------------------------------------------
		public void SaveAnnotations(TierCollection collection)
		{
			// Remove all the dependent tiers from the EAF file,
			foreach (var dependentTier in collection.GetUserDefinedTextTiers())
				RemoveTiersAnnotations(dependentTier.DisplayName);

			// Remove the free translation dependent tier explicitly since
			// it's display name is not the same as it's tier id.
			RemoveTiersAnnotations(TextTier.ElanFreeTranslationTierName);

			var transcriptionSegments = collection.GetTranscriptionTier().Segments.ToArray();
			var freeTranslationSegments = collection.GetFreeTranslationTier().Segments.ToArray();

			// At this point, it's assumed that all the time-alignable annotations elements exist
			// in the parent tier (what SayMore calls the transcription tier) and that they are
			// empty. This will loop through those elements and update them with the transcriptions
			// while also writing annotations belonging to dependent tiers (e.g. free translation
			// tier.
			int i = 0;
			foreach (var id in GetTranscriptionTierIds())
			{
				SaveTranscriptionValue(id, transcriptionSegments[i].Text);

				// Save the free translation value associated with this transcription
				var text = (i < freeTranslationSegments.Length ? freeTranslationSegments[i].Text : string.Empty);
				SaveDependentAnnotationValue(id, TextTier.ElanFreeTranslationTierName, text);

				// Save values in other tiers associated with this transcription.
				foreach (var userDefTier in collection.GetUserDefinedTextTiers())
				{
					var segments = userDefTier.Segments.ToArray();
					text = (i < segments.Length ? segments[i].Text : string.Empty);
					SaveDependentAnnotationValue(id, userDefTier.DisplayName, text);
				}

				i++;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SaveTranscriptionValue(string id, string text)
		{
			var element = Root.Elements("TIER")
				.SingleOrDefault(e => e.Attribute("TIER_ID").Value.ToLower() == TextTier.TranscriptionTierName.ToLower());

			if (element == null)
				return;

			element = element.Elements("ANNOTATION")
				.SingleOrDefault(e => e.Element("ALIGNABLE_ANNOTATION").Attribute("ANNOTATION_ID").Value == id);

			if (element == null)
				return;

			element.Element("ALIGNABLE_ANNOTATION").SetElementValue("ANNOTATION_VALUE", text ?? string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public void SaveDependentAnnotationValue(string parentId, string dependentTierId, string text)
		{
			if (dependentTierId == TextTier.SayMoreFreeTranslationTierName)
				dependentTierId = TextTier.ElanFreeTranslationTierName;

			var tierElement = Root.Elements("TIER")
				.SingleOrDefault(e => e.Attribute("TIER_ID").Value.ToLower() == dependentTierId.ToLower());

			if (tierElement == null)
				return;

			var newId = GetNextAvailableAnnotationIdAndIncrement();
			tierElement.Add(new XElement("ANNOTATION",
				new XElement("REF_ANNOTATION",
				new XAttribute("ANNOTATION_ID", newId),
				new XAttribute("ANNOTATION_REF", parentId),
				new XElement("ANNOTATION_VALUE", text ?? string.Empty))));
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			var folder = Path.GetDirectoryName(AnnotationFileName);
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			Root.Save(AnnotationFileName);
		}

		/// ------------------------------------------------------------------------------------
		public static string Save(string mediaFileName, TierCollection collection)
		{
			var timeTier = collection.GetTimeTier();
			var eafFile = CreateFileFromSegments(null, mediaFileName, timeTier.Segments);
			var helper = Load(eafFile);
			helper.SaveAnnotations(collection);
			helper.Save();
			return eafFile;
		}

		#endregion

		#region Static methods for creating EAF file.
		/// ------------------------------------------------------------------------------------
		private static string ComputeEafFileNameFromMediaFileName(string mediaFileName)
		{
			return mediaFileName + ".annotations.eaf";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will create an EAF file from an existing EAF file or an Audacity
		/// label file. If creating from an existing EAF file, that EAF file is copied.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string CreateFileFromFile(string segmentFileName, string mediaFileName)
		{
			var isElanFile = GetIsElanFile(segmentFileName);

			var eafFile = ComputeEafFileNameFromMediaFileName(mediaFileName);
			File.Copy(isElanFile ? segmentFileName :
				FileLocator.GetFileDistributedWithApplication("annotationTemplate.etf"), eafFile);

			ChangeMediaFileName(eafFile, mediaFileName);

			if (!isElanFile)
			{
				var helper = new AudacityLabelHelper(File.ReadAllLines(segmentFileName), mediaFileName);
				CreateFileFromSegments(eafFile, mediaFileName, helper.Segments);
				UsageReporter.SendNavigationNotice("Annotations/Import segment file");
			}
			else
			{
				UsageReporter.SendNavigationNotice("Annotations/Import ELAN file");
			}

			return eafFile;
		}

		/// ------------------------------------------------------------------------------------
		public static string CreateFileFromTimesAsString(string eafFile, string mediaFileName,
			IEnumerable<string> times)
		{
			return CreateFileFromSegments(eafFile, mediaFileName, GetSegmentsFromTimeStrings(times));
		}

		/// ------------------------------------------------------------------------------------
		public static IEnumerable<Segment> GetSegmentsFromTimeStrings(IEnumerable<string> times)
		{
			var prevEnd = 0f;

			foreach (var t in times)
			{
				var seg = new Segment { Start = prevEnd };
				float seconds;
				seg.End = (float.TryParse(t.Trim(), out seconds) ? seconds : 0f);

				if (prevEnd >= seg.End)
				{
					var msg = "The end of a segment ({0}) may not be less than or equal to the end of the previous segment ({1})";
					throw new Exception(string.Format(msg, seg.End, prevEnd));
				}

				prevEnd = seg.End;
				yield return seg;
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string CreateFileFromSegments(string eafFile, string mediaFileName, IEnumerable<Segment> segments)
		{
			var fileAlreadyExisted = true;

			if (string.IsNullOrEmpty(eafFile))
			{
				eafFile = ComputeEafFileNameFromMediaFileName(mediaFileName);
				File.Copy(FileLocator.GetFileDistributedWithApplication("annotationTemplate.etf"), eafFile, true);
				ChangeMediaFileName(eafFile, mediaFileName);
				fileAlreadyExisted = false;
			}

			var helper = Load(eafFile);

			if (fileAlreadyExisted)
			{
				helper.RemoveTimeSlots();
				helper.CorrectLastUsedAnnotationIdIfNecessary();
			}

			foreach (var seg in segments.ToArray())
				helper.CreateTranscriptionElement(seg);

			helper.SetMediaFile(mediaFileName);
			helper.Save();
			return eafFile;
		}

		#endregion
	}
}
