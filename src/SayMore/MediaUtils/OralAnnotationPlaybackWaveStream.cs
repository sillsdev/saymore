using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace SayMore.Media
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// NAudio can only playback audio that's 1 or 2 channels. Oral annotation audio streams
	/// are either 3 channel or 4 channel, depending on whether the original recording
	/// has 1 or 2 channels. This class acts like a 1 or 2 channel audio stream, but will
	/// read its data from the oral annotation stream merging channels into a 1 or 2
	/// channel stream.
	///
	/// When the original recording is 1 channel, then the oral annotation is 3 channel (i.e.
	/// 1 = original, 2 = careful speech, 3 = oral translation). In that case, all 3 channels
	/// in the oral annotation stream are combined into a single channel.
	///
	/// When the original recording is 2 channel, then the oral annotation is 4 channel (i.e.
	/// 1 and 2 = original, 2 = careful speech, 3 = oral translation). In that case, channels
	/// 3 and 4 in the oral annotation stream are combined with both channels 1 and 2.
	///
	/// This all works well since the upper two channels (i.e. careful speech and oral
	/// translation) will always be silent when the original is not and vice versa.
	///
	/// The oral annotations sample rate and bits per sample are preserved.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class OralAnnotationPlaybackWaveStream : WaveStream
	{
		private readonly WaveStream _source;
		private readonly WaveFormat _waveFormat;
		private readonly int _carefulSpeechChannel;
		private readonly int _oralTransSpeechChannel;

		/// ------------------------------------------------------------------------------------
		public OralAnnotationPlaybackWaveStream(WaveStream source)
		{
			_source = source;

			_waveFormat = (_source.WaveFormat.Channels == 3 ?
				new WaveFormat(_source.WaveFormat.SampleRate, _source.WaveFormat.BitsPerSample, 1) :
				new WaveFormat(_source.WaveFormat.SampleRate, _source.WaveFormat.BitsPerSample, 2));

			_carefulSpeechChannel = _source.WaveFormat.Channels - 2;
			_oralTransSpeechChannel = _source.WaveFormat.Channels - 1;
		}

		/// ------------------------------------------------------------------------------------
		public override WaveFormat WaveFormat
		{
			get { return _waveFormat; }
		}

		/// ------------------------------------------------------------------------------------
		public override long Length
		{
			get  { return (long)(_source.Length / (_waveFormat.Channels == 1 ? 3d : 2d)); }
		}

		/// ------------------------------------------------------------------------------------
		public override long Position
		{
			get { return (long)(_source.Position / (_waveFormat.Channels == 1 ? 3d : 2d)); }
			set { _source.Position = value * (_waveFormat.Channels == 1 ? 3 : 2); }
		}

		/// ------------------------------------------------------------------------------------
		public override int Read(byte[] buffer, int offset, int count)
		{
			int bytesToWrite = (int)Math.Min(count, Length - Position);

			if (bytesToWrite == 0)
				return 0;

			int bytesToRead = bytesToWrite * (_waveFormat.Channels == 1 ? 3 : 2);
			var block = new byte[_source.WaveFormat.BlockAlign];

			for (int i = 0; i < bytesToRead; i += _source.WaveFormat.BlockAlign)
			{
				_source.Read(block, 0, _source.WaveFormat.BlockAlign);
				MergeChannels(buffer, offset, block);
				offset += _waveFormat.BlockAlign;
			}

			return bytesToWrite;
		}

		/// ------------------------------------------------------------------------------------
		public void MergeChannels(IList<byte> buffer, int offset, IList<byte> block)
		{
			int bytesPerChannel = _source.WaveFormat.BlockAlign / _source.WaveFormat.Channels;

			for (int i = 0; i < bytesPerChannel; i++)
			{
				buffer[offset + i] = block[i];
				buffer[offset + i] |= block[(_carefulSpeechChannel * bytesPerChannel) + i];
				buffer[offset + i] |= block[(_oralTransSpeechChannel * bytesPerChannel) + i];

				if (_source.WaveFormat.Channels <= 3)
					continue;

				buffer[offset + bytesPerChannel + i] = block[bytesPerChannel + i];
				buffer[offset + bytesPerChannel + i] |= block[(_carefulSpeechChannel * bytesPerChannel) + i];
				buffer[offset + bytesPerChannel + i] |= block[(_oralTransSpeechChannel * bytesPerChannel) + i];
			}
		}
	}
}
