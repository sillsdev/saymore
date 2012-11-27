using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using NAudio.Wave;
using NAudio.Wave.Compression;
using Palaso.Media;
using Palaso.Media.Naudio;
using Palaso.Media.Naudio.UI;
using Palaso.Progress;
using Palaso.Reporting;
using Palaso.UI.WindowsForms.Miscellaneous;
using SayMore.Media.FFmpeg;
using SayMore.Media.MPlayer;
using SayMore.Properties;
using SayMore.UI;

namespace SayMore.Media.Audio
{
	public class AudioUtils
	{
		public static event Action<Exception> NAudioExceptionThrown;
		private static bool _displayCannotRecordWarning = true;
		private static bool _displayCannotPlayWarning = true;

		/// ------------------------------------------------------------------------------------
		public static bool GetCanRecordAudio(bool suppressWarningDisplay = false)
		{
			if (RecordingDevice.Devices.Any())
			{
				_displayCannotRecordWarning = true;
				return true;
			}

			if (!suppressWarningDisplay && _displayCannotRecordWarning)
			{
				_displayCannotRecordWarning = false; // Don't display this again until the problem is fixed.
				var msg = LocalizationManager.GetString(
					"CommonToMultipleViews.AudioUtils.NoRecordingDevicesFoundMsg",
					"Currently audio recordings cannot be made because SayMore is unable " +
					"to find any recording devices installed and enabled on this computer.");

				ErrorReport.NotifyUserOfProblem(msg);
			}
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetCanPlaybackAudio()
		{
			if (WaveOut.DeviceCount > 0)
			{
				_displayCannotPlayWarning = true;
				return true;
			}

			if (_displayCannotPlayWarning)
			{
				_displayCannotPlayWarning = false; // Don't display this again until the problem is fixed.
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

			if (NAudioExceptionThrown != null)
				NAudioExceptionThrown(e.Exception);

			DisplayNAudioError(e.Exception);
		}

		/// ------------------------------------------------------------------------------------
		public static void DisplayNAudioError(Exception error)
		{
			if (!GetCanPlaybackAudio() || !GetCanRecordAudio())
				return;

			var msg = LocalizationManager.GetString(
				"CommonToMultipleViews.AudioUtils.UnexpectedAudioErrorMsg",
				"There was an unexpected audio error.");

			if (error != null)
				ErrorReport.NotifyUserOfProblem(error, msg);
			else
				ErrorReport.NotifyUserOfProblem(msg);
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsFileStandardPcm(string audioFilePath)
		{
			var waveFormat = GetNAudioWaveFormat(audioFilePath);
			if (waveFormat == null || waveFormat.Encoding != WaveFormatEncoding.Pcm)
				return false;
			return true;
			// ENHANCE (SP-646): Turns out that when we go to generate the oral annotation file,
			// 8-bit audio is a problem. This has been fixed now, but it might make sense to
			// force conversion here instead. See Jira issue for more details.
			//switch (waveFormat.BitsPerSample)
			//{
			//    case 16: case 24: case 32: return true;
			//    default: return false;
			//}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The input media file may be audio or video.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetAudioEncoding(string mediaFilePath)
		{
			var encoding = GetNAudioEncoding(mediaFilePath);

			if (encoding != WaveFormatEncoding.Unknown)
				return encoding.ToString().Replace("WAVE_FORMAT", "WAV").Replace('_', ' ').ToUpperInvariant();

			var info = MediaFileInfo.GetInfo(mediaFilePath);
			return info == null ? string.Empty : info.AudioEncoding;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The input file must be audio. If it is not or the encoding cannot be determined,
		/// WaveFormatEncoding.Unknown is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WaveFormatEncoding GetNAudioEncoding(string audioFilePath)
		{
			var waveFormat = GetNAudioWaveFormat(audioFilePath);
			return waveFormat == null ? WaveFormatEncoding.Unknown : waveFormat.Encoding;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The input file must be audio. If it is not or the format cannot be determined,
		/// null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WaveFormat GetNAudioWaveFormat(string audioFilePath)
		{
			WaveFileReader reader = null;

			try
			{
				if (!GetDoesFileSeemToBeWave(audioFilePath))
					return null;

				reader = new WaveFileReader(audioFilePath);
				return reader.WaveFormat;
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

			return null;
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

		#region Methods for converting to PCM audio stream
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The input media file may be audio or video. If the waitMessage is null, then no
		/// "progress" dialog box will be displayed during the conversion process.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Exception ConvertToStandardPCM(string inputMediaFile,
			string outputMediaFile, Control parent, string waitMessage)
		{
			Program.SuspendBackgroundProcesses();
			Exception error = null;
			WaveFileReader outputReader = null;
			WaitCursor.Show();
			var dlg = (waitMessage == null ? null : new LoadingDlg(waitMessage));

			if (dlg != null)
				dlg.Show(parent ?? Application.OpenForms[0]);

			try
			{
				outputReader = ConvertToStandardPcmStream(inputMediaFile, outputMediaFile, null, out error);
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
				{
					try
					{
						File.Delete(outputMediaFile);
					}
					catch (Exception e)
					{
						ErrorReport.ReportNonFatalException(e);
					}
				}

				if (dlg != null)
				{
					dlg.Close();
					dlg.Dispose();
				}

				Program.ResumeBackgroundProcesses(true);
				WaitCursor.Hide();
			}
		}

		/// ------------------------------------------------------------------------------------
		public static WaveFileReader ConvertToStandardPcmStream(string inputMediaFile,
			string outputAudioFile, WaveFormat preferredOutputFormat, out Exception error)
		{
			try
			{
				error = null;
				string errorMsg;

				if (CheckConversionIsPossible(outputAudioFile, false, out errorMsg))
				{
					if (DoPcmConversion(inputMediaFile, outputAudioFile, preferredOutputFormat))
						return new WaveFileReader(outputAudioFile);

					errorMsg = LocalizationManager.GetString("SoundFileUtils.FileMayNotBeValidAudioError",
						"No audio track could be found in the specified file. Verify that the file is a valid audio file.");
				}

				var msg = LocalizationManager.GetString("SoundFileUtils.ExtractingAudioError",
					"There was an error extracting audio from the media file '{0}'\r\n\r\n{1}",
					"Second parameter is the error message.");

				error = new Exception(string.Format(msg, inputMediaFile, errorMsg));
			}
			catch (Exception e)
			{
				error = e;
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		private static bool DoPcmConversion(string inputMediaFile, string outputAudioFile,
			WaveFormat preferredOutputFormat)
		{
			// TODO: Either figure out how to get FFmpeg to correctly extract PCM audio from Viji Snow.AVI, or else figure out how to detect this condition and use Mplayer.
			// If ffmpeg is is available, then use it. Otherwise do the conversion using mplayer.
			if (!FFmpegDownloadHelper.DoesFFmpegForSayMoreExist)
			{
				string output;
				var result = MPlayerHelper.CreatePcmAudioFromMediaFile(inputMediaFile,
					outputAudioFile, preferredOutputFormat, out output);
				if ((result & MPlayerHelper.ConversionResult.FinishedConverting) == 0)
					return false;
				if ((result & MPlayerHelper.ConversionResult.PossibleError) > 0)
					ReportPossibleConversionProblem(output);
				return true;
			}

			var _model = new ConvertMediaDlgViewModel(inputMediaFile,
					ConvertMediaDlg.GetFactoryExtractToStandardPcmConversionName());

			_model.BeginConversion(null, outputAudioFile, preferredOutputFormat);

			var finishedState = _model.ConversionState & ConvertMediaUIState.AllFinishedStates;

			if (finishedState == ConvertMediaUIState.FinishedConverting)
			{
				if ((_model.ConversionState & ConvertMediaUIState.PossibleError) > 0)
					ReportPossibleConversionProblem(_model.ConversionOutput);
				return File.Exists(outputAudioFile);
			}

			if (finishedState == ConvertMediaUIState.ConversionFailed)
			{
				var e = _model.ConversionException;
				if (e != null)
					throw e;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		private static void ReportPossibleConversionProblem(string details)
		{
			var exception = new Exception(details);
			ErrorReport.NotifyUserOfProblem(exception,
			LocalizationManager.GetString("SoundFileUtils.PossibleConversionErrorMsg",
				"Conversion completed, but with possible error(s). If the converted file " +
				"does not appear to be usable, delete it and check the validity of the source " +
				"file. After checking the source file and the conversion output details, " +
				"if you believe this is a problem with SayMore, please include a copy of the " +
				"source file with your error report."));
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

			if (!File.Exists(outputPath))
				return true;

			var msg = LocalizationManager.GetString(
					"SoundFileUtils.ConversionOutputFileAlreadyErrorMsg",
					"Sorry, the file '{0}' already exists.");

			ErrorReport.NotifyUserOfProblem(msg, Path.GetFileName(outputPath));
			return false;
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
				"There was an error trying to create a standard audio file from:\r\n\r\n{0}",
				"Parameter is the file name of the media file used as the basis for the attempted conversion.");
		}

		#endregion

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

		/// ------------------------------------------------------------------------------------
		public static PeakMeterCtrl CreatePeakMeterControl(Control owningCtrl)
		{
			var peakMeter = new PeakMeterCtrl
			{
				BandsCount = 1,
				LEDCount = 12,
				FalloffSpeed = 7,
				ShowGrid = false,
				BackColor = owningCtrl.BackColor,
				ColorMedium = Settings.Default.BarColorBorder,
				ColorNormal = Settings.Default.BarColorEnd,
				ColorHigh = Color.FromArgb(215, 2, 0),
				Dock = DockStyle.Fill,
			};

			owningCtrl.Controls.Add(peakMeter);
			peakMeter.SetRange(10, 40, 50);
			peakMeter.Start(33); //the number here is how often it updates
			return peakMeter;
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
