using System;
using NAudio.Wave;

namespace SayMore.Media.Audio
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class is adapted from NAudio WaveOffsetStream. It is simplified by removal of the
	/// initial start time that is unnecessary for our purposes and improved by correctly
	/// indicating when there is no more data to read rather than infinitely padding the buffer
	/// with zeroes.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WaveSegmentStream : WaveStream
	{
		private WaveStream _sourceStream;
		private readonly long _sourceOffsetBytes;
		private readonly long _length;
		private readonly int _bytesPerSample; // includes all channels
		private long _position;
		private readonly object _lockObject = new object();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This assumes the segment length is the source length - the start time. I.e. the
		/// end of the stream.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WaveSegmentStream(WaveStream sourceStream, TimeSpan startTime)
			: this(sourceStream, startTime, sourceStream.TotalTime - startTime)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new WaveOffsetStream
		/// </summary>
		/// <param name="sourceStream">the source stream</param>
		/// <param name="sourceOffset">amount to trim off the front of the source stream</param>
		/// <param name="sourceLength">length of time to play from source stream</param>
		/// ------------------------------------------------------------------------------------
		public WaveSegmentStream(WaveStream sourceStream, TimeSpan sourceOffset, TimeSpan sourceLength)
		{
			if (sourceStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
				throw new ArgumentException("Only PCM supported");
			// TODO: add support for IEEE float + perhaps some others -
			// anything with a fixed bytes per sample

			_sourceStream = sourceStream;
			_bytesPerSample = (sourceStream.WaveFormat.BitsPerSample / 8) * sourceStream.WaveFormat.Channels;
			_sourceOffsetBytes = (long) (sourceOffset.TotalSeconds * _sourceStream.WaveFormat.SampleRate) * _bytesPerSample;
			_length = (long) (sourceLength.TotalSeconds * _sourceStream.WaveFormat.SampleRate) * _bytesPerSample;
			Position = 0;
		}

		/// <summary>
		/// Gets the block alignment for this WaveStream
		/// </summary>
		public override int BlockAlign
		{
			get { return _sourceStream.BlockAlign; }
		}

		/// <summary>
		/// Returns the stream length
		/// </summary>
		public override long Length
		{
			get { return _length; }
		}

		/// <summary>
		/// Gets or sets the current position in the stream
		/// </summary>
		public override long Position
		{
			get { return _position; }
			set
			{
				lock (_lockObject)
				{
					if (value < 0)
						throw new ArgumentOutOfRangeException("value", "Position must be greater than or equal to 0");

					// make sure we don't get out of sync
					value -= (value % BlockAlign);
					_sourceStream.Position = _sourceOffsetBytes + value;
					_position = value;
				}
			}
		}

		/// <summary>
		/// Reads bytes from this wave stream
		/// </summary>
		/// <param name="destBuffer">The destination buffer</param>
		/// <param name="offset">Offset into the destination buffer</param>
		/// <param name="numBytes">Number of bytes read</param>
		/// <returns>Number of bytes read.</returns>
		public override int Read(byte[] destBuffer, int offset, int numBytes)
		{
			if (numBytes < 0)
				throw new ArgumentOutOfRangeException("numBytes", "Must be greater than or equal to 0.");

			lock (_lockObject)
			{
				// don't read too far into source stream
				int sourceBytesRequired = (int) Math.Min(numBytes, _length + _sourceOffsetBytes - _sourceStream.Position);
				int read = _sourceStream.Read(destBuffer, offset, sourceBytesRequired);

				_position += read;
				return read;
			}
		}

		/// <summary>
		/// <see cref="WaveStream.WaveFormat"/>
		/// </summary>
		public override WaveFormat WaveFormat
		{
			get { return _sourceStream.WaveFormat; }
		}

		/// <summary>
		/// Determines whether this channel has any data to play
		/// to allow optimisation to not read, but bump position forward
		/// </summary>
		public override bool HasData(int count)
		{
			if (count < 0)
				throw new ArgumentOutOfRangeException("count", "Count must be greater than or equal to 0.");

			if (_position >= _length)
				return false;
			// Check whether the source stream has data.
			// source stream should be in the right poisition
			return _sourceStream.HasData(count);
		}

		/// <summary>
		/// Disposes this WaveStream
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_sourceStream != null)
				{
					_sourceStream.Dispose();
					_sourceStream = null;
				}
			}
			else
			{
				System.Diagnostics.Debug.Assert(false, "WaveSegmentStream was not Disposed");
			}
			base.Dispose(disposing);
		}
	}
}
