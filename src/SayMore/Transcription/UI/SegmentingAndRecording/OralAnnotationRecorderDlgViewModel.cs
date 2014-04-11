//
// Red Information red.svg icon attribution:
// Ezekiel63745 at en.wikipedia [CC-BY-SA-3.0 (http://creativecommons.org/licenses/by-sa/3.0) or GFDL (http://www.gnu.org/copyleft/fdl.html)], via Wikimedia Commons
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesktopAnalytics;
using L10NSharp;
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
		/// <summary>Successful recording</summary>
		Normal,
		/// <summary>User recorded an annotation that was not long enough</summary>
		AnnotationTooShort,
		/// <summary>NAudio threw and exception</summary>
		RecordingError,
		/// <summary>Recording was stopped prematurely (probably because microphone was removed)</summary>
		RecordingAborted,
	}

	/// ----------------------------------------------------------------------------------------
	public class OralAnnotationRecorderDlgViewModel : SegmenterDlgBaseViewModel
	{
		public Action<Exception> RecordingErrorAction { get; set; }
		public Action<Exception> PlaybackErrorAction { get; set; }
		public Action<StopAnnotationRecordingResult> RecordingCompleted;
		public event EventHandler SelectedDeviceChanged;
		public Segment CurrentUnannotatedSegment { get; private set; }
		public OralAnnotationRecorder Recorder { get; private set; }
		private AudioPlayer _annotationPlayer;
		private TimeSpan _endBoundary;
		private TimeRange _segmentBeingRecorded;
		private RecordingDeviceIndicator _recordingDeviceIndicator;

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
		public override TimeSpan VirtualBoundaryBeyondLastSegment
		{
			get { return NewSegmentEndBoundary; }
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
			return Tiers.GetIsFullyAnnotated(AnnotationType);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsRecorderInErrorState()
		{
			return Recorder == null || Recorder.GetIsInErrorState();
		}
		#endregion

		#region Segment-related methods
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
		public bool GetDoesSegmentHaveAnnotationFile(Segment segment)
		{
			if (segment == null || segment.TimeRange == _segmentBeingRecorded)
				return false;
			var path = GetFullPathToAnnotationFileForSegment(segment);
			return File.Exists(path) && AudioUtils.GetDoesFileSeemToBeWave(path);
		}

		/// ------------------------------------------------------------------------------------
		public string GetFullPathToAnnotationFileForSegment(Segment segment)
		{
			return GetFullPathOfAnnotationFileForTimeRange(segment.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		public string GetFullPathToOtherAnnotationFileForSegment(Segment segment)
		{
			return GetFullPathOfOtherAnnotationFileForTimeRange(segment.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		public virtual string GetFullPathOfAnnotationFileForTimeRange(TimeRange timeRange)
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		public virtual string GetFullPathOfOtherAnnotationFileForTimeRange(TimeRange timeRange)
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
		protected override Action GetActionToRestoreStateWhenUndoingAnIgnore(Segment segment)
		{
			var timeRange = segment.TimeRange.Copy();

			Action restoreOtherFileIfNeeded = () => { };
			var otherPath = GetFullPathOfOtherAnnotationFileForTimeRange(timeRange);
			if (File.Exists(otherPath))
			{
				BackupOralAnnotationSegmentFile(otherPath, true);
				restoreOtherFileIfNeeded = () => RestorePreviousVersionOfAnnotation(otherPath);
			}

			var path = GetFullPathOfAnnotationFileForTimeRange(timeRange);
			if (File.Exists(path))
			{
				BackupOralAnnotationSegmentFile(path, true);
				return () => { RestorePreviousVersionOfAnnotation(timeRange); restoreOtherFileIfNeeded(); };
			}
			if (segment == CurrentUnannotatedSegment)
				return () => { CurrentUnannotatedSegment = segment; restoreOtherFileIfNeeded(); };
			return base.GetActionToRestoreStateWhenUndoingAnIgnore(segment);
		}

		/// ------------------------------------------------------------------------------------
		public bool SetNextUnannotatedSegment()
		{
			CurrentUnannotatedSegment = TimeTier.Segments.FirstOrDefault(s =>
				(CurrentUnannotatedSegment == null || s.End > CurrentUnannotatedSegment.End) &&
				SegmentNeedsAnnotation(s));

			if (CurrentUnannotatedSegment == null)
				CurrentUnannotatedSegment = TimeTier.Segments.FirstOrDefault(SegmentNeedsAnnotation);

			return CurrentUnannotatedSegment != null;
		}

		/// ------------------------------------------------------------------------------------
		private bool SegmentNeedsAnnotation(Segment s)
		{
			return !GetDoesSegmentHaveAnnotationFile(s) && !GetIsSegmentIgnored(s);
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
			NewSegmentEndBoundary = _endBoundary = GetEndOfLastSegment();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSegmentDeleted(Segment segment)
		{
			base.OnSegmentDeleted(segment);
			if (segment == CurrentUnannotatedSegment)
				SetNextUnannotatedSegment();
		}

		/// ----------------------------------------------------------------------------------------
		public Tuple<float, float>[,] GetSegmentSamples(Segment segment, uint numberOfSamplesToReturn)
		{
			// If the samples for this oral annotation have not been calculated, then create a
			// helper to get those samples and cache them.
			var audioFilePath = GetFullPathToAnnotationFileForSegment(segment);
			var helper = SegmentsAnnotationSamplesToDraw.FirstOrDefault(h => h.AudioFilePath == audioFilePath);
			if (helper == null)
			{
				helper = new AudioFileHelper(audioFilePath);
				SegmentsAnnotationSamplesToDraw.Add(helper);
			}
			return helper.GetSamples(numberOfSamplesToReturn);
		}
		#endregion

		#region Annotation recording methods
		/// ------------------------------------------------------------------------------------
		public void InitializeAnnotationRecorder(PeakMeterCtrl peakMeter,
			RecordingDeviceIndicator recordingDeviceIndicator, Action<TimeSpan> recordingProgressAction)
		{
			try
			{
				Recorder = new OralAnnotationRecorder(peakMeter, recordingProgressAction);
			}
			catch
			{
				return;
			}
			_recordingDeviceIndicator = recordingDeviceIndicator;
			if (_recordingDeviceIndicator != null)
				_recordingDeviceIndicator.Recorder = Recorder;
			Recorder.RecordingStarted += (s, e) => InvokeUpdateDisplayAction();
			Recorder.Stopped += (sender, args) =>
			{
				if (args != null && RecordingErrorAction != null)
				{
					RecordingErrorAction(args.GetException());
				}
				AnnotationRecordingFinished(args != null);
				InvokeUpdateDisplayAction();
			};
			Recorder.SelectedDeviceChanged += SelectedRecordingDeviceChanged;
			Recorder.BeginMonitoring();
		}

		/// ------------------------------------------------------------------------------------
		private void SelectedRecordingDeviceChanged(object sender, EventArgs e)
		{
			if (SelectedDeviceChanged != null)
				SelectedDeviceChanged(this, e);
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
				CurrentUnannotatedSegment = TimeTier.Segments.FirstOrDefault(s => s.TimeRange == timeRange);
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

			if (recordingStarted)
				_segmentBeingRecorded = timeRange;

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
		public void StopAnnotationRecording()
		{
			AudioUtils.NAudioExceptionThrown -= HandleNAudioExceptionThrownDuringRecord;
			Recorder.Stop();
		}

		/// ------------------------------------------------------------------------------------
		private void AnnotationRecordingFinished(bool recordingAborted)
		{
			TimeRange timeRange = _segmentBeingRecorded;
			_segmentBeingRecorded = null;
			var isRecorderInErrorState = Recorder.GetIsInErrorState();
			var isRecordingTooShort = Recorder.GetIsRecordingTooShort() && !isRecorderInErrorState;

			if (isRecordingTooShort || isRecorderInErrorState || recordingAborted)
			{
				RestorePreviousVersionOfAnnotation(timeRange);
				var segment = TimeTier.Segments.FirstOrDefault(s => s.TimeRange.Equals(timeRange));
				if (segment != null && segment == CurrentUnannotatedSegment && segment.GetHasOralAnnotation(AnnotationType))
					CurrentUnannotatedSegment = null;
			}
			else
			{
				_undoStack.Push(new SegmentChange(SegmentChangeType.AnnotationAdded, timeRange, timeRange,
					c => RestorePreviousVersionOfAnnotation(timeRange)));
			}

			if (RecordingCompleted != null)
			{
				if (recordingAborted)
					RecordingCompleted(StopAnnotationRecordingResult.RecordingAborted);
				else if (isRecorderInErrorState)
					RecordingCompleted(StopAnnotationRecordingResult.RecordingError);
				else
				{
					RecordingCompleted((isRecordingTooShort ?
						StopAnnotationRecordingResult.AnnotationTooShort :
						StopAnnotationRecordingResult.Normal));
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsRecording()
		{
			return (Recorder != null && Recorder.IsRecording && !Recorder.GetIsInErrorState());
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
				_annotationPlayer.StartPlaying();
			}

			Analytics.Track("Play Annotation", new Dictionary<string, string> {
				{ "ProgramAreaForUsageReporting", ProgramAreaForUsageReporting } });
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
			var segment = TimeTier.Segments.FirstOrDefault(s => s.TimeRange.Equals(timeRange));
			if (segment != null && !segment.GetHasOralAnnotation(AnnotationType))
				CurrentUnannotatedSegment = segment;
		}

		/// ------------------------------------------------------------------------------------
		protected override void EraseAnnotation(string path)
		{
			CloseAnnotationPlayer();
			base.EraseAnnotation(path);
			if (CurrentUnannotatedSegment == null)
				SetNextUnannotatedSegment();
		}
		#endregion

		#region Source playback methods
		/// ------------------------------------------------------------------------------------
		public void PlaySource(WaveControlBasic waveControl, Action<WaveControlBasic> play)
		{
			waveControl.PlaybackStopped -= OnPlaybackStopped;
			waveControl.PlaybackStopped += OnPlaybackStopped;

			if (_recordingDeviceIndicator != null)
				_recordingDeviceIndicator.MicCheckingEnabled = false;

			play(waveControl);
		}

		/// ------------------------------------------------------------------------------------
		private void OnPlaybackStopped(WaveControlBasic ctrl, TimeSpan start, TimeSpan end)
		{
			// Restore checking for recording device
			if (_recordingDeviceIndicator != null)
				_recordingDeviceIndicator.MicCheckingEnabled = false;

			// Remove this handler
			ctrl.PlaybackStopped -= OnPlaybackStopped;
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
			get { return "Careful Speech"; }
		}

		/// ------------------------------------------------------------------------------------
		public override string GetFullPathOfAnnotationFileForTimeRange(TimeRange timeRange)
		{
			var segment = TimeTier.Segments.FirstOrDefault(s => s.TimeRange == timeRange) ??
				new Segment(null, timeRange);
			return TimeTier.GetFullPathToCarefulSpeechFile(segment);
		}

		/// ------------------------------------------------------------------------------------
		public override string GetFullPathOfOtherAnnotationFileForTimeRange(TimeRange timeRange)
		{
			var segment = TimeTier.Segments.FirstOrDefault(s => s.TimeRange == timeRange) ??
				new Segment(null, timeRange);
			return TimeTier.GetFullPathToOralTranslationFile(segment);
		}

		/// ------------------------------------------------------------------------------------
		public override OralAnnotationType AnnotationType
		{
			get { return OralAnnotationType.CarefulSpeech; }
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
			get { return "Oral Translation"; }
		}

		/// ------------------------------------------------------------------------------------
		public override string GetFullPathOfAnnotationFileForTimeRange(TimeRange timeRange)
		{
			var segment = TimeTier.Segments.FirstOrDefault(s => s.TimeRange == timeRange) ??
				new Segment(null, timeRange);
			return TimeTier.GetFullPathToOralTranslationFile(segment);
		}

		/// ------------------------------------------------------------------------------------
		public override string GetFullPathOfOtherAnnotationFileForTimeRange(TimeRange timeRange)
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
}
