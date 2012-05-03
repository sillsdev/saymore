using System;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using Palaso.Progress;
using Palaso.Progress.LogBox;
using SayMore.Properties;
using SilTools;

namespace SayMore.UI.FFmpegForSayMore
{
	public partial class FFmpegForSayMoreDlg : Form
	{
		private LogBox _logBox;

		/// ------------------------------------------------------------------------------------
		public FFmpegForSayMoreDlg()
		{
			InitializeComponent();

			if (Settings.Default.FFmpegForSayMoreDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.FFmpegForSayMoreDlg = FormSettings.Create(this);
			}

			_buttonClose.Click += delegate { Close(); };

			_labelOverview.Font = Program.DialogFont;
			_linkDownload.Font = Program.DialogFont;

			var underlinedPortion = LocalizationManager.GetString(
				"DialogBoxes.FFmpegForSayMoreDlg._linkDownload_LinkPortion",
				"Click here to download and unpack",
				"This is the portion of the text that is underlined, indicating the link to the download.");

			_linkDownload.Links.Clear();

			int i = _linkDownload.Text.IndexOf(underlinedPortion);
			if (i >= 0)
				_linkDownload.Links.Add(i, underlinedPortion.Length, FFmpegForSayMoreUtils.GetFFmpegForSayMoreUrl(true));

			_logBox = new LogBox();
			_logBox.TabStop = false;
			_logBox.ShowMenu = false;

			if (Program.DialogFont.FontFamily.IsStyleAvailable(FontStyle.Bold))
				_logBox.Font = new Font(Program.DialogFont, FontStyle.Bold);

			_logBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			_logBox.Margin = new Padding(0);
			_logBox.ReportErrorLinkClicked += delegate { Close(); };
			_tableLayoutPanel.Controls.Add(_logBox, 0, 2);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.FFmpegForSayMoreDlg.InitializeForm(this);
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDownloadLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Exception error = null;

			try
			{
				_buttonClose.Enabled = false;
				_logBox.Clear();
				WaitCursor.Show();
				Application.DoEvents();
				var msg = LocalizationManager.GetString("DialogBoxes.FFmpegForSayMoreDlg.DownloadingMsg",
					"- Downloading zip file for FFmpeg for SayMore...");

				_logBox.WriteMessage(msg);
				Application.DoEvents();
				var tempPathToZipFile = FFmpegForSayMoreUtils.DownloadZipFile();

				msg = LocalizationManager.GetString("DialogBoxes.FFmpegForSayMoreDlg.ExtractingDownloadFileMsg",
					"- Unpacking downloaded zip file...");

				_logBox.WriteMessage(msg);
				Application.DoEvents();
				if (FFmpegForSayMoreUtils.ExtractDownloadedZipFile(tempPathToZipFile))
				{
					msg = LocalizationManager.GetString("DialogBoxes.FFmpegForSayMoreDlg.ExtractingCompleteMsg",
						"Downloading and unpacking complete. Your video file is now ready to be converted.");

					_logBox.WriteMessageWithColor("Green", Environment.NewLine + msg);
					return;
				}
			}
			catch (Exception err)
			{
				error = err;
			}
			finally
			{
				WaitCursor.Hide();
				_buttonClose.Enabled = true;
			}

			LogError(error);

		}

		/// ------------------------------------------------------------------------------------
		private void LogError(Exception error)
		{
			_logBox.Clear();

			var msg = LocalizationManager.GetString(
				"DialogBoxes.FFmpegForSayMoreDlg.DownloadingErrorMsg",
				"There was a problem trying to download or unpack the zip. Verify that you " +
				"are connected to the internet.\r\n\r\nIf the problem persists, try downloading the " +
				"file manually by using the following link in your browser.");

			_logBox.WriteError(msg + Environment.NewLine);

			_logBox.WriteMessageWithColor("Blue",
				FFmpegForSayMoreUtils.GetFFmpegForSayMoreUrl(false) + Environment.NewLine);

			if (error != null)
				_logBox.WriteException(error);

			_logBox.ScrollToTop();
		}
	}
}
