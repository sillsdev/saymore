using System;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using SayMore.AudioUtils;
using SayMore.Properties;
using SayMore.UI.LowLevelControls;
using SayMore.UI.MediaPlayer;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class ManualSegmenterDlg : MonitorKeyPressDlg
	{
		private readonly string _normalRecordButtonText;
		private readonly string _segmentXofYFormat;
		private readonly string _segmentCountFormat;
		private readonly ManualSegmenterDlgViewModel _viewModel;
		private readonly ToolTip _tooltip = new ToolTip();
		private Timer _timer;

		/// ------------------------------------------------------------------------------------
		public ManualSegmenterDlg()
		{
			InitializeComponent();

			DialogResult = DialogResult.OK;
			DoubleBuffered = true;
			_normalRecordButtonText = _buttonRecordAnnotation.Text;
			_segmentCountFormat = _labelSegmentCount.Text;
			_segmentXofYFormat = _labelSegment.Text;
			_comboBoxZoom.Text = _comboBoxZoom.Items[0] as string;
			_comboBoxZoom.Font = SystemFonts.IconTitleFont;
			_labelZoom.Font = SystemFonts.IconTitleFont;
			_labelSegmentCount.Font = SystemFonts.IconTitleFont;
			_labelSegment.Font = SystemFonts.IconTitleFont;
			_labelTimeDisplay.Font = SystemFonts.IconTitleFont;
			_labelOriginalRecording.Font = FontHelper.MakeFont(SystemFonts.IconTitleFont, FontStyle.Bold);

			//_labelRecordingFormat.Font = SystemFonts.IconTitleFont;

			//var bestFormat = WaveFileUtils.GetDefaultWaveFormat(1);

			//_labelRecordingFormat.Text = string.Format(_labelRecordingFormat.Text,
			//    bestFormat.BitsPerSample, bestFormat.SampleRate);

			if (Settings.Default.OralAnnotationDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.OralAnnotationDlg = FormSettings.Create(this);
			}

			_buttonCancel.Click += delegate { Close(); };
			_buttonClose.Click += delegate { Close(); };

			var buttonRowHeight = _buttonListenToOriginal.Height + _buttonListenToOriginal.Margin.Top +
				_buttonListenToOriginal.Margin.Bottom + _buttonListenToAnnotation.Height +
				_buttonListenToAnnotation.Margin.Top + _buttonListenToAnnotation.Margin.Bottom +
				_buttonEraseAnnotation.Height + _buttonEraseAnnotation.Margin.Top +
				_buttonEraseAnnotation.Margin.Bottom + 3;

			_tableLayoutOuter.RowStyles.Clear();
			_tableLayoutOuter.RowCount = 4;
			_tableLayoutOuter.RowStyles.Add(new RowStyle());
			_tableLayoutOuter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			_tableLayoutOuter.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonRowHeight));
			_tableLayoutOuter.RowStyles.Add(new RowStyle());
		}

		/// ------------------------------------------------------------------------------------
		public ManualSegmenterDlg(ManualSegmenterDlgViewModel viewModel) : this()
		{
			_viewModel = viewModel;
			_viewModel.UpdateDisplayProvider = UpdateDisplay;

			InitializeWaveControl();

			_labelTimeDisplay.Text = MediaPlayerViewModel.GetTimeDisplay(0f,
				(float)_viewModel.OrigWaveStream.TotalTime.TotalSeconds);

			_buttonListenToOriginal.MouseUp += delegate { _waveControl.Stop(); };
			_buttonListenToAnnotation.Click += delegate { _viewModel.StartAnnotationPlayback(); };

			_buttonEraseAnnotation.Click += delegate
			{
				_waveControl.Stop();
				_viewModel.EraseAnnotation();
				UpdateDisplay();
			};

			_viewModel.SegmentNumberChangedHandler = () =>
			{
				_waveControl.Stop();
				_waveControl.ShadePlaybackAreaDuringPlayback = _viewModel.IsCurrentSegmentConfirmed;
				UpdateDisplay();
			};

			_viewModel.SelectSegment(0);
			_comboBoxZoom.Text = string.Format("{0}%", _waveControl.ZoomPercentage);
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

			_waveControl.Paint += HandleWaveControlPaint;
			_waveControl.MouseMove += HandleWaveControlMouseMove;
			_waveControl.MouseLeave += HandleWaveControlMouseLeave;
			_waveControl.SegmentClicked += HandleWaveControlSegmentClicked;
			_waveControl.SegmentBoundaryMoved += _viewModel.HandleSegmentBoundaryMoved;

			_waveControl.ZoomPercentage = Settings.Default.ZoomPercentageInCarefulSpeechRecordingDlg;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			Settings.Default.OralAnnotationDlg.InitializeForm(this);
			base.OnShown(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_waveControl.Stop();
			_viewModel.CloseAnnotationPlayer();
			_viewModel.CloseAnnotationRecorder();

			Settings.Default.ZoomPercentageInCarefulSpeechRecordingDlg = _waveControl.ZoomPercentage;

			if (DialogResult != DialogResult.Cancel)
			{
				DialogResult = (_viewModel.HaveSegmentBoundaries && (_viewModel.SegmentBoundariesChanged ||
					_viewModel.AnnotationRecordingsChanged) ? DialogResult.OK : DialogResult.Cancel);
			}

			base.OnFormClosing(e);
		}

		#region Methods for showing info. about whether or not a segment has a recorded annotation
		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlMouseMove(object sender, MouseEventArgs e)
		{
			var segMouseOver = _waveControl.GetSegmentForX(e.X);

			if (segMouseOver >= 0 && !_viewModel.GetDoesSegmentHaveAnnotationFile(segMouseOver))
			{
				if (_tooltip.GetToolTip(_waveControl) == string.Empty)
				{
					_tooltip.SetToolTip(_waveControl, LocalizationManager.GetString(
						"DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.NoAnnotationToolTipMsg",
						"This segment does not have a recorded annotaton."));
				}

				return;
			}

			if (_tooltip.GetToolTip(_waveControl) != string.Empty)
				_tooltip.SetToolTip(_waveControl, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlMouseLeave(object sender, EventArgs e)
		{
			_tooltip.SetToolTip(_waveControl, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlPaint(object sender, PaintEventArgs e)
		{
			var img = Resources.MissingAnnotation;

			int i = 0;
			foreach (var segRect in _waveControl.GetSegmentRectangles())
			{
				if (!segRect.IsEmpty && !_viewModel.GetDoesSegmentHaveAnnotationFile(i++))
				{
					var rc = new Rectangle(new Point(segRect.X + 1, segRect.Y + 1), img.Size);
					if (segRect.Contains(rc))
						e.Graphics.DrawImage(img, rc);
				}
			}
		}

		#endregion

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

			_viewModel.StopAnnotationPlayback();

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

		#region Listen/Erase/Record button handling
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
		private void HandleRecordAnnotationMouseDown(object sender, MouseEventArgs e)
		{
			if (_buttonRecordAnnotation.Enabled && _viewModel.BeginAnnotationRecording())
			{
				_buttonRecordAnnotation.Text = LocalizationManager.GetString(
					"DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.RecordingButtonText.WhenRecording",
					"Recording...");
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordAnnotationMouseUp(object sender, MouseEventArgs e)
		{
			if (!_buttonRecordAnnotation.Enabled && !_viewModel.GetIsRecording())
				return;

			if (_viewModel.StopAnnotationRecording())
			{
				_buttonRecordAnnotation.ForeColor = _buttonEraseAnnotation.ForeColor;
				_buttonRecordAnnotation.Text = _normalRecordButtonText;

				if (!_viewModel.IsCurrentSegmentConfirmed)
					_waveControl.SegmentBoundaries = _viewModel.SaveNewSegmentBoundary();
			}
			else
			{
				_buttonRecordAnnotation.ForeColor = Color.Red;
				_buttonRecordAnnotation.Text = LocalizationManager.GetString(
					"DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.RecordingButtonText.WhenRecordingTooShort",
					"Whoops. You need to hold down the SPACE bar or mouse button while talking.");
				_buttonEraseAnnotation.PerformClick();
			}

			UpdateDisplay();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public void HandleWaveControlSegmentClicked(WaveControl ctrl, int segmentNumber)
		{
			_viewModel.SelectSegment(segmentNumber);
			UpdateDisplay();
		}

		///// ------------------------------------------------------------------------------------
		//public void _HandleWaveControlMouseDown(object sender, MouseEventArgs e)
		//{
		//    if (!_waveControl.IsSegmentMovingInProgress)
		//    {
		//        _viewModel.SelectSegmentFromTime(_waveControl.GetTimeFromX(e.X));
		//        UpdateDisplay();
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_buttonListenToOriginal.Checked = _waveControl.IsPlaying || !_viewModel.HaveSegmentBoundaries;

			_buttonListenToAnnotation.Visible = (_viewModel.DoesAnnotationExistForCurrentSegment && !_viewModel.GetIsRecording());
			_buttonListenToAnnotation.Enabled = _viewModel.IsIdle && _viewModel.DoesAnnotationExistForCurrentSegment;

			_buttonRecordAnnotation.Checked = _viewModel.HaveSegmentBoundaries;
			_buttonRecordAnnotation.Visible = (!_viewModel.DoesAnnotationExistForCurrentSegment || _viewModel.GetIsRecording());
			_buttonRecordAnnotation.Enabled = !_viewModel.DoesAnnotationExistForCurrentSegment &&
				_viewModel.HaveSegmentBoundaries && (_viewModel.IsIdle || _viewModel.GetIsRecording());

			_buttonEraseAnnotation.Visible = (_viewModel.DoesAnnotationExistForCurrentSegment && !_viewModel.GetIsRecording());
			_buttonEraseAnnotation.Enabled = _viewModel.IsIdle && _viewModel.DoesAnnotationExistForCurrentSegment;

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
			if (key == Keys.ControlKey && (key & Keys.ShiftKey) != Keys.ShiftKey)
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

				//case Keys.Return:
				//    _waveControl.SegmentBoundaries = _viewModel.MarkSegmentBoundary();
				//    return true;

				case Keys.End:
				case Keys.Escape:
					_viewModel.GotoEndOfSegments();
					return true;

				case Keys.Space:
					HandleRecordAnnotationMouseDown(null, null);
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

			//if (key == Keys.Right || key == Keys.Left)
			//{
			//    ReplaySegmentPortionAfterAdjustingEndBoundary();
			//    return true;
			//}

			if (key == Keys.Space)
			{
				HandleRecordAnnotationMouseUp(null, null);
				return true;
			}

			return false;
		}

		#endregion

		#region Methods for handling zoom
		/// ------------------------------------------------------------------------------------
		private void HandleZoomComboValidating(object sender, System.ComponentModel.CancelEventArgs e)
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
