using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Mixer;
using Palaso.Reporting;

namespace SayMore.AudioUtils
{
	#region RecordingState enumeration
	public enum RecordingState
	{
		Stopped,
		Monitoring,
		Recording,
		RequestedStop
	}

	#endregion

	#region IAudioRecorder interface
	public interface IAudioRecorder
	{
		void BeginMonitoring(int recordingDevice);
		void BeginRecording(string path);
		void Stop();
		double MicrophoneLevel { get; set; }
		RecordingState RecordingState { get; }
		SampleAggregator SampleAggregator { get; }
		event EventHandler Stopped;
		WaveFormat RecordingFormat { get; set; }
		TimeSpan RecordedTime { get; }
	}

	#endregion

	#region AudioRecorder class
	public class AudioRecorder : IAudioRecorder, IDisposable
	{
		/// <summary>
		/// This guy is always working, whether we're playing, recording, or just idle (monitoring)
		/// </summary>
		private WaveIn _waveIn;

		/// <summary>
		/// This guy is recreated for each recording, and disposed of when recording stops.
		/// </summary>
		private WaveFileWriter _writer;

		private UnsignedMixerControl _volumeControl;
		private double _microphoneLevel = 100;
		private RecordingState _recordingState;
		private WaveFormat _recordingFormat;
		private Control _recordingLevelDisplayControl;
		private TrackBar _recordingLevelChangeControl;
		private Timer _monitorTimer;
		private float _peakLevel;

		public event EventHandler Stopped = delegate { };
		public event EventHandler RecordingStarted = delegate { };

		public TimeSpan RecordedTime { get; set; }
		public SampleAggregator SampleAggregator { get; private set; }
		public Color MeterLevelMaxPeakColor { get; set; }
		public Color MeterLevelMidColor { get; set; }
		public Color MeterLevelBaseColor { get; set; }

		/// ------------------------------------------------------------------------------------
		public AudioRecorder(int sampleRate, int bitsPerSample, int channels) :
			this(new WaveFormat(sampleRate, bitsPerSample, channels))
		{
		}

		/// ------------------------------------------------------------------------------------
		public AudioRecorder(WaveFormat recordingFormat)
		{
			MeterLevelMaxPeakColor = Color.Red;
			MeterLevelMidColor = Color.Yellow;
			MeterLevelBaseColor = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00);

			SampleAggregator = new SampleAggregator();
			SampleAggregator.MaximumCalculated += delegate(object sender, MaxSampleEventArgs e)
			{
				_peakLevel = Math.Max(e.MaxSample, Math.Abs(e.MinSample));
			};

			RecordingFormat = recordingFormat;
			BeginMonitoring(0);
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_waveIn != null)
			{
				_waveIn.Dispose();
				_waveIn = null;
			}

