using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DesktopAnalytics;
using L10NSharp;
using Palaso.IO;
using Palaso.Reporting;
using Palaso.Xml;
using SayMore.Media;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Utilities;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class for managing annotation files. SayMore annotation files are the same as
	/// ELAN eaf files.
	///
	/// A nice presentation on the ELAN format is available here: http://pubman.mpdl.mpg.de/pubman/item/escidoc:131150:5/component/escidoc:135959/ELAN_Augsburg.pdf
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AnnotationFileHelper
	{
		public string AnnotationFileName { get; private set; }
		public XElement Root { get; private set; }

		public static string kAnnotationsEafFileSuffix = ".annotations" + Settings.Default.AnnotationFileExtension.ToLower();

		private string _mediaFileName;
		private string _videoFileName;

		#region Constructors and static methods for loading and verifying validity of file
		/// ------------------------------------------------------------------------------------
		private AnnotationFileHelper(string annotationFileName)
		{
			AnnotationFileName = annotationFileName;
			Root = XElement.Load(AnnotationFileName);

			if (Root.Name.LocalName != "ANNOTATION_DOCUMENT")
				Root = null;
			else
				CorrectLastUsedAnnotationIdIfNecessary();
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
					"SessionsView.Transcription.AnnotationFileNotFoundMsg", "File not found: '{0}'"),
					annotationFileName));
			}

			if (!GetIsElanFile(annotationFileName))
			{
				var msg = LocalizationManager.GetString(
					"SessionsView.Transcription.BadAnnotationFileMsg",
					"File '{0}' is not a SayMore annotation file. It is possibly corrupt.");

				throw new Exception(string.Format(msg, annotationFileName));
			}

			return new AnnotationFileHelper(annotationFileName);
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsElanFile(string fileName)
		{
			try
			{
				var root = XElement.Load(fileName);
				return root.Name.LocalName == "ANNOTATION_DOCUMENT";
			}
			catch (IOException)
			{
				throw;
			}
			catch
			{
			}

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

		#region Methods for getting/creating a header element (including media descriptor element)
		/// ------------------------------------------------------------------------------------
		public XElement GetOrCreateHeader()
		{
			var header = Root.Element("HEADER");
			if (header == null)
			{
				header = new XElement("HEADER",
					new XAttribute("MEDIA_FILE", string.Empty),
					new XAttribute("TIME_UNITS", "milliseconds"));
				AddMediaDescriptorElements(header);
				Root.Add(header);
			}
			return header;
		}

		/// ------------------------------------------------------------------------------------
		public void AddMediaDescriptorElements(XElement header)
		{
			Debug.Assert(header.Element("MEDIA_DESCRIPTOR") == null);

			var videoFileName = VideoFileName;

			var element = new XElement("MEDIA_DESCRIPTOR");

			if (videoFileName != null)
				AddMediaInfoToDescriptorElement(element, videoFileName);
			else if (_mediaFileName != null)
				AddMediaInfoToDescriptorElement(element, _mediaFileName);

			header.Add(element);

			if (videoFileName != null && videoFileName != _mediaFileName)
			{
				element = new XElement("MEDIA_DESCRIPTOR");
				element.Add(new XAttribute("EXTRACTED_FROM", videoFileName));
				AddMediaInfoToDescriptorElement(element, _mediaFileName);
				header.Add(element);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void AddMediaInfoToDescriptorElement(XElement element, string fileName)
		{
			fileName = Path.GetFileName(fileName);
			Debug.Assert(fileName != null);
			element.Add(new XAttribute("MEDIA_URL", fileName),
					new XAttribute("MIME_TYPE", GetMediaFileMimeType(fileName)));
		}

		/// ------------------------------------------------------------------------------------
		public string GetMediaFileMimeType()
		{
			return GetMediaFileMimeType(_mediaFileName);
		}

		/// ------------------------------------------------------------------------------------
		private string GetMediaFileMimeType(string mediaFileName)
		{
			var ext = Path.GetExtension(mediaFileName).ToLower();

			switch (ext)
			{
				case ".wav":
					return "audio/x-wav";
				case ".mpg":
				case ".mpeg":
					return "video/mpeg";
			}

			return (FileUtils.AudioFileExtensions.Contains(ext) ? "audio" : "video") + "/*";
		}
		#endregion

		#region Methods for reading/writing media file information
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
			var mediaFileName = MediaFileName;
			return (mediaFileName == null || Path.IsPathRooted(mediaFileName) ?
				mediaFileName : Path.Combine(GetAnnotationFolderPath(), Path.GetFileName(mediaFileName)));
		}

		/// ------------------------------------------------------------------------------------
		public string GetFullPathToVideoFile()
		{
			var videoFileName = VideoFileName;
			return (videoFileName == null || Path.IsPathRooted(videoFileName) ?
				videoFileName : Path.Combine(GetAnnotationFolderPath(), Path.GetFileName(videoFileName)));
		}

		/// ------------------------------------------------------------------------------------
		public string MediaFileName
		{
			get
			{
				if (_mediaFileName == null)
				{
					var element = Root.Element("HEADER");
					if (element == null)
						return null;

					var descriptors = element.Elements("MEDIA_DESCRIPTOR");
					if (!descriptors.Any())
						return null;

					element = descriptors.FirstOrDefault(d => d.Attributes("EXTRACTED_FROM").Any()) ?? descriptors.First();

					var mediaUrl = element.Attribute("MEDIA_URL");
					if (mediaUrl != null)
						_mediaFileName = mediaUrl.Value;
				}
				return _mediaFileName;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string VideoFileName
		{
			get
			{
				if (_videoFileName == null && MediaFileName != null)
				{
					var mediaFileExt = Path.GetExtension(_mediaFileName);
					if (mediaFileExt != null)
						mediaFileExt = mediaFileExt.ToLowerInvariant();
					if (FileUtils.VideoFileExtensions.Contains(mediaFileExt))
						_videoFileName = _mediaFileName;
					else
					{
						// Media file is an audio file.

						if (Root != null)
						{
							var header = Root.Element("HEADER");
							if (header != null)
							{
								var extractedFromDescriptor = header.Elements("MEDIA_DESCRIPTOR")
									.SingleOrDefault(e => e.Attribute("EXTRACTED_FROM") != null);
								if (extractedFromDescriptor != null)
								{
									var mediaUrl = extractedFromDescriptor.Attribute("MEDIA_URL");
									if (mediaUrl != null)
									{
										_videoFileName = mediaUrl.Value;
										if (FileUtils.VideoFileExtensions.Contains(Path.GetExtension(_videoFileName)))
											return _videoFileName;
										_videoFileName = null;
									}
								}
							}
						}

						// No video (MEDIA_DESCRIPTOR with EXTRACTED_FROM attribute) specified in
						// the file. See if corresponding video file exists.
						if (AudioVideoFileTypeBase.GetIsStandardPcmAudioFile(_mediaFileName))
						{
							string videoFilePath = GetFullPathToMediaFile();
							videoFilePath = videoFilePath.Remove(videoFilePath.Length - Settings.Default.StandardAudioFileSuffix.Length) +
								FileUtils.VideoFileExtensions[0];
							foreach (var videoExt in FileUtils.VideoFileExtensions)
							{
								videoFilePath = Path.ChangeExtension(videoFilePath, videoExt);
								if (File.Exists(videoFilePath))
								{
									_videoFileName = Path.GetFileName(videoFilePath);
									break;
								}
							}
						}
					}
				}
				return _videoFileName;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetMediaFile(string mediaFileName)
		{
			_mediaFileName = mediaFileName;

			var header = Root.Element("HEADER");
			if (header == null)
			{
				GetOrCreateHeader();
				return;
			}

			XElement descriptor;
			while ((descriptor = header.Element("MEDIA_DESCRIPTOR")) != null)
				descriptor.Remove();

			AddMediaDescriptorElements(header);
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
			if (id < 0)
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

		#region Methods for creating transcription and free translation annotation elements
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates an annotation element in the transcription tier for the specified segment
		/// and returns the id of the annotation added.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CreateTranscriptionAnnotationElement(Segment seg)
		{
			var timeSlotRef1 = GetOrCreateTimeOrderElementAndReturnId(seg.Start);
			var timeSlotRef2 = GetOrCreateTimeOrderElementAndReturnId(seg.End);

			var annotationId = GetNextAvailableAnnotationIdAndIncrement();

			GetOrCreateTranscriptionTierElement().Add(new XElement("ANNOTATION",
				new XElement("ALIGNABLE_ANNOTATION",
					new XAttribute("ANNOTATION_ID", annotationId),
					new XAttribute("TIME_SLOT_REF1", timeSlotRef1),
					new XAttribute("TIME_SLOT_REF2", timeSlotRef2),
					new XElement("ANNOTATION_VALUE", XmlUtils.SanitizeString(seg.Text)))));

			return annotationId;
		}

		/// ------------------------------------------------------------------------------------
		public void CreateFreeTranslationAnnotationElement(string parentAnnotationId, string text)
		{
			var newId = GetNextAvailableAnnotationIdAndIncrement();

			GetOrCreateFreeTranslationTierElement().Add(new XElement("ANNOTATION",
				new XElement("REF_ANNOTATION",
				new XAttribute("ANNOTATION_ID", newId),
				new XAttribute("ANNOTATION_REF", parentAnnotationId),
				new XElement("ANNOTATION_VALUE", text == null ? string.Empty : XmlUtils.SanitizeString(text)))));
		}

		#endregion

		#region Methods for getting/removing the time slots
		/// ------------------------------------------------------------------------------------
		public IDictionary<string, float> GetTimeSlots()
		{
			var element = Root.Element("TIME_ORDER");
			if (element == null)
			{
				Root.Element("HEADER").AddAfterSelf(new XElement("TIME_ORDER"));
				element = Root.Element("TIME_ORDER");
			}

			// Put all the time slot ids and their values in an array of key/value pairs. Sometimes,
			// some of the time values may not be there if the user used ELAN to make "Regular
			// Annotations". Those are n number of segments created between a start and stop time.
			// the number of those segments is determined by how many slots there are without
			// time values.
			var kvpList = (from tse in element.Elements("TIME_SLOT")
						   let id = tse.Attribute("TIME_SLOT_ID").Value
						   let timeVal = (tse.Attribute("TIME_VALUE") == null ? -1 :
								 (float)(int.Parse(tse.Attribute("TIME_VALUE").Value) / 1000d))
						   select new KeyValuePair<string, float>(id, timeVal)).ToArray();

			// Now go through the list and fill-in any times whose values were not included because
			// there were "Regular Annotations". Those values will be -1 in the array of key/value
			// pairs just created.
			int endTimeIndex = -1;
			for (int i = kvpList.Length - 1; i >= 0; i--)
			{
				if (kvpList[i].Value >= 0 && endTimeIndex < 0)
					endTimeIndex = i;
				else if (kvpList[i].Value >= 0 && endTimeIndex >= 0)
				{
					var startTime = kvpList[i].Value;
					var timeDiff = (kvpList[endTimeIndex].Value - startTime) / (endTimeIndex - i);
					for (var emptyIndex = i + 1; emptyIndex < endTimeIndex; emptyIndex++)
					{
						startTime += timeDiff;
						kvpList[emptyIndex] = new KeyValuePair<string, float>(kvpList[emptyIndex].Key, startTime);
					}

					endTimeIndex = -1;
				}
			}

			return kvpList.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}

		/// ------------------------------------------------------------------------------------
		public string GetOrCreateTimeOrderElementAndReturnId(float time)
		{
			var timeSlots = GetTimeSlots();

#if combineDuplicateSlots // see http://jira.palaso.org/issues/browse/SP-597
			var slot = timeSlots.FirstOrDefault(kvp => kvp.Value.Equals(time));
			if (!slot.Equals(default(KeyValuePair<string, float>)))
				return slot.Key;
#endif
			var lastTimeSlotId = (timeSlots.Count == 0 ? 0 :
				timeSlots.Keys.Max(id => int.Parse(id.Substring(2))));

			Root.Element("TIME_ORDER").Add(new XElement("TIME_SLOT",
				new XAttribute("TIME_SLOT_ID", string.Format("ts{0}", ++lastTimeSlotId)),
				new XAttribute("TIME_VALUE", (int)Math.Round(time * 1000))));

			return string.Format("ts{0}", lastTimeSlotId);
		}

		/// ------------------------------------------------------------------------------------
		public void RemoveTimeSlots()
		{
			// Only remove time slots when there are no time subdivision tiers dependent on
			// the transcription tier. If there is, there could be a lot of time slots that
			// are not referenced by the transcription annotations. In that case we don't
			// want to lose those slots.
			if (!GetDoesTranscriptionTierHaveDepedentTimeSubdivisionTier())
			{
				var element = Root.Element("TIME_ORDER");
				if (element != null)
					element.RemoveAll();
			}
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

			var freeTransAnnotations = GetFreeTranslationTierAnnotations();

			EnsureMediaFileIsCorrect();

			var timeOrderTier = new TimeTier(GetFullPathToMediaFile());
			var transcriptionTier = new TextTier(TextTier.ElanTranscriptionTierId);
			var freeTransTier = new TextTier(TextTier.ElanTranslationTierId);

			foreach (var kvp in transcriptionAnnotations)
			{
				var start = timeSlots[kvp.Value.Attribute("TIME_SLOT_REF1").Value];
				var stop = timeSlots[kvp.Value.Attribute("TIME_SLOT_REF2").Value];
				timeOrderTier.AddSegment(start, stop);
				transcriptionTier.AddSegment(kvp.Value.Value);

				string freeTransValue;
				freeTransTier.AddSegment(freeTransAnnotations.TryGetValue(kvp.Key,
					out freeTransValue) ? freeTransValue : string.Empty);
			}

			// Add the time and transcription tiers to the collection.
			collection.Add(timeOrderTier);
			collection.Add(transcriptionTier);
			collection.Add(freeTransTier);

			timeOrderTier.ReadOnlyTimeRanges = GetDoesTranscriptionTierHaveDepedentTimeSubdivisionTier();

			return collection;
		}

		/// ------------------------------------------------------------------------------------
		private void EnsureMediaFileIsCorrect()
		{
			var mediaFilePath = GetFullPathToMediaFile();
			var sessionFolder = Path.GetDirectoryName(AnnotationFileName);
			Debug.Assert(sessionFolder != null);
			if (Path.GetDirectoryName(mediaFilePath) == sessionFolder &&
				ComputeEafFileNameFromOralAnnotationFile(mediaFilePath) == AnnotationFileName)
				return;
			if (!AnnotationFileName.EndsWith(kAnnotationsEafFileSuffix))
				return;
			// REVIEW: Should we prompt the user before making this fix. This is a very unlikely
			// scenario (see SP-698), and it's almost certainly the right thing to do, but is there
			// possibly some situation when the user wouldn't want this? Specifically, do we need
			// to consider the scenario where the EAF file points to a valid (existing) media file?
			// In this situation, I don't think anything would crash, but the program could behave
			// rather badly since it would assume that the information (duration, etc.) pulled from
			// the media file refrenced in the EAF file applied to the media file in the session.
			var eafFileName = Path.GetFileName(AnnotationFileName);
			Debug.Assert(eafFileName != null);
			var mediaFileName = eafFileName.Remove(eafFileName.Length - kAnnotationsEafFileSuffix.Length);
			if (File.Exists(Path.Combine(sessionFolder, mediaFileName)))
			{
				Analytics.Track("AnnotationFileHelper EnsureMediaFileIsCorrect Automatic repair of Media File URL",
					new Dictionary<string, string> {
						{"_mediaFileName",  _mediaFileName},
						{"AnnotationFileName",  AnnotationFileName}
					});

				SetMediaFile(mediaFileName);
				Save();
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesTranscriptionTierHaveDepedentTimeSubdivisionTier()
		{
			return Root.Elements("TIER").Where(e => e.Attribute("TIER_ID") != null &&
				e.Attribute("TIER_ID").Value.ToLower() != TextTier.ElanTranscriptionTierId.ToLower())
				.Elements("ANNOTATION").Any(e => e.Element("ALIGNABLE_ANNOTATION") != null);
		}

		#endregion

		#region Methods for getting transcription tier element and annotation information
		/// ------------------------------------------------------------------------------------
		public IDictionary<string, XElement> GetTranscriptionTierAnnotations()
		{
			var transcriptionTierElement = GetOrCreateTranscriptionTierElement();

			if (transcriptionTierElement == null)
				return new Dictionary<string, XElement>();

			return transcriptionTierElement.Elements("ANNOTATION")
				.Select(e => e.Element("ALIGNABLE_ANNOTATION"))
				.ToDictionary(e => e.Attribute("ANNOTATION_ID").Value, e => e);
		}

		/// ------------------------------------------------------------------------------------
		public XElement GetOrCreateTranscriptionTierElement()
		{
			var element = Root.Elements().FirstOrDefault(e => e.Name.LocalName == "TIER" &&
				e.Attribute("TIER_ID") != null &&
				e.Attribute("TIER_ID").Value.ToLower() == TextTier.ElanTranscriptionTierId.ToLower());

			if (element == null)
			{
				element = new XElement("TIER",
					new XAttribute("DEFAULT_LOCALE", "ipa-ext"),
					new XAttribute("LINGUISTIC_TYPE_REF", "Transcription"),
					new XAttribute("TIER_ID", TextTier.ElanTranscriptionTierId));

				Root.Add(element);
			}

			return element;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetTranscriptionTierAnnotationIds()
		{
			var transcriptionTier = GetOrCreateTranscriptionTierElement();

			return transcriptionTier.Elements("ANNOTATION")
				.Select(e => e.Element("ALIGNABLE_ANNOTATION"))
				.Where(e => e != null).Select(aae => aae.Attribute("ANNOTATION_ID").Value);
		}

		#endregion

		#region Methods for getting free translation tier element and annotation elements
		/// ------------------------------------------------------------------------------------
		public XElement GetOrCreateFreeTranslationTierElement()
		{
			var element = Root.Elements().FirstOrDefault(e => e.Name.LocalName == "TIER" &&
				e.Attribute("TIER_ID") != null &&
				e.Attribute("TIER_ID").Value.ToLower() == TextTier.ElanTranslationTierId.ToLower());

			if (element == null)
			{
				element = new XElement("TIER",
					new XAttribute("LINGUISTIC_TYPE_REF", "Transcription"),
					new XAttribute("PARENT_REF", TextTier.ElanTranscriptionTierId),
					new XAttribute("TIER_ID", TextTier.ElanTranslationTierId));

				Root.Add(element);
			}

			return element;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This returns -- from the annotation file -- a dictionary of free translation
		/// values keyed on the transcription annotation id to which the free translation
		/// refers (i.e. the free translation's parent transcription).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IDictionary<string, string> GetFreeTranslationTierAnnotations()
		{
			var freeTransAnnotationElements = GetOrCreateFreeTranslationTierElement().Elements("ANNOTATION").ToArray();

			return (from e in freeTransAnnotationElements
					let re = e.Element("REF_ANNOTATION")
					where re != null
					select re).ToDictionary(re => re.Attribute("ANNOTATION_REF").Value,
											re => re.Element("ANNOTATION_VALUE").Value);
		}

		#endregion

		#region Methods for modifying/saving EAF file
		/// ------------------------------------------------------------------------------------
		public void RemoveAnnotationsFromTier(string tierId)
		{
			var element = Root.Elements("TIER")
				.SingleOrDefault(e => e.Attribute("TIER_ID").Value.ToLower() == tierId.ToLower());

			if (element != null)
				element.RemoveNodes();
		}

		/// ------------------------------------------------------------------------------------
		private void SaveFromTierCollection(TierCollection collection)
		{
			RemoveTimeSlots();
			RemoveAnnotationsFromTier(TextTier.ElanTranscriptionTierId);
			RemoveAnnotationsFromTier(TextTier.ElanTranslationTierId);

			var timeTier = collection.GetTimeTier();
			if (timeTier == null)
				return;

			var timeSegments = timeTier.Segments.ToArray();
			if (timeSegments.Length == 0)
				return;

			var transcriptionSegments = collection.GetTranscriptionTier(true).Segments.ToArray();
			var freeTranslationSegments = collection.GetFreeTranslationTier(true).Segments.ToArray();

			for (int i = 0; i < timeSegments.Length; i++)
			{
				var annotationId = CreateTranscriptionAnnotationElement(new Segment
				{
					Start = timeSegments[i].Start,
					End = timeSegments[i].End,
					Text = (i < transcriptionSegments.Length ? transcriptionSegments[i].Text : string.Empty)
				});

				var text = (i < freeTranslationSegments.Length ? freeTranslationSegments[i].Text : string.Empty);
				CreateFreeTranslationAnnotationElement(annotationId, text);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void SaveFromSegments(IEnumerable<Segment> segments)
		{
			RemoveTimeSlots();
			RemoveAnnotationsFromTier(TextTier.ElanTranscriptionTierId);
			RemoveAnnotationsFromTier(TextTier.ElanTranslationTierId);

			var mediaInfo = MediaFileInfo.GetInfo(_mediaFileName);

			// Don't add any segments that extend beyond the end of the media file.
			foreach (var seg in segments.Where(s => s.Start <= mediaInfo.Audio.DurationInSeconds))
			{
				if (seg.End > mediaInfo.Audio.DurationInSeconds)
					seg.End = mediaInfo.Audio.DurationInSeconds;

				CreateTranscriptionAnnotationElement(seg);
			}

			Save();
		}

		/// ------------------------------------------------------------------------------------
		private void Save()
		{
			var folder = Path.GetDirectoryName(AnnotationFileName);
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			try
			{
				// SP-702: file is being used by another process
				FileSystemUtils.WaitForFileRelease(AnnotationFileName, true);
				Root.Save(AnnotationFileName);
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalException(e);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string Save(string mediaFileName, TierCollection collection)
		{
			var helper = GetOrCreateFile(null, mediaFileName);
			helper.SaveFromTierCollection(collection);
			helper.Save();
			return helper.AnnotationFileName;
		}

		#endregion

		#region Static methods for creating EAF file.
		/// ------------------------------------------------------------------------------------
		public static string ComputeEafFileNameFromOralAnnotationFile(string mediaFileName)
		{
			return mediaFileName + kAnnotationsEafFileSuffix;
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
			var eafFile = ComputeEafFileNameFromOralAnnotationFile(mediaFileName);
			File.Copy(isElanFile ? segmentFileName :
				FileLocator.GetFileDistributedWithApplication("annotationTemplate.etf"), eafFile);

			var helper = GetOrCreateFile(eafFile, mediaFileName);

			if (!isElanFile)
			{
				var labelHelper = new AudacityLabelHelper(File.ReadAllLines(segmentFileName), mediaFileName);
				helper.SaveFromSegments(labelHelper.Segments);
				Analytics.Track("AnnotationFileHelper Import segment file");
			}
			else
			{
				Analytics.Track("AnnotationFileHelper Import ELAN file");
			}

			return helper.AnnotationFileName;
		}

		/// ------------------------------------------------------------------------------------
		public static AnnotationFileHelper GetOrCreateFile(string eafFile, string mediaFileName)
		{
			if (string.IsNullOrEmpty(eafFile))
				eafFile = ComputeEafFileNameFromOralAnnotationFile(mediaFileName);

			var fileExisted = File.Exists(eafFile);

			if (!fileExisted)
			{
				File.Copy(FileLocator.GetFileDistributedWithApplication("annotationTemplate.etf"), eafFile, true);
				ChangeMediaFileName(eafFile, mediaFileName);
			}

			var helper = Load(eafFile);
			helper.SetMediaFile(mediaFileName);

			if (!fileExisted)
			{
				helper.GetOrCreateHeader();
				helper.GetOrCreateTranscriptionTierElement();
				helper.GetOrCreateFreeTranslationTierElement();
			}

			helper.Save();
			return helper;
		}

		#endregion
	}
}
