using System;
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shrinks button's font to the largest size that fits its own current ClientSize
		/// (minus the given margin), if the font ScaleUIFont already assigned doesn't fit.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ShrinkButtonFontToFit(Button button, int desiredScalePercent, 
			int margin = 8)
		{
			var maxBounds = new Size(
				Math.Max(0, button.ClientSize.Width - margin),
				Math.Max(0, button.ClientSize.Height - margin));

			button.Font = GetLargestFontThatFits(button.Text,
				UITextScaler.GetOriginalFont(button), desiredScalePercent, maxBounds);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Recursively finds every Button under root and shrinks its font to fit if the font a
		/// generic scaling pass already assigned overruns the button's own ClientSize (minus the
		/// given margin). This is intended primarily for use with controls we don't control (e.g.
		/// a Form from a third-party package). Applying GetLargestFontThatFits to every Button 
		/// costs nothing extra for any whose box already fits its scaled font (the binary search's
		/// first candidate is the full requested scale, so a Button that already fits is a same-
		/// font no-op), and the sweep self-heals automatically if a Button that once needed
		/// shrinking no longer does.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ShrinkOversizedButtonsToFit(Control root, int desiredScalePercent,
			int margin = 8)
		{
			foreach (Control child in root.Controls)
			{
				if (child is Button button)
					ShrinkButtonFontToFit(button, desiredScalePercent, margin);

				if (child.Controls.Count > 0)
					ShrinkOversizedButtonsToFit(child, desiredScalePercent, margin);
			}
		}
	}
}
