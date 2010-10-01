using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SayMore.UI.Utilities;

namespace SayMore.UI.MediaPlayer
{
	public class MediaPlayerViewModel
	{
		public delegate void PlaybackPositionChangedHandler(object sender, float position);
		public event PlaybackPositionChangedHandler PlaybackPositionChanged;
		public event EventHandler PlaybackPaused;
		public event EventHandler PlaybackResumed;
		//public event EventHandler PlaybackStarting;
		public event EventHandler PlaybackEnded;
		public event EventHandler MediaQueued;
		public event EventHandler VolumeChanged;

		private const string kFmtTimeDisplay = "{0} / {1}";
		private const string kFmtTime = "{0:00}:{1:00}.{2:0}";

		private readonly StringBuilder _outputLog = new StringBuilder();
		private Process _mplayer;
		private StreamWriter _stdIn;
		private float _prevPostion;
		private float _volume = 25f;
		private bool _paused;
		private bool _volumeMuted;
		private bool _playbackEnded;
		private string _filename;
		private MPlayerOutputLogForm _formMPlayerOutputLog;
		private Int32 _hwndVideo;
		private DataReceivedEventHandler _outputDataHandler;
		private DataReceivedEventHandler _errorDataHandler;

		public float MediaFileLength { get; private set; }
		public Size VideoPictureSize { get; private set; }
		public bool IsForVideo { get; private set; }

