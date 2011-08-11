using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

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
		private bool _allowScrolling;
		private int _virtualWaveWidth;
		private WaveOut _waveOut;

		public WaveStream MonoWaveStream { get; private set; }

		#region Construction and disposal
		/// ------------------------------------------------------------------------------------
		public OralAnnotationWaveViewer()
		{
			InitializeComponent();

			_labelOriginal.Font = new Font(SystemFonts.IconTitleFont, FontStyle.Bold);
			_labelCareful.Font = _labelOriginal.Font;
			_labelTranslation.Font = _labelOriginal.Font;

			_wavePanelOriginal.ForeColor = _wavePanelCareful.ForeColor =
				_wavePanelTranslation.ForeColor = SystemColors.WindowText;

			_wavePanelOriginal.BackColor = _wavePanelCareful.BackColor =
				_wavePanelTranslation.BackColor = SystemColors.Window;
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
			var orginalSamples = new List<float>(65535);
			var carefulSamples = new List<float>(65535);
			var translationSamples = new List<float>(65535);

			using (var threeChannelStream = new WaveFileReader(filename))
			{
				var provider = new SampleChannel(threeChannelStream);
				float[] buffer = new float[3];
				while (provider.Read(buffer, 0, 3) > 0)
				{
					orginalSamples.Add(buffer[0]);
					carefulSamples.Add(buffer[1]);
					translationSamples.Add(buffer[2]);
				}

				_wavePanelOriginal.Initialize(orginalSamples, threeChannelStream.TotalTime);
				_wavePanelCareful.Initialize(carefulSamples, threeChannelStream.TotalTime);
				_wavePanelTranslation.Initialize(translationSamples, threeChannelStream.TotalTime);

				CreateMonoStreamFrom3ChannelStream(threeChannelStream);
				threeChannelStream.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the annotation 3-channel stream to a single channel stream. Each channel
		/// contains one of the annotations. The original is in channel 1, careful speech in
		/// channel 2 and translation in channel 3.
		/// </summary>
		/// <remarks>
		/// The 3-channel stream is a series of 3, 16-bit values, or samples. Each set of
		/// 3, 16-bit samples is a block and each sample in a block corresponds to a channel.
		///
		/// +-----------------------------------------------------+--------+--------+
		/// +                        Block 1                      |       Block 2    ... etc.
		/// +-----------------------------------------------------+--------+--------+
		/// |   (Channel 1)   |   (Channel 2)   |   (Channel 3)   |   (Channel 1)   |... etc.
		/// +--------+--------+--------+--------+--------+--------+--------+--------+
		/// | byte 0 | byte 1 | byte 2 | byte 3 | byte 4 | byte 5 | byte 6 | byte 7 |... etc.
		/// +--------+--------+--------+--------+--------+--------+--------+--------+
		/// |Sample 0|Sample 1|Sample 2|Sample 3|Sample 4|Sample 5|Sample 6|Sample 7|... etc.
		/// +--------+--------+--------+--------+--------+--------+--------+--------+
		///
		/// The process of creating a mono stream involves combining the samples in each block
		/// into a single sample (i.e. 16-bit value). In this case, that's fairly easy because
		/// we know that whenever a sample contains non-zero data (i.e. sound), the other
		/// samples in the block will contain zeros (i.e. silence). The stream is read in
		/// blocks of 6 bytes (3 samples) and written to a new buffer having the bytes from
		/// each sample bitwise OR'd together.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		private void CreateMonoStreamFrom3ChannelStream(WaveStream threeChannelStream)
		{
			threeChannelStream.Position = 0;
			var samples = new List<byte>();
			var singleSample = new byte[6];

			while (threeChannelStream.Read(singleSample, 0, 6) > 0)
			{
				samples.Add((byte)(singleSample[0] | singleSample[2] | singleSample[4]));
				samples.Add((byte)(singleSample[1] | singleSample[3] | singleSample[5]));
			}

			var memStream = new MemoryStream(samples.ToArray());
			MonoWaveStream = new RawSourceWaveStream(memStream, new WaveFormat(44100, 1));
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
		public void Play()
		{
			MonoWaveStream.Position = 0;
			var provider = new SampleChannel(MonoWaveStream);
			provider.PreVolumeMeter += HandlePlaybackMetering;

			_waveOut = new WaveOut();
			_waveOut.DesiredLatency = 100;
			_waveOut.Init(new SampleToWaveProvider(provider));
			_waveOut.PlaybackStopped += delegate
			{
				_waveOut.Dispose();
				_waveOut = null;
				_wavePanelOriginal.SetCursor(0);
				_wavePanelCareful.SetCursor(0);
				_wavePanelTranslation.SetCursor(0);
			};

			_waveOut.Play();
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackMetering(object sender, StreamVolumeEventArgs e)
		{
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
