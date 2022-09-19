using System;
using System.IO;
using System.Linq;
using L10NSharp;
using SIL.Reporting;
using SayMore.Media;
using SayMore.Properties;
using SayMore.Transcription.UI;
using SayMore.Utilities;

namespace SayMore.Transcription.Model
{
	public enum BoundaryModificationResult
	{
		Success,
		SegmentNotFound,
		SegmentWillBeTooShort,
		NextSegmentWillBeTooShort,
		BlockedByOralAnnotations,
	}

	/// ----------------------------------------------------------------------------------------
	public class TimeTier : TierBase
	{
		private readonly TimeSpan _totalTime;
		public string MediaFileName { get; protected set; }
		public bool ReadOnlyTimeRanges { get; set; }
		public Action<string, bool> BackupOralAnnotationSegmentFileAction { get; set; }

		/// ------------------------------------------------------------------------------------
		public TimeTier(string filename) : this("Source", filename)
		{
		}

		/// ------------------------------------------------------------------------------------
		public TimeTier(string id, string filename)
			: base(id, tier => new AudioWaveFormColumn(tier))
		{
			MediaFileName = filename;

			void ReportAccessError(Exception exception)
			{
				var msg = LocalizationManager.GetString("SessionsView.Transcription.ErrorAccessingMediaFile",
					"There was an error accessing media file to determine the duration: '{0}'");

				ErrorReport.NotifyUserOfProblem(exception, msg, filename);
			}

			try
			{
				// SP-835: There was an error accessing media file to determine the duration:
				//  F:\Master\TsakhurProject\Corpus\TsakhurCultureAndLanguageProject\Sessions\TKR-Y-20130708-N03\TKR-Y-20130708-N03_Source.wav
				var shortName = FileSystemUtils.GetShortName(filename);
				var info = MediaFileInfo.GetInfo(shortName, out var error);
				// SP-1798, SP-2280: Using the null info (that can happen if the media file is
				// corrupt) hides the original useful error.
				if (info == null)
					ReportAccessError(error);
				else
					_totalTime = info.Duration;
			}
			catch (Exception e)
			{
				ReportAccessError(e);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override TierBase GetNewTierInstance()
		{
			return new TimeTier(Id, MediaFileName) { ReadOnlyTimeRanges = ReadOnlyTimeRanges };
		}

		#region Methods for dealing with annotation files
		/// ------------------------------------------------------------------------------------
		public string GetFullPathToCarefulSpeechFile(AnnotationSegment segment)
		{
			return Path.Combine(SegmentFileFolder, ComputeFileNameForCarefulSpeechSegment(segment));
		}

		/// ------------------------------------------------------------------------------------
		public string GetFullPathToOralTranslationFile(AnnotationSegment segment)
		{
			return Path.Combine(SegmentFileFolder, ComputeFileNameForOralTranslationSegment(segment));
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetTotalAnnotatedTime(OralAnnotationType type)
		{
			return TimeSpan.FromSeconds(Segments.Where(segment =>
				segment.GetHasOralAnnotation(type)).Sum(segment => segment.GetLength()));
		}
		#endregion

		#region Static methods for computing oral annotation segment audio file names.
		/// ------------------------------------------------------------------------------------
		public static string ComputeFileNameForCarefulSpeechSegment(AnnotationSegment segment)
		{
			return ComputeFileNameForCarefulSpeechSegment(segment.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		public static string ComputeFileNameForOralTranslationSegment(AnnotationSegment segment)
		{
			return ComputeFileNameForOralTranslationSegment(segment.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		public static string ComputeFileNameForCarefulSpeechSegment(TimeRange timeRange)
		{
			return ComputeFileNameForCarefulSpeechSegment(timeRange.StartSeconds, timeRange.EndSeconds);
		}

		/// ------------------------------------------------------------------------------------
		public static string ComputeFileNameForOralTranslationSegment(TimeRange timeRange)
		{
			return ComputeFileNameForOralTranslationSegment(timeRange.StartSeconds, timeRange.EndSeconds);
		}

		/// ------------------------------------------------------------------------------------
		public static string ComputeFileNameForCarefulSpeechSegment(float start, float end)
		{
			return string.Format("{0}_to_{1}{2}", start, end,
				Settings.Default.OralAnnotationCarefulSegmentFileSuffix);
		}

		/// ------------------------------------------------------------------------------------
		public static string ComputeFileNameForOralTranslationSegment(float start, float end)
		{
			return string.Format("{0}_to_{1}{2}", start, end,
				Settings.Default.OralAnnotationTranslationSegmentFileSuffix);
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		public override string DisplayName
		{
			get { return string.Empty; }
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan TotalTime
		{
			get { return _totalTime; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The SegmentFileFolder is used for renaming (due to segment boundary changes) and
		/// removing audio segment annotation files that are being created/modified in a
		/// temp. location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SegmentFileFolder
		{
			get { return MediaFileName + Settings.Default.OralAnnotationsFolderSuffix; }
		}

		/// ------------------------------------------------------------------------------------
		public override TierType TierType
		{
			get { return TierType.Time; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsFullySegmented
		{
			get { return EndOfLastSegment >= _totalTime; }
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan EndOfLastSegment
		{
			get
			{
				return (Segments.Count == 0 ? TimeSpan.Zero : Segments[Segments.Count - 1].TimeRange.End);
			}
		}
		#endregion

		/// ------------------------------------------------------------------------------------
		public int GetIndexOfSegment(AnnotationSegment segment)
		{
			if (segment == null)
				return -1;

			for (int i = 0; i < Segments.Count; i++)
			{
				if (Segments[i].TimeRange == segment.TimeRange)
					return i;
			}

			return -1;
		}

		/// ------------------------------------------------------------------------------------
		public AnnotationSegment GetSegmentHavingEndBoundary(float endBoundary)
		{
			return Segments.FirstOrDefault(s => s.EndsAt(endBoundary));
		}

		/// ------------------------------------------------------------------------------------
		public AnnotationSegment GetSegmentHavingStartBoundary(float startBoundary)
		{
			return Segments.FirstOrDefault(s => s.StartsAt(startBoundary));
		}

		/// ------------------------------------------------------------------------------------
		public AnnotationSegment GetSegmentEnclosingTime(float time)
		{
			return Segments.FirstOrDefault(s => s.TimeRange.GetIsTimeInRange(time, false, true));
		}

		#region Methods for Adding and removing segments
		/// ------------------------------------------------------------------------------------
		public AnnotationSegment AppendSegment(float endOfNewSegment)
		{
			var startOfNewSegment = (float)EndOfLastSegment.TotalSeconds;
			if (endOfNewSegment <= startOfNewSegment)
				throw new ArgumentException("Cannot append a segment ending at " + endOfNewSegment +
					". Appended segment must be beyond the end of the last existing segment (" + startOfNewSegment + ".");
			return AddSegment(startOfNewSegment, endOfNewSegment);
		}

		/// ------------------------------------------------------------------------------------
		public AnnotationSegment AddSegment(float start, float stop)
		{
			var segment = new AnnotationSegment(this, start, stop);
			Segments.Add(segment);
			return segment;
		}

		/// ------------------------------------------------------------------------------------
		public bool RemoveSegmentHavingEndBoundary(float endBoundary)
		{
			var segment = GetSegmentHavingEndBoundary(endBoundary);
			return segment != null && RemoveSegment(segment);
		}

		/// ------------------------------------------------------------------------------------
		public bool RemoveSegment(AnnotationSegment segment)
		{
			return RemoveSegment(GetIndexOfSegment(segment));
		}

		/// ------------------------------------------------------------------------------------
		public override bool RemoveSegment(int index)
		{
			if (Segments.Count > 0 && index >= 0 && index < Segments.Count)
			{
				var segToRemove = Segments[index];

				if (Segments.Count > 1 && index < Segments.Count - 1)
				{
					var nextSeg = Segments[index + 1];
					RenameAnnotationSegmentFile(nextSeg, segToRemove.Start, nextSeg.End);
					nextSeg.Start = segToRemove.Start;
				}

				DeleteAnnotationSegmentFile(segToRemove);
			}

			return base.RemoveSegment(index);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public BoundaryModificationResult ChangeSegmentsEndBoundary(float oldEndBoundary, float newEndBoundary)
		{
			var segment = GetSegmentHavingEndBoundary(oldEndBoundary);

			return (segment == null ? BoundaryModificationResult.SegmentNotFound :
				ChangeSegmentsEndBoundary(segment, newEndBoundary));
		}

		/// ------------------------------------------------------------------------------------
		public BoundaryModificationResult ChangeSegmentsEndBoundary(AnnotationSegment segment, float newEndBoundary)
		{
			// New boundary must be at least a minimal amount greater than the segment's start boundary.
			if (!GetIsAcceptableSegmentLength(segment.Start, newEndBoundary))
				return BoundaryModificationResult.SegmentWillBeTooShort;

			var segIndex = GetIndexOfSegment(segment);
			if (segIndex < 0)
				return BoundaryModificationResult.SegmentNotFound;

			var nextSegment = (segIndex < Segments.Count - 1 ? Segments[segIndex + 1] : null);

			if (nextSegment != null)
			{
				if (!GetIsAcceptableSegmentLength(newEndBoundary, nextSegment.End))
					return BoundaryModificationResult.NextSegmentWillBeTooShort;

				RenameAnnotationSegmentFile(nextSegment, newEndBoundary, nextSegment.End);
				nextSegment.Start = newEndBoundary;
			}

			RenameAnnotationSegmentFile(Segments[segIndex], Segments[segIndex].Start, newEndBoundary);
			Segments[segIndex].End = newEndBoundary;

			return BoundaryModificationResult.Success;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>Determines whether a segment that starts and ends at the specified points
		/// (in seconds) in an audio file would long enough to be an acceptable/reasonable
		/// length segment. 
		/// </summary>
		/// <param name="start">Start of (proposed or actual) segment, in seconds</param>
		/// <param name="end">End of (proposed or actual) segment, in seconds</param>
		/// <remarks>The "minimum" length is more of a practical minimum, determined by what
		/// can be reasonably displayed in the UI, rather than an absolute theoretical minimum.
		/// Having a minimum also prevents ridiculously short segments that couldn't possibly
		/// contain a useful utterance.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		public bool GetIsAcceptableSegmentLength(float start, float end)
		{
			// Because the floating point arithmetic can result in rounding errors at a level
			// of precision greater than is represented by the minimum length, we usse rounding
			// to prevent this method from returning false simply because of a lack of precision
			// in storing floating point numbers. (In practice, we probably don't care which way
			// it goes at the ragged edge of the cutoff, but this keeps unit tests from failing
			// in unpredictably bizarre ways.)
			return Math.Round(((end - start) * 1000f - Settings.Default.MinimumSegmentLengthInMilliseconds) * 100f) >= 0 ;
		}

		/// ------------------------------------------------------------------------------------
		public BoundaryModificationResult InsertSegmentBoundary(float newBoundary,
			Func<AnnotationSegment, bool, bool, bool> allowDeletionOfOralAnnotations = null)
		{
			float newSegStart = 0f;
			var segBeingSplit = Segments.FirstOrDefault(
				seg => seg.TimeRange.Contains(TimeSpan.FromSeconds(newBoundary), true));

			if (segBeingSplit == null)
			{
				if (Segments.GetLast() != null)
					newSegStart = Segments.GetLast().End;

				if (!GetIsAcceptableSegmentLength(newSegStart, newBoundary))
					return BoundaryModificationResult.SegmentWillBeTooShort;

				AddSegment(newSegStart, newBoundary);
				return BoundaryModificationResult.Success;
			}

			if (!GetIsAcceptableSegmentLength(segBeingSplit.Start, newBoundary))
				return BoundaryModificationResult.SegmentWillBeTooShort;

			if (!GetIsAcceptableSegmentLength(newBoundary, segBeingSplit.End))
				return BoundaryModificationResult.NextSegmentWillBeTooShort;

			if (segBeingSplit.TimeRange.EndSeconds > newBoundary)
			{
				var hasCarefulSpeech = segBeingSplit.GetHasOralAnnotation(OralAnnotationType.CarefulSpeech);
				var hasOralTranslation = segBeingSplit.GetHasOralAnnotation(OralAnnotationType.Translation);
				if ((hasCarefulSpeech || hasOralTranslation) && (allowDeletionOfOralAnnotations == null ||
					!allowDeletionOfOralAnnotations(segBeingSplit, hasCarefulSpeech, hasOralTranslation)))
				{
					return BoundaryModificationResult.BlockedByOralAnnotations;
				}
			}

			float newSegEnd = segBeingSplit.End;
			segBeingSplit.End = newBoundary;
			var newSegment = new AnnotationSegment(segBeingSplit.Tier, newBoundary, newSegEnd);
			Segments.Insert(GetIndexOfSegment(segBeingSplit) + 1, newSegment);

			return BoundaryModificationResult.Success;
		}

		#region Methods for renaming and deleting oral annotation segment files
		/// ------------------------------------------------------------------------------------
		public void RenameAnnotationSegmentFile(AnnotationSegment oldSegment, float newStart, float newEnd)
		{
			try
			{
				var oldSegmentFilePath = Path.Combine(SegmentFileFolder,
					ComputeFileNameForCarefulSpeechSegment(oldSegment));

				if (File.Exists(oldSegmentFilePath))
				{
					if (BackupOralAnnotationSegmentFileAction != null)
						BackupOralAnnotationSegmentFileAction(oldSegmentFilePath, false);

					File.Move(oldSegmentFilePath, Path.Combine(SegmentFileFolder,
						ComputeFileNameForCarefulSpeechSegment(newStart, newEnd)));
				}
			}
			catch { }

			try
			{
				var oldSegmentFilePath = Path.Combine(SegmentFileFolder,
					ComputeFileNameForOralTranslationSegment(oldSegment));

				if (File.Exists(oldSegmentFilePath))
				{
					if (BackupOralAnnotationSegmentFileAction != null)
						BackupOralAnnotationSegmentFileAction(oldSegmentFilePath, false);

					File.Move(oldSegmentFilePath, Path.Combine(SegmentFileFolder,
						ComputeFileNameForOralTranslationSegment(newStart, newEnd)));
				}
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		public void DeleteAnnotationSegmentFile(AnnotationSegment segment)
		{
			try
			{
				var path = GetFullPathToCarefulSpeechFile(segment);
				if (File.Exists(path))
				{
					if (BackupOralAnnotationSegmentFileAction != null)
						BackupOralAnnotationSegmentFileAction(path, true);
					else
						File.Delete(path);
				}
			}
			catch { }

			try
			{
				var path = GetFullPathToOralTranslationFile(segment);
				if (File.Exists(path))
				{
					if (BackupOralAnnotationSegmentFileAction != null)
						BackupOralAnnotationSegmentFileAction(path, true);
					else
						File.Delete(path);
				}
			}
			catch { }
		}

		#endregion

		#region Methods for determining whether or not a boundary can move left or right
		/// ------------------------------------------------------------------------------------
		public bool CanBoundaryMoveLeft(float boundaryToMove, float secondsToMove)
		{
			if (ReadOnlyTimeRanges)
				return false;

			var newBoundary = boundaryToMove - secondsToMove;
			var segment = GetSegmentEnclosingTime(boundaryToMove);

			return (newBoundary > 0 && (segment == null || GetIsAcceptableSegmentLength(segment.Start, newBoundary)));
		}

		/// ------------------------------------------------------------------------------------
		public bool CanBoundaryMoveRight(float boundaryToMove, float secondsToMove, float limit)
		{
			if (ReadOnlyTimeRanges)
				return false;

			var newBoundary = boundaryToMove + secondsToMove;
			if (newBoundary <= 0 || newBoundary > limit)
				return false;

			var segment = GetSegmentHavingEndBoundary(boundaryToMove);
			if (segment != null)
			{
				int i = GetIndexOfSegment(segment);
				return (i == Segments.Count - 1 || GetIsAcceptableSegmentLength(newBoundary, Segments[i + 1].End));
			}

			segment = GetSegmentEnclosingTime(boundaryToMove);
			return (segment == null || GetIsAcceptableSegmentLength(newBoundary, segment.End));
		}

		#endregion
	}
}
