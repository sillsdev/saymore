
using System;
using SayMore.UI.MediaPlayer;

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
			var length = GetLength();

			return MediaPlayerViewModel.GetRangeTimeDisplay(Start, (length.Equals(0f) ? 0 :
				(float)((decimal)Start + (decimal)length)));
		}
	}
}
