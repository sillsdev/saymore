using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Ionic.Zip;
using Localization;
using Palaso.Reporting;
using SayMore.UI.LowLevelControls;

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
		private string _downloadErrorMsg;
		private Exception _downloadError;

		/// ------------------------------------------------------------------------------------
		public FFmpegDownloadHelper()
		{
			_statusFormat = LocalizationManager.GetString(
				"DialogBoxes.ConvertMediaDlg.DownloadingProgressMsgFormat",
				"Downloaded {0:F2} MB of approximately {1:F2} MB",
				"The 'F2' in the parameters specifies displaying a number to 2 decimal places.");
		}

		#region Static methods
		/// ------------------------------------------------------------------------------------
		public static string GetFFmpegForSayMoreUrl(bool forAutoDownload)
		{
			return "https://www.dropbox.com/s/vs77d9rrfm2pvcn/FFmpegForSayMore.zip" +
				(forAutoDownload ? "?dl=1" : String.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public static string GetFFmpegForSayMoreFolder()
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			folder = Path.Combine(folder, "SIL");
			return Path.Combine(folder, "SayMore");
		}

		/// ------------------------------------------------------------------------------------
		public static string GetFullPathToFFmpegForSayMoreExe()
		{
			var path = Path.Combine(GetFFmpegForSayMoreFolder(), "FFmpegForSayMore");
			return Path.Combine(path, "ffmpeg.exe");
		}

		/// ------------------------------------------------------------------------------------
		public static bool DoesFFmpegForSayMoreExist
		{
			get { return File.Exists(GetFullPathToFFmpegForSayMoreExe()); }
		}

		#endregion

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
				return (_statusMessage ?? String.Format(_statusFormat,
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

			if (_downloadError != null)
			{
				ErrorReport.NotifyUserOfProblem(_downloadError,
					_downloadErrorMsg, GetFFmpegForSayMoreUrl(false));

				_canceled = true;
			}

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
			catch (Exception error)
			{
				ProcessDownloadError(error);
			}
			finally
			{
				if (downloadStream != null)
					downloadStream.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ProcessDownloadError(Exception error)
		{
			_downloadError = error;

			if (error is WebException)
			{
				_downloadErrorMsg = LocalizationManager.GetString(
					"DialogBoxes.ConvertMediaDlg.DownloadingFailedDueToWebExceptionMsg",
					"There was an error getting a web response trying to download FFmpeg. " +
					"Try pasting this Url in your browser to see if the file's location is available.\r\n\r\n{0}");
			}
			else
			{
				_downloadErrorMsg = LocalizationManager.GetString(
					"DialogBoxes.ConvertMediaDlg.DownloadingGeneralFailureMsg",
					"There was an error attempting to download FFmpeg. " +
					"Try pasting this Url in your browser to see if the file's location is available.\r\n\r\n{0}");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsValidFFmpegForSayMoreFile(string pathToZipFile, string msgToDisplayIfError)
		{
			try
			{
				using (var zip = new ZipFile(pathToZipFile))
				{
					if (zip.EntryFileNames.Contains("FFmpegForSayMore/ffmpeg.exe"))
						return true;

					ErrorReport.NotifyUserOfProblem(msgToDisplayIfError);
				}
			}
			catch (Exception e)
			{
				ErrorReport.NotifyUserOfProblem(e, msgToDisplayIfError);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public static bool ExtractDownloadedZipFile(string pathToZipFile, string msgToDisplayIfError)
		{
			try
			{
				var tgtFolder = GetFFmpegForSayMoreFolder();

				using (var zip = new ZipFile(pathToZipFile))
					zip.ExtractAll(tgtFolder, ExtractExistingFileAction.OverwriteSilently);

				return DoesFFmpegForSayMoreExist;
			}
			catch (Exception e)
			{
				ErrorReport.NotifyUserOfProblem(e, msgToDisplayIfError);
				return false;
			}
			finally
			{
				try
				{
					if (File.Exists(pathToZipFile))
						File.Delete(pathToZipFile);
				}
				catch { }
			}
		}
	}
}
