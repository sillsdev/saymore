using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DesktopAnalytics;
using L10NSharp;
using SIL.Media.Naudio.UI;
using SIL.Reporting;
using SIL.Windows.Forms.PortableSettingsProvider;
using SayMore.Media.Audio;
using SayMore.Properties;
using SayMore.Media.MPlayer;
using SayMore.UI;
using SayMore.Utilities;
using SIL.Media;
using SIL.Windows.Forms;

namespace SayMore.UI.SessionRecording
{
	/// ----------------------------------------------------------------------------------------
	public partial class SessionRecorderDlg : SayMoreFormBase
	{
		// Extra room (beyond the fixed-size icon and the single-line text itself) left around
		// a recording ToolStripButton's content when growing it to fit the current text scale;
		// enough to read as normal button padding rather than text crowding the button's edge.
		private const int kToolStripButtonContentGap = 4;
		private const int kToolStripButtonRightPadding = 6;
		private const int kToolStripButtonVerticalSlack = 2;

		private readonly SessionRecorderDlgViewModel _viewModel;
		private string _recordedLengthLabelFormat;
		private readonly bool _moreReliableDesignMode;
		private readonly PeakMeterCtrl _peakMeter;
		private RecordingDeviceIndicator _recDeviceIndicator;

		// Captured once, immediately after InitializeComponent() and before anything has a
		// chance to scale a font or resize the window, so later layout passes always have the
		// dialog's true, pristine Designer-authored baseline to measure against instead of
		// whatever an earlier pass already changed it to.
		private readonly (ToolStripButton Button, Size DesignTimeSize)[] _scalableToolStripButtons;
		private readonly int _designTimeClientHeight;
		private readonly int _designTimeMinimumHeight;
		private bool _resizingRecordingControlsForTextScale;

		/// ------------------------------------------------------------------------------------
		public SessionRecorderDlg()
		{
			Logger.WriteEvent("SessionRecorderDlg constructor");

			InitializeComponent();

			_scalableToolStripButtons = new[]
			{
				(_buttonRecord, _buttonRecord.Size),
				(_buttonPlayback, _buttonPlayback.Size),
				(_buttonStop, _buttonStop.Size)
			};
			_designTimeClientHeight = ClientSize.Height;
			_designTimeMinimumHeight = MinimumSize.Height;

			_recordedLengthLabelFormat = _labelRecLength.Text;
			_labelRecLength.Text = string.Format(_recordedLengthLabelFormat, "00.0");

			_moreReliableDesignMode = (DesignMode || GetService(typeof(IDesignerHost)) != null) ||
				(LicenseManager.UsageMode == LicenseUsageMode.Designtime);

			if (_moreReliableDesignMode)
				return;

			DoubleBuffered = true;

			_labelRecLength.Font = Program.DialogFont;
			_labelRecordingFormat.Font = Program.DialogFont;
			toolStrip1.Renderer = new NoToolStripBorderRenderer();

			_buttonOK.Click += delegate
			{
				DialogResult = DialogResult.OK;
				Close();
			};

			_buttonCancel.Click += delegate
			{
				DialogResult = DialogResult.Abort;
				Close();
			};

			if (Settings.Default.SessionRecorderDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.SessionRecorderDlg = FormSettings.Create(this);
			}

			UITextScaler.ScaleChanged += HandleUiTextScaleChangedForRecordingControls;
			Disposed += (s, e) => UITextScaler.ScaleChanged -= HandleUiTextScaleChangedForRecordingControls;

			// The Format label (row3) can also wrap to a second line purely from the dialog
			// being resized narrower by hand, with no text-scale change involved; re-running
			// the same fit-to-content pass on Resize keeps every row -- not just the ones a
			// scale change touches -- free of the TableLayoutPanel-clamping this method guards
			// against (see its own doc comment) regardless of what triggered the wrap.
			Resize += (s, e) => ResizeRecordingControlsForCurrentTextScale();
		}

