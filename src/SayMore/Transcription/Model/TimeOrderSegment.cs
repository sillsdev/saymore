
using System;
using System.Drawing;
using SayMore.UI.MediaPlayer;
using SilTools;

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

		/// ------------------------------------------------------------------------------------
		public void DrawPlaybackProgressBar(Graphics g, Rectangle rectangle,
			float playbackPosition, Color baseBackColor)
		{
			// Draw bar indicating playback progress.
			var rc = rectangle;

			if (playbackPosition < Math.Round(Stop, 1, MidpointRounding.AwayFromZero))
			{
				var pixelsPerSec = rc.Width / GetLength(1);
				rc.Width = (int)Math.Round(pixelsPerSec * (playbackPosition - Start), MidpointRounding.AwayFromZero);
			}

			if (rc.Width <= 0)
				return;

			rc.Height -= 6;
			rc.Y += 3;
			using (var br = new SolidBrush(ColorHelper.CalculateColor(Color.White, baseBackColor, 110)))
				g.FillRectangle(br, rc);
		}
	}
}
