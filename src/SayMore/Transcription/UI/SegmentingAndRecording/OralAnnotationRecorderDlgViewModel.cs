using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using Palaso.Media.Naudio;
using Palaso.Reporting;
using SayMore.AudioUtils;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.UI.NewEventsFromFiles;
using SayMore.UI.Utilities;

namespace SayMore.Transcription.UI
{
	public enum OralAnnotationType
	{
		Careful,
		Translation
	}

	/// ----------------------------------------------------------------------------------------
	public class OralAnnotationRecorderDlgViewModel : SegmenterDlgBaseViewModel
	{
		public int CurrentSegmentNumber { get; private set; }
		public OralAnnotationType AnnotationType { get; private set; }

		private readonly string _tempOralAnnotationsFolder;
		private readonly string _oralAnnotationsFolder;
		private AudioPlayer _annotationPlayer;
		private AudioRecorder _annotationRecorder;

		/// ------------------------------------------------------------------------------------
		public OralAnnotationRecorderDlgViewModel(ComponentFile file,
			OralAnnotationType annotationType) : base(file)
		{
			AnnotationType = annotationType;
			CurrentSegmentNumber = -1;

			_oralAnnotationsFolder = ComponentFile.PathToAnnotatedFile +
				Settings.Default.OralAnnotationsFolderAffix;

			_tempOralAnnotationsFolder = CopyOralAnnotationsToTempLocation();
		}

