using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Ionic.Zip;
using Localization;
using Palaso.CommandLineProcessing;

namespace SayMore.Media.FFmpeg
{
	public class FFmpegHelper
	{
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
		public static string GetFFmpegForSayMoreUrl(bool forAutoDownload)
		{
			return "https://www.dropbox.com/s/vs77d9rrfm2pvcn/FFmpegForSayMore.zip" +
				(forAutoDownload ? "?dl=1" : string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public static bool DoesFFmpegForSayMoreExist
		{
			get { return File.Exists(GetFullPathToFFmpegForSayMoreExe()); }
		}

		/// ------------------------------------------------------------------------------------
		public static bool OfferToDownloadFFmpegForSayMoreIfNecessary()
		{
			if (!DoesFFmpegForSayMoreExist)
			{
				using (var dlg = new FFmpegDownloadDlg())
					dlg.ShowDialog();
			}

			return DoesFFmpegForSayMoreExist;
		}

		/// ------------------------------------------------------------------------------------
		public static string DownloadZipFile()
		{
			Stream downloadStream = null;
			var tempPathToZipFile = Path.Combine(Path.GetTempPath(), "FFmpegForSayMore.zip");

			try
			{
				var req = WebRequest.Create(GetFFmpegForSayMoreUrl(true));
				var response = req.GetResponse();
				downloadStream = response.GetResponseStream();

				using (var fileStream = new FileStream(tempPathToZipFile, FileMode.Create))
				using (var binaryWriter = new BinaryWriter(fileStream))
				{
					int bytesRead;
					var buffer = new byte[1024 * 100];
					while ((bytesRead = downloadStream.Read(buffer, 0, buffer.Length)) > 0)
						binaryWriter.Write(buffer, 0, bytesRead);

					binaryWriter.Close();
					fileStream.Close();
				}
			}
			finally
			{
				if (downloadStream != null)
					downloadStream.Close();
			}

			return tempPathToZipFile;
		}

		/// ------------------------------------------------------------------------------------
		public static bool ExtractDownloadedZipFile(string tempPathToZipFile)
		{
			try
			{
				var tgtFolder = GetFFmpegForSayMoreFolder();

				using (var zip = new ZipFile(tempPathToZipFile))
					zip.ExtractAll(tgtFolder, ExtractExistingFileAction.OverwriteSilently);

				return DoesFFmpegForSayMoreExist;
			}
			finally
			{
				try
				{
					if (File.Exists(tempPathToZipFile))
						File.Delete(tempPathToZipFile);
				}
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Extracts the audio from a video. Note, it will fail if the file exists, so the client
		/// is resonsible for verifying with the user and deleting the file before calling this.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ExecutionResult ExtractMonoMp3Audio(string inputPath, string outputPath)
		{
			if (!OfferToDownloadFFmpegForSayMoreIfNecessary())
			{
				var msg = LocalizationManager.GetString(
					"CommonToMultipleViews.FFmpegMethods.UnableToFindFFmpegMsg",
					"Could not locate FFmpeg for SayMore.");

				return new ExecutionResult { StandardError = msg };
			}

			Program.SuspendBackgroundProcesses();

			var stdOut = new StringBuilder();
			var stdErrors = new StringBuilder();
			var result = new ExecutionResult();

			try
			{
				var prs = ExternalProcess.StartProcessToMonitor(GetFullPathToFFmpegForSayMoreExe(),
					GetExtractMonoMp3Args(inputPath), (s, e) => stdOut.AppendLine(e.Data),
					(s, e) => stdErrors.AppendLine(e.Data), null);

				if (prs == null)
				{
					result.ExitCode = 1;
					result.StandardError = LocalizationManager.GetString(
						"CommonToMultipleViews.FFmpegMethods.UnableToStartFFmpegProcessMsg",
						"Unable to start FFmpeg.");

					return result;
				}

				prs.WaitForExit();
				result.ExitCode = prs.ExitCode;
			}
			finally
			{
				Program.ResumeBackgroundProcesses(true);
			}

			result.StandardOutput = stdOut.ToString();
			result.StandardError = stdErrors.ToString();

			// Hide a meaningless error produced by some versions of liblame
			if (result.StandardError.Contains("lame: output buffer too small") && File.Exists(outputPath))
			{
				result.ExitCode = 0;
				result.StandardError = string.Empty;
			}

			return result;
		}

		/// ------------------------------------------------------------------------------------
		private static IEnumerable<string> GetExtractMonoMp3Args(string filePath)
		{
			yield return string.Format("-i \"{0}\"", filePath);
			yield return "-vn";
			yield return "-acodec libmp3lame";
			yield return "-ac 1";
			yield return string.Format("\"{0}\"", Path.ChangeExtension(filePath, "mp4"));
		}
	}
}
