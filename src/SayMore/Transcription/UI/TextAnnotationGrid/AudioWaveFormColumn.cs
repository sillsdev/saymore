using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using SIL.Windows.Forms;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	public class AudioWaveFormColumn : TierColumnBase
	{
		protected bool _mediaFileNeedsLoading = true;
		protected Control _gridEditControl;

		/// ------------------------------------------------------------------------------------
		public AudioWaveFormColumn(TierBase tier) : base(new AudioWaveFormCell(), tier)
		{
			Debug.Assert(tier.TierType == TierType.Time);
			ReadOnly = true;

			DefaultCellStyle.Font = FontHelper.MakeFont(Program.DialogFont, 9f);
			MinimumWidth = 40;
		}

		/// ------------------------------------------------------------------------------------
		public override DataGridViewCell CellTemplate
		{
			get { return base.CellTemplate; }
			set
			{
				// Ensure that the cell used for the template is a CalendarCell.
				if (value != null && !value.GetType().IsAssignableFrom(typeof(AudioWaveFormCell)))
					throw new InvalidCastException("Cell template must be an AudioWaveFormCell");

				base.CellTemplate = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void SubscribeToGridEvents()
		{
			//_grid.Leave += HandleGridLeave;
			//_grid.Enter += HandleGridEnter;
			_grid.CellEnter += HandleCellEnter;
			_grid.PreProcessMouseClick += HandleGridPreProcessMouseClick;
			_grid.SetPlaybackProgressReportAction(() => _grid.InvalidateCell(Index, _grid.CurrentCellAddress.Y));

			//_grid.PlaybackSpeedChanged += HandlePlaybackSpeedChanged;

			_grid.KeyDown += HandleKeyDown;
			_grid.EditingControlShowing += HandleGridEditControlShowing;
			_grid.CellEndEdit += HandleGridCellEndEdit;

			base.SubscribeToGridEvents();
		}

		/// ------------------------------------------------------------------------------------
		protected override void UnsubscribeToGridEvents()
		{
			_grid.PreProcessMouseClick -= HandleGridPreProcessMouseClick;
			_grid.KeyDown -= HandleKeyDown;
			_grid.CellEnter -= HandleCellEnter;
			_grid.SetPlaybackProgressReportAction(null);
			_grid.EditingControlShowing -= HandleGridEditControlShowing;
			_grid.CellEndEdit -= HandleGridCellEndEdit;
			base.UnsubscribeToGridEvents();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridEditControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			if (_gridEditControl != null)
				_gridEditControl.KeyDown -= HandleKeyDown;

			_gridEditControl = e.Control;
			_gridEditControl.KeyDown += HandleKeyDown;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			_gridEditControl.KeyDown -= HandleKeyDown;
			_gridEditControl = null;
		}

		/// ------------------------------------------------------------------------------------
		bool HandleGridPreProcessMouseClick(int x, int y)
		{
			var rc = _grid.GetColumnDisplayRectangle(Index, true);
			if (x < rc.X || x > rc.Right)
				return false;

			for (int i = 0; i < _grid.RowCount; i++)
			{
				var cell = _grid[Index, i] as AudioWaveFormCell;

				if (!cell.IsMouseIsOverButtonArea)
					continue;

				if (_grid.PlaybackInProgress)
					_grid.Stop();
				else
					_grid.Play();

				_grid.InvalidateCell(cell);

				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCellEnter(object sender, DataGridViewCellEventArgs e)
		{
			_grid.AnnotationPlaybackInfoProvider = (t => GetAnnotationMediaInfo(t));
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<AnnotationPlaybackInfo> GetAnnotationMediaInfo(AudioRecordingType playbackType)
		{
			var carefulSpeechFile = GetCurrentSegment().GetFullPathToCarefulSpeechFile();
			var oralTranslationFile = GetCurrentSegment().GetFullPathToOralTranslationFile();

			if ((playbackType & AudioRecordingType.Source) == AudioRecordingType.Source)
				yield return GetOriginalAnnotationPlaybackInfo();

			if ((playbackType & AudioRecordingType.Careful) == AudioRecordingType.Careful)
			{
				if (File.Exists(carefulSpeechFile))
					yield return new AnnotationPlaybackInfo { MediaFile = carefulSpeechFile };
				else if (playbackType == AudioRecordingType.Careful)
					yield return GetOriginalAnnotationPlaybackInfo();
			}

			if ((playbackType & AudioRecordingType.Translation) == AudioRecordingType.Translation)
			{
				if (File.Exists(oralTranslationFile))
					yield return new AnnotationPlaybackInfo { MediaFile = oralTranslationFile };
				else if (playbackType == AudioRecordingType.Translation)
					yield return GetOriginalAnnotationPlaybackInfo();
			}
		}

		/// ------------------------------------------------------------------------------------
		private AnnotationPlaybackInfo GetOriginalAnnotationPlaybackInfo()
		{
			var segment = GetCurrentSegment();

			return new AnnotationPlaybackInfo
			{
				MediaFile = ((TimeTier)Tier).MediaFileName,
				Start = segment.Start,
				End = segment.End,
				Length = segment.GetLength(1)
			};
		}

		/// ------------------------------------------------------------------------------------
		public AnnotationSegment GetCurrentSegment()
		{
			return Tier.Segments[_grid.CurrentCellAddress.Y];
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			base.HandleGridCellValueNeeded(sender, e);

			if (e.ColumnIndex != Index)
				return;

			e.Value = e.RowIndex + 1;
		}

		/// ------------------------------------------------------------------------------------
		public override void InitializeColumnContextMenu()
		{
			// no-op
		}

		/// ------------------------------------------------------------------------------------
		void HandleKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.F2)
				return;

			if (_grid.PlaybackInProgress)
				_grid.Pause();
			else
				_grid.Play();

			_grid.InvalidateCell(Index, _grid.CurrentCellAddress.Y);
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class AnnotationPlaybackInfo
	{
		public string MediaFile;
		public float Start;
		public float End;
		public float Length;
	}
}
