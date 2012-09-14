using System;
using System.Linq;
using Palaso.Media.Naudio;
using Palaso.Media.Naudio.UI;
using SayMore.Media.Audio;
using SayMore.Properties;

namespace SayMore.Transcription.UI
{
	public class OralAnnotationRecorder : AudioRecorder
	{
		private readonly PeakMeterCtrl _peakMeterCtrl;
		private readonly Action<TimeSpan> _recordingProgressAction;
		private bool _formerlyInErrorState;
		private int _bufferSize = Settings.Default.NAudioBufferMilliseconds;
		private int _bufferCount = Settings.Default.NumberOfNAudioRecordingBuffers;
		private bool _waveInBuffersChanged;

		/// ------------------------------------------------------------------------------------
		public OralAnnotationRecorder(PeakMeterCtrl peakMeter, Action<TimeSpan> recordingProgressAction)
			: base(20)
		{
			base.RecordingFormat = AudioUtils.GetDefaultWaveFormat(1);
			SelectedDevice = RecordingDevice.Devices.First();

			_peakMeterCtrl = peakMeter;
			_recordingProgressAction = recordingProgressAction;

			RecordingProgress += (s, e) => _recordingProgressAction(e.RecordedLength);
			PeakLevelChanged += (s, e) =>
			{
				if (_peakMeterCtrl != null)
					_peakMeterCtrl.PeakLevel = e.Level;
			};
		}

		/// ------------------------------------------------------------------------------------
		protected override void InitializeWaveIn()
		{
			base.InitializeWaveIn();
			_waveIn.NumberOfBuffers = _bufferCount;
			_waveIn.BufferMilliseconds = _bufferSize;
		}

		/// ------------------------------------------------------------------------------------
		/// Temporary code
		public int NumberOfBuffers
		{
			set
			{
				_bufferCount = value;
				_waveInBuffersChanged = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// Temporary code
		public int BufferMilliseconds
		{
			set
			{
				_bufferSize = value;
				_waveInBuffersChanged = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool BeginAnnotationRecording(string outputWaveFileName)
		{
			if (GetIsInErrorState())
				return false;

			if (_formerlyInErrorState || _waveInBuffersChanged)
			{
				_waveInBuffersChanged = false;
				CloseWaveIn();
				RecordingFormat = AudioUtils.GetDefaultWaveFormat(1);
				SelectedDevice = RecordingDevice.Devices.First();
				RecordingState = RecordingState.Stopped;
				BeginMonitoring();
				_formerlyInErrorState = false;
			}

			BeginRecording(outputWaveFileName);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public override void Stop()
		{
			if (!GetIsInErrorState() && RecordingState == RecordingState.Recording)
			{
				//if (RecordingState != RecordingState.Recording)
				//    throw new InvalidOperationException("Stop recording should not be called when recording was not initiated.");

				base.Stop();
				return;
			}

			RecordedTime = TimeSpan.Zero;
			RecordingState = RecordingState.Monitoring;
			CloseWriter();
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsRecordingTooShort()
		{
			return RecordedTime < TimeSpan.FromMilliseconds(Settings.Default.MinimumAnnotationSegmentLengthInMilliseconds);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsInErrorState()
		{
			return GetIsInErrorState(false);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsInErrorState(bool displayErrorMsg)
		{
			try
			{
				// The goal here is just to reference something in the recorder that
				// will throw an exception when something has gone wrong (e.g. the
				// user unplugged or disabled their microphone).
				if (_waveIn.GetMixerLine().Channels > 0)
					return false;
			}
			catch { }

			if (displayErrorMsg)
				AudioUtils.DisplayNAudioError(null);

			_formerlyInErrorState = true;
			return true;
		}
	}
}
