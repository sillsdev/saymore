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
		/// Extracts the audio stream from the specified video file and writes it to a wav
		/// file (i.e. raw PCM).
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
