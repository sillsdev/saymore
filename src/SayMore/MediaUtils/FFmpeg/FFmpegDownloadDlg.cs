using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using L10NSharp;
using SayMore.Properties;
using SayMore.UI.LowLevelControls;
using SilTools;

namespace SayMore.Media.FFmpeg
{
	public partial class FFmpegDownloadDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		private enum ProgressState
		{
			NotStarted,
			DownloadStarted,
			DownloadCanceled,
			Installing
		}

		private FFmpegDownloadHelper _downloadHelper;
		private ProgressState _state = ProgressState.NotStarted;

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

			_buttonCancel.MouseMove += delegate
			{
				// Not sure why both of these are necessary, but it seems to be the case.
				_buttonCancel.UseWaitCursor = false;
				_buttonCancel.Cursor = Cursors.Default;
			};

			_labelOverview.Font = FontHelper.MakeFont(Program.DialogFont, FontStyle.Bold);
			_labelFFmpegFolderLocation.Font = _labelOverview.Font;
			_labelStatus.Font = _labelOverview.Font;
			_linkManualDownload.Font = Program.DialogFont;
			_labelDownloadAndInstall.Font = Program.DialogFont;
			_labelFinishedCopying.Font = Program.DialogFont;
			_labelInstallFromZipFile1.Font = Program.DialogFont;
			_labelInstallFromZipFile2.Font = Program.DialogFont;

