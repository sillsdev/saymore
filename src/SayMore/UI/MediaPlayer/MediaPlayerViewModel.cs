using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Palaso.Reporting;

namespace SayMore.UI.MediaPlayer
{
	/// ----------------------------------------------------------------------------------------
	public class MediaPlayerViewModel
	{
		public Action<float> PlaybackPositionChanged;
		public Action PlaybackPaused;
		public Action PlaybackResumed;
		public Action VolumeChanged;

		public delegate void PlaybackEndedEventHandler(object sender, bool EndedBecauseEOF);
		public event PlaybackEndedEventHandler PlaybackEnded;
		public event EventHandler PlaybackStarted;
		public event EventHandler MediaQueued;

		private const string kFmtRangeTimeDisplay = "{0} - {1}";
		private const string kFmtTimeDisplay = "{0} / {1}";
		//private const string kFmtTime = "{0}.{1:0}";

		private readonly StringBuilder _mplayerStartInfo = new StringBuilder();
		private MPlayerProcess _mplayer;
		private StreamWriter _stdIn;
		private float _prevPostion;
		private MPlayerOutputLogForm _formMPlayerOutputLog;
		private System.Threading.Timer _loopDelayTimer;

		#region Construction and initialization
		/// ------------------------------------------------------------------------------------
		public MediaPlayerViewModel()
		{
			Volume = 25f;
			LoopDelay = 100;
			Speed = 100;
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
		public Int32 VideoWindowHandle { get; set; }

		/// ------------------------------------------------------------------------------------
		public float CurrentPosition { get; private set; }

		/// ------------------------------------------------------------------------------------
		public float PlaybackStartPosition { get; set; }

		/// ------------------------------------------------------------------------------------
		public float PlaybackLength { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsFileLoaded
		{
			get { return MediaFile != null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Use LoadFile to set this property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string MediaFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value in milliseconds indicating how much time between playback
		/// loops to wait.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int LoopDelay { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not playback loops after playback ends
		/// by coming to the end of the playback range. If playback is stopped (e.g. by the
		/// user clicking a stop playback button), then playback will not loop.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Loop { get; set; }

		#endregion

		/// ------------------------------------------------------------------------------------
		public void ShutdownMPlayerProcess()
		{
			try
			{
				if (_mplayer != null && !_mplayer.HasExited)
				{
					_mplayer.KillAndWaitForFileRelease();
					_mplayer.Dispose();
					_mplayer = null;
					_stdIn = null;
				}

				//if (_formMPlayerOutputLog != null)
				//{
				//    _formMPlayerOutputLog.Close();
				//    _formMPlayerOutputLog = null;
				//}

				if (_loopDelayTimer != null)
				{
					_loopDelayTimer.Dispose();
					_loopDelayTimer = null;
				}
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		public void LoadFile(string filename)
		{
			LoadFile(filename, 0f);
		}

		/// ------------------------------------------------------------------------------------
		public void LoadFile(string filename, float startPosition)
		{
			LoadFile(filename, 0f, 0f);
		}

		/// ------------------------------------------------------------------------------------
		public void LoadFile(string filename, float playbackStartPosition, float playbackLength)
		{
			if (string.IsNullOrEmpty(filename))
			{
				ErrorReport.NotifyUserOfProblem("Media player file name has not been specified.");
				return;
			}

			if (!File.Exists(filename))
			{
				ErrorReport.NotifyUserOfProblem("Media file '{0}' not found.", filename);
				return;
			}

			ShutdownMPlayerProcess();
			MediaFile = filename.Replace('\\', '/');
			MediaInfo = new MPlayerMediaInfo(filename);
			PlaybackStartPosition = playbackStartPosition;
			PlaybackLength = playbackLength;
			OnMediaQueued();

			if (_formMPlayerOutputLog != null)
				_formMPlayerOutputLog.Clear();
		}

		/// ------------------------------------------------------------------------------------
		private void OnMediaQueued()
		{
			_queueingInProgress = true;
			ShutdownMPlayerProcess();
			HasPlaybackStarted = false;
			CurrentPosition = PlaybackStartPosition;

			if (MediaQueued != null && GetTotalMediaDuration() > 0f && (!MediaInfo.IsVideo ||
				(MediaInfo.PictureSize.Width > 0 && MediaInfo.PictureSize.Height > 0)))
			{
				MediaQueued(this, EventArgs.Empty);
			}

			_queueingInProgress = false;
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

		/// ------------------------------------------------------------------------------------
		private float GetPlayBackEndPosition()
		{
			var mediaDuration = GetTotalMediaDuration();

			var endPosition = (PlaybackLength.Equals(0f) ?
				mediaDuration : PlaybackStartPosition + PlaybackLength);

			return Math.Min(mediaDuration, endPosition);
		}

		/// ------------------------------------------------------------------------------------
		public Image GetVideoThumbnail()
		{
			if (MediaInfo == null || !MediaInfo.IsVideo)
				return null;

			return (PlaybackStartPosition.Equals(0f) ? MediaInfo.FullSizedThumbnail :
				MPlayerHelper.GetImageFromVideo(MediaInfo.FileName, PlaybackStartPosition));
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
		public int Speed { get; set; }

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
			if (_loopDelayTimer != null)
			{
				_loopDelayTimer.Dispose();
				_loopDelayTimer = null;
			}

			if (MediaInfo.IsVideo && VideoWindowHandle == 0)
				throw new Exception("Media player needs a window handle.");

			if (_mplayer != null)
				ShutdownMPlayerProcess();

			var args = MPlayerHelper.GetPlaybackArguments(PlaybackStartPosition,
				PlaybackLength, Volume, Speed, MediaInfo.IsVideo ? VideoWindowHandle : 0);

			_mplayer = MPlayerHelper.StartProcessToMonitor(args, HandleErrorDataReceived, HandleErrorDataReceived);
			_mplayerStartInfo.AppendLine("Command Line:");
			_mplayerStartInfo.Append(_mplayer.StartInfo.FileName);
			_mplayerStartInfo.Append(" ");
			_mplayerStartInfo.AppendLine(_mplayer.StartInfo.Arguments);
			_mplayerStartInfo.AppendLine();

			_mplayer.MediaFileName = MediaFile;
			_stdIn = _mplayer.StandardInput;
			_stdIn.WriteLine(string.Format("loadfile \"{0}\" ", MediaFile));

			//if (Loop)
			//    _stdIn.WriteLine("loop 32000 ");

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
				PlaybackStartPosition = position;
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

				if (PlaybackEnded != null)
					PlaybackEnded(this, false);
			}

			if (_loopDelayTimer != null)
			{
				_loopDelayTimer.Dispose();
				_loopDelayTimer = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetSpeed(int speed)
		{
			Speed = speed;

			if (_stdIn != null)
				_stdIn.WriteLine(string.Format("speed_set {0} ", Speed / 100d));
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
					VolumeChanged();
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
		/// Combines two results of MakeTimeString into a display of a the current time
		/// position next to the total time (which is the duration of the media file).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetTimeDisplay()
		{
			return GetTimeDisplay(CurrentPosition);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines two results of MakeTimeString into a display of a the current time
		/// position next to the total time (which is the duration of the media file).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetTimeDisplay(float position)
		{
			return GetTimeDisplay(position, GetPlayBackEndPosition());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines two results of MakeTimeString into a display of a the time represented
		/// by position next to the specified length.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetTimeDisplay(float position, float endPosition)
		{
			if (endPosition <= 0)
				endPosition = GetPlayBackEndPosition();

			if (position > endPosition)
				position = endPosition;

			return string.Format(kFmtTimeDisplay,
				MakeTimeString(position), MakeTimeString(endPosition));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines two results of MakeTimeString into a display of a the time represented
		/// by position next to the specified length.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetRangeTimeDisplay(float startPosition, float endPosition)
		{
			//if (endPosition <= 0)
			//    endPosition = GetPlayBackEndPosition();

			if (startPosition > endPosition)
				startPosition = endPosition;

			return string.Format(kFmtRangeTimeDisplay,
				MakeTimeString(startPosition), MakeTimeString(endPosition));
		}

		/// ------------------------------------------------------------------------------------
		public static string MakeTimeString(float position)
		{
			if (position.Equals(0f))
				return "00.0";

			var roundedPosition = Math.Round(position, 1, MidpointRounding.AwayFromZero);

			var str = TimeSpan.FromSeconds(roundedPosition).ToString();

			while (str.StartsWith("00:"))
				str = str.Substring(3);

			str = str.TrimEnd('0');

			if (str.EndsWith("."))
				str = str + "0";

			if (!str.Contains("."))
				str = str + ".0";

			return str;
		}

		#endregion

		#region Methods for processing output from MPlayer
		/// ------------------------------------------------------------------------------------
		private void HandleErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (e.Data != null)
				HandlePlayerOutput(e.Data);
		}

		private bool _queueingInProgress;

		/// ------------------------------------------------------------------------------------
		public void HandlePlayerOutput(string data)
		{
			if (_queueingInProgress)
				return;

			if (_formMPlayerOutputLog != null)
				_formMPlayerOutputLog.UpdateLogDisplay(data);

			if (data.StartsWith("A: "))
			{
				ProcessPlaybackPositionMessage(data);
			}
			else if (data.StartsWith("EOF code:"))
			{
				OnMediaQueued();

				if (PlaybackEnded != null)
					PlaybackEnded(this, true);

				if (Loop)
					StartLoopTimer();
			}
			else if (data.StartsWith("ID_PAUSED"))
			{
				IsPaused = true;
				if (PlaybackPaused != null)
					PlaybackPaused();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void StartLoopTimer()
		{
			_loopDelayTimer = new System.Threading.Timer(a => Play(), null, LoopDelay,
				System.Threading.Timeout.Infinite);
		}

		/// ------------------------------------------------------------------------------------
		private void ProcessPlaybackPositionMessage(string data)
		{
			if (IsPaused)
			{
				IsPaused = false;
				if (PlaybackResumed != null)
					PlaybackResumed();
			}

			CurrentPosition = float.Parse(data.Substring(3, 5));

			if (CurrentPosition < 0f)
			{
				CurrentPosition = 0f;
				return;
			}

			if (CurrentPosition.Equals(_prevPostion))
				return;

			_prevPostion = CurrentPosition;

			var endPosition = GetPlayBackEndPosition();

			if (CurrentPosition > endPosition)
				CurrentPosition = endPosition;

			if (PlaybackPositionChanged != null)
				PlaybackPositionChanged(CurrentPosition);
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
