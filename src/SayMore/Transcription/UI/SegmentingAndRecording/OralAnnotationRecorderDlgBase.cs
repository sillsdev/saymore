using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Localization;
using SayMore.Media;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationRecorderBaseDlg : SegmenterDlgBase
	{
		private readonly string _normalRecordButtonText;
		private readonly ToolTip _tooltip = new ToolTip();
		private TimeSpan _currentMovingBoundaryTime;

		protected WaveControlWithRangeSelection _waveControl;

		/// ------------------------------------------------------------------------------------
		public static OralAnnotationRecorderBaseDlg Create(
			OralAnnotationRecorderDlgViewModel viewModel, OralAnnotationType annotationType)
		{
			return (annotationType == OralAnnotationType.Careful ?
				new CarefulSpeechRecorderDlg(viewModel) as OralAnnotationRecorderBaseDlg :
				new OralTranslationRecorderDlg(viewModel));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This constructor is only for the designer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public OralAnnotationRecorderBaseDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		protected OralAnnotationRecorderBaseDlg(OralAnnotationRecorderDlgViewModel viewModel)
			: base(viewModel)
		{
			InitializeComponent();

			_normalRecordButtonText = _buttonRecordAnnotation.Text;

			Controls.Remove(toolStripButtons);
			_tableLayoutOuter.Controls.Add(toolStripButtons);

			_buttonRecordAnnotation.MouseDown += HandleRecordAnnotationMouseDown;
			_buttonRecordAnnotation.MouseUp += HandleRecordAnnotationMouseUp;

			_buttonListenToOriginal.MouseUp += delegate { _waveControl.Stop(); };
			_buttonListenToAnnotation.Click += delegate
			{
				ViewModel.StartAnnotationPlayback();
				UpdateDisplay();
			};

			_buttonEraseAnnotation.Click += delegate
			{
				_waveControl.Stop();
				ViewModel.EraseAnnotation();
				_waveControl.InvalidateSelectedRegion();
				UpdateDisplay();
			};

			_waveControl.BoundaryMoved += HandleSegmentBoundaryMovedInWaveControl;
		}

		/// ------------------------------------------------------------------------------------
		protected override WaveControlBasic CreateWaveControl()
		{
			_waveControl = new 	WaveControlWithRangeSelection();
			_waveControl.Paint += HandleWaveControlPaint;
			_waveControl.MouseMove += HandleWaveControlMouseMove;
			_waveControl.MouseLeave += HandleWaveControlMouseLeave;
			_waveControl.SelectedRegionChanged += HandleWaveControlSelectedRegionChanged;

			_waveControl.BoundaryMouseDown += (ctrl, dx, boundary, boundaryNumber) =>
			{
				_waveControl.Stop();
				ViewModel.SelectSegmentFromTime(boundary);
				_waveControl.SetSelectionTimes(ViewModel.GetStartOfCurrentSegment(), ViewModel.GetEndOfCurrentSegment());
				UpdateDisplay();
			};

			_waveControl.SetCurorAtTimeOnMouseClick += (ctrl, desiredTime) =>
			{
				if (_waveControl.IsPlaying)
					return desiredTime;

				var lastSegmentEndTime = ViewModel.GetEndOfLastSegment();
				if (desiredTime < lastSegmentEndTime || _waveControl.GetIsMouseOverBoundary())
					return TimeSpan.Zero;

				_waveControl.ClearSelection();
				UpdateDisplay();
				return lastSegmentEndTime;
			};

			return _waveControl;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (ViewModel.TimeTier.Segments.Count <= 0)
				return;

			var start = TimeSpan.FromSeconds(ViewModel.TimeTier.Segments[0].Start);
			var end = TimeSpan.FromSeconds(ViewModel.TimeTier.Segments[0].End);
			ViewModel.SelectSegmentFromTime(end);
			_waveControl.SetSelectionTimes(start, end);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlSelectedRegionChanged(WaveControlWithRangeSelection ctrl,
			TimeSpan start, TimeSpan end)
		{
			if (!_waveControl.IsPlaying)
				ViewModel.SelectSegmentFromTime(start == end ? TimeSpan.FromSeconds(1).Negate() : end);
		}

		/// ------------------------------------------------------------------------------------
		private OralAnnotationRecorderDlgViewModel ViewModel
		{
			get { return _viewModel as OralAnnotationRecorderDlgViewModel; }
		}

		/// ------------------------------------------------------------------------------------
		protected override FormSettings FormSettings
		{
			get { return Settings.Default.OralAnnotationRecorderDlg; }
			set { Settings.Default.OralAnnotationRecorderDlg = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override float ZoomPercentage
		{
			get { return Settings.Default.ZoomPercentageInAnnotationRecordingDlg; }
			set { Settings.Default.ZoomPercentageInAnnotationRecordingDlg = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int GetHeightOfTableLayoutButtonRow()
		{
			return (_buttonListenToOriginal.Height * 3) + 5 +
				_buttonListenToOriginal.Margin.Top + _buttonListenToOriginal.Margin.Bottom +
				_buttonListenToAnnotation.Margin.Top + _buttonListenToAnnotation.Margin.Bottom +
				_buttonEraseAnnotation.Margin.Top + _buttonEraseAnnotation.Margin.Bottom +
				toolStripButtons.Margin.Top + toolStripButtons.Margin.Bottom;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			ViewModel.CloseAnnotationPlayer();
			ViewModel.CloseAnnotationRecorder();
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void StopAllMedia()
		{
			ViewModel.StopAnnotationPlayback();
			base.StopAllMedia();
		}

		/// ------------------------------------------------------------------------------------
		protected override void UpdateDisplay()
		{
			if (ViewModel.GetIsRecording())
			{
				_buttonListenToOriginal.Enabled = false;
				_buttonRecordAnnotation.ForeColor = _buttonEraseAnnotation.ForeColor;
				_buttonRecordAnnotation.Text = LocalizationManager.GetString(
					"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingButtonText.WhenRecording",
					"Recording...");
				return;
			}

			Utils.SetWindowRedraw(_tableLayoutOuter, false);
			var annotationExistsForCurrSegment = ViewModel.GetDoesCurrentSegmentHaveAnnotationFile();

			_buttonEraseAnnotation.Visible = annotationExistsForCurrSegment;
			_buttonEraseAnnotation.Enabled = (!_waveControl.IsPlaying && !ViewModel.GetIsAnnotationPlaying());
			_buttonListenToOriginal.Enabled = !ViewModel.GetIsAnnotationPlaying();
			_buttonListenToAnnotation.Visible = annotationExistsForCurrSegment;
			_buttonListenToAnnotation.Enabled = !_waveControl.IsPlaying;
			_buttonRecordAnnotation.Visible = !annotationExistsForCurrSegment;
			_buttonRecordAnnotation.ForeColor = _buttonEraseAnnotation.ForeColor;
			_buttonRecordAnnotation.Text = _normalRecordButtonText;
			_buttonRecordAnnotation.Enabled = (!_waveControl.IsPlaying &&
				(ViewModel.CurrentSegment != null || (_waveControl.GetCursorTime() > ViewModel.GetStartOfCurrentSegment())));

			base.UpdateDisplay();
			Utils.SetWindowRedraw(_tableLayoutOuter, true);
		}

		/// ------------------------------------------------------------------------------------
		protected override void UpdateStatusLabelsDisplay()
		{
			base.UpdateStatusLabelsDisplay();

			if (ViewModel.CurrentSegment != null)
			{
				_labelSegmentNumber.Visible = false;
				_labelSegmentXofY.Visible = true;
				_labelSegmentXofY.Text = string.Format(_segmentXofYFormat,
					ViewModel.TimeTier.GetIndexOfSegment(ViewModel.CurrentSegment) + 1, _viewModel.GetSegmentCount());
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override TimeSpan GetCurrentTimeForTimeDisplay()
		{
			return (ViewModel.CurrentSegment == null ?
				ViewModel.GetEndOfLastSegment() : ViewModel.GetStartOfCurrentSegment());
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnAdjustBoundaryUsingArrowKey(int milliseconds)
		{
			if (!base.OnAdjustBoundaryUsingArrowKey(milliseconds))
				return false;

			if (ViewModel.GetEndOfCurrentSegment() == TimeSpan.Zero)
				AdjustPotentialNewSegmentEndBoundaryOnArrowKey(milliseconds);
			else
				AdjustExistingSegmentEndBoundaryOnArrowKey(milliseconds);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjusting the selected region beyond the last segment. I.e. one the user is
		/// preparing to record an annotation for.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustPotentialNewSegmentEndBoundaryOnArrowKey(int milliseconds)
		{
			// At this point, we know we're adjusting the selected region beyond the last segment.
			// I.e. one the user is preparing to record an annotation for.
			var endTime = _waveControl.GetCursorTime() + TimeSpan.FromMilliseconds(milliseconds);
			_waveControl.SetCursor(endTime);
			_waveControl.SetSelectionTimes(ViewModel.GetStartOfCurrentSegment(), endTime);
		}

		/// ------------------------------------------------------------------------------------
		private void AdjustExistingSegmentEndBoundaryOnArrowKey(int milliseconds)
		{
			if (_currentMovingBoundaryTime == TimeSpan.Zero)
				_currentMovingBoundaryTime = ViewModel.GetEndOfCurrentSegment();

			_currentMovingBoundaryTime += TimeSpan.FromMilliseconds(milliseconds);

			_waveControl.SegmentBoundaries = ViewModel.GetSegmentEndBoundaries()
				.Select(b => b == _timeAtBeginningOfboundaryMove ? _currentMovingBoundaryTime : b);

			_waveControl.SelectedRegionChanged -= HandleWaveControlSelectedRegionChanged;
			_waveControl.SetSelectionTimes(ViewModel.GetStartOfCurrentSegment(), _currentMovingBoundaryTime);
			_waveControl.SelectedRegionChanged += HandleWaveControlSelectedRegionChanged;
		}

		/// ------------------------------------------------------------------------------------
		protected override void FinalizeBoundaryMovedUsingArrowKeys()
		{
			if (_currentMovingBoundaryTime != TimeSpan.Zero)
			{
				ViewModel.SegmentBoundaryMoved(_timeAtBeginningOfboundaryMove, _currentMovingBoundaryTime);
				PlaybackShortPortionUpToBoundary(_currentMovingBoundaryTime);
			}
			else
				PlaybackShortPortionUpToBoundary(_waveControl.GetCursorTime());

			_currentMovingBoundaryTime = TimeSpan.Zero;
			base.FinalizeBoundaryMovedUsingArrowKeys();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPlayingback(WaveControlBasic ctrl, TimeSpan current, TimeSpan total)
		{
			base.OnPlayingback(ctrl, current, total);

			if (ViewModel.CurrentSegment == null)
				_waveControl.SetSelectionTimes(ViewModel.GetStartOfCurrentSegment(), current);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPlaybackStopped(WaveControlBasic ctrl, TimeSpan start, TimeSpan end)
		{
			base.OnPlaybackStopped(ctrl, start, end);

			if (ViewModel.GetIsSegmentLongEnough(end))
			{
				_buttonRecordAnnotation.ForeColor = _buttonEraseAnnotation.ForeColor;
				_buttonRecordAnnotation.Text = _normalRecordButtonText;
			}

			if (ViewModel.CurrentSegment != null)
				_waveControl.SetCursor(TimeSpan.FromSeconds(1).Negate());
		}

		/// ------------------------------------------------------------------------------------
		protected override TimeSpan GetSubSegmentReplayEndTime()
		{
			return (ViewModel.CurrentSegment == null ?
				_waveControl.GetCursorTime() : ViewModel.GetEndOfCurrentSegment());
		}

		/// ------------------------------------------------------------------------------------
		protected override TimeSpan GetBoundaryToAdjustOnArrowKeys()
		{
			return (ViewModel.CurrentSegment == null ?
				_waveControl.GetCursorTime() : ViewModel.GetEndOfCurrentSegment());
		}

		#region Event handlers for the wave control
		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlMouseMove(object sender, MouseEventArgs e)
		{
			var segMouseOver = _waveControl.GetSegmentForX(e.X);

			if (segMouseOver >= 0 && !ViewModel.GetDoesSegmentHaveAnnotationFile(segMouseOver))
			{
				if (_tooltip.GetToolTip(_waveControl) == string.Empty)
				{
					_tooltip.SetToolTip(_waveControl, LocalizationManager.GetString(
						"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.NoAnnotationToolTipMsg",
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
				if (!segRect.IsEmpty && !ViewModel.GetDoesSegmentHaveAnnotationFile(i++))
				{
					var rc = new Rectangle(new Point(segRect.X + 1, segRect.Y + 1), img.Size);
					if (segRect.Contains(rc))
						e.Graphics.DrawImage(img, rc);
				}
			}
		}

		#endregion

		#region Annotation Listen/Erase/Record button handling
		/// ------------------------------------------------------------------------------------
		private void HandleListenToOriginalMouseDown(object sender, MouseEventArgs e)
		{
			if (_waveControl.IsPlaying)
				return;

			if (ViewModel.CurrentSegment != null)
				_waveControl.PlaySelectedRegion();
			else
			{
				var start = ViewModel.GetTimeWherePlaybackShouldStart(_waveControl.GetCursorTime());
				_waveControl.SetSelectionTimes(start, start);
				_waveControl.Play(start);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordAnnotationMouseDown(object sender, MouseEventArgs e)
		{
			if (!ViewModel.GetIsSegmentLongEnough(_waveControl.GetCursorTime()))
			{
				_buttonRecordAnnotation.ForeColor = Color.Red;
				_buttonRecordAnnotation.Text = GetSegmentTooShortText();
				return;
			}

			if (ViewModel.BeginAnnotationRecording(_waveControl.GetCursorTime()))
				UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordAnnotationMouseUp(object sender, MouseEventArgs e)
		{
			if (!ViewModel.GetIsRecording())
				return;

			if (ViewModel.StopAnnotationRecording())
			{
				_waveControl.SegmentBoundaries = ViewModel.InsertNewBoundary(_waveControl.GetCursorTime());

				if (ViewModel.CurrentSegment == null)
					_waveControl.ClearSelection();
				else
					_waveControl.SetCursor(TimeSpan.FromSeconds(1).Negate());

				UpdateDisplay();
			}
			else
			{
				_buttonRecordAnnotation.ForeColor = Color.Red;
				_buttonRecordAnnotation.Text = LocalizationManager.GetString(
					"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingButtonText.WhenRecordingTooShort",
					"Whoops! You need to hold down the SPACE bar or mouse button while talking.");
				_buttonEraseAnnotation.PerformClick();
			}
		}

		#endregion

		#region Low level keyboard handling
		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyDown(Keys key)
		{
			if (!ContainsFocus)
				return true;

			// Check that SHIFT is not down too, because Ctrl+Shift on a UI item brings up
			// the localization dialog box. We don't want it to also start playback.
			if (key == Keys.Space)
			{
				HandleListenToOriginalMouseDown(null, null);
				return true;
			}

			if (key == Keys.Enter)
			{
				HandleRecordAnnotationMouseDown(null, null);
				return true;
			}

			if ((key == Keys.Escape || key == Keys.End) && !_waveControl.IsPlaying)
			{
				_waveControl.SetCursor(ViewModel.GetEndOfLastSegment());
				_waveControl.ClearSelection();
				UpdateDisplay();
				return true;
			}

			return base.OnLowLevelKeyDown(key);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyUp(Keys key)
		{
			if (!ContainsFocus)
				return true;

			if (key == Keys.Space)
			{
				_waveControl.Stop();
				return true;
			}

			if (key == Keys.Enter)
			{
				HandleRecordAnnotationMouseUp(null, null);
				return true;
			}

			return base.OnLowLevelKeyUp(key);
		}

		#endregion
	}
}
