using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using NAudio.Wave;
using NAudio.Wave.Compression;
using Palaso.Media;
using Palaso.Media.Naudio;
using Palaso.Progress;
using Palaso.Progress.LogBox;
using Palaso.Reporting;
using SayMore.Media.UI;
using SayMore.Model.Files;
using SayMore.UI;

namespace SayMore.Media
{
	public class AudioUtils
	{
		public static Action<Exception> NAudioErrorAction { get; set; }

		/// ------------------------------------------------------------------------------------
		public static bool GetCanRecordAudio()
		{
			return (RecordingDevice.Devices.Any());
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetCanPlaybackAudio()
		{
			return (WaveOut.DeviceCount > 0);
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetCanRecordAudio(bool displayWarning)
		{
			if (GetCanRecordAudio())
				return true;

			if (displayWarning)
			{
				var msg = LocalizationManager.GetString(
					"CommonToMultipleViews.AudioUtils.NoRecordingDevicesFoundMsg",
					"Currently audio recordings cannot be made because SayMore is unable " +
					"to find any recording devices installed and enabled on this computer.");

				ErrorReport.NotifyUserOfProblem(msg);
			}
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetCanPlaybackAudio(bool displayWarning)
		{
			if (GetCanPlaybackAudio())
				return true;

			if (displayWarning)
			{
				var msg = LocalizationManager.GetString(
					"CommonToMultipleViews.AudioUtils.NoPlaybackDevicesFoundMsg",
					"Currently SayMore is unable to find any audio playback " +
					"devices installed and enabled on this computer.");

				ErrorReport.NotifyUserOfProblem(msg);
			}
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsNAudioError(Exception e)
		{
			return (e.Source == "NAudio");
		}

		/// ------------------------------------------------------------------------------------
		public static void HandleGlobalNAudioException(object sender, CancelExceptionHandlingEventArgs e)
		{
			if (!GetIsNAudioError(e.Exception))
				return;

			e.Cancel = true;

			if (GetCanPlaybackAudio(true) && GetCanRecordAudio(true))
			{
				var msg = LocalizationManager.GetString(
					"CommonToMultipleViews.AudioUtils.UnexpectedAudioErrorMsg",
					"There was an unexpected audio error.");

				ErrorReport.NotifyUserOfProblem(e.Exception, msg);
			}

			if (NAudioErrorAction != null)
				NAudioErrorAction(e.Exception);
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsFileStandardPcm(string audioFilePath)
		{
			return (GetNAudioEncoding(audioFilePath) == WaveFormatEncoding.Pcm);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The input media file may be audio or video.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetAudioEncoding(string mediaFilePath)
		{
			var encoding = GetNAudioEncoding(mediaFilePath);
			return (encoding != WaveFormatEncoding.Unknown ?
				encoding.ToString().Replace("WAVE_FORMAT", "WAV").Replace('_', ' ').ToUpperInvariant() :
				MPlayerHelper.GetAudioEncoding(mediaFilePath));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The input file must be audio. If it is not or the encoding cannot be determined,
		/// WaveFormatEncoding.Unknown is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WaveFormatEncoding GetNAudioEncoding(string audioFilePath)
		{
			WaveFileReader reader = null;

			try
			{
				if (!GetDoesFileSeemToBeWave(audioFilePath))
					return WaveFormatEncoding.Unknown;

				reader = new WaveFileReader(audioFilePath);
				return reader.WaveFormat.Encoding;
			}
			catch { }
			finally
			{
				if (reader != null)
				{
					reader.Close();
					reader.Dispose();
				}
			}

			return WaveFormatEncoding.Unknown;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will use NAduio's WaveFileReader.ReadWaveHeader to determine whether
		/// or not the specified file is a valid wave file. I would just try to create a
		/// WaveFileReader and catch the exception if construction fails. However if the
		/// file is not a valid wave file, NAudio throws an exception but does not close the
		/// file. Therefore, we open the file ourselves, then pass the stream to the
		/// WaveFileReader.ReadWaveHeader and see if that throws an exception.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool GetDoesFileSeemToBeWave(string mediaFilePath)
		{
			FileStream stream = null;

			try
			{
				stream = File.OpenRead(mediaFilePath);
				WaveFormat fmt;
				long pos;
				int len;
				WaveFileReader.ReadWaveHeader(stream, out fmt, out pos, out len, new List<RiffChunk>());
				return true;
			}
			catch { }
			finally
			{
				if (stream != null)
				{
					stream.Close();
					stream.Dispose();
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public static WaveFormat GetDefaultWaveFormat(int channels)
		{
			WaveFormat bestFormat = null;

			var pcmDriver = AcmDriver.EnumerateAcmDrivers().SingleOrDefault(d => d.ShortName == "MS-PCM");
			if (pcmDriver != null)
			{
				pcmDriver.Open();

				var formatTag = pcmDriver.FormatTags.SingleOrDefault(t => t.FormatTag == WaveFormatEncoding.Pcm);
				if (formatTag != null)
				{
					foreach (var fmt in pcmDriver.GetFormats(formatTag))
					{
						if (bestFormat == null ||
							fmt.WaveFormat.BitsPerSample > bestFormat.BitsPerSample ||
							fmt.WaveFormat.SampleRate > bestFormat.SampleRate)
						{
							bestFormat = fmt.WaveFormat;
						}
					}
				}
				else
				{
					var msg = LocalizationManager.GetString("SoundFileUtils.ErrorFindingPcmConversionCapabilitiesMsg",
						"There was an error trying to find PCM audio conversion capabilities on this computer. Ensure that you have a PCM sound driver installed.");

					ErrorReport.NotifyUserOfProblem(msg);
				}

				pcmDriver.Close();
			}
			else
			{
				var msg = LocalizationManager.GetString("SoundFileUtils.ErrorFindingPcmAudioDriverMsg",
					"There was an error trying to find a PCM audio driver on this computer. Ensure that you have a PCM sound driver installed.");

				ErrorReport.NotifyUserOfProblem(msg);
			}

			return new WaveFormat(bestFormat.SampleRate, bestFormat.BitsPerSample, channels);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The input media file may be audio or video. If the waitMessage is null, then no
		/// "progress" dialog box will be displayed during the conversion process.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Exception ConvertToStandardPCM(string inputMediaFile,
			string outputMediaFile, Control parent, string waitMessage)
		{
			Exception error = null;
			WaveFileReader outputReader = null;
			var dlg = (waitMessage == null ? null : new LoadingDlg(waitMessage));

			try
			{
				if (dlg != null)
				{
					parent = (parent ?? Application.OpenForms[0]);
					dlg.Show(parent);
				}

				WaitCursor.Show();
				var worker = new BackgroundWorker();
				worker.DoWork += delegate
				{
					// TODO: Get and use the audio's bits/sample from ffmpeg or mplayer output dump.
					int channels = GetChannelsFromMediaFile(inputMediaFile);
					var format = GetDefaultWaveFormat(channels);
					outputReader = ConvertToStandardPcmStream(inputMediaFile, outputMediaFile, format, out error);
				};

				worker.RunWorkerAsync();
				while (worker.IsBusy) { Application.DoEvents(); }
				return error;
			}
			finally
			{
				if (outputReader != null)
				{
					outputReader.Close();
					outputReader.Dispose();
				}

				if (error != null && File.Exists(outputMediaFile))
					File.Delete(outputMediaFile);

				if (dlg != null)
				{
					dlg.Close();
					dlg.Dispose();
				}

				WaitCursor.Hide();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The input media file may be audio or video.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static int GetChannelsFromMediaFile(string mediaFilePath)
		{
			var mediaInfo = new MediaFileInfo(mediaFilePath);

			// There are some media files (e.g. MTS) for which ffmpeg -- which is what the
			// MediaFileInfo class uses -- returns 0 channels. Hence the check. When that
			// happens, then get the information using mplayer.
			return (mediaInfo.Channels > 0 ? mediaInfo.Channels :
				MPlayerHelper.GetAudioChannels(mediaFilePath));
		}

		/// ------------------------------------------------------------------------------------
		public static WaveFileReader ConvertToStandardPcmStream(string inputMediaFile,
			string outputAudioFile, WaveFormat preferredOutputFormat, out Exception error)
		{
			try
			{
				error = null;
				string errorMsg = null;

				if (CheckConversionIsPossible(outputAudioFile, false, out errorMsg))
				{
					var execResult = FFmpegRunner.ExtractPcmAudio(inputMediaFile, outputAudioFile,
						preferredOutputFormat.BitsPerSample, preferredOutputFormat.SampleRate,
						preferredOutputFormat.Channels, new NullProgress());

					if (execResult.ExitCode == 0)
						return new WaveFileReader(outputAudioFile);

					errorMsg = execResult.StandardError;
				}

				var msg = LocalizationManager.GetString("SoundFileUtils.ExtractingAudioError",
					"There was an error extracting audio from the media file '{0}'\r\n\r\n{1}",
					"Second parameter is the error message.");

				error = new Exception(String.Format(msg, inputMediaFile, errorMsg));
			}
			catch (Exception e)
			{
				error = e;
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		public static bool CheckConversionIsPossible(string outputPath)
		{
			string errorMsg;
			return CheckConversionIsPossible(outputPath, true, out errorMsg);
		}

		/// ------------------------------------------------------------------------------------
		public static bool CheckConversionIsPossible(string outputPath,
			bool showMsg, out string message)
		{
			message = null;

			if (!MediaInfo.HaveNecessaryComponents)
			{
				var msg = LocalizationManager.GetString("SoundFileUtils.FFmpegMissingErrorMsg",
					"SayMore could not find the proper FFmpeg on this computer. FFmpeg is required to do that conversion.");

				ErrorReport.NotifyUserOfProblem(msg);
				return false;
			}

			if (File.Exists(outputPath))
			{
				var msg = LocalizationManager.GetString(
					"SoundFileUtils.ConversionOutputFileAlreadyErrorMsg",
					"Sorry, the file '{0}' already exists.");

				ErrorReport.NotifyUserOfProblem(msg, Path.GetFileName(outputPath));
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public static string GetConvertingToStandardPcmAudioMsg()
		{
			return LocalizationManager.GetString(
				"SoundFileUtils.ConvertToStandardWavPcmAudioMsg", "Converting...");
		}

		/// ------------------------------------------------------------------------------------
		public static string GetConvertingToStandardPcmAudioErrorMsg()
		{
			return LocalizationManager.GetString(
				"SoundFileUtils.ConvertToStandardWavPcmAudioErrorMsg",
				"There was an error trying to create a standard audio file from:\r\n\r\n{0}");
		}

		/// ------------------------------------------------------------------------------------
		public static string GetGeneralFFmpegConversionErrorMsg()
		{
			return LocalizationManager.GetString("SoundFileUtils.GeneralFFmpegFailureMsg",
				"Something didn't work out. FFmpeg reported the following (start " +
				"reading from the end):\r\n\r\n{0}");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// It's up to the caller to close and dispose of the stream.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WaveStream GetOneChannelStreamFromAudio(string audioFilePath,
			 out Exception error)
		{
			return GetOneChannelStreamFromAudio(audioFilePath, out error, new NullProgress());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// It's up to the caller to close and dispose of the stream.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WaveStream GetOneChannelStreamFromAudio(string audioFilePath,
			 out Exception error, IProgress progress)
		{
			error = null;

			try
			{
				var stream = new WaveFileReader(audioFilePath);
				if (stream.WaveFormat.Channels == 1 || !FFmpegRunner.HaveNecessaryComponents)
					return stream;
			}
			catch (Exception e)
			{
				error = e;
				return null;
			}

			var tmpFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(audioFilePath));

			if (File.Exists(tmpFile))
				File.Delete(tmpFile);

			try
			{
				FFmpegRunner.ChangeNumberOfAudioChannels(audioFilePath, tmpFile, 1, progress);

				// WaveFileReader does not read the entire file into a buffer right away. Therefore,
				// the file will remain open but we want to delete it right away. So read it
				// into our own stream and pass that to WaveFileReader so the file is no longer
				// needed and can be deleted.
				return new WaveFileReader(new MemoryStream(File.ReadAllBytes(tmpFile)));
			}
			catch (Exception e)
			{
				error = e;
				return null;
			}
			finally
			{
				if (File.Exists(tmpFile))
				{
					try { File.Delete(tmpFile); }
					catch { }
				}
			}
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Joins two or more wave files and writes them to the specified output wave file.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public static TimeSpan[] Join(IEnumerable<string> inputWaveFiles, string outputWaveFile)
		//{
		//    return Join(44100, 1, inputWaveFiles, outputWaveFile);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Joins two or more wave files and writes them to the specified output wave file.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public static TimeSpan[] Join(int ouputFileSampleRate, int ouputFileChannels,
		//    IEnumerable<string> inputWaveFiles, string outputWaveFile)
		//{
		//    if (inputWaveFiles == null)
		//        throw new ArgumentNullException("inputWaveFiles");

		//    var inputFiles = inputWaveFiles.ToArray();

		//    if (inputFiles.Length == 0)
		//        throw new Exception("List of wave files to join must contain at least one item.");

		//    var writer = new WaveFileWriter(outputWaveFile,
		//        new WaveFormat(ouputFileSampleRate, ouputFileChannels));

		//    try
		//    {
		//        var fileLengths = inputFiles.Select(f => WriteSingleWaveFile(f, ref writer)).ToArray();
		//        return fileLengths;
		//    }
		//    finally
		//    {
		//        writer.Close();
		//        writer.Dispose();
		//    }
		//}

		///// ------------------------------------------------------------------------------------
		//private static TimeSpan WriteSingleWaveFile(string inputWaveFile, ref WaveFileWriter writer)
		//{
		//    var reader = new WaveFileReader(inputWaveFile);
		//    var totalTime = reader.TotalTime;
		//    var buffer = new byte[4096];
		//    var count = int.MaxValue;

		//    while (count >= 4096)
		//    {
		//        count = reader.Read(buffer, 0, 4096);
		//        if (count > 0)
		//            writer.Write(buffer, 0, count);
		//    }

		//    reader.Close();
		//    reader.Dispose();
		//    return totalTime;
		//}
	}
}
