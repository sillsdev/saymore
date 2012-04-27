using System.IO;
using NAudio.Wave;
using NUnit.Framework;
using SayMore.Media.Audio;

namespace SayMoreTests.Media.Audio
{
	[TestFixture]
	public class OralAnnotationPlaybackWaveStreamTests
	{
		private OralAnnotationPlaybackWaveStream _stream;
		private MemoryStream _memstream;

		/// ------------------------------------------------------------------------------------
		private void CreateStream(int channels)
		{
			_memstream = new MemoryStream();

			// Simulate a block align of 12 bytes (2 blocks, 3 channels,
			// 2 bytes per channel, 6 bytes per block)
			_memstream.Write(new byte[] { 0, 1, 2, 4, 8, 16, 32, 64, 128, 148, 152, 156 }, 0, 12);

			if (channels == 4)
				_memstream.Write(new byte[] { 162, 168, 174, 178 }, 0, 4);

			_memstream.Position = 0;

			_stream = new OralAnnotationPlaybackWaveStream(
				new RawSourceWaveStream(_memstream, new WaveFormat(44100, 16, channels)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void LengthProperty_3ChannelSource_ReturnsOneThirdOfBaseStream()
		{
			CreateStream(3);
			Assert.AreEqual(_memstream.Length / 3, _stream.Length);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void LengthProperty_4ChannelSource_ReturnsHalfOfBaseStream()
		{
			CreateStream(4);
			Assert.AreEqual(_memstream.Length / 2, _stream.Length);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PositionProperty_3ChannelSource_ReturnsOneThirdOfBaseStreamValue()
		{
			CreateStream(3);
			_memstream.Position = 9;
			Assert.AreEqual(3, _stream.Position);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PositionProperty_4ChannelSource_ReturnsHalfOfBaseStreamValue()
		{
			CreateStream(4);
			_memstream.Position = 12;
			Assert.AreEqual(6, _stream.Position);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Construct_3ChannelSource_Creates1ChannelStream()
		{
			CreateStream(3);
			Assert.AreEqual(1, _stream.WaveFormat.Channels);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Construct_4ChannelSource_Creates1ChannelStream()
		{
			CreateStream(4);
			Assert.AreEqual(2, _stream.WaveFormat.Channels);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void MergeChannels_3ChannelSource_ReturnsCorrectBufferContents()
		{
			CreateStream(3);

			var block = new byte[] { 0, 1, 2, 4, 8, 16 };
			var outputBuffer = new byte[2];

			_stream.MergeChannels(outputBuffer, 0, block);

			Assert.AreEqual(0 | 2 | 8, outputBuffer[0]);
			Assert.AreEqual(1 | 4 | 16, outputBuffer[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void MergeChannels_4ChannelSource_ReturnsCorrectBufferContents()
		{
			CreateStream(4);

			var block = new byte[] { 0, 1, 2, 4, 8, 16, 32, 64 };
			var outputBuffer = new byte[4];

			_stream.MergeChannels(outputBuffer, 0, block);

			Assert.AreEqual(0 | 8 | 32, outputBuffer[0]);
			Assert.AreEqual(1 | 16 | 64, outputBuffer[1]);
			Assert.AreEqual(2 | 8 | 32, outputBuffer[2]);
			Assert.AreEqual(4 | 16 | 64, outputBuffer[3]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Read_3ChannelSource_ReturnsCorrectBufferContents()
		{
			CreateStream(3);

			var buffer = new byte[2];

			// Read first block
			Assert.AreEqual(2, _stream.Read(buffer, 0, 2));
			Assert.AreEqual(0 | 2 | 8, buffer[0]);
			Assert.AreEqual(1 | 4 | 16, buffer[1]);

			// Read second block
			Assert.AreEqual(2, _stream.Read(buffer, 0, 2));
			Assert.AreEqual(32 | 128 | 152, buffer[0]);
			Assert.AreEqual(64 | 148 | 156, buffer[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Read_4ChannelSource_ReturnsCorrectBufferContents()
		{
			CreateStream(4);

			var buffer = new byte[4];

			// Read first block
			Assert.AreEqual(2, _stream.Read(buffer, 0, 2));
			Assert.AreEqual(0 | 8 | 32, buffer[0]);
			Assert.AreEqual(1 | 16 | 64, buffer[1]);
			Assert.AreEqual(2 | 8 | 32, buffer[2]);
			Assert.AreEqual(4 | 16 | 64, buffer[3]);

			// Read second block
			Assert.AreEqual(2, _stream.Read(buffer, 0, 2));
			Assert.AreEqual(128 | 162 | 174, buffer[0]);
			Assert.AreEqual(148 | 168 | 178, buffer[1]);
			Assert.AreEqual(152 | 162 | 174, buffer[2]);
			Assert.AreEqual(156 | 168 | 178, buffer[3]);
		}
	}
}
