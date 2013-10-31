using System;
using SayMore.Media.FFmpeg;
using System.IO;

namespace SayMore.Transcription.Model.Exporters
{
	/// <summary>
	/// Exports a video file with subtitle burnt into
	/// Using third party tool ffmpeg
	/// </summary>
	public class TestFormatVideoExporter
	{
		public static void Export(string outFilePath, string inVideoPath, string subtitleFilePath)
		{
			string curDirectory = Directory.GetCurrentDirectory();
			// ffmpeg expects subtitle file in the current working directory
			string subtitleFileDir = Path.GetDirectoryName(subtitleFilePath);
			string subtitleFileName = Path.GetFileName(subtitleFilePath);
			Directory.SetCurrentDirectory(subtitleFileDir);

			string ffpath = FFmpegDownloadHelper.FFmpegForSayMoreFolder;
			string commandArgs = "-i " + inVideoPath + " -vf subtitles=" + subtitleFileName + " -y " + outFilePath;
			string commandLine = ffpath+"\\ffmpeg "+commandArgs;
			ExecuteCommandSync(commandLine);
			Directory.SetCurrentDirectory(curDirectory);
		}

		// Executes a shell command synchronously.
		private static void ExecuteCommandSync(string command)
		{
			try
			{
				// create the ProcessStartInfo using "cmd" as the program to be run,
				// and "/c " as the parameters.
				// Incidentally, /c tells cmd that we want it to execute the command that follows,
				// and then exit.
				System.Diagnostics.ProcessStartInfo procStartInfo =
					new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

				// The following commands are needed to redirect the standard output.
				// This means that it will be redirected to the Process.StandardOutput StreamReader.
				procStartInfo.RedirectStandardOutput = true;
				procStartInfo.UseShellExecute = false;
				// Do not create the black window.
				procStartInfo.CreateNoWindow = true;
				// Now we create a process, assign its ProcessStartInfo and start it
				System.Diagnostics.Process proc = new System.Diagnostics.Process();
				proc.StartInfo = procStartInfo;
				proc.Start();
				// Get the output into a string
				string result = proc.StandardOutput.ReadToEnd();
				// Display the command output.
				System.Console.WriteLine(result);
			}
			catch (System.Exception objException)
			{
				// Log the exception
				System.Console.WriteLine(objException);
			}
		}
	}
}