			_labelFFmpegFolderLocation.Text = FFmpegDownloadHelper.FFmpegForSayMoreFolder;
			_linkManualDownload.Text = FFmpegDownloadHelper.FFmpegForSayMoreZipFileName;
			_labelInstallFromZipFile2.Text = string.Format(_labelInstallFromZipFile2.Text,
				_buttonInstallFromZipFile.Text.Replace("&", string.Empty).Replace("...", string.Empty));
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeDownloadLinkLabel()
		{
			_linkManualDownload.Links.Clear();
			_linkManualDownload.LinkArea = new LinkArea(0, _linkManualDownload.Text.Length);
			_linkManualDownload.LinkClicked += HandleManualDownloadLinkClicked;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.FFmpegDownloadDlg.InitializeForm(this);
			base.OnLoad(e);
			_buttonOpenFolderLocation.Enabled = Directory.Exists(FFmpegDownloadHelper.FFmpegForSayMoreParentFolder);
			_timerCheckForFFmpeg.Enabled = true;
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

			if (_state == ProgressState.DownloadStarted)
			{
				_progressControl.Visible = true;
				_buttonAbort.Visible = true;
				_buttonCancel.Enabled = false;
				_buttonDownloadAndInstall.Enabled = false;
				_buttonInstallFromZipFile.Enabled = false;
			}
			else if (_state != ProgressState.NotStarted)
			{
				_progressControl.Visible = false;
				_buttonAbort.Visible = false;
				_buttonCancel.Enabled = true;
				_labelStatus.Visible = true;
				_buttonDownloadAndInstall.Enabled = _buttonInstallFromZipFile.Enabled = !FFmpegDownloadHelper.DoesFFmpegForSayMoreExist;

				switch (_state)
				{
					case ProgressState.DownloadCanceled:
						_labelStatus.ForeColor = Color.Red;
						_labelStatus.Text = LocalizationManager.GetString(
							"DialogBoxes.FFmpegDownloadDlg.DownloadCancelledMsg",
							"Downloading and Installing Cancelled.");
						break;

					case ProgressState.Installing:
						_labelStatus.ForeColor = ForeColor;
						_labelStatus.Text = LocalizationManager.GetString(
							"DialogBoxes.FFmpegDownloadDlg.InstallingDownloadedFileMsg",
							"Installing...");
						break;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDownloadAndInstallClicked(object sender, EventArgs e)
		{
			if (_state != ProgressState.NotStarted && _state != ProgressState.DownloadCanceled)
				return;

			UseWaitCursor = true;
			_downloadHelper = new FFmpegDownloadHelper();
			_downloadHelper.OnFinished += HandleDownloadProgressFinished;
			_progressControl.Initialize(_downloadHelper);

			_state = ProgressState.DownloadStarted;
			_timerCheckForFFmpeg.Enabled = false;
			UpdateDisplay();
			_downloadHelper.Start();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDownloadProgressFinished(object sender, ProgressFinishedArgs e)
		{
			_buttonAbort.Visible = false;

			if (e.ProgressCanceled)
				_state = ProgressState.DownloadCanceled;
			else if (InstallDownloadedFile(_downloadHelper.DownloadedZipFilePath))
			{
				MessageBox.Show(this, LocalizationManager.GetString(
					"DialogBoxes.FFmpegDownloadDlg.DownloadCompleteMsg",
					"Downloading and Installing Completed Successfully."), Text);
				Close();
				return;
			}

			_timerCheckForFFmpeg.Enabled = true;
			_downloadHelper = null;
			UpdateDisplay();
			UseWaitCursor = false;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleAbortButtonClick(object sender, EventArgs e)
		{
			_downloadHelper.Cancel();
		}

		/// ------------------------------------------------------------------------------------
		private bool InstallDownloadedFile(string downloadedZipFile)
		{
			var errorMsg = string.Format(LocalizationManager.GetString(
				"DialogBoxes.FFmpegDownloadDlg.InstallingDownloadedFileErrorMsg",
				"The file '{0}'\r\n\r\neither does not contain ffmpeg or is not a valid zip file."),
				downloadedZipFile);

			if (!FFmpegDownloadHelper.GetIsValidFFmpegForSayMoreFile(downloadedZipFile, errorMsg))
			{
				_state = ProgressState.DownloadCanceled;
				return false;
			}

			_progressControl.SetStatusMessage(LocalizationManager.GetString(
				"DialogBoxes.FFmpegDownloadDlg.InstallingDownloadedFileMsg", "Installing..."));

			Application.DoEvents();

			errorMsg = string.Format(LocalizationManager.GetString(
				"DialogBoxes.FFmpegDownloadDlg.InstallingDownloadedFileErrorMsg",
				"There was an error installing the downloaded file:\r\n\r\n{0}"),
				downloadedZipFile);

			return FFmpegDownloadHelper.ExtractDownloadedZipFile(downloadedZipFile, errorMsg);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleManualDownloadLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (_state != ProgressState.NotStarted && _state != ProgressState.DownloadCanceled)
				return;

			var prs = new Process();
			prs.StartInfo.FileName = FFmpegDownloadHelper.GetFFmpegForSayMoreUrl(false);
			prs.Start();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleOpenFolderLocationClick(object sender, EventArgs e)
		{
			Process.Start(FFmpegDownloadHelper.FFmpegForSayMoreParentFolder);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleInstallFromZipFileClick(object sender, EventArgs e)
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
				dlg.FileName = FFmpegDownloadHelper.FFmpegForSayMoreZipFileName;
				dlg.Filter = LocalizationManager.GetString(
					"DialogBoxes.FFmpegDownloadDlg.SelectDownloadedFFmpegFileDlg.FileTypesString",
					"Zip Archive (*.zip)|*.zip|All Files (*.*)|*.*");

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					_state = ProgressState.Installing;
					UpdateDisplay();
					Application.DoEvents();
					if (InstallDownloadedFile(dlg.FileName))
					{
						MessageBox.Show(this, LocalizationManager.GetString(
							"DialogBoxes.FFmpegDownloadDlg.InstallCompletedingMsg",
							"Installed Successfully."), Text);
						Close();
					}
					_state = ProgressState.NotStarted;
				}
			}

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCheckForFFmpegTimerTick(object sender, EventArgs e)
		{
			if (FFmpegDownloadHelper.DoesFFmpegForSayMoreExist)
				Close();
			_buttonOpenFolderLocation.Enabled = Directory.Exists(FFmpegDownloadHelper.FFmpegForSayMoreParentFolder);
		}
	}
}
