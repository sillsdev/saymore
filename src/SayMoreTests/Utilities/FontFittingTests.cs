using System.Drawing;
using System.Threading;
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
		/// <summary>
		/// Unlike the "too large" test above (whose box only ever fits the 100% baseline),
		/// this box is sized to fit an intermediate scale, so a degenerate implementation that
		/// always just returned baseFont unchanged would wrongly pass that test but fails this
		/// one: the result must actually grow past the baseline, not fall back to it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetLargestFontThatFits_IntermediateScaleFitsButFullScaleDoesNot_ReturnsScaleStrictlyLargerThanBase()
		{
			const string text = "Loading...";
			const int desiredScalePercent = 200;

			using (var midScaleFont = new Font(BaseFont.FontFamily, BaseFont.Size * 1.5f, BaseFont.Style))
			{
				var midScaleSize = TextRenderer.MeasureText(text, midScaleFont);
				var maxBounds = new Size(midScaleSize.Width + 2, midScaleSize.Height + 2);

				var result = FontFitting.GetLargestFontThatFits(text, BaseFont,
					desiredScalePercent, maxBounds);

				Assert.That(result.Size, Is.GreaterThan(BaseFont.Size));
				Assert.That(result.Size, Is.LessThan(BaseFont.Size * desiredScalePercent / 100f));
			}
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

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ShrinkButtonFontToFit_ScaledFontWouldOverflowButton_ShrinksFontToFitExistingBox()
		{
			const int margin = 8;

			using (var host = new Panel())
			using (var button = new Button
			{
				Text = "Some Long Caption",
				Font = new Font("Segoe UI", 8.25f),
				Size = new Size(128, 23)
			})
			{
				host.Controls.Add(button);
				var unscaledFont = button.Font;

				FontFitting.ShrinkButtonFontToFit(button, 200, margin);

				Assert.That(button.Font.Size, Is.LessThan(unscaledFont.Size * 2f),
					"The button's own fixed box can't fit the full 200% scale, so the font " +
					"must shrink below the full requested scale rather than overflowing it.");

				var measured = TextRenderer.MeasureText(button.Text, button.Font);
				Assert.That(measured.Width, Is.LessThanOrEqualTo(button.ClientSize.Width - margin));
				Assert.That(measured.Height, Is.LessThanOrEqualTo(button.ClientSize.Height - margin));
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ShrinkButtonFontToFit_ScaledFontAlreadyFits_LeavesFontAtFullRequestedScale()
		{
			const int margin = 8;

			using (var host = new Panel())
			using (var button = new Button
			{
				Text = "OK",
				Font = new Font("Segoe UI", 8.25f),
				Size = new Size(300, 100)
			})
			{
				host.Controls.Add(button);
				var unscaledFont = button.Font;

				FontFitting.ShrinkButtonFontToFit(button, 150, margin);

				Assert.That(button.Font.Size, Is.EqualTo(unscaledFont.Size * 1.5f));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Mirrors how a container nests its children in practice -- the oversized button
		/// isn't a direct child of root, and the sweep finds it without being told its name or
		/// where it lives. A sibling button whose box already fits is left alone (same font it
		/// already had), confirming the sweep doesn't perturb controls that don't need fixing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ShrinkOversizedButtonsToFit_OversizedButtonNestedInChildPanel_FindsAndShrinksItWithoutBeingNamed()
		{
			using (var root = new Panel())
			using (var nestedPanel = new Panel())
			using (var oversizedButton = new Button
			{
				Text = "Some Long Caption",
				Font = new Font("Segoe UI", 8.25f),
				Size = new Size(128, 23)
			})
			using (var fittingButton = new Button
			{
				Text = "OK",
				Font = new Font("Segoe UI", 8.25f),
				Size = new Size(300, 100)
			})
			{
				root.Controls.Add(nestedPanel);
				nestedPanel.Controls.Add(oversizedButton);
				root.Controls.Add(fittingButton);
				var unscaledFont = oversizedButton.Font;

				// Cache each button's true original font before bumping it, the same order
				// ScaleUIFont's own recursive walk follows (cache originals, then apply the
				// new scaled font) -- otherwise ShrinkButtonFontToFit's internal
				// GetOriginalFont call would cache the already-scaled font as if it were the
				// baseline.
				UITextScaler.GetOriginalFont(oversizedButton);
				UITextScaler.GetOriginalFont(fittingButton);

				// Simulate ScaleUIFont's generic font-only pass already having run.
				oversizedButton.Font = new Font(unscaledFont.FontFamily, unscaledFont.Size * 2f, unscaledFont.Style);
				fittingButton.Font = new Font(unscaledFont.FontFamily, unscaledFont.Size * 2f, unscaledFont.Style);

				FontFitting.ShrinkOversizedButtonsToFit(root, 200);

				Assert.That(oversizedButton.Font.Size, Is.LessThan(unscaledFont.Size * 2f),
					"The nested, oversized button must be found and shrunk.");
				Assert.That(fittingButton.Font.Size, Is.EqualTo(unscaledFont.Size * 2f),
					"A button whose box already fits the full requested scale must be left at " +
					"that full scale, not shrunk.");
			}
		}
	}
}
