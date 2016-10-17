using System;
using System.Windows.Forms;
using SayMore.Media.FFmpeg;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using L10NSharp;
using SIL.IO;
using SIL.Windows.Forms.Miscellaneous;
using static System.String;

namespace SayMore.Transcription.Model.Exporters
{
	/// <summary>
	/// Exports a video file with subtitle burnt into
	/// Using third party tool ffmpeg
	/// </summary>
	public class SubtitledVideoExporter
	{
		public static void Export(string outFilePath, string inVideoPath, string subtitleFilePath)
		{
			string curDirectory = Directory.GetCurrentDirectory();
			// ffmpeg expects subtitle file in the current working directory
			string subtitleFileDir = Path.GetDirectoryName(subtitleFilePath);
			string subtitleFileName = Path.GetFileName(subtitleFilePath);
			Directory.SetCurrentDirectory(subtitleFileDir);

			try
			{
				string ffpath = FFmpegDownloadHelper.FFmpegForSayMoreFolder;
				// string commandArgs = "-i " + inVideoPath + " -vf subtitles=" + subtitleFileName + " -y " + outFilePath;
				// string commandArgs = "-i " + inVideoPath + " -sub_charenc UTF-8 -i " + subtitleFileName +
				//string commandArgs = "-i \"" + inVideoPath + "\" -i " + subtitleFileName +
				//	" -map 0:v -map 0:a -c copy -map 1 -c:s:0 -metadata:s:s:0 \"" + outFilePath + "\"";
				string commandArgs = "-i \"" + inVideoPath + "\" -i " + subtitleFileName + " " + outFilePath + "\"";
				string commandLine = ffpath + "\\ffmpeg " + commandArgs;
				var output = ExecuteCommandSync(commandLine);
				if (!File.Exists(outFilePath))
				{
					var failureMessage = new Regex("(unknown)|(error)|(fail)|(invalid)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
					throw new Exception(Join(Environment.NewLine, output.Split('\r', '\n').Where(line => failureMessage.IsMatch(line))));
				}
			}
			catch (Exception e)
			{
				var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				var productName = (attributes.Length > 0) ? ((AssemblyTitleAttribute)attributes[0]).Title : "Unknown";
				var message = Format(LocalizationManager.GetString(
					"SessionsView.Transcription.TextAnnotationEditor.ExportingVideoWithSubtitles.Error",
					"Failed to export video with subtitles to {0}"), outFilePath);
				if (!IsNullOrEmpty(e.Message))
				{
					message += Environment.NewLine + Environment.NewLine +
						LocalizationManager.GetString(
						"SessionsView.Transcription.TextAnnotationEditor.ExportingVideoWithSubtitles.ErrorDetailsLabel",
						"Error details:") + Environment.NewLine + e.Message;
				}
				MessageBox.Show(Program.ProjectWindow, message, productName);

				// Log the exception
				DesktopAnalytics.Analytics.ReportException(e);
				Console.WriteLine(e);
			}
			finally
			{
				Directory.SetCurrentDirectory(curDirectory);
			}
		}

		// Executes a shell command synchronously.
		private static string ExecuteCommandSync(string command)
		{
			WaitCursor.Show();

			try
			{
				Console.WriteLine("Command being executed:" + Environment.NewLine + command);

				var fontConfigPath = FileLocator.GetDirectoryDistributedWithApplication("mplayer", "fonts");

				// create the ProcessStartInfo using "cmd" as the program to be run,
				// and "/c " as the parameters.
				// Incidentally, /c tells cmd that we want it to execute the command that follows,
				// and then exit.
				System.Diagnostics.ProcessStartInfo procStartInfo =
					new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

				// The following commands are needed to redirect the standard error (which ffmpeg uses for its output).
				// These will be redirected to Process.StandardError.
				procStartInfo.RedirectStandardError = true;
				procStartInfo.UseShellExecute = false;

				// Do not create the black window.
				procStartInfo.CreateNoWindow = true;

				procStartInfo.EnvironmentVariables.Add("FC_CONFIG_DIR", fontConfigPath);
				procStartInfo.EnvironmentVariables.Add("FONTCONFIG_PATH", fontConfigPath);
				procStartInfo.EnvironmentVariables.Add("FC_CONFIG_FILE", Path.Combine(fontConfigPath, "fonts.conf"));

				// Now we create a process, assign its ProcessStartInfo and start it
				System.Diagnostics.Process proc = new System.Diagnostics.Process();
				proc.StartInfo = procStartInfo;
				proc.Start();

				// Get the output into a string
				string output = proc.StandardError.ReadToEnd();

				// Display the command output.
				Console.WriteLine(output);

				return output;
			}
			finally
			{
				WaitCursor.Hide();
			}
		}
	}
}
