using System;
using System.IO;
using NUnit.Framework;
using SayMore.Media.Audio;

namespace SayMoreTests.MediaUtils.Audio
{
	[TestFixture]
	public class AudioFileHelperTests
	{
		private AudioFileHelper _helper;
		private string _testAudioFileName;

		[SetUp]
		public void SetUp()
		{
			_testAudioFileName = MediaFileInfoTests.GetLongerTestAudioFile();
			_helper = new AudioFileHelper(_testAudioFileName);
		}

		[TearDown]
		public void TearDown()
		{
			if (_testAudioFileName != null && File.Exists(_testAudioFileName))
				File.Delete(_testAudioFileName);
		}

		[Test]
		public void AudioDuration_ReturnsCorrectValue()
		{
			Assert.AreEqual(TimeSpan.FromSeconds(56.775), _helper.AudioDuration);
		}

		[Test]
		public void GetSamples_Request123SamplesFrom1ChannelAudio_ReturnsSamplesFor1Channel()
		{
			var samples = _helper.GetSamples(123);
			Assert.AreEqual(123, samples.GetLength(0));
			Assert.AreEqual(1, samples.GetLength(1));
		}

		[Test]
		public void GetSamples_SampleCountLessThanRequested_ReturnsAllSamples()
		{
			var samples = _helper.GetSamples(int.MaxValue);
			Assert.AreEqual(2503778, samples.GetLength(0));
			Assert.AreEqual(1, samples.GetLength(1));
		}

		[Test]
		public void GetSamples_RequestZeroSamples_ReturnsNoSamples()
		{
			Assert.AreEqual(0, _helper.GetSamples(0).Length);
		}
	}
}
