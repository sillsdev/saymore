using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.UI;

namespace SayMoreTests.UI
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests for SplashScreenForm's extracted "largest font that fits" helper.
	///
	/// SplashScreenForm itself is never constructed here. GetLargestFontThatFits is a pure
	/// function that can be tested directly.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SplashScreenFormTests
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

			var result = SplashScreenForm.GetLargestFontThatFits(text, BaseFont,
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

			var result = SplashScreenForm.GetLargestFontThatFits(text, BaseFont,
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

			var result = SplashScreenForm.GetLargestFontThatFits(text, BaseFont,
				desiredScalePercent, maxBounds);

			Assert.That(result, Is.SameAs(BaseFont));
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void PositionBottomLabelsAndGetRequiredClientHeight_LabelsNeedMoreRoomThanUnscaledHeight_GrowsAndStacksLabels()
		{
			const int unscaledTop = 151;
			const int unscaledClientHeight = 189;
			const int margin = 8;

			// A Label never actually recalculates its AutoSize-preferred height unless it's
			// hosted in a Parent with an active layout engine -- confirmed empirically: a bare,
			// unparented Label stays at its default 23px Height regardless of Font or an
			// AutoSize off/on toggle, silently making any assertion about font-driven growth
			// a false positive.
			using (var host = new Panel())
			using (var topLabel = new Label { AutoSize = true, Font = new Font("Segoe UI", 24f), Text = "Copyright text" })
			using (var bottomLabel = new Label { AutoSize = true, Font = new Font("Segoe UI", 24f), Text = "Version text" })
			{
				host.Controls.Add(topLabel);
				host.Controls.Add(bottomLabel);

				var result = SplashScreenForm.PositionBottomLabelsAndGetRequiredClientHeight(
					topLabel, bottomLabel, unscaledTop, unscaledClientHeight, margin);

				Assert.That(topLabel.Top, Is.EqualTo(unscaledTop));
				Assert.That(bottomLabel.Top, Is.EqualTo(topLabel.Bottom + margin));
				Assert.That(result, Is.EqualTo(bottomLabel.Bottom + margin));
				Assert.That(result, Is.GreaterThan(unscaledClientHeight));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Unlike the other tests here, this one uses a generous synthetic unscaledClientHeight
		/// rather than SplashScreenForm's own real constants: those labels' real AutoSize
		/// preferred height (confirmed empirically, ~20px at their own 8.25pt Designer font)
		/// is already taller than the ~13px the Designer's hand-typed Size assumed, so no font
		/// size -- not even one far below the real minimum scale -- fits the real constants
		/// with room to spare. That mismatch is a separate, pre-existing concern; this test's
		/// job is only to confirm the floor-preserving behavior in isolation.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void PositionBottomLabelsAndGetRequiredClientHeight_LabelsFitWithinUnscaledHeight_ReturnsUnscaledHeightUnchanged()
		{
			const int unscaledTop = 10;
			const int unscaledClientHeight = 1000;
			const int margin = 8;

			using (var host = new Panel())
			using (var topLabel = new Label { AutoSize = true, Font = new Font("Segoe UI", 6f), Text = "x" })
			using (var bottomLabel = new Label { AutoSize = true, Font = new Font("Segoe UI", 6f), Text = "x" })
			{
				host.Controls.Add(topLabel);
				host.Controls.Add(bottomLabel);

				var result = SplashScreenForm.PositionBottomLabelsAndGetRequiredClientHeight(
					topLabel, bottomLabel, unscaledTop, unscaledClientHeight, margin);

				Assert.That(result, Is.EqualTo(unscaledClientHeight));
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void PositionBottomLabelsAndGetRequiredClientHeight_CalledAgainAtSmallerScale_ShrinksBackDown()
		{
			const int unscaledTop = 10;
			const int unscaledClientHeight = 1000;
			const int margin = 8;

			using (var host = new Panel())
			using (var topLabel = new Label { AutoSize = true, Font = new Font("Segoe UI", 24f), Text = "Copyright text" })
			using (var bottomLabel = new Label { AutoSize = true, Font = new Font("Segoe UI", 24f), Text = "Version text" })
			{
				host.Controls.Add(topLabel);
				host.Controls.Add(bottomLabel);

				SplashScreenForm.PositionBottomLabelsAndGetRequiredClientHeight(
					topLabel, bottomLabel, unscaledTop, unscaledClientHeight, margin);

				topLabel.Font = bottomLabel.Font = new Font("Segoe UI", 6f);
				var result = SplashScreenForm.PositionBottomLabelsAndGetRequiredClientHeight(
					topLabel, bottomLabel, unscaledTop, unscaledClientHeight, margin);

				Assert.That(result, Is.EqualTo(unscaledClientHeight));
			}
		}
	}
}
