using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SayMore.Media;
using SayMore.Media.FFmpeg;

namespace SayMore.MediaUtils
{
	public class VideoConversionUtils
	{
		public static void ConvertToMp4(string filePath)
		{
			if (!FFmpegHelper.DoesFFmpegForSayMoreExist)
			{
				using (var dlg = new FFmpegDownloadDlg())
					dlg.ShowDialog();
			}

			if (!FFmpegHelper.DoesFFmpegForSayMoreExist)
				return;

			Program.SuspendBackgroundProcesses();

			try
			{
				var prs = ExternalProcess.StartProcessToMonitor(
					FFmpegHelper.GetFullPathToFFmpegForSayMoreExe(),
					GetConvertToMp4Args(filePath),
					HandleFFmpegOutputDataReceived, HandleFFmpegOutputDataReceived, "");

				prs.WaitForExit();
			}
			finally
			{
				Program.ResumeBackgroundProcesses(true);
			}
			//LocalizationManager.GetString(
			//    "CommonToMultipleViews.MediaPlayer.UnableToStartMplayerProcessMsg",
			//    "Unable to start mplayer.")

		}

		private static IEnumerable<string> GetConvertToMp4Args(string filePath)
		{
			var mediaInfo = MediaFileInfo.GetInfo(filePath);

			yield return string.Format("-i \"{0}\"", filePath);
			yield return "-vb " + mediaInfo.VideoBitRate;
			yield return "-vcodec mpeg4";
			yield return "-ab " + mediaInfo.Audio.BitRate;
			yield return "-acodec aac";
			yield return "-strict -2";
			yield return string.Format("\"{0}\"", Path.ChangeExtension(filePath, "mp4"));
		}

		// ffmpeg -i {0} -vb 1000k -vcodec mpeg4 -ab 320k -acodec aac -strict -2  {1}
		// ffmpeg -i {0} -vb 1000k -vcodec mpeg4 -ab 320k -acodec libfaac {1}
		//-f s16le -acodec pcm_s16le

		/// ------------------------------------------------------------------------------------
		private static void HandleFFmpegOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(e.Data as string);

//frame=   93 fps=0.0 q=2.0 size=     178kB time=00:00:09.30 bitrate= 157.2kbits/s
//frame=  178 fps=173 q=2.0 size=     427kB time=00:00:17.80 bitrate= 196.4kbits/s
//frame=  298 fps=193 q=2.0 size=     678kB time=00:00:29.80 bitrate= 186.5kbits/s
//frame=  408 fps=198 q=3.1 size=    1068kB time=00:00:40.80 bitrate= 214.4kbits/s
//frame=  509 fps=198 q=2.0 size=    1279kB time=00:00:50.90 bitrate= 205.8kbits/s
//frame=  593 fps=192 q=3.6 size=    1628kB time=00:00:59.30 bitrate= 224.9kbits/s
//frame=  683 fps=190 q=6.9 size=    1896kB time=00:01:08.30 bitrate= 227.4kbits/s
//frame=  778 fps=189 q=17.5 size=    2128kB time=00:01:17.80 bitrate= 224.1kbits/s
//frame=  855 fps=186 q=14.9 Lsize=    2334kB time=00:01:25.50 bitrate= 223.6kbits/s


			//if (e.Data != null)
			//    HandlePlayerOutput(e.Data);
		}
	}
}
