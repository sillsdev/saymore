using System;
using System.ComponentModel;
using System.Drawing;
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
	public partial class ExportMediaDlg : Form
	{
		private readonly ConvertMediaDlgViewModel _viewModel;
		private string _conversionInProgressStatusFormat;

		public static DialogResult Show(string inputFile, string initialConversionName, string outputFile)
		{
			var viewModel = new ConvertMediaDlgViewModel(inputFile, initialConversionName, outputFile);
			using (var dlg = new ExportMediaDlg(viewModel))
				return dlg.ShowDialog();
		}

		/// ------------------------------------------------------------------------------------
		public ExportMediaDlg()
		{
			InitializeComponent();
			DialogResult = DialogResult.Cancel;
		}

		/// ------------------------------------------------------------------------------------
		public ExportMediaDlg(ConvertMediaDlgViewModel viewModel) : this()
		{
			Logger.WriteEvent("ExportMediaDlg constructor. file = {0}", viewModel.InputFile);
			_viewModel = viewModel;

			if (Settings.Default.ExportMediaDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.ExportMediaDlg = FormSettings.Create(this);
			}

			InitializeFonts();

			if (_viewModel.MediaInfo == null)
			{
				_labelStatus.Text = LocalizationManager.GetString("DialogBoxes.ConvertMediaDlg.InvalidMediaFile",
					"File does not appear to be a valid media file.");
				_labelStatus.ForeColor = Color.Red;
			}
			else
			{
				_labelOutputFileValue.Text = _viewModel.GetNewOutputFileName(true);
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

			Shown += delegate
			{
				BeginConversion();
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
			Settings.Default.ExportMediaDlg.InitializeForm(this);
			base.OnLoad(e);
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			Program.ResumeBackgroundProcesses(true);

			if ((_viewModel.ConversionState & ConvertMediaUIState.FinishedConverting) > 0)
				DialogResult = DialogResult.OK;

			base.OnClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeFonts()
		{
			_textBoxOutput.Font = Program.DialogFont;
			_labelOutputFile.Font = Program.DialogFont;
			_labelOutputFileValue.Font = FontHelper.MakeFont(Program.DialogFont, FontStyle.Bold);
			_labelDownloadNeeded.Font = Program.DialogFont;
			_labelStatus.Font = _labelOutputFileValue.Font;
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
				_viewModel.SelectedConversion);

			_labelOutputFile.Visible = true;

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

			MinimumSize = new Size(MinimumSize.Width, MinimumSize.Height + 50);

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
		private void BeginConversion()
		{
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
			_labelStatus.Text = string.Format(_conversionInProgressStatusFormat,
				convertedSoFar.ToString(@"hh\:mm\:ss"),
				_viewModel.MediaInfo.Duration.ToString(@"hh\:mm\:ss"));
		}
	}
}
