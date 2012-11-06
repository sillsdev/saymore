using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Palaso.IO;
using Localization;

namespace SayMore.Media.MPlayer
{
	#region MPlayerHelper class
	/// ----------------------------------------------------------------------------------------
	public static class MPlayerHelper
	{
		[Flags]
		public enum ConversionResult
		{
			InProgress = 0,
			ConversionFailed = 1,
			FinishedConverting = 2,
			PossibleError = 4,
		}

		/// ------------------------------------------------------------------------------------
		public static string MPlayerPath
		{
			get { return FileLocator.GetFileDistributedWithApplication("mplayer", "mplayer.exe"); }
		}

		/// ------------------------------------------------------------------------------------
		public static ExternalProcess StartProcessToMonitor(IEnumerable<string> args,
			DataReceivedEventHandler outputDataHandler, DataReceivedEventHandler errorDataHandler)
		{
			return ExternalProcess.StartProcessToMonitor(MPlayerPath, args,
				outputDataHandler, errorDataHandler, LocalizationManager.GetString(
				"CommonToMultipleViews.MediaPlayer.UnableToStartMplayerProcessMsg",
				"Unable to start mplayer."));
		}

		#region Methods for building MPlayer command-line arguments.
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<string> GetPlaybackArguments(float startPosition, float duration,
			float volume, int speed, bool resampleToMono, int hwndVideo, int bitsPerSample)
		{
			var mplayerConfigPath = Path.Combine(GetPathToThisAssembly(), "MPlayerSettings.conf");

			if (File.Exists(mplayerConfigPath))
			{
				yield return string.Format("-include {0}", mplayerConfigPath);
			}
			else
			{
				yield return "-slave";
				yield return "-noquiet";
				yield return "-idle ";
				yield return "-msglevel identify=9:global=9";
				yield return "-nofontconfig";
				yield return "-autosync 100";
				yield return "-priority abovenormal";
				yield return "-osdlevel 0";
				yield return string.Format("-volume {0}", volume);
				yield return (resampleToMono ? "-af pan=1:1:1:1,scaletempo" : "-af scaletempo");

				if (speed != 100)
					yield return string.Format("-speed {0}", speed / 100d);

				if (startPosition > 0f)
					yield return string.Format("-ss {0}", startPosition);

				if (duration > 0f)
					yield return string.Format("-endpos {0}", duration);

				// A window handle of -1 means we're only playing back
				// the audio portion of a video file.
				if (hwndVideo == -1)
					yield return "-novideo";

				if (hwndVideo > 0)
				{
					yield return "-fixed-vo";

#if !__MonoCS__
					yield return "-vo gl";
#endif
					yield return string.Format("-wid {0}", hwndVideo);
				}
#if !__MonoCS__
				else if (bitsPerSample > 16)
				{
					// REVIEW: For some reason, 24-bit audio doesn't work correctly with the -ao win32 switch, unless the -format
					// switch is also included. I can't come up with a logical explanation for this, especially since it doesn't
					// seem to matter which format is used.
					yield return "-format u16le";
				}
				// This forces the use of the native windows audio driver, which seems to fix SP-403: playback truncation
				yield return "-ao win32";
#endif
			}
		}

