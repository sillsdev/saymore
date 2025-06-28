using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DesktopAnalytics;
using L10NSharp;
using SIL.Reporting;
using SIL.Windows.Forms;
using SIL.Windows.Forms.Extensions;
using SIL.Windows.Forms.Widgets.BetterGrid;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Media.MPlayer;
using static SIL.Windows.Forms.Extensions.ControlExtensions.ErrorHandlingAction;
using GridSettings = SIL.Windows.Forms.Widgets.BetterGrid.GridSettings;

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
		public MediaPlayerViewModel PlayerViewModel { get; }
		public bool PlaybackInProgress { get; private set; }
		public bool PreventPlayback { get; set; }

		private AnnotationComponentFile _annotationFile;
		private readonly List<AnnotationPlaybackInfo> _mediaFileQueue = new List<AnnotationPlaybackInfo>();
		private int _annotationPlaybackLoopCount;
		private Action _playbackProgressReportingAction = () => { };
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

			RowCount = _annotationFile.Tiers.Select(AddColumnForTier).Concat(new[] { 0 }).Max();
			Font = Columns[0].DefaultCellStyle.Font;

			this.SetWindowRedraw(true);
			if (IsHandleCreated)
				Invalidate();
			else
			{
				try
				{
					CreateHandle();
				}
				catch (ObjectDisposedException)
				{
					// This probably can't happen, but just in case
					return;
				}
			}

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

			try
			{
				FirstDisplayedScrollingRowIndex = targetRow;
			}
			catch (InvalidOperationException)
			{
				// SP-942: There is an edge case where Height > ColumnHeadersHeight, but the
				// DataGridView still thinks it doesn't have enough room to display anything.
				// I suspect that the border thickness is coming into play, but I can't figure
				// out what the actual calculation should be. Ironically, unless I'm catching
				// exceptions in the debugger, I never see this exception, even though there is
				// no apparent try-catch in the call stack. But a real user got this exception,
				// so this catch should prevent this problem.
			}
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

			if (tier.GridColumn is TextAnnotationColumn col)
			{
				col.SegmentChangedAction = _annotationFile.Save;
				col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}

			return tier.Segments.Count;
		}

		/// ------------------------------------------------------------------------------------
		public Font TranscriptionFont
		{
			set
			{
				TranscriptionFontChanged?.Invoke(value);
				Refresh();
			}
		}

		/// ------------------------------------------------------------------------------------
		public Font TranslationFont
		{
			set
			{
				TranslationFontChanged?.Invoke(value);
				Refresh();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetColumnFonts(Font transcriptionFont, Font freeTranslationFont)
		{
			foreach (TextAnnotationColumn col in GetColumns().OfType<TextAnnotationColumn>())
			{
				col.DefaultCellStyle.Font = col is TranscriptionAnnotationColumn ?
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
		/// Asynchronously sets the current cell to the specified column and row
		/// </summary>
		/// <param name="column">The column index of the cell to set as current</param>
		/// <param name="row">The row index of the cell to set as current</param>
		/// <param name="methodNameAndContext">The name of the method from which this invocation
		/// was initiated, optionally followed by additional context information (used only for
		/// error reporting).</param>
		/// ------------------------------------------------------------------------------------
		private void InvokeSetCurrentCell(int column, int row, string methodNameAndContext)
		{
			var existingCell = CurrentCellAddress;
			// Changing the cell is going to require us to remove the media of the current cell
			// from the queue and then queue up the new media. There can be a race condition that
			// can result in deadlock if the media playback handling code happens to get this
			// lock and then subsequently tries to update the UI, so we want to stop playback
			// (which can itself invoke -- synchronously -- on the UI thread) and then we need to
			// ensure that we get this lock before we try to invoke the UI update code.
			Stop();
			lock (_mediaFileQueue)
			{
				this.SafeInvoke(() =>
					{
						// If something happened to change the current cell while we were waiting to
						// invoke this, it is probably no longer safe to set the current cell to the
						// one we were intending to make current. This is probably really unlikely,
						// but I'm adding this safeguard mainly because of uncertainty about the
						// behavior when running under Parallels on macOS.
						if (CurrentCellAddress.Equals(existingCell))
							CurrentCell = this[column, row];
					}, $"{GetType().Name}.{methodNameAndContext}",
					IgnoreIfDisposed);
			}
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
					InvokeSetCurrentCell(1, CurrentCellAddress.Y + 1,
						$"{nameof(ProcessCmdKey)} - Tab");
					return true;
				}

				if (CurrentCellAddress.X == 1 && ModifierKeys == Keys.Shift &&
					CurrentCellAddress.Y > 0)
				{
					InvokeSetCurrentCell(ColumnCount - 1, CurrentCellAddress.Y - 1,
						$"{nameof(ProcessCmdKey)} - Shift+Tab");
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
				InvokeSetCurrentCell(CurrentCellAddress.X, targetRow, nameof(ProcessDownKey));
		
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
				InvokeSetCurrentCell(CurrentCellAddress.X, targetRow, nameof(ProcessUpKey));

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
			var enabled = (CurrentRow != null && CurrentRow.Index == i) || !GetIgnoreStateForRow(i);
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

			if (e.ColumnIndex < 0 || e.RowIndex < 0 || e.Button != MouseButtons.Right ||
			    !(Columns[e.ColumnIndex] is TierColumnBase col))
				return;

			var menuItems = col.GetContextMenuCommands().ToArray();
			if (menuItems.Length == 0)
				return;

			var clickedCell = Rows[e.RowIndex].Cells[e.ColumnIndex];
			// To avoid possibility of churn or weird side effects (esp. on Mac Parallels),
			// let's only set this if the click occured in a cell other than the current one.
			if (CurrentCell != clickedCell)
			{
				// No need to Invoke, since OnCellMouseClick can only be called on the UI thread.
				CurrentCell = clickedCell;
			}

			var menu = new ContextMenuStrip();
			menu.Items.AddRange(menuItems);
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
			// See https://stackoverflow.com/questions/3008958/datagridview-retains-waitcursor-when-updated-from-thread/13808474#13808474
			Cursor = Cursors.Default;

			EditMode = GetIgnoreStateForRow(CurrentCellAddress.Y) ? DataGridViewEditMode.EditProgrammatically : DataGridViewEditMode.EditOnEnter;
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
		/// <summary>This is a fix for SP-960, needed to fully display the last row of the text
		/// annotation grid when the row has been resized in response to the user entering it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnRowHeightChanged(DataGridViewRowEventArgs e)
		{
			base.OnRowHeightChanged(e);
			var row = e.Row;
			if (CurrentRow == row && row.Index == RowCount - 1)
			{
				// SP-2360: Not safe to set FirstDisplayedScrollingRowIndex while laying out.
				this.SafeInvoke(() =>
					{
						// This redundant check is a safeguard on the off-chance the state has
						// changed between the original check and this invoke. 
						if (CurrentRow == row && row.Index == RowCount - 1)
							FirstDisplayedScrollingRowIndex = row.Index;
					},
					$"{nameof(OnRowHeightChanged)} fix for SP-960", IgnoreAll);
			}
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

			// The check below to ensure that CurrentCellAddress.X is in range seems unnecessary.
			// However, an out-of-range value seems to be the likely explanation for the error
			// reported in SP-2219/SP-2220. This does not normally fire on the UI thread, so using
			// SafeInvoke to ensure that CurrentCellAddress is not in some weird state.
			int iCol = 0;
			this.SafeInvoke(() => { iCol = CurrentCellAddress.X; }, nameof(Play), IgnoreIfDisposed,
				true);

			var currCol = iCol >= 0 && iCol < ColumnCount ?
				Columns[iCol] as TextAnnotationColumnWithMenu : null;
			var playbackType = currCol?.PlaybackType ?? AudioRecordingType.Source;

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
				this.SafeInvoke(() => { ErrorReport.NotifyUserOfProblem(errorMessage); },
					nameof(InternalPlay), IgnoreIfDisposed);
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
			var firstAnnotation = _mediaFileQueue[0];
			if (firstAnnotation.Length > 0f)
			{
				PlayerViewModel.LoadFile(firstAnnotation.MediaFile, firstAnnotation.Start,
					firstAnnotation.Length);
			}
			else
			{
				PlayerViewModel.LoadFile(firstAnnotation.MediaFile);
				firstAnnotation.Length = firstAnnotation.End =
					PlayerViewModel.GetTotalMediaDuration();
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
		private void HandleMediaPlaybackEnded(object sender, bool endedBecauseEOF)
		{
			//_paused = false;

			ReportPlayBackProgress("playback ended");

			if (!endedBecauseEOF)
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
					ReportPlayBackProgress("playback ended after max iterations");
				}
			}
		}

		private void ReportPlayBackProgress(string context)
		{
			this.SafeInvoke(_playbackProgressReportingAction, context, IgnoreIfDisposed);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlayStarted(object sender, EventArgs e)
		{
			if (_playbackProgressReportingAction == null)
			{
				// SP-952/SP-953: This is theoretically impossible, but it seems to have happened
				// twice to a real user. I've moved the initialization of this field up to where
				// it should be even more impossible, but for safety's sake, I'm also checking
				// for null here.
				Analytics.Track("Error: _playbackProgressReportingAction is null in HandleMediaPlayStarted");
				return;
			}
			ReportPlayBackProgress("playback started");
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlaybackPositionChanged(float pos)
		{
			if (!Visible)
				Stop();
			else
				ReportPlayBackProgress("playback position changed");
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
