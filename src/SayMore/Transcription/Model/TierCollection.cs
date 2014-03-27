using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SayMore.Properties;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class TierCollection : Collection<TierBase>
	{
		public const string kIgnoreSegment = "%ignore%";

		public bool PreventSegmentBoundaryMovingWhereTextAnnotationsAreAdjacent { get; set; }

		/// ------------------------------------------------------------------------------------
		public static TierCollection LoadFromAnnotationFile(string eafFilePath)
		{
			var helper = AnnotationFileHelper.Load(eafFilePath);
			var collection = helper.GetTierCollection();
			collection.AnnotatedMediaFile = helper.GetFullPathToMediaFile();

			return collection;
		}

		/// ------------------------------------------------------------------------------------
		public TierCollection()
		{
			PreventSegmentBoundaryMovingWhereTextAnnotationsAreAdjacent =
				Settings.Default.PreventSegmentBoundaryMovingWhereTextAnnotationsAreAdjacent;
		}

		/// ------------------------------------------------------------------------------------
		public TierCollection(string annotatedMediaFile)
		{
			AnnotatedMediaFile = annotatedMediaFile;
			Add(new TimeTier(annotatedMediaFile));
			Add(new TextTier(TextTier.ElanTranscriptionTierId));
			Add(new TextTier(TextTier.ElanTranslationTierId));
		}

		/// ------------------------------------------------------------------------------------
		public string AnnotatedMediaFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		public TierCollection Copy()
		{
			var copy = new TierCollection();

			copy.AnnotatedMediaFile = AnnotatedMediaFile;

			foreach (var tier in this)
				copy.Add(tier.Copy());

			return copy;
		}

		/// ------------------------------------------------------------------------------------
		public void AddTextTierWithEmptySegments(string id)
		{
			var newTier = new TextTier(id);
			Add(newTier);

			for (int i = 0; i < GetTimeTier().Segments.Count; i++)
				newTier.AddSegment(string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoTimeSegmentsExist()
		{
			return (GetTimeTier() != null && GetTimeTier().Segments.Count > 0);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsFullyAnnotated(OralAnnotationType type)
		{
			var timeTier = GetTimeTier();
			if (timeTier == null || !timeTier.IsFullySegmented)
				return false;

			return GetCountOfContiguousAnnotatedSegments(type) == timeTier.Segments.Count;
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsAdequatelyAnnotated(OralAnnotationType type)
		{
			var timeTier = GetTimeTier();
			if (timeTier == null)
				return false;

			bool fullySegmented = timeTier.IsFullySegmented;
			// If not fully segmented, you must have more than 1 segment per minute and be
			// segmented to within the final 10 seconds of the recording.
			if (!fullySegmented &&
				(timeTier.Segments.Count <= timeTier.TotalTime.TotalSeconds / 60 ||
				timeTier.EndOfLastSegment.TotalSeconds + 10 < timeTier.TotalTime.TotalSeconds))
			{
				return false;
			}

			// If fully segmented and last segment isn't annotated, that's okay (as long as
			// there is more than 1 segment)
			int requiredAnnotations = timeTier.Segments.Count;
			if (fullySegmented && requiredAnnotations > 1)
				requiredAnnotations--;
			return GetCountOfContiguousAnnotatedSegments(type) >= requiredAnnotations;
		}

		/// ------------------------------------------------------------------------------------
		private int GetCountOfContiguousAnnotatedSegments(OralAnnotationType type)
		{
			var timeTier = GetTimeTier();
			var transcriptionTier = GetTranscriptionTier();
			Segment transcriptionSegment;
			Func<Segment, string> getPathToAnnotationFile = (type == OralAnnotationType.CarefulSpeech) ?
				timeTier.GetFullPathToCarefulSpeechFile :
				(Func<Segment, string>)timeTier.GetFullPathToOralTranslationFile;

			int iSegment = 0;
			while (iSegment < timeTier.Segments.Count && ((transcriptionTier != null &&
				transcriptionTier.TryGetSegment(iSegment, out transcriptionSegment) &&
				SegmentIsIgnored(transcriptionSegment)) ||
				File.Exists(getPathToAnnotationFile(timeTier.Segments[iSegment]))))
			{
				iSegment++;
			}

			return iSegment;
		}

		/// ------------------------------------------------------------------------------------
		private bool SegmentIsIgnored(Segment transcriptionSegment)
		{
			return transcriptionSegment.Text == kIgnoreSegment
				|| transcriptionSegment.Text == "%junk%" /*old indicator*/;
		}

		/// ------------------------------------------------------------------------------------
		public void MarkSegmentAsIgnored(int iSegment)
		{
			GetTranscriptionTier().Segments[iSegment].Text = kIgnoreSegment;
		}

		/// ------------------------------------------------------------------------------------
		public void MarkSegmentAsUnignored(int iSegment)
		{
			GetTranscriptionTier().Segments[iSegment].Text = string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		public void AddIgnoredSegment(float boundary)
		{
			var timeTier = GetTimeTier();

			timeTier.AppendSegment(boundary);
			//var i = timeTier.GetIndexOfSegment(newSeg);

			var transcriptionTier = GetTranscriptionTier();
			foreach (var tier in this.OfType<TextTier>())
			{
				var text = (tier == transcriptionTier) ? kIgnoreSegment : string.Empty;
				tier.Segments.Add(new Segment(tier, text));
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsSegmentIgnored(int segmentIndex)
		{
			var transcriptionTier = GetTranscriptionTier();
			return transcriptionTier != null && segmentIndex >= 0 &&
				segmentIndex < transcriptionTier.Segments.Count &&
				SegmentIsIgnored(transcriptionTier.Segments[segmentIndex]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the segment in each tier at the specified index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool RemoveTierSegments(int index)
		{
			if (Count == 0 || index < 0 || index >= this[0].Segments.Count)
				return false;

			for (int i = 0; i < Count; i++)
				this[i].RemoveSegment(index);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public BoundaryModificationResult InsertTierSegment(float boundary,
			Func<Segment, bool, bool, bool> allowDeletionOfOralAnnotations = null)
		{
			var timeTier = GetTimeTier();

			if (boundary <= 0f || timeTier == null || timeTier.GetSegmentHavingEndBoundary(boundary) != null)
				return BoundaryModificationResult.SegmentWillBeTooShort;

			var result = timeTier.InsertSegmentBoundary(boundary, allowDeletionOfOralAnnotations);
			if (result == BoundaryModificationResult.Success)
			{
				var newSeg = timeTier.GetSegmentHavingEndBoundary(boundary);
				var i = timeTier.GetIndexOfSegment(newSeg);

				// If this segment is ignored, then this is a split of an existing ignored segment.
				var transcription = GetIsSegmentIgnored(i) ? kIgnoreSegment : string.Empty;
				foreach (var tier in this.OfType<TextTier>())
				{
					tier.Segments.Insert(i, new Segment(tier,
						(tier.TierType == TierType.Transcription) ? transcription : string.Empty));
				}
			}
			return result;
		}

		/// ------------------------------------------------------------------------------------
		public bool HasAdjacentAnnotation(float boundary)
		{
			var timeTier = GetTimeTier();
			if (!timeTier.Segments.Any())
				return false;

			var segment = timeTier.GetSegmentHavingEndBoundary(boundary);
			if (segment == null && TimeSpan.FromSeconds(boundary) > timeTier.Segments.Last().TimeRange.End)
				return false;

			var i = timeTier.GetIndexOfSegment(segment);

			if (PreventSegmentBoundaryMovingWhereTextAnnotationsAreAdjacent)
			{
				// ReSharper disable once LoopCanBeConvertedToQuery
				foreach (TextTier textTier in this.OfType<TextTier>())
				{
					var segments = textTier.Segments;
					if (segments == null || i < 0 || segments.Count <= i) continue;
					if (!string.IsNullOrEmpty(segments[i].Text) ||
						segments.Count > i + 1 && !string.IsNullOrEmpty(segments[i + 1].Text))
						return true;
				}
			}

			// SP-891: Object reference not set to an instance of an object
			if (segment != null)
			{
				if (File.Exists(segment.GetFullPathToCarefulSpeechFile()) ||
					File.Exists(segment.GetFullPathToOralTranslationFile()))
					return true;
			}

			if (timeTier.Segments.Count > i + 1)
			{
				segment = timeTier.Segments[i + 1];
				return segment.GetHasAnyOralAnnotation();
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetTotalAnnotatedTime(TextAnnotationType type)
		{
			TextTier textTier = (type == TextAnnotationType.Transcription ? GetTranscriptionTier() :
				GetFreeTranslationTier());
			TimeSpan totalTime = TimeSpan.Zero;

			// ReSharper disable once LoopCanBeConvertedToQuery
			for (var i = 0; i < GetTimeTier().Segments.Count; i++)
			{
				if (!string.IsNullOrEmpty(textTier.Segments[i].Text))
					totalTime += GetTimeTier().Segments[i].TimeRange.Duration;
			}

			return totalTime;
		}

		#region Methods for Saving tiers to EAF file
		/// ------------------------------------------------------------------------------------
		public string Save()
		{
			Debug.Assert(AnnotatedMediaFile != null);
			Debug.Assert(File.Exists(AnnotatedMediaFile));
			return Save(AnnotatedMediaFile);
		}

		/// ------------------------------------------------------------------------------------
		public string Save(string annotatedMediaFile)
		{
			return AnnotationFileHelper.Save(annotatedMediaFile, this);
		}

		#endregion

		#region Methods for getting various tiers
		/// ------------------------------------------------------------------------------------
		public TimeTier GetTimeTier()
		{
			return this.FirstOrDefault(t => t.TierType == TierType.Time) as TimeTier;
		}

		/// ------------------------------------------------------------------------------------
		public TextTier GetTranscriptionTier(bool createEmptyIfNotExist = false)
		{
			var tier = this.FirstOrDefault(t => t.TierType == TierType.Transcription) as TextTier;
			return (tier == null && createEmptyIfNotExist ?
				new TextTier(TextTier.ElanTranscriptionTierId) : tier);
		}

		/// ------------------------------------------------------------------------------------
		public TextTier GetFreeTranslationTier(bool createEmptyIfNotExist = false)
		{
			var tier = this.FirstOrDefault(t => t.TierType == TierType.FreeTranslation) as TextTier;
			return (tier == null && createEmptyIfNotExist ?
				new TextTier(TextTier.ElanTranslationTierId) : tier);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets all the text tiers other than the transcription tier.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<TextTier> GetDependentTextTiers()
		{
			return this.OfType<TextTier>().Where(t => t.TierType != TierType.Transcription);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets all the text tiers other than the transcription and free translation.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<TextTier> GetUserDefinedTextTiers()
		{
			return this.OfType<TextTier>().Where(t => t.TierType != TierType.Transcription &&
				t.TierType != TierType.FreeTranslation);
		}

		#endregion
	}
}