		/// ------------------------------------------------------------------------------------
		public SessionRecorderDlg(SessionRecorderDlgViewModel viewModel) : this()
		{
			if (_moreReliableDesignMode)
				return;

			_viewModel = viewModel;
			_viewModel.UpdateAction += delegate
			{
				if (InvokeRequired)
				{
					BeginInvoke((Action)UpdateDisplay);
					return;
				}
				UpdateDisplay();
			};
			_viewModel.Recorder.PeakLevelChanged += ((s, e) => _peakMeter.PeakLevel = e.Level);
			_viewModel.Recorder.RecordingProgress += HandleRecorderProgress;

			_peakMeter = AudioUtils.CreatePeakMeterControl(_panelPeakMeter);
			SetupRecordingDeviceButton();

		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecorderProgress(object sender, RecordingProgressEventArgs e)
		{
			if (InvokeRequired)
			{
				BeginInvoke((Action)(() => UpdateRecordingDuration(e.RecordedLength)));
				return;
			}
			UpdateRecordingDuration(e.RecordedLength);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateRecordingDuration(TimeSpan duration)
		{
			_labelRecLength.Text = string.Format(_recordedLengthLabelFormat,
				MediaPlayerViewModel.MakeTimeString((float)duration.TotalSeconds));
		}

		/// ------------------------------------------------------------------------------------
		private void SetupRecordingDeviceButton()
		{
			_recDeviceIndicator = new RecordingDeviceIndicator();
			_recDeviceIndicator.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			_recDeviceIndicator.AutoSize = true;
			_recDeviceIndicator.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			_recDeviceIndicator.Margin = new Padding(0);
			tableLayoutPanel1.Controls.Add(_recDeviceIndicator, 0, 3);
			tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.AutoSize;
			_recDeviceIndicator.Recorder = _viewModel.Recorder;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (_moreReliableDesignMode)
				return;

			// base.OnLoad(e) just raised Load, which SayMoreFormBase's own handler used to
			// apply the current text scale's font to every control, including the recording
			// buttons and the Format/Recorded-Length labels. Re-measuring here, now that the
			// scaled fonts are in place, keeps the dialog in sync with that same pass from the
			// moment it first appears.
			ResizeRecordingControlsForCurrentTextScale();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			Settings.Default.SessionRecorderDlg.InitializeForm(this);
			base.OnShown(e);

			if (_moreReliableDesignMode)
				return;

			// Same safety-net reasoning as SayMoreFormBase's own Load/Shown pair
			// (src/SayMore/UI/SayMoreFormBase.cs): Shown is guaranteed to fire only after the
			// entire OnLoad chain has returned, so it catches anything a subclass set after
			// OnLoad's own call to ResizeRecordingControlsForCurrentTextScale above.
			ResizeRecordingControlsForCurrentTextScale();

			_viewModel.Recorder.BeginMonitoring();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_viewModel.CloseAll();
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordClick(object sender, EventArgs e)
		{
			int retry = 0;
			do
			{
				try
				{
					_viewModel.BeginRecording();
					break;
				}
				catch (IOException failure)
				{
					string failureMsg = LocalizationManager.GetString(
						"DialogBoxes.SessionRecorderDlg.UnableToStartRecording",
						"Unable to start recording.") + Environment.NewLine +
						failure.Message + Environment.NewLine +
						LocalizationManager.GetString(
						"DialogBoxes.SessionRecorderDlg.RetryAfterUnauthorizedAccessExceptionMsg",
						"If you can determine which program is using this file, close it and click Retry.");
					if (retry++ > 0)
					{
						if (MessageBox.Show(failureMsg, Application.ProductName, MessageBoxButtons.RetryCancel,
							MessageBoxIcon.Warning) == DialogResult.Cancel)
						{
							Analytics.Track("Session Recording Cancelled", new Dictionary<string, string> {
								{"failureMsg", failureMsg}, {"Automatic retries", "1"}, {"User-requested retries", (retry - 1).ToString()} });
							Analytics.ReportException(failure);
							retry = 0;
						}
					}
				}
			} while (retry > 0);

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackButtonClick(object sender, EventArgs e)
		{
			_viewModel.BeginPlayback();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleStopClick(object sender, EventArgs e)
		{
			_viewModel.Stop();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_labelRecordingFormat.Text = string.Format(_labelRecordingFormat.Text,
				_viewModel.Recorder.RecordingFormat.BitsPerSample, _viewModel.Recorder.RecordingFormat.SampleRate);

			_buttonRecord.Enabled = _viewModel.CanRecordNow;
			_buttonPlayback.Enabled = (_viewModel.CanPlay && !_viewModel.IsPlaying);
			_buttonStop.Enabled = (_viewModel.IsPlaying || _viewModel.IsRecording);

			_buttonOK.Enabled = !_viewModel.IsPlaying && !_viewModel.IsRecording && _viewModel.CanPlay;
			_buttonCancel.Enabled = !_viewModel.IsPlaying && !_viewModel.IsRecording;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleUiTextScaleChangedForRecordingControls(object sender, EventArgs e)
		{
			ResizeRecordingControlsForCurrentTextScale();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// _buttonRecord/_buttonPlayback/_buttonStop are ToolStripButtons with AutoSize=false
		/// and a Designer-fixed Size, so nothing resizes them in response to a Font change the
		/// way the generic recursive UI scaling pass does for an AutoSize control. Once the
		/// current Font's own single-line text height grows past the small, Designer-authored
		/// 24px budgeted for the unscaled font, the content that no longer fits -- most visibly
		/// the record/playback icon's own fixed, unstretched bottom edge, but also a descender
		/// on a letter like the "y" in "Play Back Recording" -- gets clipped against that frozen
		/// Size. Re-measuring each button's own single-line text against the current Font --
		/// forcing <see cref="TextFormatFlags.SingleLine"/> rather than
		/// <see cref="ToolStripItem.GetPreferredSize"/>, which can report an unconstrained,
		/// seemingly-wrapped size for a Table-layout ToolStripButton at large scales --
		/// plus its fixed-size icon, and growing -- never shrinking below the Designer's own
		/// baseline -- keeps the native-size icon and the full scaled text line, descenders
		/// included, inside the button's bounds at every scale.
		///
		/// Growing the buttons alone is not sufficient: toolStrip1 sits in
		/// tableLayoutPanel1's one Percent-sized row, which only ever receives whatever height
		/// is left over after the dialog's other, Designer-fixed-size rows (the Recorded-Length
		/// label and the OK/Cancel/Format row) take their own share -- and TableLayoutPanel
		/// clamps an AutoSize control's actual bounds to its row's available height, so
		/// toolStrip1 (and the buttons inside it) would still end up clipped at larger scales if
		/// the dialog's own ClientSize never grows to make that leftover large enough. Measuring
		/// every row's own current content height and growing ClientSize/MinimumSize to fit --
		/// again, only ever growing, never shrinking below the Designer's baseline -- keeps
		/// every row, not just the buttons themselves, free of this same frozen-container
		/// clipping at every scale.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ResizeRecordingControlsForCurrentTextScale()
		{
			// Re-entrancy guard: changing ClientSize.Height below raises this form's own
			// Resize event, which this dialog's constructor wires back to this same method.
			if (_resizingRecordingControlsForTextScale)
				return;

			_resizingRecordingControlsForTextScale = true;
			try
			{
				using (var g = CreateGraphics())
				{
					foreach (var (button, designTimeSize) in _scalableToolStripButtons)
					{
						var textSize = TextRenderer.MeasureText(g, button.Text, button.Font,
							Size.Empty, TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
						var imageSize = button.Image?.Size ?? Size.Empty;
						var neededWidth = imageSize.Width + kToolStripButtonContentGap +
							textSize.Width + kToolStripButtonRightPadding;
						var neededHeight = Math.Max(imageSize.Height, textSize.Height) +
							kToolStripButtonVerticalSlack;
						var targetSize = new Size(Math.Max(designTimeSize.Width, neededWidth),
							Math.Max(designTimeSize.Height, neededHeight));
						if (button.Size != targetSize)
							button.Size = targetSize;
					}
				}

				toolStrip1.PerformLayout();
				tableLayoutPanel1.PerformLayout();

				// Rows 2 and 3 are AutoSize rows: TableLayoutPanel always grants an AutoSize
				// row its own full natural height first -- including whatever height
				// _labelRecordingFormat's anchor-driven wrap just settled on above -- and only
				// the single Percent row (row1, toolStrip1/_panelPeakMeter's row) ever absorbs a
				// shortfall by being clamped smaller than its content needs. That means rows 2
				// and 3's own current (post-layout) height already reflects their true need
				// unclamped, unlike PreferredSize.Height, which -- for an anchored, wrapping
				// Label like _labelRecordingFormat -- reports its unconstrained single-line size
				// and misses however much taller its actual current wrap just made it. A row's
				// own height is its tallest control's Height *plus that control's own vertical
				// Margin* (TableLayoutPanel sizes an AutoSize row to fit the control plus its
				// margin, not the bare control alone), so every term below adds Margin.Vertical
				// alongside Height, the same way row1's sum already does via toolStripRowMargin.
				//
				// Row1's own need comes only from the three recording buttons: _panelPeakMeter
				// (row1's other occupant) is Anchor=Top|Bottom, so it always stretches to match
				// whatever height row1 ends up with rather than ever setting a floor of its own.
				// The Math.Max against _designTimeClientHeight below floors the dialog's whole
				// ClientSize, not row1's own share of it specifically -- once another row (2 or
				// 3) genuinely needs more room than the Designer baseline provided, row1 (and so
				// _panelPeakMeter) can end up with less of the resulting taller client than it
				// had at the original 100% layout. That's the correct trade, not a gap to guard
				// against: row1Needed is always recomputed fresh from the buttons' own current
				// Height each call, so it never reflects a stale or previously-grown value --
				// the room goes to whichever row's content actually needs it this time.
				const int toolStripRowMargin = 5; // matches each recording button's own Margin(0, 0, 0, 5)
				var row1Needed = _buttonRecord.Height + _buttonPlayback.Height + _buttonStop.Height +
					3 * toolStripRowMargin;
				var row2Needed = _labelRecLength.Height + _labelRecLength.Margin.Vertical;
				var row3Needed = Math.Max(Math.Max(
					_buttonOK.Height + _buttonOK.Margin.Vertical,
					_buttonCancel.Height + _buttonCancel.Margin.Vertical),
					_labelRecordingFormat.Height + _labelRecordingFormat.Margin.Vertical);
				var neededClientHeight = row1Needed + row2Needed + row3Needed +
					Padding.Top + Padding.Bottom;
				var targetClientHeight = Math.Max(_designTimeClientHeight, neededClientHeight);

				if (ClientSize.Height != targetClientHeight)
				{
					var growth = Math.Max(0, targetClientHeight - _designTimeClientHeight);
					MinimumSize = new Size(MinimumSize.Width, _designTimeMinimumHeight + growth);
					ClientSize = new Size(ClientSize.Width, targetClientHeight);
				}
			}
			finally
			{
				_resizingRecordingControlsForTextScale = false;
			}
		}
	}
}
