using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Localization;
using SayMore.Media.FFmpeg;
using SayMore.Properties;
using SayMore.Utilities;
using SilTools;

namespace SayMore.UI
{
	public partial class ConvertMediaDlg : Form
	{
		private readonly ConvertMediaDlgViewModel _viewModel;
		private string _conversionInProgressStatusFormat;
		private int _prevConvertedSeconds;
		private bool _showOutput = false;

		/// ------------------------------------------------------------------------------------
		public static string Show(string inputFile, string initialConversionName)
		{
			var viewModel = new ConvertMediaDlgViewModel(inputFile, initialConversionName);
			using (var dlg = new ConvertMediaDlg(viewModel))
			{
				return (dlg.ShowDialog() == DialogResult.Cancel ?
					null : viewModel.GetOutputFileName(true));
			}
		}

		/// ------------------------------------------------------------------------------------
		public ConvertMediaDlg()
		{
			InitializeComponent();
			DialogResult = DialogResult.Cancel;
		}

		/// ------------------------------------------------------------------------------------
		public ConvertMediaDlg(ConvertMediaDlgViewModel viewModel) : this()
		{
			_viewModel = viewModel;
			_showOutput = Settings.Default.ShowFFmpegDetailsWhenConvertingMedia;

			if (Settings.Default.ConvertMediaDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.ConvertMediaDlg = FormSettings.Create(this);
			}

			InitializeFonts();
			_labelFileToConvertValue.Text = Path.GetFileName(_viewModel.InputFile);
			_comboAvailableConversions.Items.AddRange(_viewModel.AvailableConversions);
			_comboAvailableConversions.SelectedItem = _viewModel.SelectedConversion;
			_comboAvailableConversions.SelectionChangeCommitted += delegate
			{
				_viewModel.SelectedConversion = _comboAvailableConversions.SelectedItem as FFmpegConversionInfo;
				UpdateDisplay();
			};

			_buttonBeginConversion.Click += HandleBeginConversionClick;
			_buttonCancel.Click += delegate { _viewModel.Cancel(); };
			_buttonClose.Click += delegate { Close(); };
			_buttonDownload.Click += delegate
			{
				_viewModel.DownloadFFmpeg();
				UpdateDisplay();
			};

			_buttonShowOutput.Click += delegate
			{
				_showOutput = true;
				UpdateDisplay();
			};

			_buttonHideOutput.Click += delegate
			{
				_showOutput = false;
				UpdateDisplay();
			};

			_labelDownloadNeeded.Tag = _labelDownloadNeeded.Text;
			Localization.UI.LocalizeItemDlg.StringsLocalized += delegate
			{
				_labelDownloadNeeded.Tag = _labelDownloadNeeded.Text;
				UpdateDisplay();
			};

			UpdateDisplay();
			Program.SuspendBackgroundProcesses();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.ConvertMediaDlg.InitializeForm(this);
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			Settings.Default.ShowFFmpegDetailsWhenConvertingMedia = _showOutput;
			Program.ResumeBackgroundProcesses(true);

			if (_viewModel.ConversionState == ConvertMediaUIState.FinishedConverting)
				DialogResult = DialogResult.OK;

			base.OnClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeFonts()
		{
			_textBoxOutput.Font = Program.DialogFont;
			_labelOverview.Font = Program.DialogFont;
			_labelAvailableConversions.Font = Program.DialogFont;
			_labelFileToConvert.Font = Program.DialogFont;
			_labelFileToConvertValue.Font = FontHelper.MakeFont(Program.DialogFont, FontStyle.Bold);
			_labelDownloadNeeded.Font = Program.DialogFont;
			_comboAvailableConversions.Font = Program.DialogFont;
			_labelStatus.Font = _labelFileToConvertValue.Font;
		}

		/// ------------------------------------------------------------------------------------
		public static string GetFactoryMp4ConversionName()
		{
			return LocalizationManager.GetString(
				"DialogBoxes.ConvertMediaDlg.ConvertToMp4AndAACConversionName",
				"Convert to mpeg4 with AAC Audio");
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_labelDownloadNeeded.Text = string.Format(_labelDownloadNeeded.Tag as string,
				_comboAvailableConversions.SelectedItem);

			_buttonBeginConversion.Enabled =
				(_viewModel.ConversionState == ConvertMediaUIState.WaitingToConvert &&
				_comboAvailableConversions.SelectedItem != null);

			_tableLayoutFFmpegMissing.Visible = (_viewModel.ConversionState == ConvertMediaUIState.FFmpegDownloadNeeded);

			_progressBar.Visible = (_viewModel.ConversionState == ConvertMediaUIState.Converting);

			_buttonCancel.Visible = (_viewModel.ConversionState == ConvertMediaUIState.Converting);
			_buttonClose.Visible = (_viewModel.ConversionState != ConvertMediaUIState.Converting);

			_labelStatus.Visible =
				(_viewModel.ConversionState != ConvertMediaUIState.FFmpegDownloadNeeded &&
				_viewModel.ConversionState != ConvertMediaUIState.WaitingToConvert);

			_buttonShowOutput.Visible = (!_showOutput &&
				(_viewModel.ConversionState != ConvertMediaUIState.FFmpegDownloadNeeded &&
				_viewModel.ConversionState != ConvertMediaUIState.WaitingToConvert));

			_buttonHideOutput.Visible = (_showOutput &&
				(_viewModel.ConversionState != ConvertMediaUIState.FFmpegDownloadNeeded &&
				_viewModel.ConversionState != ConvertMediaUIState.WaitingToConvert));

			_textBoxOutput.Visible = _buttonHideOutput.Visible;

			switch (_viewModel.ConversionState)
			{
				case ConvertMediaUIState.ConversionCancelled:
					_labelStatus.ForeColor = Color.Red;
					_labelStatus.TextAlign = ContentAlignment.MiddleCenter;
					_labelStatus.Text = LocalizationManager.GetString(
						"DialogBoxes.ConvertMediaDlg.ConversionCancelledMsg",
						"Conversion Cancelled.");
					break;

				case ConvertMediaUIState.ConversionFailed:
					_labelStatus.ForeColor = Color.Red;
					_labelStatus.TextAlign = ContentAlignment.MiddleCenter;
					_labelStatus.Text = LocalizationManager.GetString(
						"DialogBoxes.ConvertMediaDlg.ConversionFailedMsg",
						"Conversion Failed.");
					break;

				case ConvertMediaUIState.FinishedConverting:
					_labelStatus.ForeColor = Color.Green;
					_labelStatus.TextAlign = ContentAlignment.MiddleCenter;
					_labelStatus.Text = LocalizationManager.GetString(
						"DialogBoxes.ConvertMediaDlg.ConversionSucceededMsg",
						"Conversion Finished Successfully.");
					break;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBeginConversionClick(object sender, EventArgs e)
		{
			if (!CheckOutputFileAlreadyExists())
				return;

			_progressBar.Maximum = (int)_viewModel.MediaInfo.Duration.TotalSeconds;

			_conversionInProgressStatusFormat = LocalizationManager.GetString(
				"DialogBoxes.ConvertMediaDlg.ConversionProgressStatusMsgFormat",
				"{0} of {1} Converted...");

			_viewModel.BeginConversion(HandleUpdateDisplayDuringConversion);
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private bool CheckOutputFileAlreadyExists()
		{
			var outputFile = _viewModel.GetOutputFileName(false);

			if (!File.Exists(outputFile))
				return true;

			var msg = LocalizationManager.GetString(
				"DialogBoxes.ConvertMediaDlg.OutputFileAlreadyExistsMsg",
				"This conversion will create the output file '{0}', " +
				"but that file already exists. If you continue, the file will be overwritten." +
				"\r\n\r\nDo you want to continue?");

			msg = string.Format(msg, outputFile);

			var result = MessageBox.Show(this, msg, Application.ProductName,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (result == DialogResult.No)
				return false;

			FileSystemUtils.WaitForFileRelease(outputFile);
			File.Delete(outputFile);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleUpdateDisplayDuringConversion(TimeSpan convertedSoFar, string rawData)
		{
			if (InvokeRequired)
				Invoke((Action)(() => UpdateDisplayDuringConversion(convertedSoFar, rawData)));
			else
				UpdateDisplayDuringConversion(convertedSoFar, rawData);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplayDuringConversion(TimeSpan convertedSoFar, string rawData)
		{
			if (!_labelStatus.Visible)
				UpdateDisplay();

			_textBoxOutput.AppendText(rawData + Environment.NewLine);

			if (_prevConvertedSeconds == _progressBar.Maximum ||
				_prevConvertedSeconds + 10 < convertedSoFar.TotalSeconds)
			{
				return;
			}

			_prevConvertedSeconds = (int)convertedSoFar.TotalSeconds;
			_progressBar.Value = Math.Min(_progressBar.Maximum, _prevConvertedSeconds);
			_labelStatus.Text = string.Format(_conversionInProgressStatusFormat,
				convertedSoFar.ToString(@"hh\:mm\:ss"),
				_viewModel.MediaInfo.Duration.ToString(@"hh\:mm\:ss"));
		}
	}
}
