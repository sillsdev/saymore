using System;
using System.IO;
using System.Linq;
using NAudio.Wave;
using NUnit.Framework;
using SayMore.Media.Audio;
using SayMore.Properties;
using SayMore.Transcription.Model;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class AutoSegmenterTests
	{
		private class DummyWaveStream : IWaveStreamReader
		{
			const float intTofloat = 1000000;

			int _channels;
			long _currentOffset = long.MaxValue;
			private float[] _samples;

			public TimeSpan TotalTime { get; set; }

			/// ------------------------------------------------------------------------------------
			public void SetSamples(long count, int channels, float maxHighSample, float minHighSample,
				Action<float[]> setSpecificValues)
			{
				_channels = channels;

				int maxHighSampleAsInt = (int)(maxHighSample * intTofloat);
				int minHighSampleAsInt = (int)(minHighSample * intTofloat);

				Random r = new Random();

				_samples = new float[count * channels];

				for (int i = 0; i < count * channels; i++)
				{
					float amplitude = r.Next(minHighSampleAsInt, maxHighSampleAsInt) / intTofloat;
					if (r.Next(1, 2) == 2)
						amplitude *= -1;

					_samples[i] = amplitude;
				}

				if (setSpecificValues != null)
					setSpecificValues(_samples);
			}

			/// ------------------------------------------------------------------------------------
			public long SampleCount
			{
				get { return _samples.Length / _channels; }
			}

			/// ------------------------------------------------------------------------------------
			public int ChannelCount
			{
				get { return _channels; }
			}

			/// ------------------------------------------------------------------------------------
			public int BitsPerSample
			{
				get { return 32; }
			}

			/// ------------------------------------------------------------------------------------
			public WaveFormatEncoding Encoding
			{
				get { return WaveFormatEncoding.IeeeFloat; }
			}

			/// ------------------------------------------------------------------------------------
			public void Seek(long offset, SeekOrigin origin)
			{
				switch (origin)
				{
					case SeekOrigin.Begin: _currentOffset = offset; break;
					case SeekOrigin.Current: _currentOffset += offset; break;
					case SeekOrigin.End: _currentOffset = offset; break;
				}
			}

			/// ------------------------------------------------------------------------------------
			public int Read(float[] buffer, int bufferSize)
			{
				int read = Math.Min(bufferSize, (int)(SampleCount * _channels - _currentOffset));
				Array.Copy(_samples, _currentOffset, buffer, 0, read);
				_currentOffset += read;
				return read;
			}

			/// ------------------------------------------------------------------------------------
			public void Close()
			{
			}
		}

		DummyWaveStream _sampleProvider;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_sampleProvider = new DummyWaveStream();
		}

		#region GetNaturalBreaks tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNaturalBreaks_ZeroSamples_ReturnsEmptyEnumeration()
		{
			_sampleProvider.SetSamples(0, 1, 0, 0, null);
			var segmenter = new AutoSegmenter(_sampleProvider);
			Assert.AreEqual(0, segmenter.GetNaturalBreaks().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNaturalBreaks_ShortWavFileWith1000IdenticalNonZeroSamples_ReturnsSingleBreakAtEnd()
		{
			_sampleProvider.SetSamples(1000, 1, 0.999f, 0.999f, null);
			var duration = TimeSpan.FromSeconds(3);
			_sampleProvider.TotalTime = duration;

			Settings.Default.MaximumSegmentLengthInMilliseconds = 6000;

			var segmenter = new AutoSegmenter(_sampleProvider);
			var breaks = segmenter.GetNaturalBreaks().ToList();
			Assert.AreEqual(1, breaks.Count());
			Assert.AreEqual(duration, breaks[0]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNaturalBreaks_ShortWavFileWithIdenticalNonZeroSamplesPlusZeroSampleAtEnd_ReturnsSingleBreakAtEnd()
		{
			_sampleProvider.SetSamples(1000, 1, 0.999f, 0.999f, s => s[999] = 0);

			var duration = TimeSpan.FromSeconds(3);
			_sampleProvider.TotalTime = duration;

			Settings.Default.MaximumSegmentLengthInMilliseconds = 6000;

			var segmenter = new AutoSegmenter(_sampleProvider);
			var breaks = segmenter.GetNaturalBreaks().ToList();
			Assert.AreEqual(1, breaks.Count());
			Assert.AreEqual(duration, breaks[0]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNaturalBreaks_3SecondsWithTwoFixedSingleInternalLowsMaxSegment1200Milliseconds_ReturnsTwoInternalBreaksAndOneAtEnd()
		{
			const int totalSamples = 6000;
			var duration = TimeSpan.FromSeconds(3);
			_sampleProvider.SetSamples(totalSamples, 1, 0.999f, 0.999f, s =>
			{
				s[2000] = 0;
				s[4000] = 0;
			});
			_sampleProvider.TotalTime = duration;

			Settings.Default.AutoSegmenterPreferrerdPauseLength = TimeSpan.FromMilliseconds(10);
			Settings.Default.MaximumSegmentLengthInMilliseconds = 1200;

			var segmenter = new AutoSegmenter(_sampleProvider);
			var breaks = segmenter.GetNaturalBreaks().ToArray();
			Assert.AreEqual(3, breaks.Length);
			Assert.AreEqual(TimeSpan.FromSeconds(1), breaks[0]);
			Assert.AreEqual(TimeSpan.FromSeconds(2), breaks[1]);
			Assert.AreEqual(TimeSpan.FromSeconds(3), breaks[2]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNaturalBreaks_3SecondsWithTwoFixedShortInternalLowsMaxSegment2Seconds_ReturnsTwoInternalBreaksAndOneAtEnd()
		{
			const int totalSamples = 6000;
			var duration = TimeSpan.FromSeconds(3);
			_sampleProvider.SetSamples(totalSamples, 1, 0.999f, 0.999f, s =>
			{
				s[1999] = 0;
				s[2000] = 0;
				s[2001] = 0;
				s[3999] = 0;
				s[4000] = 0;
				s[4001] = 0;
			});
			_sampleProvider.TotalTime = duration;

			Settings.Default.AutoSegmenterPreferrerdPauseLength = TimeSpan.FromMilliseconds(10);
			Settings.Default.MaximumSegmentLengthInMilliseconds = 2000;
			var segmenter = new AutoSegmenter(_sampleProvider);
			var breaks = segmenter.GetNaturalBreaks().ToArray();
			Assert.AreEqual(3, breaks.Length);
			Assert.AreEqual(TimeSpan.FromSeconds(1), breaks[0]);
			Assert.AreEqual(TimeSpan.FromSeconds(2), breaks[1]);
			Assert.AreEqual(TimeSpan.FromSeconds(3), breaks[2]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNaturalBreaks_3SecondsWithTwoFixedShortInternalLowsMaxSegment2200Milliseconds_ReturnsTwoInternalBreaksAndOneAtEnd()
		{
			const int totalSamples = 6000;
			var duration = TimeSpan.FromSeconds(3);
			_sampleProvider.SetSamples(totalSamples, 1, 0.999f, 0.999f, s =>
			{
				s[1999] = 0;
				s[2000] = 0;
				s[2001] = 0;
				s[3999] = 0;
				s[4000] = 0;
				s[4001] = 0;
			});
			_sampleProvider.TotalTime = duration;

			Settings.Default.AutoSegmenterPreferrerdPauseLength = TimeSpan.FromMilliseconds(10);
			Settings.Default.MaximumSegmentLengthInMilliseconds = 2200;

			var segmenter = new AutoSegmenter(_sampleProvider);
			var breaks = segmenter.GetNaturalBreaks().ToArray();
			Assert.AreEqual(2, breaks.Length);
			Assert.AreEqual(TimeSpan.FromSeconds(2), breaks[0]);
			Assert.AreEqual(TimeSpan.FromSeconds(3), breaks[1]);
		}


		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNaturalBreaks_12SecondsWithTwoVariableMultiSampleInternalLows_ReturnsTwoInternalBreaksAndOneAtEnd()
		{
			const int totalSamples = 12000;
			var duration = TimeSpan.FromSeconds(12);
			_sampleProvider.SetSamples(totalSamples, 1, 0.999f, 0.2f, s =>
			{
				s[4995] = 0.001f;
				s[4996] = 0.09f;
				s[4997] = 0.03f;
				s[4998] = 0.02f;
				s[4999] = 0.009f;
				s[5000] = 0.08f;
				s[5001] = 0.15f;
				s[5002] = 0.09f;
				s[5003] = 0.04f;
				s[5004] = 0.007f;
				s[5005] = 0.03f;

				s[8995] = 0.003f;
				s[8996] = 0.07f;
				s[8997] = 0.04f;
				s[8998] = 0.08f;
				s[8999] = 0.09f;
				s[9000] = 0.01f;
				s[9001] = 0.01f;
				s[9002] = 0.09f;
				s[9003] = 0.009f;
				s[9004] = 0.007f;
				s[9005] = 0.006f;
			});

			_sampleProvider.TotalTime = duration;

			Settings.Default.AutoSegmenterPreferrerdPauseLength = TimeSpan.FromMilliseconds(10);
			Settings.Default.MaximumSegmentLengthInMilliseconds = 6000;

			var segmenter = new AutoSegmenter(_sampleProvider);
			var breaks = segmenter.GetNaturalBreaks().ToArray();
			Assert.AreEqual(3, breaks.Length);
			Assert.IsTrue(TimeSpan.FromMilliseconds(4998) <= breaks[0]);
			Assert.IsTrue(TimeSpan.FromMilliseconds(5002) >= breaks[0]);
			Assert.IsTrue(TimeSpan.FromMilliseconds(8998) <= breaks[1]);
			Assert.IsTrue(TimeSpan.FromMilliseconds(9002) >= breaks[1]);
			Assert.AreEqual(TimeSpan.FromSeconds(12), breaks[2]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNaturalBreaks_12SecondsMultiChannelWithFixedMultiSampleLowsEvery500MillisecondsMaxSegment6Seconds_ReturnsThreeInternalBreaksAndOneAtEnd()
		{
			const int totalSamples = 6000;
			var duration = TimeSpan.FromSeconds(12);

			_sampleProvider.SetSamples(totalSamples, 2, 0.999f, 0.999f, s =>
			{
				for (int i = 0; i < 22; i++)
					s[i] = 0;

				for (int b = 1; b < 23; b++)
				{
					for (int i = 0; i < 22; i++)
					{
						s[b * 500 - 10 + i] = 0;
						s[b * 500 - 10 + i] = 0;
					}
				}

				for (int i = 11980; i < 12000; i++)
					s[i] = 0;
			});

			_sampleProvider.TotalTime = duration;

			Settings.Default.MaximumSegmentLengthInMilliseconds = 6000;

			var segmenter = new AutoSegmenter(_sampleProvider);
			var breaks = segmenter.GetNaturalBreaks().ToArray();
			Assert.AreEqual(3, breaks.Length, "Breaks are expected roughly halfway between the min and max segment lengths, not at every pause");
			Assert.AreEqual(TimeSpan.FromMilliseconds(3500), breaks[0]);
			Assert.AreEqual(TimeSpan.FromSeconds(7), breaks[1]);
			Assert.AreEqual(TimeSpan.FromSeconds(12), breaks[2]);
		}
		#endregion
	}
}
