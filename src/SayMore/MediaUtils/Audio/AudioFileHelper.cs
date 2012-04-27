using System;
using System.IO;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SayMore.Media.Audio
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class to cache the duration and agregated samples (see comment on GetSamples) for a
	/// sound file.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AudioFileHelper
	{
		private Tuple<float, float>[,] _cachedSamples;
		public string AudioFilePath { get; private set; }
		private TimeSpan _audioDuration;

		/// ------------------------------------------------------------------------------------
		public AudioFileHelper(string audioFilePath)
		{
			AudioFilePath = audioFilePath;
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan AudioDuration
		{
			get
			{
				if (_audioDuration == default(TimeSpan))
				{
					using (WaveFileReader stream = new WaveFileReader(AudioFilePath))
					{
						_audioDuration = stream.TotalTime;
						stream.Close();
					}
				}
				return _audioDuration;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will return a two-dimensional array of min and max samples, where the
		/// first dimension is the number of samples requested (assuming the file has enough)
		/// and the second dimension is the number of channels.
		///
		/// Since the number of samples requested will almost always be less than the number
		/// found in the file, the values in the file will be aggregated to provide a min and
		/// max across multiple contiguous samples.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Tuple<float, float>[,] GetSamples(uint numberOfSamplesToReturn)
		{
			if (_cachedSamples == null || _cachedSamples.Length != numberOfSamplesToReturn)
			{
				using (var stream = new WaveFileReader(AudioFilePath))
				{
					_audioDuration = stream.TotalTime;
					_cachedSamples = GetSamples(stream, numberOfSamplesToReturn);
					stream.Close();
				}
			}

			return _cachedSamples;
		}

		/// ------------------------------------------------------------------------------------
		public static Tuple<float, float>[,] GetSamples(WaveFileReader stream,
			uint numberOfSamplesToReturn)
		{
			if (numberOfSamplesToReturn == 0 ||
				(stream.WaveFormat.BitsPerSample == 32 && stream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat))
			{
				return new Tuple<float, float>[0, 0];
			}

			var channels = stream.WaveFormat.Channels;
			var sampleCount = stream.SampleCount;
			if (sampleCount < numberOfSamplesToReturn)
				numberOfSamplesToReturn = (uint)sampleCount;

			var samplesPerAggregate = sampleCount / (double)numberOfSamplesToReturn;

			var samplesToReturn = new Tuple<float, float>[numberOfSamplesToReturn, channels];

			stream.Seek(0, SeekOrigin.Begin);
			var provider = new SampleChannel(stream);

			int bufferSize = (int)samplesPerAggregate * provider.WaveFormat.Channels;
			var buffer = new float[bufferSize];

			int sampleIndex = 0;
			int read;
			while (sampleIndex < numberOfSamplesToReturn && (read = provider.Read(buffer, 0, bufferSize)) > 0)
			{
				for (var c = 0; c < channels; c++)
				{
					var biggestSample = float.MinValue;
					var smallestSample = float.MaxValue;

					for (int i = 0; i < read; i += provider.WaveFormat.Channels)
					{
						biggestSample = Math.Max(biggestSample, buffer[i + c]);
						smallestSample = Math.Min(smallestSample, buffer[i + c]);
					}

					samplesToReturn[sampleIndex, c] = new Tuple<float, float>(biggestSample, smallestSample);
				}

				sampleIndex++;
			}

			return samplesToReturn;
		}
	}
}
