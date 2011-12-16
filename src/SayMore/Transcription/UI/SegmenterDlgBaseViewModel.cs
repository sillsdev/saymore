using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using SayMore.AudioUtils;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	public class SegmenterDlgBaseViewModel : IDisposable
	{
		protected class SegBoundary
		{
			public TimeSpan start;
			public TimeSpan end;
			public SegBoundary(TimeSpan s, TimeSpan e) { start = s; end = e; }
			public override string ToString() { return start + " - " + end; }
		}

		protected readonly List<SegBoundary> _segments;

		public int CurrentSegmentNumber { get; private set; }
		public bool IsIdle { get; protected set; }
		public ComponentFile ComponentFile { get; private set; }
		public WaveStream OrigWaveStream { get; private set; }
		public TimeSpan PlaybackStartPosition { get; set; }
		public TimeSpan PlaybackEndPosition { get; set; }
		public bool HaveSegmentBoundaries { get; set; }
		public Action UpdateDisplayProvider { get; set; }
		public Action SegmentNumberChangedHandler { get; set; }

		/// ------------------------------------------------------------------------------------
		public SegmenterDlgBaseViewModel(ComponentFile file)
		{
			IsIdle = true;
			ComponentFile = file;
			OrigWaveStream = new WaveFileReader(ComponentFile.PathToAnnotatedFile);

			_segments = (InitializeSegments(ComponentFile) ?? new List<SegBoundary>()).ToList();

			PlaybackStartPosition = TimeSpan.Zero;
			PlaybackEndPosition = TimeSpan.Zero;
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
		public bool DoSegmentsExist
		{
			get { return _segments.Count > 0; }
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
		public bool HaveUnconfirmedSegmentBoundariesBeenEstablished
		{
			get { return (!IsCurrentSegmentConfirmed && PlaybackEndPosition > PlaybackStartPosition); }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsSegmentLongEnough
		{
			get { return (PlaybackEndPosition.TotalSeconds - PlaybackStartPosition.TotalSeconds >= 1); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string ProgramAreaForUsageReporting
		{
			get { return "ManualSegmentation"; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool WereChangesMade
		{
			get { return SegmentBoundariesChanged; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected IEnumerable<SegBoundary> InitializeSegments(ComponentFile file)
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
		public virtual void SelectSegment(int segmentNumber)
		{
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

		/// ------------------------------------------------------------------------------------
		public int GetSegmentCount()
		{
			return _segments.Count;
		}

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
		protected virtual void RenameAnnotationForResizedSegment(SegBoundary oldSegment, SegBoundary newSegment)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected void InvokeUpdateDisplayAction()
		{
			if (UpdateDisplayProvider != null)
				UpdateDisplayProvider();
		}
	}
}
