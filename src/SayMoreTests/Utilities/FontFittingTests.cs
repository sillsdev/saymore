using System.Drawing;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Utilities;

namespace SayMoreTests.Utilities
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>Tests for FontFitting's "largest font that fits" helper.</summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class FontFittingTests
	{
		private static readonly Font BaseFont = new Font("Segoe UI", 8.25F, FontStyle.Regular);

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetLargestFontThatFits_RequestedScaleFits_ReturnsRequestedScale()
		{
			const string text = "Loading...";
			const int desiredScalePercent = 150;

			// Plenty of room for the text at 150% of the base font.
			var maxBounds = new Size(1000, 1000);

			var result = FontFitting.GetLargestFontThatFits(text, BaseFont,
				desiredScalePercent, maxBounds);

			Assert.That(result.Size, Is.EqualTo(BaseFont.Size * desiredScalePercent / 100f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetLargestFontThatFits_RequestedScaleTooLarge_ReturnsLargestScaleThatFits()
		{
			const string text = "Loading...";
			const int desiredScalePercent = 200;

			// Measure how big the text is at the baseline (100%) and at the full desired
			// scale (200%), then pick a box that only the baseline is guaranteed to fit in,
			// so the helper is forced to step down from 200% to something smaller than 200%
			// but at least as large as 100%.
			var baselineSize = TextRenderer.MeasureText(text, BaseFont);
			var maxBounds = new Size(baselineSize.Width + 2, baselineSize.Height + 2);

			var result = FontFitting.GetLargestFontThatFits(text, BaseFont,
				desiredScalePercent, maxBounds);

			Assert.That(result.Size, Is.LessThan(BaseFont.Size * desiredScalePercent / 100f));
			Assert.That(result.Size, Is.GreaterThanOrEqualTo(BaseFont.Size));

			var resultMeasured = TextRenderer.MeasureText(text, result);
			Assert.That(resultMeasured.Width, Is.LessThanOrEqualTo(maxBounds.Width));
			Assert.That(resultMeasured.Height, Is.LessThanOrEqualTo(maxBounds.Height));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetLargestFontThatFits_EvenBaselineDoesNotFit_ReturnsBaselineUnchanged()
		{
			const string text = "Loading: a much longer status message than normal...";
			const int desiredScalePercent = 200;

			// A box far too small for the text even at the unscaled baseline font.
			var maxBounds = new Size(1, 1);

			var result = FontFitting.GetLargestFontThatFits(text, BaseFont,
				desiredScalePercent, maxBounds);

			Assert.That(result, Is.SameAs(BaseFont));
		}
	}
}
