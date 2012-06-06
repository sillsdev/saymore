using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Localization;
using NAudio.Wave;
using Palaso.Media.Naudio;
using Palaso.Media.Naudio.UI;
using Palaso.Reporting;
using SayMore.Media.Audio;
using SayMore.Model.Files;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public enum StopAnnotationRecordingResult
	{
		Normal,
		AnnotationTooShort,
		RecordingError
	}

	/// ----------------------------------------------------------------------------------------
	public class OralAnnotationRecorderDlgViewModel : SegmenterDlgBaseViewModel
	{
		public Action<Exception> RecordingErrorAction { get; set; }
		public Action<Exception> PlaybackErrorAction { get; set; }
		public Segment CurrentUnannotatedSegment { get; private set; }
		public OralAnnotationRecorder Recorder { get; private set; }
		private AudioPlayer _annotationPlayer;
		private TimeSpan _endBoundary;

		/// ----------------------------------------------------------------------------------------
		public static OralAnnotationRecorderDlgViewModel Create(ComponentFile file,
			AudioRecordingType annotationType)
		{
			return (annotationType == AudioRecordingType.Careful ?
				new CarefulSpeechAnnotationRecorderDlgViewModel(file) as OralAnnotationRecorderDlgViewModel :
				new OralTranslationAnnotationRecorderDlgViewModel(file));
		}

		/// ------------------------------------------------------------------------------------
		protected OralAnnotationRecorderDlgViewModel(ComponentFile file) : base(file)
		{
			NewSegmentEndBoundary = GetEndOfLastSegment();
		}

		/// ------------------------------------------------------------------------------------
		public override void Dispose()
		{
			CloseAnnotationPlayer();
			CloseAnnotationRecorder();
			base.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		public void RemoveInvalidAnnotationFiles()
		{
			var annotationFiles = from s in TimeTier.Segments
								  where GetDoesSegmentHaveAnnotationFile(s)
								  select GetFullPathToAnnotationFileForSegment(s);

			foreach (var path in annotationFiles.Where(p => !AudioUtils.GetDoesFileSeemToBeWave(p)))
				BackupOralAnnotationSegmentFile(path, true);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public virtual OralAnnotationType AnnotationType
		{
			get { throw new NotImplementedException(); }
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan NewSegmentEndBoundary
		{
			get { return _endBoundary; }
			set { _endBoundary = (value < GetEndOfLastSegment()) ? GetEndOfLastSegment() : value; }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsFullySegmented
		{
			get { return TimeTier.IsFullySegmented; }
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsFullyAnnotated()
		{
			return TimeTier.GetIsFullyAnnotated(AnnotationType);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsRecorderInErrorState()
		{
			return Recorder == null || Recorder.GetIsInErrorState();
		}
		#endregion

		#region Segment-related methods
		/// ----------------------------------------------------------------------------------------
		public Segment GetSegment(int index)
		{
			return (index < 0 || index >= TimeTier.Segments.Count ?
				null : TimeTier.Segments[index]);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool UpdateSegmentBoundary(TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			var moved = base.UpdateSegmentBoundary(oldEndTime, newEndTime);
			if (oldEndTime == NewSegmentEndBoundary)
			{
				NewSegmentEndBoundary = newEndTime;
				return true;
			}
			return moved;
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<TimeSpan> GetSegmentEndBoundaries()
		{
			foreach (var boundary in base.GetSegmentEndBoundaries())
				yield return boundary;

			if (!IsFullySegmented && GetHasNewSegment())
				yield return NewSegmentEndBoundary;
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesSegmentHaveAnnotationFile(int segmentIndex)
		{
			if (segmentIndex < 0 || segmentIndex >= TimeTier.Segments.Count)
				return false;

			return GetDoesSegmentHaveAnnotationFile(TimeTier.Segments[segmentIndex]);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesSegmentHaveAnnotationFile(TimeRange timeRange)
		{
			var segment = TimeTier.GetSegmentHavingEndBoundary(timeRange.EndSeconds);

			if (segment != null)
				Debug.Assert(segment.Start.Equals(timeRange.StartSeconds));

			return GetDoesSegmentHaveAnnotationFile(segment);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesSegmentHaveAnnotationFile(Segment segment)
		{
			return (segment != null && File.Exists(GetFullPathToAnnotationFileForSegment(segment)));
		}

		/// ------------------------------------------------------------------------------------
		public string GetFullPathToAnnotationFileForSegment(Segment segment)
		{
			return GetFullPathOfAnnotationFileForTimeRange(segment.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		public virtual string GetFullPathOfAnnotationFileForTimeRange(TimeRange timeRange)
		{
			throw new NotImplementedException();
		}

		/// ----------------------------------------------------------------------------------------
		public bool GetHasNewSegment()
		{
			return NewSegmentEndBoundary > GetEndOfLastSegment();
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesHaveSegments()
		{
			return (Tiers.GetDoTimeSegmentsExist());
		}

		/// ------------------------------------------------------------------------------------
		public bool SetNextUnannotatedSegment()
		{
			CurrentUnannotatedSegment = TimeTier.Segments.FirstOrDefault(s =>
				(CurrentUnannotatedSegment == null || s.End > CurrentUnannotatedSegment.End) &&
				!GetDoesSegmentHaveAnnotationFile(s));

			if (CurrentUnannotatedSegment == null)
			{
				CurrentUnannotatedSegment = TimeTier.Segments
					.FirstOrDefault(s => !GetDoesSegmentHaveAnnotationFile(s));
			}

			return CurrentUnannotatedSegment != null;
		}

		/// ------------------------------------------------------------------------------------
		public TimeRange GetSelectedTimeRange()
		{
			return (CurrentUnannotatedSegment != null) ? CurrentUnannotatedSegment.TimeRange :
				new TimeRange(GetEndOfLastSegment(), NewSegmentEndBoundary);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetSelectedSegmentIsLongEnough()
		{
			return TimeTier.GetIsAcceptableSegmentLength(GetSelectedTimeRange().StartSeconds,
				GetSelectedTimeRange().EndSeconds);
		}

		/// ----------------------------------------------------------------------------------------
		public IEnumerable<TimeSpan> MakeSegmentForEndBoundary()
		{
			return InsertNewBoundary(NewSegmentEndBoundary);
		}

		/// ------------------------------------------------------------------------------------
		protected override void RevertNewSegment(SegmentChange change)
		{
			base.RevertNewSegment(change);
			_endBoundary = GetEndOfLastSegment();
		}
		#endregion

		#region Annotation recording methods
		/// ------------------------------------------------------------------------------------
		public void InitializeAnnotationRecorder(PeakMeterCtrl peakMeter,
			Action<TimeSpan> recordingProgressAction)
		{
			Recorder = new OralAnnotationRecorder(peakMeter, recordingProgressAction);
			Recorder.RecordingStarted += (s, e) => InvokeUpdateDisplayAction();
			Recorder.Stopped += (sender, args) => InvokeUpdateDisplayAction();
			Recorder.BeginMonitoring();
		}

		/// ------------------------------------------------------------------------------------
		public void CloseAnnotationRecorder()
		{
			AudioUtils.NAudioExceptionThrown -= HandleNAudioExceptionThrownDuringRecord;

			if (Recorder != null)
				Recorder.Dispose();

			Recorder = null;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleNAudioExceptionThrownDuringRecord(Exception exception)
		{
			CloseAnnotationRecorder();

			if (RecordingErrorAction != null)
				RecordingErrorAction(exception);
		}
		/// ------------------------------------------------------------------------------------
		public bool BeginAnnotationRecording(TimeRange timeRange)
		{
			if (GetIsRecording())
				return false;

			var path = GetFullPathOfAnnotationFileForTimeRange(timeRange);
			var backupCreated = false;
			if (File.Exists(path))
			{
				BackupOralAnnotationSegmentFile(path, true);
				backupCreated = true;
			}
			else
			{
				var dir = Path.GetDirectoryName(path);
				if (dir != null && !Directory.Exists(dir))
					Directory.CreateDirectory(dir);
			}

			var recordingStarted = AttemptBeginAnnotationRecording(path);

			if (!recordingStarted && backupCreated)
				RestorePreviousVersionOfAnnotation(path);

			return recordingStarted;
		}

		/// ------------------------------------------------------------------------------------
		private bool AttemptBeginAnnotationRecording(string path)
		{
			try
			{
				AudioUtils.NAudioExceptionThrown += HandleNAudioExceptionThrownDuringRecord;
				return Recorder.BeginAnnotationRecording(path);
			}
			catch (Exception e)
			{
				AudioUtils.NAudioExceptionThrown -= HandleNAudioExceptionThrownDuringRecord;
				var args = new CancelExceptionHandlingEventArgs(e);
				AudioUtils.HandleGlobalNAudioException(this, args);
				if (!args.Cancel)
				{
					ErrorReport.NotifyUserOfProblem(e, LocalizationManager.GetString(
						"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.UnexpectedErrorAttemptingToRecordMsg",
						"An unexpected error occurred when attempting to record an annotation."));
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		public StopAnnotationRecordingResult StopAnnotationRecording(TimeRange timeRange)
		{
			AudioUtils.NAudioExceptionThrown -= HandleNAudioExceptionThrownDuringRecord;

			Recorder.Stop();

			var isRecorderInErrorState = Recorder.GetIsInErrorState();
			var isRecordingTooShort = Recorder.GetIsRecordingTooShort() && !isRecorderInErrorState;

			if (isRecordingTooShort || isRecorderInErrorState)
			{
				RestorePreviousVersionOfAnnotation(timeRange);
			}
			else
			{
				_undoStack.Push(new SegmentChange(SegmentChangeType.AnnotationAdded, timeRange, timeRange,
					c => RestorePreviousVersionOfAnnotation(timeRange)));
			}

			//DeleteTemporarilySavedAnnotation();

			if (isRecorderInErrorState)
				return StopAnnotationRecordingResult.RecordingError;

			return (isRecordingTooShort ?
				StopAnnotationRecordingResult.AnnotationTooShort :
				StopAnnotationRecordingResult.Normal);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsRecording()
		{
			return (Recorder != null && !Recorder.GetIsInErrorState() &&
				Recorder.RecordingState == RecordingState.Recording);
		}

		#endregion

		#region Annotation playback methods
		/// ------------------------------------------------------------------------------------
		public bool InitializeAnnotationPlayer(Segment segment)
		{
			CloseAnnotationPlayer();

			var filename = GetFullPathToAnnotationFileForSegment(segment);
			if (!File.Exists(filename))
				return false;

			var fi = new FileInfo(filename);
			if (fi.Length == 0)
			{
				fi.Delete();
				return false;
			}

			AudioUtils.NAudioExceptionThrown += HandleNAudioExceptionThrownDuringPlayback;

			_annotationPlayer = new AudioPlayer();
			_annotationPlayer.LoadFile(filename);
			_annotationPlayer.PlaybackStarted += (sender, args) => InvokeUpdateDisplayAction();
			_annotationPlayer.Stopped += delegate
			{
				InvokeUpdateDisplayAction();
				CloseAnnotationPlayer();
			};

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public void CloseAnnotationPlayer()
		{
			AudioUtils.NAudioExceptionThrown -= HandleNAudioExceptionThrownDuringPlayback;

			if (_annotationPlayer != null)
				_annotationPlayer.Dispose();

			_annotationPlayer = null;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleNAudioExceptionThrownDuringPlayback(Exception exception)
		{
			CloseAnnotationPlayer();

			if (PlaybackErrorAction != null)
				PlaybackErrorAction(exception);
		}

		/// ------------------------------------------------------------------------------------
		public void StartAnnotationPlayback(Segment segment,
			Action<PlaybackProgressEventArgs> playbackProgressAction,
			Action playbackStoppedAction)
		{
			if (!GetDoesSegmentHaveAnnotationFile(segment))
				return;

			if (InitializeAnnotationPlayer(segment))
			{
				_annotationPlayer.PlaybackProgress += (sender, args) => playbackProgressAction(args);
				_annotationPlayer.Stopped += (sender, args) => playbackStoppedAction();
				_annotationPlayer.Play();
			}

			UsageReporter.SendNavigationNotice(ProgramAreaForUsageReporting + "/PlayAnnotation");
		}

		/// ------------------------------------------------------------------------------------
		public void StopAnnotationPlayback()
		{
			if (_annotationPlayer != null)
				_annotationPlayer.Stop();
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsAnnotationPlaying()
		{
			return (_annotationPlayer != null &&
				_annotationPlayer.PlaybackState == PlaybackState.Playing);
		}

		#endregion

		#region Methods for erasing an annotation recording
		/// ------------------------------------------------------------------------------------
		protected void RestorePreviousVersionOfAnnotation(TimeRange timeRange)
		{
			var audioFilePath = GetFullPathOfAnnotationFileForTimeRange(timeRange);
			RestorePreviousVersionOfAnnotation(audioFilePath);
			SegmentsAnnotationSamplesToDraw.RemoveWhere(h => h.AudioFilePath == audioFilePath);
		}

		/// ------------------------------------------------------------------------------------
		protected override void EraseAnnotation(string path)
		{
			CloseAnnotationPlayer();
			base.EraseAnnotation(path);
		}
		#endregion
	}

	#region CarefulSpeechAnnotationRecorderDlgViewModel class
	/// ----------------------------------------------------------------------------------------
	public class CarefulSpeechAnnotationRecorderDlgViewModel : OralAnnotationRecorderDlgViewModel
	{
		/// ------------------------------------------------------------------------------------
		public CarefulSpeechAnnotationRecorderDlgViewModel(ComponentFile file)
			: base(file)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override string ProgramAreaForUsageReporting
		{
			get { return "Annotations/Oral/" + AudioRecordingType.Careful; }
		}

		/// ------------------------------------------------------------------------------------
		public override string GetFullPathOfAnnotationFileForTimeRange(TimeRange timeRange)
		{
			var segment = TimeTier.Segments.FirstOrDefault(s => s.TimeRange == timeRange) ??
				new Segment(null, timeRange);
			return TimeTier.GetFullPathToCarefulSpeechFile(segment);
		}

		/// ------------------------------------------------------------------------------------
		public override OralAnnotationType AnnotationType
		{
			get { return OralAnnotationType.Translation; }
		}
	}
	#endregion

	#region OralTranslationAnnotationRecorderDlgViewModel
	/// ----------------------------------------------------------------------------------------
	public class OralTranslationAnnotationRecorderDlgViewModel : OralAnnotationRecorderDlgViewModel
	{
		/// ------------------------------------------------------------------------------------
		public OralTranslationAnnotationRecorderDlgViewModel(ComponentFile file) : base(file)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override string ProgramAreaForUsageReporting
		{
			get { return "Annotations/Oral/" + AudioRecordingType.Translation; }
		}

		/// ------------------------------------------------------------------------------------
		public override string GetFullPathOfAnnotationFileForTimeRange(TimeRange timeRange)
		{
			var segment = TimeTier.Segments.FirstOrDefault(s => s.TimeRange == timeRange) ??
				new Segment(null, timeRange);

			return TimeTier.GetFullPathToOralTranslationFile(segment);
		}

		/// ------------------------------------------------------------------------------------
		public override OralAnnotationType AnnotationType
		{
			get { return OralAnnotationType.Translation; }
		}
	}

	#endregion
}
