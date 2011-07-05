
using System;

namespace SayMore.Transcription.Model
{
	public class TimeOrderSegment : SegmentBase, ITimeOrderSegment
	{
		public float Start { get; private set; }
		public float Stop { get; private set; }

		/// ------------------------------------------------------------------------------------
		public TimeOrderSegment(ITier tier, float start, float stop) : base(tier)
		{
			Start = start;
			Stop = stop;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the length of the segment in seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float GetLength()
		{
			return (float)((decimal)Stop - (decimal)Start);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the length of the segment in seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float GetLength(int decimalPlaces)
		{
			return (float)Math.Round((decimal)Stop - (decimal)Start, decimalPlaces,
				MidpointRounding.AwayFromZero);
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return string.Format("Start={0};  Length={1}", Start, Stop);
		}
	}
}
