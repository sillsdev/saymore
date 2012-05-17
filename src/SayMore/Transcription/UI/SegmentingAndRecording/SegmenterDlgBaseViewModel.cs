using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NAudio.Wave;
using Palaso.Reporting;
using SayMore.Media.Audio;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.UI.NewEventsFromFiles;
using SayMore.Utilities;

namespace SayMore.Transcription.UI
{
	public class SegmenterDlgBaseViewModel : IDisposable
	{
		/// ------------------------------------------------------------------------------------
		protected enum SegmentChangeType
		{
			Addition,
			Deletion,
			EndBoundaryMoved,
			AnnotationAdded,
		}

		#region SegmentChange class
		/// ------------------------------------------------------------------------------------
		protected class SegmentChange
		{
			private readonly SegmentChangeType _type;
			private readonly TimeRange _originalRange;

			public SegmentChangeType Type { get { return _type; } }
			public TimeRange OriginalRange { get { return _originalRange; } }
			public Action<SegmentChange> UndoAction { get; private set; }
			public TimeRange NewRange { get; private set; }

			/// ------------------------------------------------------------------------------------
			public SegmentChange(TimeRange newRange, Action<SegmentChange> undoAction) :
				this(SegmentChangeType.Addition, null, newRange, undoAction)
			{
			}

			/// ------------------------------------------------------------------------------------
			public SegmentChange(SegmentChangeType type, TimeRange originalRange,
				TimeRange newRange, Action<SegmentChange> undoAction)
			{
				_type = type;
				_originalRange = originalRange;
				NewRange = newRange;
				UndoAction = undoAction;
			}

			/// ------------------------------------------------------------------------------------
			public bool TryUpdate(SegmentChange newChange)
			{
				if (NewRange == newChange.OriginalRange && newChange.Type == SegmentChangeType.EndBoundaryMoved)
				{
					NewRange = newChange.NewRange;
					return true;
				}
				if (newChange.Type == SegmentChangeType.Addition && Type == SegmentChangeType.AnnotationAdded &&
					newChange.NewRange == OriginalRange)
				{
					UndoAction = c => { newChange.UndoAction(c); UndoAction(c); };
					return true;
				}

				return false;
			}

			/// ------------------------------------------------------------------------------------
			public void Undo()
			{
				UndoAction(this);
			}

			/// ------------------------------------------------------------------------------------
			public override string ToString()
			{
				switch (Type)
				{
					case SegmentChangeType.Deletion: return "Deletion of segment " + OriginalRange;
					case SegmentChangeType.Addition: return "Addition of segment " + NewRange;
					case SegmentChangeType.EndBoundaryMoved: return "Segment boundary change from " + OriginalRange + " to " + NewRange;
					case SegmentChangeType.AnnotationAdded: return "Annotation addition " + OriginalRange;
					default: return "Unknown action";
				}
			}
		}

		#endregion

		#region UndoStack class
		/// ------------------------------------------------------------------------------------
		protected class UndoStack
		{
			private readonly Stack<SegmentChange> _undoStack = new Stack<SegmentChange>();
			private bool _inUndo;

			/// ------------------------------------------------------------------------------------
			public bool SegmentBoundariesChanged
			{
				get { return _undoStack.Any(c => c.Type != SegmentChangeType.AnnotationAdded); }
			}

			/// ------------------------------------------------------------------------------------
			public bool IsEmpty
			{
				get { return _undoStack.Count == 0; }
			}

			/// ------------------------------------------------------------------------------------
			public TimeRange TimeRangeForUndo
			{
				get { return (_undoStack.Count == 0) ? null : _undoStack.Peek().NewRange; }
			}

			/// ------------------------------------------------------------------------------------
			public void Push(SegmentChange segmentChange)
			{
				if (!_inUndo)
				{
					if (_undoStack.Count == 0 || !_undoStack.Peek().TryUpdate(segmentChange))
						_undoStack.Push(segmentChange);
				}
			}

			/// ------------------------------------------------------------------------------------
			public void Undo()
			{
				if (_undoStack.Count == 0)
					throw new InvalidOperationException("Undo stack is empty!");

				if (_undoStack.Peek().UndoAction == null)
					throw new NotImplementedException(_undoStack.Peek() + " cannot be undone!");

				_inUndo = true;
				_undoStack.Pop().Undo();
				_inUndo = false;
			}
		}

		#endregion

		public ComponentFile ComponentFile { get; protected set; }
		public WaveStream OrigWaveStream { get; protected set; }
		public bool HaveSegmentBoundaries { get; set; }
		public Action UpdateDisplayProvider { get; set; }
		public TierCollection Tiers { get; protected set; }
		public TimeTier TimeTier { get; protected set; }
		public HashSet<AudioFileHelper> SegmentsAnnotationSamplesToDraw { get; private set; }
		public Action OralAnnotationWaveAreaRefreshAction { get; set; }

		public string TempOralAnnotationsFolder { get; protected set; }
		public string OralAnnotationsFolder { get; protected set; }

		protected List<string> _oralAnnotationFilesBeforeChanges = new List<string>();
		protected readonly UndoStack _undoStack = new UndoStack();