			if (_writer != null)
			{
				_writer.Dispose();
				_writer = null;
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public WaveFormat RecordingFormat
		{
			get { return _recordingFormat; }
			set
			{
				_recordingFormat = value;
				SampleAggregator.NotificationCount = value.SampleRate / 10;
			}
		}

		/// ------------------------------------------------------------------------------------
		public double MicrophoneLevel
		{
			get { return _microphoneLevel; }
			set
			{
				_microphoneLevel = value;
				if (_volumeControl != null)
					_volumeControl.Percent = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public RecordingState RecordingState
		{
			get { return _recordingState; }
			private set
			{
				_recordingState = value;
				Debug.WriteLine("recorder state--> " + value);
			}
		}

		#endregion

		#region Methods for beginning recording, monitoring and stopping
		/// ------------------------------------------------------------------------------------
		public void BeginRecording(string waveFileName)
		{
			if (_recordingState != RecordingState.Monitoring)
			{
				throw new InvalidOperationException(
					"Can't begin recording while we are in this state: " + _recordingState);
			}

			var folder = Path.GetDirectoryName(waveFileName);
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			_writer = new WaveFileWriter(waveFileName, _recordingFormat);
			RecordingState = RecordingState.Recording;
			RecordingStarted(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public void BeginMonitoring(int recordingDevice)
		{
			Debug.Assert(_waveIn == null, "only call this once");
			try
			{
				if (_recordingState != RecordingState.Stopped)
				{
					throw new InvalidOperationException("Can't begin monitoring while we are in this state: " +
						_recordingState.ToString());
				}

				Debug.Assert(_waveIn == null);
				_waveIn = new WaveIn();
				_waveIn.DeviceNumber = recordingDevice;
				_waveIn.DataAvailable += HandleWaveInDataAvailable;
				_waveIn.WaveFormat = _recordingFormat;
				_waveIn.StartRecording();
				TryGetVolumeControl();

				if (_volumeControl != null)
					_microphoneLevel = Math.Round(_volumeControl.Percent, MidpointRounding.AwayFromZero);

				RecordingState = RecordingState.Monitoring;
			}
			catch (Exception e)
			{
				ErrorReport.NotifyUserOfProblem(new ShowOncePerSessionBasedOnExactMessagePolicy(),
					e, "There was a problem starting up volume monitoring.");

				if (_waveIn != null)
				{
					_waveIn.Dispose();
					_waveIn = null;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			if (_recordingState == RecordingState.Recording)
				RecordingState = RecordingState.RequestedStop;

			TransitionFromRecordingToMonitoring();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// As far as naudio is concerned, we are still "recording", but we aren't writing
		/// this file anymore
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void TransitionFromRecordingToMonitoring()
		{
			RecordingState = RecordingState.Monitoring;

			if (_writer != null)
			{
				RecordedTime = TimeSpan.FromSeconds((double)_writer.Length / _writer.WaveFormat.AverageBytesPerSecond);
				_writer.Dispose();
				_writer = null;
			}

			Stopped(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private void TryGetVolumeControl()
		{
			// Check for Vista OS or newer.
			if (Environment.OSVersion.Version.Major >= 6)
			{
				// This is a bit of a kludge, but it would seem there may be a bug in the NAudio
				// library having to do with some static values being initialized the first
				// time GetMixerLine() is called. If after that, the user goes to the Windows
				// control panel to change the default recording format, the first subsequent
				// WaveIn that is created throws an exception. Therefore, if that is going
				// to happen, we get that one exception out of the program's system and then
				// carry on normally. An exception thrown after that is one we want to handle.
				// See SP-268
				try { _waveIn.GetMixerLine(); }
				catch { }

				var mixerLine = _waveIn.GetMixerLine();
				foreach (var control in mixerLine.Controls.Where(c => c.ControlType == MixerControlType.Volume))
				{
					_volumeControl = control as UnsignedMixerControl;
					break;
				}

				return;
			}

			// OS is older than Vista.
			var mixer = new Mixer(_waveIn.DeviceNumber);
			foreach (var destination in mixer.Destinations.Where(d => d.ComponentType == MixerLineComponentType.DestinationWaveIn))
			{
				foreach (var source in destination.Sources.Where(s => s.ComponentType == MixerLineComponentType.SourceMicrophone))
				{
					foreach (var control in source.Controls.Where(c => c.ControlType == MixerControlType.Volume))
					{
						_volumeControl = control as UnsignedMixerControl;
						break;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveInDataAvailable(object sender, WaveInEventArgs e)
		{
			var buffer = e.Buffer;
			int bytesRecorded = e.BytesRecorded;
			WriteToFile(buffer, bytesRecorded);

			var bytesPerSample = _waveIn.WaveFormat.BitsPerSample / 8;

			// It appears the data only occupies 2 bytes of those in a sample and that
			// those 2 are always the last two in each sample. The other bytes are zero
			// filled. Therefore, when getting those two bytes, the first index into a
			// sample needs to be 0 for 16 bit samples, 1 for 24 bit samples and 2 for
			// 32 bit samples. I'm not sure what to do for 8 bit samples. I could never
			// figure out the correct conversion of a byte in an 8 bit per sample buffer
			// to a float sample value. However, I doubt folks are going to be recording
			// at 8 bits/sample so I'm ignoring that problem.
			for (var index = bytesPerSample - 2; index < bytesRecorded - 1; index += bytesPerSample)
			{
				var sample = (short)((buffer[index + 1] << 8) | buffer[index]);
				var sample32 = sample / 32768f;
				SampleAggregator.Add(sample32);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void WriteToFile(byte[] buffer, int bytesRecorded)
		{
			long maxFileLength = _recordingFormat.AverageBytesPerSecond * 60;

			if (_recordingState != RecordingState.Recording && _recordingState != RecordingState.RequestedStop)
				return;

			int toWrite = (int)Math.Min(maxFileLength - _writer.Length, bytesRecorded);
			if (toWrite > 0)
				_writer.Write(buffer, 0, bytesRecorded);
			else
				Stop();
		}

		#region Methods for displaying and controling the recording level
		/// ------------------------------------------------------------------------------------
		public void SetRecordingLevelChangeControl(TrackBar trackBar)
		{
			if (_recordingLevelChangeControl != null)
				_recordingLevelChangeControl.ValueChanged -= HandleRecordingLevelValueChanged;

			_recordingLevelChangeControl = trackBar;
			if (_recordingLevelChangeControl == null)
				return;

			_recordingLevelChangeControl.Minimum = 0;
			_recordingLevelChangeControl.Maximum = 100;
			_recordingLevelChangeControl.TickFrequency = 5;
			_recordingLevelChangeControl.LargeChange = 20;
			_recordingLevelChangeControl.SmallChange = 5;
			_recordingLevelChangeControl.Value = (int)MicrophoneLevel;
			_recordingLevelChangeControl.ValueChanged += HandleRecordingLevelValueChanged;
		}

		/// ------------------------------------------------------------------------------------
		void HandleRecordingLevelValueChanged(object sender, EventArgs e)
		{
			MicrophoneLevel = _recordingLevelChangeControl.Value;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the control in which the recorder will display the recording level.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetRecordingLevelDisplayControl(Control ctrl)
		{
			if (_monitorTimer != null)
				_monitorTimer.Dispose();

			if (_recordingLevelDisplayControl != null)
			{
				_recordingLevelDisplayControl.Paint -= HandleLevelControlPaint;
				_recordingLevelDisplayControl.HandleDestroyed -= HandleLevelControlHandleDestroyed;
			}

			_recordingLevelDisplayControl = ctrl;

			if (_recordingLevelDisplayControl == null)
				return;

			_recordingLevelDisplayControl.Paint += HandleLevelControlPaint;
			_recordingLevelDisplayControl.HandleDestroyed += HandleLevelControlHandleDestroyed;

			_monitorTimer = new Timer();
			_monitorTimer.Interval = 10;
			_monitorTimer.Tick += delegate { _recordingLevelDisplayControl.Invalidate(); };
			_monitorTimer.Start();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLevelControlHandleDestroyed(object sender, EventArgs e)
		{
			if (_monitorTimer != null)
				_monitorTimer.Dispose();

			_recordingLevelDisplayControl.Paint -= HandleLevelControlPaint;
			_recordingLevelDisplayControl.HandleDestroyed -= HandleLevelControlHandleDestroyed;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLevelControlPaint(object sender, PaintEventArgs e)
		{
			if (_recordingLevelDisplayControl.Width < _recordingLevelDisplayControl.Height)
				DrawVerticalMeter(e.Graphics);
			else
				DrawHorizontalMeter(e.Graphics);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawHorizontalMeter(Graphics g)
		{
			var fullExtent = _recordingLevelDisplayControl.ClientSize.Width;

			// The first step involves painting the entire control so it looks maxed out.
			// After that, erase (using the control's background color) from the top of
			// the control, to a point along the Y coordinate that represents the peak level.

			// Draw yellow fading to red. The gradient to and yellow take up 10% of the meter.
			var partialExtent = (int)(fullExtent * 0.10);
			var rc = new Rectangle(fullExtent - partialExtent - 1, 0, partialExtent + 2,
				_recordingLevelDisplayControl.ClientSize.Height);

			using (var br = new LinearGradientBrush(rc, MeterLevelMidColor, MeterLevelMaxPeakColor, 0f))
			{
				var blend = new Blend();
				blend.Positions = new[] { 0.0f, 0.4f, 0.9f, 1.0f };
				blend.Factors = new[] { 0.0f, 0.5f, 1.0f, 1.0f };
				br.Blend = blend;
				g.FillRectangle(br, rc);
			}

			// Draw green fading to yellow. The gradient green to yellow take up 90% of the meter.
			rc.X = 0;
			rc.Width = (int)(fullExtent * 0.90) + 2;

			using (var br = new LinearGradientBrush(rc, MeterLevelBaseColor, MeterLevelMidColor, 0f))
			{
				rc.Width--;
				g.FillRectangle(br, rc);
			}

			// If the meter is maxed out, then we're done.
			if (_peakLevel.Equals(1f))
				return;

			// Now use the back ground color to erase the part of control
			// that represents what's above the peak level.
			partialExtent = fullExtent -
				(int)(Math.Round(_peakLevel * fullExtent, MidpointRounding.AwayFromZero));

			rc = new Rectangle(fullExtent - partialExtent, 0, partialExtent,
				_recordingLevelDisplayControl.ClientSize.Height);

			using (var br = new SolidBrush(_recordingLevelDisplayControl.BackColor))
				g.FillRectangle(br, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawVerticalMeter(Graphics g)
		{
			var fullExtent = _recordingLevelDisplayControl.ClientSize.Height;

			// The first step involves painting the entire control so it looks maxed out.
			// After that, erase (using the control's background color) from the top of
			// the control, to a point along the Y coordinate that represents the peak level.

			// Draw yellow fading to red. The gradient to and yellow take up 10% of the meter.
			var partialExtent = (int)(fullExtent * 0.10);
			var rc = new Rectangle(0, 0, _recordingLevelDisplayControl.ClientSize.Width, partialExtent + 1);

			using (var br = new LinearGradientBrush(rc, MeterLevelMaxPeakColor, MeterLevelMidColor, 90f))
			{
				var blend = new Blend();
				blend.Positions = new[] { 0.0f, 0.4f, 0.9f, 1.0f };
				blend.Factors = new[] { 0.0f, 0.5f, 1.0f, 1.0f };
				br.Blend = blend;
				g.FillRectangle(br, rc);
			}

			// Draw green fading to yellow. The gradient green to yellow take up 90% of the meter.
			rc.Y = partialExtent - 1;
			rc.Height = (int)(fullExtent * 0.90) + 2;

			using (var br = new LinearGradientBrush(rc, MeterLevelMidColor, MeterLevelBaseColor, 90f))
			{
				rc.Y++;
				g.FillRectangle(br, rc);
			}

			// If the meter is maxed out, then we're done.
			if (_peakLevel.Equals(1f))
				return;

			// Now use the back ground color to erase the part of control
			// that represents what's above the peak level.
			partialExtent = fullExtent -
				(int)(Math.Round(_peakLevel * fullExtent, MidpointRounding.AwayFromZero));

			rc = new Rectangle(0, 0, _recordingLevelDisplayControl.ClientSize.Width, partialExtent);

			using (var br = new SolidBrush(_recordingLevelDisplayControl.BackColor))
				g.FillRectangle(br, rc);
		}

		#endregion
	}

	#endregion
}
