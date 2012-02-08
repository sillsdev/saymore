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
		public class SegmentBoundaries
		{
			public TimeSpan start;
			public TimeSpan end;
			public SegmentBoundaries(TimeSpan s, TimeSpan e) { start = s; end = e; }
			public override string ToString() { return start + " - " + end; }
		}

		public event EventHandler BoundariesUpdated;

		public ComponentFile ComponentFile { get; protected set; }
		public WaveStream OrigWaveStream { get; protected set; }
		public bool HaveSegmentBoundaries { get; set; }
		public Action UpdateDisplayProvider { get; set; }
		public TierCollection Tiers { get; protected set; }

		protected List<SegmentBoundaries> _segments;

		/// ------------------------------------------------------------------------------------
		public SegmenterDlgBaseViewModel(ComponentFile file)
		{
			ComponentFile = file;
			OrigWaveStream = new WaveFileReader(ComponentFile.PathToAnnotatedFile); // GetStreamFromAudio(ComponentFile.PathToAnnotatedFile);

			Tiers = file.GetAnnotationFile() != null ?
				file.GetAnnotationFile().Tiers.Copy() : new TierCollection();

			_segments = InitializeSegments(Tiers).ToList();
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
		public IEnumerable<SegmentBoundaries> InitializeSegments(TierCollection tiers)
		{
			var toTier = tiers.GetTimeTier();
			if (toTier == null)
				return new List<SegmentBoundaries>();

			return toTier.Segments.Select(s =>
				new SegmentBoundaries(TimeSpan.FromSeconds(s.Start), TimeSpan.FromSeconds(s.End)));
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
			//var minSegLength = TimeSpan.FromMilliseconds(Settings.Default.MinimumAnnotationSegmentLengthInMilliseconds);
			var minSegLength = TimeSpan.Zero;

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
		public TimeSpan GetPreviousBoundary(TimeSpan boundary)
		{
			var i = _segments.Select(s => s.end).ToList().IndexOf(boundary);
			return (i < 0 ? TimeSpan.Zero : _segments[i].start);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetNextBoundary(TimeSpan boundary)
		{
			var i = _segments.Select(s => s.start).ToList().IndexOf(boundary);
			return (i < 0 ? TimeSpan.Zero : _segments[i].end);
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

			var	timeTier = Tiers.GetTimeTier();

			if (index < _segments.Count - 1)
			{
				RenameAnnotationForResizedSegment(_segments[index + 1],
					new SegmentBoundaries(newBoundary, _segments[index + 1].end));

				timeTier.Segments.ElementAt(index + 1).Start = (float)newBoundary.TotalSeconds;
			}

			RenameAnnotationForResizedSegment(_segments[index],
				new SegmentBoundaries(_segments[index].start, newBoundary));

			timeTier.Segments.ElementAt(index).End = (float)newBoundary.TotalSeconds;

			_segments = InitializeSegments(Tiers).ToList();
			SegmentBoundariesChanged = true;

			if (BoundariesUpdated != null)
				BoundariesUpdated(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void DeleteBoundary(TimeSpan boundary)
		{
			var i = _segments.Select(s => s.end).ToList().IndexOf(boundary);
			if (i < 0)
				return;

			foreach (var tier in Tiers)
				tier.RemoveSegment(i);

			_segments = InitializeSegments(Tiers).ToList();

			if (BoundariesUpdated != null)
				BoundariesUpdated(this, EventArgs.Empty);
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
