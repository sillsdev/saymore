using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using Palaso.Reporting;
using Palaso.UI.WindowsForms;
using Palaso.UI.WindowsForms.Extensions;
using Palaso.UI.WindowsForms.Widgets.BetterGrid;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Media.MPlayer;
using GridSettings = Palaso.UI.WindowsForms.Widgets.BetterGrid.GridSettings;

namespace SayMore.Transcription.UI
{
	public class TextAnnotationEditorGrid : BetterGrid
	{
		private const int WM_LBUTTONDOWN = 0x201;
		private const int WM_LBUTTONUP = 0x202;
		private bool _eatNextLButtonUpEvent;

		public delegate bool PreProcessMouseClickHandler(int x, int y);
		public event PreProcessMouseClickHandler PreProcessMouseClick;

		public delegate void FontChangedHandler(Font newFont);
		public event FontChangedHandler TranscriptionFontChanged;
		public event FontChangedHandler TranslationFontChanged;

		public Func<AudioRecordingType, IEnumerable<AnnotationPlaybackInfo>> AnnotationPlaybackInfoProvider;
		public MediaPlayerViewModel PlayerViewModel { get; private set; }
		public bool PlaybackInProgress { get; private set; }
		public bool PreventPlayback { get; set; }

		private AnnotationComponentFile _annotationFile;
		private readonly List<AnnotationPlaybackInfo> _mediaFileQueue = new List<AnnotationPlaybackInfo>();
		private int _annotationPlaybackLoopCount;
		private Action _playbackProgressReportingAction;
//		private ToolTip _toolTip = new ToolTip();
//		private bool _paused = false;

		private System.Threading.Timer _delayBeginRowPlayingTimer;
		private bool _resizingColumnHeaders;

		/// ------------------------------------------------------------------------------------
		public TextAnnotationEditorGrid(Font transcriptionFont, Font translationFont)
		{
			Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			Margin = new Padding(0);
			VirtualMode = true;
			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			AllowUserToResizeRows = false;
			RowHeadersVisible = false;
			EditMode = DataGridViewEditMode.EditOnEnter;
			ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
			FullRowFocusRectangleColor = DefaultCellStyle.SelectionBackColor;
			DefaultCellStyle.SelectionForeColor = DefaultCellStyle.ForeColor;
			DefaultCellStyle.SelectionBackColor =
				ColorHelper.CalculateColor(Color.White, DefaultCellStyle.SelectionBackColor, 140);

			RowTemplate.Height = 25;
			RowTemplate.MinimumHeight = 24;

			PlayerViewModel = new MediaPlayerViewModel();

			PlayerViewModel.SetVolume(100);
			PlayerViewModel.SetSpeed(Settings.Default.AnnotationEditorPlaybackSpeedIndex);

			SetPlaybackProgressReportAction(null);

			SetColumnFonts(transcriptionFont, translationFont);
		}

		///// ------------------------------------------------------------------------------------
		//internal bool ShowF2ToolTip
		//{
		//    set
		//    {
		//        if (value)
		//        {
		//            ShowCellToolTips = false;
		//            ToolTip = PlaybackInProgress && !_paused ?
		//                LocalizationManager.GetString("SessionsView.Transcription.TextAnnotationEditor.F2ToPause",
		//                    "F2: Pause") :
		//                LocalizationManager.GetString("SessionsView.Transcription.TextAnnotationEditor.F2ToPlay",
		//                    "F2: Play");
		//        }
		//        else
		//        {
		//            ShowCellToolTips = true;
		//            ToolTip = string.Empty;
		//        }
		//    }
		//}

