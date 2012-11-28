using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Localization;
using NAudio.Wave;
using SayMore.Media;
using SayMore.Media.FFmpeg;
using SayMore.Utilities;

namespace SayMore.UI
{
	[Flags]
	public enum ConvertMediaUIState
	{
		FFmpegDownloadNeeded = 0,
		WaitingToConvert = 1,
		Converting = 2,
		ConversionCancelled = 4,
		ConversionFailed = 8,
		FinishedConverting = 16,
		PossibleError = 32,
		InvalidMediaFile = 64,
		AllFinishedStates = ConversionCancelled | ConversionFailed | FinishedConverting
	}

	/// ----------------------------------------------------------------------------------------
	public class ConvertMediaDlgViewModel
	{
		public string InputFile { get; private set; }
		public FFmpegConversionInfo SelectedConversion { get; set; }
		public FFmpegConversionInfo[] AvailableConversions { get; private set; }
		public ConvertMediaUIState ConversionState { get; private set; }
		public MediaFileInfo MediaInfo { get; private set; }
		public string OutputFileCreated { get; private set; }

		private Thread _workerThread;
		private ExternalProcess _process;
		private Action<TimeSpan, string> _conversionReportingAction;
		private TimeSpan _prevReportedTime;
		private StringBuilder _conversionOutput;
		private string _codecError;

		/// ------------------------------------------------------------------------------------
		public ConvertMediaDlgViewModel(string inputFile, string initialConversionName)
		{
			InputFile = inputFile;

			MediaInfo = MediaFileInfo.GetInfo(inputFile);
			if (MediaInfo == null)
				ConversionState = ConvertMediaUIState.InvalidMediaFile;
			else
			{
				AvailableConversions = FFmpegConversionInfo.GetConversions(inputFile).OrderBy(c => c.Name).ToArray();
				SelectedConversion = AvailableConversions.FirstOrDefault(c => c.Name == initialConversionName) ?? AvailableConversions[0];

				ConversionState = (FFmpegDownloadHelper.DoesFFmpegForSayMoreExist ?
					ConvertMediaUIState.WaitingToConvert : ConvertMediaUIState.FFmpegDownloadNeeded);
			}
		}

		/// ------------------------------------------------------------------------------------
		public Exception ConversionException
		{
			get
			{
				return _codecError == null ? null :
					new Exception(_codecError + Environment.NewLine +
					LocalizationManager.GetString("ConvertMedia.FullConversionOutput", "Full output from conversion:") +
					Environment.NewLine + _conversionOutput);
			}
		}

