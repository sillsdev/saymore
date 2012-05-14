using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using Localization.UI;
using Palaso.Media.Naudio.UI;
using SayMore.Media.Audio;
using SayMore.Properties;
using SayMore.Media.MPlayer;
using SilTools;
using NoToolStripBorderRenderer = SilTools.NoToolStripBorderRenderer;

namespace SayMore.UI.EventRecording
{
	/// ----------------------------------------------------------------------------------------
	public partial class EventRecorderDlg : Form
	{
		private readonly EventRecorderDlgViewModel _viewModel;
		private string _recordedLengthLabelFormat;
		private readonly bool _moreReliableDesignMode;
		private PeakMeterCtrl _peakMeter;
		private RecordingDeviceButton _recDeviceButton;

		/// ------------------------------------------------------------------------------------
		public EventRecorderDlg()
		{
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

			if (Settings.Default.EventRecorderDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.EventRecorderDlg = FormSettings.Create(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		public EventRecorderDlg(EventRecorderDlgViewModel viewModel) : this()
		{
			if (_moreReliableDesignMode)
				return;

			_viewModel = viewModel;
			_viewModel.UpdateAction = UpdateDisplay;
			_viewModel.Recorder.PeakLevelChanged += ((s, e) => _peakMeter.PeakLevel = e.Level);
			_viewModel.Recorder.RecordingProgress += (s, e) =>
				_labelRecLength.Text = string.Format(_recordedLengthLabelFormat,
					MediaPlayerViewModel.MakeTimeString((float)e.RecordedLength.TotalSeconds));

			_peakMeter = AudioUtils.CreatePeakMeterControl(_panelPeakMeter);
			SetupRecordingDeviceButton();

			LocalizeItemDlg.StringsLocalized += delegate { _recordedLengthLabelFormat = _labelRecLength.Text; };
		}

		/// ------------------------------------------------------------------------------------
		private void SetupRecordingDeviceButton()
		{
			_recDeviceButton = new RecordingDeviceButton();
			_recDeviceButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			_recDeviceButton.AutoSize = true;
			_recDeviceButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			_recDeviceButton.Margin = new Padding(0);
			tableLayoutPanel1.Controls.Add(_recDeviceButton, 0, 3);
			tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.AutoSize;
			_recDeviceButton.Recorder = _viewModel.Recorder;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			Settings.Default.EventRecorderDlg.InitializeForm(this);
			base.OnShown(e);

			if (_moreReliableDesignMode)
				return;

			UpdateDisplay();
			_viewModel.Recorder.BeginMonitoring();
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
			_viewModel.BeginRecording();
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
