using System;
using SayMore.Media.MPlayer;

namespace SayMore.Transcription.Model
{
	public class TimeRange
	{
		public TimeSpan Start { get; set; }
		public TimeSpan End { get; set; }

		/// ------------------------------------------------------------------------------------
		public TimeRange(TimeSpan start, TimeSpan end)
		{
			Start = start;
			End = end;
		}

		/// ------------------------------------------------------------------------------------
		public TimeRange(float start, float end)
		{
			StartSeconds = start;
			EndSeconds = end;
		}

		/// ------------------------------------------------------------------------------------
		public float StartSeconds
		{
			get { return (float)Start.TotalSeconds; }
			set { Start = TimeSpan.FromSeconds(value); }
		}

		/// ------------------------------------------------------------------------------------
		public float EndSeconds
		{
			get { return (float)End.TotalSeconds; }
			set { End = TimeSpan.FromSeconds(value); }
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan Duration
		{
			get { return End - Start; }
		}

		/// ------------------------------------------------------------------------------------
		public float DurationSeconds
		{
			get { return Math.Max(EndSeconds - StartSeconds, 0f); }
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsTimeInRange(float seconds, bool includeStart, bool includeEnd)
		{
			if (includeStart && includeEnd)
				return (seconds >= StartSeconds && seconds <= EndSeconds);

			if (includeStart)
				return (seconds >= StartSeconds && seconds < EndSeconds);

			if (includeEnd)
				return (seconds > StartSeconds && seconds <= EndSeconds);

			return (seconds > StartSeconds && seconds < EndSeconds);
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return MediaPlayerViewModel.GetRangeTimeDisplay(StartSeconds, EndSeconds);
		}

		/// ------------------------------------------------------------------------------------
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(TimeRange)) return false;
			return Equals((TimeRange)obj);
		}

		/// ------------------------------------------------------------------------------------
		public bool Equals(TimeRange other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.Start.Equals(Start) && other.End.Equals(End);
		}

		/// ------------------------------------------------------------------------------------
		public bool Equals(TimeSpan start, TimeSpan end)
		{
			return Equals(new TimeRange(start, end));
		}

		/// ------------------------------------------------------------------------------------
		public override int GetHashCode()
		{
			unchecked
			{
				return (Start.GetHashCode() * 397) ^ End.GetHashCode();
			}
		}

		/// ------------------------------------------------------------------------------------
		public TimeRange Copy()
		{
			return new TimeRange(Start, End);
		}

		/// ------------------------------------------------------------------------------------
		public static bool operator ==(TimeRange left, TimeRange right)
		{
			return Equals(left, right);
		}

		/// ------------------------------------------------------------------------------------
		public static bool operator !=(TimeRange left, TimeRange right)
		{
			return !Equals(left, right);
		}

		/// ------------------------------------------------------------------------------------
		public static bool IsNullOrZeroLength(TimeRange timeRange)
		{
			return (timeRange == null || timeRange.Duration == default(TimeSpan));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>Non-inclusive</summary>
		/// ------------------------------------------------------------------------------------
		public bool Contains(TimeSpan time)
		{
			return (Start < time && End > time);
		}
	}
}
