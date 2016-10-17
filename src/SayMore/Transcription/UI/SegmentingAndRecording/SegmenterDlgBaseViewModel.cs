using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using L10NSharp;
using NAudio.Wave;
using SIL.Reporting;
using SayMore.Media.Audio;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.UI.NewSessionsFromFiles;
using SayMore.Utilities;

// ReSharper disable once CheckNamespace
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
			AnnotationDeleted,
			Ignored,
			Unignored,
		}

		#region SegmentChange class
		/// ------------------------------------------------------------------------------------
		protected class SegmentChange
		{
			private SegmentChangeType _type;
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
					_type = SegmentChangeType.Addition;
					Action<SegmentChange> originalUndoAction = UndoAction;
					UndoAction = c => { originalUndoAction(c); newChange.UndoAction(c); };
					return true;
				}
				if (newChange.Type == SegmentChangeType.AnnotationDeleted && Type == SegmentChangeType.AnnotationDeleted &&
					newChange.NewRange == OriginalRange)
				{
					Action<SegmentChange> originalUndoAction = UndoAction;
					UndoAction = c => { originalUndoAction(c); newChange.UndoAction(c); };
					return true;
				}
				if (newChange.Type == SegmentChangeType.Addition && Type == SegmentChangeType.AnnotationDeleted &&
					newChange.NewRange.Start == OriginalRange.Start)
				{
					Action<SegmentChange> originalUndoAction = UndoAction;
					UndoAction = c => { originalUndoAction(c); newChange.UndoAction(c); };
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
				string fmt;
				switch (Type)
				{
					case SegmentChangeType.Deletion:
						fmt = LocalizationManager.GetString(
							"DialogBoxes.Transcription.SegmenterDlgBase.UndoAction.SegmentDeletion",
							"Deletion of segment {0}",
							"Parameter is time range of deleted segment.");
						return String.Format(fmt, OriginalRange);
					case SegmentChangeType.Addition:
						fmt = LocalizationManager.GetString(
							"DialogBoxes.Transcription.SegmenterDlgBase.UndoAction.SegmentAddition",
							"Addition of segment {0}",
							"Parameter is time range of added segment.");
						return String.Format(fmt, OriginalRange);
					case SegmentChangeType.EndBoundaryMoved:
						fmt = LocalizationManager.GetString(
							"DialogBoxes.Transcription.SegmenterDlgBase.UndoAction.SegmentBoundaryMove",
							"Segment boundary change from {0} to {1}",
							"Parameter 0 is the original time range of the segment. Parameter 1 is the new time range.");
						return String.Format(fmt, OriginalRange, NewRange);
					case SegmentChangeType.AnnotationAdded:
						fmt = LocalizationManager.GetString(
							"DialogBoxes.Transcription.SegmenterDlgBase.UndoAction.AnnotationRecording",
							"Recording annotation for segment {0}",
							"Parameter is time range of the segment for which the annotation was recorded.");
						return String.Format(fmt, OriginalRange);
					case SegmentChangeType.AnnotationDeleted:
						fmt = LocalizationManager.GetString(
							"DialogBoxes.Transcription.SegmenterDlgBase.UndoAction.AnnotationDeleted",
							"Deletion of annotation for segment {0}",
							"Parameter is time range of the segment for which the annotation was deleted (This undo action is included for completeness, but currently never gets displayed in UI).");
						return String.Format(fmt, OriginalRange);
					case SegmentChangeType.Ignored:
						fmt = LocalizationManager.GetString(
							"DialogBoxes.Transcription.SegmenterDlgBase.UndoAction.SegmentIgnored",
							"Ignoring segment {0}",
							"Parameter is time range of the segment that was ignored.");
						return String.Format(fmt, OriginalRange);
					case SegmentChangeType.Unignored:
						fmt = LocalizationManager.GetString(
							"DialogBoxes.Transcription.SegmenterDlgBase.UndoAction.SegmentUnignored",
							"Marking segment {0} as relevant",
							"Parameter is time range of the segment that was ignored.");
						return String.Format(fmt, OriginalRange);
					default:
						return "Unknown action";
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
				get { return _undoStack.Any(c => c.Type != SegmentChangeType.AnnotationAdded &&
					c.Type != SegmentChangeType.AnnotationDeleted); }
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
			public string DescriptionForUndo
			{
				get { return (_undoStack.Count == 0) ? null : _undoStack.Peek().ToString(); }
			}

			/// ------------------------------------------------------------------------------------
			public SegmentChangeType ChangeTypeToUndo
			{
				get { return _undoStack.Peek().Type; }
			}

			/// ------------------------------------------------------------------------------------
			public int SequenceId
			{
				get { return _undoStack.Count; }
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

		private const string kBackupVersionPrefix = ".b";

		public ComponentFile ComponentFile { get; protected set; }
		public WaveStream OrigWaveStream { get; protected set; }
		public bool HaveSegmentBoundaries { get; set; }
		public Action UpdateDisplayProvider { get; set; }
		public Func<bool, bool, bool> AllowDeletionOfOralAnnotations { get; set; }
		public TierCollection Tiers { get; protected set; }
		public TimeTier TimeTier { get; protected set; }
		public Action OralAnnotationWaveAreaRefreshAction { get; set; }

		public string TempOralAnnotationsFolder { get; protected set; }
		public string OralAnnotationsFolder { get; protected set; }

		protected readonly HashSet<AudioFileHelper> _segmentsAnnotationSamplesToDraw = new HashSet<AudioFileHelper>();
		protected List<string> _oralAnnotationFilesBeforeChanges = new List<string>();
		protected readonly UndoStack _undoStack = new UndoStack();
		private List<string> _oralAnnotationFilesToDelete;

		#region Construction and disposal
		/// ------------------------------------------------------------------------------------
		public SegmenterDlgBaseViewModel(ComponentFile file)
		{
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
			if (Directory.Exists(TempOralAnnotationsFolder))
			{
				foreach (var tempFile in Directory.EnumerateFiles(TempOralAnnotationsFolder))
					File.Delete(tempFile);
			}
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
// ReSharper disable once EmptyGeneralCatchClause
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
		protected virtual void EraseAnnotation(string path)
		{
			FileSystemUtils.WaitForFileRelease(path);

			lock (_segmentsAnnotationSamplesToDraw)
			{
				if (File.Exists(path))
				{
					File.Delete(path);
					_segmentsAnnotationSamplesToDraw.RemoveWhere(h => h.AudioFilePath == path);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public void BackupOralAnnotationSegmentFile(string srcfile, bool deleteAfterBackingUp)
		{
			var fileName = Path.GetFileName(srcfile);
			if (fileName == null)
				return;
			var dstFile = Path.Combine(TempOralAnnotationsFolder, fileName);

			// If the file has already been backed up, then make a backup with the next
			// available sequence number.
			if (File.Exists(dstFile))
			{
				int latestBackup = GetLatestBackupNumberForFile(dstFile);
				dstFile += kBackupVersionPrefix + (latestBackup + 1);
			}

			if (!Directory.Exists(TempOralAnnotationsFolder))
				FileSystemUtils.CreateDirectory(TempOralAnnotationsFolder);

			CopyFilesViewModel.Copy(srcfile, dstFile);

			if (deleteAfterBackingUp)
				EraseAnnotation(srcfile);
		}

		/// ------------------------------------------------------------------------------------
		protected void RestorePreviousVersionOfAnnotation(string dstFile)
		{
			var fileName = Path.GetFileName(dstFile);
			if (fileName == null) return;

			var backupFile = Path.Combine(TempOralAnnotationsFolder, fileName);
			if (File.Exists(backupFile))
			{
				var versionNumber = GetLatestBackupNumberForFile(dstFile);
				if (versionNumber > 0)
					backupFile += kBackupVersionPrefix + versionNumber;
				if (File.Exists(dstFile))
				{
					// SP-714: Access denied trying to delete file
					FileSystemUtils.WaitForFileRelease(dstFile);
					File.Delete(dstFile);
				}

				try
				{
					File.Move(backupFile, dstFile);
				}
				catch (IOException e)
				{
					// SP-988: We think the lock around _segmentsAnnotationSamplesToDraw (in
					// the call to this method) may now effectively prevent this error, but just
					// in case, we're reporting it as non-fatal. If the user ignores this error,
					// the only "bad" thing that happens is we fail to restore a backup. But since
					// this can only happen if something (presumably some other thread in SM)
					// creates a new version of the file right after we check for its existence
					// and/or delete it, something else is probably happening or about to happen
					// that will overwrite the file anyway.
					ErrorReport.ReportNonFatalException(e);
				}
			}
			else
				EraseAnnotation(dstFile);
		}

		/// ------------------------------------------------------------------------------------
		private int GetLatestBackupNumberForFile(string dstFile)
		{
			int max = 0;
			foreach (var bakFile in Directory.EnumerateFiles(TempOralAnnotationsFolder, Path.GetFileName(dstFile) + kBackupVersionPrefix + "*"))
			{
				var ich = bakFile.LastIndexOf(kBackupVersionPrefix, bakFile.Length, StringComparison.Ordinal) + 1;
				int backupNum;
				if (ich > 0 && ich < bakFile.Length && Int32.TryParse(bakFile.Substring(ich + 1), out backupNum))
					max = Math.Max(max, backupNum);
			}
			return max;
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

			// ReSharper disable once AssignNullToNotNullAttribute - Path.GetFileName(f)
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
			get { return "Manual Segmentation"; }
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

		/// ----------------------------------------------------------------------------------------
		public AnnotationSegment GetSegment(int index)
		{
			return (index < 0 || index >= TimeTier.Segments.Count ?
				null : TimeTier.Segments[index]);
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<TimeSpan> GetSegmentEndBoundaries()
		{
			return TimeTier.Segments.Select(s => s.TimeRange.End);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetEndOfLastSegment()
		{
			return TimeTier.EndOfLastSegment;
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetEndOfSegment(int index)
		{
			return TimeTier.Segments[index].TimeRange.End;
		}

		/// ------------------------------------------------------------------------------------
		public virtual TimeSpan VirtualBoundaryBeyondLastSegment
		{
			get { return OrigWaveStream.TotalTime; }
		}

		/// ------------------------------------------------------------------------------------
		public TimeRange TimeRangeForUndo
		{
			get { return _undoStack.TimeRangeForUndo; }
		}

		/// ------------------------------------------------------------------------------------
		public string DescriptionForUndo
		{
			get { return _undoStack.DescriptionForUndo; }
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
				if (TimeTier.Segments[i].TimeRange.End <= proposedEndTime)
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
			OnSegmentBoundaryChanged();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool UpdateSegmentBoundary(TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			var seg = TimeTier.Segments.FirstOrDefault(s => s.TimeRange.End == oldEndTime);
			return seg != null && UpdateSegmentBoundary(seg, newEndTime);
		}

		/// ------------------------------------------------------------------------------------
		private bool UpdateSegmentBoundary(AnnotationSegment seg, TimeSpan newEndTime)
		{
			var origTimeRange = seg.TimeRange.Copy();

			if (TimeTier.ChangeSegmentsEndBoundary(seg.TimeRange.EndSeconds, (float)newEndTime.TotalSeconds) !=
				BoundaryModificationResult.Success)
			{
				return false;
			}

			_undoStack.Push(new SegmentChange(SegmentChangeType.EndBoundaryMoved, origTimeRange, seg.TimeRange.Copy(),
				c => SegmentBoundaryMoved(c.NewRange.End, c.OriginalRange.End)));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<TimeSpan> InsertNewBoundary(TimeSpan newBoundary)
		{
			if (Tiers.InsertTierSegment((float)newBoundary.TotalSeconds, ConfirmDeletionOfOralAnnotations) == BoundaryModificationResult.Success)
			{
				Action<SegmentChange> undoAction = RevertNewSegment;
				if (_oralAnnotationFilesToDelete != null)
				{
					foreach (var file in _oralAnnotationFilesToDelete)
					{
						BackupOralAnnotationSegmentFile(file, true);
						Action<SegmentChange> undoActionOrig = undoAction;
						var file1 = file;
						undoAction = c => { undoActionOrig(c); RestorePreviousVersionOfAnnotation(file1); };
					}
					_oralAnnotationFilesToDelete = null;
				}
				_undoStack.Push(new SegmentChange(TimeTier.Segments.First(s => s.TimeRange.End == newBoundary).TimeRange.Copy(),
					undoAction));
				OnSegmentBoundaryChanged();
			}

			return GetSegmentEndBoundaries();
		}

		/// ------------------------------------------------------------------------------------
		protected bool ConfirmDeletionOfOralAnnotations(AnnotationSegment segment, bool hasCarefulSpeech, bool hasOralTranslation)
		{
			if (AllowDeletionOfOralAnnotations == null || !AllowDeletionOfOralAnnotations(hasCarefulSpeech, hasOralTranslation))
				return false;

			_oralAnnotationFilesToDelete = new List<string>(2);

			var carefulSpeechAnnotationPath = segment.GetFullPathToCarefulSpeechFile();
			if (carefulSpeechAnnotationPath != null && File.Exists(carefulSpeechAnnotationPath))
				_oralAnnotationFilesToDelete.Add(carefulSpeechAnnotationPath);

			var oralTranslationAnnotationPath = segment.GetFullPathToOralTranslationFile();
			if (oralTranslationAnnotationPath != null && File.Exists(oralTranslationAnnotationPath))
				_oralAnnotationFilesToDelete.Add(oralTranslationAnnotationPath);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public void SetIgnoredFlagForSegment(AnnotationSegment segment, bool ignore)
		{
			if (segment != null)
			{
				var segmentIndex = TimeTier.GetIndexOfSegment(segment);
				var timeRange = segment.TimeRange.Copy();
				if (ignore)
				{
					Action restoreState = GetActionToRestoreStateWhenUndoingAnIgnore(segment);

					Tiers.MarkSegmentAsIgnored(segmentIndex);
					_undoStack.Push(new SegmentChange(SegmentChangeType.Ignored, timeRange, timeRange, sc =>
					{
						Tiers.MarkSegmentAsUnignored(segmentIndex);
						restoreState();
					}));
				}
				else
				{
					Tiers.MarkSegmentAsUnignored(segmentIndex);
					_undoStack.Push(new SegmentChange(SegmentChangeType.Unignored, timeRange, timeRange,
						sc => Tiers.MarkSegmentAsIgnored(segmentIndex)));
				}
			}
			else
			{
				if (!ignore)
					throw new InvalidOperationException("New segment can never be unignored.");
				AddIgnoredSegment(VirtualBoundaryBeyondLastSegment);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual Action GetActionToRestoreStateWhenUndoingAnIgnore(AnnotationSegment segment)
		{
			return () => { };
		}

		/// ------------------------------------------------------------------------------------
		public void AddIgnoredSegment(TimeSpan newBoundary)
		{
			Tiers.AddIgnoredSegment((float)newBoundary.TotalSeconds);
			_undoStack.Push(new SegmentChange(TimeTier.Segments.First(s => s.TimeRange.End == newBoundary).TimeRange.Copy(),
				c => { Tiers.GetTranscriptionTier().Segments.Last().Text = string.Empty; RevertNewSegment(c); }));
			OnSegmentBoundaryChanged();
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsSegmentIgnored(AnnotationSegment segment)
		{
			return GetIsSegmentIgnored(TimeTier.GetIndexOfSegment(segment));
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsSegmentIgnored(int segmentIndex)
		{
			if (Tiers.GetTranscriptionTier() == null || segmentIndex < 0 || segmentIndex >= Tiers.GetTranscriptionTier().Segments.Count)
				return false;
			return Tiers.GetIsSegmentIgnored(segmentIndex);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<TimeRange> GetIgnoredSegmentRanges()
		{
			for (int i = 0; i < GetSegmentCount(); i++)
			{
				if (GetIsSegmentIgnored(i))
					yield return TimeTier.Segments[i].TimeRange;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void RevertNewSegment(SegmentChange change)
		{
			DeleteBoundary(change.NewRange.End);
		}

		/// ------------------------------------------------------------------------------------
		public bool DeleteBoundary(TimeSpan boundary)
		{
			var seg = TimeTier.GetSegmentHavingEndBoundary((float)boundary.TotalSeconds);

			if (!Tiers.RemoveTierSegments(TimeTier.GetIndexOfSegment(seg)))
				return false;

			OnSegmentDeleted(seg);
			OnSegmentBoundaryChanged();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public void Undo()
		{
			_undoStack.Undo();
		}

		/// ------------------------------------------------------------------------------------
		public bool NextUndoItemIsAddition
		{
			get { return !_undoStack.IsEmpty && _undoStack.ChangeTypeToUndo == SegmentChangeType.Addition; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnSegmentDeleted(AnnotationSegment segment)
		{
			_undoStack.Push(new SegmentChange(SegmentChangeType.Deletion, segment.TimeRange.Copy(), null, null));
		}

		/// ------------------------------------------------------------------------------------
		private void OnSegmentBoundaryChanged()
		{
			lock(_segmentsAnnotationSamplesToDraw)
				_segmentsAnnotationSamplesToDraw.Clear();

			if (OralAnnotationWaveAreaRefreshAction != null)
				OralAnnotationWaveAreaRefreshAction();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a value indicating whether the given boundary cannot be deleted or moved in
		/// either direction. A boundary is absolutely "permanent" if the time tier has readonly
		/// ranges. Typically it will also be considered permanent if it is adjacent to a
		/// segment that already has an oral annotation (text annotations can also prevent moves
		/// if PreventSegmentBoundaryMovingWhereTextAnnotationsAreAdjacent is set); however, the
		/// caller can elect to disregard annotations when determining this.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsBoundaryPermanent(TimeSpan boundary, bool disregardAnnotations = false)
		{
			return (TimeTier.ReadOnlyTimeRanges || (!disregardAnnotations &&
				Tiers.HasAdjacentAnnotation((float)boundary.TotalSeconds)));
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If not fully segmented and final segment is within 5 seconds of the end of the
		/// source recording, either extend existing final segment to end of wavestream or add
		/// a final ignored segment.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddFinalSegmentIfAlmostComplete()
		{
			var lastSegment = TimeTier.Segments.GetLast();
			if (lastSegment == null)
				return;
			var endOfLastSegment = lastSegment.End;
			var endOfWaveStream = (float)OrigWaveStream.TotalTime.TotalSeconds;
			if (endOfLastSegment.Equals(endOfWaveStream) || endOfWaveStream - endOfLastSegment > 5)
				return;
			if ((endOfWaveStream - endOfLastSegment) * 1000 < Settings.Default.MinimumSegmentLengthInMilliseconds)
				UpdateSegmentBoundary(lastSegment, OrigWaveStream.TotalTime);
			else
				AddIgnoredSegment(OrigWaveStream.TotalTime);
		}
	}
}
