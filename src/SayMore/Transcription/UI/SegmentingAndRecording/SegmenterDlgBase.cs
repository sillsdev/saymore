using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.UI;
using NAudio.Wave;
using SIL.Windows.Forms;
using SIL.Windows.Forms.Miscellaneous;
using SIL.Windows.Forms.PortableSettingsProvider;
using SayMore.Media.Audio;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.UI.LowLevelControls;
using SayMore.Media.MPlayer;
using SayMore.Utilities;
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
		protected TimeSpan _startTimeOfSegmentWhoseEndIsMoving = TimeSpan.FromSeconds(1).Negate();
		protected bool _moreReliableDesignMode;
		private WaveControlWithMovableBoundaries _waveControl;
		protected SegmentDefinitionMode _newSegmentDefinedBy;

		private Image _hotPlayInSegmentButton;
		private Image _normalPlayInSegmentButton;
		protected Size _playButtonSize = ResourceImageCache.ListenToSegment.Size;
		private Rectangle _lastPlayButtonRc;

		protected PercentageFormatter _pctFormatter = new PercentageFormatter();
		private bool _changeActiveControlOnEnter;
		private bool _inHandleZoomKeyDown;
		private bool _closeDlgRequested;


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

			_toolStripStatus.Renderer = new SIL.Windows.Forms.NoToolStripBorderRenderer();
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

			_normalPlayInSegmentButton = ResourceImageCache.ListenToSegment;
			_hotPlayInSegmentButton = PaintingHelper.MakeHotImage(_normalPlayInSegmentButton);

			_waveControl = CreateWaveControl();
			InitializeZoomCombo(); // For efficiency, do this before initializing the wave control.
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

			HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		public void InitializeWaveControl()
		{
			_waveControl.LoadFile(_viewModel.OrigWaveStream as WaveFileReader);
			_waveControl.SegmentBoundaries = _viewModel.GetSegmentEndBoundaries();
			_waveControl.Painter.SetIgnoredRegions(_viewModel.GetIgnoredSegmentRanges());
			_waveControl.PostPaintAction = HandleWaveControlPostPaint;
			_waveControl.PlaybackStarted += OnPlaybackStarted;
			_waveControl.PlaybackUpdate += OnPlayingback;
			_waveControl.PlaybackStopped += OnPlaybackStopped;
			_waveControl.MouseMove += HandleWaveControlMouseMove;
			_waveControl.MouseLeave += HandleWaveControlMouseLeave;
			_waveControl.MouseClick += HandleWaveControlMouseClick;
			_waveControl.MouseDown += (sender, args) => KillTimer();
			_waveControl.Controls.Add(_currentSegmentMenuStrip);
			_currentSegmentMenuStrip.UseWaitCursor = false;
			_waveControl.Controls.Add(_lastSegmentMenuStrip);
			_lastSegmentMenuStrip.UseWaitCursor = false;
			_waveControl.CanBoundaryBeMoved = CanBoundaryBeMoved;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				LocalizeItemDlg.StringsLocalized -= HandleStringsLocalized;

				if (components != null)
					components.Dispose();

				if (_waveControl != null)
				{
					_waveControl.PlaybackStarted -= OnPlaybackStarted;
					_waveControl.PlaybackUpdate -= OnPlayingback;
					_waveControl.PlaybackStopped -= OnPlaybackStopped;
					_waveControl.MouseMove -= HandleWaveControlMouseMove;
					_waveControl.MouseLeave -= HandleWaveControlMouseLeave;
					_waveControl.MouseClick -= HandleWaveControlMouseClick;
					_waveControl.FormatNotSupportedMessageProvider = null;
				}
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual WaveControlWithMovableBoundaries CreateWaveControl()
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleStringsLocalized()
		{
			_segmentXofYFormat = _labelSegmentXofY.Text;
			_segmentNumberFormat = _labelSegmentNumber.Text;
			var zoomToolTip = LocalizationManager.GetLocalizedToolTipForControl(_comboBoxZoom);
			if (!string.IsNullOrEmpty(zoomToolTip))
				_tooltip.SetToolTip(_labelZoom, zoomToolTip);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeZoomCombo()
		{
			_comboBoxZoom.Items.AddRange((new [] {100, 125, 150, 175, 200, 250, 300, 500, 750, 1000})
				.Select(_pctFormatter.Format).Cast<Object>().ToArray());

			_comboBoxZoom.Font = Program.DialogFont;
			ZoomPercentage = DefaultZoomPercentage;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual float DefaultZoomPercentage
		{
			get
			{
				double value;
				return _pctFormatter.Format(_comboBoxZoom.Items[0] as string, out value) != null ? (float)value : 100f;
			}
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
		protected virtual Rectangle PlayOrigButtonRectangle
		{
			get
			{
				return GetPlayOrigButtonRectangleForSegment(HotSegmentRectangle);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected Rectangle GetPlayOrigButtonRectangleForSegment(Rectangle rc)
		{
			if (rc.IsEmpty || _playButtonSize.Width + 6 > rc.Width)
				return Rectangle.Empty;

			return new Rectangle(rc.X + 6,
				rc.Bottom - MarginFromBottomOfPlayOrigButton - _playButtonSize.Height,
				_playButtonSize.Width, _playButtonSize.Height);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual int MarginFromBottomOfPlayOrigButton
		{
			get { return 5; }
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
		protected AnnotationSegment HotSegment
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

				return HotRectangleBeyondFinalSegment;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual Rectangle HotRectangleBeyondFinalSegment
		{
			get { return Rectangle.Empty; }
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
			get { return _waveControl.ZoomPercentage; }
			set
			{
				bool setZoomText = string.IsNullOrEmpty(_waveControl.Text);
				if (!_waveControl.ZoomPercentage.Equals(value))
				{
					_waveControl.ZoomPercentage = value;
					setZoomText = true;
				}
				if (setZoomText)
					SetZoomTextInComboBox();
			}
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

			// Cancel means the user closed the form using the X or Alt+F4. In that
			// case whether they want to save changes is ambiguous. So ask them.
			if (DialogResult == DialogResult.Cancel && _viewModel.WereChangesMade)
			{
				DialogResult = DialogResult.OK;

				var msg = LocalizationManager.GetString(
					"DialogBoxes.Transcription.SegmenterDlgBase.SaveChangesQuestion",
					"Would you like to save your changes?");

				DialogResult = MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (DialogResult == DialogResult.Cancel)
					return;
				DialogResult = DialogResult.OK;
			}

			if (_waveControl.IsPlaying)
			{
				// This is weird, but if we close the dialog while playing, OnPlayingBack gets called and
				// throws an ObjectDisposedException even though the dlg is only being destroyed and has not
				// yet been disposed. Stopping playback is asynchronous, so we just need to note that the user
				// wishes to close and wait for playback to stop.
				_closeDlgRequested = true;
				return;
			}

			e.Cancel = false;

			if (DialogResult == DialogResult.OK)
			{
				// Create final segment if necessary to complete segmentation if last segment break is within 5
				// seconds of the end of the recording.
				_viewModel.AddFinalSegmentIfAlmostComplete();
				_viewModel.CreateMissingTextSegmentsToMatchTimeSegmentCount();
			}

			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTableLayoutButtonsPaint(object sender, PaintEventArgs e)
		{
			using (var pen = new Pen(AppColors.BarBorder))
				e.Graphics.DrawLine(pen, 0, 0, _tableLayoutButtons.Width, 0);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool CanBoundaryBeMoved(TimeSpan boundary, bool disregardAnnotations)
		{
			return !_viewModel.IsBoundaryPermanent(boundary, disregardAnnotations);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnPlaybackStarted(WaveControlBasic ctrl, TimeSpan start, TimeSpan end)
		{
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnPlayingback(WaveControlBasic ctrl, TimeSpan current, TimeSpan total)
		{
			if (InvokeRequired)
			{
				Invoke(new Action(() => OnPlayingback(ctrl, current, total)));
			}
			else
			{
				_labelTimeDisplay.Text = MediaPlayerViewModel.GetTimeDisplay(
					(float) current.TotalSeconds, (float) total.TotalSeconds);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnPlaybackStopped(WaveControlBasic ctrl, TimeSpan start, TimeSpan end)
		{
			if (_closeDlgRequested)
				Close();
			else
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
		protected virtual void HandleWaveControlPostPaint(PaintEventArgs e)
		{
			_lastPlayButtonRc = PlayOrigButtonRectangle;
			DrawPlayButtonInSegment(e.Graphics, _lastPlayButtonRc);
		}

		/// ------------------------------------------------------------------------------------
		protected void DrawPlayButtonInSegment(Graphics g, Rectangle rc)
		{
			if (rc == Rectangle.Empty)
				return;

			var img = _normalPlayInSegmentButton;

			if (rc.Contains(MousePositionInWaveControl))
				img = _hotPlayInSegmentButton;

			g.DrawImage(img, rc);
		}

		private Point _lastMousePosition;
		private Point _lastWaveControlScrollPosition;
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleWaveControlMouseMove(object sender, MouseEventArgs e)
		{
			// This is utter insanity, but we're constantly getting mouse move events when the
			// mouse isn't moving, and the processing here is using all the CPU. If nothing
			// has changed, let's just get out of here.
			if (e.Location == _lastMousePosition && _waveControl.AutoScrollPosition == _lastWaveControlScrollPosition)
				return;
			_lastMousePosition = e.Location;
			_lastWaveControlScrollPosition = _waveControl.AutoScrollPosition;

			bool enableIgnoreMenu = false;
			var rcHotSegment = HotSegmentRectangle;
			if (rcHotSegment != Rectangle.Empty)
			{
				if (!_waveControl.IsPlaying)
				{
					enableIgnoreMenu = true;

					var currentSegmentMenuStripLocation = new Point(Math.Max(0, rcHotSegment.Right -
						_currentSegmentMenuStrip.Width - Math.Min(5, (rcHotSegment.Width - _currentSegmentMenuStrip.Width) / 2)),
						rcHotSegment.Top + 5);

					var hotSegment = HotSegment;

					// If the undo button is also showing for this same segment. we need to shift the
					// ignore to the left or down to allow it to fit without overlapping.
					if (UndoImplemented && _viewModel.TimeRangeForUndo != null &&
						hotSegment != null && _viewModel.TimeRangeForUndo == hotSegment.TimeRange)
					{

						if (rcHotSegment.Width > _lastSegmentMenuStrip.Width + _currentSegmentMenuStrip.Width + 15)
							currentSegmentMenuStripLocation.Offset(-(_lastSegmentMenuStrip.Width + 5), 0); // shift left
						else
							currentSegmentMenuStripLocation.Offset(0, _lastSegmentMenuStrip.Height + 5); // shift down
					}

					_currentSegmentMenuStrip.Location = currentSegmentMenuStripLocation;

					_ignoreToolStripMenuItem.Checked = hotSegment != null && _viewModel.GetIsSegmentIgnored(hotSegment);
				}
			}
			_currentSegmentMenuStrip.Visible = _ignoreToolStripMenuItem.Enabled = enableIgnoreMenu;
			var rcPlayOrigButton = PlayOrigButtonRectangle;
			if (rcPlayOrigButton != _lastPlayButtonRc)
				_waveControl.InvalidateIfNeeded(_lastPlayButtonRc);
			_waveControl.InvalidateIfNeeded(PlayOrigButtonRectangle);

			var toolTipText = GetWaveControlToolTip(e.Location);
			if (_tooltip.GetToolTip(_waveControl) != toolTipText)
				_tooltip.SetToolTip(_waveControl, toolTipText);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string GetWaveControlToolTip(Point mouseLocation)
		{
			var segMouseOver = _waveControl.GetSegmentForX(mouseLocation.X);

			if (segMouseOver >= 0)
			{
				if (PlayOrigButtonRectangle.Contains(mouseLocation))
				{
					return LocalizationManager.GetString(
						"DialogBoxes.Transcription.SegmenterDlgBase.PlayOriginalToolTipMsg",
						"Listen to this segment.");
				}
			}

			return string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlMouseLeave(object sender, EventArgs e)
		{
			_tooltip.SetToolTip(_waveControl, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleWaveControlMouseClick(object sender, MouseEventArgs e)
		{
			bool playSource = PlayOrigButtonRectangle.Contains(e.Location);
			if (!playSource)
				return;

			int iSegment = _waveControl.GetSegmentForX(e.X);

			if (iSegment >= 0)
			{
				var segment = _viewModel.GetSegment(iSegment);
				PlaySource(segment);
			}
			else
			{
				_waveControl.Play(_viewModel.TimeTier.EndOfLastSegment);
			}
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void PlaySource(AnnotationSegment segment)
		{
			_waveControl.Play(segment.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleIgnoreToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			_ignoreToolStripMenuItem.Image = _ignoreToolStripMenuItem.Checked ?
				ResourceImageCache.CheckedBox : ResourceImageCache.UncheckedBox;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleIgnoreButtonClick(object sender, EventArgs e)
		{
			var time = _waveControl.GetTimeFromX(((ToolStripMenuItem) sender).Owner.Location.X);
			var hotSegment = _viewModel.TimeTier.Segments.FirstOrDefault(s => s.TimeRange.Start <= time && s.TimeRange.End >= time);
			var ignore = _ignoreToolStripMenuItem.Checked;
			if (hotSegment == null)
			{
				if (ignore)
				{
					_viewModel.SetIgnoredFlagForSegment(null, ignore);
					hotSegment = _viewModel.TimeTier.Segments.Last();
					_waveControl.InvalidateIfNeeded(GetFullRectangleForTimeRange(hotSegment.TimeRange));
				}
				else
				{
#if DEBUG
					throw new Exception(
						"How can this be null? It has happened twice, and it makes no sense! Now using above logic instead of HotSegment.");
#else
					return;
#endif
				}
			}
			else
			{
				_viewModel.SetIgnoredFlagForSegment(hotSegment, ignore);
				_waveControl.InvalidateIfNeeded(HotSegmentRectangle);
			}
			if (ignore)
				_waveControl.Painter.AddIgnoredRegion(hotSegment.TimeRange);
			else
				_waveControl.Painter.RemoveIgnoredRegion(hotSegment.TimeRange.End);
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
			_startTimeOfSegmentWhoseEndIsMoving = TimeSpan.FromSeconds(1).Negate();
			if (i > 0)
			{
				_startTimeOfSegmentWhoseEndIsMoving = boundaries[i - 1];
				if (playbackStartTime < _startTimeOfSegmentWhoseEndIsMoving)
					playbackStartTime = _startTimeOfSegmentWhoseEndIsMoving;
			}

			_timer = new Timer();
			_timer.Interval = Settings.Default.MillisecondsToDelayPlaybackAfterAdjustingSegmentBoundary;
			_timer.Tick += delegate
			{
				if (!_waveControl.IsPlaying)
				{
					_waveControl.Play(playbackStartTime, boundary);
					_waveControl.PlaybackStopped -= PlaybackShortPortionUpToBoundary;
					_waveControl.PlaybackStopped += PlaybackShortPortionUpToBoundary;
				}

				KillTimer();
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
			KillTimer();
			_waveControl.Stop();
		}

		private void KillTimer()
		{
			if (_timer != null)
			{
				_timer.Stop();
				_timer.Dispose();
				_timer = null;
			}
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
					_lastSegmentMenuStrip.Location = new Point(Math.Max(0, x), 5);
					_undoToolStripMenuItem.ToolTipText = String.Format(LocalizationManager.GetString(
						"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.UndoToolTipMsg",
						"Undo: {0} (Ctrl-Z or Z)"), _viewModel.DescriptionForUndo);
				}
			}
			else
				_undoToolStripMenuItem.Enabled = false;
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
		protected bool ZoomComboIsActiveControl
		{
			get { return ActiveControl == _comboBoxZoom; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyDown(Keys key)
		{
			if (!ContainsFocus)
				return true;

			bool ctrlKeyPressed = (ModifierKeys & Keys.Control) != 0;

			if (!ctrlKeyPressed && ZoomComboIsActiveControl)
				return false;

			switch (key)
			{
				case Keys.Right:
					if (ActiveControl == _waveControl)
					{
						OnAdjustBoundaryUsingArrowKey(Settings.Default.MillisecondsToAdvanceSegmentBoundaryOnRightArrow);
						return true;
					}
					break;

				case Keys.Left:
					if (ActiveControl == _waveControl)
					{
						OnAdjustBoundaryUsingArrowKey(-Settings.Default.MillisecondsToBackupSegmentBoundaryOnLeftArrow);
						return true;
					}
					break;

				case Keys.Z: // For some reason, they want a plain Z to also function as an undo.
					if (!ctrlKeyPressed && _undoToolStripMenuItem.Enabled)
					{
						_undoToolStripMenuItem.PerformClick();
						return true;
					}
					break;

				case Keys.D1:
				case Keys.NumPad1:
					if (ctrlKeyPressed)
					{
						ZoomPercentage += 10;
						return true;
					}
					break;

				case Keys.D2:
				case Keys.NumPad2:
					if (ctrlKeyPressed)
					{
						ZoomPercentage = 100f;
						return true;
					}
					break;

				case Keys.D3:
				case Keys.NumPad3:
					if (ctrlKeyPressed)
					{
						ZoomPercentage -= 10;
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

			if (key == Keys.Enter && _changeActiveControlOnEnter)
			{
				if (ZoomComboIsActiveControl)
				{
					// By now, the new zoom value has been successfully applied, so activate the
					// wave control. If we keep the zoom control focused, then SPACE and ENTER
					// will get "eaten" by this control and won't behave as the user expects.
					ActiveControl = _waveControl;
				}
				_changeActiveControlOnEnter = false;
			}

			return result;
		}

		#endregion

		#region Methods for handling zoom
		/// ------------------------------------------------------------------------------------
		private void HandleZoomComboValidating(object sender, CancelEventArgs e)
		{
			e.Cancel = !SetZoom();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleZoomSelectedIndexChanged(object sender, EventArgs e)
		{
			if (_inHandleZoomKeyDown)
				return;
			SetZoom();
			// Can't make the wave control active if it hasn't yet been added to the form.
			if (!_comboBoxZoom.DroppedDown && _waveControl.Parent != null)
				ActiveControl = _waveControl;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleZoomKeyDown(object sender, KeyEventArgs e)
		{
			_inHandleZoomKeyDown = true;
			if (e.KeyCode == Keys.Enter)
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
				_changeActiveControlOnEnter = SetZoom();
			}
			_inHandleZoomKeyDown = false;
		}

		/// ------------------------------------------------------------------------------------
		private bool SetZoom()
		{
			double newValue;
			if (_pctFormatter.Format(_comboBoxZoom.Text, out newValue) != null)
			{
				ZoomPercentage = (float)newValue;
				return ZoomPercentage.Equals((float)newValue);
			}
			SetZoomTextInComboBox();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected void SetZoomTextInComboBox()
		{
			_comboBoxZoom.Text = _pctFormatter.Format(_waveControl.ZoomPercentage / 100f);
		}
		#endregion
	}
}
