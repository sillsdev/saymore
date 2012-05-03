using System;
using System.IO;
using System.Net;
using Ionic.Zip;

namespace SayMore.Media.FFmpeg
{
	public class FFmpegForSayMoreUtils
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
	}
}