		///// ------------------------------------------------------------------------------------
		//private string ToolTip
		//{
		//    set
		//    {
		//        if (_toolTip.GetToolTip(this) != value)
		//            _toolTip.SetToolTip(this, value);
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		public void Load(AnnotationComponentFile file)
		{
			_annotationFile = file;

			this.SetWindowRedraw(false);
			EndEdit();
			RowCount = 0;
			Columns.Clear();

			if (_annotationFile == null || !_annotationFile.Tiers.Any())
				return;

			int rowCount = 0;

			foreach (var tier in _annotationFile.Tiers)
				rowCount = Math.Max(rowCount, AddColumnForTier(tier));
			RowCount = rowCount;
			Font = Columns[0].DefaultCellStyle.Font;

			this.SetWindowRedraw(true);
			Invalidate();

			if (Settings.Default.SegmentGrid != null)
				Settings.Default.SegmentGrid.InitializeGrid(this);

			// Select the first non-ignored row
			int targetRow = 0;
			int segmentCount = _annotationFile.Tiers.GetTimeTier().Segments.Count;
			while (targetRow < segmentCount && _annotationFile.Tiers.GetIsSegmentIgnored(targetRow))
				targetRow++;
			if (targetRow < segmentCount && CurrentCellAddress.X >= 0) // found a row that is not ignored.
				CurrentCell = Rows[targetRow].Cells[CurrentCellAddress.X];
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			var targetRow = CurrentCellAddress.Y;

			if (targetRow < 0 || !Visible || Height <= ColumnHeadersHeight)
				return;

			FirstDisplayedScrollingRowIndex = targetRow;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the initial width of text annotation columns so they fill the available,
		/// visible, grid space.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void FirstTimeColumnInitialization()
		{
			// If the grid already has settings saved, then don't
			// adjust the column widths to fit the available width.
			if (Settings.Default.SegmentGrid != null)
				return;

			var annotationCols = Columns.OfType<TextAnnotationColumn>().ToArray();

			var widthOfOtherCols = Columns.Cast<DataGridViewColumn>()
				.Where(col => !(col is TextAnnotationColumn)).Sum(col => col.Width);

			var availableWidthForAnnotationCols = ClientSize.Width - widthOfOtherCols -
				RowHeadersWidth - SystemInformation.VerticalScrollBarWidth;

			// Distribute the annotation columns evenly within the available space.
			foreach (var col in annotationCols)
				col.Width = availableWidthForAnnotationCols / annotationCols.Length - 1;
		}

		/// ------------------------------------------------------------------------------------
		private int AddColumnForTier(TierBase tier)
		{
			tier.GridColumn.InitializeColumnContextMenu();
			Columns.Add(tier.GridColumn);

			var col = tier.GridColumn as TextAnnotationColumn;
			if (col != null)
			{
				col.SegmentChangedAction = _annotationFile.Save;
				col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}

			return tier.Segments.Count();
		}

		/// ------------------------------------------------------------------------------------
		public Font TranscriptionFont
		{
			set
			{
				if (TranscriptionFontChanged != null)
					TranscriptionFontChanged(value);
				Refresh();
			}
		}

		/// ------------------------------------------------------------------------------------
		public Font TranlationFont
		{
			set
			{
				if (TranslationFontChanged != null)
					TranslationFontChanged(value);
				Refresh();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetColumnFonts(Font transcriptionFont, Font freeTranslationFont)
		{
			foreach (TextAnnotationColumn col in GetColumns().OfType<TextAnnotationColumn>())
			{
				col.DefaultCellStyle.Font = (col is TranscriptionAnnotationColumn) ?
					transcriptionFont : freeTranslationFont;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex > 0 && GetIgnoreStateForRow(e.RowIndex))
			{
				e.CellStyle.Font = Font;
				e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			}
			else
				e.CellStyle.Font = Columns[e.ColumnIndex].DefaultCellStyle.Font;
			base.OnCellFormatting(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the user is in a transcription cell, this will intercept the tab and shift+tab
		/// keys so they move to the next transcription cell or previous transcription cell
		/// respectively. I.e. the TimeTier column is bypassed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (msg.WParam.ToInt32() == (int)Keys.Tab)
			{
				if (IsCurrentCellInEditMode)
					EndEdit();

				if (CurrentCellAddress.X == ColumnCount - 1 && ModifierKeys != Keys.Shift &&
					CurrentCellAddress.Y < RowCount - 1)
				{
					CurrentCell = this[1, CurrentCellAddress.Y + 1];
					return true;
				}

				if (CurrentCellAddress.X == 1 && ModifierKeys == Keys.Shift &&
					CurrentCellAddress.Y > 0)
				{
					CurrentCell = this[ColumnCount - 1, CurrentCellAddress.Y - 1];
					return true;
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ProcessDownKey(TextBox txtBox)
		{
			if (base.ProcessDownKey(txtBox))
				return true;
			int targetRow = CurrentCellAddress.Y + 1;
			int segmentCount = _annotationFile.Tiers.GetTimeTier().Segments.Count;
			while (targetRow < segmentCount && _annotationFile.Tiers.GetIsSegmentIgnored(targetRow))
				targetRow++;
			if (targetRow < segmentCount) // found a subsequent row that is not ignored.
				CurrentCell = Rows[targetRow].Cells[CurrentCellAddress.X];
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ProcessUpKey(TextBox txtBox)
		{
			if (base.ProcessDownKey(txtBox))
				return true;
			int targetRow = CurrentCellAddress.Y - 1;
			while (targetRow >= 0 && _annotationFile.Tiers.GetIsSegmentIgnored(targetRow))
				targetRow--;
			if (targetRow >= 0) // found a previous row that is not ignored.
				CurrentCell = Rows[targetRow].Cells[CurrentCellAddress.X];
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_LBUTTONUP && _eatNextLButtonUpEvent)
			{
				// The last left button down was "eaten", so we need to eat the corresponding
				// button up. This is necessary because we found that if the user moves the
				// mouse between the down and the up, it results in the OnCellClick event
				// being fired (and possibly other click events we're not monitoring).
				_eatNextLButtonUpEvent = false;
				m.Msg = 0;
				m.Result = IntPtr.Zero;
			}
			else if (m.Msg == WM_LBUTTONDOWN && PreProcessMouseClick != null)
			{
				int x = (short)(m.LParam.ToInt32() & 0x0000FFFF);
				int y = (short)((m.LParam.ToInt32() & 0xFFFF0000) >> 16);

				if (HitTest(x, y).RowIndex != CurrentCellAddress.Y)
					Stop();
				else if (PreProcessMouseClick(x, y))
				{
					_eatNextLButtonUpEvent = true;
					m.Msg = 0;
					m.Result = IntPtr.Zero;
				}
			}

			base.WndProc(ref m);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
		{
			base.OnColumnWidthChanged(e);
			if (_resizingColumnHeaders)
				return;
			BeginInvoke((Action)ResizeColumnHeaders);

			// If this control doesn't have focus or all three standard columns have not yet
			// been added, the resize is not the result of the user dragging the column divider.
			if (ContainsFocus && ColumnCount >= 3)
				Settings.Default.SegmentGrid = GridSettings.Create(this);
		}

		/// ------------------------------------------------------------------------------------
		private void ResizeColumnHeaders()
		{
			_resizingColumnHeaders = true;
			AutoResizeColumnHeadersHeight();
			ColumnHeadersHeight += 8;
			_resizingColumnHeaders = false;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnRowPrePaint(DataGridViewRowPrePaintEventArgs e)
		{
			var i = e.RowIndex;
			if (i < 0)
				return;
			var enabled = ((CurrentRow != null && CurrentRow.Index == i) || !GetIgnoreStateForRow(i));
			var backColor = enabled ? BackgroundColor : ColorHelper.CalculateColor(GridColor, BackgroundColor, 128);
			Rows[i].DefaultCellStyle.BackColor = backColor;
			Rows[i].DefaultCellStyle.ForeColor = enabled ? ForeColor : ColorHelper.CalculateColor(ForeColor, backColor, 128);
			base.OnRowPrePaint(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (ColumnCount > 0)
				return;

			var hint = LocalizationManager.GetString("SessionsView.Transcription.TextAnnotationEditor.NoTranscriptionAnnotationsFoundMsg",
				"There are no transcription annotations found in\n'{0}'", "Parameter is file name.");

			DrawMessageInCenterOfGrid(e.Graphics, string.Format(hint,
				Path.GetFileName(_annotationFile.PathToAnnotatedFile)), 0);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
		{
			base.OnCellMouseClick(e);

			if (e.ColumnIndex < 0 || e.RowIndex < 0 || e.Button != MouseButtons.Right)
				return;

			var col = Columns[e.ColumnIndex] as TierColumnBase;

			if (col == null)
				return;

			var menuItems = col.GetContextMenuCommands().ToArray();
			if (menuItems.Length == 0)
				return;

			CurrentCell = Rows[e.RowIndex].Cells[e.ColumnIndex];
			var menu = new ContextMenuStrip();
			menu.Items.AddRange(menuItems.ToArray());
			menu.Show(MousePosition);
		}

		#region Playback methods
		/// ------------------------------------------------------------------------------------
		public void SetPlaybackSpeed(int playbackSpeed)
		{
			if (PlayerViewModel.Speed != playbackSpeed)
				PlayerViewModel.SetSpeed(playbackSpeed);
		}

		/// ------------------------------------------------------------------------------------
		public void SetPlaybackProgressReportAction(Action action)
		{
			_playbackProgressReportingAction = (action ?? (() => { }));
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
		{
			Stop();

			base.OnEditingControlShowing(e);

			SchedulePlaybackForCell();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellEnter(DataGridViewCellEventArgs e)
		{
			base.OnCellEnter(e);

			// Looks weird, but this solves an annoying problem where the wait cursor gets stuck
			// on randomly after displaying one of the segmentation dialogs.
			// See http://stackoverflow.com/questions/3008958/datagridview-retains-waitcursor-when-updated-from-thread/13808474#13808474
			Cursor = Cursors.Default;

			EditMode = (GetIgnoreStateForRow(CurrentCellAddress.Y)) ? DataGridViewEditMode.EditProgrammatically : DataGridViewEditMode.EditOnEnter;
			if (e.ColumnIndex != 0 || CurrentCellAddress.Y < 0 || (!Focused && (EditingControl == null || !EditingControl.Focused)))
			{
				var minHeight = RowTemplate.Height * 3;
				if (CurrentRow != null && CurrentRow.Height < minHeight)
					CurrentRow.MinimumHeight = minHeight;

				//ShowF2ToolTip = (Columns[e.ColumnIndex] is TextAnnotationColumn);
				return;
			}
			SchedulePlaybackForCell();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellLeave(DataGridViewCellEventArgs e)
		{
			base.OnCellLeave(e);
			if (e.RowIndex >= 0 && e.RowIndex < RowCount)
				Rows[e.RowIndex].MinimumHeight = RowTemplate.MinimumHeight;
		}

		/// ------------------------------------------------------------------------------------
		private void SchedulePlaybackForCell()
		{
			if (PreventPlayback)
				return;

			// Now that we're on a new row, wait a 1/4 of a second before beginning to
			// play this row's media segment. Do this just in case the user is moving
			// from row to row rapidly. Before the 1/4 sec. delay, the program's
			// responsiveness to moving from row to row rapidly was very sluggish. This
			// forces the user to settle on a row, at least briefly, before we attempt
			// to begin playback.
			_delayBeginRowPlayingTimer = new System.Threading.Timer(
				a => Play(), null, 250, System.Threading.Timeout.Infinite);
		}

		/// ------------------------------------------------------------------------------------
		private void DisableTimer()
		{
			if (_delayBeginRowPlayingTimer != null)
			{
				_delayBeginRowPlayingTimer.Dispose();
				_delayBeginRowPlayingTimer = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);

			if (Focused && !PlayerViewModel.HasPlaybackStarted &&
				!GetIgnoreStateForRow(CurrentCellAddress.Y))
			{
				Play();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);

			DisableTimer();
		}

		/// ------------------------------------------------------------------------------------
		public void Play()
		{
			Play(true);
		}

		/// ------------------------------------------------------------------------------------
		public void Play(bool resetLoopCounter)
		{
			if (RowCount == 0 || PreventPlayback)
				return;

			DisableTimer();

			if (PlayerViewModel.HasPlaybackStarted)
				Stop();

			if (resetLoopCounter)
				_annotationPlaybackLoopCount = 0;

			Debug.Assert(AnnotationPlaybackInfoProvider != null);

			if (AnnotationPlaybackInfoProvider == null)
				return;

			var currCol = Columns[CurrentCellAddress.X] as TextAnnotationColumnWithMenu;
			var playbackType = (currCol != null ? currCol.PlaybackType : AudioRecordingType.Source);

			lock (_mediaFileQueue)
			{
				_mediaFileQueue.Clear();
				_mediaFileQueue.AddRange(AnnotationPlaybackInfoProvider(playbackType));
			}
			InternalPlay();
		}

		/// ------------------------------------------------------------------------------------
		private bool InternalPlay()
		{
			string errorMessage = null;
			lock (_mediaFileQueue)
			{
				if (_mediaFileQueue.Count == 0)
					return false;

				PlayerViewModel.PlaybackStarted -= HandleMediaPlayStarted;
				PlayerViewModel.PlaybackEnded -= HandleMediaPlaybackEnded;

				try
				{
					LoadFile();
				}
				catch (Exception e)
				{
					_mediaFileQueue.Clear();
					errorMessage = e.Message;
				}
			}
			if (errorMessage != null)
			{
				if (InvokeRequired)
					Invoke((Action)(() => ErrorReport.NotifyUserOfProblem(errorMessage)));
				else
					ErrorReport.NotifyUserOfProblem(errorMessage);
				return false;
			}

			PlayerViewModel.PlaybackStarted += HandleMediaPlayStarted;
			PlayerViewModel.PlaybackEnded += HandleMediaPlaybackEnded;
			PlayerViewModel.PlaybackPositionChanged = HandleMediaPlaybackPositionChanged;
			//_paused = false;
			PlayerViewModel.Play();
			PlaybackInProgress = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void LoadFile()
		{
			if (_mediaFileQueue[0].Length > 0f)
			{
				PlayerViewModel.LoadFile(_mediaFileQueue[0].MediaFile, _mediaFileQueue[0].Start, _mediaFileQueue[0].Length);
			}
			else
			{
				PlayerViewModel.LoadFile(_mediaFileQueue[0].MediaFile);
				_mediaFileQueue[0].Length = _mediaFileQueue[0].End = PlayerViewModel.GetTotalMediaDuration();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			PlaybackInProgress = false;
			//_paused = false;
			_annotationPlaybackLoopCount = 0;

			DisableTimer();
			PlayerViewModel.PlaybackStarted -= HandleMediaPlayStarted;
			PlayerViewModel.PlaybackEnded -= HandleMediaPlaybackEnded;
			PlayerViewModel.PlaybackPositionChanged = null;
			PlayerViewModel.Stop();
			lock (_mediaFileQueue)
			{
				_mediaFileQueue.Clear();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Pause()
		{
			if (!PlaybackInProgress)
				return;

			DisableTimer();
			//_paused = !_paused;
			PlayerViewModel.Pause();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlaybackEnded(object sender, bool EndedBecauseEOF)
		{
			//_paused = false;

			if (InvokeRequired)
				Invoke(_playbackProgressReportingAction);
			else
				_playbackProgressReportingAction();

			if (!EndedBecauseEOF)
				return;

			lock (_mediaFileQueue)
			{
				if (_mediaFileQueue.Count > 0)
					_mediaFileQueue.RemoveAt(0);
			}


			if (!Visible)
				return;

			if (!InternalPlay())
			{
				if (_annotationPlaybackLoopCount++ < 4)
					Play(false);
				else
				{
					PlaybackInProgress = false;
					if (InvokeRequired)
						Invoke(_playbackProgressReportingAction);
					else
						_playbackProgressReportingAction();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlayStarted(object sender, EventArgs e)
		{
			if (InvokeRequired)
				Invoke(_playbackProgressReportingAction);
			else
				_playbackProgressReportingAction();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlaybackPositionChanged(float pos)
		{
			if (!Visible)
				Stop();
			else
				Invoke(_playbackProgressReportingAction);
		}

		/// ------------------------------------------------------------------------------------
		public void DrawPlaybackProgressBar(Graphics g, Rectangle rc, Color baseBackColor)
		{
			lock (_mediaFileQueue)
			{
				if (_mediaFileQueue.Count == 0 || !PlaybackInProgress)
					return;

				var playbackPosition = PlayerViewModel.CurrentPosition;
				if (playbackPosition.Equals(0f))
					return;

				var start = _mediaFileQueue[0].Start;
				var end = _mediaFileQueue[0].End;

				var length = Math.Round(end - start, 1, MidpointRounding.AwayFromZero);
				var pixelsPerSec = rc.Width / length;
				rc.Width = (int)Math.Ceiling(pixelsPerSec * (playbackPosition - start));
			}

			if (rc.Width <= 0)
				return;

			rc.Height -= 6;
			rc.Y += 3;
			using (var br = new SolidBrush(ColorHelper.CalculateColor(Color.White, baseBackColor, 110)))
				g.FillRectangle(br, rc);
		}
		#endregion

		#region Methods for getting/setting ignore state
		/// ------------------------------------------------------------------------------------
		public bool GetIgnoreStateForRow(int rowIndex)
		{
			return _annotationFile.Tiers.GetIsSegmentIgnored(rowIndex);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIgnoreStateForCurrentRow()
		{
			return GetIgnoreStateForRow(CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		public void SetIgnoreStateForCurrentRow(bool ignored)
		{
			if (ignored)
			{
				_annotationFile.Tiers.MarkSegmentAsIgnored(CurrentCellAddress.Y);
				Stop();
				EditMode = DataGridViewEditMode.EditProgrammatically;
				EndEdit();
			}
			else
			{
				_annotationFile.Tiers.MarkSegmentAsUnignored(CurrentCellAddress.Y);
				BeginEdit(true);
			}
			IsDirty = true;
			IgnoredColumn.HandleProgramaticValueChange();
			InvalidateRow(CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		private TextAnnotationColumn IgnoredColumn
		{
			get
			{
				return Columns.OfType<TextAnnotationColumn>().Single(c => c.Tier.TierType == TierType.Transcription);
			}
		}

		/// ------------------------------------------------------------------------------------
		public TimeRange GetTimeRangeForRow(int rowIndex)
		{
			// SP-912: Index was out of range. Must be non-negative and less than the size of the collection.
			// (This is happening after a segment is deleted)
			if (rowIndex >= _annotationFile.Tiers.GetTimeTier().Segments.Count)
				return new TimeRange(TimeSpan.Zero, TimeSpan.Zero);

			return _annotationFile.Tiers.GetTimeTier().Segments[rowIndex].TimeRange;
		}
		#endregion
	}
}
