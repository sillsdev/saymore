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

		/// ------------------------------------------------------------------------------------
		public OralAnnotationRecorder(PeakMeterCtrl peakMeter, Action<TimeSpan> recordingProgressAction)
			: base(20)
		{
			RecordingFormat = AudioUtils.GetDefaultWaveFormat(1);
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
		public bool BeginAnnotationRecording(string outputWaveFileName)
		{
			if (GetIsInErrorState())
				return false;

			if (_formerlyInErrorState)
			{
				CloseWaveIn();
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
				base.Stop();
				return;
			}

			AbortRecording();
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
