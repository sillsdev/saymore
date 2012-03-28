using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Localization;
using NAudio.Wave;
using Palaso.Media.Naudio;
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
		private readonly Image _hotPlayAnnotationButton;
		private readonly Image _hotPlayOriginalButton;
		private readonly Image _hotRecordAnnotationButton;
		private readonly Image _normalPlayOriginalButton;
		private readonly Image _normalRecordAnnotationButton;

		private TimeSpan _annotationPlaybackLength;
		private int _annotationPlaybackCursorX;
		private Rectangle _annotationPlaybackRectangle;

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

			_tableLayoutTop.Visible = false;

			_normalRecordButtonText = "TODO: Hold down Space to record";

			_normalPlayOriginalButton = _labelListenButton.Image;
			_normalRecordAnnotationButton = _labelRecordButton.Image;
			_hotPlayOriginalButton = PaintingHelper.MakeHotImage(_normalPlayOriginalButton);
			_hotRecordAnnotationButton = PaintingHelper.MakeHotImage(_normalRecordAnnotationButton);
			_hotPlayAnnotationButton = PaintingHelper.MakeHotImage(Resources.PlaySegment);

			_labelListenButton.MouseEnter += delegate { _labelListenButton.Image = _hotPlayOriginalButton; };
			_labelListenButton.MouseLeave += delegate { _labelListenButton.Image = _normalPlayOriginalButton; };
			_labelListenButton.MouseUp += delegate { _waveControl.Stop(); };

			_labelRecordButton.MouseEnter += delegate { _labelRecordButton.Image = _hotRecordAnnotationButton; };
			_labelRecordButton.MouseLeave += delegate { _labelRecordButton.Image = _normalRecordAnnotationButton; };
			_labelRecordButton.MouseDown += HandleRecordAnnotationMouseDown;
			_labelRecordButton.MouseUp += HandleRecordAnnotationMouseUp;

			//_buttonListenToAnnotation.Click += delegate
			//{
			//    ViewModel.StartAnnotationPlayback();
			//    UpdateDisplay();
			//};

			//_buttonEraseAnnotation.Click += delegate
			//{
			//    _waveControl.Stop();
			//    ViewModel.EraseAnnotation();
			//    _waveControl.InvalidateSelectedRegion();
			//    UpdateDisplay();
			//};

			_waveControl.BoundaryMoved += HandleSegmentBoundaryMovedInWaveControl;

			_tableLayoutMediaButtons.Dock = DockStyle.Left;
			_panelWaveControl.Controls.Add(_tableLayoutMediaButtons);
			_waveControl.BringToFront();
			_tableLayoutMediaButtons.RowStyles[0].SizeType = SizeType.AutoSize;
			_tableLayoutMediaButtons.RowStyles[2].SizeType = SizeType.Absolute;
			_tableLayoutMediaButtons.Controls.Add(_labelOriginalRecording, 0, 0);
			_labelOriginalRecording.TextAlign = ContentAlignment.TopCenter;
			_labelOriginalRecording.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			var margin = _labelOriginalRecording.Margin;
			margin.Top = 10;
			_labelOriginalRecording.Margin = margin;

			_waveControl.ClientSizeChanged += delegate
			{
				_tableLayoutMediaButtons.RowStyles[2].Height = Settings.Default.AnnotationWaveViewHeight +
					(_waveControl.HorizontalScroll.Visible ? SystemInformation.HorizontalScrollBarHeight : 0);
			};
		}

		/// ------------------------------------------------------------------------------------
		protected override WaveControlBasic CreateWaveControl()
		{
			_waveControl = new 	WaveControlWithRangeSelection();
			_waveControl.BottomReservedAreaHeight = Settings.Default.AnnotationWaveViewHeight;
			_waveControl.BottomReservedAreaBorderColor = Settings.Default.DataEntryPanelColorBorder;
			_waveControl.BottomReservedAreaColor = Color.FromArgb(130, Settings.Default.DataEntryPanelColorBegin);
			_waveControl.BottomReservedAreaPaintAction = HandlePaintingAnnotatedWaveArea;
			_waveControl.PostPaintAction = HandleWaveControlPostPaint;
			_waveControl.Paint += HandleWaveControlPaint;
			_waveControl.MouseMove += HandleWaveControlMouseMove;
			_waveControl.MouseLeave += HandleWaveControlMouseLeave;
			_waveControl.MouseDoubleClick += HandleWaveControlMouseDoubleClick;
			_waveControl.SelectedRegionChanged += HandleWaveControlSelectedRegionChanged;

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
				_labelListenButton.Enabled = false;
			//	_buttonRecordAnnotation.ForeColor = _buttonEraseAnnotation.ForeColor;
			//	_labelRecordButton.Text = LocalizationManager.GetString(
			//		"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingButtonText.WhenRecording",
			//		"Recording...");
				return;
			}

			Utils.SetWindowRedraw(_tableLayoutOuter, false);
			var annotationExistsForCurrSegment = ViewModel.GetDoesCurrentSegmentHaveAnnotationFile();

			//_buttonEraseAnnotation.Visible = annotationExistsForCurrSegment;
			//_buttonEraseAnnotation.Enabled = (!_waveControl.IsPlaying && !ViewModel.GetIsAnnotationPlaying());
			_labelListenButton.Enabled = !ViewModel.GetIsAnnotationPlaying();
			//_buttonListenToAnnotation.Visible = annotationExistsForCurrSegment;
			//_buttonListenToAnnotation.Enabled = !_waveControl.IsPlaying;
			//_buttonRecordAnnotation.Visible = !annotationExistsForCurrSegment;
			//_buttonRecordAnnotation.ForeColor = _buttonEraseAnnotation.ForeColor;
			//_buttonRecordAnnotation.Text = _normalRecordButtonText;
			_labelRecordButton.Enabled = !_waveControl.IsPlaying;
			// TODO: Use re-record image:	(ViewModel.CurrentSegment != null || (_waveControl.GetCursorTime() > ViewModel.GetStartOfCurrentSegment())));

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

			System.Diagnostics.Debug.WriteLine(current);

			if (ViewModel.CurrentSegment == null)
				_waveControl.SetSelectionTimes(ViewModel.GetStartOfCurrentSegment(), current);
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
		private void HandleWaveControlMouseDoubleClick(object sender, MouseEventArgs e)
		{
			var segMouseOver = _waveControl.GetSegmentForX(e.X);
			if (segMouseOver < 0)
				return;

			var segment = ViewModel.TimeTier.Segments[segMouseOver];

			if (e.Y < _waveControl.ClientRectangle.Bottom - _waveControl.BottomReservedAreaHeight)
				_waveControl.Play(TimeSpan.FromSeconds(segment.Start), TimeSpan.FromSeconds(segment.End));
			else if (ViewModel.GetDoesSegmentHaveAnnotationFile(segMouseOver))
			{
				var path = ViewModel.GetFullPathToAnnotationFileForSegment(segment);
				using (var stream = new WaveFileReader(path))
					_annotationPlaybackLength = stream.TotalTime;

				_annotationPlaybackRectangle = _waveControl.GetRectangleBetweenBoundaries(
					TimeSpan.FromSeconds(segment.Start), TimeSpan.FromSeconds(segment.End));

				_annotationPlaybackRectangle.Y = _annotationPlaybackRectangle.Bottom;
				_annotationPlaybackRectangle.Height = _waveControl.BottomReservedAreaHeight;
				_annotationPlaybackRectangle.Inflate(-2, 0);

				ViewModel.StartAnnotationPlayback(HandleAnnotationPlaybackProgress,
					() => _annotationPlaybackCursorX = 0);

				UpdateDisplay();
			}
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
				Invoke((Action<Rectangle>)_waveControl.Invalidate, _annotationPlaybackRectangle);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePaintingAnnotatedWaveArea(PaintEventArgs e, Rectangle areaRectangle)
		{
			int i = 0;
			foreach (var segRect in _waveControl.GetSegmentRectangles())
			{
				var segment = ViewModel.TimeTier.Segments[i];
				if (!ViewModel.GetDoesSegmentHaveAnnotationFile(i++))
					continue;

				var rc = new Rectangle(segRect.X, areaRectangle.Y + 1, segRect.Width, areaRectangle.Height - 1);
				if (rc.X == 0)
					rc.Width -= 2;
				else
					rc.Inflate(-2, 0);

				using (var br = new SolidBrush(Color.FromArgb(175, Settings.Default.DataEntryPanelColorBegin)))
					e.Graphics.FillRectangle(br, rc);

				rc.Inflate(0, -3);
				rc.Width--;

				using (var painter = new WavePainterBasic { ForeColor = Color.Black, BackColor = Color.Black })
				{
					var audioFilePath = segment.GetFullPathToCarefulSpeechFile();
					var helper = ViewModel.SegmentsAnnotationSamplesToDraw.FirstOrDefault(h => h.AudioFilePath == audioFilePath);
					if (helper == null)
					{
						helper = new AudioFileHelper(audioFilePath);
						ViewModel.SegmentsAnnotationSamplesToDraw.Add(helper);
					}

					painter.SetSamplesToDraw(helper.GetSamples((uint)rc.Width));
					painter.Draw(e, rc);
				}

				if (_annotationPlaybackCursorX > 0 && _annotationPlaybackCursorX >= rc.X &&
					_annotationPlaybackCursorX <= rc.Right)
				{
					rc.Inflate(0, 3);
					e.Graphics.DrawLine(Pens.Green, _annotationPlaybackCursorX, rc.Y - 150,
						_annotationPlaybackCursorX, rc.Bottom);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlPostPaint(PaintEventArgs e)
		{
			if (_waveControl.IsPlaying)
				return;

			var playButtonRects = GetPlayButtonRectanglesForSegmentMouseIsOver();

			if (playButtonRects == null)
				return;

			DrawPlayButtonsInSegments(e.Graphics, playButtonRects.Item1);
			DrawPlayButtonsInSegments(e.Graphics, playButtonRects.Item2);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawPlayButtonsInSegments(Graphics g, Rectangle rc)
		{
			if (rc == Rectangle.Empty)
				return;

			Image img = Resources.PlaySegment;
			var mousePos = _waveControl.PointToClient(MousePosition);

			var buttonRect = new Rectangle(rc.X + 5, rc.Bottom - (img.Height + 5), img.Width, img.Height);
			if (buttonRect.Contains(mousePos))
				img = _hotPlayAnnotationButton;

			g.DrawImage(img, buttonRect);
		}

		/// ------------------------------------------------------------------------------------
		private Tuple<Rectangle, Rectangle> GetPlayButtonRectanglesForSegmentMouseIsOver()
		{
			var mousePos = _waveControl.PointToClient(MousePosition);

			int i = 0;
			var rc = _waveControl.GetSegmentRectangles().FirstOrDefault(r =>
			{
				r.Height += Settings.Default.AnnotationWaveViewHeight;
				if (r.Contains(mousePos))
					return true;

				i++;
				return false;
			});

			Size playButtonSize = Resources.PlaySegment.Size;

			if (rc.IsEmpty || playButtonSize.Width + 5 > rc.Width)
				return null;

			var originalRecordingButtonRect = new Rectangle(rc.X + 5,
				rc.Bottom - 5 - playButtonSize.Height, playButtonSize.Width, playButtonSize.Height);

			var annotationRecordingButtonRect = (!ViewModel.GetDoesSegmentHaveAnnotationFile(i) ?
				Rectangle.Empty : new Rectangle(rc.X + 5,
				rc.Bottom + _waveControl.BottomReservedAreaHeight - 5 - playButtonSize.Height,
				playButtonSize.Width, playButtonSize.Height));

			return new Tuple<Rectangle, Rectangle>(originalRecordingButtonRect, annotationRecordingButtonRect);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlPaint(object sender, PaintEventArgs e)
		{
			//var img = Resources.MissingAnnotation;

			//int i = 0;
			//foreach (var segRect in _waveControl.GetSegmentRectangles())
			//{
			//    if (!segRect.IsEmpty && !ViewModel.GetDoesSegmentHaveAnnotationFile(i++))
			//    {
			//        var rc = new Rectangle(new Point(segRect.X + 1, segRect.Y + 1), img.Size);
			//        if (segRect.Contains(rc))
			//            e.Graphics.DrawImage(img, rc);
			//    }
			//}
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
		private void HandleListenToOriginalMouseDown(object sender, MouseEventArgs e)
		{
			if (_waveControl.IsPlaying ||
				(ViewModel.CurrentSegment == null && _waveControl.GetCursorTime() == ViewModel.OrigWaveStream.TotalTime))
			{
				return;
			}

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
			//if (!ViewModel.GetIsSegmentLongEnough(_waveControl.GetCursorTime()))
			//{
			//    _labelRecordButton.BackColor = Color.Red;
			//    _buttonRecordAnnotation.Text = GetSegmentTooShortText();
			//    return;
			//}

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
				{
					if (!ViewModel.IsFullySegmented)
					{
						_waveControl.ClearSelection();
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
				_waveControl.EnsureRangeIsVisible(start, end);
			}
			else if (!ViewModel.IsFullySegmented)
			{
				_waveControl.ClearSelection();
				var time = ViewModel.GetEndOfLastSegment();
				_waveControl.SetCursor(time);
				//_waveControl.EnsureRangeIsVisible(time, ViewModel.OrigWaveStream.TotalTime);
				_waveControl.EnsureRangeIsVisible(time, time);
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
