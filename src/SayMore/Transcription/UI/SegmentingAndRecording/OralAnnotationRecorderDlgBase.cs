using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using DesktopAnalytics;
using L10NSharp;
using L10NSharp.TMXUtils;
using L10NSharp.UI;
using SIL.Media.Naudio;
using SIL.Media.Naudio.UI;
using SIL.Reporting;
using SIL.Windows.Forms;
using SIL.Windows.Forms.PortableSettingsProvider;
using SayMore.Media.Audio;
using SayMore.Media.MPlayer;
using SayMore.Properties;
using SayMore.Transcription.Model;

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
			Error,
		}

		private enum AdvanceOptionsAfterRecording
		{
			Advance,
			AdvanceOnlyToContiguousUnsegmentedAudio,
			DoNotAdvance,
		}

		private PeakMeterCtrl _peakMeter;
		private RecordingDeviceIndicator _recDeviceIndicator;
		private string _recordingErrorMessage;
		private Image _hotPlaySourceButton;
		private Image _hotRecordAnnotationButton;
		private Image _normalPlaySourceButton;
		private Image _normalRecordAnnotationButton;
		private Image _normalRerecordAnnotationButton;
		private Image _hotRerecordAnnotationButton;

		private TimeSpan _elapsedRecordingTime;
		private TimeSpan _annotationPlaybackLength;
		private TimeSpan _lastAnnotationPlaybackPosition;
		private AnnotationSegment _segmentWhoseAnnotationIsBeingPlayedBack;
		private Font _annotationSegmentFont;
		private TimeRange _segmentBeingRecorded;
		private bool _listeningOrRecordingUsingSpaceBar;
		private bool _spaceBarIsDown;
		private bool _playingBackUsingHoldDownButton;
		private bool _reRecording;
		private bool _userHasListenedToSelectedSegment;
		private SpaceBarMode _spaceBarMode;
		private readonly Color _selectedSegmentHighlighColor = Color.Moccasin;
		private AdvanceOptionsAfterRecording _advanceOption;

		protected WaveControlWithRangeSelection _waveControl;
		private bool _needToShowRecordingAbortedMessage;

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
			int listenToOriginalRecordingDownWidth = -1, listenToOriginalRecording = -1,
				recordingOralAnnotationInProgressWidth = -1, recordOralAnnotationWidth = -1,
				green_checkWidth = -1, information_redWidth = -1, information_blueWidth = -1;

			try
			{
				// SP-950: Check for corrupt resources, out of memory, or ???
				listenToOriginalRecordingDownWidth = ResourceImageCache.ListenToOriginalRecordingDown.Width;
				listenToOriginalRecording = ResourceImageCache.ListenToOriginalRecording.Width;
				recordingOralAnnotationInProgressWidth = ResourceImageCache.RecordingOralAnnotationInProgress.Width;
				recordOralAnnotationWidth = ResourceImageCache.RecordOralAnnotation.Width;
				green_checkWidth = ResourceImageCache.Green_check.Width;
				information_redWidth = ResourceImageCache.Information_red.Width;
				information_blueWidth = ResourceImageCache.Information_blue.Width;
				Logger.WriteEvent(string.Format("SP-950 Debug Info: listenToOriginalRecordingDownWidth = {0}; listenToOriginalRecording = {1}; " +
					"recordingOralAnnotationInProgressWidth = {2}; recordOralAnnotationWidth = {3}; green_checkWidth = {4}; " +
					"information_redWidth = {5}; information_blueWidth = {6};",
					listenToOriginalRecordingDownWidth, listenToOriginalRecording, recordingOralAnnotationInProgressWidth, recordOralAnnotationWidth,
					green_checkWidth, information_redWidth, information_blueWidth));
				Analytics.Track(string.Format("OARBD: {0}; {1}; {2}; {3}; {4}; {5}; {6};",
					listenToOriginalRecordingDownWidth, listenToOriginalRecording, recordingOralAnnotationInProgressWidth, recordOralAnnotationWidth,
					green_checkWidth, information_redWidth, information_blueWidth));
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalExceptionWithMessage(e,
					"Problem with Image from resources. Please report this information to help us fix a difficult bug! SP-950 Debug info: " +
					"listenToOriginalRecordingDownWidth = {0}; " +
					"listenToOriginalRecording = {1}; " +
					"recordingOralAnnotationInProgressWidth = {2}; " +
					"recordOralAnnotationWidth = {3}; " +
					"green_checkWidth = {4}; " +
					"information_redWidth = {5}; " +
					"information_blueWidth = {6}",
					listenToOriginalRecordingDownWidth,
					listenToOriginalRecording,
					recordingOralAnnotationInProgressWidth,
					recordOralAnnotationWidth,
					green_checkWidth,
					information_redWidth,
					information_blueWidth);
			}

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
			viewModel.RecordingCompleted = RecordingCompleted;
			viewModel.SelectedDeviceChanged += SelectedRecordingDeviceChanged;

			SegmentIgnored += HandleSegmentIgnored;
		}

		/// ------------------------------------------------------------------------------------
		void HandleSegmentIgnored()
		{
			KillRecordingErrorMessage();

			if (HotSegment == null || ViewModel.CurrentUnannotatedSegment == null || HotSegment == ViewModel.CurrentUnannotatedSegment)
				GoToNextUnannotatedSegment();
			else
				SetModeToListenOrFinished();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleNAudioExceptionThrown(Exception exception)
		{
			if (!AudioUtils.GetCanRecordAudio(true))
			{
				if (_spaceBarMode == SpaceBarMode.Record)
					_spaceBarMode = SpaceBarMode.Error;
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
				ViewModel.InitializeAnnotationRecorder(_peakMeter, _recDeviceIndicator,
					HandleAnnotationRecordingProgress);
				if (_spaceBarMode == SpaceBarMode.Error)
				{
					_spaceBarMode = _userHasListenedToSelectedSegment ? SpaceBarMode.Record :
						SpaceBarMode.Listen;
				}
				_waveControl.Invalidate();
				UpdateDisplay();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();
				_annotationSegmentFont.Dispose();
				_hotPlaySourceButton.Dispose();
				_hotRecordAnnotationButton.Dispose();
				if (_waveControl != null)
					_waveControl.Dispose();
				AudioUtils.NAudioExceptionThrown -= HandleNAudioExceptionThrown;

				LocalizeItemDlg<TMXDocument>.StringsLocalized -= HandleStringsLocalized;
			}

			base.Dispose(disposing);
		}

		#region Initialization methods
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			if (_moreReliableDesignMode)
				return;

			ViewModel.InitializeAnnotationRecorder(_peakMeter, _recDeviceIndicator,
				HandleAnnotationRecordingProgress);
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
			_recDeviceIndicator.MicCheckingEnabled = true;

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

			_recDeviceIndicator = new RecordingDeviceIndicator(1500, false);
			_recDeviceIndicator.Anchor = AnchorStyles.Top;
			_recDeviceIndicator.AutoSize = true;
			_recDeviceIndicator.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			_recDeviceIndicator.Margin = new Padding(4, 3, 0, 3);
			_tableLayoutRecordAnnotations.Controls.Add(_recDeviceIndicator, 0, 2);

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
			_labelRecordButton.MouseUp += delegate { StopRecording(AdvanceOptionsAfterRecording.Advance); };
		}

		/// ------------------------------------------------------------------------------------
		private void FinishListeningUsingEarOrSpace()
		{
			if (_waveControl.IsPlaying)
				_waveControl.Stop();

			_playingBackUsingHoldDownButton = false;
			if (ViewModel.GetSelectedSegmentIsLongEnough() &&
				(_userHasListenedToSelectedSegment || ViewModel.CurrentUnannotatedSegment == null))
			{
				_userHasListenedToSelectedSegment = true;
				_spaceBarMode = (AudioUtils.GetCanRecordAudio(true)) ? SpaceBarMode.Record : SpaceBarMode.Error;
			}

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

			_normalPlaySourceButton = _labelListenButton.Image;
			_normalRecordAnnotationButton = _labelRecordButton.Image;
			_normalRerecordAnnotationButton = ResourceImageCache.RerecordOralAnnotation;
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
			_labelSourceRecording.ForeColor = _labelListenButton.ForeColor;
			_videoHelpMenu.Font = _labelSourceRecording.Font;

			_annotationSegmentFont = FontHelper.MakeFont(Program.DialogFont, 8, FontStyle.Bold);

			LocalizeItemDlg<TMXDocument>.StringsLocalized += HandleStringsLocalized;
		}

		private const int kNumberOfRows = 4;

		/// ------------------------------------------------------------------------------------
		private void InitializeTableLayoutButtonControls()
		{
			_tableLayoutButtons.RowCount = kNumberOfRows;
			_tableLayoutButtons.Controls.Add(_videoHelpMenuStrip, 1, kNumberOfRows - 1);
			_tableLayoutButtons.Height += _videoHelpMenuStrip.Height;
			_videoHelpMenuStrip.BackColor = BackColor;

			SetTableLayoutForListenOrRecordMode();
		}

		/// ------------------------------------------------------------------------------------
		private void SetTableLayoutForListenOrRecordMode()
		{
			_tableLayoutButtons.RowStyles[0].Height = 25f;
			_tableLayoutButtons.RowStyles[1].Height = 25f;
			_tableLayoutButtons.RowStyles[2].Height = 25f;
			_tableLayoutButtons.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1f));

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
		protected override WaveControlWithMovableBoundaries CreateWaveControl()
		{
			_waveControl = new WaveControlWithRangeSelection();
			_waveControl.BottomReservedAreaBorderColor = Settings.Default.DataEntryPanelColorBorder;
			_waveControl.BottomReservedAreaColor = _tableLayoutRecordAnnotations.BackColor;

			_waveControl.BottomReservedAreaPaintAction = HandlePaintingAnnotatedWaveArea;
			_waveControl.MouseDown += HandleWaveControlMouseDown;
			_waveControl.BoundaryMoved += HandleSegmentBoundaryMovedInWaveControl;
			_waveControl.PlaybackStarted += delegate { KillRecordingErrorMessage(); };
			_waveControl.PlaybackErrorAction = HandlePlaybackError;

			_waveControl.MouseUp += delegate
			{
				if (_reRecording)
					StopRecording(AdvanceOptionsAfterRecording.AdvanceOnlyToContiguousUnsegmentedAudio);
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

		/// ------------------------------------------------------------------------------------
		/// <summary>Overridden to allow play button and Ignore checkbox to be displayed in
		/// the rectanlge for the segment being defined (not yet a real segment)</summary>
		/// ------------------------------------------------------------------------------------
		protected override Rectangle HotRectangleBeyondFinalSegment
		{
			get
			{
				var rc = GetRectangleForTimeRangeBeyondEndOfLastSegment(_viewModel.VirtualBoundaryBeyondLastSegment);
				return rc.Contains(MousePositionInWaveControl) ? rc : base.HotRectangleBeyondFinalSegment;
			}
		}
		#endregion

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			ViewModel.CloseAnnotationPlayer();
			//ViewModel.CloseAnnotationRecorder();
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
			_recDeviceIndicator.UpdateDisplay();

			if (_waveControl.IsPlaying && _playingBackUsingHoldDownButton)
				SetLabelImage(_labelListenButton, ResourceImageCache.ListenToOriginalRecordingDown, "ListenToOriginalRecordingDown");
			else
				SetLabelImage(_labelListenButton, ResourceImageCache.ListenToOriginalRecording, "ListenToOriginalRecording");

			if (ViewModel.GetIsRecording())
				SetLabelImage(_labelRecordButton, ResourceImageCache.RecordingOralAnnotationInProgress, "RecordingOralAnnotationInProgress");
			else
				SetLabelImage(_labelRecordButton, ResourceImageCache.RecordOralAnnotation, "RecordOralAnnotation");

			_labelListenButton.Enabled = !ViewModel.GetIsRecording() &&
										(ViewModel.CurrentUnannotatedSegment != null || !ViewModel.GetIsFullyAnnotated());

			_labelRecordButton.Enabled = (ViewModel.GetSelectedSegmentIsLongEnough() &&
										_userHasListenedToSelectedSegment &&
										AudioUtils.GetCanRecordAudio(true) &&
										!_waveControl.IsPlaying && !ViewModel.GetIsAnnotationPlaying());

			_labelListenHint.Visible = _spaceBarMode == SpaceBarMode.Listen && _labelListenButton.Enabled;
			_labelRecordHint.Visible = _spaceBarMode == SpaceBarMode.Record && _labelRecordButton.Enabled && !_reRecording &&
										_recordingErrorMessage == null;

			if (_spaceBarMode == SpaceBarMode.Done && _recordingErrorMessage == null)
			{
				if (!_labelFinishedHint.Visible)
				{
					SetLabelImage(_pictureIcon, ResourceImageCache.Green_check, "Green_check");
					_labelFinishedHint.Visible = true;
					_tableLayoutButtons.Controls.Add(_labelFinishedHint, 1, 0);
					_tableLayoutButtons.SetRowSpan(_labelFinishedHint, 3);
					AcceptButton = _buttonOK;
				}
			}
			else
			{
				UdateErrorMessageDisplay();

				if (_labelErrorInfo.Visible)
				{
					SetLabelImage(_pictureIcon, ResourceImageCache.Information_red, "Information_red");
					if (_labelFinishedHint.Visible)
					{
						_labelFinishedHint.Visible = false;
						_tableLayoutButtons.Controls.Remove(_labelFinishedHint);
					}
					_labelRecordHint.Visible = false;
				}
				else
				{
					SetLabelImage(_pictureIcon, ResourceImageCache.Information_blue, "Information_blue");
				}

				float percentage = (_labelErrorInfo.Visible) ? 50 : 100;
				_tableLayoutButtons.RowStyles[0].Height = (_labelErrorInfo.Visible) ? percentage : 0;
				_tableLayoutButtons.RowStyles[1].Height = (_labelListenHint.Visible) ? percentage : 0;
				_tableLayoutButtons.RowStyles[2].Height = (_labelRecordHint.Visible) ? percentage : 0;
			}

			base.UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is an attempt to track down SP-950
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetLabelImage(Control control, Bitmap image, string name)
		{
			try
			{
				Label label = control as Label;
				if (label != null)
					label.Image = image;
				else
				{
					((PictureBox) control).Image = image;
				}
			}
			catch (Exception e)
			{
				Analytics.Track("SetImageLabelError", new Dictionary<string, string> { { "name", name } });
				ErrorReport.ReportNonFatalExceptionWithMessage(e,
					"Problem setting Image from resources: Control: {0}; Image: {1}", control.Name, name);
				Analytics.ReportException(e);
			}
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
				else if (_recordingErrorMessage != null)
				{
					_labelErrorInfo.Visible = true;
					_labelErrorInfo.Text = _recordingErrorMessage;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void UpdateStatusLabelsDisplay()
		{
			base.UpdateStatusLabelsDisplay();

			var currentSegment = HotSegment;
			if (currentSegment == null)
				return;

			_labelSegmentNumber.Visible = false;
			_tableLayoutZoom.Visible = false;
			_labelSegmentXofY.Visible = true;
			_labelSegmentXofY.Text = string.Format(_segmentXofYFormat,
				ViewModel.TimeTier.GetIndexOfSegment(currentSegment) + 1, _viewModel.GetSegmentCount());
		}

		/// ------------------------------------------------------------------------------------
		protected override TimeSpan GetCurrentTimeForTimeDisplay()
		{
			var currentSegment = HotSegment;
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

			var selectedTimeRange = ViewModel.GetSelectedTimeRange();
			if (!selectedTimeRange.Contains(end, true))
			{
				if (ViewModel.GetHasNewSegment())
					_waveControl.SetCursor(end);
				else if (_startTimeOfSegmentWhoseEndIsMoving < TimeSpan.Zero)
				{
					// If this isn't the end of playback for a moving segment, then we set
					// the cursor back to the start of the selected time
					// range in hopes of keeping the user on track with the task of recording.
					_waveControl.SetCursor(selectedTimeRange.Start);
				}
			}

			_startTimeOfSegmentWhoseEndIsMoving = TimeSpan.FromSeconds(1).Negate();

			if (end > ViewModel.GetEndOfLastSegment())
			{
				var rc1 = GetNewSegmentCursorRectangle();
				if (_playingBackUsingHoldDownButton)
					SetNewSegmentEndBoundary(end);

				var rc2 = GetNewSegmentRectangle();
				rc2.Inflate(rc1.Width / 2, 0);
				_waveControl.InvalidateIfNeeded(rc2);
			}

			if (end == ViewModel.GetSelectedTimeRange().End &&
				(ViewModel.CurrentUnannotatedSegment != null))
			{
				InvalidateBottomReservedRectangleForCurrentUnannotatedSegment();
				_userHasListenedToSelectedSegment = true;
				_spaceBarMode = (AudioUtils.GetCanRecordAudio(true)) ? SpaceBarMode.Record : SpaceBarMode.Error;
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
			KillRecordingErrorMessage();
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
		protected override void HandleWaveControlMouseMove(object sender, MouseEventArgs e)
		{
			if (ViewModel.GetIsRecording())
				return;

			_waveControl.InvalidateIfNeeded(PlayAnnotationButtonRectangle);
			_waveControl.InvalidateIfNeeded(GetRerecordButtonRectangleForSegmentMouseIsOver());

			base.HandleWaveControlMouseMove(sender, e);
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetWaveControlToolTip(Point mouseLocation)
		{
			var segMouseOver = _waveControl.GetSegmentForX(mouseLocation.X);

			if (segMouseOver >= 0)
			{
				if (PlayOrigButtonRectangle.Contains(mouseLocation))
				{
					var toolTipText = base.GetWaveControlToolTip(mouseLocation);
					if (ViewModel.CurrentUnannotatedSegment != null &&
						ViewModel.GetSegment(segMouseOver) == ViewModel.CurrentUnannotatedSegment &&
						_labelListenButton.Enabled)
					{
						toolTipText += " " + LocalizationManager.GetString(
							"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.PlayOriginalShortcutToolTipHint",
							"Keyboard shortcut: 'b'");
					}
					return toolTipText;
				}
				if (PlayAnnotationButtonRectangle.Contains(mouseLocation))
				{
					return LocalizationManager.GetString(
						"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.PlayAnnotationToolTipMsg",
						"Listen to this annotation.");
				}
				if (!ViewModel.GetDoesSegmentHaveAnnotationFile(segMouseOver))
				{
					return (ViewModel.GetIsSegmentIgnored(segMouseOver)) ?
						LocalizationManager.GetString(
							"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.SkippedSegmentToolTipMsg",
							"This segment was skipped.") :
						LocalizationManager.GetString(
							"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.NoAnnotationToolTipMsg",
							"This segment does not have a recorded annotation.");
				}
				if (GetRerecordButtonRectangleForSegmentMouseIsOver().Contains(mouseLocation))
				{
					return LocalizationManager.GetString(
						"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RerecordAnnotationToolTipMsg",
						"To erase the recorded annotation for this segment and record a new one, press and hold this button.");
				}
			}

			return base.GetWaveControlToolTip(mouseLocation);
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleWaveControlMouseClick(object sender, MouseEventArgs e)
		{
			// Don't call base.

			if (ViewModel.GetIsRecording())
				return;

			bool playSource = PlayOrigButtonRectangle.Contains(e.Location);
			bool playAnnotation = PlayAnnotationButtonRectangle.Contains(e.Location);
			if (!playSource && !playAnnotation)
				return;

			// Normally playing gets stopped by mouse down, but if the mouse down happens
			// and then playing starts (e.g., by pressing 'b') before the mouse comes up,
			// we just ignore this.
			if (_waveControl.IsPlaying)
				return;

			var segMouseOver = _waveControl.GetSegmentForX(e.X);

			StopAnnotationPlayBackIfNeeded();

			var segment = ViewModel.GetSegment(segMouseOver);

			if (playSource)
				PlaySource(segment);
			else
			{
				_waveControl.EnsureTimeIsVisible(segment.TimeRange.Start, segment.TimeRange, true, true);

				_segmentWhoseAnnotationIsBeingPlayedBack = segment;

				KillRecordingErrorMessage();

				_annotationPlaybackLength = ViewModel.GetAnnotationFileAudioDuration(segment);

				ViewModel.StartAnnotationPlayback(segment, HandleAnnotationPlaybackProgress, () =>
				{
					_lastAnnotationPlaybackPosition = TimeSpan.Zero;
					_waveControl.InvalidateIfNeeded(GetBottomReservedRectangleForSegment(_segmentWhoseAnnotationIsBeingPlayedBack));
					_segmentWhoseAnnotationIsBeingPlayedBack = null;
					_waveControl.DiscardScrollCalculator();
				});
			}

			UpdateDisplay();
		}

		private void StopAnnotationPlayBackIfNeeded()
		{
			if (ViewModel.GetIsAnnotationPlaying())
			{
				ViewModel.StopAnnotationPlayback();
				_lastAnnotationPlaybackPosition = TimeSpan.Zero;
				_waveControl.InvalidateIfNeeded(GetAnnotationPlaybackRectangle());
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void PlaySource(AnnotationSegment segment)
		{
			ViewModel.PlaySource(_waveControl, ctrl =>
				{
					if (segment == null) // Play the source recording for the new segment.
						ctrl.Play(ViewModel.GetEndOfLastSegment(), ViewModel.NewSegmentEndBoundary);
					else
						ctrl.Play(segment.TimeRange);
				});
		}

		/// ------------------------------------------------------------------------------------
		private void HandleVideoHelpButtonClick(object sender, EventArgs e)
		{
			MessageBox.Show("Sorry, we don't actually have a video for this yet. Contributions welcome!");
			Analytics.Track("Video Help requested in " + ViewModel.AnnotationType + " dialog box.");
		}

		/// ------------------------------------------------------------------------------------
		protected override bool UndoImplemented
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleUndoButtonClick(object sender, EventArgs e)
		{
			UpdateDisplayForChangeInNewSegmentEndBoundary(delegate
			{
				var timeRangeToInvalidate = ViewModel.TimeRangeForUndo;
				int additionalPixelsToInvalidate = ViewModel.NextUndoItemIsAddition ? 2 : 0;

				ViewModel.Undo();
				_waveControl.Painter.SetIgnoredRegions(_viewModel.GetIgnoredSegmentRanges());
				SetModeToListenOrFinished();

				// If Undo causes an annotation to be removed for a pre-existing segment, that
				// segment will now be the current unnanotated segment
				if (ViewModel.CurrentUnannotatedSegment != null)
					_waveControl.SetSelectionTimes(ViewModel.CurrentUnannotatedSegment.TimeRange, _selectedSegmentHighlighColor);

				if (_spaceBarMode == SpaceBarMode.Listen)
					ScrollInPreparationForListenOrRecord(_labelListenButton);
				_waveControl.InvalidateRegionBetweenTimes(timeRangeToInvalidate, additionalPixelsToInvalidate);
			});
		}

		/// ------------------------------------------------------------------------------------
		private void SetModeToListenOrFinished()
		{
			_spaceBarMode = ViewModel.GetIsFullyAnnotated() ? SpaceBarMode.Done : SpaceBarMode.Listen;
			if (_labelFinishedHint.Visible && _spaceBarMode != SpaceBarMode.Done)
			{
				_pictureIcon.Image = ResourceImageCache.Information_blue;
				_labelFinishedHint.Visible = false;
				_tableLayoutButtons.Controls.Remove(_labelFinishedHint);
				AcceptButton = null;

				SetTableLayoutForListenOrRecordMode();
			}
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetBottomReservedRectangleForSegment(AnnotationSegment segment)
		{
			return _waveControl.Painter.GetBottomReservedRectangleForTimeRange(segment.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		protected override int MarginFromBottomOfPlayOrigButton
		{
			get { return base.MarginFromBottomOfPlayOrigButton + AnnotationAreaHeight; }
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
		private void HandleWaveControlMouseDown(object sender, MouseEventArgs e)
		{
			if (!GetRerecordButtonRectangleForSegmentMouseIsOver().Contains(e.Location))
				return;

			if (ViewModel.Recorder == null || ViewModel.Recorder.GetIsInErrorState(true))
			{
				UdateErrorMessageDisplay();
				return;
			}

			var segMouseOver = HotSegment;
			_reRecording = true;
			BeginRecording(segMouseOver.TimeRange);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnDeactivate(EventArgs e)
		{
			if (_moreReliableDesignMode)
				return;

			if (ViewModel.GetIsRecording())
			{
				StopRecording(_reRecording ? AdvanceOptionsAfterRecording.DoNotAdvance :
					AdvanceOptionsAfterRecording.Advance);
			}
			base.OnDeactivate(e);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplayForNotRecording(AdvanceOptionsAfterRecording advanceOption)
		{
			_pictureRecording.Visible = false;
			_waveControl.SelectSegmentOnMouseOver = true;

			if (_reRecording)
			{
				var rc = _waveControl.Painter.GetBottomReservedRectangleForTimeRange(ViewModel.GetSelectedTimeRange());
				_waveControl.InvalidateIfNeeded(rc);
				_reRecording = false;
			}

			_advanceOption = advanceOption;
		}

		/// ------------------------------------------------------------------------------------
		private void StopRecording(AdvanceOptionsAfterRecording advanceOption)
		{
			UpdateDisplayForNotRecording(advanceOption);
			ViewModel.StopAnnotationRecording();
		}

		/// ------------------------------------------------------------------------------------
		private void RecordingCompleted(StopAnnotationRecordingResult stopResult)
		{
			_waveControl.InvalidateIfNeeded(GetVisibleAnnotationRectangleForSegmentBeingRecorded());

			if (stopResult != StopAnnotationRecordingResult.Normal)
			{
				if (stopResult == StopAnnotationRecordingResult.AnnotationTooShort)
					DisplayRecordingTooShortMessage();
				else if (stopResult == StopAnnotationRecordingResult.RecordingAborted)
				{
					ShowRecordingInterruptedMessage();
				}

				_segmentBeingRecorded = null;
				return;
			}

			if (_advanceOption == AdvanceOptionsAfterRecording.AdvanceOnlyToContiguousUnsegmentedAudio &&
				ViewModel.GetEndOfLastSegment() == _segmentBeingRecorded.End)
			{
				_advanceOption = AdvanceOptionsAfterRecording.Advance;
			}

			_segmentBeingRecorded = null;

			if (ViewModel.CurrentUnannotatedSegment == null)
			{
				_waveControl.SegmentBoundaries = ViewModel.MakeSegmentForEndBoundary();
				_waveControl.SetCursor(TimeSpan.FromSeconds(1).Negate());
			}

			if (_advanceOption == AdvanceOptionsAfterRecording.Advance)
			{
				_userHasListenedToSelectedSegment = false;
				GoToNextUnannotatedSegment();
			}
			else
			{
				if (_advanceOption == AdvanceOptionsAfterRecording.AdvanceOnlyToContiguousUnsegmentedAudio)
				{
					// Though we don't actually want to highlight the segment or move to it yet, we
					// want to set this, so that if the user presses the space bar or mouses over the
					// listen button, they will advance to the next segment.
					ViewModel.SetNextUnannotatedSegment();
				}
				_spaceBarMode = ViewModel.GetIsFullyAnnotated() ? SpaceBarMode.Done : SpaceBarMode.Listen;
			}

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void ShowRecordingInterruptedMessage()
		{
			if (_spaceBarIsDown)
				_needToShowRecordingAbortedMessage = true;
			else
			{
				MessageBox.Show(this, LocalizationManager.GetString(
					"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingAbortedMessage",
					"The recording was interrupted by a change in the default recording device.\n" +
					"This can happen if you turn off or disconnect the microphone.\n" +
					"After you correct the problem, click OK, and then record again."),
								LocalizationManager.GetString(
									"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingAbortedCaption",
									"Recording Failed"));
				_needToShowRecordingAbortedMessage = false;
				UpdateDisplay();
			}
		}

		/// ------------------------------------------------------------------------------------
		void SelectedRecordingDeviceChanged(object sender, EventArgs e)
		{
			KillRecordingErrorMessage();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void KillRecordingErrorMessage()
		{
			if (_recordingErrorMessage != null)
			{
				_recordingErrorMessage = null;
				UpdateDisplay();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DisplayRecordingTooShortMessage()
		{
			_recordingErrorMessage = (ViewModel.GetSelectedTimeRange() == _segmentBeingRecorded) ?
				LocalizationManager.GetString(
				"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingTooShortMessage.WhenSpaceOrMouseIsValid",
				"Whoops. You need to hold down the SPACE BAR or mouse button while talking.")
				: LocalizationManager.GetString(
				"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordingTooShortMessage.WhenOnlyMouseIsValid",
				"Whoops. You need to hold down the mouse button while talking.");
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleAnnotationPlaybackProgress(PlaybackProgressEventArgs args)
		{
			var newX = GetAnnotationPlaybackCursorX(args.PlaybackPosition);

			if (newX != GetAnnotationPlaybackCursorX(_lastAnnotationPlaybackPosition))
			{
				_lastAnnotationPlaybackPosition = args.PlaybackPosition;
				try
				{
					if (_waveControl.EnsureXIsVisible(newX))
						Invoke((Action<Rectangle>)_waveControl.InvalidateIfNeeded, GetAnnotationPlaybackRectangle());
				}
				catch (ObjectDisposedException)
				{
					// This happened once. I haven't been able to reproduce it, but it's no big deal.
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private int GetAnnotationPlaybackCursorX(TimeSpan playbackPosition)
		{
			var rc = GetAnnotationPlaybackRectangle();
			var pixelPerMillisecond = _annotationPlaybackLength.TotalMilliseconds.Equals(0) ? 0 :
				rc.Width / _annotationPlaybackLength.TotalMilliseconds;

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

				if (!areaRectangle.IntersectsWith(rc) || !ViewModel.GetDoesSegmentHaveAnnotationFile(i))
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
		private void DrawOralAnnotationWave(PaintEventArgs e, Rectangle rc, AnnotationSegment segment)
		{
			// The reason we wrap this in a try/catch block is because in some rare cases
			// when an audio error occurs (e.g. unplugging the mic. while recording) we'll
			// get to this method to paint the annotation before the original annotation
			// is restored because of the error.
			try
			{
				// Draw the oral annotation's wave in the bottom, reserved area of the wave control.
				using (var painter = new WavePainterBasic { ForeColor = Color.Black, BackColor = Color.Black })
				{
					painter.SetSamplesToDraw(ViewModel.GetSegmentSamples(segment, (uint)rc.Width));
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
			if ((_spaceBarMode == SpaceBarMode.Done || _spaceBarMode == SpaceBarMode.Error ||
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
		protected override void HandleWaveControlPostPaint(PaintEventArgs e)
		{
			// Don't call base.

			if (!ViewModel.GetIsRecording())
			{
				DrawPlayButtonInSegment(e.Graphics, PlayOrigButtonRectangle);
				var currentSegment = ViewModel.CurrentUnannotatedSegment;
				if (currentSegment != null && HotSegment != currentSegment)
				{
					DrawPlayButtonInSegment(e.Graphics,
						GetPlayOrigButtonRectangleForSegment(WavePainter.GetFullRectangleForTimeRange(
						currentSegment.TimeRange)));
				}
				DrawPlayButtonInSegment(e.Graphics, PlayAnnotationButtonRectangle);
			}

			DrawRerecordButtonInSegment(e.Graphics);

			if (ViewModel.GetIsRecording())
				DrawTextInAnnotationWaveCellWhileRecording(e.Graphics);
			else if (ViewModel.Recorder == null || ViewModel.Recorder.RecordingState == RecordingState.Stopped || ViewModel.GetIsRecorderInErrorState())
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
		public int AnnotationAreaHeight
		{
			get { return _waveControl.BottomReservedAreaHeight; }
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetNewSegmentCursorRectangle()
		{
			if (_spaceBarMode == SpaceBarMode.Done || ViewModel.CurrentUnannotatedSegment != null)
				return Rectangle.Empty;
			var x = _waveControl.Painter.ConvertTimeToXCoordinate(ViewModel.NewSegmentEndBoundary);
			return new Rectangle(x - 1, 0, 3, _waveControl.ClientSize.Height - AnnotationAreaHeight);
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
		internal Rectangle GetNewSegmentRectangle()
		{
			if (!ViewModel.GetHasNewSegment())
				return Rectangle.Empty;

			return GetRectangleForTimeRangeBeyondEndOfLastSegment(ViewModel.NewSegmentEndBoundary);
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
		private void DrawRerecordButtonInSegment(Graphics g)
		{
			var rerecordButtonRect = GetRerecordButtonRectangleForSegmentMouseIsOver();
			if (rerecordButtonRect == Rectangle.Empty)
				return;

			var img = rerecordButtonRect.Contains(MousePositionInWaveControl) ?
				_hotRerecordAnnotationButton : _normalRerecordAnnotationButton;

			g.DrawImage(img, rerecordButtonRect);
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetRerecordButtonRectangleForSegmentMouseIsOver()
		{
			if ((ViewModel.GetIsRecording() && !_reRecording) || HotSegment == null ||
				_waveControl.IsPlaying || ViewModel.GetIsAnnotationPlaying())
			{
				return Rectangle.Empty;
			}

			var rc = HotSegmentRectangle;
			var rerecordButtonSize = _normalRerecordAnnotationButton.Size;

			if (rc.IsEmpty || rerecordButtonSize.Width + 6 > rc.Width ||
				(!GetDoesSegmentHaveAnnotationFile(HotSegment) && !ViewModel.GetIsRecording()))
			{
				return Rectangle.Empty;
			}

			return new Rectangle(rc.Right - 6 - rerecordButtonSize.Width,
				rc.Bottom - 5 - rerecordButtonSize.Height,
				rerecordButtonSize.Width, rerecordButtonSize.Height);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesSegmentHaveAnnotationFile(AnnotationSegment segment)
		{
			return ViewModel.GetDoesSegmentHaveAnnotationFile(segment);
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

			ViewModel.PlaySource(_waveControl, ctrl =>
				{
					if (ViewModel.CurrentUnannotatedSegment == null)
						_waveControl.Play(ViewModel.NewSegmentEndBoundary);
					else
					{
						if (ViewModel.CurrentUnannotatedSegment.TimeRange.Contains(_waveControl.GetCursorTime()))
							_waveControl.Play(_waveControl.GetCursorTime(), ViewModel.CurrentUnannotatedSegment.TimeRange.End);
						else
							_waveControl.Play(ViewModel.CurrentUnannotatedSegment.TimeRange);
					}
				});
		}

		/// ------------------------------------------------------------------------------------
		public void HandleRecordingError(Exception e)
		{
			_listeningOrRecordingUsingSpaceBar = false;
			UpdateDisplayForNotRecording(AdvanceOptionsAfterRecording.DoNotAdvance);
			_waveControl.Invalidate();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		public void HandlePlaybackError(Exception e)
		{
			_listeningOrRecordingUsingSpaceBar = false;
			_waveControl.Stop();
			_waveControl.Invalidate();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordAnnotationMouseDown(object sender, MouseEventArgs e)
		{
			ScrollInPreparationForListenOrRecord(_labelRecordButton);

			// SP-703: Presumably some sort of NAudio error got the Recorder to be set to null
			// but the exact timing somehow still allowed us to get here. If we upgrade to
			// NAudio 1.6 or later, we probably won't need this check.
			// We've upgraded to 1.6, but I'm just going to leave this in here because I don't
			// know if it's really fixed.
			if (ViewModel.Recorder == null)
				return;
			if (ViewModel.Recorder.GetIsInErrorState(true))
				UdateErrorMessageDisplay();
			else
				BeginRecording(ViewModel.GetSelectedTimeRange());
		}

		/// ------------------------------------------------------------------------------------
		private void BeginRecording(TimeRange timeRangeOfSourceBeingAnnotated)
		{
			if (!ViewModel.BeginAnnotationRecording(timeRangeOfSourceBeingAnnotated))
			{
				_listeningOrRecordingUsingSpaceBar = false;
				return;
			}

			KillRecordingErrorMessage();

			UpdateDisplay();

			if (ViewModel.CurrentUnannotatedSegment != null)
				_waveControl.InvalidateIfNeeded(GetFullRectangleForTimeRange(ViewModel.CurrentUnannotatedSegment.TimeRange));

			if (HotSegment != null && HotSegment != ViewModel.CurrentUnannotatedSegment)
				_waveControl.InvalidateIfNeeded(HotSegmentRectangle);

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

			BeginInvoke((Action<Rectangle>)_waveControl.InvalidateIfNeeded,
				GetVisibleAnnotationRectangleForSegmentBeingRecorded());
		}

		/// ------------------------------------------------------------------------------------
		private void GoToNextUnannotatedSegment()
		{
			TimeRange timeRange;
			if (ViewModel.SetNextUnannotatedSegment())
			{
				timeRange = ViewModel.CurrentUnannotatedSegment.TimeRange;
				_waveControl.SetSelectionTimes(timeRange, _selectedSegmentHighlighColor);
				_waveControl.EnsureTimeIsVisible(timeRange.Start, timeRange, true, false);
			}
			else
			{
				_waveControl.SetSelectionTimes(new TimeRange(TimeSpan.Zero, TimeSpan.Zero),
					_selectedSegmentHighlighColor);
				timeRange = new TimeRange(ViewModel.GetEndOfLastSegment(), ViewModel.OrigWaveStream.TotalTime);
				_waveControl.InvalidateIfNeeded(GetNewSegmentCursorRectangle());
			}
			SetModeToListenOrFinished();
			if (_spaceBarMode == SpaceBarMode.Listen)
			{
				_waveControl.SetCursor(timeRange.Start);
				_waveControl.EnsureTimeIsVisible(timeRange.Start, timeRange, true, false);
			}
		}
		#endregion

		#region Low level keyboard handling
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (e.KeyChar == 'b' && _labelListenButton.Enabled &&
				(ViewModel.CurrentUnannotatedSegment != null || ViewModel.GetHasNewSegment()) &&
				!_waveControl.IsPlaying)
			{
				StopAnnotationPlayBackIfNeeded();
				PlaySource(ViewModel.CurrentUnannotatedSegment);
			}
			base.OnKeyPress(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyDown(Keys key)
		{
			if (key == Keys.Space)
				_spaceBarIsDown = true;

			if (!ContainsFocus || _waveControl.IsBoundaryMovingInProgress)
				return true;

			if (key == Keys.Space)
			{
				if (_listeningOrRecordingUsingSpaceBar || IsBoundaryMovingInProgressUsingArrowKeys)
					return true;

				_listeningOrRecordingUsingSpaceBar = true;

				if (_spaceBarMode == SpaceBarMode.Record &&
					(_labelRecordHint.Visible || (_labelErrorInfo.Visible && _labelErrorInfo.Text == _recordingErrorMessage)))
				{
					HandleRecordAnnotationMouseDown(null, null);
				}
				else if (_labelListenHint.Visible && _spaceBarMode == SpaceBarMode.Listen)
				{
					HandleListenToSourceMouseDown(null, null);
				}

				return true;
			}

			//if (key == Keys.End && !_waveControl.IsPlaying)
			//{
			//    _waveControl.SetCursor(ViewModel.GetEndOfLastSegment());
			//    UpdateDisplay();
			//    return true;
			//}

			return base.OnLowLevelKeyDown(key);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyUp(Keys key)
		{
			if (key == Keys.Space)
			{
				_spaceBarIsDown = false;
				if (_needToShowRecordingAbortedMessage)
					ShowRecordingInterruptedMessage();
			}

			if (!ContainsFocus)
				return true;

			if (key == Keys.Space)
			{
				if (!IsBoundaryMovingInProgressUsingArrowKeys && _listeningOrRecordingUsingSpaceBar)
				{
					_listeningOrRecordingUsingSpaceBar = false;

					if (_playingBackUsingHoldDownButton)
					{
						_newSegmentDefinedBy = SegmentDefinitionMode.HoldingSpace;
						FinishListeningUsingEarOrSpace();
					}
					else if (!_reRecording && ViewModel.GetIsRecording())
					{
						StopRecording(AdvanceOptionsAfterRecording.Advance);
					}
				}

				return true;
			}

			return base.OnLowLevelKeyUp(key);
		}
		#endregion

		#region Hot segment - the segment the mouse is over
		/// ------------------------------------------------------------------------------------
		protected override TimeRange CurrentTimeRange
		{
			get { return WavePainter.DefaultSelectedRange; }
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle PlayAnnotationButtonRectangle
		{
			get
			{
				var rc = HotSegmentRectangle;
				var hotSegment = HotSegment;

				if (hotSegment == null || !GetDoesSegmentHaveAnnotationFile(hotSegment) ||
				    rc.IsEmpty || _playButtonSize.Width + 6 > rc.Width)
					return Rectangle.Empty;

				// SP-1000: Reduce minimum segment size
				// Reducing the minimum segment size below 850 ms requires repositioning the Play Annotation button
				if (rc.Width < 80)
					return new Rectangle(rc.Right - 6 - _playButtonSize.Width, rc.Bottom - 40 - _playButtonSize.Height,
						_playButtonSize.Width, _playButtonSize.Height);

				return new Rectangle(rc.X + 6, rc.Bottom - 5 - _playButtonSize.Height,
					_playButtonSize.Width, _playButtonSize.Height);
			}
		}
		#endregion
	}
}
