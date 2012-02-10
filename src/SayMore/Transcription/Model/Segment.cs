
using System;
using System.Drawing;
using SayMore.UI.MediaPlayer;
using SilTools;

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

		///// ------------------------------------------------------------------------------------
		//public string GetCarefulSpeechFileName()
		//{
		//    return string.Format(Settings.Default.OralAnnotationSegmentFileFormat,
		//        (float)Start.TotalSeconds, (float)End.TotalSeconds,
		//        Settings.Default.OralAnnotationCarefulSegmentFileAffix);
		//}

		///// ------------------------------------------------------------------------------------
		//public string GetOralTranslationFileName()
		//{
		//    return string.Format(Settings.Default.OralAnnotationSegmentFileFormat,
		//        (float)Start.TotalSeconds, (float)End.TotalSeconds,
		//        Settings.Default.OralAnnotationTranslationSegmentFileAffix);
		//}

		/// ------------------------------------------------------------------------------------
		public void DrawPlaybackProgressBar(Graphics g, Rectangle rectangle,
			float playbackPosition, Color baseBackColor)
		{
			InternalDrawPlaybackProgressBar(g, rectangle,
				playbackPosition, baseBackColor, Start, End, GetLength(1));
		}

		/// ------------------------------------------------------------------------------------
		private static void InternalDrawPlaybackProgressBar(Graphics g, Rectangle rectangle,
			float playbackPosition, Color baseBackColor, float start, float end, float length)
		{
			// Draw bar indicating playback progress.
			var rc = rectangle;

			if (playbackPosition < Math.Round(end, 1, MidpointRounding.AwayFromZero))
			{
				var pixelsPerSec = rc.Width / length;
				rc.Width = (int)Math.Ceiling(pixelsPerSec * (playbackPosition - start));
			}

			if (rc.Width <= 0)
				return;

			rc.Height -= 6;
			rc.Y += 3;
			using (var br = new SolidBrush(ColorHelper.CalculateColor(Color.White, baseBackColor, 110)))
				g.FillRectangle(br, rc);
		}

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
