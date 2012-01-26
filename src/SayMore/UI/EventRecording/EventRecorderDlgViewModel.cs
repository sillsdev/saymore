using System;
using System.IO;
using System.Linq;
using Localization;
using NAudio.Wave;
using Palaso.Media.Naudio;
using Palaso.Reporting;
using SayMore.Model;

namespace SayMore.UI.EventRecording
{
	public class EventRecorderDlgViewModel : IDisposable
	{
		public Action UpdateAction { get; set; }
		public AudioRecorder Recorder { get; private set; }
		private readonly AudioPlayer _player;
		private readonly string _path;

		/// ------------------------------------------------------------------------------------
		public EventRecorderDlgViewModel()
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
			Recorder.Stopped += delegate { UpdateAction(); };

			_player = new AudioPlayer();
			_player.Stopped += delegate { UpdateAction(); };

			_path = Path.Combine(Path.GetTempPath(), "tmpSayMoreEvent.wav");
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_player != null)
				_player.Dispose();

			if (File.Exists(_path))
			{
				try { File.Delete(_path); }
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		public void BeginRecording()
		{
			Recorder.BeginRecording(_path);
		}

		/// ------------------------------------------------------------------------------------
		public void BeginPlayback()
		{
			_player.LoadFile(_path);
			_player.Play();
		}

		/// ------------------------------------------------------------------------------------
		public void StopPlayback()
		{
			_player.Stop();
		}

		/// ------------------------------------------------------------------------------------
		public bool CanRecordNow
		{
			get
			{
				return Recorder != null && (_player.PlaybackState == PlaybackState.Stopped &&
					(Recorder.RecordingState == RecordingState.Monitoring ||
					Recorder.RecordingState == RecordingState.Stopped));
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
				return (Recorder != null && Recorder.RecordingState != RecordingState.Recording &&
					!string.IsNullOrEmpty(_path) && File.Exists(_path));
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool IsPlaying
		{
			get { return _player.PlaybackState == PlaybackState.Playing; }
		}

		/// ------------------------------------------------------------------------------------
		public void MoveRecordingToEventFolder(Event evnt)
		{
			try
			{
				File.Move(_path, Path.Combine(evnt.FolderPath, evnt.Id + ".wav"));
			}
			catch (Exception e)
			{
				var msg = LocalizationManager.GetString(
					"DialogBoxes.EventRecorderDlg.ErrorMovingRecordingToEventFolder",
					"There was an error moving your recording to the event folder for '{0}'.");

				ErrorReport.NotifyUserOfProblem(e, msg, evnt.Id);
			}
		}
	}
}
