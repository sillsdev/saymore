using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NAudio.Wave;
using Palaso.Media.Naudio;
using Palaso.Reporting;
using SayMore.Media;
using SayMore.Model.Files;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public class OralAnnotationRecorderDlgViewModel : SegmenterDlgBaseViewModel
	{
		public Segment CurrentUnannotatedSegment { get; private set; }
		private AudioPlayer _annotationPlayer;
		private AudioRecorder _annotationRecorder;
		private readonly List<string> _fullPathsToAddedRecordings = new List<string>();

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
		}

		#region Properties
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

		/// ------------------------------------------------------------------------------------
		public void RemoveInvalidAnnotationFiles()
		{
			var annotationFiles = from s in TimeTier.Segments
								  where GetDoesSegmentHaveAnnotationFile(s)
								  select GetFullPathToAnnotationFileForSegment(s);

			foreach (var path in annotationFiles.Where(p => !AudioUtils.GetDoesFileSeemToBeWave(p)))
				EraseAnnotation(path);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsRecordingTooShort()
		{
			return (_annotationRecorder != null &&
				_annotationRecorder.RecordedTime <= TimeSpan.FromMilliseconds(500));
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesSegmentHaveAnnotationFile(int segmentIndex)
		{
			if (segmentIndex < 0 && segmentIndex >= TimeTier.Segments.Count)
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

		#region Annotation record/player methods
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
			if (_annotationPlayer != null)
				_annotationPlayer.Dispose();

			_annotationPlayer = null;
		}

		/// ------------------------------------------------------------------------------------
		public void CloseAnnotationRecorder()
		{
			if (_annotationRecorder != null)
				_annotationRecorder.Dispose();

			_annotationRecorder = null;
		}

		/// ------------------------------------------------------------------------------------
		public bool BeginAnnotationRecording(TimeRange timeRange,
			Action<TimeSpan> recordingProgressAction, string genericErrorMsg)
		{
			return BeginAnnotationRecording(GetFullPathOfAnnotationFileForTimeRange(timeRange),
				recordingProgressAction, genericErrorMsg);
		}

		/// ------------------------------------------------------------------------------------
		private bool BeginAnnotationRecording(string path,
			Action<TimeSpan> recordingProgressAction, string genericErrorMsg)
		{
			if (GetIsRecording() || File.Exists(path))
				return false;

			if (!Directory.Exists(Path.GetDirectoryName(path)))
				Directory.CreateDirectory(Path.GetDirectoryName(path));

			try
			{
				_fullPathsToAddedRecordings.Add(path);

				if (_annotationRecorder == null)
				{
					_annotationRecorder = new AudioRecorder(20);
					_annotationRecorder.RecordingFormat = AudioUtils.GetDefaultWaveFormat(1);
					_annotationRecorder.SelectedDevice = RecordingDevice.Devices.First();
					_annotationRecorder.RecordingStarted += (s, e) => InvokeUpdateDisplayAction();
					_annotationRecorder.Stopped += (sender, args) => InvokeUpdateDisplayAction();
					_annotationRecorder.RecordingProgress += (s, e) => recordingProgressAction(e.RecordedLength);
					_annotationRecorder.BeginMonitoring();
				}

				_annotationRecorder.BeginRecording(path);
				return true;
			}
			catch (Exception e)
			{
				CloseAnnotationPlayer();
				CloseAnnotationRecorder();
				ErrorReport.NotifyUserOfProblem(e, genericErrorMsg);
				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool StopAnnotationRecording()
		{
			_annotationRecorder.Stop();
			AnnotationRecordingsChanged = (AnnotationRecordingsChanged || !GetIsRecordingTooShort());
			if (!GetIsRecordingTooShort())
				return true;

			_fullPathsToAddedRecordings.RemoveAt(_fullPathsToAddedRecordings.Count - 1);
			return false;
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

		/// ------------------------------------------------------------------------------------
		public bool GetIsRecording()
		{
			return (_annotationRecorder != null &&
				_annotationRecorder.RecordingState == RecordingState.Recording);
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
