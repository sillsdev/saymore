using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.UI;

namespace SayMoreTests.UI
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests for SplashScreenForm's bottom-label stacking/height-growth helper.
	///
	/// SplashScreenForm itself is never constructed here.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SplashScreenFormTests
	{
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
