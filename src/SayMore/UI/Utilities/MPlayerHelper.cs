// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: MPlayerRunner.cs
// Responsibility: Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SayMore.UI.Utilities
{
	#region MPlayerHelper class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public static class MPlayerHelper
	{
		private static string s_mPlayerPath = DefaultMPlayerPath;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an assumed path for mplayer.exe. It's assumed it's in a folder called
		/// 'mplayer' that is a subfolder of the current assembly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string DefaultMPlayerPath
		{
			get
			{
				var path = Path.Combine(GetPathToThisAssembly(), "mplayer");
				return Path.Combine(path, "mplayer.exe");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string MPlayerPath
		{
			get { return (File.Exists(s_mPlayerPath) ? s_mPlayerPath : null); }
			set
			{
				if (File.Exists(value))
					s_mPlayerPath = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Process StartProcessToMonitor(Int32 hwndVideo,
			DataReceivedEventHandler outputDataHandler, DataReceivedEventHandler errorDataHandler)
		{
			if (hwndVideo == 0)
				throw new ArgumentException("No window handle specified.");

			if (outputDataHandler == null)
				throw new ArgumentNullException("outputDataHandler");

			if (errorDataHandler == null)
				throw new ArgumentNullException("errorDataHandler");

			var prs = new Process();
			prs.StartInfo.CreateNoWindow = true;
			prs.StartInfo.UseShellExecute = false;
			prs.StartInfo.RedirectStandardError = true;
			prs.StartInfo.RedirectStandardInput = true;
			prs.StartInfo.RedirectStandardOutput = true;
			prs.StartInfo.FileName = MPlayerPath;
			prs.OutputDataReceived += outputDataHandler;
			prs.ErrorDataReceived += errorDataHandler;
			prs.StartInfo.Arguments = GetMPlayerCommandLineForPlayback(hwndVideo);

			if (!prs.Start())
			{
				// REVIEW: Revise to do something a little less drastic.
				prs = null;
				throw new ApplicationException("Unable to start mplayer.");
			}

			prs.StandardInput.AutoFlush = true;
			prs.BeginOutputReadLine();
			prs.BeginErrorReadLine();

			return prs;
		}

		/// ------------------------------------------------------------------------------------
		private static string GetMPlayerCommandLineForPlayback(int hwndVideo)
		{
			// If we find an MPlayer config file in the same path as this assembly, then
			// use that for the settings instead of our default set.
			var mplayerConfigPath = Path.Combine(GetPathToThisAssembly(), "MPlayerSettings.conf");

			string cmdLine;

			if (File.Exists(mplayerConfigPath))
				cmdLine = string.Format("-include {0} -wid {1} ", mplayerConfigPath, hwndVideo);
			else
			{
				cmdLine = string.Format("-slave -noquiet -idle " +
					"-msglevel identify=9:global=9 -nofontconfig -autosync 100 -priority abovenormal " +
					" −osdlevel 0 -af volnorm=2 -volume 0 -fixed-vo -wid {0} ", hwndVideo);
#if !MONO
				cmdLine += " -vo gl";
#endif
			}

			return cmdLine;
		}

		/// ------------------------------------------------------------------------------------
		private static string GetPathToThisAssembly()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var mplayerConfigPath = assembly.CodeBase.Replace("file:", string.Empty).TrimStart('/');
			return Path.GetDirectoryName(mplayerConfigPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Extracts the audio stream from the specified video file and writes it to a wave
		/// (i.e. raw PCM) format file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ExtractAudioToWave(string videoPath, string audioOutPath)
		{
			var prs = new Process();
			prs.StartInfo.CreateNoWindow = true;
			prs.StartInfo.UseShellExecute = false;
			prs.StartInfo.RedirectStandardOutput = true;
			prs.StartInfo.FileName = MPlayerPath;

			videoPath = videoPath.Replace('\\', '/');
			//audioOutPath = audioOutPath.Replace('\\', '/');

			prs.StartInfo.Arguments =
				string.Format("\"{0}\" -nofontconfig -vo null -vc null -ao pcm:fast:file=%{1}%\"{2}\"",
				videoPath, audioOutPath.Length, audioOutPath);

			prs.Start();
		}
	}

	#endregion

	#region MediaFileInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class that, using MPlayer, gets the information about a specific audio or video file.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class MediaFileInfo
	{
		private int _videoResolutionX;
		private int _videoResolutionY;

		public string MediaFilePath { get; private set; }
		public long LengthInBytes { get; private set; }
		public TimeSpan Duration { get; private set; }
		public float FramesPerSecond { get; private set; }
		public int Channels { get; private set; }
		public int SamplesPerSecond { get; private set; }
		public int AudioBitRate { get; private set; }
		public int VideoBitRate { get; private set; }
		public string AudioCodec { get; private set; }

		/// ------------------------------------------------------------------------------------
		public string Resolution
		{
			get { return string.Format("{0}x{1}", _videoResolutionX, _videoResolutionY); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the bit depth for PCM audio. Other audio types don't have a bit depth.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BitDepth
		{
			get
			{
				if (AudioCodec.ToLower() != "pcm")
					return 0;

				return AudioBitRate / (SamplesPerSecond * Channels);
			}
		}

		/// ------------------------------------------------------------------------------------
		public MediaFileInfo(string mediaFilePath)
		{
			MediaFilePath = mediaFilePath;
			LengthInBytes = new FileInfo(mediaFilePath).Length;
			LoadMPlayer(mediaFilePath);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadMPlayer(string path)
		{
			var prs = new Process();
			prs.StartInfo.CreateNoWindow = true;
			prs.StartInfo.UseShellExecute = false;
			prs.StartInfo.RedirectStandardOutput = true;
			prs.StartInfo.FileName = MPlayerHelper.MPlayerPath;

			path = path.Replace('\\', '/');
			prs.StartInfo.Arguments = string.Format("-msglevel all=6 -identify " +
				"-nofontconfig -frames 0 -ao null -vc null -vo null \"{0}\"", path);

			if (prs.Start())
			{
				prs.WaitForExit(1500);
				string line;
				while ((line = prs.StandardOutput.ReadLine()) != null)
					ParseMPlayerOutputLine(line);

				prs.Close();
			}
			else
			{
				prs = null;
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem("Gathering audio/video " +
					"statistics failed. Please verify that MPlayer is installed in the folder '{0}'.",
					Path.GetDirectoryName(path));
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ParseMPlayerOutputLine(string data)
		{
			if (data == null)
				return;

			if (data.StartsWith("ID_VIDEO_FPS="))
			{
				FramesPerSecond = (float)Math.Round(
					double.Parse(data.Substring(13)), 3, MidpointRounding.AwayFromZero);
			}
			else if (data.StartsWith("ID_LENGTH="))
			{
				Duration = TimeSpan.FromSeconds(float.Parse(data.Substring(10)));
			}
			else if (data.StartsWith("ID_VIDEO_WIDTH="))
			{
				_videoResolutionX = int.Parse(data.Substring(15));
			}
			else if (data.StartsWith("ID_VIDEO_HEIGHT="))
			{
				_videoResolutionY = int.Parse(data.Substring(16));
			}
			else if (data.StartsWith("Channels: "))
			{
				Channels = int.Parse(data.Substring(10, 1));
			}
			//else if (data.StartsWith("ID_AUDIO_NCH="))
			//{
			//	  // This doesn't always report the correct number of channels.
			//    Channels = int.Parse(data.Substring(13));
			//}
			else if (data.StartsWith("ID_AUDIO_RATE="))
			{
				SamplesPerSecond = int.Parse(data.Substring(14));
			}
			else if (data.StartsWith("ID_AUDIO_BITRATE="))
			{
				AudioBitRate = int.Parse(data.Substring(17));
			}
			else if (data.StartsWith("ID_VIDEO_BITRATE="))
			{
				VideoBitRate = int.Parse(data.Substring(17));
			}
			else if (data.StartsWith("ID_AUDIO_CODEC="))
			{
				AudioCodec = data.Substring(15);
			}
		}
	}

	#endregion

	#region MPlayerOutputLogForm class
	/// ------------------------------------------------------------------------------------
	public class MPlayerOutputLogForm : Form
	{
		private readonly TextBox _textBox;

		/// --------------------------------------------------------------------------------
		public MPlayerOutputLogForm(string outputLog)
		{
			_textBox = new TextBox();
			_textBox.ReadOnly = true;
			_textBox.BackColor = SystemColors.Window;
			_textBox.Multiline = true;
			_textBox.WordWrap = false;
			_textBox.ScrollBars = ScrollBars.Both;
			_textBox.Dock = DockStyle.Fill;
			_textBox.Font = SystemFonts.MessageBoxFont;

			var rc = Screen.PrimaryScreen.Bounds;
			Width = (rc.Width / 4);
			Height = (int)(rc.Height * 0.75);
			Location = new Point(rc.Right - Width, 0);
			StartPosition = FormStartPosition.Manual;
			Padding = new Padding(10);
			Text = "Player Output Log";
			ShowIcon = false;
			ShowInTaskbar = false;
			MaximizeBox = false;
			MinimizeBox = false;
			Controls.Add(_textBox);

			UpdateLogDisplay(outputLog);
		}

		/// --------------------------------------------------------------------------------
		public void UpdateLogDisplay(string outputLog)
		{
			_textBox.Text = outputLog;
			_textBox.SelectionStart = _textBox.Text.Length;
			_textBox.ScrollToCaret();
		}

		/// --------------------------------------------------------------------------------
		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}
	}

	#endregion
}
