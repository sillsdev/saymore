using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using SayMore.Properties;
using SayMore.Utilities.LowLevelControls;
using SilTools;

namespace SayMore.Media.FFmpeg
{
	public partial class FFmpegDownloadDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		private enum ProgressSate
		{
			NotStarted,
			DownloadStarted,
			DownloadedAndInstallSucceeded,
			DownloadCanceled,
			Installing,
			Installed
		}

		private FFmpegDownloadHelper _downloadHelper;
		private ProgressSate _state = ProgressSate.NotStarted;

		/// ------------------------------------------------------------------------------------
		public FFmpegDownloadDlg()
		{
			InitializeComponent();
			InitializeDownloadLinkLabel();

			if (Settings.Default.FFmpegDownloadDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.FFmpegDownloadDlg = FormSettings.Create(this);
			}

			_buttonClose.Click += delegate { Close(); };

			_buttonCancel.Click += delegate
			{
				if (_downloadHelper == null)
					Close();
				else
					_downloadHelper.Cancel();
			};

			_buttonCancel.MouseMove += delegate
			{
				// Not sure why both of these are necessary, but it seems to be the case.
				_buttonCancel.UseWaitCursor = false;
				_buttonCancel.Cursor = Cursors.Default;
			};

			_labelOverview.Font = FontHelper.MakeFont(Program.DialogFont, FontStyle.Bold);
			_labelOr.Font = _labelOverview.Font;
			_labelStatus.Font = _labelOverview.Font;
			_linkAutoDownload.Font = Program.DialogFont;
			_linkManualDownload.Font = Program.DialogFont;
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeDownloadLinkLabel()
		{
			var underlinedPortionOfAutoLink = LocalizationManager.GetString(
				"DialogBoxes.FFmpegDownloadDlg._linkAutoDownload_LinkPortion",
				"Click here and SayMore will automatically download",
				"This is the portion of the text that is underlined, indicating the link to the download.");

			var underlinedPortionOfManualLink = LocalizationManager.GetString(
				"DialogBoxes.FFmpegDownloadDlg._linkAutoDownload_LinkPortion",
				"Click here to download",
				"This is the portion of the text that is underlined, indicating the link to the download.");

			_linkAutoDownload.Links.Clear();
			_linkManualDownload.Links.Clear();

			int i = _linkAutoDownload.Text.IndexOf(underlinedPortionOfAutoLink);
			if (i >= 0)
				_linkAutoDownload.LinkArea = new LinkArea(i, underlinedPortionOfAutoLink.Length);

			i = _linkManualDownload.Text.IndexOf(underlinedPortionOfManualLink);
			if (i >= 0)
				_linkManualDownload.LinkArea = new LinkArea(i, underlinedPortionOfManualLink.Length);

			_linkAutoDownload.LinkClicked += HanleAudoAutoDownloadLinkClicked;
			_linkManualDownload.LinkClicked += HandleManualDownloadLinkClicked;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.FFmpegDownloadDlg.InitializeForm(this);
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_labelStatus.Visible = false;

			if (_state == ProgressSate.DownloadStarted)
				_progressControl.Visible = true;
			else if (_state != ProgressSate.NotStarted)
			{
				_progressControl.Visible = false;

				switch (_state)
				{
					case ProgressSate.DownloadCanceled:
						_labelStatus.ForeColor = Color.Red;
						_labelStatus.Text = LocalizationManager.GetString(
							"DialogBoxes.FFmpegDownloadDlg.DownloadCancelledMsg",
							"Downloading and Installing Cancelled.");

						break;

					case ProgressSate.DownloadedAndInstallSucceeded:
						_labelStatus.ForeColor = Color.Green;
						_labelStatus.Text = LocalizationManager.GetString(
							"DialogBoxes.FFmpegDownloadDlg.DownloadCompleteMsg",
							"Downloading and Installing Completed Successfully.");

						break;

					case ProgressSate.Installing:
						_labelStatus.ForeColor = ForeColor;
						_labelStatus.Text = LocalizationManager.GetString(
							"DialogBoxes.FFmpegDownloadDlg.InstallingDownloadedFileMsg",
							"Installing...");

						break;

					case ProgressSate.Installed:
						_labelStatus.ForeColor = Color.Green;
						_labelStatus.Text = LocalizationManager.GetString(
							"DialogBoxes.FFmpegDownloadDlg.InstallCompletedingMsg",
							"Installed Successfully.");

						break;
				}

				_labelStatus.Visible = true;
			}

			_buttonInstall.Enabled = (_state != ProgressSate.DownloadStarted && !FFmpegDownloadHelper.DoesFFmpegForSayMoreExist);
			_buttonCancel.Visible = (_state == ProgressSate.DownloadStarted);
			_buttonClose.Visible = !_buttonCancel.Visible;

			if (!_buttonClose.Visible)
			{
				_tableLayoutPanel.SetColumn(_buttonClose, 0);
				_tableLayoutPanel.SetColumn(_buttonCancel, 2);
			}
			else
			{
				_tableLayoutPanel.SetColumn(_buttonCancel, 1);
				_tableLayoutPanel.SetColumn(_buttonClose, 2);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HanleAudoAutoDownloadLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (_state != ProgressSate.NotStarted && _state != ProgressSate.DownloadCanceled)
				return;

			UseWaitCursor = true;
			_downloadHelper = new FFmpegDownloadHelper();
			_downloadHelper.OnFinished += HandleDownloadProgressFinished;
			_progressControl.Initialize(_downloadHelper);

			_state = ProgressSate.DownloadStarted;
			UpdateDisplay();
			_downloadHelper.Start();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDownloadProgressFinished(object sender, ProgressFinishedArgs e)
		{
			if (e.ProgressCanceled)
				_state = ProgressSate.DownloadCanceled;
			else if (InstallDownloadedFile(_downloadHelper.DownloadedZipFilePath))
				_state = ProgressSate.DownloadedAndInstallSucceeded;

			_downloadHelper = null;
			UpdateDisplay();
			UseWaitCursor = false;
		}

		/// ------------------------------------------------------------------------------------
		private bool InstallDownloadedFile(string downloadedZipFile)
		{
			var errorMsg = string.Format(LocalizationManager.GetString(
				"DialogBoxes.FFmpegDownloadDlg.InstallingDownloadedFileErrorMsg",
				"The file '{0}'\r\n\r\neither does not contain ffmpeg or is not a valid zip file."),
				downloadedZipFile);

			if (!_downloadHelper.GetIsValidFFmpegForSayMoreFile(downloadedZipFile, errorMsg))
			{
				_state = ProgressSate.DownloadCanceled;
				return false;
			}

			_progressControl.SetStatusMessage(LocalizationManager.GetString(
				"DialogBoxes.FFmpegDownloadDlg.InstallingDownloadedFileMsg", "Installing..."));

			Application.DoEvents();

			errorMsg = string.Format(LocalizationManager.GetString(
				"DialogBoxes.FFmpegDownloadDlg.InstallingDownloadedFileErrorMsg",
				"There was an error installing the downloaded file:\r\n\r\n{0}"),
				downloadedZipFile);

			return _downloadHelper.ExtractDownloadedZipFile(downloadedZipFile, errorMsg);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleManualDownloadLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (_state != ProgressSate.NotStarted && _state != ProgressSate.DownloadCanceled)
				return;

			var prs = new Process();
			prs.StartInfo.FileName = FFmpegDownloadHelper.GetFFmpegForSayMoreUrl(false);
			prs.Start();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleInstallClick(object sender, EventArgs e)
		{
			var caption = LocalizationManager.GetString(
				"DialogBoxes.FFmpegDownloadDlg.SelectDownloadedFFmpegFileDlg.Caption",
				"Select the Downloaded FFmpeg File",
				"This is the caption for the 'Open File' dialog box when specifying the manually downloaded FFmpeg zip file.");

			using (var dlg = new OpenFileDialog())
			{
				dlg.Title = caption;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;
				dlg.Filter = LocalizationManager.GetString(
					"DialogBoxes.FFmpegDownloadDlg.SelectDownloadedFFmpegFileDlg.FileTypesString",
					"Zip Archive (*.zip)|*.zip|All Files (*.*)|*.*");

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					_state = ProgressSate.Installing;
					UpdateDisplay();
					Application.DoEvents();
					_state = (InstallDownloadedFile(dlg.FileName) ?
						ProgressSate.Installed : ProgressSate.NotStarted);
				}
			}

			UpdateDisplay();
		}
	}
}
