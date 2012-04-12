using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Localization;
using Localization.UI;
using NAudio.Wave;
using Palaso.Progress;
using SayMore.Media;
using SayMore.Properties;
using SayMore.UI.LowLevelControls;
using SayMore.Media.UI;
using SayMore.UI.Utilities;
using SilTools;
using Timer = System.Windows.Forms.Timer;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class SegmenterDlgBase : MonitorKeyPressDlg
	{
		protected readonly SegmenterDlgBaseViewModel _viewModel;
		protected string _segmentNumberFormat;
		protected string _segmentXofYFormat;
		protected Timer _timer;
		protected TimeSpan _timeAtBeginningOfboundaryMove = TimeSpan.FromSeconds(1).Negate();
		protected bool _moreReliableDesignMode;
		private WaveControlBasic _waveControl;

		/// ------------------------------------------------------------------------------------
		public SegmenterDlgBase()
		{
			WaitCursor.Show();

			_moreReliableDesignMode = (DesignMode || GetService(typeof(IDesignerHost)) != null) ||
				(LicenseManager.UsageMode == LicenseUsageMode.Designtime);

			InitializeComponent();
			base.DoubleBuffered = true;

			_toolStripStatus.Renderer = new SilTools.NoToolStripBorderRenderer();
			_tableLayoutButtons.BackColor = Settings.Default.BarColorEnd;
			_panelWaveControl.BackColor = Settings.Default.BarColorBorder;

			_buttonCancel.Click += delegate { Close(); };
			_buttonOK.Click += delegate { Close(); };

			_segmentXofYFormat = _labelSegmentXofY.Text;
			_segmentNumberFormat = _labelSegmentNumber.Text;

			LocalizeItemDlg.StringsLocalized += delegate
			{
				_segmentXofYFormat = _labelSegmentXofY.Text;
				_segmentNumberFormat = _labelSegmentNumber.Text;
			};
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
			_tableLayoutOuter.RowStyles.Add(new RowStyle(SizeType.Absolute, GetHeightOfTableLayoutButtonRow()));
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
			_comboBoxZoom.Font = SystemFonts.MenuFont;
			_labelZoom.Font = SystemFonts.MenuFont;
			_labelSegmentXofY.Font = SystemFonts.MenuFont;
			_labelSegmentNumber.Font = SystemFonts.MenuFont;
			_labelTimeDisplay.Font = SystemFonts.MenuFont;
			_labelOriginalRecording.Font = FontHelper.MakeFont(SystemFonts.MenuFont, FontStyle.Bold);

			// If we ever get zooming working again, remove the following two
			// lines and uncomment the two below them.
			_labelZoom.Visible = false;
			_comboBoxZoom.Visible = false;
			//_waveControl.ZoomPercentage = 300; //ZoomPercentage;
			//_comboBoxZoom.Text = string.Format("{0}%", ZoomPercentage);

			HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
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

		/// ------------------------------------------------------------------------------------
		protected string GetSegmentTooShortText()
		{
			return LocalizationManager.GetString(
				"DialogBoxes.Transcription.SegmenterDlgBase.ButtonTextWhenSegmentTooShort",
				"Whoops! The segment will be too short. Continue playing.");
		}

		/// ------------------------------------------------------------------------------------
		public void InitializeWaveControl()
		{
			_waveControl.Initialize(_viewModel.OrigWaveStream as WaveFileReader);
			_waveControl.SegmentBoundaries = _viewModel.GetSegmentEndBoundaries();
			_waveControl.PlaybackStarted += OnPlaybackStarted;
			_waveControl.PlaybackUpdate += OnPlayingback;
			_waveControl.PlaybackStopped += OnPlaybackStopped;
			if (_waveControl is WaveControlWithMovableBoundaries)
			{
				((WaveControlWithMovableBoundaries)_waveControl).InitiatiatingBoundaryMove += (sender, e) =>
				{
					e.Cancel = _viewModel.IsBoundaryPermanent(e.BoundaryBeingMoved);
				};
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual int GetHeightOfTableLayoutButtonRow()
		{
			return 0;
		}

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
			}

			e.Cancel = false;

			if (DialogResult == DialogResult.OK || DialogResult == DialogResult.Yes)
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

			if (boundary == TimeSpan.Zero || _viewModel.IsBoundaryPermanent(boundary) ||
				!_viewModel.CanMoveBoundary(boundary, milliseconds))
			{
				return false;
			}

			if (_timeAtBeginningOfboundaryMove <= TimeSpan.Zero)
				_timeAtBeginningOfboundaryMove = boundary;

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void FinalizeBoundaryMovedUsingArrowKeys()
		{
			_waveControl.SegmentBoundaries = _viewModel.GetSegmentEndBoundaries();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual TimeSpan GetBoundaryToAdjustOnArrowKeys()
		{
			return TimeSpan.Zero;
		}

		/// ------------------------------------------------------------------------------------
		public bool HandleSegmentBoundaryMovedInWaveControl(WaveControlWithMovableBoundaries waveCtrl,
			TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			return OnSegmentBoundaryMovedInWaveControl(oldEndTime, newEndTime);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool OnSegmentBoundaryMovedInWaveControl(TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			StopAllMedia();
			var segMoved = (_viewModel.SegmentBoundaryMoved(oldEndTime, newEndTime));
			_waveControl.SegmentBoundaries = _viewModel.GetSegmentEndBoundaries();
			UpdateDisplay();

			if (segMoved)
				PlaybackShortPortionUpToBoundary(newEndTime);

			return segMoved;
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
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyUp(Keys key)
		{
			var result = base.OnLowLevelKeyUp(key);

			if (_timeAtBeginningOfboundaryMove >= TimeSpan.Zero)
			{
				FinalizeBoundaryMovedUsingArrowKeys();
				_timeAtBeginningOfboundaryMove = TimeSpan.FromSeconds(1).Negate();
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
