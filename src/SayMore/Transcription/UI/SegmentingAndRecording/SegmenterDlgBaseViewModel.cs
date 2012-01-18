using System;
using System.Collections.Generic;
using System.Globalization;
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
		protected class SegmentBoundaries
		{
			public TimeSpan start;
			public TimeSpan end;
			public SegmentBoundaries(TimeSpan s, TimeSpan e) { start = s; end = e; }
			public override string ToString() { return start + " - " + end; }
		}

		public ComponentFile ComponentFile { get; protected set; }
		public WaveStream OrigWaveStream { get; protected set; }
		public bool HaveSegmentBoundaries { get; set; }
		public Action UpdateDisplayProvider { get; set; }

		protected readonly List<SegmentBoundaries> _segments;

		/// ------------------------------------------------------------------------------------
		public SegmenterDlgBaseViewModel(ComponentFile file)
		{
			ComponentFile = file;
			OrigWaveStream = GetStreamFromAudio(ComponentFile.PathToAnnotatedFile);
			_segments = (InitializeSegments(ComponentFile) ?? new List<SegmentBoundaries>()).ToList();
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Dispose()
		{
			if (OrigWaveStream != null)
			{
				OrigWaveStream.Close();
				OrigWaveStream.Dispose();
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public bool SegmentBoundariesChanged { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public bool DoSegmentsExist
		{
			get { return _segments.Count > 0; }
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
		protected virtual WaveStream GetStreamFromAudio(string audioFilePath)
		{
			Exception error;

			return (WaveFileUtils.GetOneChannelStreamFromAudio(audioFilePath, out error) ??
				new WaveFileReader(audioFilePath));
		}

		/// ------------------------------------------------------------------------------------
		protected IEnumerable<SegmentBoundaries> InitializeSegments(ComponentFile file)
		{
			if (file.GetAnnotationFile() == null)
				return null;

			var toTier = file.GetAnnotationFile().Tiers.FirstOrDefault(t => t is TimeOrderTier);
			if (toTier == null)
				return null;

			return from seg in toTier.GetAllSegments().Cast<ITimeOrderSegment>()
				select new SegmentBoundaries(TimeSpan.FromSeconds(seg.Start), TimeSpan.FromSeconds(seg.Stop));
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<TimeSpan> GetSegmentBoundaries()
		{
			return _segments.Select(s => s.end);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetSegments()
		{
			return GetSegmentBoundaries().Select(b => b.TotalSeconds.ToString(CultureInfo.InvariantCulture));
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetEndOfLastSegment()
		{
			return (_segments.Count == 0 ? TimeSpan.Zero : _segments[_segments.Count - 1].end);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the time between the proposed end time and the closest
		/// boundary to it's left will make a long enough segment.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual bool GetIsSegmentLongEnough(TimeSpan proposedEndTime)
		{
			for (int i = _segments.Count - 1; i >= 0; i--)
			{
				if (_segments[i].end < proposedEndTime)
					return (proposedEndTime.TotalMilliseconds - _segments[i].end.TotalMilliseconds >= Settings.Default.MinimumAnnotationSegmentLengthInMilliseconds);
			}

			return (proposedEndTime.TotalMilliseconds >= Settings.Default.MinimumAnnotationSegmentLengthInMilliseconds);
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool MoveExistingSegmentBoundary(TimeSpan boundaryToAdjust, int millisecondsToMove)
		{
			int i = GetSegmentBoundaries().ToList().IndexOf(boundaryToAdjust);
			if (i < 0)
				return false;

			var newBoundary = boundaryToAdjust + TimeSpan.FromMilliseconds(millisecondsToMove);
			var minSegLength = TimeSpan.FromMilliseconds(Settings.Default.MinimumAnnotationSegmentLengthInMilliseconds);

			// Check if moving the existing boundary left will make the segment too short.
			if (newBoundary <= _segments[i].start || (i > 0 && newBoundary - _segments[i].start < minSegLength))
				return false;

			if (i == _segments.Count - 1)
			{
				// Check if the moved boundary will go beyond the end of the audio's length.
				if (newBoundary > OrigWaveStream.TotalTime - minSegLength)
					return false;
			}
			else if	(_segments[i + 1].end - newBoundary < minSegLength)
			{
				// The moved boundary will make the next segment too short.
				return false;
			}

			ChangeSegmentsEndBoundary(i, newBoundary);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public int GetSegmentCount()
		{
			return _segments.Count;
		}

		/// ------------------------------------------------------------------------------------
		public void SegmentBoundaryMoved(TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			if (oldEndTime != newEndTime)
				ChangeSegmentsEndBoundary(GetSegmentBoundaries().ToList().IndexOf(oldEndTime), newEndTime);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void ChangeSegmentsEndBoundary(int index, TimeSpan newBoundary)
		{
			if (index < 0 || index >= _segments.Count)
				return;

			if (index < _segments.Count - 1)
			{
				RenameAnnotationForResizedSegment(_segments[index + 1],
					new SegmentBoundaries(newBoundary, _segments[index + 1].end));
				_segments[index + 1].start = newBoundary;
			}

			RenameAnnotationForResizedSegment(_segments[index],
				new SegmentBoundaries(_segments[index].start, newBoundary));

			_segments[index].end = newBoundary;
			SegmentBoundariesChanged = true;
		}


		/// ------------------------------------------------------------------------------------
		public virtual void DeleteBoundary(TimeSpan boundary)
		{
			var i = _segments.Select(s => s.end).ToList().IndexOf(boundary);
			if (i < 0)
				return;

			_segments.RemoveAt(i);

			if (_segments.Count == 0 || i == _segments.Count)
				return;

			if (i == 0)
				_segments[0].start = TimeSpan.Zero;
			else
				_segments[i].start = _segments[i - 1].end;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void RenameAnnotationForResizedSegment(SegmentBoundaries oldSegment,
			SegmentBoundaries newSegment)
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
