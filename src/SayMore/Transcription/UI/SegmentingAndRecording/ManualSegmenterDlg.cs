using System;
using System.Windows.Forms;
using SayMore.AudioUtils;
using SayMore.Properties;
using SayMore.Transcription.UI.SegmentingAndRecording;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class ManualSegmenterDlg : SegmenterDlgBase
	{
		private readonly string _origAddSegBoundaryButtonText;
		private WaveControlWithBoundarySelection _waveControl;

		/// ------------------------------------------------------------------------------------
		public ManualSegmenterDlg(ManualSegmenterDlgViewModel viewModel) : base(viewModel)
		{
			InitializeComponent();

			Controls.Remove(toolStripButtons);
			_tableLayoutOuter.Controls.Add(toolStripButtons);

			_origAddSegBoundaryButtonText = _buttonAddSegmentBoundary.Text;

			_buttonListenToOriginal.Click += delegate
			{
				if (!_waveControl.IsPlaying)
					_waveControl.Play(_waveControl.GetCursorTime());
			};

			_buttonStopOriginal.Click += delegate { _waveControl.Stop(); };

			_buttonAddSegmentBoundary.Click += delegate
			{
				//if (_viewModel.GetIsSegmentLongEnough(_waveControl.GetCursorTime()))
					_waveControl.SegmentBoundaries = ViewModel.SaveNewBoundary(_waveControl.GetCursorTime());
					_waveControl.SetSelectedBoundary(_waveControl.GetCursorTime());
					_waveControl.SetCursor(TimeSpan.Zero);
				//else
				//{
				//	_buttonAddSegmentBoundary.ForeColor = Color.Red;
				//	_buttonAddSegmentBoundary.Text = GetSegmentTooShortText();
				//}
			};

			_buttonDeleteSegment.Click += delegate
			{
				var boundary = _waveControl.GetSelectedBoundary();
				_waveControl.ClearSelectedBoundary();
				ViewModel.DeleteBoundary(boundary);
				UpdateDisplay();
			};

			_waveControl.BoundaryMoved += HandleSegmentBoundaryMoved;
			_waveControl.BoundaryMouseDown += delegate { UpdateDisplay(); };
			_waveControl.CursorTimeChanged += delegate { UpdateDisplay(); };
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private ManualSegmenterDlgViewModel ViewModel
		{
			get { return _viewModel as ManualSegmenterDlgViewModel; }
		}

		/// ------------------------------------------------------------------------------------
		protected override WaveControlBasic CreateWaveControl()
		{
			_waveControl = new WaveControlWithBoundarySelection();
			return _waveControl;
		}

		/// ------------------------------------------------------------------------------------
		protected override FormSettings FormSettings
		{
			get { return Settings.Default.ManualSegmenterDlg; }
			set { Settings.Default.ManualSegmenterDlg = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override float ZoomPercentage
		{
			get { return Settings.Default.ZoomPercentageInManualSegmenterDlg; }
			set { Settings.Default.ZoomPercentageInManualSegmenterDlg = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ShouldShadePlaybackAreaDuringPlayback
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int GetHeightOfTableLayoutButtonRow()
		{
			return (_buttonListenToOriginal.Height * 3) + 5 +
				_buttonListenToOriginal.Margin.Top + _buttonListenToOriginal.Margin.Bottom +
				_buttonAddSegmentBoundary.Margin.Top + _buttonAddSegmentBoundary.Margin.Bottom +
				_buttonDeleteSegment.Margin.Top + _buttonDeleteSegment.Margin.Bottom +
				toolStripButtons.Margin.Top + toolStripButtons.Margin.Bottom;
		}

		/// ------------------------------------------------------------------------------------
		protected override void UpdateDisplay()
		{
			_buttonListenToOriginal.Visible = !_waveControl.IsPlaying;
			_buttonStopOriginal.Visible = _waveControl.IsPlaying;

			_buttonAddSegmentBoundary.ForeColor = _buttonListenToOriginal.ForeColor;
			_buttonAddSegmentBoundary.Text = _origAddSegBoundaryButtonText;
			_buttonAddSegmentBoundary.Enabled = _waveControl.GetCursorTime() > TimeSpan.Zero;
			_buttonDeleteSegment.Enabled = _waveControl.GetSelectedBoundary() > TimeSpan.Zero;

			base.UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected override TimeSpan GetCurrentTimeForTimeDisplay()
		{
			return (_waveControl.GetCursorTime() == TimeSpan.Zero ?
				_waveControl.GetSelectedBoundary() : base.GetCurrentTimeForTimeDisplay());
		}

		/// ------------------------------------------------------------------------------------
		protected override TimeSpan GetSubSegmentReplayEndTime()
		{
			return _waveControl.GetSelectedBoundary();
		}

		/// ------------------------------------------------------------------------------------
		protected override TimeSpan GetBoundaryToAdjustOnArrowKeys()
		{
			return _waveControl.GetSelectedBoundary();
		}

		/// ------------------------------------------------------------------------------------
		protected override void PlaybackShortPortionUpToBoundary(WaveControlBasic ctrl,
			TimeSpan time1, TimeSpan time2)
		{
			base.PlaybackShortPortionUpToBoundary(ctrl, time1, time2);
			_waveControl.SetCursor(TimeSpan.Zero);
		}

		#region Low level keyboard handling
		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyDown(Keys key)
		{
			if (!ContainsFocus)
				return true;

			if (key == Keys.Delete)
				_buttonDeleteSegment.PerformClick();
			else if (key == Keys.Space)
			{
				if (_waveControl.IsPlaying)
					_buttonStopOriginal.PerformClick();
				else
					_buttonListenToOriginal.PerformClick();
			}

			return base.OnLowLevelKeyDown(key);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyUp(Keys key)
		{
			if (!ContainsFocus)
				return true;

			if (key == Keys.Space)
				return false;

			if (key == Keys.Enter)
			{
				_buttonAddSegmentBoundary.PerformClick();
				return true;
			}

			return base.OnLowLevelKeyUp(key);
		}

		#endregion
	}
}
