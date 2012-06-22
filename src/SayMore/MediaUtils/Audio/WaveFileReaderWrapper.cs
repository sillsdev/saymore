using System;
using System.Diagnostics;
using System.IO;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SayMore.Media.Audio
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Interface to make testing easier
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IWaveStreamReader
	{
		/// ------------------------------------------------------------------------------------
		long SampleCount { get; }

		/// ------------------------------------------------------------------------------------
		/// The actual number of channels contained in the underlying data
		/// ------------------------------------------------------------------------------------
		int NativeChannelCount { get; }

		/// ------------------------------------------------------------------------------------
		/// The number of channels returned in the buffer when calling Read
		/// ------------------------------------------------------------------------------------
		int SamplingChannelCount { get; }

		/// ------------------------------------------------------------------------------------
		TimeSpan TotalTime { get; }

		/// ------------------------------------------------------------------------------------
		int BitsPerSample { get; }

		/// ------------------------------------------------------------------------------------
		WaveFormatEncoding Encoding { get; }

		/// ------------------------------------------------------------------------------------
		void Seek(long offset, SeekOrigin origin);

		/// ------------------------------------------------------------------------------------
		int Read(float[] buffer, int bufferSize);

		/// ------------------------------------------------------------------------------------
		void Close();
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class to support the reading of WAV files which adapts the underlying WaveFileReader to
	/// implement IWaveStreamReader.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WaveFileReaderWrapper : IWaveStreamReader, IDisposable
	{
		private readonly bool _createdReader;
		private WaveFileReader _reader;
		private SampleChannel _sampleChannel;

		/// ------------------------------------------------------------------------------------
		public WaveFileReaderWrapper(string audioPath)
		{
			_createdReader = true;
			_reader = new WaveFileReader(audioPath);
		}

		/// ------------------------------------------------------------------------------------
		public WaveFileReaderWrapper(WaveFileReader reader)
		{
			_createdReader = false;
			_reader = reader;
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_reader != null && _createdReader)
			{
				_reader.Dispose();
				_reader = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan TotalTime
		{
			get { return _reader.TotalTime; }
		}

		/// ------------------------------------------------------------------------------------
		public long SampleCount
		{
			get { return _reader.SampleCount; }
		}

		/// ------------------------------------------------------------------------------------
		/// The actual number of channels contained in the underlying data
		/// ------------------------------------------------------------------------------------
		public int NativeChannelCount
		{
			get { return _reader.WaveFormat.Channels; }
		}

		/// ------------------------------------------------------------------------------------
		/// The number of channels returned in the buffer when calling Read (can be higher than
		/// the NativeChannelCount because SampleChannel can magically turn mono inputs into
		/// stereo).
		/// ------------------------------------------------------------------------------------
		public int SamplingChannelCount
		{
			get
			{
				if (_sampleChannel == null)
					Seek(0, SeekOrigin.Begin);
				return _sampleChannel.WaveFormat.Channels;
			}
		}

		/// ------------------------------------------------------------------------------------
		public int BitsPerSample
		{
			get { return _reader.WaveFormat.BitsPerSample; }
		}

		/// ------------------------------------------------------------------------------------
		public WaveFormatEncoding Encoding
		{
			get { return _reader.WaveFormat.Encoding; }
		}

		/// ------------------------------------------------------------------------------------
		public void Seek(long offset, SeekOrigin origin)
		{
			_reader.Seek(offset, origin);
			_sampleChannel = new SampleChannel(_reader);
		}

		/// ------------------------------------------------------------------------------------
		public int Read(float[] buffer, int bufferSize)
		{
			Debug.Assert(_sampleChannel != null);
			return _sampleChannel.Read(buffer, 0, bufferSize);
		}

		/// ------------------------------------------------------------------------------------
		public void Close()
		{
			_sampleChannel = null;
			_reader.Close();
		}
	}
}
