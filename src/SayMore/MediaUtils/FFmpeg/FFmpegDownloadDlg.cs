using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using L10NSharp;
using Palaso.IO;
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
			_labelStatus.Font = _labelOverview.Font;
			_linkManualDownload.Font = Program.DialogFont;
			_labelDownloadAndInstall.Font = Program.DialogFont;
			_labelCopyFromAnotherComputer.Font = Program.DialogFont;
			_labelInstallFromZipFile.Font = Program.DialogFont;

			_labelCopyFromAnotherComputer.Text = string.Format(_labelCopyFromAnotherComputer.Text, FFmpegDownloadHelper.FFmpegForSayMoreFolder);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeDownloadLinkLabel()
		{
			_linkManualDownload.Links.Clear();
			_linkManualDownload.LinkArea = new LinkArea(0, _linkManualDownload.Text.Length);
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

			if (_state == ProgressState.DownloadStarted)
			{
				_progressControl.Visible = true;
				_buttonAbort.Visible = true;
				_buttonCancel.Enabled = false;
				_linkDownloadAndInstall.Enabled = false;
				_linkInstallFromZipFile.Enabled = false;
				_linkCopyFromFolder.Enabled = false;
				_linkManualDownload.Enabled = false;
			}
			else if (_state != ProgressState.NotStarted)
			{
				_progressControl.Visible = false;
				_buttonAbort.Visible = false;
				_buttonCancel.Enabled = true;
				_labelStatus.Visible = true;
				_linkDownloadAndInstall.Enabled = true;
				_linkInstallFromZipFile.Enabled = true;
				_linkCopyFromFolder.Enabled = true;
				_linkManualDownload.Enabled = true;

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
		private void HandleDownloadAndInstallClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (_state != ProgressState.NotStarted && _state != ProgressState.DownloadCanceled)
				return;

			UseWaitCursor = true;
			_downloadHelper = new FFmpegDownloadHelper();
			_downloadHelper.OnFinished += HandleDownloadProgressFinished;
			_progressControl.Initialize(_downloadHelper);

			_state = ProgressState.DownloadStarted;
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
		private void HandleCopyFromFolderClick(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var description = LocalizationManager.GetString(
				"DialogBoxes.FFmpegDownloadDlg.SelectExistingFFmpegFolderDlg.Description",
				"Select the FFmpeg for SayMore folder from another computer",
				"This is the descriptive text for the 'Open Folder' dialog box when specifying the location of an \"FFmpeg for SayMore\" folder on (or copied from) another computer.");

			using (var dlg = new FolderBrowserDialog())
			{
				dlg.Description = description;
				dlg.ShowNewFolderButton = false;

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					if (Path.GetFileName(dlg.SelectedPath) != FFmpegDownloadHelper.kFFmpegForSayMoreFolderName ||
						!File.Exists(Path.Combine(dlg.SelectedPath, FFmpegDownloadHelper.kFFmpegForSayMoreExeFilename)))
					{
						MessageBox.Show(this, LocalizationManager.GetString(
							"DialogBoxes.FFmpegDownloadDlg.NotAnFfmpegFolderMsg",
							"The selected folder is not a valid \"FFmpeg for SayMore\" folder."), Text);
						return;
					}
					if (DirectoryUtilities.AreDirectoriesEquivalent(dlg.SelectedPath, FFmpegDownloadHelper.FFmpegForSayMoreParentFolder))
					{
						// Cool. The user must have copied it to the correct location already. Just close this dlg and move on.
						Close();
					}

					_state = ProgressState.Installing;
					UpdateDisplay();
					Application.DoEvents();
					if (DirectoryUtilities.CopyDirectory(dlg.SelectedPath, FFmpegDownloadHelper.FFmpegForSayMoreParentFolder))
					{
						MessageBox.Show(this, LocalizationManager.GetString(
							"DialogBoxes.FFmpegDownloadDlg.CopyCompletedingMsg",
							"Files Copied Successfully."), Text);
						Close();
					}
					else
						_state = ProgressState.NotStarted;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleInstallFromZipFileClick(object sender, LinkLabelLinkClickedEventArgs e)
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
	}
}
