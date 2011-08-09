using System;
using NAudio.Wave;

namespace SayMore.AudioUtils
{
	#region IAudioPlayer interface
	public interface IAudioPlayer : IDisposable
	{
		void LoadFile(string path);
		void Play();
		void Stop();
		TimeSpan CurrentPosition { get; set; }
		TimeSpan StartPosition { get; set; }
		TimeSpan EndPosition { get; set; }
	}

	#endregion

	#region AudioPlayer class
	public class AudioPlayer : IAudioPlayer
	{
		public event EventHandler Stopped;

		private WaveOut _waveOut;
		private TrimWaveStream _inStream;

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			CloseWaveOut();
			CloseInStream();
		}

		#region Methods for loading file, playing and stopping.
		/// ------------------------------------------------------------------------------------
		public void LoadFile(string path)
		{
			CloseWaveOut();
			CloseInStream();

			AudioFilePath = path;

			_inStream = path.ToLower().EndsWith(".mp3") ?
				new TrimWaveStream(new Mp3FileReader(path)) :
				new TrimWaveStream(new WaveFileReader(path));
		}

		/// ------------------------------------------------------------------------------------
		public void Play()
		{
			if (PlaybackState == PlaybackState.Stopped)
			{
				CreateWaveOut();
				_inStream.Position = 0;
				_waveOut.Play();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			_waveOut.Stop();
			_inStream.Position = 0;
		}

		/// ------------------------------------------------------------------------------------
		void HandlePlaybackStopped(object sender, EventArgs e)
		{
			CloseWaveOut();
			if (Stopped != null)
				Stopped.Invoke(sender, e);
		}

		#endregion

		#region Properties
		public TimeSpan CurrentPosition { get; set; }
		public string AudioFilePath { get; private set; }

		/// ------------------------------------------------------------------------------------
		public TimeSpan StartPosition
		{
			get { return _inStream.StartPosition; }
			set { _inStream.StartPosition = value; }
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan EndPosition
		{
			get { return _inStream.EndPosition; }
			set { _inStream.EndPosition = value; }
		}

		/// ------------------------------------------------------------------------------------
		public PlaybackState PlaybackState
		{
			get { return _waveOut == null ? PlaybackState.Stopped : _waveOut.PlaybackState; }
		}

		#endregion

		#region Methods for creating/closing stream and wave out
		/// ------------------------------------------------------------------------------------
		private void CreateWaveOut()
		{
			if (_waveOut == null)
			{
				_waveOut = new WaveOut();
				_waveOut.Init(_inStream);
				_waveOut.PlaybackStopped += HandlePlaybackStopped;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void CloseInStream()
		{
			if (_inStream != null)
			{
				_inStream.Dispose();
				_inStream = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void CloseWaveOut()
		{
			if (_waveOut != null)
			{
				_waveOut.Dispose();
				_waveOut = null;
			}
		}

		#endregion
	}

	#endregion
}
