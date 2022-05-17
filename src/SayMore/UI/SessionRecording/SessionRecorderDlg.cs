using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Forms;
using DesktopAnalytics;
using L10NSharp;
using L10NSharp.TMXUtils;
using L10NSharp.UI;
using SIL.Media.Naudio.UI;
using SIL.Reporting;
using SIL.Windows.Forms.PortableSettingsProvider;
using SayMore.Media.Audio;
using SayMore.Properties;
using SayMore.Media.MPlayer;
using SIL.Media;

namespace SayMore.UI.SessionRecording
{
	/// ----------------------------------------------------------------------------------------
	public partial class SessionRecorderDlg : Form
	{
		private readonly SessionRecorderDlgViewModel _viewModel;
		private string _recordedLengthLabelFormat;
		private readonly bool _moreReliableDesignMode;
		private readonly PeakMeterCtrl _peakMeter;
		private RecordingDeviceIndicator _recDeviceIndicator;

		/// ------------------------------------------------------------------------------------
		public SessionRecorderDlg()
		{
			Logger.WriteEvent("SessionRecorderDlg constructor");

			InitializeComponent();

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

			LocalizeItemDlg<TMXDocument>.StringsLocalized += HandleStringsLocalized;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleStringsLocalized(ILocalizationManager lm)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
				_recordedLengthLabelFormat = _labelRecLength.Text;
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
		protected override void OnShown(EventArgs e)
		{
			Settings.Default.SessionRecorderDlg.InitializeForm(this);
			base.OnShown(e);

			if (_moreReliableDesignMode)
				return;

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
	}
}
