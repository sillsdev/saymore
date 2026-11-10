using System.Drawing;
using System.Windows.Forms;

namespace SayMore.Utilities
{
	/// ----------------------------------------------------------------------------------------
	public static class FontFitting
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Finds the largest font -- scaled from baseFont toward desiredScalePercent -- at
		/// which text still measures within maxBounds, via binary search. This lets a control
		/// grow as large as its real available space allows even when the full requested scale
		/// would overflow, rather than reverting all the way back to the unscaled baseline
		/// whenever the exact requested scale doesn't fit. A desiredScalePercent at or below
		/// 100% only ever shrinks or keeps baseFont's size, which -- being the original,
		/// already-working baseline -- needs no bounds check. If even that baseline doesn't fit
		/// within maxBounds, it's returned unchanged: there's nothing smaller left that would
		/// still represent it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Font GetLargestFontThatFits(string text, Font baseFont,
			int desiredScalePercent, Size maxBounds)
		{
			if (desiredScalePercent <= 100)
			{
				return desiredScalePercent == 100 ? baseFont :
					new Font(baseFont.FontFamily, baseFont.Size * desiredScalePercent / 100f, baseFont.Style, baseFont.Unit);
			}

			// Binary search for the largest percent in (100, desiredScalePercent] at which the
			// text still measures within maxBounds: fit is monotonic in font size (a larger
			// font never measures smaller), so this needs ~log2(desiredScalePercent - 100)
			// candidates instead of checking every integer percent down from
			// desiredScalePercent one at a time.
			var lo = 100;
			var hi = desiredScalePercent;
			Font best = null;
			while (lo < hi)
			{
				var mid = lo + (hi - lo + 1) / 2;
				var candidate = new Font(baseFont.FontFamily, baseFont.Size * mid / 100f, baseFont.Style, baseFont.Unit);
				var measured = TextRenderer.MeasureText(text, candidate);
				if (measured.Width <= maxBounds.Width && measured.Height <= maxBounds.Height)
				{
					// Only the font this method eventually returns should survive: every
					// rejected candidate holds a GDI font handle that needs to be released
					// explicitly rather than left for the finalizer.
					best?.Dispose();
					best = candidate;
					lo = mid;
				}
				else
				{
					candidate.Dispose();
					hi = mid - 1;
				}
			}

			return best ?? baseFont;
		}
	}
}
