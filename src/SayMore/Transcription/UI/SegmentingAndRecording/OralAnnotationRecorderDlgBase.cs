using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Localization;
using Localization.UI;
using Palaso.Media.Naudio;
using SayMore.Media;
using SayMore.Media.UI;
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
		private Timer _segTooShortMsgTimer;
		private TimeSpan _currentMovingBoundaryTime;
		private Image _hotPlayInSegmentButton;
		private Image _hotPlayOriginalButton;
		private Image _hotRecordAnnotationButton;
		private Image _normalPlayInSegmentButton;
		private Image _normalPlayOriginalButton;
		private Image _normalRecordAnnotationButton;
		private Image _normalRerecordAnnotationButton;
		private Image _hotRerecordAnnotationButton;

		private TimeSpan _elapsedRecordingTime;
		private TimeSpan _endOfTempSegment;
		private TimeSpan _annotationPlaybackLength;
		private TimeSpan _lastAnnotationPlaybackPosition;
		private Segment _segmentWhoseAnnotationIsBeingPlayedBack;
		private Font _annotationSegmentFont;
		private TimeRange _segmentBeingRecorded;
		private bool _spaceKeyIsDown;
		private bool _playingBackUsingHoldDownButton;
		private bool _reRecording;
		private SpaceBarMode _spaceBarMode;
		//private readonly Color _unannotatedSegmentHighlighColor = Color.FromArgb(90, 0xC0, 0x42, 0x00);
		private readonly Color _unannotatedSegmentHighlighColor = Color.FromArgb(90, Color.Orange);

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

			Padding = new Padding(0);
			_cursorBlinkTimer.Enabled = true;
			_cursorBlinkTimer.Tag = true;

			_scrollTimer.Tick += delegate
			{
				_scrollTimer.Stop();
				ScrollInPreparationForListenOrRecord((Label)_scrollTimer.Tag);
			};

			InitializeListenAndRecordButtonEvents();

			_endOfTempSegment = ViewModel.GetEndOfLastSegment();
			_toolStripStatus.Visible = false;
			InitializeTableLayouts();

			_spaceBarMode = viewModel.GetIsFullyAnnotated() ? SpaceBarMode.Done : SpaceBarMode.Listen;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
				_annotationSegmentFont.Dispose();
				_hotPlayOriginalButton.Dispose();
				_hotRecordAnnotationButton.Dispose();
				LocalizeItemDlg.StringsLocalized -= HandleStringsLocalized;
			}

			base.Dispose(disposing);
		}

		#region Initialization methods
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			InitializeInfoLabels();
			ViewModel.RemoveInvalidAnnotationFiles();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (_moreReliableDesignMode || ViewModel.TimeTier.Segments.Count <= 0)
				return;

			if (ViewModel.SetNextUnannotatedSegment())
				_waveControl.SetSelectionTimes(ViewModel.CurrentUnannotatedSegment.TimeRange, _unannotatedSegmentHighlighColor);

			ScrollInPreparationForListenOrRecord(_labelListenButton);
			UpdateDisplay();

			//ViewModel.SelectSegmentFromTime(ViewModel.TimeTier.Segments[0].TimeRange.End);
			//_waveControl.SetSelectionTimes(ViewModel.TimeTier.Segments[0].TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeListenAndRecordButtonEvents()
		{
			_labelListenButton.MouseEnter += delegate
			{
				HandleListenOrRecordButtonMouseEnter(_labelListenButton, _hotPlayOriginalButton);
			};

			_labelListenButton.MouseLeave += delegate
			{
				_labelListenButton.Image = _normalPlayOriginalButton;
				_scrollTimer.Stop();
			};

			_labelListenButton.MouseUp += delegate { FinishListeningUsingEarOrSpace(); };

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
			_spaceBarMode = SpaceBarMode.Record;

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeTableLayouts()
		{
			_tableLayoutTop.Visible = false;

			_tableLayoutSegmentInfo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			_tableLayoutSegmentInfo.AutoSize = true;
			_tableLayoutSegmentInfo.Anchor = AnchorStyles.Left | AnchorStyles.Top;
			_tableLayoutButtons.Controls.Add(_tableLayoutSegmentInfo, 0, 0);

			_tableLayoutMediaButtons.Dock = DockStyle.Left;
			_panelWaveControl.Controls.Add(_tableLayoutMediaButtons);
			_tableLayoutMediaButtons.BackColor = Settings.Default.BarColorBegin;
			_tableLayoutMediaButtons.RowStyles[0].SizeType = SizeType.AutoSize;
			_tableLayoutMediaButtons.RowStyles[_tableLayoutMediaButtons.RowCount - 1].SizeType = SizeType.Absolute;
			_tableLayoutMediaButtons.Controls.Add(_labelOriginalRecording, 0, 0);
			_labelOriginalRecording.TextAlign = ContentAlignment.TopCenter;
			_labelOriginalRecording.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			var margin = _labelOriginalRecording.Margin;
			margin.Top = 10;
			_labelOriginalRecording.Margin = margin;
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeWaveControlContextActionImages()
		{
			if (_moreReliableDesignMode)
				return;

			_normalPlayInSegmentButton = Resources.ListenToSegmentsAnnotation;
			_normalPlayOriginalButton = _labelListenButton.Image;
			_normalRecordAnnotationButton = _labelRecordButton.Image;
			_normalRerecordAnnotationButton = Resources.RerecordOralAnnotation;
			_hotPlayInSegmentButton = PaintingHelper.MakeHotImage(_normalPlayInSegmentButton);
			_hotPlayOriginalButton = PaintingHelper.MakeHotImage(_normalPlayOriginalButton);
			_hotRecordAnnotationButton = PaintingHelper.MakeHotImage(_normalRecordAnnotationButton);
			_hotRerecordAnnotationButton = PaintingHelper.MakeHotImage(_normalRerecordAnnotationButton);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeInfoLabels()
		{
			_labelTotalDuration.Font = FontHelper.MakeFont(SystemFonts.MenuFont, 8);
			_labelTotalSegments.Font = _labelTotalDuration.Font;
			_labelHighlightedSegment.Font = _labelTotalDuration.Font;
			_labelListenHint.Font = _labelOriginalRecording.Font;
			_labelRecordHint.Font = _labelOriginalRecording.Font;

			_labelListenHint.Text = LocalizationManager.GetString(
				"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._labelListenHint",
				"Listen\r\n(Press and hold\r\nSPACE key)", null, _labelListenHint);

			_labelRecordHint.Text = LocalizationManager.GetString(
				"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._labelRecordHint",
				"Record\r\n(Press and hold\r\nSPACE key)", null, _labelRecordHint);

			_annotationSegmentFont = FontHelper.MakeFont(SystemFonts.MenuFont, 8, FontStyle.Bold);

			LocalizeItemDlg.StringsLocalized += HandleStringsLocalized;
		}

		/// ------------------------------------------------------------------------------------
		protected override WaveControlBasic CreateWaveControl()
		{
			_waveControl = new 	WaveControlWithRangeSelection();
			_waveControl.BottomReservedAreaBorderColor = Settings.Default.DataEntryPanelColorBorder;
			_waveControl.BottomReservedAreaColor = Color.FromArgb(130, Settings.Default.DataEntryPanelColorBegin);
			_waveControl.BottomReservedAreaPaintAction = HandlePaintingAnnotatedWaveArea;
			_waveControl.PostPaintAction = HandleWaveControlPostPaint;
			_waveControl.MouseMove += HandleWaveControlMouseMove;
			_waveControl.MouseLeave += HandleWaveControlMouseLeave;
			_waveControl.MouseClick += HandleWaveControlMouseClick;
			_waveControl.MouseDown += HandleWaveControlMouseDown;
			_waveControl.MouseUp += delegate { FinishRecording(false); };
			_waveControl.BoundaryMoved += HandleSegmentBoundaryMovedInWaveControl;
			_waveControl.PlaybackStarted += delegate { KillSegTooShortMsgTimer(); };

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
		protected virtual string ReadyToRecordMessage
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			ViewModel.CloseAnnotationPlayer();
			ViewModel.CloseAnnotationRecorder();
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			base.HandleStringsLocalized();

			_labelTotalDuration.Tag = _labelTotalDuration.Text;
			_labelTotalSegments.Tag = _labelTotalSegments.Text;
			_labelHighlightedSegment.Tag = _labelHighlightedSegment.Text;
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
			_labelListenButton.Image = (_waveControl.IsPlaying && _playingBackUsingHoldDownButton ?
				Resources.ListenToOriginalRecordingDown : Resources.ListenToOriginalRecording);

			_labelRecordButton.Image = (ViewModel.GetIsRecording() ?
				Resources.RecordingOralAnnotationInProgress : Resources.RecordOralAnnotation);

			_labelListenButton.Enabled = !ViewModel.GetIsRecording();

			_labelRecordButton.Enabled =
				(ViewModel.CurrentUnannotatedSegment != null || _endOfTempSegment > ViewModel.GetEndOfLastSegment()) &&
				!_waveControl.IsPlaying && !ViewModel.GetIsAnnotationPlaying();

			_labelListenHint.Visible = _spaceBarMode == SpaceBarMode.Listen && _labelListenButton.Enabled;
			_labelRecordHint.Visible = _spaceBarMode == SpaceBarMode.Record && _labelRecordButton.Enabled && !_reRecording;

			Utils.SetWindowRedraw(_tableLayoutSegmentInfo, false);

			_labelTotalDuration.Text = string.Format((string)_labelTotalDuration.Tag,
				MediaPlayerViewModel.MakeTimeString((float)ViewModel.OrigWaveStream.TotalTime.TotalSeconds));

			_labelTotalSegments.Text = string.Format((string)_labelTotalSegments.Tag,
				ViewModel.TimeTier.Segments.Count);

			//	var currentSegment = GetHighlightedSegment();
			_labelHighlightedSegment.Visible = false; //				(currentSegment != null);

			//if (currentSegment != null)
			//{
			//    _labelHighlightedSegment.Text = string.Format((string)_labelHighlightedSegment.Tag,
			//        ViewModel.TimeTier.GetIndexOfSegment(currentSegment) + 1,
			//        MediaPlayerViewModel.MakeTimeString(currentSegment.Start),
			//        MediaPlayerViewModel.MakeTimeString(currentSegment.End));

			//    // Code for displaying start time and duration of a segment
			//    //_labelSegmentStart.Text = string.Format((string)_labelSegmentStart.Tag,
			//    //    MediaPlayerViewModel.MakeTimeString(currentSegment.Start));
			//    //_labelSegmentDuration.Text = string.Format((string)_labelSegmentDuration.Tag,
			//    //    MediaPlayerViewModel.MakeTimeString(currentSegment.End - currentSegment.Start));
			//}

			Utils.SetWindowRedraw(_tableLayoutSegmentInfo, true);

			base.UpdateDisplay();
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

			AdjustPotentialNewSegmentEndBoundaryOnArrowKey(milliseconds);
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
			var newEndTime = _endOfTempSegment + TimeSpan.FromMilliseconds(milliseconds);
			if (newEndTime < ViewModel.GetEndOfLastSegment())
				return;

			_cursorBlinkTimer.Tag = false;
			_cursorBlinkTimer.Enabled = false;

			// At this point, we know we're adjusting the selected region beyond the last segment.
			// I.e. one the user is preparing to record an annotation for.
			var oldEndTime = _endOfTempSegment;
			_endOfTempSegment = newEndTime;
			_waveControl.InvalidateRegionBetweenTimes((milliseconds > 0 ? oldEndTime : _endOfTempSegment),
				(milliseconds > 0 ? _endOfTempSegment : oldEndTime));

			_waveControl.SetCursor(_endOfTempSegment);
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
			if (_currentMovingBoundaryTime != TimeSpan.Zero)
			{
				ViewModel.SegmentBoundaryMoved(_timeAtBeginningOfboundaryMove, _currentMovingBoundaryTime);
				PlaybackShortPortionUpToBoundary(_currentMovingBoundaryTime);
			}
			else
			{
				_cursorBlinkTimer.Tag = false;
				_cursorBlinkTimer.Enabled = false;
				_waveControl.InvalidateIfNeeded(GetTempCursorRectangle());
				PlaybackShortPortionUpToBoundary(_waveControl.GetCursorTime());
			}

			_currentMovingBoundaryTime = TimeSpan.Zero;
			base.FinalizeBoundaryMovedUsingArrowKeys();
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
			if (current > endOfLastSegment && current > _endOfTempSegment)
			{
				_endOfTempSegment = current;
				_waveControl.InvalidateIfNeeded(GetTempSegmentRectangle());
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPlaybackStopped(WaveControlBasic ctrl, TimeSpan start, TimeSpan end)
		{
			base.OnPlaybackStopped(ctrl, start, end);

			if (GetHighlightedSegment() != null)
				_waveControl.SetCursor(TimeSpan.FromSeconds(1).Negate());

			if (end > ViewModel.GetEndOfLastSegment())
			{
				var rc1 = GetTempCursorRectangle();
				_endOfTempSegment = end;
				var rc2 = GetTempSegmentRectangle();
				rc2.Inflate(rc1.Width / 2, 0);
				_waveControl.InvalidateIfNeeded(rc2);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override TimeSpan GetBoundaryToAdjustOnArrowKeys()
		{
			if (_endOfTempSegment > ViewModel.GetEndOfLastSegment())
				return _endOfTempSegment;

			return base.GetBoundaryToAdjustOnArrowKeys();

			// REVIEW: How to adjust existing boundaries using arrow keys?
			//return (ViewModel.HighlightedSegment == null ?
			//    _waveControl.GetCursorTime() : ViewModel.GetEndOfCurrentSegment());
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

			bool playOriginal = playButtonRectangles.Item1.Contains(e.Location);
			bool playAnnotation = playButtonRectangles.Item2.Contains(e.Location);
			if (!playOriginal && !playAnnotation)
				return;

			var segMouseOver = _waveControl.GetSegmentForX(e.X);

			if (ViewModel.GetIsAnnotationPlaying())
			{
				ViewModel.StopAnnotationPlayback();
				_lastAnnotationPlaybackPosition = TimeSpan.Zero;
				_waveControl.InvalidateIfNeeded(GetAnnotationPlaybackRectangle());
			}

			if (segMouseOver < 0)
			{
				// Play the original recording for our temp segment.
				_waveControl.Play(ViewModel.GetEndOfLastSegment(), _endOfTempSegment);
			}
			else
			{
				var segment = ViewModel.TimeTier.Segments[segMouseOver];

				if (playOriginal)
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
			var timeRangeOfHighlightedSegment = ((WavePainterWithRangeSelection)_waveControl.Painter).DefaultSelectedRange;
			return ViewModel.TimeTier.Segments.FirstOrDefault(s => s.TimeRange == timeRangeOfHighlightedSegment);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlMouseDown(object sender, MouseEventArgs e)
		{
			if (!GetRerecordButtonRectangleForSegmentMouseIsOver().Contains(e.Location))
				return;

			var segMouseOver = GetHighlightedSegment();
			ViewModel.TemporarilySaveAnnotationBeingRerecorded(segMouseOver.TimeRange);
			ViewModel.EraseAnnotation(segMouseOver);
			_reRecording = true;
			BeginRecording(segMouseOver.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		private void FinishRecording(bool advanceToNextUnannotatedSegment)
		{
			if (!ViewModel.GetIsRecording())
				return;

			_pictureRecording.Visible = false;
			_waveControl.SelectSegmentOnMouseOver = true;

			if (_reRecording)
			{
				var rc = _waveControl.Painter.GetBottomReservedRectangleForTimeRange(GetOrangeTimeRange());
				_waveControl.InvalidateIfNeeded(rc);
				_reRecording = false;
			}

			if (!ViewModel.StopAnnotationRecording(_segmentBeingRecorded))
			{
				DisplayRecordingTooShortMessage();
				_segmentBeingRecorded = null;
				return;
			}

			_waveControl.InvalidateIfNeeded(GetVisibleAnnotationRectangleForSegmentBeingRecorded());
			_segmentBeingRecorded = null;

			if (ViewModel.CurrentUnannotatedSegment == null)
			{
				_waveControl.SegmentBoundaries = ViewModel.InsertNewBoundary(_endOfTempSegment);
				_waveControl.SetCursor(TimeSpan.FromSeconds(1).Negate());
			}

			if (advanceToNextUnannotatedSegment)
				GoToNextUnannotatedSegment();

			_spaceBarMode = ViewModel.GetIsFullyAnnotated() ? SpaceBarMode.Done : SpaceBarMode.Listen;

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void KillSegTooShortMsgTimer()
		{
			if (_segTooShortMsgTimer != null)
			{
				var rc = _waveControl.Painter.GetBottomReservedRectangleForTimeRange((TimeRange)_segTooShortMsgTimer.Tag);
				_segTooShortMsgTimer.Stop();
				_segTooShortMsgTimer.Dispose();
				_segTooShortMsgTimer = null;

				_waveControl.InvalidateIfNeeded(rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DisplayRecordingTooShortMessage()
		{
			KillSegTooShortMsgTimer();

			_segTooShortMsgTimer = new Timer();
			_segTooShortMsgTimer.Interval = 4000;
			_segTooShortMsgTimer.Tag = _segmentBeingRecorded;
			_segTooShortMsgTimer.Tick += delegate { KillSegTooShortMsgTimer(); };
			_segTooShortMsgTimer.Start();
			_waveControl.InvalidateIfNeeded(GetVisibleAnnotationRectangleForSegmentBeingRecorded());
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

			for (int i = 0; i < segRects.Length; i++)
			{
				var rc = new Rectangle(segRects[i].X, areaRectangle.Y + 1,
					segRects[i].Width, areaRectangle.Height - 1);

				if (!areaRectangle.IntersectsWith(rc))
					continue;

				var segment = ViewModel.TimeTier.Segments[i];
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

				if (_segmentBeingRecorded == segment.TimeRange && ViewModel.GetIsRecording())
					continue;

				DrawOralAnnotationWave(e, rc, segment);
				DrawCursorInOralAnnotationWave(e, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DrawOralAnnotationWave(PaintEventArgs e, Rectangle rc, Segment segment)
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
			var tempCursorRect = GetTempCursorRectangle();
			if (tempCursorRect == Rectangle.Empty || _waveControl.IsPlaying ||
				ViewModel.GetIsAnnotationPlaying() || ViewModel.GetIsRecording())
			{
				// Next time it does get painted, make sure it gets drawn in the "on" state.
				_cursorBlinkTimer.Tag = true;
				return;
			}

			_cursorBlinkTimer.Tag = !(bool)_cursorBlinkTimer.Tag;
			_waveControl.InvalidateIfNeeded(tempCursorRect);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlPostPaint(PaintEventArgs e)
		{
			var rc = GetTempSegmentRectangle();
			if (rc != Rectangle.Empty && rc.IntersectsWith(e.ClipRectangle))
			{
				using (var br = new SolidBrush(_unannotatedSegmentHighlighColor))
					e.Graphics.FillRectangle(br, rc);
			}

			var playButtonRects = GetPlayButtonRectanglesForSegmentMouseIsOver();

			if (playButtonRects != null)
			{
				DrawPlayButtonInSegment(e.Graphics, playButtonRects.Item1);
				DrawPlayButtonInSegment(e.Graphics, playButtonRects.Item2);
			}

			DrawRerecordButtonInSegment(e.Graphics);

			if (ViewModel.GetIsRecording())
				DrawTextInAnnotationWaveCellWhileRecording(e.Graphics);
			else
				DrawTextInAnnotationWaveCellWhileNotRecording(e.Graphics);

			var cursorRect = GetTempCursorRectangle();
			if (cursorRect != Rectangle.Empty)
			{
				using (var pen = new Pen(Settings.Default.BarColorBorder))
				{
					e.Graphics.DrawLine(pen, cursorRect.X + 1, 0, cursorRect.X + 1, _waveControl.ClientSize.Height);
					if ((bool)_cursorBlinkTimer.Tag)
					{
						e.Graphics.DrawLine(pen, cursorRect.X, 0, cursorRect.X, _waveControl.ClientSize.Height);
						e.Graphics.DrawLine(pen, cursorRect.X + 2, 0, cursorRect.X + 2, _waveControl.ClientSize.Height);
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DrawSegmentTooShortMessage(Graphics g, bool segmentReadyToRecordHadRecordingThatWasTooShort)
		{
			var rc = _waveControl.Painter.GetBottomReservedRectangleForTimeRange((TimeRange)_segTooShortMsgTimer.Tag);
			rc.Inflate(-5, -5);

			var msg = segmentReadyToRecordHadRecordingThatWasTooShort ?
				LocalizationManager.GetString(
					"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingTooShortMessage.WhenSpaceOrMouseIsValid",
					"Whoops. You need to hold down the SPACE bar or mouse button while talking.") :
				LocalizationManager.GetString(
					"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingTooShortMessage.WhenOnlyMouseIsValid",
					"Whoops. You need to hold down the mouse button while talking.");

			TextRenderer.DrawText(g, msg, _annotationSegmentFont, rc, Color.Red,
				TextFormatFlags.WordBreak | TextFormatFlags.WordEllipsis);
		}

		/// ------------------------------------------------------------------------------------
		private TimeRange GetOrangeTimeRange()
		{
			return (ViewModel.CurrentUnannotatedSegment != null) ?
				ViewModel.CurrentUnannotatedSegment.TimeRange :
				new TimeRange(ViewModel.GetEndOfLastSegment(), _endOfTempSegment);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawTextInAnnotationWaveCellWhileNotRecording(Graphics g)
		{
			var timeRangeReadyToRecord = GetOrangeTimeRange();

			bool segmentReadyToRecordHadRecordingThatWasTooShort =
				(_segTooShortMsgTimer != null && timeRangeReadyToRecord == (TimeRange)_segTooShortMsgTimer.Tag);

			if (_segTooShortMsgTimer != null)
				DrawSegmentTooShortMessage(g, segmentReadyToRecordHadRecordingThatWasTooShort);

			if (timeRangeReadyToRecord.DurationSeconds.Equals(0f))
				return;

			if (segmentReadyToRecordHadRecordingThatWasTooShort || _waveControl.IsPlaying ||
				ViewModel.GetIsAnnotationPlaying() || !_labelRecordButton.Enabled)
				return;

			var rc = _waveControl.Painter.GetBottomReservedRectangleForTimeRange(timeRangeReadyToRecord);

			if (rc == Rectangle.Empty)
				return;

			DrawHighlightedBorderForRecording(g, rc);

			rc.Inflate(-5, -5);

			TextRenderer.DrawText(g, ReadyToRecordMessage, _annotationSegmentFont, rc, Color.Black,
				TextFormatFlags.WordBreak | TextFormatFlags.WordEllipsis);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawHighlightedBorderForRecording(Graphics g, Rectangle rc)
		{
			if (_labelRecordButton.ClientRectangle.Contains(_labelRecordButton.PointToClient(MousePosition)) ||
				ViewModel.GetIsRecording())
			{
				using (var pen = new Pen(Settings.Default.BarColorBorder))
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
			//var rc = _pictureRecording.Bounds;
			//rc.Inflate(2, 2);
			//rc.Width--;
			//rc.Height--;
			//g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			//g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			//g.FillEllipse(Brushes.White, rc);
			//g.DrawEllipse(Pens.Black, rc);

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
		private Rectangle GetTempCursorRectangle()
		{
			if (_spaceBarMode == SpaceBarMode.Done || ViewModel.CurrentUnannotatedSegment != null)
				return Rectangle.Empty;
			var x = _waveControl.Painter.ConvertTimeToXCoordinate(_endOfTempSegment);
			return new Rectangle(x - 1, 0, 3, _waveControl.ClientSize.Height);
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetTempSegmentRectangle()
		{
			var timeAtEndOfLastSegment = ViewModel.GetEndOfLastSegment();

			if (_endOfTempSegment == timeAtEndOfLastSegment)
				return Rectangle.Empty;

			var x1 = _waveControl.Painter.ConvertTimeToXCoordinate(timeAtEndOfLastSegment);
			var x2 = _waveControl.Painter.ConvertTimeToXCoordinate(_endOfTempSegment);
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

			if (segment == null)
				return Rectangle.Empty;

			return _waveControl.Painter.GetFullRectangleForTimeRange(segment.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		private Tuple<Rectangle, Rectangle> GetPlayButtonRectanglesForSegmentMouseIsOver()
		{
			var playButtonSize = _normalPlayInSegmentButton.Size;

			if (ViewModel.GetIsRecording())
				return null;

			if (GetHighlightedSegment() == null)
			{
				var tempRect = GetTempSegmentRectangle();
				if (!tempRect.Contains(_waveControl.PointToClient(MousePosition)))
					return null;

				return new Tuple<Rectangle, Rectangle>(new Rectangle(tempRect.X + 6,
					tempRect.Bottom - _waveControl.BottomReservedAreaHeight - 5 - playButtonSize.Height,
					playButtonSize.Width, playButtonSize.Height), Rectangle.Empty);
			}

			Segment hotSegment;
			var rc = GetRectangleAndIndexOfHotSegment(out hotSegment);

			if (rc.IsEmpty || playButtonSize.Width + 6 > rc.Width)
				return null;

			var originalRecordingButtonRect = new Rectangle(rc.X + 6,
				rc.Bottom - _waveControl.BottomReservedAreaHeight - 5 - playButtonSize.Height,
				playButtonSize.Width, playButtonSize.Height);

			var annotationRecordingButtonRect = (!ViewModel.GetDoesSegmentHaveAnnotationFile(hotSegment) ?
				Rectangle.Empty : new Rectangle(rc.X + 6, rc.Bottom - 5 - playButtonSize.Height,
				playButtonSize.Width, playButtonSize.Height));

			return new Tuple<Rectangle, Rectangle>(originalRecordingButtonRect, annotationRecordingButtonRect);
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
				e.Graphics.DrawLine(pen, rc.Right - 1, rc.Y, rc.Right - 1, rc.Bottom);
		}

		#endregion

		#region Annotation Listen/Erase/Record button handling
		/// ------------------------------------------------------------------------------------
		private void ScrollInPreparationForListenOrRecord(Label button)
		{
			if (ViewModel.CurrentUnannotatedSegment == null)
			{
				var endOfLastSegment = ViewModel.GetEndOfLastSegment();

				if (_endOfTempSegment < endOfLastSegment)
					_endOfTempSegment = endOfLastSegment;

				if (_waveControl.GetCursorTime() != _endOfTempSegment)
					_waveControl.SetCursor(_endOfTempSegment);

				var targetTime = (button == _labelListenButton ? _endOfTempSegment : endOfLastSegment);

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

			//var endOfLastSegment = ViewModel.GetEndOfLastSegment();

			//if (_endOfTempSegment < endOfLastSegment)
			//    _endOfTempSegment = endOfLastSegment;

			//if (_waveControl.GetCursorTime() != _endOfTempSegment)
			//    _waveControl.SetCursor(_endOfTempSegment);

			//var targetTime = (button == _labelListenButton ? _endOfTempSegment : endOfLastSegment);

			//_waveControl.EnsureTimeIsVisible(targetTime,
			//    new TimeRange(endOfLastSegment, _waveControl.WaveStream.TotalTime),
			//    true, button == _labelListenButton);
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
		private void HandleListenToOriginalMouseDown(object sender, MouseEventArgs e)
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
				_waveControl.Play(_endOfTempSegment);
			else
				_waveControl.Play(ViewModel.CurrentUnannotatedSegment.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordAnnotationMouseDown(object sender, MouseEventArgs e)
		{
			//if (!ViewModel.GetIsSegmentLongEnough(_waveControl.GetCursorTime()))
			//{
			//    _labelRecordButton.BackColor = Color.Red;
			//    _buttonRecordAnnotation.Text = GetSegmentTooShortText();
			//    return;
			//}

			BeginRecording(GetOrangeTimeRange());
		}

		/// ------------------------------------------------------------------------------------
		private void BeginRecording(TimeRange timeRangeOfOriginalBeingAnnotated)
		{
			var genericRecordingErrorMsg = LocalizationManager.GetString(
				"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.GenericRecordingErrorMessage",
				"There was an error while attempting to begin recording annotation.");

			Segment hotSegment;
			var rcHot = GetRectangleAndIndexOfHotSegment(out hotSegment);

			if (!ViewModel.BeginAnnotationRecording(timeRangeOfOriginalBeingAnnotated,
				HandleAnnotationRecordingProgress, genericRecordingErrorMsg))
			{
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
			_segmentBeingRecorded = timeRangeOfOriginalBeingAnnotated.Copy();

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
				_waveControl.SetSelectionTimes(timeRange, _unannotatedSegmentHighlighColor);
				_waveControl.EnsureTimeIsVisible(timeRange.Start, timeRange, true, false);
			}
			else
			{
				_waveControl.SetSelectionTimes(new TimeRange(TimeSpan.Zero, TimeSpan.Zero),
					_unannotatedSegmentHighlighColor);

				if (!ViewModel.IsFullySegmented)
				{
					var timeRange = new TimeRange(ViewModel.GetEndOfLastSegment(), ViewModel.OrigWaveStream.TotalTime);
					_waveControl.SetCursor(timeRange.Start);
					_waveControl.EnsureTimeIsVisible(timeRange.Start, timeRange, true, false);
					_waveControl.InvalidateIfNeeded(GetTempCursorRectangle());
				}
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
				if (_spaceKeyIsDown)
					return true;

				if (_spaceBarMode == SpaceBarMode.Record && _labelRecordHint.Visible)
					HandleRecordAnnotationMouseDown(null, null);
				else if (_spaceBarMode == SpaceBarMode.Listen && _labelListenHint.Visible)
					HandleListenToOriginalMouseDown(null, null);

				_spaceKeyIsDown = true;
				return true;
			}

			//if (key == Keys.Enter)
			//{
			//    HandleRecordAnnotationMouseDown(null, null);
			//    return true;
			//}

			if ((key == Keys.Escape || key == Keys.End) && !_waveControl.IsPlaying)
			{
				_waveControl.SetCursor(ViewModel.GetEndOfLastSegment());
// REVIEW				_waveControl.ClearSelection();
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

			if (key == Keys.Space && _spaceKeyIsDown)
			{
				_spaceKeyIsDown = false;

				if (_playingBackUsingHoldDownButton)
					FinishListeningUsingEarOrSpace();
				else if (!_reRecording && ViewModel.GetIsRecording())
					FinishRecording(true);

				return true;
			}

			//if (key == Keys.Enter)
			//{
			//    HandleRecordAnnotationMouseUp(null, null);
			//    return true;
			//}

			return base.OnLowLevelKeyUp(key);
		}

		#endregion
	}
}
