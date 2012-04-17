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
using SayMore.Media;
using SayMore.Model.Files;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI.SegmentingAndRecording;
using SayMore.UI.NewEventsFromFiles;

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
		private TimeRange _timeRangeForAnnotationBeingRerecorded;
		private TimeSpan _endBoundary;

		/// ----------------------------------------------------------------------------------------
		public static OralAnnotationRecorderDlgViewModel Create(ComponentFile file,
			OralAnnotationType annotationType)
		{
			return (annotationType == OralAnnotationType.Careful ?
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
				EraseAnnotation(path);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public TimeSpan NewSegmentEndBoundary
		{
			get { return _endBoundary; }
			set { _endBoundary = (value < GetEndOfLastSegment()) ? GetEndOfLastSegment() : value; }
		}

		/// ------------------------------------------------------------------------------------
		public bool AnnotationRecordingsChanged { get; private set; }

		/// ------------------------------------------------------------------------------------
		public override bool WereChangesMade
		{
			get { return base.WereChangesMade || AnnotationRecordingsChanged; }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsFullySegmented
		{
			get { return (GetEndOfLastSegment() == OrigWaveStream.TotalTime); }
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsFullyAnnotated()
		{
			return IsFullySegmented && TimeTier.Segments.All(GetDoesSegmentHaveAnnotationFile);
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
		public override bool SegmentBoundaryMoved(TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			if (oldEndTime == NewSegmentEndBoundary)
			{
				NewSegmentEndBoundary = newEndTime;
				return true;
			}

			return base.SegmentBoundaryMoved(oldEndTime, newEndTime);
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<TimeSpan> GetSegmentEndBoundaries()
		{
			foreach (var boundary in base.GetSegmentEndBoundaries())
				yield return boundary;

			if (!IsFullySegmented)
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

		/// ----------------------------------------------------------------------------------------
		public IEnumerable<TimeSpan> MakeSegmentForEndBoundary()
		{
			return InsertNewBoundary(NewSegmentEndBoundary);
		}

		#endregion

		#region Annotation recording methods
		/// ------------------------------------------------------------------------------------
		public void InitializeAnnotationRecorder(PeakMeterCtrl peakMeter,
			Action<TimeSpan> recordingProgressAction)
		{
			CloseAnnotationRecorder();
			Recorder = new OralAnnotationRecorder(peakMeter, recordingProgressAction);
			Recorder.RecordingStarted += (s, e) => InvokeUpdateDisplayAction();
			Recorder.Stopped += (sender, args) => InvokeUpdateDisplayAction();
			Recorder.BeginMonitoring();
		}

		/// ------------------------------------------------------------------------------------
		public void CloseAnnotationRecorder()
		{
			AudioUtils.NAudioErrorAction = null;

			if (Recorder != null)
				Recorder.Dispose();

			Recorder = null;
		}

		/// ------------------------------------------------------------------------------------
		public bool BeginAnnotationRecording(TimeRange timeRange)
		{
			return BeginAnnotationRecording(GetFullPathOfAnnotationFileForTimeRange(timeRange));
		}

		/// ------------------------------------------------------------------------------------
		private bool BeginAnnotationRecording(string path)
		{
			if (GetIsRecording() || File.Exists(path))
				return false;

			if (!Directory.Exists(Path.GetDirectoryName(path)))
				Directory.CreateDirectory(Path.GetDirectoryName(path));

			try
			{
				AudioUtils.NAudioErrorAction = exception =>
				{
					AudioUtils.NAudioErrorAction = null;
					if (RecordingErrorAction != null)
						RecordingErrorAction(exception);
				};

				return Recorder.BeginAnnotationRecording(path);
			}
			catch (Exception e)
			{
				AudioUtils.NAudioErrorAction = null;
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
			AudioUtils.NAudioErrorAction = null;

			Recorder.Stop();

			var isRecorderInErrorState = Recorder.GetIsInErrorState();
			var isRecordingTooShort = Recorder.GetIsRecordingTooShort() && !isRecorderInErrorState;

			AnnotationRecordingsChanged = (AnnotationRecordingsChanged || !isRecordingTooShort);

			if (isRecordingTooShort || isRecorderInErrorState)
			{
				EraseAnnotation(timeRange);
				RecoverTemporarilySavedAnnotation();
			}

			DeleteTemporarilySavedAnnotation();

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

		#region Methods for dealing with temporarily saved rerecorded annotation
		/// ------------------------------------------------------------------------------------
		public void TemporarilySaveAnnotationBeingRerecorded(TimeRange timeRange)
		{
			var srcFile = GetFullPathOfAnnotationFileForTimeRange(timeRange);
			var dstFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(srcFile));
			CopyFilesViewModel.Copy(srcFile, dstFile, true);
			_timeRangeForAnnotationBeingRerecorded = timeRange;
		}

		/// ------------------------------------------------------------------------------------
		public void RecoverTemporarilySavedAnnotation()
		{
			if (_timeRangeForAnnotationBeingRerecorded == null)
				return;

			var dstFile = GetFullPathOfAnnotationFileForTimeRange(_timeRangeForAnnotationBeingRerecorded);
			var srcFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(dstFile));
			CopyFilesViewModel.Copy(srcFile, dstFile, true);
		}

		/// ------------------------------------------------------------------------------------
		public void DeleteTemporarilySavedAnnotation()
		{
			if (_timeRangeForAnnotationBeingRerecorded == null)
				return;

			var path = GetFullPathOfAnnotationFileForTimeRange(_timeRangeForAnnotationBeingRerecorded);
			path = Path.Combine(Path.GetTempPath(), Path.GetFileName(path));
			File.Delete(path);
			_timeRangeForAnnotationBeingRerecorded = null;
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

			AudioUtils.NAudioErrorAction = exception =>
			{
				CloseAnnotationPlayer();
				if (PlaybackErrorAction != null)
					PlaybackErrorAction(exception);
			};

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
			AudioUtils.NAudioErrorAction = null;

			if (_annotationPlayer != null)
				_annotationPlayer.Dispose();

			_annotationPlayer = null;
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
		public void EraseAnnotation(TimeRange timeRange)
		{
			EraseAnnotation(GetFullPathOfAnnotationFileForTimeRange(timeRange));
		}

		/// ------------------------------------------------------------------------------------
		public void EraseAnnotation(Segment segment)
		{
			EraseAnnotation(GetFullPathToAnnotationFileForSegment(segment));
		}

		/// ------------------------------------------------------------------------------------
		private void EraseAnnotation(string path)
		{
			CloseAnnotationPlayer();
			ComponentFile.WaitForFileRelease(path);

			try
			{
				if (File.Exists(path))
				{
					BackupOralAnnotationSegmentFile(path);
					File.Delete(path);
					SegmentsAnnotationSamplesToDraw.RemoveWhere(h => h.AudioFilePath == path);
				}

				//InitializeAnnotationPlayer();
				UsageReporter.SendNavigationNotice(ProgramAreaForUsageReporting + "/EraseAnnotation");
			}
			catch (Exception e)
			{
				ErrorReport.NotifyUserOfProblem(e,
					"Could not remove that annotation. If this problem persists, try restarting your computer.");
			}
		}

		#endregion
	}

	#region CarefulSpeechAnnotationRecorderDlgViewModel class
	/// ----------------------------------------------------------------------------------------
	public class CarefulSpeechAnnotationRecorderDlgViewModel : OralAnnotationRecorderDlgViewModel
	{
		/// ------------------------------------------------------------------------------------
		public CarefulSpeechAnnotationRecorderDlgViewModel(ComponentFile file) : base(file)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override string ProgramAreaForUsageReporting
		{
			get { return "Annotations/Oral/" + OralAnnotationType.Careful; }
		}

		/// ------------------------------------------------------------------------------------
		public override string GetFullPathOfAnnotationFileForTimeRange(TimeRange timeRange)
		{
			var segment = TimeTier.Segments.FirstOrDefault(s => s.TimeRange == timeRange) ??
				new Segment(null, timeRange);
			return TimeTier.GetFullPathToCarefulSpeechFile(segment);
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
			get { return "Annotations/Oral/" + OralAnnotationType.Translation; }
		}

		/// ------------------------------------------------------------------------------------
		public override string GetFullPathOfAnnotationFileForTimeRange(TimeRange timeRange)
		{
			var segment = TimeTier.Segments.FirstOrDefault(s => s.TimeRange == timeRange) ??
				new Segment(null, timeRange);
			return TimeTier.GetFullPathToOralTranslationFile(segment);
		}
	}

	#endregion
}