		#region Construction and initialization
		/// ------------------------------------------------------------------------------------
		public MediaPlayerViewModel()
		{
			VideoPictureSize = Size.Empty;
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize(Int32 hwndVideo, DataReceivedEventHandler outputDataHandler,
			DataReceivedEventHandler errorDataHandler)
		{
			_hwndVideo = hwndVideo;
			_outputDataHandler = outputDataHandler;
			_errorDataHandler = errorDataHandler;
			Reinitialize();
		}

		/// ------------------------------------------------------------------------------------
		public void Reinitialize()
		{
			if (_hwndVideo == 0 || _outputDataHandler == null || _errorDataHandler == null)
				throw new Exception("You must call Initialize before calling Reinitialize.");

			_outputLog.Length = 0;

			if (_mplayer != null)
				ShutdownMPlayerProcess();

			_mplayer = MPlayerHelper.StartProcessToMonitor(_hwndVideo,
				_outputDataHandler, _errorDataHandler);

			_outputLog.AppendLine("Command Line:");
			_outputLog.Append(_mplayer.StartInfo.FileName);
			_outputLog.Append(" ");
			_outputLog.AppendLine(_mplayer.StartInfo.Arguments);
			_outputLog.AppendLine();

			_stdIn = _mplayer.StandardInput;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public void ShutdownMPlayerProcess()
		{
			if (_mplayer != null && !_mplayer.HasExited)
			{
				Application.DoEvents();
				_mplayer.Kill();
				_mplayer.Dispose();
				_mplayer = null;
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
			LoadFile(filename, false);
		}

		/// ------------------------------------------------------------------------------------
		public void LoadFile(string filename, bool playImmediately)
		{
			_filename = filename.Replace('\\', '/');
			_stdIn.WriteLine(string.Format("loadfile \"{0}\" ", _filename));
			_stdIn.WriteLine("volume 0 ");

			if (playImmediately)
				SendVolumeMessageToPlayer();
			else
			{
				_paused = true;
				_stdIn.WriteLine("pause ");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a string containing all the output from the MPlayer process.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string OutputLog
		{
			get { return _outputLog.ToString(); }
		}

		#region Button state methods
		/// ------------------------------------------------------------------------------------
		public bool GetIsPlayButtonVisible()
		{
			return (_paused || _playbackEnded);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsPauseButtonVisible()
		{
			return (!_paused && !_playbackEnded);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsStopEnabled()
		{
			return GetIsPauseButtonVisible();
		}

		#endregion

		#region Misc. Properties
		/// ------------------------------------------------------------------------------------
		public bool IsPaused
		{
			get { return _paused; }
		}

		/// ------------------------------------------------------------------------------------
		public float Volume
		{
			get { return _volume; }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsVolumeMuted
		{
			get { return _volumeMuted; }
		}

		#endregion

		#region Play/Pause/Seek methods
		/// ------------------------------------------------------------------------------------
		public void Play()
		{
			if (_playbackEnded)
				LoadFile(_filename);

			if (_paused)
			{
				_stdIn.WriteLine("pause ");
				SendVolumeMessageToPlayer();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Pauses the player when it's playing and resumes play when it's paused.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Pause()
		{
			_stdIn.WriteLine("pause ");
		}

		/// ------------------------------------------------------------------------------------
		public void Seek(float position)
		{
			_stdIn.WriteLine(string.Format("seek {0} 2", position));
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			_stdIn.WriteLine("stop ");
			LoadFile(_filename);
		}

		/// ------------------------------------------------------------------------------------
		public void SetVolume(float volume)
		{
			if (volume >= 0f && volume <= 100f)
			{
				_volume = volume;
				if (!_paused)
					SendVolumeMessageToPlayer();

				if (VolumeChanged != null)
					VolumeChanged(this, EventArgs.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void ToggleVolumeMute()
		{
			_volumeMuted = !_volumeMuted;
			if (!_paused)
				SendVolumeMessageToPlayer();
		}

		/// ------------------------------------------------------------------------------------
		private void SendVolumeMessageToPlayer()
		{
			if (_stdIn != null)
				_stdIn.WriteLine(string.Format("volume {0} 1 ", _volumeMuted ? 0 : _volume));
		}

		#endregion

		#region Time display methods
		/// ------------------------------------------------------------------------------------
		public string GetTimeDisplay(float position)
		{
			if (position > MediaFileLength)
				position = MediaFileLength;

			return string.Format(kFmtTimeDisplay, MakeTimeString(position),
				MakeTimeString(MediaFileLength));
		}

		/// ------------------------------------------------------------------------------------
		private static string MakeTimeString(float position)
		{
			int seconds = (int)position;
			int minutes = seconds / 60;
			int tenths = (int)Math.Round(((position - seconds) * 10));
			seconds -= (minutes * 60);
			return string.Format(kFmtTime, minutes, seconds, tenths);
		}

		#endregion

		#region Methods for processing output from MPlayer
		/// ------------------------------------------------------------------------------------
		public void HandlePlayerOutput(string data)
		{
			// REVIEW: Maybe this should output to a temp. file since it doesn't
			// take a very long media file to generate gobs of output while playing.
			_outputLog.AppendLine(data);
			if (_formMPlayerOutputLog != null)
				_formMPlayerOutputLog.UpdateLogDisplay(_outputLog.ToString());

			if (data.StartsWith("A: "))
			{
				ProcessPlaybackPositionMessage(data);
			}
			else if (data.StartsWith("EOF code:"))
			{
				_playbackEnded = true;
				_paused = true;

				if (PlaybackEnded != null)
					PlaybackEnded(this, EventArgs.Empty);
			}
			else if (data.StartsWith("ID_LENGTH="))
			{
				MediaFileLength = float.Parse(data.Substring(10));
				CheckIfAllMediaQueuedInfoFound();
			}
			else if (data.StartsWith("ID_VIDEO_WIDTH="))
			{
				VideoPictureSize = new Size(int.Parse(data.Substring(15)), VideoPictureSize.Height);
				CheckIfAllMediaQueuedInfoFound();
			}
			else if (data.StartsWith("ID_VIDEO_HEIGHT="))
			{
				VideoPictureSize = new Size(VideoPictureSize.Width, int.Parse(data.Substring(16)));
				CheckIfAllMediaQueuedInfoFound();
			}
			else if (data.StartsWith("ID_PAUSED"))
			{
				_paused = true;
				if (PlaybackPaused != null)
					PlaybackPaused(this, EventArgs.Empty);
			}
			else if (data.StartsWith("ID_VIDEO_FORMAT"))
			{
				IsForVideo = true;
			}
			//else if (data.StartsWith("ANS_volume="))
			//{
			//    _volume = float.Parse(data.Substring(11));
			//}
		}

		/// ------------------------------------------------------------------------------------
		private void CheckIfAllMediaQueuedInfoFound()
		{
			if (MediaQueued == null || MediaFileLength == 0f || (IsForVideo &&
				(VideoPictureSize.Width == 0 || VideoPictureSize.Height == 0)))
			{
				return;
			}

			MediaQueued(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private void ProcessPlaybackPositionMessage(string data)
		{
			_playbackEnded = false;

			if (_paused)
			{
				_paused = false;
				if (PlaybackResumed != null)
					PlaybackResumed(this, EventArgs.Empty);
			}

			var position = float.Parse(data.Substring(3, 5));
			if (position < 0f || position == _prevPostion)
				return;

			_prevPostion = position;

			if (position > MediaFileLength)
				position = MediaFileLength;

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
					_formMPlayerOutputLog.UpdateLogDisplay(_outputLog.ToString());
				else
				{
					_formMPlayerOutputLog = new MPlayerOutputLogForm(_outputLog.ToString());
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
