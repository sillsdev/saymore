using System.IO;
using System.Linq;
using Localization;
using SayMore.Properties;
using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	public enum BoundaryModificationResult
	{
		Success,
		SegmentNotFound,
		SegmentWillBeTooShort,
		NextSegmentWillBeTooShort
	}

	/// ----------------------------------------------------------------------------------------
	public class TimeTier : TierBase
	{
		public string MediaFileName { get; protected set; }
		public string SegmentFileFolder { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public TimeTier(string filename) :
			this(LocalizationManager.GetString("EventsView.Transcription.TierNames.OriginalRecording", "Original"), filename)
		{
		}

		/// ------------------------------------------------------------------------------------
		public TimeTier(string displayName, string filename) :
			base(displayName, tier => new AudioWaveFormColumn(tier))
		{
			MediaFileName = filename;
			SegmentFileFolder = MediaFileName + Settings.Default.OralAnnotationsFolderAffix;
		}

		#region Static methods for computing oral annotation segment audio file names.
		/// ------------------------------------------------------------------------------------
		public static string ComputeFileNameForCarefulSpeechSegment(Segment segment)
		{
			return ComputeFileNameForCarefulSpeechSegment(segment.Start, segment.End);
		}

		/// ------------------------------------------------------------------------------------
		public static string ComputeFileNameForOralTranslationSegment(Segment segment)
		{
			return ComputeFileNameForOralTranslationSegment(segment.Start, segment.End);
		}

		/// ------------------------------------------------------------------------------------
		public static string ComputeFileNameForCarefulSpeechSegment(float start, float end)
		{
			return string.Format("{0}_to_{1}{2}", start, end,
				Settings.Default.OralAnnotationCarefulSegmentFileAffix);
		}

		/// ------------------------------------------------------------------------------------
		public static string ComputeFileNameForOralTranslationSegment(float start, float end)
		{
			return string.Format("{0}_to_{1}{2}", start, end,
				Settings.Default.OralAnnotationTranslationSegmentFileAffix);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected override TierBase GetNewTierInstance()
		{
			return new TimeTier(DisplayName, MediaFileName);
		}

		/// ------------------------------------------------------------------------------------
		public override TierType TierType
		{
			get { return TierType.Time; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		public int GetIndexOfSegment(Segment segment)
		{
			for (int i = 0; i < Segments.Count; i++)
			{
				if (Segments[i].Start.Equals(segment.Start) && Segments[i].End.Equals(segment.End))
					return i;
			}

			return -1;
		}

		#region Methods for Adding and removing segments
		/// ------------------------------------------------------------------------------------
		public Segment AddSegment(float start, float stop)
		{
			var segment = new Segment(this, start, stop);
			Segments.Add(segment);
			return segment;
		}

		/// ------------------------------------------------------------------------------------
		public bool RemoveSegment(Segment segment)
		{
			return RemoveSegment(GetIndexOfSegment(segment));
		}

		/// ------------------------------------------------------------------------------------
		public override bool RemoveSegment(int index)
		{
			if (Segments.Count > 0 && index >= 0 && index < Segments.Count)
			{
				var segToRemove = Segments[index];

				if (index == 0)
				{
					var nextSeg = Segments[index + 1];
					RenameAnnotationSegmentFile(nextSeg, segToRemove.Start, nextSeg.End);
					nextSeg.Start = segToRemove.Start;
				}
				else if (index < Segments.Count - 1)
				{
					var prevSeg = Segments[index - 1];
					RenameAnnotationSegmentFile(prevSeg, prevSeg.Start, segToRemove.End);
					prevSeg.End = segToRemove.End;
				}

				DeleteAnnotationSegmentFile(segToRemove);
			}

			return base.RemoveSegment(index);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public BoundaryModificationResult ChangeSegmentsEndBoundary(Segment segment, float newBoundary)
		{
			// New boundary must be at least 1/2 second greater than the segment's start boundary.
			if (segment.Start >= newBoundary - 0.5f)
				return BoundaryModificationResult.SegmentWillBeTooShort;

			var segIndex = GetIndexOfSegment(segment);
			if (segIndex < 0)
				return BoundaryModificationResult.SegmentNotFound;

			var nextSegment = (segIndex < Segments.Count - 1 ? Segments[segIndex + 1] : null);

			if (nextSegment != null)
			{
				if (newBoundary + 0.5f >= nextSegment.End)
					return BoundaryModificationResult.NextSegmentWillBeTooShort;

				RenameAnnotationSegmentFile(nextSegment, newBoundary, nextSegment.End);
				nextSegment.Start = newBoundary;
			}

			RenameAnnotationSegmentFile(Segments[segIndex], Segments[segIndex].Start, newBoundary);
			Segments[segIndex].End = newBoundary;

			return BoundaryModificationResult.Success;
		}


		/// ------------------------------------------------------------------------------------
		public BoundaryModificationResult InsertSegmentBoundary(float newBoundary)
		{
			float newSegStart = 0f;
			var segBeingSplit = Segments.FirstOrDefault(seg => newBoundary > seg.Start && newBoundary <= seg.End);

			if (segBeingSplit == null)
			{
				if (Segments.GetLast() != null)
					newSegStart = Segments.GetLast().End;

				if (newSegStart >= newBoundary - 0.5f)
					return BoundaryModificationResult.SegmentWillBeTooShort;

				AddSegment(newSegStart, newBoundary);
				return BoundaryModificationResult.Success;
			}

			if (segBeingSplit.Start >= newBoundary - 0.5f)
				return BoundaryModificationResult.SegmentWillBeTooShort;

			if (newBoundary + 0.5f >= segBeingSplit.End)
				return BoundaryModificationResult.NextSegmentWillBeTooShort;

			RenameAnnotationSegmentFile(segBeingSplit, segBeingSplit.Start, newBoundary);

			float newSegEnd = segBeingSplit.End;
			segBeingSplit.End = newBoundary;
			var newSegment = new Segment(segBeingSplit.Tier, newBoundary, newSegEnd);
			Segments.Insert(GetIndexOfSegment(segBeingSplit) + 1, newSegment);

			return BoundaryModificationResult.Success;
		}

		#region Methods for renaming and deleting oral annotation segment files
		/// ------------------------------------------------------------------------------------
		public void RenameAnnotationSegmentFile(Segment oldSegment, float newStart, float newEnd)
		{
			try
			{
				var oldSegmentFilePath = Path.Combine(SegmentFileFolder,
					ComputeFileNameForCarefulSpeechSegment(oldSegment));

				if (File.Exists(oldSegmentFilePath))
				{
					File.Move(oldSegmentFilePath,
						Path.Combine(SegmentFileFolder, ComputeFileNameForCarefulSpeechSegment(newStart, newEnd)));
				}
			}
			catch { }

			try
			{
				var oldSegmentFilePath = Path.Combine(SegmentFileFolder,
					ComputeFileNameForOralTranslationSegment(oldSegment));

				if (File.Exists(oldSegmentFilePath))
				{
					File.Move(oldSegmentFilePath,
						Path.Combine(SegmentFileFolder, ComputeFileNameForOralTranslationSegment(newStart, newEnd)));
				}
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		public void DeleteAnnotationSegmentFile(Segment segment)
		{
			try
			{
				var path = Path.Combine(SegmentFileFolder,
					ComputeFileNameForCarefulSpeechSegment(segment));

				if (File.Exists(path))
					File.Delete(path);
			}
			catch { }

			try
			{
				var path = Path.Combine(SegmentFileFolder,
					ComputeFileNameForOralTranslationSegment(segment));

				if (File.Exists(path))
					File.Delete(path);
			}
			catch { }
		}

		#endregion
	}
}
