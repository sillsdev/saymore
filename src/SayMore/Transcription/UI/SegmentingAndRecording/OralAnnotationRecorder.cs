using System;
using System.Linq;
using SIL.Media.Naudio;
using SIL.Media.Naudio.UI;
using SayMore.Media.Audio;
using SayMore.Properties;
using SIL.Media;

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
			lock (this)
			{
				if (GetIsInErrorState())
					return false;

				if (_formerlyInErrorState)
				{
					if (RecordingState == RecordingState.Stopped)
					{
						CloseWaveIn();
						SelectedDevice = RecordingDevice.Devices.First();
						RecordingState = RecordingState.Stopped;
						BeginMonitoring();
					}
					_formerlyInErrorState = false;
				}

				BeginRecording(outputWaveFileName);
			}
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public override void Stop()
		{
			lock (this)
			{
				if (GetIsInErrorState())
					AbortRecording();
				else if (RecordingState == RecordingState.Recording)
					base.Stop();
			}
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
			lock (this)
			{
				try
				{
					// The goal here is just to reference something in the recorder that
					// will throw an exception when something has gone wrong (e.g. the
					// user unplugged or disabled their microphone).
					if (_waveIn.GetMixerLine().Channels > 0)
						return false;
				}
				catch
				{
				}

				if (displayErrorMsg)
					AudioUtils.DisplayNAudioError(null);

				_formerlyInErrorState = true;
			}
			return true;
		}
	}
}
