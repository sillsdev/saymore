using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Localization;
using Localization.UI;
using NAudio.Wave;
using Palaso.UI.WindowsForms.Miscellaneous;
using SayMore.Media.Audio;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.UI.LowLevelControls;
using SayMore.Media.MPlayer;
using SayMore.Utilities;
using SilTools;
using Timer = System.Windows.Forms.Timer;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class SegmenterDlgBase : MonitorKeyPressDlg
	{
		protected enum SegmentDefinitionMode
		{
			HoldingSpace,
			PressingButton,
			AddButtonWhileListening,
			Manual,
		}

		protected readonly SegmenterDlgBaseViewModel _viewModel;
		protected string _segmentNumberFormat;
		protected string _segmentXofYFormat;
		protected Timer _timer;
		protected TimeSpan _timeAtBeginningOfBoundaryMove = TimeSpan.FromSeconds(1).Negate();
		protected bool _moreReliableDesignMode;
		private WaveControlBasic _waveControl;
		protected SegmentDefinitionMode _newSegmentDefinedBy;

		public event Action SegmentIgnored;

		#region Constructor
		/// ------------------------------------------------------------------------------------
		public SegmenterDlgBase()
		{
			InitializeComponent();
			base.DoubleBuffered = true;

			_moreReliableDesignMode = (DesignMode || GetService(typeof(IDesignerHost)) != null) ||
				(LicenseManager.UsageMode == LicenseUsageMode.Designtime);

			if (_moreReliableDesignMode)
				return;

			WaitCursor.Show();

			_toolStripStatus.Renderer = new SilTools.NoToolStripBorderRenderer();
			_panelWaveControl.BackColor = Settings.Default.BarColorBorder;

			_buttonCancel.Click += delegate { Close(); };
			_buttonOK.Click += delegate { Close(); };

			_segmentXofYFormat = _labelSegmentXofY.Text;
			_segmentNumberFormat = _labelSegmentNumber.Text;

			LocalizeItemDlg.StringsLocalized += HandleStringsLocalized;
		}

		/// ------------------------------------------------------------------------------------
		public SegmenterDlgBase(SegmenterDlgBaseViewModel viewModel) : this()
		{
			_viewModel = viewModel;
			_viewModel.UpdateDisplayProvider = UpdateDisplay;
			_viewModel.OralAnnotationWaveAreaRefreshAction = () => _waveControl.InvalidateBottomReservedArea();

			if (!_moreReliableDesignMode && FormSettings == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				FormSettings = FormSettings.Create(this);
			}

			_labelTimeDisplay.Text = MediaPlayerViewModel.GetTimeDisplay(0f,
				(float)_viewModel.OrigWaveStream.TotalTime.TotalSeconds);
		}
		#endregion

		#region Loading, initialization, etc.
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (_moreReliableDesignMode)
				return;

			_tableLayoutOuter.RowStyles.Clear();
			_tableLayoutOuter.RowCount = 4;
			_tableLayoutOuter.RowStyles.Add(new RowStyle());
			_tableLayoutOuter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			_tableLayoutOuter.RowStyles.Add(new RowStyle(SizeType.Absolute, HeightOfTableLayoutButtonRow));
			_tableLayoutOuter.RowStyles.Add(new RowStyle());

			_waveControl = CreateWaveControl();
			InitializeWaveControl();
			_waveControl.Dock = DockStyle.Fill;
			_panelWaveControl.Controls.Add(_waveControl);
			_waveControl.BringToFront();

			_waveControl.FormatNotSupportedMessageProvider = waveFormat =>
			{
				if (waveFormat.Channels > 2)
				{
					return string.Format(LocalizationManager.GetString(
						"DialogBoxes.Transcription.SegmenterDlgBase.Segmenting3ChannelAudioFilesNotSupportedMsg",
						"Segmenting {0} channel audio files is not supported."), waveFormat.Channels);
				}

				return string.Format(LocalizationManager.GetString(
					"DialogBoxes.Transcription.SegmenterDlgBase.SegmentingAudioFormatNotSupportedMsg",
					"Segmenting {0} bit, {1} audio files is not supported."),
					waveFormat.BitsPerSample, waveFormat.Encoding);
			};

			InitializeZoomComboItems();
			_comboBoxZoom.Text = _comboBoxZoom.Items[0] as string;
			_comboBoxZoom.Font = Program.DialogFont;
			_labelZoom.Font = Program.DialogFont;
			_labelSegmentXofY.Font = Program.DialogFont;
			_labelSegmentNumber.Font = Program.DialogFont;
			_labelTimeDisplay.Font = Program.DialogFont;
			_labelSourceRecording.Font = _undoToolStripMenuItem.Font = _ignoreToolStripMenuItem.Font =
				FontHelper.MakeFont(Program.DialogFont, FontStyle.Bold);
			_undoToolStripMenuItem.AutoSize = false;
			_ignoreToolStripMenuItem.AutoSize = false;
			_undoToolStripMenuItem.Width = _ignoreToolStripMenuItem.Width =
				Math.Max(_ignoreToolStripMenuItem.Width, _undoToolStripMenuItem.Width);
			_undoToolStripMenuItem.Height *= 2;
			_ignoreToolStripMenuItem.Height = _undoToolStripMenuItem.Height;

			// If we ever get zooming working again, remove the following two
			// lines and uncomment the two below them.
			_labelZoom.Visible = false;
			_comboBoxZoom.Visible = false;
			//_waveControl.ZoomPercentage = 300; //ZoomPercentage;
			//_comboBoxZoom.Text = string.Format("{0}%", ZoomPercentage);

			HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		public void InitializeWaveControl()
		{
			_waveControl.Initialize(_viewModel.OrigWaveStream as WaveFileReader);
			_waveControl.SegmentBoundaries = _viewModel.GetSegmentEndBoundaries();
			_waveControl.Painter.SetIgnoredRegions(_viewModel.GetIgnoredSegmentRanges());
			_waveControl.PlaybackStarted += OnPlaybackStarted;
			_waveControl.PlaybackUpdate += OnPlayingback;
			_waveControl.PlaybackStopped += OnPlaybackStopped;
			_waveControl.MouseMove += HandleWaveControlMouseMove;
			_waveControl.Controls.Add(_currentSegmentMenuStrip);
			_currentSegmentMenuStrip.UseWaitCursor = false;
			_waveControl.Controls.Add(_lastSegmentMenuStrip);
			_lastSegmentMenuStrip.UseWaitCursor = false;

			var waveCtrl = _waveControl as WaveControlWithMovableBoundaries;
			if (waveCtrl != null)
				waveCtrl.CanBoundaryBeMoved += b => !_viewModel.IsBoundaryPermanent(b);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				LocalizeItemDlg.StringsLocalized -= HandleStringsLocalized;

				if (components != null)
					components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual WaveControlBasic CreateWaveControl()
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleStringsLocalized()
		{
			_segmentXofYFormat = _labelSegmentXofY.Text;
			_segmentNumberFormat = _labelSegmentNumber.Text;
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeZoomComboItems()
		{
			_comboBoxZoom.Items.Add(LocalizationManager.GetString(
				"DialogBoxes.Transcription.SegmenterDlgBase.ZoomPercentages.100Pct", "100%"));
			_comboBoxZoom.Items.Add(LocalizationManager.GetString(
				"DialogBoxes.Transcription.SegmenterDlgBase.ZoomPercentages.125Pct", "125%"));
			_comboBoxZoom.Items.Add(LocalizationManager.GetString(
				"DialogBoxes.Transcription.SegmenterDlgBase.ZoomPercentages.150Pct", "150%"));
			_comboBoxZoom.Items.Add(LocalizationManager.GetString(
				"DialogBoxes.Transcription.SegmenterDlgBase.ZoomPercentages.175Pct", "175%"));
			_comboBoxZoom.Items.Add(LocalizationManager.GetString(
				"DialogBoxes.Transcription.SegmenterDlgBase.ZoomPercentages.200Pct", "200%"));
			_comboBoxZoom.Items.Add(LocalizationManager.GetString(
				"DialogBoxes.Transcription.SegmenterDlgBase.ZoomPercentages.250Pct", "250%"));
			_comboBoxZoom.Items.Add(LocalizationManager.GetString(
				"DialogBoxes.Transcription.SegmenterDlgBase.ZoomPercentages.300Pct", "300%"));
			_comboBoxZoom.Items.Add(LocalizationManager.GetString(
				"DialogBoxes.Transcription.SegmenterDlgBase.ZoomPercentages.500Pct", "500%"));
			_comboBoxZoom.Items.Add(LocalizationManager.GetString(
				"DialogBoxes.Transcription.SegmenterDlgBase.ZoomPercentages.750Pct", "750%"));
			_comboBoxZoom.Items.Add(LocalizationManager.GetString(
				"DialogBoxes.Transcription.SegmenterDlgBase.ZoomPercentages.1000Pct", "1000%"));
		}
		#endregion

		/// ------------------------------------------------------------------------------------
		protected string GetSegmentTooShortText()
		{
			switch (_newSegmentDefinedBy)
			{
				case SegmentDefinitionMode.AddButtonWhileListening:
					return LocalizationManager.GetString(
						"DialogBoxes.Transcription.ManualSegmenterDlg.ButtonTextWhenSegmentTooShort",
						"Whoops! The segment will be too short. Continue listening.");
				case SegmentDefinitionMode.HoldingSpace:
					return LocalizationManager.GetString(
						"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.MessageWhenSegmentTooShortHoldingSpace",
						"You need to keep the SPACE BAR down while you listen.");
				case SegmentDefinitionMode.PressingButton:
					return LocalizationManager.GetString(
						"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.MessageWhenSegmentTooShortPressingButton",
						"You need to hold the button down while you listen.");
				default:
					return LocalizationManager.GetString(
						"DialogBoxes.Transcription.SegmenterDlgBase.MessageWhenSegmentTooShortManualDragging",
						"Whoops! The segment will be too short.");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual int HeightOfTableLayoutButtonRow
		{
			get { return 0; }
		}

		/// ------------------------------------------------------------------------------------
		internal Rectangle GetFullRectangleForTimeRange(TimeRange timeRange)
		{
			return _waveControl.Painter.GetFullRectangleForTimeRange(timeRange);
		}

		/// ------------------------------------------------------------------------------------
		protected Rectangle GetRectangleForTimeRangeBeyondEndOfLastSegment(TimeSpan end)
		{
			var start = _viewModel.GetEndOfLastSegment();
			if (end <= start)
				return Rectangle.Empty;

			var x1 = _waveControl.Painter.ConvertTimeToXCoordinate(start);
			var x2 = _waveControl.Painter.ConvertTimeToXCoordinate(end);
			return new Rectangle(x1, 0, x2 - x1 + 1, _waveControl.ClientSize.Height);
		}

		#region Hot segment - the segment the mouse is over
		/// ------------------------------------------------------------------------------------
		internal Point MousePositionInWaveControl
		{
			get { return _waveControl.PointToClient(MousePosition); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual TimeRange CurrentTimeRange
		{
			get { return _waveControl.GetTimeRangeEnclosingMouseX(); }
		}

		/// ------------------------------------------------------------------------------------
		protected Segment HotSegment
		{
			get
			{
				return _viewModel.TimeTier.Segments.FirstOrDefault(s => s.TimeRange == CurrentTimeRange);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected Rectangle HotSegmentRectangle
		{
			get
			{
				var hotSegment = HotSegment;
				if (hotSegment != null)
					return GetFullRectangleForTimeRange(hotSegment.TimeRange);

				var rc = GetRectangleForTimeRangeBeyondEndOfLastSegment(_viewModel.VirtualBoundaryBeyondLastSegment);
				return rc.Contains(MousePositionInWaveControl) ? rc : Rectangle.Empty;
			}
		}
		#endregion

		/// ------------------------------------------------------------------------------------
		protected virtual FormSettings FormSettings
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual float ZoomPercentage
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool ShouldShadePlaybackAreaDuringPlayback
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>This is really probably just a temporary kludge to allow me to put most
		/// of the Undo code into this base class in preparation for the day when users will ask
		/// for Undo to work in the manual segmenter dialog. It's mostly ready to go by just
		/// making this return true, but there are some actions that the user can do in the
		/// manual segmented dialog that cannot yet be undone.</summary>
		/// ------------------------------------------------------------------------------------
		protected virtual bool UndoImplemented
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		protected bool IsBoundaryMovingInProgressUsingArrowKeys
		{
			get { return (_timeAtBeginningOfBoundaryMove >= TimeSpan.Zero); }
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			if (!_moreReliableDesignMode)
				FormSettings.InitializeForm(this);

			base.OnShown(e);

			Application.DoEvents();
			Opacity = 1f;
			WaitCursor.Hide();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			e.Cancel = true;
			StopAllMedia();

			if (!_moreReliableDesignMode)
				ZoomPercentage = _waveControl.ZoomPercentage;

			// Cancel means the user closed the form using the X or Alt+F4. In that
			// case whether they want to save changes is ambiguous. So ask them.
			if (DialogResult == DialogResult.Cancel && _viewModel.WereChangesMade)
			{
				DialogResult = DialogResult.OK;

				var msg = LocalizationManager.GetString(
					"DialogBoxes.Transcription.SegmenterDlgBase.SaveChangesQuestion",
					"Would you like to save your changes?");

				DialogResult = Utils.MsgBox(msg, MessageBoxButtons.YesNoCancel);
				if (DialogResult == DialogResult.Cancel)
					return;
				DialogResult = DialogResult.OK;
			}

			e.Cancel = false;

			if (DialogResult == DialogResult.OK)
				_viewModel.CreateMissingTextSegmentsToMatchTimeSegmentCount();

			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTableLayoutButtonsPaint(object sender, PaintEventArgs e)
		{
			//using (var br = new LinearGradientBrush(_tableLayoutButtons.ClientRectangle,
			//    AppColors.BarEnd, AppColors.BarBegin, 0f))
			//{
			//        e.Graphics.FillRectangle(br, _tableLayoutButtons.ClientRectangle);
			//}

			using (var pen = new Pen(AppColors.BarBorder))
				e.Graphics.DrawLine(pen, 0, 0, _tableLayoutButtons.Width, 0);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnPlaybackStarted(WaveControlBasic ctrl, TimeSpan start, TimeSpan end)
		{
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnPlayingback(WaveControlBasic ctrl, TimeSpan current, TimeSpan total)
		{
			_labelTimeDisplay.Text = MediaPlayerViewModel.GetTimeDisplay(
				(float)current.TotalSeconds, (float)total.TotalSeconds);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnPlaybackStopped(WaveControlBasic ctrl, TimeSpan start, TimeSpan end)
		{
			UpdateDisplay();
		}

		#region Methods for adjusting/saving/playing within segment boundaries
		/// ------------------------------------------------------------------------------------
		protected virtual bool OnAdjustBoundaryUsingArrowKey(int milliseconds)
		{
			StopAllMedia();

			var boundary = GetBoundaryToAdjustOnArrowKeys();

			var timeAtBeginningOfBoundaryMove = (IsBoundaryMovingInProgressUsingArrowKeys) ?
				_timeAtBeginningOfBoundaryMove : boundary;

			if (boundary == TimeSpan.Zero || _viewModel.IsBoundaryPermanent(timeAtBeginningOfBoundaryMove) ||
				!_viewModel.CanMoveBoundary(boundary, milliseconds))
			{
				return false;
			}

			_timeAtBeginningOfBoundaryMove = timeAtBeginningOfBoundaryMove;
			_waveControl.IgnoreMouseProcessing(true);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void FinalizeBoundaryMovedUsingArrowKeys()
		{
			_waveControl.SegmentBoundaries = _viewModel.GetSegmentEndBoundaries();
			_waveControl.IgnoreMouseProcessing(false);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual TimeSpan GetBoundaryToAdjustOnArrowKeys()
		{
			return TimeSpan.Zero;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleWaveControlMouseMove(object sender, MouseEventArgs e)
		{
			bool enableIgnoreMenu = false;
			var rc = HotSegmentRectangle;
			if (rc != Rectangle.Empty)
			{
				if (!_waveControl.IsPlaying)
				{
					enableIgnoreMenu = true;

					var currentSegmentMenuStripLocation = new Point(rc.Right -
						_currentSegmentMenuStrip.Width - Math.Min(5, (rc.Width - _currentSegmentMenuStrip.Width) / 2),
						rc.Top + 5);

					var hotSegment = HotSegment;

					// If the undo button is also showing for this same segment. we need to shift the
					// ignore to the left or down to allow it to fit without overlapping.
					if (UndoImplemented && _viewModel.TimeRangeForUndo != null &&
						hotSegment != null && _viewModel.TimeRangeForUndo == hotSegment.TimeRange)
					{

						if (rc.Width > _lastSegmentMenuStrip.Width + _currentSegmentMenuStrip.Width + 15)
							currentSegmentMenuStripLocation.Offset(-(_lastSegmentMenuStrip.Width + 5), 0); // shift left
						else
							currentSegmentMenuStripLocation.Offset(0, _lastSegmentMenuStrip.Height + 5); // shift down
					}

					_currentSegmentMenuStrip.Location = currentSegmentMenuStripLocation;

					_ignoreToolStripMenuItem.Checked = hotSegment != null && _viewModel.GetIsSegmentIgnored(hotSegment);
				}
			}
			_currentSegmentMenuStrip.Visible = _ignoreToolStripMenuItem.Enabled = enableIgnoreMenu;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleIgnoreToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			_ignoreToolStripMenuItem.Image = _ignoreToolStripMenuItem.Checked ?
				Resources.CheckedBox : Resources.UncheckedBox;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleIgnoreButtonClick(object sender, EventArgs e)
		{
			var ignore = _ignoreToolStripMenuItem.Checked;
			_viewModel.SetIgnoredFlagForSegment(HotSegment, ignore);
			_waveControl.InvalidateIfNeeded(HotSegmentRectangle);
			if (ignore)
				_waveControl.Painter.AddIgnoredRegion(HotSegment.TimeRange);
			else
				_waveControl.Painter.RemoveIgnoredRegion(HotSegment.TimeRange.End);
			if (SegmentIgnored != null)
				SegmentIgnored();
			HandleWaveControlMouseMove(_waveControl,
				new MouseEventArgs(MouseButtons.None, 0, MousePosition.X, MousePosition.Y, 0));
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleUndoButtonClick(object sender, EventArgs e)
		{
			_viewModel.Undo();
			_waveControl.Painter.SetIgnoredRegions(_viewModel.GetIgnoredSegmentRanges());
			HandleWaveControlMouseMove(_waveControl,
				new MouseEventArgs(MouseButtons.None, 0, MousePosition.X, MousePosition.Y, 0));
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		public bool HandleSegmentBoundaryMovedInWaveControl(WaveControlWithMovableBoundaries waveCtrl,
			TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			StopAllMedia();
			var segMoved = _viewModel.SegmentBoundaryMoved(oldEndTime, newEndTime);
			_waveControl.SegmentBoundaries = _viewModel.GetSegmentEndBoundaries();
			OnSegmentBoundaryMovedInWaveControl(segMoved, oldEndTime, newEndTime);
			return segMoved;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnSegmentBoundaryMovedInWaveControl(bool segMoved,
			TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			UpdateDisplay();

			if (segMoved)
				PlaybackShortPortionUpToBoundary(newEndTime);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void PlaybackShortPortionUpToBoundary(TimeSpan boundary)
		{
			if (boundary == TimeSpan.Zero)
				return;

			var playbackStartTime = boundary.Subtract(TimeSpan.FromMilliseconds(
				Settings.Default.MillisecondsToRePlayAfterAdjustingSegmentBoundary));

			if (playbackStartTime < TimeSpan.Zero)
				playbackStartTime = TimeSpan.Zero;

			// Make sure the playback doesn't start before the beginning of the segment.
			var boundaries = _viewModel.GetSegmentEndBoundaries().ToList();
			var i = boundaries.IndexOf(boundary);
			if (i > 0 && playbackStartTime < boundaries[i - 1])
				playbackStartTime = boundaries[i - 1];

			_timer = new Timer();
			_timer.Interval = Settings.Default.MillisecondsToDelayPlaybackAfterAdjustingSegmentBoundary;
			_timer.Tick += delegate
			{
				if (!_waveControl.IsPlaying)
				{
					_waveControl.Play(playbackStartTime, boundary);
					_waveControl.PlaybackStopped += PlaybackShortPortionUpToBoundary;
				}

				_timer.Stop();
				_timer.Dispose();
				_timer = null;
			};

			_timer.Start();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void PlaybackShortPortionUpToBoundary(WaveControlBasic ctrl,
			TimeSpan time1, TimeSpan time2)
		{
			_waveControl.PlaybackStopped -= PlaybackShortPortionUpToBoundary;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void StopAllMedia()
		{
			if (_timer != null)
			{
				_timer.Stop();
				_timer.Dispose();
				_timer = null;
			}

			_waveControl.Stop();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected virtual void UpdateDisplay()
		{
			if (UndoImplemented)
			{
				var undoableSegmentRange = _viewModel.TimeRangeForUndo;
				_lastSegmentMenuStrip.Visible = _undoToolStripMenuItem.Enabled = (undoableSegmentRange != null);
				if (_lastSegmentMenuStrip.Visible)
				{
					var rcUndoSegment = GetFullRectangleForTimeRange(undoableSegmentRange);
					int x = (rcUndoSegment.Width > _lastSegmentMenuStrip.Width + 10) ?
						rcUndoSegment.Right - _lastSegmentMenuStrip.Width - 5 :
						rcUndoSegment.Right -
						_lastSegmentMenuStrip.Width - Math.Min(5, (rcUndoSegment.Width - _lastSegmentMenuStrip.Width) / 2);
					_lastSegmentMenuStrip.Location = new Point(x, 5);
					_undoToolStripMenuItem.ToolTipText = String.Format(LocalizationManager.GetString(
						"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.UndoToolTipMsg",
						"Undo: {0} (Ctrl-Z or Z)"), _viewModel.DescriptionForUndo);
				}
			}
			UpdateStatusLabelsDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void UpdateStatusLabelsDisplay()
		{
			_labelSegmentXofY.Visible = false;
			_labelSegmentNumber.Visible = true;
			_labelSegmentNumber.Text = string.Format(_segmentNumberFormat, _viewModel.GetSegmentCount());

			_labelTimeDisplay.Text = MediaPlayerViewModel.GetTimeDisplay(
				(float)GetCurrentTimeForTimeDisplay().TotalSeconds,
				(float)_viewModel.OrigWaveStream.TotalTime.TotalSeconds);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual TimeSpan GetCurrentTimeForTimeDisplay()
		{
			return _waveControl.GetCursorTime();
		}

		#region Low level keyboard handling
		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyDown(Keys key)
		{
			if (!ContainsFocus)
				return true;

			switch (key)
			{
				case Keys.Right:
					OnAdjustBoundaryUsingArrowKey(Settings.Default.MillisecondsToAdvanceSegmentBoundaryOnRightArrow);
					return true;

				case Keys.Left:
					OnAdjustBoundaryUsingArrowKey(-Settings.Default.MillisecondsToBackupSegmentBoundaryOnLeftArrow);
					return true;

				case Keys.Z:
					if ((ModifierKeys & Keys.Control) == 0 && _undoToolStripMenuItem.Enabled)
					{
						_undoToolStripMenuItem.PerformClick();
						return true;
					}
					break;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyUp(Keys key)
		{
			var result = base.OnLowLevelKeyUp(key);

			if (IsBoundaryMovingInProgressUsingArrowKeys)
			{
				FinalizeBoundaryMovedUsingArrowKeys();
				_timeAtBeginningOfBoundaryMove = TimeSpan.FromSeconds(1).Negate();
			}

			return result;
		}

		#endregion

		#region Methods for handling zoom
		/// ------------------------------------------------------------------------------------
		private void HandleZoomComboValidating(object sender, CancelEventArgs e)
		{
			SetZoom();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleZoomSelectedIndexChanged(object sender, EventArgs e)
		{
			SetZoom();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleZoomKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
				SetZoom();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void SetZoom()
		{
			var text = _comboBoxZoom.Text.Replace("%", string.Empty).Trim();
			float newValue;
			if (float.TryParse(text, out newValue))
				_waveControl.ZoomPercentage = newValue;

			_comboBoxZoom.Text = string.Format("{0}%", _waveControl.ZoomPercentage);
		}

		#endregion
	}
}
