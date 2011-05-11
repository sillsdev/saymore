using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SayMore.UI.Archiving;

namespace SayMore.UI.MediaPlayer
{
	public class MediaPlayerViewModel
	{
		public delegate void PlaybackPositionChangedHandler(object sender, float position);
		public event PlaybackPositionChangedHandler PlaybackPositionChanged;
		public event EventHandler PlaybackPaused;
		public event EventHandler PlaybackResumed;
		public event EventHandler PlaybackEnded;
		public event EventHandler PlaybackStarted;
		public event EventHandler MediaQueued;
		public event EventHandler VolumeChanged;

		private const string kFmtTimeDisplay = "{0} / {1}";
		private const string kFmtTime = "{0}.{1:0}";

		private readonly StringBuilder _mplayerStartInfo = new StringBuilder();
		private MPlayerProcess _mplayer;
		private StreamWriter _stdIn;
		private float _prevPostion;
		private float _playbackStartPosition;
		private string _filename;
		private MPlayerOutputLogForm _formMPlayerOutputLog;
		private Int32 _hwndVideo;
		private DataReceivedEventHandler _outputDataHandler;
		private DataReceivedEventHandler _errorDataHandler;

		#region Construction and initialization
		/// ------------------------------------------------------------------------------------
		public MediaPlayerViewModel()
		{
			Volume = 25f;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Call this only from tests to initialize the standard input stream.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetStdInForTest(StreamWriter stdIn)
		{
			_stdIn = stdIn;
			_stdIn.AutoFlush = true;
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize(Int32 hwndVideo, DataReceivedEventHandler outputDataHandler,
			DataReceivedEventHandler errorDataHandler)
		{
			_hwndVideo = hwndVideo;
			_outputDataHandler = outputDataHandler;
			_errorDataHandler = errorDataHandler;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public void ShutdownMPlayerProcess()
		{
			if (_mplayer != null && !_mplayer.HasExited)
			{
				_mplayer.KillAndWaitForFileRelease();
				_mplayer.Dispose();
				_mplayer = null;
				_stdIn = null;
			}

			if (_formMPlayerOutputLog != null)
			{
				_formMPlayerOutputLog.Close();
				_formMPlayerOutputLog = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void LoadFile(string filename)
		{
			// REVIEW: Should something be reported to user if filename is not valid?
			if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
				return;

			_filename = filename.Replace('\\', '/');
			MediaInfo = new MPlayerMediaInfo(filename);
			OnMediaQueued();
		}

		/// ------------------------------------------------------------------------------------
		private void OnMediaQueued()
		{
			ShutdownMPlayerProcess();
			HasPlaybackStarted = false;
			_playbackStartPosition = 0;

			if (MediaQueued != null && GetTotalMediaDuration() > 0f && (!MediaInfo.IsVideo ||
				(MediaInfo.PictureSize.Width > 0 && MediaInfo.PictureSize.Height > 0)))
			{
				MediaQueued(this, EventArgs.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the duration plus the start time. For audio files, the start time is zero.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float GetTotalMediaDuration()
		{
			return (MediaInfo == null ? 0f : MediaInfo.Duration + MediaInfo.StartTime);
		}

		#region Button state properties
		/// ------------------------------------------------------------------------------------
		public bool IsPlayButtonVisible
		{
			get { return (IsPaused || !HasPlaybackStarted); }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsPauseButtonVisible
		{
			get { return (!IsPaused && HasPlaybackStarted); }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsStopEnabled
		{
			get { return HasPlaybackStarted; }
		}

		#endregion

		#region Misc. Properties
		/// ------------------------------------------------------------------------------------
		public bool HasPlaybackStarted { get; private set; }

		/// ------------------------------------------------------------------------------------
		public bool IsPaused { get; private set; }

		/// ------------------------------------------------------------------------------------
		public float Volume { get; private set; }

		/// ------------------------------------------------------------------------------------
		public bool IsVolumeMuted { get; private set; }

		/// ------------------------------------------------------------------------------------
		public MPlayerMediaInfo MediaInfo { get; private set; }

		#endregion

		#region Play/Pause/Seek methods
		/// ------------------------------------------------------------------------------------
		public void ConfigurePlayingForTest()
		{
			HasPlaybackStarted = true;
		}

		/// ------------------------------------------------------------------------------------
		public void Play()
		{
			if (!HasPlaybackStarted)
				StartPlayback();
			else if (IsPaused)
				_stdIn.WriteLine("pause ");
		}

		/// ------------------------------------------------------------------------------------
		private void StartPlayback()
		{
			if (_hwndVideo == 0)
				throw new Exception("Media player needs a window handle.");

			if (_outputDataHandler == null)
				throw new Exception("Media player needs am output data handler.");

			if (_errorDataHandler == null)
				throw new Exception("Media player needs am error data handler.");

			if (_mplayer != null)
				ShutdownMPlayerProcess();

			_mplayer = MPlayerHelper.StartProcessToMonitor(_playbackStartPosition,
				Volume, _hwndVideo, _outputDataHandler, _errorDataHandler);

			_mplayerStartInfo.AppendLine("Command Line:");
			_mplayerStartInfo.Append(_mplayer.StartInfo.FileName);
			_mplayerStartInfo.Append(" ");
			_mplayerStartInfo.AppendLine(_mplayer.StartInfo.Arguments);
			_mplayerStartInfo.AppendLine();

			_mplayer.MediaFileName = _filename;
			_stdIn = _mplayer.StandardInput;
			_stdIn.WriteLine(string.Format("loadfile \"{0}\" ", _filename));
			HasPlaybackStarted = true;

			if (PlaybackStarted != null)
				PlaybackStarted(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Pauses the player when it's playing and resumes play when it's paused.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Pause()
		{
			if (_stdIn != null)
				_stdIn.WriteLine("pause ");
		}

		/// ------------------------------------------------------------------------------------
		public void Seek(float position)
		{
			if (_stdIn != null)
			{
				_stdIn.WriteLine(IsPaused ?
					string.Format("pause {0}seek {1} 2", Environment.NewLine, position) :
					string.Format("seek {0} 2", position));
			}
			else
			{
				_playbackStartPosition = position;
				Play();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			if (HasPlaybackStarted)
			{
				_stdIn.WriteLine("stop ");
				OnMediaQueued();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetVolume(float volume)
		{
			if (volume >= 0f && volume <= 100f)
			{
				Volume = volume;
				if (!IsPaused)
					SendVolumeMessageToPlayer();

				if (VolumeChanged != null)
					VolumeChanged(this, EventArgs.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void ToggleVolumeMute()
		{
			IsVolumeMuted = !IsVolumeMuted;
			if (!IsPaused)
				SendVolumeMessageToPlayer();
		}

		/// ------------------------------------------------------------------------------------
		private void SendVolumeMessageToPlayer()
		{
			if (_stdIn != null)
				_stdIn.WriteLine(string.Format("volume {0} 1 ", IsVolumeMuted ? 0 : Volume));
		}

		#endregion

		#region Time display methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines two results of MakeTimeString into a display of a the time represented
		/// by position next to the total time (which is the duration of the media file).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetTimeDisplay(float position)
		{
			var totalDuration = GetTotalMediaDuration();

			if (position > totalDuration)
				position = totalDuration;

			return string.Format(kFmtTimeDisplay, MakeTimeString(position),
				MakeTimeString(totalDuration));
		}

		/// ------------------------------------------------------------------------------------
		public static string MakeTimeString(float position)
		{
			int seconds = (int)Math.Floor(position);
			int tenths = (int)Math.Round(((position - seconds) * 10));
			var span = TimeSpan.FromSeconds(seconds);

			var str = span.ToString();
			while (str.StartsWith("00:"))
				str = str.Substring(3);

			return string.Format(kFmtTime, str, tenths);
		}

		#endregion

		#region Methods for processing output from MPlayer
		/// ------------------------------------------------------------------------------------
		public void HandlePlayerOutput(string data)
		{
			if (_formMPlayerOutputLog != null)
				_formMPlayerOutputLog.UpdateLogDisplay(data);

			if (data.StartsWith("A: "))
			{
				ProcessPlaybackPositionMessage(data);
			}
			else if (data.StartsWith("EOF code:"))
			{
				OnMediaQueued();
				IsPaused = true;

				if (PlaybackEnded != null)
					PlaybackEnded(this, EventArgs.Empty);
			}
			else if (data.StartsWith("ID_PAUSED"))
			{
				IsPaused = true;
				if (PlaybackPaused != null)
					PlaybackPaused(this, EventArgs.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ProcessPlaybackPositionMessage(string data)
		{
			if (IsPaused)
			{
				IsPaused = false;
				if (PlaybackResumed != null)
					PlaybackResumed(this, EventArgs.Empty);
			}

			var position = float.Parse(data.Substring(3, 5));
			if (position < 0f || position == _prevPostion)
				return;

			_prevPostion = position;

			var totalDuration = GetTotalMediaDuration();
			if (position > totalDuration)
				position = totalDuration;

			if (PlaybackPositionChanged != null)
				PlaybackPositionChanged(this, position);
		}

		#endregion

		#region Method and class for showing log of output from MPlayer
		/// ------------------------------------------------------------------------------------
		public void ShowPlayerOutputLog(bool show)
		{
			if (show)
			{
				if (_formMPlayerOutputLog != null)
					_formMPlayerOutputLog.UpdateLogDisplay(_mplayerStartInfo.ToString());
				else
				{
					_formMPlayerOutputLog = new MPlayerOutputLogForm(_mplayerStartInfo.ToString());
					_formMPlayerOutputLog.FormClosing += HandleOutputLogFormClosing;
					_formMPlayerOutputLog.Show();
				}
			}
			else
			{
				if (_formMPlayerOutputLog != null)
				{
					_formMPlayerOutputLog.Close();
					_formMPlayerOutputLog = null;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleOutputLogFormClosing(object sender, FormClosingEventArgs e)
		{
			_formMPlayerOutputLog.FormClosing -= HandleOutputLogFormClosing;
			_formMPlayerOutputLog = null;
		}

		#endregion
	}
}
