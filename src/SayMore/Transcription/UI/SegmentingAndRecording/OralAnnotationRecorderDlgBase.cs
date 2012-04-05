using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Localization;
using Palaso.Media.Naudio;
using SayMore.Media;
using SayMore.Media.UI;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SilTools;
using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationRecorderBaseDlg : SegmenterDlgBase
	{
		//private readonly string _normalRecordButtonText;
		private readonly ToolTip _tooltip = new ToolTip();
		private TimeSpan _currentMovingBoundaryTime;
		private Image _hotPlayOriginalButton;
		private Image _hotRecordAnnotationButton;
		private Image _normalPlayOriginalButton;
		private Image _normalRecordAnnotationButton;
		private Image _normalRerecordAnnotationButton;
		private Image _hotRerecordAnnotationButton;

		private TimeSpan _elapsedRecordingTime;
		private TimeSpan _endOfTempSegment;
		private TimeSpan _annotationPlaybackLength;
		private int _annotationPlaybackCursorX;
		private Rectangle _annotationPlaybackRectangle;
		private Font _annotationSegmentFont;
		private Tuple<TimeSpan, TimeSpan> _segmentBeingRecorded = null;

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
			InitializeInfoLabels();
			_tableLayoutTop.Visible = false;
			_cursorBlinkTimer.Enabled = true;
			_cursorBlinkTimer.Tag = true;

			//_normalRecordButtonText = "TODO: Hold down Space to record";

			_scrollTimer.Tick += delegate
			{
				_scrollTimer.Stop();
				ScrollInPreparationForListenOrRecord((Label)_scrollTimer.Tag);
			};

			_labelListenButton.MouseEnter += delegate
			{
				HandleListenOrRecordButtonMouseEnter(_labelListenButton, _hotPlayOriginalButton);
			};

			_labelListenButton.MouseLeave += delegate
			{
				_labelListenButton.Image = _normalPlayOriginalButton;
				_scrollTimer.Stop();
			};

			_labelListenButton.MouseUp += delegate
			{
				_waveControl.Stop();
			};

			_labelRecordButton.MouseEnter += delegate
			{
				HandleListenOrRecordButtonMouseEnter(_labelRecordButton, _hotRecordAnnotationButton);
			};

			_labelRecordButton.MouseLeave += delegate
			{
				_labelRecordButton.Image = _normalRecordAnnotationButton;
				_scrollTimer.Stop();
			};

			_labelRecordButton.MouseDown += HandleRecordAnnotationMouseDown;
			_labelRecordButton.MouseUp += HandleRecordAnnotationMouseUp;

			//_buttonEraseAnnotation.Click += delegate
			//{
			//    _waveControl.Stop();
			//    ViewModel.EraseAnnotation();
			//    _waveControl.InvalidateSelectedRegion();
			//    UpdateDisplay();
			//};

			_endOfTempSegment = ViewModel.GetEndOfLastSegment();

			_toolStripStatus.Visible = false;

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
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
				_annotationSegmentFont.Dispose();
				_hotPlayOriginalButton.Dispose();
				_hotRecordAnnotationButton.Dispose();
				Localization.UI.LocalizeItemDlg.StringsLocalized -= HandleStringsLocalized;
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeWaveControlContextActionImages()
		{
			if (_moreReliableDesignMode)
				return;

			_normalPlayOriginalButton = _labelListenButton.Image;
			_normalRecordAnnotationButton = _labelRecordButton.Image;
			_normalRerecordAnnotationButton = Resources.RerecordOralAnnotation;
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
			//_labelHighlightedSegment.Font = FontHelper.MakeFont(SystemFonts.MenuFont, 8, FontStyle.Bold);
			//_labelSegmentStart.Font = _labelTotalDuration.Font;
			//_labelSegmentDuration.Font = _labelTotalDuration.Font;

			_annotationSegmentFont = FontHelper.MakeFont(SystemFonts.MenuFont, 8, FontStyle.Bold);

			Localization.UI.LocalizeItemDlg.StringsLocalized += HandleStringsLocalized;
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			base.HandleStringsLocalized();

			_labelTotalDuration.Tag = _labelTotalDuration.Text;
			_labelTotalSegments.Tag = _labelTotalSegments.Text;
			_labelHighlightedSegment.Tag = _labelHighlightedSegment.Text;
			//_labelSegmentStart.Tag = _labelSegmentStart.Text;
			//_labelSegmentDuration.Tag = _labelSegmentDuration.Text;

			UpdateDisplay();
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
			_waveControl.MouseUp += HandleWaveControlMouseUp;
			_waveControl.SelectedRegionChanged += HandleWaveControlSelectedRegionChanged;
			_waveControl.BoundaryMoved += HandleSegmentBoundaryMovedInWaveControl;

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
				ViewModel.SelectSegmentFromTime(boundary);
				_waveControl.SetSelectionTimes(ViewModel.GetStartOfCurrentSegment(), ViewModel.GetEndOfCurrentSegment());
				UpdateDisplay();
			};

			_waveControl.SetCursorAtTimeOnMouseClick += (ctrl, desiredTime) =>
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

			_waveControl.Controls.Add(_pictureRecording);
			InitializeWaveControlContextActionImages();

			return _waveControl;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (DesignMode || ViewModel.TimeTier.Segments.Count <= 0)
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
			if (!_waveControl.IsPlaying && !ViewModel.GetIsAnnotationPlaying())
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
			//if (ViewModel.GetIsRecording())
			//{
			////	_buttonRecordAnnotation.ForeColor = _buttonEraseAnnotation.ForeColor;
			////	_labelRecordButton.Text = LocalizationManager.GetString(
			////		"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingButtonText.WhenRecording",
			////		"Recording...");
			////	return;
			//}

			_labelListenButton.Enabled = !ViewModel.GetIsRecording();

			_labelRecordButton.Enabled = _endOfTempSegment > ViewModel.GetEndOfLastSegment() &&
				!_waveControl.IsPlaying && !ViewModel.GetIsAnnotationPlaying();

			Utils.SetWindowRedraw(_tableLayoutSegmentInfo, false);

			_labelTotalDuration.Text = string.Format((string)_labelTotalDuration.Tag,
				MediaPlayerViewModel.MakeTimeString((float)ViewModel.OrigWaveStream.TotalTime.TotalSeconds));
			_labelTotalSegments.Text = string.Format((string)_labelTotalSegments.Tag, ViewModel.TimeTier.Segments.Count);

			var currentSegment = ViewModel.CurrentSegment;
			_labelHighlightedSegment.Visible = (currentSegment != null);

			if (currentSegment != null)
			{
				_labelHighlightedSegment.Text = string.Format((string)_labelHighlightedSegment.Tag,
					ViewModel.TimeTier.GetIndexOfSegment(currentSegment) + 1,
					MediaPlayerViewModel.MakeTimeString(currentSegment.Start),
					MediaPlayerViewModel.MakeTimeString(currentSegment.End));

				// Code for displaying start time and duration of a segment
				//_labelSegmentStart.Text = string.Format((string)_labelSegmentStart.Tag,
				//    MediaPlayerViewModel.MakeTimeString(currentSegment.Start));
				//_labelSegmentDuration.Text = string.Format((string)_labelSegmentDuration.Tag,
				//    MediaPlayerViewModel.MakeTimeString(currentSegment.End - currentSegment.Start));
			}

			Utils.SetWindowRedraw(_tableLayoutSegmentInfo, true);

			//	var annotationExistsForCurrSegment = ViewModel.GetDoesCurrentSegmentHaveAnnotationFile();

			//_buttonEraseAnnotation.Visible = annotationExistsForCurrSegment;
			//_buttonEraseAnnotation.Enabled = (!_waveControl.IsPlaying && !ViewModel.GetIsAnnotationPlaying());
			//_buttonListenToAnnotation.Visible = annotationExistsForCurrSegment;
			//_buttonListenToAnnotation.Enabled = !_waveControl.IsPlaying;
			//_buttonRecordAnnotation.Visible = !annotationExistsForCurrSegment;
			//_buttonRecordAnnotation.ForeColor = _buttonEraseAnnotation.ForeColor;
			//_buttonRecordAnnotation.Text = _normalRecordButtonText;
			// TODO: Use re-record image:	(ViewModel.CurrentSegment != null || (_waveControl.GetCursorTime() > ViewModel.GetStartOfCurrentSegment())));

			base.UpdateDisplay();
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

			//if (ViewModel.GetIsSegmentLongEnough(end))
			//{
			//    _buttonRecordAnnotation.ForeColor = _buttonEraseAnnotation.ForeColor;
			//    _buttonRecordAnnotation.Text = _normalRecordButtonText;
			//}

			if (ViewModel.CurrentSegment != null)
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
				_annotationPlaybackCursorX = 0;
				_waveControl.InvalidateIfNeeded(_annotationPlaybackRectangle);
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
					_waveControl.Play(TimeSpan.FromSeconds(segment.Start), TimeSpan.FromSeconds(segment.End));
				else
				{
					var path = ViewModel.GetFullPathToAnnotationFileForSegment(segment);
					_annotationPlaybackLength = ViewModel.SegmentsAnnotationSamplesToDraw
						.First(h => h.AudioFilePath == path).AudioDuration;

					_annotationPlaybackRectangle = _waveControl.GetRectangleBetweenBoundaries(
						TimeSpan.FromSeconds(segment.Start), TimeSpan.FromSeconds(segment.End));

					_annotationPlaybackRectangle.Y = _annotationPlaybackRectangle.Bottom;
					_annotationPlaybackRectangle.Height = _waveControl.BottomReservedAreaHeight;
					_annotationPlaybackRectangle.Inflate(-2, 0);

					ViewModel.StartAnnotationPlayback(segment, HandleAnnotationPlaybackProgress,
						() => _annotationPlaybackCursorX = 0);
				}
			}

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlMouseDown(object sender, MouseEventArgs e)
		{
			if (!GetRerecordButtonRectangleForSegmentMouseIsOver().Contains(e.Location))
				return;

			ViewModel.EraseAnnotation();

			var segMouseOver = ViewModel.TimeTier.Segments[_waveControl.GetSegmentForX(e.X)];
			if (!ViewModel.BeginAnnotationRecording(segMouseOver, HandleAnnotationRecordingProgress))
				return;

			UpdateDisplay();

			_waveControl.SelectSegmentOnMouseOver = false;

			var start = TimeSpan.FromSeconds(segMouseOver.Start);
			var end = TimeSpan.FromSeconds(segMouseOver.End);

			var x1 = _waveControl.Painter.ConvertTimeToXCoordinate(start);
			var x2 = _waveControl.Painter.ConvertTimeToXCoordinate(end);
			var rc = new Rectangle(x1, 0, x2 - x1 + 1, _waveControl.ClientSize.Height);
			rc.Y = rc.Bottom - _waveControl.BottomReservedAreaHeight;
			rc.Height = _waveControl.BottomReservedAreaHeight;
			rc.Inflate(-5, -5);
			_pictureRecording.Location = rc.Location;
			_pictureRecording.Visible = true;
			_segmentBeingRecorded = new Tuple<TimeSpan, TimeSpan>(start, end);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlMouseUp(object sender, MouseEventArgs e)
		{
			if (!ViewModel.GetIsRecording())
				return;

			_pictureRecording.Visible = false;
			_waveControl.SelectSegmentOnMouseOver = true;
			ViewModel.StopAnnotationRecording();
			_waveControl.InvalidateIfNeeded(GetAnnotationRectangleForSegmentBeingRecorded());
			_segmentBeingRecorded = null;
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleAnnotationPlaybackProgress(PlaybackProgressEventArgs args)
		{
			var pixelPerMillisecond = _annotationPlaybackRectangle.Width / _annotationPlaybackLength.TotalMilliseconds;

			var newX  = _annotationPlaybackRectangle.X +
				(int)(Math.Ceiling(args.PlaybackPosition.TotalMilliseconds * pixelPerMillisecond));

			if (newX != _annotationPlaybackCursorX)
			{
				_annotationPlaybackCursorX = newX;
				Invoke((Action<Rectangle>)_waveControl.InvalidateIfNeeded, _annotationPlaybackRectangle);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePaintingAnnotatedWaveArea(PaintEventArgs e, Rectangle areaRectangle)
		{
			var segRects = _waveControl.GetSegmentRectangles().ToArray();

			for (int i = 0; i < segRects.Length; i++)
			{
				var rc = new Rectangle(segRects[i].X, areaRectangle.Y + 1,
					segRects[i].Width, areaRectangle.Height - 1);

				if (rc.X > e.ClipRectangle.Right || rc.Right < e.ClipRectangle.X)
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

				if (ViewModel.GetIsRecording())
					return;

				DrawOralAnnotationWave(e, rc, segment);
				DrawCursorInOralAnnotationWave(e, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DrawOralAnnotationWave(PaintEventArgs e, Rectangle rc, Segment segment)
		{
			// If the samples to paint for this oral annotation have not been calculated,
			// then create a helper to get those samples, then cache them in the ViewModel.
			var audioFilePath = segment.GetFullPathToCarefulSpeechFile();
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
			if (_annotationPlaybackCursorX > 0 && _annotationPlaybackCursorX >= rc.X &&
				_annotationPlaybackCursorX <= rc.Right)
			{
				rc.Inflate(0, 3);
				using (var pen = new Pen(_waveControl.Painter.CursorColor))
				{
					e.Graphics.DrawLine(pen, _annotationPlaybackCursorX, rc.Y,
						_annotationPlaybackCursorX, rc.Bottom);
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
			if (rc != Rectangle.Empty)
			{
				using (var br = new SolidBrush(Color.FromArgb(90, Settings.Default.DataEntryPanelColorBorder)))
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
			else if (!_waveControl.IsPlaying && !ViewModel.GetIsAnnotationPlaying() && _labelRecordButton.Enabled)
				DrawTextInAnnotationWaveCellWhileNotRecording(e.Graphics);

			using (var pen = new Pen(Settings.Default.BarColorBorder))
			{
				var cursorRect = GetTempCursorRectangle();
				e.Graphics.DrawLine(pen, cursorRect.X + 1, 0, cursorRect.X + 1, _waveControl.ClientSize.Height);
				if ((bool)_cursorBlinkTimer.Tag)
				{
					e.Graphics.DrawLine(pen, cursorRect.X, 0, cursorRect.X, _waveControl.ClientSize.Height);
					e.Graphics.DrawLine(pen, cursorRect.X + 2, 0, cursorRect.X + 2, _waveControl.ClientSize.Height);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DrawTextInAnnotationWaveCellWhileNotRecording(Graphics g)
		{
			var rc = GetTempSegmentAnnotationRectangle();
			rc.Inflate(-3, -3);

			TextRenderer.DrawText(g, ReadyToRecordMessage, _annotationSegmentFont, rc, Color.Black,
				TextFormatFlags.WordBreak | TextFormatFlags.WordEllipsis);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string ReadyToRecordMessage
		{
			get { throw new NotImplementedException(); }
		}

		/// ------------------------------------------------------------------------------------
		private void DrawTextInAnnotationWaveCellWhileRecording(Graphics g)
		{
			var rc = _pictureRecording.Bounds;
			//rc.Inflate(2, 2);
			//rc.Width--;
			//rc.Height--;
			//g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			//g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			//g.FillEllipse(Brushes.White, rc);
			//g.DrawEllipse(Pens.Black, rc);

			rc = GetTempSegmentAnnotationRectangle();
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
		private Rectangle GetTempSegmentAnnotationRectangle()
		{
			var rc = GetTempSegmentRectangle();
			rc.Y = rc.Bottom - _waveControl.BottomReservedAreaHeight;
			rc.Height = _waveControl.BottomReservedAreaHeight;
			return rc;
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetAnnotationRectangleForSegmentBeingRecorded()
		{
			if (_segmentBeingRecorded == null)
				return Rectangle.Empty;

			return new Rectangle(_waveControl.Painter.ConvertTimeToXCoordinate(_segmentBeingRecorded.Item1),
				_waveControl.ClientRectangle.Bottom - _waveControl.BottomReservedAreaHeight,
				_waveControl.Painter.ConvertTimeToXCoordinate(_segmentBeingRecorded.Item2),
				_waveControl.BottomReservedAreaHeight);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawPlayButtonInSegment(Graphics g, Rectangle rc)
		{
			if (rc == Rectangle.Empty)
				return;

			var img = StandardAudioButtons.PlayButtonImage;
			var mousePos = _waveControl.PointToClient(MousePosition);

			if (rc.Contains(mousePos))
				img = StandardAudioButtons.HotPlayButtonImage;

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
		private Rectangle GetRectangleAndIndexOfHotSegment(out int index)
		{
			var mousePos = _waveControl.PointToClient(MousePosition);
			var i = 0;
			var rc = _waveControl.GetSegmentRectangles().FirstOrDefault(r =>
			{
				r.Height += _waveControl.BottomReservedAreaHeight;
				if (r.Contains(mousePos))
					return true;

				i++;
				return false;
			});

			index = i;
			return rc;
		}

		/// ------------------------------------------------------------------------------------
		private Tuple<Rectangle, Rectangle> GetPlayButtonRectanglesForSegmentMouseIsOver()
		{
			var playButtonSize = StandardAudioButtons.PlayButtonImage.Size;

			if (!_waveControl.GetHasSelection())
			{
				var tempRect = GetTempSegmentRectangle();
				if (!tempRect.Contains(_waveControl.PointToClient(MousePosition)))
					return null;

				return new Tuple<Rectangle, Rectangle>(new Rectangle(tempRect.X + 6,
					tempRect.Bottom - _waveControl.BottomReservedAreaHeight - 5 - playButtonSize.Height,
					playButtonSize.Width, playButtonSize.Height), Rectangle.Empty);
			}

			int i;
			var rc = GetRectangleAndIndexOfHotSegment(out i);

			if (rc.IsEmpty || playButtonSize.Width + 6 > rc.Width)
				return null;

			var originalRecordingButtonRect = new Rectangle(rc.X + 6,
				rc.Bottom - 5 - playButtonSize.Height, playButtonSize.Width, playButtonSize.Height);

			var annotationRecordingButtonRect = (!ViewModel.GetDoesSegmentHaveAnnotationFile(i) ?
				Rectangle.Empty : new Rectangle(rc.X + 6,
				rc.Bottom + _waveControl.BottomReservedAreaHeight - 5 - playButtonSize.Height,
				playButtonSize.Width, playButtonSize.Height));

			return new Tuple<Rectangle, Rectangle>(originalRecordingButtonRect, annotationRecordingButtonRect);
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetRerecordButtonRectangleForSegmentMouseIsOver()
		{
			var rerecordButtonSize = _normalRerecordAnnotationButton.Size;

			if (!_waveControl.GetHasSelection())
				return Rectangle.Empty;

			int i;
			var rc = GetRectangleAndIndexOfHotSegment(out i);

			if (rc.IsEmpty || rerecordButtonSize.Width + 6 > rc.Width ||
				(!ViewModel.GetDoesSegmentHaveAnnotationFile(i) && !ViewModel.GetIsRecording()))
			{
				return Rectangle.Empty;
			}

			return new Rectangle(rc.Right - 6 - rerecordButtonSize.Width,
				rc.Bottom + _waveControl.BottomReservedAreaHeight - 5 - rerecordButtonSize.Height,
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
			var endOfLastSegment = ViewModel.GetEndOfLastSegment();

			if (_endOfTempSegment < endOfLastSegment)
				_endOfTempSegment = endOfLastSegment;

			if (_waveControl.GetCursorTime() != _endOfTempSegment)
				_waveControl.SetCursor(_endOfTempSegment);

			var targetTime = (button == _labelListenButton ? _endOfTempSegment : endOfLastSegment);

			_waveControl.EnsureTimeIsVisible(targetTime, endOfLastSegment,
				_waveControl.WaveStream.TotalTime, true, button == _labelListenButton);
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

			_waveControl.Play(_endOfTempSegment);
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

			//if (_waveControl.IsPlaying || ViewModel.GetIsAnnotationPlaying())
			//{
			//    if (_waveControl.IsPlaying)
			//        _waveControl.Stop();
			//    else
			//        ViewModel.StopAnnotationPlayback();

			//    ScrollInPreparationForListenOrRecord(_labelRecordButton);
			//}

			if (ViewModel.BeginAnnotationRecording(_endOfTempSegment, HandleAnnotationRecordingProgress))
			{
				UpdateDisplay();

				_waveControl.SelectSegmentOnMouseOver = false;
				var rc = GetTempSegmentAnnotationRectangle();
				rc.Inflate(-5, -5);
				_pictureRecording.Location = rc.Location;
				_pictureRecording.Visible = true;
				_segmentBeingRecorded = new Tuple<TimeSpan, TimeSpan>(ViewModel.GetEndOfLastSegment(), _endOfTempSegment);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleAnnotationRecordingProgress(TimeSpan elapsedRecordedTime)
		{
			_elapsedRecordingTime = elapsedRecordedTime;

			Invoke((Action<Rectangle>)_waveControl.InvalidateIfNeeded,
				GetAnnotationRectangleForSegmentBeingRecorded());
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordAnnotationMouseUp(object sender, MouseEventArgs e)
		{
			if (!ViewModel.GetIsRecording())
				return;

			_pictureRecording.Visible = false;
			_waveControl.SelectSegmentOnMouseOver = true;

			if (ViewModel.StopAnnotationRecording())
			{
				_waveControl.SegmentBoundaries = ViewModel.InsertNewBoundary(_waveControl.GetCursorTime());

				if (ViewModel.CurrentSegment == null)
				{
					if (!ViewModel.IsFullySegmented)
					{
						_waveControl.ClearSelection();
						UpdateDisplay();
						return;
					}

					ViewModel.SelectSegmentFromTime(ViewModel.OrigWaveStream.TotalTime);
					_waveControl.SetSelectionTimes(ViewModel.GetStartOfCurrentSegment(), ViewModel.GetEndOfCurrentSegment());
				}

				_waveControl.SetCursor(TimeSpan.FromSeconds(1).Negate());
				GoToNextUnannotatedSegment();
				UpdateDisplay();
			}
			else
			{
				//_buttonRecordAnnotation.ForeColor = Color.Red;
				//_buttonRecordAnnotation.Text = LocalizationManager.GetString(
				//    "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingButtonText.WhenRecordingTooShort",
				//    "Whoops! You need to hold down the SPACE bar or mouse button while talking.");
				//_buttonEraseAnnotation.PerformClick();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void GoToNextUnannotatedSegment()
		{
			if (ViewModel.SetNextUnannotatedSegment())
			{
				var start = TimeSpan.FromSeconds(ViewModel.CurrentSegment.Start);
				var end = TimeSpan.FromSeconds(ViewModel.CurrentSegment.End);

				_waveControl.SetSelectionTimes(start, end);
				_waveControl.EnsureTimeIsVisible(start, start, end, true, false);
			}
			else if (!ViewModel.IsFullySegmented)
			{
				_waveControl.ClearSelection();
				var time = ViewModel.GetEndOfLastSegment();
				_waveControl.SetCursor(time);
				_waveControl.EnsureTimeIsVisible(time, time, ViewModel.OrigWaveStream.TotalTime, true, false);
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
