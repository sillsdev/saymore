using System;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Properties;
using SilTools;

namespace SayMore.UI.EventRecording
{
	/// ----------------------------------------------------------------------------------------
	public partial class EventRecorderDlg : Form
	{
		private readonly EventRecorderDlgViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		public EventRecorderDlg()
		{
			InitializeComponent();

			DoubleBuffered = true;

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
			_recDeviceButton.Recorder = _viewModel.Recorder;
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
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			base.OnClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordClick(object sender, EventArgs e)
		{
			_viewModel.BeginRecording();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleStopRecordingClick(object sender, EventArgs e)
		{
			_viewModel.Recorder.Stop();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackButtonClick(object sender, EventArgs e)
		{
			_viewModel.BeginPlayback();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleStopPlaybackClick(object sender, EventArgs e)
		{
			_viewModel.StopPlayback();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_labelRecordingFormat.Text = string.Format(_labelRecordingFormat.Text,
				_viewModel.Recorder.RecordingFormat.BitsPerSample, _viewModel.Recorder.RecordingFormat.SampleRate);

			_buttonRecord.Visible = !_viewModel.IsRecording;
			_buttonRecord.Enabled = _viewModel.CanRecordNow;
			_buttonPlayback.Visible = !_viewModel.IsPlaying;
			_buttonPlayback.Enabled = _viewModel.CanPlay;

			_buttonStopRecording.Visible = _viewModel.IsRecording;
			_buttonStopPlaying.Visible = _viewModel.IsPlaying;

			_buttonOK.Enabled = !_viewModel.IsPlaying && !_viewModel.IsRecording;
			_buttonCancel.Enabled = !_viewModel.IsPlaying && !_viewModel.IsRecording;
		}
	}
}
