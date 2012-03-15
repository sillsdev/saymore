using System;
using System.Collections.Generic;
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
		public Segment CurrentSegment { get; private set; }
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
			CurrentSegment = (TimeTier.Segments.Count == 0 ? null : TimeTier.Segments[0]);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public bool AnnotationRecordingsChanged { get; private set; }

		/// ------------------------------------------------------------------------------------
		public override bool WereChangesMade
		{
			get { return base.WereChangesMade || AnnotationRecordingsChanged; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public bool GetIsRecordingTooShort()
		{
			return (_annotationRecorder != null &&
				_annotationRecorder.RecordedTime <= TimeSpan.FromMilliseconds(500));
		}

		/// ------------------------------------------------------------------------------------
		public override bool GetIsSegmentLongEnough(TimeSpan proposedEndTime)
		{
			var segEndTime = (CurrentSegment == null ? proposedEndTime : GetEndOfCurrentSegment());
			return base.GetIsSegmentLongEnough(segEndTime);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesCurrentSegmentHaveAnnotationFile()
		{
			return GetDoesSegmentHaveAnnotationFile(CurrentSegment);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesSegmentHaveAnnotationFile(int segmentIndex)
		{
			if (segmentIndex < 0 && segmentIndex >= TimeTier.Segments.Count)
				return false;

			return GetDoesSegmentHaveAnnotationFile(TimeTier.Segments[segmentIndex]);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesSegmentHaveAnnotationFile(Segment segment)
		{
			return (segment != null && File.Exists(GetFullPathToAnnotationFileForSegment(segment)));
		}

		/// ------------------------------------------------------------------------------------
		public virtual string GetFullPathToAnnotationFileForSegment(Segment segment)
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		public virtual string GetFullPathForNewSegmentAnnotationFile(TimeSpan start, TimeSpan end)
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetTimeWherePlaybackShouldStart(TimeSpan proposedTime)
		{
			var start = GetStartOfCurrentSegment();
			return (start < proposedTime ? proposedTime : start);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetStartOfCurrentSegment()
		{
			return (CurrentSegment == null ? GetEndOfLastSegment() : TimeSpan.FromSeconds(CurrentSegment.Start));
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetEndOfCurrentSegment()
		{
			return (CurrentSegment == null ? TimeSpan.Zero : TimeSpan.FromSeconds(CurrentSegment.End));
		}

		/// ------------------------------------------------------------------------------------
		public void GotoEndOfSegments()
		{
			SelectSegmentFromTime(GetEndOfLastSegment());
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesHaveSegments()
		{
			return (Tiers.GetDoTimeSegmentsExist());
		}

		/// ------------------------------------------------------------------------------------
		public void SelectSegmentFromTime(TimeSpan time)
		{
			CloseAnnotationRecorder();

			if (_annotationPlayer != null && _annotationPlayer.PlaybackState == PlaybackState.Playing)
				_annotationPlayer.Stop();

			CurrentSegment = TimeTier.GetSegmentHavingEndBoundary((float)time.TotalSeconds);
			InvokeUpdateDisplayAction();
		}

		#region Annotation record/player methods
		/// ------------------------------------------------------------------------------------
		public bool InitializeAnnotationPlayer()
		{
			CloseAnnotationPlayer();

			var filename = GetFullPathToAnnotationFileForSegment(CurrentSegment);
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
		public bool BeginAnnotationRecording(TimeSpan cursorTime)
		{
			if (GetIsRecording() || GetDoesCurrentSegmentHaveAnnotationFile())
				return false;

			var path = (CurrentSegment != null ?
				GetFullPathToAnnotationFileForSegment(CurrentSegment) :
				GetFullPathForNewSegmentAnnotationFile(GetStartOfCurrentSegment(), cursorTime));

			_fullPathsToAddedRecordings.Add(path);

			_annotationRecorder = new AudioRecorder(20);
			_annotationRecorder.RecordingFormat = AudioUtils.GetDefaultWaveFormat(1);
			_annotationRecorder.SelectedDevice = RecordingDevice.Devices.First();
			_annotationRecorder.RecordingStarted += (s, e) => InvokeUpdateDisplayAction();
			_annotationRecorder.Stopped += (sender, args) => InvokeUpdateDisplayAction();
			_annotationRecorder.BeginMonitoring();
			_annotationRecorder.BeginRecording(path);

			return true;
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
		public void StartAnnotationPlayback()
		{
			if (!GetDoesCurrentSegmentHaveAnnotationFile())
				return;

			if (InitializeAnnotationPlayer())
				_annotationPlayer.Play();

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
		public void EraseAnnotation()
		{
			CloseAnnotationPlayer();
			var path = GetFullPathToAnnotationFileForSegment(CurrentSegment);
			ComponentFile.WaitForFileRelease(path);

			try
			{
				if (File.Exists(path))
				{
					BackupOralAnnotationSegmentFile(path);
					File.Delete(path);
				}

				InitializeAnnotationPlayer();
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
		public override string GetFullPathToAnnotationFileForSegment(Segment segment)
		{
			return Path.Combine(OralAnnotationsFolder, segment.GetFullPathToCarefulSpeechFile());
		}

		/// ------------------------------------------------------------------------------------
		public override string GetFullPathForNewSegmentAnnotationFile(TimeSpan start, TimeSpan end)
		{
			return TimeTier.GetFullPathToCarefulSpeechFile(
				new Segment(null, (float)start.TotalSeconds, (float)end.TotalSeconds));
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
		public override string GetFullPathToAnnotationFileForSegment(Segment segment)
		{
			return Path.Combine(OralAnnotationsFolder, segment.GetFullPathToOralTranslationFile());
		}

		/// ------------------------------------------------------------------------------------
		public override string GetFullPathForNewSegmentAnnotationFile(TimeSpan start, TimeSpan end)
		{
			return TimeTier.GetFullPathToOralTranslationFile(
				new Segment(null, (float)start.TotalSeconds, (float)end.TotalSeconds));
		}
	}

	#endregion
}
