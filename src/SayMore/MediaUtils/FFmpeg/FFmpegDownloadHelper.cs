using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Localization;
using Palaso.Reporting;
using SayMore.Utilities.LowLevelControls;

namespace SayMore.Media.FFmpeg
{
	public class FFmpegDownloadHelper : IProgressViewModel
	{
		public event EventHandler<ProgressFinishedArgs> OnFinished;
		public event EventHandler OnUpdateProgress;
		public event EventHandler OnUpdateStatus;

		public string DownloadedZipFilePath { get; private set; }

		private BackgroundWorker _worker;
		private bool _canceled;
		private readonly string _statusFormat;
		private string _statusMessage;
		private const int kApproxDownloadSize = 20480000;

		/// ------------------------------------------------------------------------------------
		public FFmpegDownloadHelper()
		{
			_statusFormat = LocalizationManager.GetString(
				"DialogBoxes.ConvertMediaDlg.DownloadingProgressMsgFormat",
				"Downloaded {0:F2} MB of approximately {1:F2} MB",
				"The 'F2' in the parameters specifies displaying a number to 2 decimal places.");
		}

		#region IProgressViewModel implementation
		/// ------------------------------------------------------------------------------------
		public int MaximumProgressValue
		{
			get { return kApproxDownloadSize; }
		}

		/// ------------------------------------------------------------------------------------
		public int CurrentProgressValue { get; private set; }

		/// ------------------------------------------------------------------------------------
		public string StatusString
		{
			get
			{
				return (_statusMessage ?? string.Format(_statusFormat,
					(double)CurrentProgressValue / (1024 * 1024),
					(double)kApproxDownloadSize / (1024 * 1024)));
			}
			set { _statusMessage = value; }
		}

		/// ------------------------------------------------------------------------------------
		public void Cancel()
		{
			_canceled = true;
			_worker.CancelAsync();
			UsageReporter.SendNavigationNotice("FFmpeg download cancelled.");
		}

		/// ------------------------------------------------------------------------------------
		public void Start()
		{
			UsageReporter.SendNavigationNotice("FFmpeg download started.");

			if (OnUpdateStatus != null)
				OnUpdateStatus(this, EventArgs.Empty);

			_worker = new BackgroundWorker();
			_worker.WorkerSupportsCancellation = true;
			_worker.DoWork += Download;
			_worker.RunWorkerAsync();
			while (_worker.IsBusy) { Application.DoEvents(); }

			if (_canceled)
			{
				try { File.Delete(DownloadedZipFilePath); }
				catch { }
			}

			if (OnFinished != null)
				OnFinished(this, new ProgressFinishedArgs(_canceled, null));
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public static string GetFFmpegForSayMoreUrl(bool forAutoDownload)
		{
			return "https://www.dropbox.com/s/vs77d9rrfm2pvcn/FFmpegForSayMore.zip" +
				(forAutoDownload ? "?dl=1" : string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private void Download(object sender, DoWorkEventArgs e)
		{
			Stream downloadStream = null;
			DownloadedZipFilePath = Path.Combine(Path.GetTempPath(), "FFmpegForSayMore.zip");

			try
			{
				var req = WebRequest.Create(GetFFmpegForSayMoreUrl(true));
				var response = req.GetResponse();
				downloadStream = response.GetResponseStream();

				using (var fileStream = new FileStream(DownloadedZipFilePath, FileMode.Create))
				using (var binaryWriter = new BinaryWriter(fileStream))
				{
					int bytesRead;
					var buffer = new byte[1024 * 500];
					while ((bytesRead = downloadStream.Read(buffer, 0, buffer.Length)) > 0)
					{
						if (_worker.CancellationPending)
							break;

						binaryWriter.Write(buffer, 0, bytesRead);
						CurrentProgressValue += bytesRead;

						if (OnUpdateProgress != null)
							OnUpdateProgress(this, EventArgs.Empty);

						if (OnUpdateStatus != null)
							OnUpdateStatus(this, EventArgs.Empty);
					}

					binaryWriter.Close();
					fileStream.Close();
				}
			}
			finally
			{
				if (downloadStream != null)
					downloadStream.Close();
			}
		}
	}
}
