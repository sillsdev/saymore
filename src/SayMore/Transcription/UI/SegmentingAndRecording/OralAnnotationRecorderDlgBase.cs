using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using Localization;
using Localization.UI;
using Palaso.Media.Naudio;
using Palaso.Media.Naudio.UI;
using Palaso.Reporting;
using SayMore.Media.Audio;
using SayMore.Media.MPlayer;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SilTools;
using Timer = System.Windows.Forms.Timer;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationRecorderBaseDlg : SegmenterDlgBase
	{
		private enum SpaceBarMode
		{
			Listen,
			Record,
			Done,
		}

		private readonly ToolTip _tooltip = new ToolTip();
		private PeakMeterCtrl _peakMeter;
		private RecordingDeviceButton _recDeviceButton;
		private Timer _recordingTooShortMsgTimer;
		private Image _hotPlayInSegmentButton;
		private Image _hotPlaySourceButton;
		private Image _hotRecordAnnotationButton;
		private Image _normalPlayInSegmentButton;
		private Image _normalPlaySourceButton;
		private Image _normalRecordAnnotationButton;
		private Image _normalRerecordAnnotationButton;
		private Image _hotRerecordAnnotationButton;

		private TimeSpan _elapsedRecordingTime;
		private TimeSpan _annotationPlaybackLength;
		private TimeSpan _lastAnnotationPlaybackPosition;
		private Segment _segmentWhoseAnnotationIsBeingPlayedBack;
		private Font _annotationSegmentFont;
		private TimeRange _segmentBeingRecorded;
		private bool _spaceKeyIsDown;
		private bool _playingBackUsingHoldDownButton;
		private bool _reRecording;
		private bool _userHasListenedToSelectedSegment;
		private SpaceBarMode _spaceBarMode;
		private readonly Color _selectedSegmentHighlighColor = Color.Moccasin;

		protected WaveControlWithRangeSelection _waveControl;

		/// ------------------------------------------------------------------------------------
		public static OralAnnotationRecorderBaseDlg Create(
			OralAnnotationRecorderDlgViewModel viewModel, AudioRecordingType annotationType)
		{
			return (annotationType == AudioRecordingType.Careful ?
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
			AudioUtils.NAudioExceptionThrown += HandleNAudioExceptionThrown;

			InitializeComponent();

			_cursorBlinkTimer.Tag = true;

			_scrollTimer.Tick += delegate
			{
				_scrollTimer.Stop();
				ScrollInPreparationForListenOrRecord((Label)_scrollTimer.Tag);
			};

			InitializeListenAndRecordButtonEvents();

			_toolStripStatus.Visible = false;

			BackColor = Settings.Default.BarColorBorder;

			InitializeTableLayouts();
			SetupPeakMeterAndRecordingDeviceIndicator();

			_spaceBarMode = viewModel.GetIsFullyAnnotated() ? SpaceBarMode.Done : SpaceBarMode.Listen;
			viewModel.PlaybackErrorAction = HandlePlaybackError;
			viewModel.RecordingErrorAction = HandleRecordingError;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleNAudioExceptionThrown(Exception exception)
		{
			if (!AudioUtils.GetCanRecordAudio(true))
			{
				ViewModel.CloseAnnotationRecorder();
				if (!_checkForRecordingDevice.Enabled)
					_checkForRecordingDevice.Start();
			}
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		void CheckForRecordingDevice(object sender, EventArgs e)
		{
			if (AudioUtils.GetCanRecordAudio())
			{
				_checkForRecordingDevice.Stop();
				ViewModel.InitializeAnnotationRecorder(_peakMeter, HandleAnnotationRecordingProgress);
				_waveControl.Invalidate();
				UpdateDisplay();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
				_annotationSegmentFont.Dispose();
				_hotPlaySourceButton.Dispose();
				_hotRecordAnnotationButton.Dispose();
				LocalizeItemDlg.StringsLocalized -= HandleStringsLocalized;
				AudioUtils.NAudioExceptionThrown -= HandleNAudioExceptionThrown;
			}

			base.Dispose(disposing);
		}

		#region Initialization methods
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			ViewModel.InitializeAnnotationRecorder(_peakMeter, HandleAnnotationRecordingProgress);
			base.OnLoad(e);
			InitializeTableLayoutButtonControls();
			InitializeHintLabelsAndButtonFonts();
			ViewModel.RemoveInvalidAnnotationFiles();
			WavePainter.UnsegmentedBackgroundColor = _panelListen.BackColor;
			WavePainter.SegmentedBackgroundColor = Settings.Default.BarColorBegin;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (_moreReliableDesignMode)
				return;

			_cursorBlinkTimer.Enabled = true;

			if (ViewModel.GetSegmentCount() > 0)
			{
				if (ViewModel.SetNextUnannotatedSegment())
					_waveControl.SetSelectionTimes(ViewModel.CurrentUnannotatedSegment.TimeRange, _selectedSegmentHighlighColor);

				ScrollInPreparationForListenOrRecord(_labelListenButton);
			}

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void SetupPeakMeterAndRecordingDeviceIndicator()
		{
			_tableLayoutRecordAnnotations.ColumnStyles[0].SizeType = SizeType.AutoSize;
			_tableLayoutRecordAnnotations.ColumnStyles[1].SizeType = SizeType.Percent;

			_recDeviceButton = new RecordingDeviceButton();
			_recDeviceButton.Anchor = AnchorStyles.Top;
			_recDeviceButton.AutoSize = true;
			_recDeviceButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			_recDeviceButton.Margin = new Padding(4, 3, 0, 3);
			_tableLayoutRecordAnnotations.Controls.Add(_recDeviceButton, 0, 2);
			_recDeviceButton.Recorder = ViewModel.Recorder;

			_panelPeakMeter.BorderColor = Settings.Default.BarColorBorder;
			_panelPeakMeter.BackColor = Settings.Default.BarColorBegin;
			_peakMeter = AudioUtils.CreatePeakMeterControl(_panelPeakMeter);
			_peakMeter.MeterStyle = PeakMeterStyle.PMS_Vertical;
			_peakMeter.LEDCount = 10;
			_peakMeter.SetRange(10, 40, 50);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeListenAndRecordButtonEvents()
		{
			_labelListenButton.MouseEnter += delegate
			{
				HandleListenOrRecordButtonMouseEnter(_labelListenButton, _hotPlaySourceButton);
			};

			_labelListenButton.MouseLeave += delegate
			{
				_labelListenButton.Image = _normalPlaySourceButton;
				_scrollTimer.Stop();
			};

			_labelListenButton.MouseUp += delegate
			{
				_newSegmentDefinedBy = SegmentDefinitionMode.PressingButton;
				FinishListeningUsingEarOrSpace();
			};

			_labelRecordButton.MouseEnter += delegate
			{
				InvalidateBottomReservedRectangleForCurrentUnannotatedSegment();
				HandleListenOrRecordButtonMouseEnter(_labelRecordButton, _hotRecordAnnotationButton);
			};

			_labelRecordButton.MouseLeave += delegate
			{
				InvalidateBottomReservedRectangleForCurrentUnannotatedSegment();
				_labelRecordButton.Image = _normalRecordAnnotationButton;
				_scrollTimer.Stop();
			};

			_labelRecordButton.MouseDown += HandleRecordAnnotationMouseDown;
			_labelRecordButton.MouseUp += delegate { FinishRecording(true); };
		}

		/// ------------------------------------------------------------------------------------
		private void FinishListeningUsingEarOrSpace()
		{
			if (_waveControl.IsPlaying)
				_waveControl.Stop();

			_playingBackUsingHoldDownButton = false;
			if (ViewModel.GetSelectedSegmentIsLongEnough() && _userHasListenedToSelectedSegment)
				_spaceBarMode = SpaceBarMode.Record;

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeTableLayouts()
		{
			_tableLayoutTop.Visible = false;

			_tableLayoutMediaButtons.Dock = DockStyle.Left;
			_panelWaveControl.Controls.Add(_tableLayoutMediaButtons);
			_tableLayoutMediaButtons.BringToFront();
			_tableLayoutMediaButtons.RowStyles[0].SizeType = SizeType.AutoSize;
			_tableLayoutMediaButtons.RowStyles[_tableLayoutMediaButtons.RowCount - 1].SizeType = SizeType.Absolute;
			Panel panel = new Panel();
			panel.Dock = DockStyle.Fill;
			panel.BackColor = Color.AliceBlue;
			panel.AutoSize = true;
			_tableLayoutMediaButtons.Controls.Add(panel, 0, 0);
			panel.Margin = _panelListen.Margin;
			_labelSourceRecording.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			var margin = _labelSourceRecording.Margin;
			margin.Top = 10;
			margin.Left = margin.Right;
			_labelSourceRecording.Margin = margin;
			panel.Controls.Add(_labelSourceRecording);
			_labelSourceRecording.Location = new Point(3, 10);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeWaveControlContextActionImages()
		{
			if (_moreReliableDesignMode)
				return;

			_normalPlayInSegmentButton = Resources.ListenToSegmentsAnnotation;
			_normalPlaySourceButton = _labelListenButton.Image;
			_normalRecordAnnotationButton = _labelRecordButton.Image;
			_normalRerecordAnnotationButton = Resources.RerecordOralAnnotation;
			_hotPlayInSegmentButton = PaintingHelper.MakeHotImage(_normalPlayInSegmentButton);
			_hotPlaySourceButton = PaintingHelper.MakeHotImage(_normalPlaySourceButton);
			_hotRecordAnnotationButton = PaintingHelper.MakeHotImage(_normalRecordAnnotationButton);
			_hotRerecordAnnotationButton = PaintingHelper.MakeHotImage(_normalRerecordAnnotationButton);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeHintLabelsAndButtonFonts()
		{
			_labelListenHint.Font = _labelSourceRecording.Font;
			_labelRecordHint.Font = _labelSourceRecording.Font;
			_labelErrorInfo.Font = _labelSourceRecording.Font;
			_labelFinishedHint.Font = FontHelper.MakeFont(Program.DialogFont, 10, FontStyle.Bold);
			_labelListenButton.Font = Program.DialogFont;
			_labelRecordButton.Font = Program.DialogFont;
			_undoToolStripMenuItem.Font = Program.DialogFont;
			_labelSourceRecording.ForeColor = _labelListenButton.ForeColor;
			_videoHelpMenu.Font = _labelSourceRecording.Font;

			_annotationSegmentFont = FontHelper.MakeFont(Program.DialogFont, 8, FontStyle.Bold);

			LocalizeItemDlg.StringsLocalized += HandleStringsLocalized;
		}

		private const int kNumberOfRows = 4;

		/// ------------------------------------------------------------------------------------
		private void InitializeTableLayoutButtonControls()
		{
			_tableLayoutButtons.RowCount = kNumberOfRows;
			_tableLayoutButtons.Controls.Add(_videoHelpMenuStrip, 1, kNumberOfRows - 1);
			_tableLayoutButtons.RowStyles[0].Height = 25f;
			_tableLayoutButtons.RowStyles[1].Height = 25f;
			_tableLayoutButtons.RowStyles[2].Height = 25f;
			_tableLayoutButtons.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1f));
			_tableLayoutButtons.Height += _videoHelpMenuStrip.Height;
			_videoHelpMenuStrip.BackColor = BackColor;

			var okButtonPos = _tableLayoutButtons.GetPositionFromControl(_buttonOK);
			_tableLayoutButtons.Controls.Add(_buttonOK, okButtonPos.Column, kNumberOfRows - 1);
			_tableLayoutButtons.SetRowSpan(_buttonOK, 1);
			var cancelButtonPos = _tableLayoutButtons.GetPositionFromControl(_buttonCancel);
			_tableLayoutButtons.Controls.Add(_buttonCancel, cancelButtonPos.Column, kNumberOfRows - 1);
			_tableLayoutButtons.SetRowSpan(_buttonCancel, 1);

			_tableLayoutButtons.Controls.Add(_pictureIcon, 0, 0);
			_tableLayoutButtons.SetRowSpan(_pictureIcon, 3);
			_pictureIcon.Anchor = AnchorStyles.Left | AnchorStyles.Right;

			_tableLayoutButtons.Controls.Add(_labelErrorInfo, 1, 0);
			_tableLayoutButtons.Controls.Add(_labelListenHint, 1, 1);
			_tableLayoutButtons.Controls.Add(_labelRecordHint, 1, 2);

			_tableLayoutButtons.ColumnStyles[0].SizeType = SizeType.AutoSize;
			_tableLayoutButtons.ColumnStyles[1].SizeType = SizeType.Percent;
		}

		/// ------------------------------------------------------------------------------------
		protected void InitializeRecordingLabel(Label labelRecording)
		{
			labelRecording.Margin = _labelSourceRecording.Margin;
			labelRecording.TextAlign = _labelSourceRecording.TextAlign;
			labelRecording.Anchor = _labelSourceRecording.Anchor;
			_tableLayoutRecordAnnotations.RowStyles[0].SizeType = SizeType.AutoSize;
			_tableLayoutRecordAnnotations.Controls.Add(labelRecording, 0, 0);
			_tableLayoutRecordAnnotations.SetColumnSpan(labelRecording, 2);
			labelRecording.ForeColor = _labelRecordButton.ForeColor;
			_labelSourceRecording.FontChanged += delegate { labelRecording.Font = _labelSourceRecording.Font; };
		}

		/// ------------------------------------------------------------------------------------
		protected override WaveControlBasic CreateWaveControl()
		{
			_waveControl = new WaveControlWithRangeSelection();
			_waveControl.BottomReservedAreaBorderColor = Settings.Default.DataEntryPanelColorBorder;
			_waveControl.BottomReservedAreaColor = _tableLayoutRecordAnnotations.BackColor;
			_waveControl.Controls.Add(_lastSegmentMenuStrip);
			_lastSegmentMenuStrip.UseWaitCursor = false;

			_waveControl.BottomReservedAreaPaintAction = HandlePaintingAnnotatedWaveArea;
			_waveControl.PostPaintAction = HandleWaveControlPostPaint;
			_waveControl.MouseMove += HandleWaveControlMouseMove;
			_waveControl.MouseLeave += HandleWaveControlMouseLeave;
			_waveControl.MouseClick += HandleWaveControlMouseClick;
			_waveControl.MouseDown += HandleWaveControlMouseDown;
			_waveControl.BoundaryMoved += HandleSegmentBoundaryMovedInWaveControl;
			_waveControl.PlaybackStarted += delegate { KillSegTooShortMsgTimer(); };
			_waveControl.PlaybackErrorAction = HandlePlaybackError;

			_waveControl.MouseUp += delegate
			{
				if (_reRecording)
					FinishRecording(false);
			};

			_waveControl.ClientSizeChanged += delegate
			{
				_waveControl.BottomReservedAreaHeight =
					_waveControl.ClientSize.Height / (ViewModel.OrigWaveStream.WaveFormat.Channels + 1);

				_tableLayoutMediaButtons.RowStyles[_tableLayoutMediaButtons.RowCount - 1].Height =
					_waveControl.BottomReservedAreaHeight +
					(_waveControl.HorizontalScroll.Visible ? SystemInformation.HorizontalScrollBarHeight : 0);
			};

			_waveControl.BoundaryMouseDown += (ctrl, dx, boundary, boundaryNumber) =>
			{
				_waveControl.Stop();
				_newSegmentDefinedBy = SegmentDefinitionMode.Manual;
				UpdateDisplay();
			};

			_waveControl.SetCursorAtTimeOnMouseClick += (ctrl, desiredTime) =>
			{
				if (_waveControl.IsPlaying)
					return desiredTime;

				var lastSegmentEndTime = ViewModel.GetEndOfLastSegment();
				if (desiredTime < lastSegmentEndTime || _waveControl.GetIsMouseOverBoundary())
					return TimeSpan.Zero;

				UpdateDisplay();
				return lastSegmentEndTime;
			};

			_waveControl.Controls.Add(_pictureRecording);
			InitializeWaveControlContextActionImages();

			return _waveControl;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		WavePainterWithRangeSelection WavePainter
		{
			get { return (WavePainterWithRangeSelection)_waveControl.Painter; }
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

		#endregion

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			ViewModel.CloseAnnotationPlayer();
			ViewModel.CloseAnnotationRecorder();
			base.OnFormClosed(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			base.HandleStringsLocalized();
			UpdateDisplay();
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
			var undoableSegmentRange = ViewModel.TimeRangeForUndo;
			_lastSegmentMenuStrip.Visible = _undoToolStripMenuItem.Enabled = (undoableSegmentRange != null);
			if (_lastSegmentMenuStrip.Visible)
			{
				_lastSegmentMenuStrip.Location = new Point(/*_waveControl.Left +*/
					WavePainter.ConvertTimeToXCoordinate(undoableSegmentRange.End) - _lastSegmentMenuStrip.Width - 5,
					/*Padding.Top + _waveControl.Top + */5);
				_undoToolStripMenuItem.ToolTipText = String.Format(LocalizationManager.GetString(
					"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.UndoToolTipMsg",
					"Undo: {0} (Ctrl-Z)"), ViewModel.DescriptionForUndo);
			}

			_labelListenButton.Image = (_waveControl.IsPlaying && _playingBackUsingHoldDownButton ?
				Resources.ListenToOriginalRecordingDown : Resources.ListenToOriginalRecording);

			_labelRecordButton.Image = (ViewModel.GetIsRecording() ?
				Resources.RecordingOralAnnotationInProgress : Resources.RecordOralAnnotation);

			_labelListenButton.Enabled = !ViewModel.GetIsRecording() &&
				(ViewModel.CurrentUnannotatedSegment != null || !ViewModel.GetIsFullyAnnotated());

			_labelRecordButton.Enabled = (ViewModel.GetSelectedSegmentIsLongEnough() &&
				_userHasListenedToSelectedSegment &&
				AudioUtils.GetCanRecordAudio(true) &&
				!_waveControl.IsPlaying && !ViewModel.GetIsAnnotationPlaying());

			_labelListenHint.Visible = _spaceBarMode == SpaceBarMode.Listen && _labelListenButton.Enabled;
			_labelRecordHint.Visible = _spaceBarMode == SpaceBarMode.Record && _labelRecordButton.Enabled && !_reRecording;

			if (_spaceBarMode == SpaceBarMode.Done)
			{
				if (!_labelFinishedHint.Visible)
				{
					_pictureIcon.Image = Resources.Green_check;
					_labelFinishedHint.Visible = true;
					_tableLayoutButtons.Controls.Add(_labelFinishedHint, 1, 0);
					_tableLayoutButtons.SetRowSpan(_labelFinishedHint, 3);
					AcceptButton = _buttonOK;
				}
			}
			else
			{
				UdateErrorMessageDisplay();

				_pictureIcon.Image = (_labelErrorInfo.Visible) ? Resources.Information_red : Resources.Information_blue;

				float percentage = (_labelErrorInfo.Visible) ? 50 : 100;
				_tableLayoutButtons.RowStyles[0].Height = (_labelErrorInfo.Visible) ? percentage : 0;
				_tableLayoutButtons.RowStyles[1].Height = (_labelListenHint.Visible) ? percentage : 0;
				_tableLayoutButtons.RowStyles[2].Height = (_labelRecordHint.Visible) ? percentage : 0;
			}
			base.UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void UdateErrorMessageDisplay()
		{
			_labelErrorInfo.Visible = false;
			if (!_playingBackUsingHoldDownButton && !_waveControl.IsBoundaryMovingInProgress)
			{
				if (ViewModel.GetHasNewSegment() && !ViewModel.GetSelectedSegmentIsLongEnough())
				{
					_labelErrorInfo.Visible = true;
					_labelErrorInfo.Text = GetSegmentTooShortText();
				}
				else if (ViewModel.GetIsRecorderInErrorState())
				{
					_labelErrorInfo.Visible = true;
					_labelErrorInfo.Text = LocalizationManager.GetString(
					"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.CannotRecordErrorMsg",
					"Recording not working. Please make sure your microphone is plugged in.");
				}
				else if (_recordingTooShortMsgTimer != null)
				{
					_labelErrorInfo.Visible = true;

					bool selectedSegmentHadRecordingThatWasTooShort =
					(_recordingTooShortMsgTimer != null && ViewModel.GetSelectedTimeRange() == (TimeRange)_recordingTooShortMsgTimer.Tag);

					_labelErrorInfo.Text = selectedSegmentHadRecordingThatWasTooShort ?
						LocalizationManager.GetString(
						"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingTooShortMessage.WhenSpaceOrMouseIsValid",
						"Whoops. You need to hold down the SPACE BAR or mouse button while talking.")
						: LocalizationManager.GetString(
						"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingTooShortMessage.WhenOnlyMouseIsValid",
						"Whoops. You need to hold down the mouse button while talking.");
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void UpdateStatusLabelsDisplay()
		{
			base.UpdateStatusLabelsDisplay();

			var currentSegment = GetHighlightedSegment();
			if (currentSegment == null)
				return;

			_labelSegmentNumber.Visible = false;
			_labelSegmentXofY.Visible = true;
			_labelSegmentXofY.Text = string.Format(_segmentXofYFormat,
				ViewModel.TimeTier.GetIndexOfSegment(currentSegment) + 1, _viewModel.GetSegmentCount());
		}

		/// ------------------------------------------------------------------------------------
		protected override TimeSpan GetCurrentTimeForTimeDisplay()
		{
			var currentSegment = GetHighlightedSegment();
			return (currentSegment == null ? ViewModel.GetEndOfLastSegment() : currentSegment.TimeRange.Start);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnAdjustBoundaryUsingArrowKey(int milliseconds)
		{
			if (!base.OnAdjustBoundaryUsingArrowKey(milliseconds))
				return false;

			AdjustNewSegmentEndBoundaryOnArrowKey(milliseconds);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjusting the selected region beyond the last segment. I.e. one the user is
		/// preparing to record an annotation for.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustNewSegmentEndBoundaryOnArrowKey(int milliseconds)
		{
			var newEndTime = ViewModel.NewSegmentEndBoundary + TimeSpan.FromMilliseconds(milliseconds);
			if (newEndTime < ViewModel.GetEndOfLastSegment())
				return;

			_newSegmentDefinedBy = SegmentDefinitionMode.Manual;

			_cursorBlinkTimer.Tag = false;
			_cursorBlinkTimer.Enabled = false;

			// At this point, we know we're adjusting the selected region beyond the last segment.
			// I.e. one the user is preparing to record an annotation for.
			SetNewSegmentEndBoundary(newEndTime);

			// The above call selects the new range using the "current" color, but this segment is
			// also the "hot" segment, so we need to highlight it using that color as well.
			WavePainter.SetSelectionTimes(ViewModel.GetEndOfLastSegment(), newEndTime);

			_waveControl.SetCursor(ViewModel.NewSegmentEndBoundary);
			_cursorBlinkTimer.Enabled = true;
		}

		// REVIEW: How to adjust existing boundaries using arrow keys?
		///// ------------------------------------------------------------------------------------
		//private void AdjustExistingSegmentEndBoundaryOnArrowKey(int milliseconds)
		//{
		//    if (_currentMovingBoundaryTime == TimeSpan.Zero)
		//        _currentMovingBoundaryTime = ViewModel.GetEndOfCurrentSegment();

		//    _currentMovingBoundaryTime += TimeSpan.FromMilliseconds(milliseconds);

		//    _waveControl.SegmentBoundaries = ViewModel.GetSegmentEndBoundaries()
		//        .Select(b => b == _timeAtBeginningOfboundaryMove ? _currentMovingBoundaryTime : b);

		//    _waveControl.SetSelectionTimes(ViewModel.GetStartOfCurrentSegment(), _currentMovingBoundaryTime);
		//}

		/// ------------------------------------------------------------------------------------
		protected override void FinalizeBoundaryMovedUsingArrowKeys()
		{
			_cursorBlinkTimer.Tag = false;
			_cursorBlinkTimer.Enabled = false;
			_waveControl.InvalidateIfNeeded(GetNewSegmentCursorRectangle());
			if (ViewModel.GetHasNewSegment())
				UpdateFollowingNewSegmentBoundaryMove();
			else
			{
				_spaceBarMode = SpaceBarMode.Listen;
				UpdateDisplay();
			}
			base.FinalizeBoundaryMovedUsingArrowKeys();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSegmentBoundaryMovedInWaveControl(bool segMoved,
			TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			if (newEndTime == ViewModel.NewSegmentEndBoundary)
				UpdateFollowingNewSegmentBoundaryMove();
			else
				base.OnSegmentBoundaryMovedInWaveControl(segMoved, oldEndTime, newEndTime);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateFollowingNewSegmentBoundaryMove()
		{
			PlaybackShortPortionUpToBoundary(ViewModel.NewSegmentEndBoundary);
			_spaceBarMode = (ViewModel.GetIsSegmentLongEnough(ViewModel.NewSegmentEndBoundary)) ?
				SpaceBarMode.Record : SpaceBarMode.Listen;
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected override void PlaybackShortPortionUpToBoundary(WaveControlBasic ctrl,
			TimeSpan time1, TimeSpan time2)
		{
			_cursorBlinkTimer.Enabled = true;
			base.PlaybackShortPortionUpToBoundary(ctrl, time1, time2);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPlayingback(WaveControlBasic ctrl, TimeSpan current, TimeSpan total)
		{
			base.OnPlayingback(ctrl, current, total);

			var endOfLastSegment = ViewModel.GetEndOfLastSegment();
			if (current > endOfLastSegment && current > ViewModel.NewSegmentEndBoundary)
				SetNewSegmentEndBoundary(current);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPlaybackStopped(WaveControlBasic ctrl, TimeSpan start, TimeSpan end)
		{
			base.OnPlaybackStopped(ctrl, start, end);

			if (!ViewModel.GetSelectedTimeRange().Contains(end))
			{
				if (GetHighlightedSegment() != null)
					_waveControl.SetCursor(TimeSpan.FromSeconds(1).Negate());
				else if (ViewModel.GetHasNewSegment())
					_waveControl.SetCursor(end);
			}

			if (end > ViewModel.GetEndOfLastSegment())
			{
				var rc1 = GetNewSegmentCursorRectangle();
				if (_playingBackUsingHoldDownButton)
					SetNewSegmentEndBoundary(end);

				var rc2 = GetNewSegmentRectangle();
				rc2.Inflate(rc1.Width / 2, 0);
				_waveControl.InvalidateIfNeeded(rc2);
			}

			if (end == ViewModel.GetSelectedTimeRange().End)
			{
				InvalidateBottomReservedRectangleForCurrentUnannotatedSegment();
				_userHasListenedToSelectedSegment = true;
				UpdateDisplay();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void SetNewSegmentEndBoundary(TimeSpan end)
		{
			UpdateDisplayForChangeInNewSegmentEndBoundary(delegate { ViewModel.NewSegmentEndBoundary = end; });
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplayForChangeInNewSegmentEndBoundary(Action action)
		{
			var origNewSegmentEndBoundary = ViewModel.NewSegmentEndBoundary;
			var origNewSegmentCursorRectangle = GetNewSegmentCursorRectangle();
			action();
			var newEnd = ViewModel.NewSegmentEndBoundary;
			if (origNewSegmentEndBoundary != newEnd)
			{
				WavePainter.SetSelectionTimes(new TimeRange(ViewModel.GetEndOfLastSegment(), newEnd),
					_selectedSegmentHighlighColor);
				_waveControl.InvalidateIfNeeded((newEnd < origNewSegmentEndBoundary) ?
					origNewSegmentCursorRectangle : Rectangle.Empty);
			}
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected override TimeSpan GetBoundaryToAdjustOnArrowKeys()
		{
			return (ViewModel.GetHasNewSegment() ? ViewModel.NewSegmentEndBoundary :
				base.GetBoundaryToAdjustOnArrowKeys());
		}

		#region Event handlers for the wave control
		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlMouseMove(object sender, MouseEventArgs e)
		{
			if (ViewModel.GetIsRecording())
				return;

			var playButtonRects = GetPlayButtonRectanglesForSegmentMouseIsOver();
			if (playButtonRects != null)
			{
				_waveControl.InvalidateIfNeeded(playButtonRects.Item1);
				_waveControl.InvalidateIfNeeded(playButtonRects.Item2);
			}

			_waveControl.InvalidateIfNeeded(GetRerecordButtonRectangleForSegmentMouseIsOver());

			var segMouseOver = _waveControl.GetSegmentForX(e.X);

			if (segMouseOver >= 0)
			{
				if (!ViewModel.GetDoesSegmentHaveAnnotationFile(segMouseOver))
				{
					if (_tooltip.GetToolTip(_waveControl) == string.Empty)
					{
						_tooltip.SetToolTip(_waveControl, LocalizationManager.GetString(
							"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.NoAnnotationToolTipMsg",
							"This segment does not have a recorded annotaton."));
					}
					return;
				}
				if (GetRerecordButtonRectangleForSegmentMouseIsOver().Contains(e.Location))
				{
					if (_tooltip.GetToolTip(_waveControl) == string.Empty)
					{
						_tooltip.SetToolTip(_waveControl, LocalizationManager.GetString(
						"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RerecordAnnotationToolTipMsg",
						"To erase the recorded annotaton for this segment and record a new one, press and hold this button."));
					}
					return;
				}
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
		private void HandleWaveControlMouseClick(object sender, MouseEventArgs e)
		{
			var playButtonRectangles = GetPlayButtonRectanglesForSegmentMouseIsOver();
			if (playButtonRectangles == null || ViewModel.GetIsRecording())
				return;

			bool playSource = playButtonRectangles.Item1.Contains(e.Location);
			bool playAnnotation = playButtonRectangles.Item2.Contains(e.Location);
			if (!playSource && !playAnnotation)
				return;

			var segMouseOver = _waveControl.GetSegmentForX(e.X);

			if (ViewModel.GetIsAnnotationPlaying())
			{
				ViewModel.StopAnnotationPlayback();
				_lastAnnotationPlaybackPosition = TimeSpan.Zero;
				_waveControl.InvalidateIfNeeded(GetAnnotationPlaybackRectangle());
			}

			var segment = ViewModel.GetSegment(segMouseOver);

			if (segment == null)
			{
				// Play the source recording for the new segment.
				_waveControl.Play(ViewModel.GetEndOfLastSegment(), ViewModel.NewSegmentEndBoundary);
			}
			else
			{
				if (playSource)
					_waveControl.Play(segment.TimeRange);
				else
				{
					_waveControl.EnsureTimeIsVisible(segment.TimeRange.Start, segment.TimeRange, true, true);

					_segmentWhoseAnnotationIsBeingPlayedBack = segment;

					var path = ViewModel.GetFullPathToAnnotationFileForSegment(segment);
					_annotationPlaybackLength = ViewModel.SegmentsAnnotationSamplesToDraw.First(
						h => h.AudioFilePath == path).AudioDuration;

					KillSegTooShortMsgTimer();

					ViewModel.StartAnnotationPlayback(segment, HandleAnnotationPlaybackProgress, () =>
					{
						_lastAnnotationPlaybackPosition = TimeSpan.Zero;
						_waveControl.InvalidateIfNeeded(GetBottomReservedRectangleForSegment(_segmentWhoseAnnotationIsBeingPlayedBack));
						_segmentWhoseAnnotationIsBeingPlayedBack = null;
						_waveControl.DiscardScrollCalculator();
					});
				}
			}

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleVideoHelpButtonClick(object sender, EventArgs e)
		{
			UsageReporter.SendNavigationNotice("Video Help requested in " + ViewModel.AnnotationType + " dialog box.");
		}

		/// ------------------------------------------------------------------------------------
		private void HandleUndoButtonClick(object sender, EventArgs e)
		{
			UpdateDisplayForChangeInNewSegmentEndBoundary(delegate
			{
				var timeRangeToInvalidate = ViewModel.TimeRangeForUndo;
				ViewModel.Undo();
				_spaceBarMode = SpaceBarMode.Listen;

				// If Undo causes an annotation to be removed for a pre-existing segment, that
				// segment will now be the current unnanotated segment
				if (ViewModel.CurrentUnannotatedSegment != null)
					_waveControl.SetSelectionTimes(ViewModel.CurrentUnannotatedSegment.TimeRange, _selectedSegmentHighlighColor);

				ScrollInPreparationForListenOrRecord(_labelListenButton);
				_waveControl.InvalidateRegionBetweenTimes(timeRangeToInvalidate);

				if (_labelFinishedHint.Visible)
				{
					_pictureIcon.Image = Resources.Information_blue;
					_labelFinishedHint.Visible = false;
					_tableLayoutButtons.Controls.Remove(_labelFinishedHint);
					AcceptButton = null;

					InitializeTableLayoutButtonControls();
				}
			});
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetBottomReservedRectangleForSegment(Segment segment)
		{
			return _waveControl.Painter.GetBottomReservedRectangleForTimeRange(segment.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetAnnotationPlaybackRectangle()
		{
			if (_segmentWhoseAnnotationIsBeingPlayedBack == null)
				return Rectangle.Empty;

			var rc = GetBottomReservedRectangleForSegment(_segmentWhoseAnnotationIsBeingPlayedBack);
			rc.Inflate(-2, 0);
			return rc;
		}

		/// ------------------------------------------------------------------------------------
		private Segment GetHighlightedSegment()
		{
			var timeRangeOfHighlightedSegment = WavePainter.DefaultSelectedRange;
			return ViewModel.TimeTier.Segments.FirstOrDefault(s => s.TimeRange == timeRangeOfHighlightedSegment);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlMouseDown(object sender, MouseEventArgs e)
		{
			if (!GetRerecordButtonRectangleForSegmentMouseIsOver().Contains(e.Location))
				return;

			if (ViewModel.Recorder.GetIsInErrorState(true))
				return;

			var segMouseOver = GetHighlightedSegment();
			_reRecording = true;
			BeginRecording(segMouseOver.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnDeactivate(EventArgs e)
		{
			if (ViewModel.GetIsRecording())
				FinishRecording(!_reRecording);

			base.OnDeactivate(e);
		}

		/// ------------------------------------------------------------------------------------
		private void FinishRecording(bool advanceToNextUnannotatedSegment)
		{
			_pictureRecording.Visible = false;
			_waveControl.SelectSegmentOnMouseOver = true;

			if (_reRecording)
			{
				var rc = _waveControl.Painter.GetBottomReservedRectangleForTimeRange(ViewModel.GetSelectedTimeRange());
				_waveControl.InvalidateIfNeeded(rc);
				_reRecording = false;
			}

			var stopResult = ViewModel.StopAnnotationRecording(_segmentBeingRecorded);
			_waveControl.InvalidateIfNeeded(GetVisibleAnnotationRectangleForSegmentBeingRecorded());

			if (stopResult == StopAnnotationRecordingResult.AnnotationTooShort)
			{
				DisplayRecordingTooShortMessage();
				_segmentBeingRecorded = null;
				return;
			}

			_segmentBeingRecorded = null;

			if (stopResult == StopAnnotationRecordingResult.RecordingError)
				return;

			if (ViewModel.CurrentUnannotatedSegment == null)
			{
				_waveControl.SegmentBoundaries = ViewModel.MakeSegmentForEndBoundary();
				_waveControl.SetCursor(TimeSpan.FromSeconds(1).Negate());
			}

			if (advanceToNextUnannotatedSegment)
			{
				_userHasListenedToSelectedSegment = false;
				GoToNextUnannotatedSegment();
			}

			_spaceBarMode = ViewModel.GetIsFullyAnnotated() ? SpaceBarMode.Done : SpaceBarMode.Listen;

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void KillSegTooShortMsgTimer()
		{
			if (_recordingTooShortMsgTimer != null)
			{
				_recordingTooShortMsgTimer.Stop();
				_recordingTooShortMsgTimer.Dispose();
				_recordingTooShortMsgTimer = null;
				UpdateDisplay();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DisplayRecordingTooShortMessage()
		{
			KillSegTooShortMsgTimer();

			_recordingTooShortMsgTimer = new Timer();
			_recordingTooShortMsgTimer.Interval = Int32.MaxValue;
			_recordingTooShortMsgTimer.Tag = _segmentBeingRecorded;
			_recordingTooShortMsgTimer.Tick += delegate { KillSegTooShortMsgTimer(); };
			_recordingTooShortMsgTimer.Start();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleAnnotationPlaybackProgress(PlaybackProgressEventArgs args)
		{
			var newX = GetAnnotationPlaybackCursorX(args.PlaybackPosition);

			if (newX != GetAnnotationPlaybackCursorX(_lastAnnotationPlaybackPosition))
			{
				_lastAnnotationPlaybackPosition = args.PlaybackPosition;
				if (_waveControl.EnsureXIsVisible(newX))
					Invoke((Action<Rectangle>)_waveControl.InvalidateIfNeeded, GetAnnotationPlaybackRectangle());
			}
		}

		/// ------------------------------------------------------------------------------------
		private int GetAnnotationPlaybackCursorX(TimeSpan playbackPosition)
		{
			var rc = GetAnnotationPlaybackRectangle();
			var pixelPerMillisecond = rc.Width / _annotationPlaybackLength.TotalMilliseconds;

			return rc.X + (int)(Math.Ceiling(playbackPosition.TotalMilliseconds * pixelPerMillisecond));
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePaintingAnnotatedWaveArea(PaintEventArgs e, Rectangle areaRectangle)
		{
			var segRects = _waveControl.GetSegmentRectangles().ToArray();

			for (int i = 0; i < ViewModel.GetSegmentCount(); i++)
			{
				var rc = new Rectangle(segRects[i].X, areaRectangle.Y + 1,
					segRects[i].Width, areaRectangle.Height - 1);

				if (!areaRectangle.IntersectsWith(rc))
					continue;

				if (!ViewModel.GetDoesSegmentHaveAnnotationFile(i))
					continue;

				if (rc.X == 0)
					rc.Width -= 2;
				else
					rc.Inflate(-2, 0);

				using (var br = new SolidBrush(Color.FromArgb(175, Settings.Default.DataEntryPanelColorBegin)))
					e.Graphics.FillRectangle(br, rc);

				rc.Inflate(0, -3);
				rc.Width--;

				var segment = ViewModel.GetSegment(i);
				if (_segmentBeingRecorded == segment.TimeRange && ViewModel.GetIsRecording())
					continue;

				DrawOralAnnotationWave(e, rc, segment);
				DrawCursorInOralAnnotationWave(e, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DrawOralAnnotationWave(PaintEventArgs e, Rectangle rc, Segment segment)
		{
			// The reason we wrap this in a try/catch block is because in some rare cases
			// when an audio error occurs (e.g. unplugging the mic. while recording) we'll
			// get to this method to paint the annotation before the original annotation
			// is restored because of the error.
			try
			{
				// If the samples to paint for this oral annotation have not been calculated,
				// then create a helper to get those samples, then cache them in the ViewModel.
				var audioFilePath = ViewModel.GetFullPathToAnnotationFileForSegment(segment);
				var helper = ViewModel.SegmentsAnnotationSamplesToDraw.FirstOrDefault(h => h.AudioFilePath == audioFilePath);
				if (helper == null)
				{
					helper = new AudioFileHelper(audioFilePath);
					ViewModel.SegmentsAnnotationSamplesToDraw.Add(helper);
				}

				// Draw the oral annotation's wave in the bottom, reserved area of the wave control.
				using (var painter = new WavePainterBasic { ForeColor = Color.Black, BackColor = Color.Black })
				{
					painter.SetSamplesToDraw(helper.GetSamples((uint)rc.Width));
					painter.Draw(e, rc);
				}
			}
			catch (IOException)
			{
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DrawCursorInOralAnnotationWave(PaintEventArgs e, Rectangle rc)
		{
			// Draw oral annotation's playback cursor if it's non-zero.
			var x = GetAnnotationPlaybackCursorX(_lastAnnotationPlaybackPosition);
			if (x > 0 && x >= rc.X && x <= rc.Right)
			{
				rc.Inflate(0, 3);
				using (var pen = new Pen(_waveControl.Painter.CursorColor))
				{
					e.Graphics.DrawLine(pen, x, rc.Y, x, rc.Bottom);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCursorBlinkTimerTick(object sender, EventArgs e)
		{
			var newSegmentCursorRect = GetNewSegmentCursorRectangle();
			if ((_spaceBarMode == SpaceBarMode.Done ||
				(_spaceBarMode == SpaceBarMode.Listen && newSegmentCursorRect == Rectangle.Empty)) ||
				_waveControl.IsPlaying ||
				ViewModel.GetIsAnnotationPlaying() ||
				ViewModel.GetIsRecording())
			{
				// Next time it does get painted, make sure it gets drawn in the "on" state.
				_cursorBlinkTimer.Tag = true;
				return;
			}

			_cursorBlinkTimer.Tag = !(bool)_cursorBlinkTimer.Tag;
			_waveControl.InvalidateIfNeeded(newSegmentCursorRect);
			_waveControl.InvalidateIfNeeded(GetReadyToRecordCursorRectangle());
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlPostPaint(PaintEventArgs e)
		{
			var playButtonRects = GetPlayButtonRectanglesForSegmentMouseIsOver();

			if (playButtonRects != null)
			{
				DrawPlayButtonInSegment(e.Graphics, playButtonRects.Item1);
				DrawPlayButtonInSegment(e.Graphics, playButtonRects.Item2);
			}

			DrawRerecordButtonInSegment(e.Graphics);

			if (ViewModel.GetIsRecording())
				DrawTextInAnnotationWaveCellWhileRecording(e.Graphics);
			else if (ViewModel.GetIsRecorderInErrorState())
				_pictureRecording.Visible = false;
			else if (_spaceBarMode == SpaceBarMode.Record && (bool)_cursorBlinkTimer.Tag)
			{
				var rc = GetReadyToRecordCursorRectangle();
				if (rc != Rectangle.Empty)
				{
					using (var brush = new SolidBrush(_labelRecordButton.ForeColor))
						e.Graphics.FillRectangle(brush, rc);
				}
			}

			DrawNewSegmentCursor(e.Graphics);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawNewSegmentCursor(Graphics g)
		{
			var rc = GetNewSegmentCursorRectangle();
			if (rc == Rectangle.Empty)
				return;

			using (var lightPen = new Pen(ColorHelper.CalculateColor(Color.White, _labelListenButton.ForeColor, 75)))
			using (var darkPen = new Pen(_labelListenButton.ForeColor))
			{
				var showThickCursor = (bool)_cursorBlinkTimer.Tag || _spaceBarMode == SpaceBarMode.Record;
				var pen = showThickCursor ? darkPen : lightPen;

				g.DrawLine(pen, rc.X + 1, 0, rc.X + 1, rc.Height);
				if (showThickCursor)
				{
					g.DrawLine(lightPen, rc.X, 0, rc.X, rc.Height);
					g.DrawLine(lightPen, rc.X + 2, 0, rc.X + 2, rc.Height);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DrawHighlightedBorderForRecording(Graphics g, Rectangle rc)
		{
			if (_labelRecordButton.ClientRectangle.Contains(_labelRecordButton.PointToClient(MousePosition)) ||
				ViewModel.GetIsRecording())
			{
				using (var pen = new Pen(_labelRecordButton.ForeColor))
				{
					var rcHighlight = rc;
					rcHighlight.Y--;
					rcHighlight.Width--;
					rcHighlight.Inflate(-1, -1);
					g.DrawRectangle(pen, rcHighlight);
					rcHighlight.Inflate(-1, -1);
					g.DrawRectangle(pen, rcHighlight);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DrawTextInAnnotationWaveCellWhileRecording(Graphics g)
		{
			var rc = GetVisibleAnnotationRectangleForSegmentBeingRecorded();
			DrawHighlightedBorderForRecording(g, rc);
			rc.Inflate(-5, -5);
			rc.X += (_pictureRecording.Width + 6);
			rc.Width -= (_pictureRecording.Width + 6);

			var text = LocalizationManager.GetString(
				"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingAnnotationMsg",
				"Recording...\r\nLength: {0}");

			text = string.Format(text, MediaPlayerViewModel.MakeTimeString((float)_elapsedRecordingTime.TotalSeconds));

			TextRenderer.DrawText(g, text, _annotationSegmentFont, rc, Color.Black,
				TextFormatFlags.WordBreak | TextFormatFlags.WordEllipsis);
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetNewSegmentCursorRectangle()
		{
			if (_spaceBarMode == SpaceBarMode.Done || ViewModel.CurrentUnannotatedSegment != null)
				return Rectangle.Empty;
			var x = _waveControl.Painter.ConvertTimeToXCoordinate(ViewModel.NewSegmentEndBoundary);
			return new Rectangle(x - 1, 0, 3, _waveControl.ClientSize.Height - _waveControl.BottomReservedAreaHeight);
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetReadyToRecordCursorRectangle()
		{
			if (!ViewModel.GetSelectedSegmentIsLongEnough())
				return Rectangle.Empty;

			var rc = _waveControl.Painter.GetBottomReservedRectangleForTimeRange(ViewModel.GetSelectedTimeRange());

			if (rc != Rectangle.Empty)
				rc.Width = 5;
			return rc;
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetNewSegmentRectangle()
		{
			if (!ViewModel.GetHasNewSegment())
				return Rectangle.Empty;

			var x1 = _waveControl.Painter.ConvertTimeToXCoordinate(ViewModel.GetEndOfLastSegment());
			var x2 = _waveControl.Painter.ConvertTimeToXCoordinate(ViewModel.NewSegmentEndBoundary);
			return new Rectangle(x1, 0, x2 - x1 + 1, _waveControl.ClientSize.Height);
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetVisibleAnnotationRectangleForSegmentBeingRecorded()
		{
			var rc = (_segmentBeingRecorded == null ? Rectangle.Empty :
				_waveControl.Painter.GetBottomReservedRectangleForTimeRange(_segmentBeingRecorded));

			if (rc.X < 0)
			{
				rc.Width = rc.Right;
				rc.X = 0;
			}

			return rc;
		}

		/// ------------------------------------------------------------------------------------
		private void DrawPlayButtonInSegment(Graphics g, Rectangle rc)
		{
			if (rc == Rectangle.Empty)
				return;

			var img = _normalPlayInSegmentButton;
			var mousePos = _waveControl.PointToClient(MousePosition);

			if (rc.Contains(mousePos))
				img = _hotPlayInSegmentButton;

			g.DrawImage(img, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawRerecordButtonInSegment(Graphics g)
		{
			var rerecordButtonRect = GetRerecordButtonRectangleForSegmentMouseIsOver();
			if (rerecordButtonRect == Rectangle.Empty)
				return;

			var mousePos = _waveControl.PointToClient(MousePosition);

			var img = rerecordButtonRect.Contains(mousePos) ? _hotRerecordAnnotationButton :
				_normalRerecordAnnotationButton;

			g.DrawImage(img, rerecordButtonRect);
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetRectangleAndIndexOfHotSegment(out Segment segment)
		{
			segment = GetHighlightedSegment();

			return (segment == null ? Rectangle.Empty :
				_waveControl.Painter.GetFullRectangleForTimeRange(segment.TimeRange));
		}

		/// ------------------------------------------------------------------------------------
		private Tuple<Rectangle, Rectangle> GetPlayButtonRectanglesForSegmentMouseIsOver()
		{
			var playButtonSize = _normalPlayInSegmentButton.Size;

			if (ViewModel.GetIsRecording())
				return null;

			if (GetHighlightedSegment() == null)
			{
				var newSegmentRectangle = GetNewSegmentRectangle();
				if (!newSegmentRectangle.Contains(_waveControl.PointToClient(MousePosition)))
					return null;

				return new Tuple<Rectangle, Rectangle>(new Rectangle(newSegmentRectangle.X + 6,
					newSegmentRectangle.Bottom - _waveControl.BottomReservedAreaHeight - 5 - playButtonSize.Height,
					playButtonSize.Width, playButtonSize.Height), Rectangle.Empty);
			}

			Segment hotSegment;
			var rc = GetRectangleAndIndexOfHotSegment(out hotSegment);

			if (rc.IsEmpty || playButtonSize.Width + 6 > rc.Width)
				return null;

			var sourceRecordingButtonRect = new Rectangle(rc.X + 6,
				rc.Bottom - _waveControl.BottomReservedAreaHeight - 5 - playButtonSize.Height,
				playButtonSize.Width, playButtonSize.Height);

			var annotationRecordingButtonRect = (!ViewModel.GetDoesSegmentHaveAnnotationFile(hotSegment) ?
				Rectangle.Empty : new Rectangle(rc.X + 6, rc.Bottom - 5 - playButtonSize.Height,
				playButtonSize.Width, playButtonSize.Height));

			return new Tuple<Rectangle, Rectangle>(sourceRecordingButtonRect, annotationRecordingButtonRect);
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetRerecordButtonRectangleForSegmentMouseIsOver()
		{
			if ((ViewModel.GetIsRecording() && !_reRecording) || GetHighlightedSegment() == null ||
				_waveControl.IsPlaying || ViewModel.GetIsAnnotationPlaying())
			{
				return Rectangle.Empty;
			}

			Segment hotSegment;
			var rc = GetRectangleAndIndexOfHotSegment(out hotSegment);
			var rerecordButtonSize = _normalRerecordAnnotationButton.Size;

			if (rc.IsEmpty || rerecordButtonSize.Width + 6 > rc.Width ||
				(!ViewModel.GetDoesSegmentHaveAnnotationFile(hotSegment) && !ViewModel.GetIsRecording()))
			{
				return Rectangle.Empty;
			}

			return new Rectangle(rc.Right - 6 - rerecordButtonSize.Width,
				rc.Bottom - 5 - rerecordButtonSize.Height,
				rerecordButtonSize.Width, rerecordButtonSize.Height);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaButtonTableLayoutPaint(object sender, PaintEventArgs e)
		{
			var rc = _tableLayoutMediaButtons.ClientRectangle;

			using (var pen = new Pen(Settings.Default.BarColorBorder))
			{
				e.Graphics.DrawLine(pen, rc.X, rc.Y, rc.X, rc.Bottom);
				e.Graphics.DrawLine(pen, rc.Right - 1, rc.Y, rc.Right - 1, rc.Bottom);
			}
		}

		#endregion

		#region Annotation Listen/Erase/Record button handling
		/// ------------------------------------------------------------------------------------
		private void ScrollInPreparationForListenOrRecord(Label button)
		{
			if (ViewModel.CurrentUnannotatedSegment == null)
			{
				if (_waveControl.GetCursorTime() != ViewModel.NewSegmentEndBoundary)
					_waveControl.SetCursor(ViewModel.NewSegmentEndBoundary);

				var endOfLastSegment = ViewModel.GetEndOfLastSegment();

				var targetTime = (button == _labelListenButton ? ViewModel.NewSegmentEndBoundary : endOfLastSegment);

				_waveControl.EnsureTimeIsVisible(targetTime,
					new TimeRange(endOfLastSegment, _waveControl.WaveStream.TotalTime),
					true, button == _labelListenButton);
			}
			else
			{
				if (_waveControl.GetCursorTime() != ViewModel.CurrentUnannotatedSegment.TimeRange.Start)
					_waveControl.SetCursor(ViewModel.CurrentUnannotatedSegment.TimeRange.Start);

				var targetTime = (button == _labelListenButton ?
					ViewModel.CurrentUnannotatedSegment.TimeRange.End : ViewModel.CurrentUnannotatedSegment.TimeRange.Start);

				_waveControl.EnsureTimeIsVisible(targetTime,
					ViewModel.CurrentUnannotatedSegment.TimeRange, true, button == _labelListenButton);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void InvalidateBottomReservedRectangleForCurrentUnannotatedSegment()
		{
			if (ViewModel.CurrentUnannotatedSegment != null)
			{
				_waveControl.InvalidateIfNeeded(GetBottomReservedRectangleForSegment(
					ViewModel.CurrentUnannotatedSegment));
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleListenOrRecordButtonMouseEnter(Label button, Image hotImage)
		{
			button.Image = hotImage;
			if (_waveControl.IsPlaying || ViewModel.GetIsAnnotationPlaying() || ViewModel.GetIsRecording())
				return;

			_scrollTimer.Stop();
			_scrollTimer.Tag = button;
			_scrollTimer.Start();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleListenToSourceMouseDown(object sender, MouseEventArgs e)
		{
			if (_waveControl.GetCursorTime() == ViewModel.OrigWaveStream.TotalTime)
				return;

			if (_waveControl.IsPlaying || ViewModel.GetIsAnnotationPlaying())
			{
				if (_waveControl.IsPlaying)
					_waveControl.Stop();
				else
					ViewModel.StopAnnotationPlayback();

				ScrollInPreparationForListenOrRecord(_labelListenButton);
			}

			_playingBackUsingHoldDownButton = true;

			if (ViewModel.CurrentUnannotatedSegment == null)
				_waveControl.Play(ViewModel.NewSegmentEndBoundary);
			else
			{
				if (ViewModel.CurrentUnannotatedSegment.TimeRange.Contains(_waveControl.GetCursorTime()))
					_waveControl.Play(_waveControl.GetCursorTime(), ViewModel.CurrentUnannotatedSegment.TimeRange.End);
				else
					_waveControl.Play(ViewModel.CurrentUnannotatedSegment.TimeRange);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void HandleRecordingError(Exception e)
		{
			_spaceKeyIsDown = false;
			FinishRecording(false);
			_waveControl.Invalidate();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		public void HandlePlaybackError(Exception e)
		{
			_spaceKeyIsDown = false;
			_waveControl.Stop();
			_waveControl.Invalidate();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordAnnotationMouseDown(object sender, MouseEventArgs e)
		{
			if (!ViewModel.Recorder.GetIsInErrorState(true))
				BeginRecording(ViewModel.GetSelectedTimeRange());
		}

		/// ------------------------------------------------------------------------------------
		private void BeginRecording(TimeRange timeRangeOfSourceBeingAnnotated)
		{
			Segment hotSegment;
			var rcHot = GetRectangleAndIndexOfHotSegment(out hotSegment);

			if (!ViewModel.BeginAnnotationRecording(timeRangeOfSourceBeingAnnotated))
			{
				_spaceKeyIsDown = false;
				return;
			}

			KillSegTooShortMsgTimer();

			UpdateDisplay();

			if (ViewModel.CurrentUnannotatedSegment != null)
			{
				_waveControl.InvalidateIfNeeded(_waveControl.Painter.GetFullRectangleForTimeRange(
					ViewModel.CurrentUnannotatedSegment.TimeRange));
			}

			if (hotSegment != null && hotSegment != ViewModel.CurrentUnannotatedSegment)
				_waveControl.InvalidateIfNeeded(rcHot);

			_waveControl.SelectSegmentOnMouseOver = false;
			_segmentBeingRecorded = timeRangeOfSourceBeingAnnotated.Copy();

			var rc = GetVisibleAnnotationRectangleForSegmentBeingRecorded();
			rc.Inflate(-5, -5);
			_pictureRecording.Location = rc.Location;
			_pictureRecording.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleAnnotationRecordingProgress(TimeSpan elapsedRecordedTime)
		{
			_elapsedRecordingTime = elapsedRecordedTime;

			Invoke((Action<Rectangle>)_waveControl.InvalidateIfNeeded,
				GetVisibleAnnotationRectangleForSegmentBeingRecorded());
		}

		/// ------------------------------------------------------------------------------------
		private void GoToNextUnannotatedSegment()
		{
			if (ViewModel.SetNextUnannotatedSegment())
			{
				var timeRange = ViewModel.CurrentUnannotatedSegment.TimeRange;
				_waveControl.SetSelectionTimes(timeRange, _selectedSegmentHighlighColor);
				_waveControl.EnsureTimeIsVisible(timeRange.Start, timeRange, true, false);
			}
			else
			{
				_waveControl.SetSelectionTimes(new TimeRange(TimeSpan.Zero, TimeSpan.Zero),
					_selectedSegmentHighlighColor);

				if (!ViewModel.IsFullySegmented)
				{
					var timeRange = new TimeRange(ViewModel.GetEndOfLastSegment(), ViewModel.OrigWaveStream.TotalTime);
					_waveControl.SetCursor(timeRange.Start);
					_waveControl.EnsureTimeIsVisible(timeRange.Start, timeRange, true, false);
					_waveControl.InvalidateIfNeeded(GetNewSegmentCursorRectangle());
				}
			}
		}

		#endregion

		#region Low level keyboard handling
		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyDown(Keys key)
		{
			if (!ContainsFocus || _waveControl.IsBoundaryMovingInProgress)
				return true;

			if (key == Keys.Space)
			{
				if (_spaceKeyIsDown || IsBoundaryMovingInProgressUsingArrowKeys)
					return true;

				_spaceKeyIsDown = true;

				if (_spaceBarMode == SpaceBarMode.Record && _labelRecordHint.Visible)
					HandleRecordAnnotationMouseDown(null, null);
				else if (_spaceBarMode == SpaceBarMode.Listen && _labelListenHint.Visible)
					HandleListenToSourceMouseDown(null, null);

				return true;
			}

			if ((key == Keys.Escape || key == Keys.End) && !_waveControl.IsPlaying)
			{
				_waveControl.SetCursor(ViewModel.GetEndOfLastSegment());
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
				if (!IsBoundaryMovingInProgressUsingArrowKeys && _spaceKeyIsDown)
				{
					_spaceKeyIsDown = false;

					if (_playingBackUsingHoldDownButton)
					{
						_newSegmentDefinedBy = SegmentDefinitionMode.HoldingSpace;
						FinishListeningUsingEarOrSpace();
					}
					else if (!_reRecording && ViewModel.GetIsRecording())
						FinishRecording(true);
				}

				return true;
			}

			return base.OnLowLevelKeyUp(key);
		}

		#endregion
	}
}
