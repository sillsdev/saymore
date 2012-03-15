using System;
using System.IO;
using System.Linq;
using Localization;
using NAudio.Wave;
using NAudio.Wave.Compression;
using Palaso.Media;
using Palaso.Progress;
using Palaso.Progress.LogBox;
using Palaso.Reporting;

namespace SayMore.AudioUtils
{
	public class WaveFileUtils
	{
		/// ------------------------------------------------------------------------------------
		public static bool GetIsFilePlainPcm(string audioFilePath)
		{
			return (GetFileAudioFormat(audioFilePath) == WaveFormatEncoding.Pcm);
		}

		/// ------------------------------------------------------------------------------------
		public static WaveFormatEncoding GetFileAudioFormat(string audioFilePath)
		{
			WaveFileReader reader = null;

			try
			{
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
		public static WaveFileReader GetPlainPcmStream(string inputMediaFile,
			string outputAudioFile, WaveFormat preferredOutputFormat, out Exception error)
		{
			error = null;

			try
			{
				WaitCursor.Show();

				var execResult = FFmpegRunner.ExtractPcmAudio(inputMediaFile, outputAudioFile,
					preferredOutputFormat.BitsPerSample, preferredOutputFormat.SampleRate,
					preferredOutputFormat.Channels, new NullProgress());

				if (execResult.ExitCode == 0)
					return new WaveFileReader(outputAudioFile);

				var msg = LocalizationManager.GetString("SoundFileUtils.ExtractingAudioError",
					"There was an error extracting audio from the media file '{0}'\r\n\r\n{1}",
					"Second parameter is the error message.");

				error = new Exception(string.Format(msg, inputMediaFile, execResult.StandardError));
				return null;
			}
			finally
			{
				WaitCursor.Hide();
			}
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
