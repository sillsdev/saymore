
using System;
using System.IO;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class Segment
	{
		public TierBase Tier { get; private set; }
		public string Text { get; set; }

		private TimeRange _timeRange;

		/// ------------------------------------------------------------------------------------
		public Segment(TierBase tier)
		{
			Tier = tier;
			TimeRange = null;
		}

		/// ------------------------------------------------------------------------------------
		public Segment() : this(null)
		{
		}

		/// ------------------------------------------------------------------------------------
		public Segment(TierBase tier, TimeRange timeRange) : this(tier)
		{
			TimeRange = timeRange;
		}

		/// ------------------------------------------------------------------------------------
		public Segment(TierBase tier, float start, float end) : this(tier, new TimeRange(start, end))
		{
		}

		/// ------------------------------------------------------------------------------------
		public Segment(TierBase tier, string text) : this(tier)
		{
			Text = text;
		}

		/// ------------------------------------------------------------------------------------
		public TimeRange TimeRange
		{
			get { return _timeRange; }
			set { _timeRange = (value ?? new TimeRange(-1f, -1f)); }
		}

		/// ------------------------------------------------------------------------------------
		public float Start
		{
			get { return TimeRange.StartSeconds; }
			set { TimeRange.StartSeconds = value; }
		}

		/// ------------------------------------------------------------------------------------
		public float End
		{
			get { return TimeRange.EndSeconds; }
			set { TimeRange.EndSeconds = value; }
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
			return TimeRange.DurationSeconds;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the length of the segment in seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float GetLength(int decimalPlaces)
		{
			return (float)Math.Round(TimeRange.DurationSeconds, decimalPlaces, MidpointRounding.AwayFromZero);
		}

		/// ------------------------------------------------------------------------------------
		public string GetFullPathToCarefulSpeechFile()
		{
			return (Tier == null || Tier.TierType != TierType.Time ? null :
				((TimeTier)Tier).GetFullPathToCarefulSpeechFile(this));
		}

		/// ------------------------------------------------------------------------------------
		public string GetFullPathToOralTranslationFile()
		{
			return (Tier == null || Tier.TierType != TierType.Time ? null :
				((TimeTier)Tier).GetFullPathToOralTranslationFile(this));
		}

		/// ------------------------------------------------------------------------------------
		public bool GetHasOralAnnotation()
		{
			var pathToCarefulSpeechFile = GetFullPathToCarefulSpeechFile();
			var pathToOralTranslationFile = GetFullPathToOralTranslationFile();
			return ((pathToCarefulSpeechFile != null && File.Exists(pathToCarefulSpeechFile)) ||
				(pathToOralTranslationFile != null && File.Exists(pathToOralTranslationFile)));
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return (TimeRange.StartSeconds >= 0f || TimeRange.EndSeconds >= 0f ?
				TimeRange.ToString() : Text);
		}
	}
}