		/// ------------------------------------------------------------------------------------
		public override void Dispose()
		{
			FileSystemUtils.RemoveDirectory(_tempOralAnnotationsFolder);
			base.Dispose();
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
		public override bool WereChangesMade
		{
			get { return base.WereChangesMade || AnnotationRecordingsChanged; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public string CopyOralAnnotationsToTempLocation()
		{
			var tmpFolder = Path.Combine(Path.GetTempPath(), "SayMoreOralAnnotations");

			if (Directory.Exists(_oralAnnotationsFolder))
				CopyAnnotationFiles(_oralAnnotationsFolder, tmpFolder);
			else
				FileSystemUtils.CreateDirectory(tmpFolder);

			return tmpFolder;
		}

		/// ------------------------------------------------------------------------------------
		public bool SaveNewOralAnnoationsInPermanentLocation()
		{
			return CopyAnnotationFiles(_tempOralAnnotationsFolder, _oralAnnotationsFolder);
		}

		/// ------------------------------------------------------------------------------------
		private bool CopyAnnotationFiles(string sourceFolder, string targetFolder)
		{
			FileSystemUtils.RemoveDirectory(targetFolder);

			int retryCount = 0;
			Exception error = null;

			while (retryCount < 10)
			{
				try
				{
					FileSystemUtils.CreateDirectory(targetFolder);

					var pairs = Directory.GetFiles(sourceFolder, "*.wav", SearchOption.TopDirectoryOnly)
						.Select(f => new KeyValuePair<string, string>(f, Path.Combine(targetFolder, Path.GetFileName(f))));

					var model = new CopyFilesViewModel(pairs);
					model.Start();
					return true;
				}
				catch (Exception e)
				{
					Application.DoEvents();
					retryCount++;
					error = e;
				}
			}

			ErrorReport.NotifyUserOfProblem(error,
				"Error trying to copy oral annotation files from '{0}' to '{1}'", sourceFolder, targetFolder);

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsRecordingTooShort()
		{
			return (_annotationRecorder != null &&
				_annotationRecorder.RecordedTime <= TimeSpan.FromMilliseconds(500));
		}

		/// ------------------------------------------------------------------------------------
		public override bool GetIsSegmentLongEnough(TimeSpan proposedEndTime)
		{
			var segEndTime = (CurrentSegmentNumber < 0 ? proposedEndTime : GetEndOfCurrentSegment());
			return base.GetIsSegmentLongEnough(segEndTime);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesAnnotationExistForCurrentSegment()
		{
			return File.Exists(GetPathToCurrentAnnotationFile());
		}

		/// ------------------------------------------------------------------------------------
		public string GetPathToAnnotationFileForSegment(int segmentNumber)
		{
			if (segmentNumber < 0 || segmentNumber == _segments.Count)
				return string.Empty;

			return GetPathToAnnotationFileForSegment(_segments[segmentNumber]);
		}

		/// ------------------------------------------------------------------------------------
		private string GetPathToAnnotationFileForSegment(SegmentBoundaries segment)
		{
			return GetPathToAnnotationFileForSegment(segment.start, segment.end);
		}

		/// ------------------------------------------------------------------------------------
		public string GetPathToAnnotationFileForSegment(TimeSpan start, TimeSpan end)
		{
			var filename = (AnnotationType == OralAnnotationType.Careful ?
				TimeTier.ComputeFileNameForCarefulSpeechSegment((float)start.TotalSeconds, (float)end.TotalSeconds) :
				TimeTier.ComputeFileNameForOralTranslationSegment((float)start.TotalSeconds, (float)end.TotalSeconds));

			return Path.Combine(_tempOralAnnotationsFolder, filename);
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
		public TimeSpan GetTimeWherePlaybackShouldStart(TimeSpan proposedTime)
		{
			var start = GetStartOfCurrentSegment();
			return (start < proposedTime ? proposedTime : start);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetStartOfCurrentSegment()
		{
			return (CurrentSegmentNumber < 0 ? GetEndOfLastSegment()  :
				_segments[CurrentSegmentNumber].start);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetEndOfCurrentSegment()
		{
			return (CurrentSegmentNumber < 0 ? TimeSpan.Zero : _segments[CurrentSegmentNumber].end);
		}

		/// ------------------------------------------------------------------------------------
		public void GotoEndOfSegments()
		{
			SelectSegment(_segments.Count);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesHaveSegments()
		{
			return (_segments.Count > 0);
		}

		/// ------------------------------------------------------------------------------------
		public void SelectSegmentFromTime(TimeSpan time)
		{
			int i = 0;
			for (; i < _segments.Count; i++)
			{
				if (time >= _segments[i].start && time <= _segments[i].end)
					break;
			}

			SelectSegment(i);
		}

		/// ------------------------------------------------------------------------------------
		public void SelectSegment(int segmentNumber)
		{
			CloseAnnotationRecorder();

			if (_annotationPlayer != null && _annotationPlayer.PlaybackState == PlaybackState.Playing)
				_annotationPlayer.Stop();

			CurrentSegmentNumber = (segmentNumber >= 0 && segmentNumber < _segments.Count ? segmentNumber : -1);
			InvokeUpdateDisplayAction();
		}

		#region Annotation record/player methods
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
			if (GetIsRecording())
				return false;

			var segEndTime = (CurrentSegmentNumber < 0 ? cursorTime : GetEndOfCurrentSegment());

			if (GetDoesAnnotationExistForCurrentSegment())
				return false;

			var path = GetPathToAnnotationFileForSegment(GetStartOfCurrentSegment(), segEndTime);

			_annotationRecorder = new AudioRecorder(20);
			_annotationRecorder.RecordingFormat = WaveFileUtils.GetDefaultWaveFormat(1);
			_annotationRecorder.SelectedDevice = RecordingDevice.Devices.First();
			_annotationRecorder.RecordingStarted += (sender, args) => InvokeUpdateDisplayAction();
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
			return !GetIsRecordingTooShort();
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<TimeSpan> SaveNewSegment(TimeSpan endTime)
		{
			if (CurrentSegmentNumber < 0)
			{
				var start = GetStartOfCurrentSegment();
				_segments.Add(new SegmentBoundaries(start, endTime));
				_segments.Sort((x, y) => x.start.CompareTo(y.start));
				SegmentBoundariesChanged = true;
			}

			return _segments.Select(s => s.end);
		}

		/// ------------------------------------------------------------------------------------
		public void StartAnnotationPlayback()
		{
			if (!GetDoesAnnotationExistForCurrentSegment())
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
		protected override void RenameAnnotationForResizedSegment(SegmentBoundaries oldSegment, SegmentBoundaries newSegment)
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
