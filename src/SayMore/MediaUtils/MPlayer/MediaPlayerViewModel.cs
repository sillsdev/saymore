using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using L10NSharp;
using SayMore.Utilities;

// ReSharper disable once CheckNamespace
namespace SayMore.Media.MPlayer
{
	/// ----------------------------------------------------------------------------------------
	public class MediaPlayerViewModel
	{
		public Action<float> PlaybackPositionChanged;
		public Action PlaybackPaused;
		public Action PlaybackResumed;
		public Action VolumeChanged;

		public delegate void PlaybackEndedEventHandler(object sender, bool endedBecauseEOF);
		public event PlaybackEndedEventHandler PlaybackEnded;
		public event EventHandler PlaybackStarted;
		public event EventHandler MediaQueued;

		private readonly StringBuilder _mplayerStartInfo = new StringBuilder();

		private readonly object _lock = new object();
		private ExternalProcess _mplayerProcess;
		private StreamWriter _stdIn;
		private System.Threading.Timer _loopDelayTimer;
		
		private float _prevPosition;
		private MPlayerOutputLogForm _formMPlayerOutputLog;
		private ILogger _outputDebuggingWindow;


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
			lock (_lock)
			{
				_stdIn = stdIn;
				_stdIn.AutoFlush = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		public int VideoWindowHandle { get; set; }

		/// ------------------------------------------------------------------------------------
		public float CurrentPosition { get; private set; }

		/// ------------------------------------------------------------------------------------
		public float PlaybackStartPosition { get; set; }

		/// ------------------------------------------------------------------------------------
		public float PlaybackLength { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsFileLoaded => MediaFile != null;

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
		/// Gets or sets a value indicating whether playback loops after playback ends
		/// by coming to the end of the playback range. If playback is stopped (e.g. by the
		/// user clicking a stop playback button), then playback will not loop.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Loop { get; set; }

		#endregion

		/// ------------------------------------------------------------------------------------
		public void ShutdownMPlayerProcess()
		{
			lock (_lock)
			{
				try
				{
					if (_mplayerProcess != null && !_mplayerProcess.HasExited)
					{
						_mplayerProcess.KillProcess();
						_mplayerProcess.Dispose();
						_mplayerProcess = null;
						_stdIn = null;
					}

					CancelLoopDelayTimer();
				}
				catch
				{
				}
			}
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
			ShutdownMPlayerProcess();

			// On Windows, We can't get unicode over the command-line barrier, so
			// instead create 8.3 filename, which, happily, will have no non-english characters
			// for any part of the path.
			filename = FileSystemUtils.GetShortName(filename,
                () => LocalizationManager.GetString("CommonToMultipleViews.MediaPlayer.LoadFailure",
                    "Media player - failure to load file."));

			if (string.IsNullOrEmpty(filename))
			{
				throw new ArgumentException(LocalizationManager.GetString(
					"CommonToMultipleViews.MediaPlayer.MediaFileNotSpecifiedMsg",
					"Media player file name has not been specified."));
			}

			if (!File.Exists(filename))
			{
				throw new FileNotFoundException(String.Format(LocalizationManager.GetString(
					"CommonToMultipleViews.MediaPlayer.MediaFileNotFoundMsg",
					"Media file not found: {0}"), filename), filename);
			}

			MediaInfo = MediaFileInfo.GetInfo(filename, out var error);
			if (MediaInfo == null)
			{
				throw new FileFormatException(String.Format(LocalizationManager.GetString(
					"CommonToMultipleViews.MediaPlayer.InvalidMediaFile",
					"File does not appear to be a valid media file: {0}"), filename), error);
			}

			MediaFile = filename.Replace('\\', '/');
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
			lock (_lock)
			{
				ShutdownMPlayerProcess();
				HasPlaybackStarted = false;
			}

			CurrentPosition = PlaybackStartPosition;

			if (MediaQueued != null && GetTotalMediaDuration() > 0f && (!MediaInfo.IsVideo ||
				(MediaInfo.Video.Width > 0 && MediaInfo.Video.Height > 0)))
			{
				MediaQueued(this, EventArgs.Empty);
			}

			_queueingInProgress = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the total duration in seconds. For audio files, the duration is always the
		/// duration of the audio. For video files, it is the total duration, counting from the
		/// start of the first track to the end of the last track (audio and video tracks are
		/// not guaranteed to start and end simultaneously).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float GetTotalMediaDuration() => (float)(MediaInfo?.Duration.TotalSeconds ?? 0f);

		/// ------------------------------------------------------------------------------------
		private float GetPlayBackEndPosition()
		{
			var mediaDuration = GetTotalMediaDuration();

			var endPosition = PlaybackLength.Equals(0f) ?
				mediaDuration : PlaybackStartPosition + PlaybackLength;

			return Math.Min(mediaDuration, endPosition);
		}

		/// ------------------------------------------------------------------------------------
		public Image GetVideoThumbnail()
		{
			if (MediaInfo == null || !MediaInfo.IsVideo)
				return null;

			return (PlaybackStartPosition.Equals(0f) ? MediaInfo.FullSizedThumbnail :
				MPlayerHelper.GetImageFromVideo(MediaInfo.MediaFilePath, PlaybackStartPosition));
		}

		#region Button state properties
		/// ------------------------------------------------------------------------------------
		public bool IsPlayButtonVisible => (IsPaused || !HasPlaybackStarted);

		/// ------------------------------------------------------------------------------------
		public bool IsPauseButtonVisible => (!IsPaused && HasPlaybackStarted);

		/// ------------------------------------------------------------------------------------
		public bool IsStopEnabled => HasPlaybackStarted;

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
		public MediaFileInfo MediaInfo { get; private set; }

		/// ------------------------------------------------------------------------------------
		private ILogger DebugOutput
		{
			set
			{
				if (_outputDebuggingWindow == null)
				{
					_outputDebuggingWindow = value;
					if (_outputDebuggingWindow != null)
						_outputDebuggingWindow.Disposed += delegate { _outputDebuggingWindow = null; };
				}
			}
		}
		#endregion

		#region Play/Pause/Seek methods
		/// ------------------------------------------------------------------------------------
		public void ConfigurePlayingForTest()
		{
			lock (_lock)
				HasPlaybackStarted = true;
		}

		/// ------------------------------------------------------------------------------------
		public void Play()
		{
			Play(false);
		}

		/// ------------------------------------------------------------------------------------
		public void Play(bool resampleToMono)
		{
			lock (_lock)
			{
				if (!HasPlaybackStarted)
					StartPlayback(resampleToMono);
				else if (IsPaused)
					_stdIn.WriteLine("pause ");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Starts playback. Caller is responsible for obtaining the lock.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void StartPlayback(bool resampleToMono)
		{
			DebugOutput = Application.OpenForms.OfType<ILogger>().FirstOrDefault();

			CancelLoopDelayTimer();

			if (_mplayerProcess != null)
				ShutdownMPlayerProcess();

			var videoWindowHandle = 0;
			int bitsPerSample = 0;

			// If the file is a video file and we don't have a window in which to play
			// the video, then set the handle to -1 to indicate to the helper that we
			// only want to play the audio from the video file.
			if (MediaInfo != null)
			{
				if (MediaInfo.IsVideo)
					videoWindowHandle = (VideoWindowHandle > 0 ? VideoWindowHandle : -1);
				bitsPerSample = MediaInfo.BitsPerSample;
			}

			var args = MPlayerHelper.GetPlaybackArguments(PlaybackStartPosition,
				PlaybackLength, Volume, Speed, resampleToMono, videoWindowHandle, bitsPerSample);

			_mplayerProcess = MPlayerHelper.StartProcessToMonitor(args,
				HandleDataReceived, HandleDataReceived);

			if (_outputDebuggingWindow != null)
			{
				_mplayerStartInfo.Length = 0;
				_mplayerStartInfo.AppendLine("*** COMMAND LINE:");
				_mplayerStartInfo.Append(_mplayerProcess.StartInfo.FileName);
				_mplayerStartInfo.Append(" ");
				_mplayerStartInfo.AppendLine(_mplayerProcess.StartInfo.Arguments);
				_mplayerStartInfo.Append("*** MEDIA FILE: ");
				_mplayerStartInfo.AppendLine(MediaFile);
				_mplayerStartInfo.AppendLine();
				_outputDebuggingWindow.AddText(_mplayerStartInfo.ToString());
			}

			_mplayerProcess.FileOpenedByProcess = MediaFile;
			_stdIn = _mplayerProcess.StandardInput;
			_stdIn.WriteLine("loadfile \"{0}\" ", MediaFile);

			HasPlaybackStarted = true;

			PlaybackStarted?.Invoke(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Disposes and discards the loop delay timer. Caller is responsible for obtaining the
		/// lock.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CancelLoopDelayTimer()
		{
			if (_loopDelayTimer != null)
			{
				_loopDelayTimer.Dispose();
				_loopDelayTimer = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Pauses the player when it's playing and resumes play when it's paused.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Pause()
		{
			lock (_lock)
				_stdIn?.WriteLine("pause ");
		}

		/// ------------------------------------------------------------------------------------
		public void Seek(float position)
		{
			lock (_lock)
			{
				if (_stdIn != null)
				{
					_stdIn.WriteLine(IsPaused ? $"pause {Environment.NewLine}seek {position} 2" :
						$"seek {position} 2");
				}
				else
				{
					PlaybackStartPosition = position;
					Play();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			Stop(false);
		}

		/// ------------------------------------------------------------------------------------
		public void Stop(bool waitForMediaFileToBeReleased)
		{
			lock (_lock)
			{
				if (HasPlaybackStarted)
				{
					_stdIn.WriteLine("stop ");
					OnMediaQueued();

					PlaybackEnded?.Invoke(this, false);

					if (waitForMediaFileToBeReleased && MediaFile != null)
						FileSystemUtils.WaitForFileRelease(MediaFile);
				}

				CancelLoopDelayTimer();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetSpeed(int speed)
		{
			Speed = speed;

			lock (_lock)
				_stdIn?.WriteLine("speed_set {0} ", Speed / 100d);
		}

		/// ------------------------------------------------------------------------------------
		public void SetVolume(float volume)
		{
			if (volume >= 0f && volume <= 100f)
			{
				Volume = volume;
				if (!IsPaused)
					SendVolumeMessageToPlayer();

				VolumeChanged?.Invoke();
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
			lock (_lock)
				_stdIn?.WriteLine("volume {0} 1 ", IsVolumeMuted ? 0 : Volume);
		}

		#endregion

		#region Time display methods

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines two results of MakeTimeString into a display of the current time
		/// position next to the total time (which is the duration of the media file).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetTimeDisplay(float position)
		{
			return GetTimeDisplay(position, GetPlayBackEndPosition());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines two results of MakeTimeString into a display of the time represented
		/// by position next to the specified length.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetTimeDisplay(float position, float endPosition)
		{
			if (position > endPosition)
				position = endPosition;

			return string.Format(LocalizationManager.GetString(
				"CommonToMultipleViews.MediaPlayer.TimeCurrentOfTotalDisplayFormat", "{0} / {1}"),
				MakeTimeString(position), MakeTimeString(endPosition));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines two results of MakeTimeString into a display of the time represented
		/// by position next to the specified length.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetRangeTimeDisplay(float startPosition, float endPosition)
		{
			//if (endPosition <= 0)
			//    endPosition = GetPlayBackEndPosition();

			if (startPosition > endPosition)
				startPosition = endPosition;

			return string.Format(LocalizationManager.GetString(
				"CommonToMultipleViews.MediaPlayer.TimeRangeDisplayFormat", "{0} - {1}"),
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

			if (str.IndexOf('.') >= 0)
				str = str.TrimEnd('0');

			if (str.EndsWith("."))
				str += "0";

			if (!str.Contains("."))
				str += ".0";

			return str;
		}

		#endregion

		#region Methods for processing output from MPlayer
		/// ------------------------------------------------------------------------------------
		private void HandleDataReceived(object sender, DataReceivedEventArgs e)
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

			_outputDebuggingWindow?.AddText(data);

			if (data.StartsWith("A: "))
			{
				ProcessPlaybackPositionMessage(data);
			}
			else if (data.StartsWith("EOF code:") || data.StartsWith("Failed to open"))
			{
				EnsurePlaybackUntilEndOfMedia();

				lock (_lock)
				{
					// If we get the lock and the process has already been set to null, then
					// playback was stopped externally, and it won't make sense to do any of this.
					if (_mplayerProcess == null)
						return;
					OnMediaQueued();

					PlaybackEnded?.Invoke(this, true);

					if (Loop)
						StartLoopTimer();
				}
			}
			else if (data.StartsWith("ID_PAUSED"))
			{
				IsPaused = true;
				PlaybackPaused?.Invoke();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void EnsurePlaybackUntilEndOfMedia()
		{
			// This method is called when mplayer reports that it has come to the end of
			// playing a media file. For some reason, mplayer sometimes reports that it's
			// at the end when there's actually a little bit remaining (e.g. by about one
			// second). mplayer won't stop playing until the real end, however, so in
			// order to avoid passing on the false report that we're at EOF (which will
			// cause our code to prematurely terminate playback) this method will figure
			// out how long to delay (and then delay) before passing on an accurate report
			// that we've reached EOF.

			var secondsLeftToPlay = GetPlayBackEndPosition() - _prevPosition;
			if (secondsLeftToPlay <= 0)
				return;

			var finishedTime = DateTime.Now.AddSeconds(secondsLeftToPlay);
			while (finishedTime >= DateTime.Now)
			{
				var newPosition = _prevPosition + (finishedTime - DateTime.Now).TotalSeconds;
				ProcessPlaybackPositionMessage((float)newPosition);
				lock (_lock)
				{
					if (_mplayerProcess == null)
						break;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void StartLoopTimer()
		{
			_loopDelayTimer = new System.Threading.Timer(a => Play(), null, LoopDelay, Timeout.Infinite);
		}

		/// ------------------------------------------------------------------------------------
		private void ProcessPlaybackPositionMessage(string data)
		{
			if (IsPaused)
			{
				IsPaused = false;
				PlaybackResumed?.Invoke();
			}

			try
			{
				ProcessPlaybackPositionMessage(float.Parse(
					data.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1],
						CultureInfo.InvariantCulture.NumberFormat));
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		private void ProcessPlaybackPositionMessage(float newCurrentTime)
		{
			if (IsPaused)
			{
				IsPaused = false;
				PlaybackResumed?.Invoke();
			}

			CurrentPosition = Math.Max(0, newCurrentTime);

			if (CurrentPosition.Equals(0) || CurrentPosition.Equals(_prevPosition))
				return;

			_prevPosition = CurrentPosition;

			var endPosition = GetPlayBackEndPosition();

			if (CurrentPosition > endPosition)
				CurrentPosition = endPosition;

			PlaybackPositionChanged?.Invoke(CurrentPosition);
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
