
using System;
using System.IO;
using SayMore.UI.MediaPlayer;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class Segment
	{
		public TierBase Tier { get; private set; }
		public string Text { get; set; }
		public float Start { get; set; }
		public float End { get; set; }

		/// ------------------------------------------------------------------------------------
		public Segment() : this(null)
		{
		}

		/// ------------------------------------------------------------------------------------
		public Segment(TierBase tier)
		{
			Tier = tier;
			Start = End = -1f;
		}

		/// ------------------------------------------------------------------------------------
		public Segment(TierBase tier, float start, float end) : this(tier)
		{
			Start = start;
			End = end;
		}

		/// ------------------------------------------------------------------------------------
		public Segment(TierBase tier, string text) : this(tier)
		{
			Text = text;
		}

		/// ------------------------------------------------------------------------------------
		public virtual Segment Copy(TierBase owningTier)
		{
			return new Segment(owningTier, Start, End) { Text = Text };
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the length of the segment in seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float GetLength()
		{
			return (End < Start ? 0f : (float)((decimal)End - (decimal)Start));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the length of the segment in seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float GetLength(int decimalPlaces)
		{
			return (float)Math.Round((decimal)End - (decimal)Start, decimalPlaces,
				MidpointRounding.AwayFromZero);
		}

		/// ------------------------------------------------------------------------------------
		public string GetPathToCarefulSpeechFile()
		{
			if (Tier == null || Tier.TierType != TierType.Time)
				return null;

			return Path.Combine(((TimeTier)Tier).SegmentFileFolder,
				TimeTier.ComputeFileNameForCarefulSpeechSegment(this));
		}

		///// ------------------------------------------------------------------------------------
		//public string GetPathToOralTranslationFile()
		//{
		//    if (Tier == null || Tier.TierType != TierType.Time)
		//        return null;

		//    return Path.Combine(((TimeTier)Tier).SegmentFileFolder,
		//        TimeTier.ComputeFileNameForOralTranslationSegment(this));
		//}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			if (Start >= 0f || End >= 0f)
			{
				var length = GetLength();

				return MediaPlayerViewModel.GetRangeTimeDisplay(Start, (length.Equals(0f) ? 0 :
					(float)((decimal)Start + (decimal)length)));
			}

			return Text;
		}
	}
}
