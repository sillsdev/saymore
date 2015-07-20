using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.UI;
using SIL.Reporting;
using SIL.Windows.Forms;
using SIL.Windows.Forms.PortableSettingsProvider;
using SayMore.Media.FFmpeg;
using SayMore.Properties;

namespace SayMore.UI
{
	public partial class ConvertMediaDlg : Form
	{
		private readonly ConvertMediaDlgViewModel _viewModel;
		private string _conversionInProgressStatusFormat;
		private bool _showOutput;

		/// ------------------------------------------------------------------------------------
		public static string Show(string inputFile, string initialConversionName)
		{
			var viewModel = new ConvertMediaDlgViewModel(inputFile, initialConversionName);
			using (var dlg = new ConvertMediaDlg(viewModel))
				return (dlg.ShowDialog() == DialogResult.Cancel ? null : viewModel.OutputFileCreated);
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
			Logger.WriteEvent("ConvertMediaDlg constructor. file = {0}", viewModel.InputFile);
			_viewModel = viewModel;

			_showOutput = Settings.Default.ShowFFmpegDetailsWhenConvertingMedia;

			if (Settings.Default.ConvertMediaDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.ConvertMediaDlg = FormSettings.Create(this);
			}

			InitializeFonts();
			_labelFileToConvertValue.Text = Path.GetFileName(_viewModel.InputFile);
			if (_viewModel.MediaInfo == null)
			{
				_labelStatus.Text = LocalizationManager.GetString("DialogBoxes.ConvertMediaDlg.InvalidMediaFile",
					"File does not appear to be a valid media file.");
				_labelStatus.ForeColor = Color.Red;
			}
			else
			{
				_labelOutputFileValue.Text = _viewModel.GetNewOutputFileName(true);
				_comboAvailableConversions.Items.AddRange(_viewModel.AvailableConversions);
				_comboAvailableConversions.SelectedItem = _viewModel.SelectedConversion;
				_comboAvailableConversions.SelectionChangeCommitted += delegate
				{
					_viewModel.SelectedConversion = _comboAvailableConversions.SelectedItem as FFmpegConversionInfo;
					_labelOutputFileValue.Text = _viewModel.GetNewOutputFileName(true);
					UpdateDisplay();
				};

				_buttonBeginConversion.Click += HandleBeginConversionClick;
				_buttonCancel.Click += delegate { _viewModel.Cancel(); };

				_buttonCancel.MouseMove += delegate
				{
					// Not sure why both of these are necessary, but it seems to be the case.
					_buttonCancel.UseWaitCursor = false;
					_buttonCancel.Cursor = Cursors.Default;
				};
			}

			_buttonDownload.Click += delegate
			{
				using (var dlg = new FFmpegDownloadDlg())
					dlg.ShowDialog(this);
				_viewModel.SetConversionStateBasedOnPresenceOfFfmpegForSayMore();
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
			LocalizeItemDlg.StringsLocalized += HandleStringsLocalized;

			Program.SuspendBackgroundProcesses();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleStringsLocalized()
		{
			_labelDownloadNeeded.Tag = _labelDownloadNeeded.Text;
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.ConvertMediaDlg.InitializeForm(this);
			base.OnLoad(e);
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			Settings.Default.ShowFFmpegDetailsWhenConvertingMedia = _showOutput;
			Program.ResumeBackgroundProcesses(true);

			if ((_viewModel.ConversionState & ConvertMediaUIState.FinishedConverting) > 0)
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
			_labelOutputFile.Font = Program.DialogFont;
			_labelOutputFileValue.Font = _labelFileToConvertValue.Font;
			_labelDownloadNeeded.Font = Program.DialogFont;
			_comboAvailableConversions.Font = Program.DialogFont;
			_labelStatus.Font = _labelFileToConvertValue.Font;
		}

		/// ------------------------------------------------------------------------------------
		public static string GetFactoryConvertToH263Mp4ConversionName()
		{
			return LocalizationManager.GetString(
				"DialogBoxes.ConvertMediaDlg.ConvertToMp4AndAACConversionName",
				"Convert to H.263/MPEG-4 video with AAC Audio");
		}

		/// ------------------------------------------------------------------------------------
		public static string GetFactoryExtractToStandardPcmConversionName()
		{
			return LocalizationManager.GetString(
				"DialogBoxes.ConvertMediaDlg.ExtractStandardPcmFromVideoMenuText",
				"Extract audio to standard WAV PCM audio file");
		}

		/// ------------------------------------------------------------------------------------
		public static string GetFactoryExtractToMp3AudioConversionName()
		{
			return LocalizationManager.GetString(
			"CommonToMultipleViews.FileList.Convert.ExtractMp3AudioMenuText",
			"Extract audio to mono mp3 audio file (low quality)");
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_labelDownloadNeeded.Text = String.Format(_labelDownloadNeeded.Tag as string,
				_comboAvailableConversions.SelectedItem);

			_labelOutputFile.Visible = _labelOutputFileValue.Visible = _viewModel.MediaInfo != null;

			_buttonBeginConversion.Enabled =
				(_viewModel.ConversionState != ConvertMediaUIState.FFmpegDownloadNeeded &&
				(_viewModel.ConversionState & ConvertMediaUIState.Converting) == 0 &&
				(_viewModel.ConversionState & ConvertMediaUIState.FinishedConverting) == 0 &&
				_comboAvailableConversions.SelectedItem != null);

			_comboAvailableConversions.Enabled = _buttonBeginConversion.Enabled;
			_tableLayoutFFmpegMissing.Visible = (_viewModel.ConversionState == ConvertMediaUIState.FFmpegDownloadNeeded);
			_progressBar.Visible = (_viewModel.ConversionState & ConvertMediaUIState.Converting) > 0;
			_buttonClose.Visible = (_viewModel.ConversionState & ConvertMediaUIState.FinishedConverting) != 0;
			_buttonCancel.Visible = !_buttonClose.Visible;

			_labelStatus.Visible =
				(_viewModel.ConversionState != ConvertMediaUIState.FFmpegDownloadNeeded &&
				_viewModel.ConversionState != ConvertMediaUIState.WaitingToConvert);

			var outputAvailable = _viewModel.ConversionState != ConvertMediaUIState.FFmpegDownloadNeeded &&
				_viewModel.ConversionState != ConvertMediaUIState.WaitingToConvert &&
				_viewModel.ConversionState != ConvertMediaUIState.InvalidMediaFile;

			_buttonShowOutput.Visible = (!_showOutput && outputAvailable);
			_buttonHideOutput.Visible = (_showOutput && outputAvailable);

			if (_textBoxOutput.Visible != _buttonHideOutput.Visible)
			{
				_textBoxOutput.Visible = _buttonHideOutput.Visible;
				if (_textBoxOutput.Visible)
					MinimumSize = new Size(MinimumSize.Width, MinimumSize.Height + 50);
				else
					MinimumSize = new Size(MinimumSize.Width, MinimumSize.Height - 50);
			}

			if ((_viewModel.ConversionState & ConvertMediaUIState.AllFinishedStates) > 0)
				UpdateDisplayWhenConversionFinished();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplayWhenConversionFinished()
		{
			_labelStatus.ForeColor =
				((_viewModel.ConversionState & ConvertMediaUIState.FinishedConverting) > 0 ? Color.Green : Color.Red);

			switch (_viewModel.ConversionState & ConvertMediaUIState.AllFinishedStates)
			{
				case ConvertMediaUIState.ConversionCancelled:
					_viewModel.DeleteOutputFile();
					_labelStatus.Text = LocalizationManager.GetString(
						"DialogBoxes.ConvertMediaDlg.ConversionCancelledMsg",
						"Conversion Cancelled.");
					break;

				case ConvertMediaUIState.ConversionFailed:
					_viewModel.DeleteOutputFile();
					_labelStatus.Text = LocalizationManager.GetString(
						"DialogBoxes.ConvertMediaDlg.ConversionFailedMsg",
						"Conversion Failed.");
					break;

				case ConvertMediaUIState.FinishedConverting:
					if ((_viewModel.ConversionState & ConvertMediaUIState.PossibleError) > 0)
					{
						_labelStatus.ForeColor = Color.Orange;
						_labelStatus.Text = LocalizationManager.GetString(
							"DialogBoxes.ConvertMediaDlg.ConversionFinishedWithPossibleErrorsMsg",
							"Conversion Finished, but with possible errors.");
					}
					else
					{
						_labelStatus.Text = LocalizationManager.GetString(
							"DialogBoxes.ConvertMediaDlg.ConversionSucceededMsg",
							"Conversion Finished Successfully.");
					}
					break;
			}

			Logger.WriteEvent("Media Conversion Finished. {0}", _labelStatus.Text);

			_labelStatus.TextAlign = ContentAlignment.MiddleCenter;
			_textBoxOutput.AppendText(_labelStatus.Text);
			UseWaitCursor = false;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBeginConversionClick(object sender, EventArgs e)
		{
			//if (!CheckOutputFileAlreadyExists())
			//    return;

			UseWaitCursor = true;
			_textBoxOutput.Clear();
			_progressBar.Maximum = (int)_viewModel.MediaInfo.Duration.TotalSeconds;
			_labelStatus.Visible = false;
			_labelStatus.ForeColor = SystemColors.ControlText;
			_labelStatus.TextAlign = ContentAlignment.MiddleLeft;

			_conversionInProgressStatusFormat = LocalizationManager.GetString(
				"DialogBoxes.ConvertMediaDlg.ConversionProgressStatusMsgFormat",
				"{0} of {1} Converted...");

			_viewModel.BeginConversion(HandleUpdateDisplayDuringConversion);
			UpdateDisplay();
		}

		///// ------------------------------------------------------------------------------------
		//private bool CheckOutputFileAlreadyExists()
		//{
		//    var outputFile = _viewModel.GetNewOutputFileName(false);

		//    if (!File.Exists(outputFile))
		//        return true;

		//    var msg = LocalizationManager.GetString(
		//        "DialogBoxes.ConvertMediaDlg.OutputFileAlreadyExistsMsg",
		//        "This conversion will create the output file '{0}', " +
		//        "but that file already exists. If you continue, the file will be overwritten." +
		//        "\r\n\r\nDo you want to continue?");

		//    msg = string.Format(msg, outputFile);

		//    var result = MessageBox.Show(this, msg, Application.ProductName,
		//        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

		//    if (result == DialogResult.No)
		//        return false;

		//    FileSystemUtils.WaitForFileRelease(outputFile);
		//    File.Delete(outputFile);
		//    return true;
		//}

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

			_progressBar.Value = Math.Min(_progressBar.Maximum, (int)convertedSoFar.TotalSeconds);
			_labelStatus.Text = String.Format(_conversionInProgressStatusFormat,
				convertedSoFar.ToString(@"hh\:mm\:ss"),
				_viewModel.MediaInfo.Duration.ToString(@"hh\:mm\:ss"));
		}
	}
}