		/// ------------------------------------------------------------------------------------
		private static IEnumerable<string> GetArgumentsToCreatePcmAudio(MediaFileInfo info,
			string audioOutPath)
		{
			yield return "\"" + info.MediaFilePath + "\"";
			yield return "-nofontconfig";
			yield return "-nocorrect-pts";
			yield return "-vo null";
			yield return "-vc null";
			yield return string.Format("-af channels={0}", info.Audio.Channels);
			yield return string.Format("-ao pcm:fast:file=%{0}%\"{1}\"", audioOutPath.Length, audioOutPath);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private static string GetPathToThisAssembly()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var mplayerConfigPath = assembly.CodeBase.Replace("file:", string.Empty).TrimStart('/');
			return Path.GetDirectoryName(mplayerConfigPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Extracts the audio stream from the specified video file and writes it to a wav
		/// file (i.e. raw PCM).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ConversionResult CreatePcmAudioFromMediaFile(string mediaInPath,
			string audioOutPath, out string output)
		{
			output = null;

			mediaInPath = mediaInPath.Replace('\\', '/');
			var info = MediaFileInfo.GetInfo(mediaInPath);
			if (info == null)
				return ConversionResult.ConversionFailed;

			var args = GetArgumentsToCreatePcmAudio(info, audioOutPath);
			var finishedProcessing = false;
			var result = ConversionResult.InProgress;
			var outputBuilder = new StringBuilder();

			var prs = StartProcessToMonitor(args,
				(s, e) =>
					{
						if (e.Data != null)
							outputBuilder.AppendLine(e.Data);
						finishedProcessing = (e.Data == "Exiting... (End of file)");
					},
				(s, e) =>
					{
						if (e.Data != null)
						{
							outputBuilder.AppendLine(e.Data);
							if (e.Data == "Seek failed")
								result |= ConversionResult.PossibleError;
							else if (e.Data.StartsWith("Failed to open"))
								result = ConversionResult.ConversionFailed;
						}
					});

			if (prs == null)
				return ConversionResult.ConversionFailed;

			while (!finishedProcessing && result != ConversionResult.ConversionFailed)
				Application.DoEvents();

			prs.Dispose();
			if (result != ConversionResult.ConversionFailed)
			{
				result |= ConversionResult.FinishedConverting;
				output = outputBuilder.ToString();
			}
			return result;
		}

		/// ------------------------------------------------------------------------------------
		public static Image GetImageFromVideo(string videoPath, float seconds)
		{
			Image img = null;
			videoPath = videoPath.Replace('\\', '/');
			var prs = new ExternalProcess(MPlayerPath);
			var tmpFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(tmpFolder);
			var tmpFile = Path.Combine(tmpFolder, "00000001.jpg");

			try
			{
				prs.StartInfo.Arguments =
					string.Format("-nocache -nofontconfig -really-quiet -frames 1 -ss {0} -nosound -vo jpeg:outdir=\"\"\"{1}\"\"\" \"{2}\"",
					seconds, tmpFolder, videoPath);

				prs.StartProcess();
				prs.WaitForExit();
				prs.Close();

				// I'm hesitant to comment out this line, but because of SP-248, we'll see what happens.
				//ComponentFile.WaitForFileRelease(videoPath);

				if (File.Exists(tmpFile))
				{
					// I could use Image.FromFile, but that leaves
					// a lock on the file, for some reason.
					var stream = new FileStream(tmpFile, FileMode.Open);
					img = Image.FromStream(stream);
					stream.Close();
				}
			}
			finally
			{
				if (File.Exists(tmpFile))
					File.Delete(tmpFile);

				try { Directory.Delete(tmpFolder); }
				catch { }
			}

			return img;
		}
	}

	#endregion

	#region MPlayerOutputLogForm class
	/// ------------------------------------------------------------------------------------
	public class MPlayerOutputLogForm : Form
	{
		private readonly TextBox _textBox;

		/// --------------------------------------------------------------------------------
		public MPlayerOutputLogForm(string initialOutput)
		{
			_textBox = new TextBox();
			_textBox.ReadOnly = true;
			_textBox.BackColor = SystemColors.Window;
			_textBox.Multiline = true;
			_textBox.WordWrap = false;
			_textBox.ScrollBars = ScrollBars.Both;
			_textBox.Dock = DockStyle.Fill;
			_textBox.Font = SystemFonts.MessageBoxFont;
			_textBox.Text = initialOutput;

			var rc = Screen.PrimaryScreen.Bounds;
			Width = (rc.Width / 4);
			Height = (int)(rc.Height * 0.75);
			Location = new Point(rc.Right - Width, 0);
			StartPosition = FormStartPosition.Manual;
			Padding = new Padding(10);
			Text = @"Player Output Log";
			ShowIcon = false;
			ShowInTaskbar = false;
			MaximizeBox = false;
			MinimizeBox = false;
			Controls.Add(_textBox);
		}

		/// --------------------------------------------------------------------------------
		public void Clear()
		{
			Invoke((Action)(() => _textBox.Text = string.Empty));
		}

		/// --------------------------------------------------------------------------------
		public void UpdateLogDisplay(string output)
		{
			Invoke((Action)(() => _textBox.SelectionStart = _textBox.Text.Length));
			Invoke((Action<string>)(text => _textBox.SelectedText = text + Environment.NewLine), output);
		}

		/// --------------------------------------------------------------------------------
		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}
	}

	#endregion
}