		/// ------------------------------------------------------------------------------------
		public string ConversionOutput
		{
			get { return _conversionOutput.ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		public void DownloadFFmpeg()
		{
			using (var dlg = new FFmpegDownloadDlg())
				dlg.ShowDialog();

			if (FFmpegDownloadHelper.DoesFFmpegForSayMoreExist)
				ConversionState = ConvertMediaUIState.WaitingToConvert;
		}

		/// ------------------------------------------------------------------------------------
		public string GetNewOutputFileName(bool returnFileNameOnly)
		{
			if (SelectedConversion == null)
				return null;

			var outputFile = Path.ChangeExtension(InputFile, SelectedConversion.OutputExtension);

			while (File.Exists(outputFile))
			{
				int fileNumber = 1;

				var fileWOExt = Path.GetFileNameWithoutExtension(outputFile);
				if (fileWOExt.Length > 3)
				{
					if (fileWOExt[fileWOExt.Length - 3] == '_')
					{
						fileNumber = (int.TryParse(fileWOExt.Substring(fileWOExt.Length - 2),
							out fileNumber) ? fileNumber + 1 : 1);

						fileWOExt = fileWOExt.Substring(0, fileWOExt.Length - 3);
					}
				}

				fileWOExt += string.Format("_{0:D2}", fileNumber);
				outputFile = Path.Combine(Path.GetDirectoryName(outputFile),
					fileWOExt + "." + SelectedConversion.OutputExtension);
			}

			return (returnFileNameOnly ? Path.GetFileName(outputFile) : outputFile);
		}

		/// ------------------------------------------------------------------------------------
		public void BeginConversion(Action<TimeSpan, string> conversionReportingAction,
			string outputFile = null, WaveFormat preferredOutputFormat = null)
		{
			if (MediaInfo == null)
			{
				ConversionState = ConvertMediaUIState.ConversionFailed;
				return;
			}

			if (outputFile == null)
				outputFile = GetNewOutputFileName(false);
			var commandLine = BuildCommandLine(outputFile, preferredOutputFormat);
			ConversionState = ConvertMediaUIState.Converting;

			_conversionReportingAction = conversionReportingAction;
			if (_conversionReportingAction != null)
			{
				_conversionReportingAction(default(TimeSpan),
					"Command Line: " + commandLine + Environment.NewLine);
			}

			_workerThread = new Thread(DoConversion);
			_workerThread.Name = "FFmpegConversion";
			_workerThread.Priority = ThreadPriority.Normal;
			_workerThread.Start(commandLine);

			while (_workerThread.IsAlive)
				Application.DoEvents();

			if ((ConversionState & ConvertMediaUIState.Converting) > 0)
			{
				if (File.Exists(outputFile))
				{
					ConversionState ^= ConvertMediaUIState.Converting;
					ConversionState |= ConvertMediaUIState.FinishedConverting;
				}
				else
					ConversionState = ConvertMediaUIState.ConversionFailed;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string BuildCommandLine(string outputFileName, WaveFormat preferredOutputFormat = null)
		{
			OutputFileCreated = outputFileName;

			var switches = (SelectedConversion.CommandLine != null ?
				SelectedConversion.CommandLine.Trim() : string.Empty);

			if (!switches.Contains(" -ar ") && preferredOutputFormat != null &&
				preferredOutputFormat.SampleRate > 0 && preferredOutputFormat.SampleRate != MediaInfo.SamplesPerSecond)
			{
				switches += " -ar " + preferredOutputFormat.SampleRate;
			}

			var commandLine = string.Format("-i \"{0}\" {1} \"{2}\"",
				InputFile, switches, OutputFileCreated);

			var bitRate = (MediaInfo.VideoBitRate == 0 ? string.Empty :
				MediaInfo.VideoBitRate.ToString(CultureInfo.InvariantCulture));

			if (commandLine.Contains("{pcm}"))
			{
				var bps = MediaInfo.BitsPerSample;
				if (preferredOutputFormat != null && preferredOutputFormat.BitsPerSample > bps)
					bps = preferredOutputFormat.BitsPerSample;
				switch (bps)
				{
					case 32: commandLine = commandLine.Replace("{pcm}", "pcm_f32le"); break;
					case 24: commandLine = commandLine.Replace("{pcm}", "pcm_s24le"); break;
					// ffmpeg says: pcm_s8 codec not supported in WAVE format. Probably don't want this anyway.
					//case 8: commandLine = commandLine.Replace("{pcm}", "pcm_s8"); break;
					default: commandLine = commandLine.Replace("{pcm}", "pcm_s16le"); break;
				}
			}

			commandLine = commandLine.Replace("{vb}", bitRate);

			bitRate = (MediaInfo.Audio.BitRate == 0 ? string.Empty :
				MediaInfo.Audio.BitRate.ToString(CultureInfo.InvariantCulture));

			return commandLine.Replace("{ab}", bitRate);
		}

		/// ------------------------------------------------------------------------------------
		private void DoConversion(object commandLine)
		{
			var exePath = FFmpegDownloadHelper.GetFullPathToFFmpegForSayMoreExe();
			_conversionOutput = new StringBuilder(exePath);
			_conversionOutput.Append(commandLine);

			// ffmpeg always seems to write the output to standarderror.
			// I don't understand why and that's wrong, but we'll deal with it.
			_process = ExternalProcess.StartProcessToMonitor(exePath, commandLine as string,
				HandleProcessDataReceived, HandleProcessDataReceived, null);

			try
			{
				_process.PriorityClass = ProcessPriorityClass.BelowNormal;
			}
			catch (InvalidOperationException)
			{
				// process probably already exited
			}
			_process.WaitForExit();

			if (_conversionReportingAction != null)
				_conversionReportingAction(TimeSpan.FromSeconds(int.MaxValue), null);
		}

		/// ------------------------------------------------------------------------------------
		public void Cancel()
		{
			if (_process == null || _process.HasExited)
				return;

			ConversionState = ConvertMediaUIState.ConversionCancelled;
			_process.KillProcess();
			_process.Dispose();

			DeleteOutputFile();
		}

		/// ------------------------------------------------------------------------------------
		public void DeleteOutputFile()
		{
			if (OutputFileCreated != null && File.Exists(OutputFileCreated))
			{
				FileSystemUtils.WaitForFileRelease(OutputFileCreated);
				File.Delete(OutputFileCreated);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleProcessDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (e.Data != null)
			{
				_conversionOutput.Append(Environment.NewLine);
				_conversionOutput.Append(e.Data);

				if (e.Data.ToLower().Contains("could not find codec"))
				{
					if (_codecError == null)
						_codecError = e.Data;
					ConversionState = ConvertMediaUIState.ConversionFailed;
				}
				else if (e.Data.ToLower().Contains("error"))
					ConversionState |= ConvertMediaUIState.PossibleError;
			}

			if (_conversionReportingAction != null)
				_conversionReportingAction(GetTimeOfProgress(e.Data), e.Data);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetTimeOfProgress(string outputData)
		{
			// A sample line from ffmpeg output looks like this:
			// frame=  593 fps=192 q=3.6 size=    1628kB time=00:00:59.30 bitrate= 224.9kbits/s

			if (outputData == null)
				return _prevReportedTime;

			var data = outputData.ToLower();
			int i = data.IndexOf("time=", StringComparison.Ordinal);

			if ((data.StartsWith("frame=") || data.StartsWith("size=")) && i >= 0)
			{
				var time = data.Substring(i + 5, 11);
				TimeSpan returnTime;
				if (TimeSpan.TryParse(time, out returnTime))
					_prevReportedTime = returnTime;
			}

			return _prevReportedTime;
		}
	}
}
