using System;
using System.IO;
using System.Linq;
using Localization;
using NAudio.Wave;
using Palaso.Media.Naudio;
using Palaso.Reporting;
using SayMore.Media.Audio;
using SayMore.Model;
using SayMore.Properties;

namespace SayMore.UI.SessionRecording
{
	public class SessionRecorderDlgViewModel : IDisposable
	{
		public event EventHandler UpdateAction;
		public AudioRecorder Recorder { get; private set; }
		private AudioPlayer _player;
		private readonly string _path;

		/// ------------------------------------------------------------------------------------
		public SessionRecorderDlgViewModel()
		{
			// This code was used to do some testing of what NAudio returns. At some point,
			// in general, it may prove to lead to something useful for getting the supported
			// formats for a recording device.
			//var devices = new MMDeviceEnumerator();
			//var defaultDevice = devices.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
			//var recDev = RecordingDevice.Devices.First();
			//recDev.Capabilities = WaveIn.GetCapabilities(0);
			//recDev.GenericName = defaultDevice.FriendlyName;
			//Recorder = new AudioRecorder();
			//Recorder.SelectedDevice = recDev;

			Recorder = new AudioRecorder(60); // 1 hour
			Recorder.SelectedDevice = RecordingDevice.Devices.First();
			Recorder.Stopped += (sender, e) =>
			{
				if (UpdateAction != null)
					UpdateAction(sender, e);
			};
			_path = Path.Combine(Path.GetTempPath(),
				string.Format("SayMoreSessionRecording_{0}.wav",
				DateTime.Now.ToString("yyyyMMdd_HHmmss")));

			if (File.Exists(_path))
			{
				try { File.Delete(_path); }
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			CloseAll();

			if (File.Exists(_path))
			{
				try { File.Delete(_path); }
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		public void BeginRecording()
		{
			if (AudioUtils.GetCanRecordAudio())
				Recorder.BeginRecording(_path, true);
		}

		/// ------------------------------------------------------------------------------------
		public void BeginPlayback()
		{
			if (!AudioUtils.GetCanPlaybackAudio())
				return;

			_player = new AudioPlayer();
			_player.Stopped += (sender, e) =>
			{
				_player.Dispose(); _player = null;
				if (UpdateAction != null)
					UpdateAction(sender, e);
			};
			_player.LoadFile(_path);
			_player.Play();
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			if (IsRecording)
			{
				Recorder.Stop();
				return;
			}

			if (_player == null)
				return;

			_player.Stop();
			_player.Dispose();
			_player = null;
		}

		/// ------------------------------------------------------------------------------------
		public bool CanRecordNow
		{
			get
			{
				return _player == null && Recorder != null &&
					(Recorder.RecordingState == RecordingState.Monitoring ||
					Recorder.RecordingState == RecordingState.Stopped);
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool IsRecording
		{
			get
			{
				return (Recorder.RecordingState == RecordingState.Recording ||
					Recorder.RecordingState == RecordingState.RequestedStop);
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool CanPlay
		{
			get
			{
				return (Recorder != null && !IsRecording && !string.IsNullOrEmpty(_path) && File.Exists(_path));
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool IsPlaying
		{
			get { return _player != null && _player.PlaybackState == PlaybackState.Playing; }
		}

		/// ------------------------------------------------------------------------------------
		public void CloseAll()
		{
			if (Recorder == null)
				return;

			Stop();
			Recorder.Dispose();
			Recorder = null;
		}

		/// ------------------------------------------------------------------------------------
		public void MoveRecordingToSessionFolder(Session session)
		{
			try
			{
				CloseAll();
				File.Move(_path, Path.Combine(session.FolderPath, session.Id + ".wav"));
			}
			catch (Exception e)
			{
				var msg = LocalizationManager.GetString(
					"DialogBoxes.SessionRecorderDlg.ErrorMovingRecordingToSessionFolder",
					"There was an error moving your recording to the session folder for '{0}'.\r\n\r\n" +
					"Unexpectedly, SayMore has probably kept a lock on the file; therefore, the recording will not " +
					"be deleted and it may be copied from your temporary folder after closing " +
					"SayMore.\r\n\r\nThe file is:\r\n\r\n{1}.");

				ErrorReport.NotifyUserOfProblem(e, msg, session.Id, _path);
			}
		}
	}
}
