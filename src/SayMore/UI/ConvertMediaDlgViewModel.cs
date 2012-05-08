using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
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

		private Thread _workerThread;
		private ExternalProcess _process;
		private Action<TimeSpan, string> _conversionReportingAction;
		private TimeSpan _prevReportedTime;

		/// ------------------------------------------------------------------------------------
		public ConvertMediaDlgViewModel(string inputFile, string initialConversionName)
		{
			InputFile = inputFile;

			MediaInfo = MediaFileInfo.GetInfo(inputFile);
			AvailableConversions = FFmpegConversionInfo.GetConversions().OrderBy(c => c.Name).ToArray();
			SelectedConversion = AvailableConversions.FirstOrDefault(c => c.Name == initialConversionName) ?? AvailableConversions[0];

			ConversionState = (FFmpegHelper.DoesFFmpegForSayMoreExist ?
				ConvertMediaUIState.WaitingToConvert : ConvertMediaUIState.FFmpegDownloadNeeded);
		}

		/// ------------------------------------------------------------------------------------
		public void DownloadFFmpeg()
		{
			using (var dlg = new FFmpegDownloadDlg())
				dlg.ShowDialog();

			if (FFmpegHelper.DoesFFmpegForSayMoreExist)
				ConversionState = ConvertMediaUIState.WaitingToConvert;
		}

		/// ------------------------------------------------------------------------------------
		public string GetOutputFileName(bool returnFileNameOnly)
		{
			if (SelectedConversion == null)
				return null;

			var outputFile = Path.ChangeExtension(InputFile, SelectedConversion.OutputExtension);
			return (returnFileNameOnly ? Path.GetFileName(outputFile) : outputFile);
		}

		/// ------------------------------------------------------------------------------------
		public void BeginConversion(Action<TimeSpan, string> conversionReportingAction)
		{
			ConversionState = ConvertMediaUIState.Converting;

			var commandLine = BuildCommandLine();

			_conversionReportingAction = conversionReportingAction;
			_conversionReportingAction(default(TimeSpan), "Command Line: " + commandLine + Environment.NewLine);

			_workerThread = new Thread(DoConversion);
			_workerThread.Name = "FFmpegConversion";
			_workerThread.Priority = ThreadPriority.Normal;
			_workerThread.Start(commandLine);

			while (_workerThread.IsAlive)
				Application.DoEvents();

			if (ConversionState == ConvertMediaUIState.Converting)
				ConversionState = ConvertMediaUIState.FinishedConverting;
		}

		/// ------------------------------------------------------------------------------------
		private string BuildCommandLine()
		{
			var commandLine = "-i \"" + InputFile + "\" " +
				SelectedConversion.CommandLine + " \"" + GetOutputFileName(false) + "\"";

			var bitRate = (MediaInfo.VideoBitRate == 0 ? string.Empty :
				MediaInfo.VideoBitRate.ToString(CultureInfo.InvariantCulture));

			commandLine = commandLine.Replace("{vb}", bitRate);

			bitRate = (MediaInfo.Audio.BitRate == 0 ? string.Empty :
				MediaInfo.Audio.BitRate.ToString(CultureInfo.InvariantCulture));

			return commandLine.Replace("{ab}", bitRate);
		}

		/// ------------------------------------------------------------------------------------
		private void DoConversion(object commandLine)
		{
			// ffmpeg always seems to write the output to standarderror.
			// I don't understand why and that's wrong, but we'll deal with it.
			_process = ExternalProcess.StartProcessToMonitor(
				FFmpegHelper.GetFullPathToFFmpegForSayMoreExe(), commandLine as string,
				HandleProcessDataReceived, HandleProcessDataReceived, null);

			_process.WaitForExit();
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

			var outputFile = GetOutputFileName(false);
			FileSystemUtils.WaitForFileRelease(outputFile);
			if (File.Exists(outputFile))
				File.Delete(outputFile);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleProcessDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (e.Data != null && e.Data.ToLower().Contains("error"))
				ConversionState = ConvertMediaUIState.ConversionFailed;

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

			if (data.StartsWith("frame=") && i >= 0)
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