		#region Construction and disposal
		/// ------------------------------------------------------------------------------------
		public SegmenterDlgBaseViewModel(ComponentFile file)
		{
			SegmentsAnnotationSamplesToDraw = new HashSet<AudioFileHelper>();
			ComponentFile = file;
			OrigWaveStream = new WaveFileReader(ComponentFile.PathToAnnotatedFile);

			Tiers = file.GetAnnotationFile() != null ?
				file.GetAnnotationFile().Tiers.Copy() : new TierCollection(ComponentFile.PathToAnnotatedFile);

			TimeTier = Tiers.GetTimeTier();

			if (TimeTier == null)
			{
				TimeTier = new TimeTier(ComponentFile.PathToAnnotatedFile);
				Tiers.Insert(0, TimeTier);
			}

			OralAnnotationsFolder = ComponentFile.PathToAnnotatedFile +
				Settings.Default.OralAnnotationsFolderSuffix;

			TempOralAnnotationsFolder = Path.Combine(Path.GetTempPath(), "SayMoreOralAnnotations");
			_oralAnnotationFilesBeforeChanges = GetListOfOralAnnotationSegmentFilesBeforeChanges().ToList();
			TimeTier.BackupOralAnnotationSegmentFileAction = BackupOralAnnotationSegmentFile;
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Dispose()
		{
			TimeTier.BackupOralAnnotationSegmentFileAction = null;

			if (OrigWaveStream != null)
			{
				OrigWaveStream.Close();
				OrigWaveStream.Dispose();
			}

			try
			{
				Directory.Delete(TempOralAnnotationsFolder, true);
			}
			catch { }
		}

		#endregion

		#region Methods for backing up and restoring oral annotation recorded segment files when they're renamed or deleted.
		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetListOfOralAnnotationSegmentFilesBeforeChanges()
		{
			if (!Directory.Exists(OralAnnotationsFolder))
				return new string[0];

			return Directory.GetFiles(OralAnnotationsFolder, "*.wav").Where(file =>
				file.EndsWith(Settings.Default.OralAnnotationCarefulSegmentFileSuffix) ||
				file.EndsWith(Settings.Default.OralAnnotationTranslationSegmentFileSuffix));
		}

		/// ------------------------------------------------------------------------------------
		public void BackupOralAnnotationSegmentFile(string srcfile)
		{
			var dstFile = Path.Combine(TempOralAnnotationsFolder, Path.GetFileName(srcfile));

			// If the file has already been backed up, then there's no need to do it again.
			if (File.Exists(dstFile))
				return;

			// If the source file is not one of the original oral annotation
			// segment files, then there's no need to back it up
			if (_oralAnnotationFilesBeforeChanges.All(f => Path.GetFileName(f) != Path.GetFileName(srcfile)))
				return;

			if (!Directory.Exists(TempOralAnnotationsFolder))
				FileSystemUtils.CreateDirectory(TempOralAnnotationsFolder);

			CopyFilesViewModel.Copy(srcfile, dstFile);
		}

		/// ------------------------------------------------------------------------------------
		public void DiscardChanges()
		{
			DiscardRecordedAnnotations();
			RestoreOriginalRecordedAnnotations();
		}

		/// ------------------------------------------------------------------------------------
		public void DiscardRecordedAnnotations()
		{
			if (!Directory.Exists(OralAnnotationsFolder))
				return;

			foreach (var file in Directory.GetFiles(OralAnnotationsFolder, "*.wav")
				.Where(f => !_oralAnnotationFilesBeforeChanges.Contains(f)))
			{
				try
				{
					File.Delete(file);
				}
				catch (Exception e)
				{
					ErrorReport.NotifyUserOfProblem(e,
						"Error trying to discard the oral annotation recording file '{0}'", file);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public void RestoreOriginalRecordedAnnotations()
		{
			if (_oralAnnotationFilesBeforeChanges.Count == 0)
				return;

			var filesToRestore = (from f in _oralAnnotationFilesBeforeChanges
								  let srcfile = Path.Combine(TempOralAnnotationsFolder, Path.GetFileName(f))
								  where File.Exists(srcfile)
								  select new KeyValuePair<string, string>(srcfile, f)).ToArray();

			if (filesToRestore.Length == 0)
				return;

			var model = new CopyFilesViewModel(filesToRestore, true);
			model.Start();
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		public bool SegmentBoundariesChanged
		{
			get { return _undoStack.SegmentBoundariesChanged; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string ProgramAreaForUsageReporting
		{
			get { return "ManualSegmentation"; }
		}

		/// ------------------------------------------------------------------------------------
		public bool WereChangesMade
		{
			get { return !_undoStack.IsEmpty; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected virtual WaveStream GetStreamFromAudio(string audioFilePath)
		{
			Exception error;

			return (AudioUtils.GetOneChannelStreamFromAudio(audioFilePath, out error) ??
				new WaveFileReader(audioFilePath));
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<TimeSpan> GetSegmentEndBoundaries()
		{
			return TimeTier.Segments.Select(s => s.TimeRange.End);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetEndOfLastSegment()
		{
			return (TimeTier.Segments.Count == 0 ? TimeSpan.Zero :
				TimeTier.Segments[TimeTier.Segments.Count - 1].TimeRange.End);
		}

		/// ------------------------------------------------------------------------------------
		public TimeRange TimeRangeForUndo
		{
			get { return _undoStack.TimeRangeForUndo; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the time between the proposed end time and the closest
		/// boundary to it's left will make a long enough segment.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GetIsSegmentLongEnough(TimeSpan proposedEndTime)
		{
			var minSize = TimeSpan.FromMilliseconds(Settings.Default.MinimumSegmentLengthInMilliseconds);

			for (int i = TimeTier.Segments.Count - 1; i >= 0; i--)
			{
				if (TimeTier.Segments[i].TimeRange.End < proposedEndTime)
					return (proposedEndTime - TimeTier.Segments[i].TimeRange.End >= minSize);
			}

			return (proposedEndTime >= minSize);
		}

		/// ------------------------------------------------------------------------------------
		public int GetSegmentCount()
		{
			return TimeTier.Segments.Count;
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool SegmentBoundaryMoved(TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			if (oldEndTime == newEndTime)
				return false;

			if (!UpdateSegmentBoundary(oldEndTime, newEndTime))
				return false;
			OnBoundaryWasDeletedInsertedOrMoved();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool UpdateSegmentBoundary(TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			var seg = TimeTier.Segments.First(s => s.TimeRange.End == oldEndTime);
			// TODO: Figure out why dragging new segment end boundary to the left can cause seg to be null (resulting in a crash).
			var origTimeRange = seg.TimeRange.Copy();

			var result = TimeTier.ChangeSegmentsEndBoundary(
				(float)oldEndTime.TotalSeconds, (float)newEndTime.TotalSeconds);

			if (result != BoundaryModificationResult.Success)
				return false;

			_undoStack.Push(new SegmentChange(SegmentChangeType.EndBoundaryMoved, origTimeRange, seg.TimeRange.Copy(),
				c => SegmentBoundaryMoved(c.NewRange.End, c.OriginalRange.End)));

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<TimeSpan> InsertNewBoundary(TimeSpan newBoundary)
		{
			if (Tiers.InsertTierSegment((float)newBoundary.TotalSeconds) == BoundaryModificationResult.Success)
			{
				_undoStack.Push(new SegmentChange(TimeTier.Segments.First(s => s.TimeRange.End == newBoundary).TimeRange.Copy(),
					c => DeleteBoundary(c.NewRange.End)));
				OnBoundaryWasDeletedInsertedOrMoved();
			}

			return GetSegmentEndBoundaries();
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool DeleteBoundary(TimeSpan boundary)
		{
			var seg = TimeTier.GetSegmentHavingEndBoundary((float)boundary.TotalSeconds);

			if (!Tiers.RemoveTierSegments(TimeTier.GetIndexOfSegment(seg)))
				return false;

			_undoStack.Push(new SegmentChange(SegmentChangeType.Deletion, seg.TimeRange.Copy(), null, null));
			OnBoundaryWasDeletedInsertedOrMoved();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public void Undo()
		{
			_undoStack.Undo();
		}

		/// ------------------------------------------------------------------------------------
		protected void OnBoundaryWasDeletedInsertedOrMoved()
		{
			SegmentsAnnotationSamplesToDraw.Clear();

			if (OralAnnotationWaveAreaRefreshAction != null)
				OralAnnotationWaveAreaRefreshAction();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a value indicating whether the given boundary cannot be deleted or moved in
		/// either direction. A boundary is "permanent" if the time tier has readonly ranges
		/// or if it is adjacent to a segment that already has any kind of annotation (text or oral)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsBoundaryPermanent(TimeSpan boundary)
		{
			return (TimeTier.ReadOnlyTimeRanges || Tiers.HasAdjacentAnnotation((float)boundary.TotalSeconds));
		}

		/// ------------------------------------------------------------------------------------
		public bool CanMoveBoundary(TimeSpan boundaryToAdjust, int millisecondsToMove)
		{
			var secondsToMove = Math.Abs(millisecondsToMove) / 1000f;
			var boundary = (float)boundaryToAdjust.TotalSeconds;

			return (millisecondsToMove < 0 ?
				TimeTier.CanBoundaryMoveLeft(boundary, secondsToMove) :
				TimeTier.CanBoundaryMoveRight(boundary, secondsToMove, (float)OrigWaveStream.TotalTime.TotalSeconds));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the time tier contains more segments than all the other text tiers, this method
		/// will add a number of segments to each text tier so each tier contains the same
		/// number of segments. Added text segments are filled with an empty string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CreateMissingTextSegmentsToMatchTimeSegmentCount()
		{
			foreach (var textTier in Tiers.OfType<TextTier>()
				.Where(t => t.Segments.Count < TimeTier.Segments.Count))
			{
				while (textTier.Segments.Count < TimeTier.Segments.Count)
					textTier.AddSegment(string.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected void InvokeUpdateDisplayAction()
		{
			if (UpdateDisplayProvider != null)
				UpdateDisplayProvider();
		}
	}
}
