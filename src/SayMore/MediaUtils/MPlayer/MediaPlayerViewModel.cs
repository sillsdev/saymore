using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Palaso.Reporting;
using Localization;
using SayMore.Utilities;

namespace SayMore.Media.MPlayer
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

		private readonly StringBuilder _mplayerStartInfo = new StringBuilder();
		private ExternalProcess _mplayerProcess;
		private StreamWriter _stdIn;
		private float _prevPostion;
		private MPlayerOutputLogForm _formMPlayerOutputLog;
		private MPlayerDebuggingOutputWindow _outputDebuggingWindow;

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
				if (_mplayerProcess != null && !_mplayerProcess.HasExited)
				{
					_mplayerProcess.KillProcess();
					_mplayerProcess.Dispose();
					_mplayerProcess = null;
					_stdIn = null;
				}

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
			ShutdownMPlayerProcess();

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

			MediaInfo = MediaFileInfo.GetInfo(filename);
			if (MediaInfo == null)
			{
				throw new FileFormatException(String.Format(LocalizationManager.GetString(
					"CommonToMultipleViews.MediaPlayer.InvalidMediaFile",
					"File does not appear to be a valid media file: {0}"), filename));
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
			ShutdownMPlayerProcess();
			HasPlaybackStarted = false;
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
		/// not guaranteed to start and end simultaneouly).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float GetTotalMediaDuration()
		{
			return (MediaInfo == null ? 0f : (float)MediaInfo.Duration.TotalSeconds);
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
				MPlayerHelper.GetImageFromVideo(MediaInfo.MediaFilePath, PlaybackStartPosition));
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
		public MediaFileInfo MediaInfo { get; private set; }

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
			Play(false);
		}

		/// ------------------------------------------------------------------------------------
		public void Play(bool resampleToMono)
		{
			if (!HasPlaybackStarted)
				StartPlayback(resampleToMono);
			else if (IsPaused)
				_stdIn.WriteLine("pause ");
		}

		/// ------------------------------------------------------------------------------------
		private void StartPlayback(bool resampleToMono)
		{
			_outputDebuggingWindow = Application.OpenForms.Cast<Form>()
				.FirstOrDefault(f => f is MPlayerDebuggingOutputWindow) as MPlayerDebuggingOutputWindow;

			if (_loopDelayTimer != null)
			{
				_loopDelayTimer.Dispose();
				_loopDelayTimer = null;
			}

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
				HandleErrorDataReceived, HandleErrorDataReceived);

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
			_stdIn.WriteLine(string.Format("loadfile \"{0}\" ", MediaFile));

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
			Stop(false);
		}

		/// ------------------------------------------------------------------------------------
		public void Stop(bool waitForMediaFileToBeReleased)
		{
			if (HasPlaybackStarted)
			{
				_stdIn.WriteLine("stop ");
				OnMediaQueued();

				if (PlaybackEnded != null)
					PlaybackEnded(this, false);

				if (waitForMediaFileToBeReleased && MediaFile != null)
					FileSystemUtils.WaitForFileRelease(MediaFile);
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

			if (_outputDebuggingWindow != null)
				_outputDebuggingWindow.AddText(data);

			if (data.StartsWith("A: "))
			{
				ProcessPlaybackPositionMessage(data);
			}
			else if (data.StartsWith("EOF code:"))
			{
				EnsurePlaybackUntilEndOfMedia();
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

			var secondsLeftToPlay = (GetPlayBackEndPosition() - _prevPostion);
			if (secondsLeftToPlay <= 0)
				return;

			var finishedTime = DateTime.Now.AddSeconds(secondsLeftToPlay);
			while (finishedTime >= DateTime.Now)
			{
				var newPosition = _prevPostion + (finishedTime - DateTime.Now).TotalSeconds;
				ProcessPlaybackPositionMessage((float)newPosition);
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
				if (PlaybackResumed != null)
					PlaybackResumed();
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
				if (PlaybackResumed != null)
					PlaybackResumed();
			}

			CurrentPosition = Math.Max(0, newCurrentTime);

			if (CurrentPosition.Equals(0) || CurrentPosition.Equals(_prevPostion))
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
