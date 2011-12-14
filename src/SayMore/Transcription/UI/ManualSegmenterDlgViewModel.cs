using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAudio.Wave;
using Palaso.Reporting;
using SayMore.AudioUtils;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	public class ManualSegmenterDlgViewModel : IDisposable
	{
		private class SegBoundary
		{
			public SegBoundary(TimeSpan s, TimeSpan e) { start = s; end = e; }
			public TimeSpan start;
			public TimeSpan end;
			public override string ToString()
			{
				return start + " - " + end;
			}
		}

		private readonly string _pathToAnnotationsFolder;
		private AudioPlayer _annotationPlayer;
		private AudioRecorder _annotationRecorder;
		private readonly List<SegBoundary> _segments;

		public int CurrentSegmentNumber { get; private set; }
		public bool IsIdle { get; private set; }
		public ComponentFile ComponentFile { get; private set; }
		public OralAnnotationType AnnotationType;
		public WaveStream OrigWaveStream { get; private set; }
		public TimeSpan PlaybackStartPosition { get; set; }
		public TimeSpan PlaybackEndPosition { get; set; }
		public bool HaveSegmentBoundaries { get; set; }
		public Action UpdateDisplayProvider { get; set; }
		public Action SegmentNumberChangedHandler { get; set; }

		/// ------------------------------------------------------------------------------------
		public ManualSegmenterDlgViewModel(ComponentFile file)
		{
			IsIdle = true;
			ComponentFile = file;
			OrigWaveStream = new WaveFileReader(ComponentFile.PathToAnnotatedFile);

			_segments = (InitializeSegments(ComponentFile) ?? new List<SegBoundary>()).ToList();

			PlaybackStartPosition = TimeSpan.Zero;
			PlaybackEndPosition = TimeSpan.Zero;

			_pathToAnnotationsFolder = ComponentFile.PathToAnnotatedFile +
				Settings.Default.OralAnnotationsFolderAffix;
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (OrigWaveStream != null)
			{
				OrigWaveStream.Close();
				OrigWaveStream.Dispose();
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public bool SegmentBoundariesChanged { get; private set; }

		/// ------------------------------------------------------------------------------------
		public bool AnnotationRecordingsChanged { get; private set; }

		/// ------------------------------------------------------------------------------------
		public bool DoesAnnotationExistForCurrentSegment
		{
			get { return (File.Exists(GetPathToCurrentAnnotationFile())); }
		}

		/// ------------------------------------------------------------------------------------
		public bool DoSegmentsExist
		{
			get { return _segments.Count > 0; }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsRecordingTooShort
		{
			get { return (_annotationRecorder.RecordedTime <= TimeSpan.FromMilliseconds(500)); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the current segment's boundaries have
		/// been saved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsCurrentSegmentConfirmed
		{
			get { return CurrentSegmentNumber < _segments.Count; }
		}

		/// ------------------------------------------------------------------------------------
		private string ProgramAreaForUsageReporting
		{
			get { return "ManualAnnotations/Oral/" + AnnotationType; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private IEnumerable<SegBoundary> InitializeSegments(ComponentFile file)
		{
			if (file.GetAnnotationFile() == null)
				return null;

			var toTier = file.GetAnnotationFile().Tiers.FirstOrDefault(t => t is TimeOrderTier);
			if (toTier == null)
				return null;

			return from seg in toTier.GetAllSegments().Cast<ITimeOrderSegment>()
				   select new SegBoundary(TimeSpan.FromSeconds(seg.Start), TimeSpan.FromSeconds(seg.Stop));
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<TimeSpan> GetSegmentBoundaries()
		{
			return _segments.Select(s => s.end);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetSegments()
		{
			return GetSegmentBoundaries().Select(b => b.TotalSeconds.ToString());
		}

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
		public void GotoEndOfSegments()
		{
			SelectSegment(_segments.Count);
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

			if (segmentNumber < 0)
				segmentNumber = _segments.Count;

			CurrentSegmentNumber = segmentNumber;
			HaveSegmentBoundaries = (segmentNumber < _segments.Count);

			if (segmentNumber == _segments.Count)
			{
				PlaybackStartPosition = (!DoSegmentsExist ? TimeSpan.Zero : _segments[segmentNumber - 1].end);
				PlaybackEndPosition = PlaybackStartPosition;
			}
			else
			{
				PlaybackStartPosition = _segments[segmentNumber].start;
				PlaybackEndPosition = _segments[segmentNumber].end;
			}

			InitializeAnnotationRecorder();

			if (SegmentNumberChangedHandler != null)
				SegmentNumberChangedHandler();
		}

		/// ------------------------------------------------------------------------------------
		public bool MoveExistingSegmentBoundary(int millisecondsToMove)
		{
			var timeAdjustment = TimeSpan.FromMilliseconds(Math.Abs(millisecondsToMove));
			var newEndPosition = PlaybackEndPosition + (millisecondsToMove < 0 ? -timeAdjustment : timeAdjustment);

			if (newEndPosition <= PlaybackStartPosition)
				return false;

			PlaybackEndPosition = (newEndPosition >= OrigWaveStream.TotalTime ?
				OrigWaveStream.TotalTime : newEndPosition);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<TimeSpan> SaveNewSegmentBoundary()
		{
			if (CurrentSegmentNumber == _segments.Count)
			{
				_segments.Add(new SegBoundary(PlaybackStartPosition, PlaybackEndPosition));
				SegmentBoundariesChanged = true;
			}

			SelectSegment(_segments.Count);
			return _segments.Select(s => s.end);
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

		/// ------------------------------------------------------------------------------------
		public int GetSegmentCount()
		{
			return _segments.Count;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetStartPositionOfSubSegmentAtEndOfCurrentSegment()
		{
			if (!IsIdle || !HaveSegmentBoundaries)
				return TimeSpan.MaxValue;

			var subSegStartPosition = PlaybackEndPosition -
				TimeSpan.FromMilliseconds(Settings.Default.MillisecondsToRePlayAfterAdjustingSegmentBoundary);

			return (subSegStartPosition < PlaybackStartPosition ?
				PlaybackStartPosition : subSegStartPosition);
		}

		/// ------------------------------------------------------------------------------------
		public void PlaybackOfOriginalRecordingStarted()
		{
			IsIdle = false;
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan PlaybackOfOriginalRecordingStopped(TimeSpan start, TimeSpan end)
		{
			IsIdle = true;

			if (!HaveSegmentBoundaries)
			{
				HaveSegmentBoundaries = true;
				PlaybackEndPosition = end;
			}

			return (IsCurrentSegmentConfirmed ? PlaybackStartPosition : PlaybackEndPosition);
		}

		/// ------------------------------------------------------------------------------------
		public void HandleSegmentBoundaryMoved(WaveControl waveCtrl, TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			if (oldEndTime == newEndTime)
				return;

			for (int i = 0; i < _segments.Count; i++)
			{
				if (_segments[i].end != oldEndTime)
					continue;

				if (i + 1 < _segments.Count)
				{
					RenameAnnotationForResizedSegment(_segments[i + 1],
						new SegBoundary(newEndTime, _segments[i + 1].end));
					_segments[i + 1].start = newEndTime;
				}

				RenameAnnotationForResizedSegment(_segments[i],
					new SegBoundary(_segments[i].start, newEndTime));

				_segments[i].end = newEndTime;
				SelectSegment(CurrentSegmentNumber);
				waveCtrl.SetSelectionTimes(PlaybackStartPosition, PlaybackEndPosition);
				SegmentBoundariesChanged = true;
				break;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void RenameAnnotationForResizedSegment(SegBoundary oldSegment, SegBoundary newSegment)
		{
			var oldFilePath = GetPathToAnnotationFileForSegment(oldSegment);
			var newFilePath = GetPathToAnnotationFileForSegment(newSegment);

			try
			{
				File.Move(oldFilePath, newFilePath);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		private void InvokeUpdateDisplayAction()
		{
			if (UpdateDisplayProvider != null)
				UpdateDisplayProvider();
		}
	}
}
