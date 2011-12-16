using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using Palaso.Progress;
using SayMore.AudioUtils;
using SayMore.Properties;
using SayMore.UI.LowLevelControls;
using SayMore.UI.MediaPlayer;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class SegmenterDlgBase : MonitorKeyPressDlg
	{
		private readonly string _segmentXofYFormat;
		private readonly string _segmentCountFormat;
		private Timer _timer;
		protected readonly SegmenterDlgBaseViewModel _viewModel;
		protected bool _moreReliableDesignMode;

		/// ------------------------------------------------------------------------------------
		public SegmenterDlgBase()
		{
			WaitCursor.Show();

			_moreReliableDesignMode = (DesignMode || GetService(typeof(IDesignerHost)) != null) ||
				(LicenseManager.UsageMode == LicenseUsageMode.Designtime);

			Opacity = 0f;

			InitializeComponent();

			DialogResult = DialogResult.OK;
			DoubleBuffered = true;
			_segmentCountFormat = _labelSegmentCount.Text;
			_segmentXofYFormat = _labelSegment.Text;
			_comboBoxZoom.Text = _comboBoxZoom.Items[0] as string;
			_comboBoxZoom.Font = SystemFonts.IconTitleFont;
			_labelZoom.Font = SystemFonts.IconTitleFont;
			_labelSegmentCount.Font = SystemFonts.IconTitleFont;
			_labelSegment.Font = SystemFonts.IconTitleFont;
			_labelTimeDisplay.Font = SystemFonts.IconTitleFont;
			_labelOriginalRecording.Font = FontHelper.MakeFont(SystemFonts.IconTitleFont, FontStyle.Bold);

			_buttonCancel.Click += delegate { Close(); };
			_buttonOK.Click += delegate { Close(); };
		}

		/// ------------------------------------------------------------------------------------
		public SegmenterDlgBase(SegmenterDlgBaseViewModel viewModel) : this()
		{
			_viewModel = viewModel;
			_viewModel.UpdateDisplayProvider = UpdateDisplay;

			if (!_moreReliableDesignMode && FormSettings == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				FormSettings = FormSettings.Create(this);
			}

			InitializeWaveControl();

			_labelTimeDisplay.Text = MediaPlayerViewModel.GetTimeDisplay(0f,
				(float)_viewModel.OrigWaveStream.TotalTime.TotalSeconds);

			_buttonListenToOriginal.MouseUp += delegate { _waveControl.Stop(); };

			_viewModel.SegmentNumberChangedHandler = () =>
			{
				_waveControl.Stop();
				_waveControl.ShadePlaybackAreaDuringPlayback = _viewModel.IsCurrentSegmentConfirmed;
				UpdateDisplay();
			};
		}

		/// ------------------------------------------------------------------------------------
		public void InitializeWaveControl()
		{
			_waveControl.Initialize(_viewModel.OrigWaveStream);
			_waveControl.SegmentBoundaries = _viewModel.GetSegmentBoundaries();

			_waveControl.PlaybackStarted = () =>
			{
				_viewModel.PlaybackOfOriginalRecordingStarted();
				UpdateDisplay();
				_waveControl.PlaybackUpdate = (position, totalTime) =>
				{
					_labelTimeDisplay.Text = MediaPlayerViewModel.GetTimeDisplay(
						(float)position.TotalSeconds, (float)totalTime.TotalSeconds);
				};
			};

			_waveControl.Stopped = (start, end) =>
			{
				_waveControl.PlaybackUpdate = null;
				var newCursorPosition = _viewModel.PlaybackOfOriginalRecordingStopped(start, end);
				_waveControl.SetCursor(newCursorPosition);
				UpdateDisplay();
			};

			_waveControl.SegmentClicked += HandleWaveControlSegmentClicked;
			_waveControl.SegmentBoundaryMoved += _viewModel.HandleSegmentBoundaryMoved;
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

			_waveControl.ZoomPercentage = ZoomPercentage;
			_comboBoxZoom.Text = string.Format("{0}%", ZoomPercentage);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual int GetHeightOfTableLayoutButtonRow()
		{
			throw new NotImplementedException();
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
		protected override void OnShown(EventArgs e)
		{
			if (!_moreReliableDesignMode)
			{
				FormSettings.InitializeForm(this);
				_viewModel.SelectSegment(0);
			}

			base.OnShown(e);
			Opacity = 1f;
			WaitCursor.Hide();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			StopAllMedia();

			if (!_moreReliableDesignMode)
				ZoomPercentage = _waveControl.ZoomPercentage;

			if (DialogResult != DialogResult.Cancel)
				DialogResult = (_viewModel.WereChangesMade ? DialogResult.OK : DialogResult.Cancel);

			base.OnFormClosing(e);
		}

		#region Methods for adjusting/saving/playing within segment boundaries
		/// ------------------------------------------------------------------------------------
		private void AdjustSegmentEndBoundary(int milliseconds)
		{
			if (_timer != null)
			{
				_timer.Stop();
				_timer.Dispose();
				_timer = null;
			}

			if (_viewModel.IsCurrentSegmentConfirmed)
				return;

			StopAllMedia();

			if (!_viewModel.MoveExistingSegmentBoundary(milliseconds))
				return;

			UpdateDisplay();

			_timer = new Timer();
			_timer.Interval = Settings.Default.MillisecondsToDelayPlaybackAfterAdjustingSegmentBoundary;
			_timer.Tick += delegate
			{
				if (_viewModel.IsIdle && !_waveControl.IsPlaying)
					ReplaySegmentPortionAfterAdjustingEndBoundary();

				_timer.Stop();
				_timer.Dispose();
				_timer = null;
			};

			_timer.Start();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void StopAllMedia()
		{
			_waveControl.Stop();
		}

		/// ------------------------------------------------------------------------------------
		private void ReplaySegmentPortionAfterAdjustingEndBoundary()
		{
			if (!_viewModel.IsCurrentSegmentConfirmed)
			{
				_waveControl.ShadePlaybackAreaDuringPlayback = false;
				var subSegStartPosition = _viewModel.GetStartPositionOfSubSegmentAtEndOfCurrentSegment();
				if (subSegStartPosition < TimeSpan.MaxValue)
					_waveControl.Play(subSegStartPosition, _viewModel.PlaybackEndPosition);
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void HandleListenToOriginalMouseDown(object sender, MouseEventArgs e)
		{
			if (_viewModel.IsIdle)
			{
				_waveControl.ShadePlaybackAreaDuringPlayback = !_viewModel.IsCurrentSegmentConfirmed;
				_waveControl.Play(_viewModel.PlaybackStartPosition, _viewModel.PlaybackEndPosition);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void HandleWaveControlSegmentClicked(WaveControl ctrl, int segmentNumber)
		{
			_viewModel.SelectSegment(segmentNumber);
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void UpdateDisplay()
		{
			_buttonListenToOriginal.Checked = _waveControl.IsPlaying || !_viewModel.HaveSegmentBoundaries;

			if (_viewModel.IsCurrentSegmentConfirmed)
			{
				_labelSegment.Visible = true;
				_labelSegmentCount.Visible = false;
				_labelSegment.Text = string.Format(_segmentXofYFormat,
					_viewModel.CurrentSegmentNumber + 1, _viewModel.GetSegmentCount());
			}
			else
			{
				_labelSegment.Visible = false;
				_labelSegmentCount.Visible = true;
				_labelSegmentCount.Text = string.Format(_segmentCountFormat, _viewModel.GetSegmentCount());
			}

			_labelTimeDisplay.Text = MediaPlayerViewModel.GetTimeDisplay(
				(float)_waveControl.GetCursorTime().TotalSeconds,
				(float)_viewModel.OrigWaveStream.TotalTime.TotalSeconds);

			if (_viewModel.IsIdle)
			{
				_waveControl.SetSelectionTimes(_viewModel.PlaybackStartPosition, _viewModel.PlaybackEndPosition);
				_waveControl.SetCursor(_viewModel.IsCurrentSegmentConfirmed ?
					_viewModel.PlaybackStartPosition : _viewModel.PlaybackEndPosition);
			}
		}

		#region Low level keyboard handling
		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyDown(Keys key)
		{
			// Check that SHIFT is not down too, because Ctrl+Shift on a UI item brings up
			// the localization dialog box. We don't want it to also start playback.
			if (key == Keys.ControlKey && (ModifierKeys & Keys.Shift) != Keys.Shift)
			{
				HandleListenToOriginalMouseDown(null, null);
				return true;
			}

			if (!_viewModel.HaveSegmentBoundaries || _waveControl.IsPlaying)
				return false;

			switch (key)
			{
				case Keys.Right:
					AdjustSegmentEndBoundary(Settings.Default.MillisecondsToAdvanceSegmentBoundaryOnRightArrow);
					return true;

				case Keys.Left:
					AdjustSegmentEndBoundary(-Settings.Default.MillisecondsToBackupSegmentBoundaryOnLeftArrow);
					return true;

				case Keys.End:
				case Keys.Escape:
					_viewModel.GotoEndOfSegments();
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyUp(Keys key)
		{
			if (key == Keys.ControlKey)
			{
				_waveControl.Stop();
				_waveControl.ShadePlaybackAreaDuringPlayback = false;
				return true;
			}

			return false;
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
