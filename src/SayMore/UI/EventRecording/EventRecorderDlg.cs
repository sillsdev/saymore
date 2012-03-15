using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using Localization.UI;
using SayMore.Properties;
using SayMore.Media.UI;
using SilTools;
using NoToolStripBorderRenderer = SilTools.NoToolStripBorderRenderer;

namespace SayMore.UI.EventRecording
{
	/// ----------------------------------------------------------------------------------------
	public partial class EventRecorderDlg : Form
	{
		private readonly EventRecorderDlgViewModel _viewModel;
		private string _recordedLengthLabelFormat;

		/// ------------------------------------------------------------------------------------
		public EventRecorderDlg()
		{
			InitializeComponent();

			_recordedLengthLabelFormat = _labelRecLength.Text;
			_labelRecLength.Text = string.Format(_recordedLengthLabelFormat, "00.0");

			if ((DesignMode || GetService(typeof(IDesignerHost)) != null) ||
				LicenseManager.UsageMode == LicenseUsageMode.Designtime)
			{
				return;
			}

			DoubleBuffered = true;

			_labelRecLength.Font = SystemFonts.MenuFont;
			_labelRecordingFormat.Font = SystemFonts.MenuFont;
			toolStrip1.Renderer = new NoToolStripBorderRenderer();
			_peakMeter.Start(33); //the number here is how often it updates

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
			_viewModel = viewModel;
			_viewModel.UpdateAction = UpdateDisplay;
			_viewModel.Recorder.PeakLevelChanged += ((s, e) => _peakMeter.PeakLevel = e.Level);
			_viewModel.Recorder.RecordingProgress += (s, e) =>
				_labelRecLength.Text = string.Format(_recordedLengthLabelFormat,
					MediaPlayerViewModel.MakeTimeString((float)e.RecordedLength.TotalSeconds));

			_recDeviceButton.Recorder = _viewModel.Recorder;
			LocalizeItemDlg.StringsLocalized += delegate { _recordedLengthLabelFormat = _labelRecLength.Text; };
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			Settings.Default.EventRecorderDlg.InitializeForm(this);
			base.OnShown(e);
			UpdateDisplay();
			_viewModel.Recorder.BeginMonitoring();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (_viewModel.IsRecording)
				_viewModel.Recorder.Stop();

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
			if (_viewModel.IsRecording)
				_viewModel.Recorder.Stop();
			else
				_viewModel.StopPlayback();

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
