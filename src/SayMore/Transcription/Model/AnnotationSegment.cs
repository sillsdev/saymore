
using System;
using System.IO;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class AnnotationSegment
	{
		public TierBase Tier { get; private set; }
		public string Text { get; set; }

		private TimeRange _timeRange;

		/// ------------------------------------------------------------------------------------
		public AnnotationSegment(TierBase tier)
		{
			Tier = tier;
			TimeRange = null;
		}

		/// ------------------------------------------------------------------------------------
		public AnnotationSegment() : this(null)
		{
		}

		/// ------------------------------------------------------------------------------------
		public AnnotationSegment(TierBase tier, TimeRange timeRange) : this(tier)
		{
			TimeRange = timeRange;
		}

		/// ------------------------------------------------------------------------------------
		public AnnotationSegment(TierBase tier, float start, float end) : this(tier, new TimeRange(start, end))
		{
		}

		/// ------------------------------------------------------------------------------------
		public AnnotationSegment(TierBase tier, string text) : this(tier)
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
		public bool StartsAt(float boundary)
		{
			// We need to use TimeSpan to do the comparison since floats have a lot more
			// preceision than TimeSpan -- two floats that are very nearly (but not quite)
			// equal will result in the same time span, so we want to consider them as equal.
			return TimeRange.Start.Equals(TimeSpan.FromSeconds(boundary));
		}

		/// ------------------------------------------------------------------------------------
		public bool EndsAt(float boundary)
		{
			// We need to use TimeSpan to do the comparison since floats have a lot more
			// preceision than TimeSpan -- two floats that are very nearly (but not quite)
			// equal will result in the same time span, so we want to consider them as equal.
			return TimeRange.End.Equals(TimeSpan.FromSeconds(boundary));
		}

		/// ------------------------------------------------------------------------------------
		public virtual AnnotationSegment Copy(TierBase owningTier)
		{
			return new AnnotationSegment(owningTier, Start, End) { Text = Text };
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
		public bool GetHasOralAnnotation(OralAnnotationType type)
		{
			var pathToOralAnnotation = (type == OralAnnotationType.CarefulSpeech ?
				GetFullPathToCarefulSpeechFile() : GetFullPathToOralTranslationFile());
			return (pathToOralAnnotation != null && File.Exists(pathToOralAnnotation));
		}

		/// ------------------------------------------------------------------------------------
		public bool GetHasAnyOralAnnotation()
		{
			return GetHasOralAnnotation(OralAnnotationType.CarefulSpeech) ||
				GetHasOralAnnotation(OralAnnotationType.Translation);
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return (TimeRange.StartSeconds >= 0f || TimeRange.EndSeconds >= 0f ?
				TimeRange.ToString() : Text);
		}
	}
}
