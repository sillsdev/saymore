using System;
using System.IO;
using NAudio.Wave;
using Palaso.Reporting;
using SayMore.AudioUtils;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.Transcription.UI
{
	public enum OralAnnotationType
	{
		Careful,
		Translation
	}

	public class OralAnnotationRecorderDlgViewModel : SegmenterDlgBaseViewModel
	{
		private readonly string _pathToAnnotationsFolder;
		private AudioPlayer _annotationPlayer;
		private AudioRecorder _annotationRecorder;

		public OralAnnotationType AnnotationType { get; private set; }

		/// ------------------------------------------------------------------------------------
		public OralAnnotationRecorderDlgViewModel(ComponentFile file,
			OralAnnotationType annotationType) : base(file)
		{
			AnnotationType = annotationType;

			_pathToAnnotationsFolder = ComponentFile.PathToAnnotatedFile +
				Settings.Default.OralAnnotationsFolderAffix;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		protected override string ProgramAreaForUsageReporting
		{
			get { return "Annotations/Oral/" + AnnotationType; }
		}

		/// ------------------------------------------------------------------------------------
		public bool AnnotationRecordingsChanged { get; private set; }

		/// ------------------------------------------------------------------------------------
		public bool DoesAnnotationExistForCurrentSegment
		{
			get { return File.Exists(GetPathToCurrentAnnotationFile()); }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsRecordingTooShort
		{
			get { return (_annotationRecorder.RecordedTime <= TimeSpan.FromMilliseconds(500)); }
		}

		/// ------------------------------------------------------------------------------------
		public override bool WereChangesMade
		{
			get { return base.WereChangesMade || AnnotationRecordingsChanged; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public string GetPathToAnnotationFileForSegment(int segmentNumber)
		{
			if (segmentNumber == _segments.Count)
				return string.Empty;

			return GetPathToAnnotationFileForSegment(_segments[segmentNumber]);
		}

		/// ------------------------------------------------------------------------------------
		private string GetPathToAnnotationFileForSegment(SegBoundary segment)
		{
			return GetPathToAnnotationFileForSegment(segment.start, segment.end);
		}

		/// ------------------------------------------------------------------------------------
		public string GetPathToAnnotationFileForSegment(TimeSpan start, TimeSpan end)
		{
			var affix = (AnnotationType == OralAnnotationType.Careful ?
				Settings.Default.OralAnnotationCarefulSegmentFileAffix :
				Settings.Default.OralAnnotationTranslationSegmentFileAffix);

			return Path.Combine(_pathToAnnotationsFolder,
				string.Format(Settings.Default.OralAnnotationSegmentFileFormat,
				(float)start.TotalSeconds, (float)end.TotalSeconds, affix));
		}

		/// ------------------------------------------------------------------------------------
		public string GetPathToCurrentAnnotationFile()
		{
			return GetPathToAnnotationFileForSegment(CurrentSegmentNumber);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesSegmentHaveAnnotationFile(int segmentNumber)
		{
			return File.Exists(GetPathToAnnotationFileForSegment(segmentNumber));
		}

		/// ------------------------------------------------------------------------------------
		public override void SelectSegment(int segmentNumber)
		{
			CloseAnnotationRecorder();

			if (_annotationPlayer != null && _annotationPlayer.PlaybackState == PlaybackState.Playing)
				_annotationPlayer.Stop();

			InitializeAnnotationRecorder();
			base.SelectSegment(segmentNumber);
		}

		#region Annotation record/player methods
		/// ------------------------------------------------------------------------------------
		private void InitializeAnnotationRecorder()
		{
			_annotationRecorder = new AudioRecorder(WaveFileUtils.GetDefaultWaveFormat(1));
			_annotationRecorder.RecordingStarted += delegate
			{
				IsIdle = false;
				InvokeUpdateDisplayAction();
			};

			_annotationRecorder.Stopped += delegate
			{
				IsIdle = true;
				InvokeUpdateDisplayAction();
			};
		}

		/// ------------------------------------------------------------------------------------
		public bool InitializeAnnotationPlayer()
		{
			CloseAnnotationPlayer();

			var filename = GetPathToCurrentAnnotationFile();
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
			_annotationPlayer.PlaybackStarted += delegate
			{
				IsIdle = false;
				InvokeUpdateDisplayAction();
			};

			_annotationPlayer.Stopped += delegate
			{
				IsIdle = true;
				InvokeUpdateDisplayAction();
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
		public bool BeginAnnotationRecording()
		{
			if (!IsIdle || DoesAnnotationExistForCurrentSegment || !HaveSegmentBoundaries)
				return false;

			var path = GetPathToAnnotationFileForSegment(PlaybackStartPosition, PlaybackEndPosition);
			_annotationRecorder.BeginRecording(path);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public bool StopAnnotationRecording()
		{
			_annotationRecorder.Stop();
			AnnotationRecordingsChanged = (AnnotationRecordingsChanged || !IsRecordingTooShort);
			return !IsRecordingTooShort;
		}

		/// ------------------------------------------------------------------------------------
		public void StartAnnotationPlayback()
		{
			if (!IsIdle || !DoesAnnotationExistForCurrentSegment)
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
		public void EraseAnnotation()
		{
			CloseAnnotationPlayer();
			var path = GetPathToCurrentAnnotationFile();
			ComponentFile.WaitForFileRelease(path);

			try
			{
				if (File.Exists(path))
					File.Delete(path);

				InitializeAnnotationPlayer();
				UsageReporter.SendNavigationNotice(ProgramAreaForUsageReporting + "/EraseAnnotation");
			}
			catch (Exception error)
			{
				ErrorReport.NotifyUserOfProblem(error,
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

		/// ------------------------------------------------------------------------------------
		protected override void RenameAnnotationForResizedSegment(SegBoundary oldSegment, SegBoundary newSegment)
		{
			try
			{
				var oldFilePath = GetPathToAnnotationFileForSegment(oldSegment);
				if (File.Exists(oldFilePath))
					File.Move(oldFilePath, GetPathToAnnotationFileForSegment(newSegment));
			}
			catch { }
		}
	}
}
