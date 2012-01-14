using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SayMore.AudioUtils;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class wraps three wave controls representing the three channels in an audio
	/// transcription (i.e. original recording, careful speech and oral translation). This
	/// wrapper will sychronize the display each wave control during playback and scrolling.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationWaveViewer : UserControl
	{
		public EventHandler PlaybackStopped;

		private bool _allowScrolling;
		private int _virtualWaveWidth;
		private BackgroundWorker _worker;
		private WaveOut _waveOut;

		public WaveStream MonoWaveStream { get; private set; }

		#region Construction and disposal
		/// ------------------------------------------------------------------------------------
		public OralAnnotationWaveViewer()
		{
			DoubleBuffered = true;

			InitializeComponent();

			_labelOriginal.Font = new Font(SystemFonts.IconTitleFont, FontStyle.Bold);
			_labelCareful.Font = _labelOriginal.Font;
			_labelTranslation.Font = _labelOriginal.Font;

			_wavePanelOriginal.MouseClick += HandleWavePanelMouseClick;
			_wavePanelCareful.MouseClick += HandleWavePanelMouseClick;
			_wavePanelTranslation.MouseClick += HandleWavePanelMouseClick;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				if (_waveOut != null)
				{
					if (_waveOut.PlaybackState != PlaybackState.Stopped)
						_waveOut.Stop();

					_waveOut.Dispose();
					_waveOut = null;
				}

				if (MonoWaveStream != null)
				{
					MonoWaveStream.Close();
					MonoWaveStream.Dispose();
				}

				components.Dispose();
			}

			base.Dispose(disposing);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads a 3-channel annotation file.
		/// </summary>
		/// <remarks>
		/// The process of loading the file involves creating 3 buffers, containing the
		/// samples for each channel. It is assumed the original recording is in the first
		/// channel, the careful speech annotation in the second channel and the oral
		/// translation in the third. Once each channel's buffer of samples is filled, those
		/// are passed off to their respective wave controls for display. Finally, for the
		/// sake of playing back the set of annotations, a single channel stream is created
		/// from the three channels stream.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		public void LoadAnnotationAudioFile(string filename)
		{
			_worker = new BackgroundWorker();
			_worker.WorkerReportsProgress = true;
			_worker.WorkerSupportsCancellation = true;
			_worker.ProgressChanged += HandleWorkerProgressChanged;
			_worker.DoWork += InternalLoadAnnotationAudioFile;
			_worker.RunWorkerAsync(filename);

			while (_worker.IsBusy)
				Application.DoEvents();

			_worker.Dispose();
			_worker = null;
		}

		/// ------------------------------------------------------------------------------------
		public void CancelLoading()
		{
			if (_worker != null && _worker.IsBusy)
				_worker.CancelAsync();
		}

		/// ------------------------------------------------------------------------------------
		public bool IsBusyLoading
		{
			get { return _worker != null; }
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.UserState is Exception)
				throw e.UserState as Exception;
		}

		/// ------------------------------------------------------------------------------------
		private void InternalLoadAnnotationAudioFile(object sender, DoWorkEventArgs e)
		{
			try
			{
				using (var threeChannelStream = new WaveFileReader(e.Argument as string))
				{
					var orginalSamples = new float[threeChannelStream.SampleCount];
					var carefulSamples = new float[threeChannelStream.SampleCount];
					var translationSamples = new float[threeChannelStream.SampleCount];

					var provider = new SampleChannel(threeChannelStream);
					float[] buffer = new float[3];

					for (int i = 0; i < threeChannelStream.SampleCount && provider.Read(buffer, 0, 3) > 0; i++)
					{
						orginalSamples[i] = buffer[0];
						carefulSamples[i] = buffer[1];
						translationSamples[i] = buffer[2];

						if (_worker.CancellationPending)
							return;
					}

					_wavePanelOriginal.Initialize(orginalSamples, threeChannelStream.TotalTime);
					_wavePanelCareful.Initialize(carefulSamples, threeChannelStream.TotalTime);
					_wavePanelTranslation.Initialize(translationSamples, threeChannelStream.TotalTime);

					CreateMonoStreamFrom3ChannelStream(threeChannelStream);
					threeChannelStream.Close();
				}
			}
			catch (Exception error)
			{
				_worker.ReportProgress(0, error);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the annotation 3-channel stream to a single channel stream. Each channel
		/// contains one of the annotations. The original is in channel 1, careful speech in
		/// channel 2 and translation in channel 3.
		/// </summary>
		/// <remarks>
		/// The 3-channel stream is a series of 3 samples of bytes, each sample containing
		/// a number of bytes equal to the number of bits per sample divided by 2. Each
		/// set of samples for the three channels is a block.
		///
		/// Example for 3-channel, 16-bits per sample
		/// +-----------------------------------------------------+--------+--------+
		/// +                        Block 1                      |      Block 2     ... etc.
		/// +-----------------------------------------------------+--------+--------+
		/// |   (Channel 1)   |   (Channel 2)   |   (Channel 3)   |   (Channel 1)   |... etc.
		/// +--------+--------+--------+--------+--------+--------+--------+--------+
		/// | byte 0 | byte 1 | byte 2 | byte 3 | byte 4 | byte 5 | byte 6 | byte 7 |... etc.
		/// +--------+--------+--------+--------+--------+--------+--------+--------+
		/// |     Sample 0    |     Sample 1    |     Sample 2    |     Sample 3    |... etc.
		/// +--------+--------+--------+--------+--------+--------+--------+--------+
		///
		/// The process of creating a mono stream involves combining the samples in each block
		/// into a single sample (i.e. 16-bit value in the example case). That's fairly easy
		/// because we know that whenever a sample contains non-zero data (i.e. sound), the
		/// other samples in the block will contain zeros (i.e. silence). The stream is read in
		/// blocks and written to a new buffer having the bytes from each sample bitwise
		/// OR'd together.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		private void CreateMonoStreamFrom3ChannelStream(WaveFileReader threeChannelStream)
		{
			threeChannelStream.Position = 0;

			// 2 for 16 bit, 3 for 24 bit, 4 for 32 bit, etc.
			var bytesPerSample = threeChannelStream.WaveFormat.BitsPerSample / 8;

			var bytesPerBlock = bytesPerSample * threeChannelStream.WaveFormat.Channels;
			var buffer = new byte[threeChannelStream.SampleCount * bytesPerSample];
			var singleBlock = new byte[bytesPerBlock];

			for (int i = 0; i < threeChannelStream.SampleCount * bytesPerSample;)
			{
				threeChannelStream.Read(singleBlock, 0, bytesPerBlock);

				for (int byteInSample = 0; byteInSample < bytesPerSample; byteInSample++)
				{
					for (int channel = 0; channel < threeChannelStream.WaveFormat.Channels; channel++)
						buffer[i] |= singleBlock[byteInSample + (channel * bytesPerSample)];

					i++;

					if (_worker.CancellationPending)
						return;
				}

				if (_worker.CancellationPending)
					return;
			}

			var memStream = new MemoryStream(buffer);

			var format = new WaveFormat(threeChannelStream.WaveFormat.SampleRate,
				threeChannelStream.WaveFormat.BitsPerSample, 1);

			MonoWaveStream = new RawSourceWaveStream(memStream, format);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			Utils.SetWindowRedraw(this, false);
			base.OnResize(e);
			Utils.SetWindowRedraw(this, true);
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(false)]
		public bool AllowScrolling
		{
			get { return _allowScrolling; }
			set
			{
				_allowScrolling = value;
				AutoScroll = value;

				// TODO: Deal with resizing the table layout and what to set the virtual width.
			}
		}

		/// ------------------------------------------------------------------------------------
		public int VirtualWaveWidth
		{
			get { return _virtualWaveWidth; }
			set
			{
				_virtualWaveWidth = value;
				// TODO: Deal with resizing the table layout and what to do with the AutoScroll property.
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTableLayoutPaint(object sender, PaintEventArgs e)
		{
			var dx = _wavePanelOriginal.Left - 1;

			using (var pen = new Pen(VisualStyleInformation.TextControlBorder))
				e.Graphics.DrawLine(pen, dx, 0, dx, _tableLayout.Height);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWavePanelMouseClick(object sender, MouseEventArgs e)
		{
			if (_waveOut != null && _waveOut.PlaybackState == PlaybackState.Playing)
				Stop();

			_wavePanelOriginal.SetCursor(e.X);
			_wavePanelCareful.SetCursor(e.X);
			_wavePanelTranslation.SetCursor(e.X);
		}

		/// ------------------------------------------------------------------------------------
		public void Play()
		{
			var stream = MonoWaveStream;

			if (_wavePanelOriginal.GetCursorTime() > TimeSpan.Zero)
			{
				var cursorTime = _wavePanelOriginal.GetCursorTime();
				stream = new WaveSegmentStream(MonoWaveStream, cursorTime);
			}

			var provider = new SampleChannel(stream);
			provider.PreVolumeMeter += HandlePlaybackMetering;

			_waveOut = new WaveOut();
			_waveOut.DesiredLatency = 100;
			_waveOut.Init(new SampleToWaveProvider(provider));
			_waveOut.PlaybackStopped += delegate
			{
				_waveOut.Dispose();
				_waveOut = null;
				_wavePanelOriginal.SetCursor(TimeSpan.Zero);
				_wavePanelCareful.SetCursor(TimeSpan.Zero);
				_wavePanelTranslation.SetCursor(TimeSpan.Zero);

				if (PlaybackStopped != null)
					PlaybackStopped(this, EventArgs.Empty);
			};

			_waveOut.Play();
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackMetering(object sender, StreamVolumeEventArgs e)
		{
			// If playback didn't start at zero, then we're using a WaveOffsetStream and
			// when using that kind of stream, we never get a  PlaybackStopped event on
			// the WaveOut, so we have to force it here.
			if (MonoWaveStream.CurrentTime == MonoWaveStream.TotalTime)
			{
				_waveOut.Stop();
				MonoWaveStream.CurrentTime = TimeSpan.Zero;
			}

			_wavePanelOriginal.SetCursor(MonoWaveStream.CurrentTime);
			_wavePanelCareful.SetCursor(MonoWaveStream.CurrentTime);
			_wavePanelTranslation.SetCursor(MonoWaveStream.CurrentTime);
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			if (_waveOut != null)
				_waveOut.Stop();
		}
	}
}
