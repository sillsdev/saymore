using System;
using NAudio.Wave;

namespace SayMore.AudioUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class is an NAudio WaveOffsetStream without the initial start time.
	/// That is unnecessary for our purposes.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WaveSegmentStream : WaveOffsetStream
	{
		/// ------------------------------------------------------------------------------------
		public WaveSegmentStream(WaveStream sourceStream, TimeSpan startTime, TimeSpan segmentlength)
			: base(sourceStream, TimeSpan.Zero, startTime, segmentlength)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This assumes the segment length is the source length - the start time. I.e. the
		/// end of the stream.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WaveSegmentStream(WaveStream sourceStream, TimeSpan startTime)
			: base(sourceStream, TimeSpan.Zero, startTime, sourceStream.TotalTime - startTime)
		{
		}
	}
}
