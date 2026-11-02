using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Properties;
using SayMore.Utilities;

namespace SayMoreTests.Utilities
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class UITextScalerTests
	{
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void NotifyScaleChanged_SubscriberRegistered_SubscriberIsInvoked()
		{
			var wasCalled = false;
			EventHandler handler = (s, e) => wasCalled = true;

			UITextScaler.ScaleChanged += handler;
			try
			{
				UITextScaler.NotifyScaleChanged();
				Assert.IsTrue(wasCalled);
			}
			finally
			{
				UITextScaler.ScaleChanged -= handler;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void NotifyScaleChanged_NoSubscribers_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => UITextScaler.NotifyScaleChanged());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void NotifyScaleChanged_SubscriberUnregistered_SubscriberIsNotInvoked()
		{
			var wasCalled = false;
			EventHandler handler = (s, e) => wasCalled = true;

			UITextScaler.ScaleChanged += handler;
			UITextScaler.ScaleChanged -= handler;

			UITextScaler.NotifyScaleChanged();

			Assert.IsFalse(wasCalled);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ClampScalePercent_ValueWithinRange_ReturnsSameValue()
		{
			Assert.AreEqual(150, UITextScaler.ClampScalePercent(150));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ClampScalePercent_ValueZero_ReturnsMinimum()
		{
			Assert.AreEqual(UITextScaler.kMinScalePercent, UITextScaler.ClampScalePercent(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ClampScalePercent_NegativeValue_ReturnsMinimum()
		{
			Assert.AreEqual(UITextScaler.kMinScalePercent, UITextScaler.ClampScalePercent(-50));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ClampScalePercent_ValueBelowMinimum_ReturnsMinimum()
		{
			Assert.AreEqual(UITextScaler.kMinScalePercent,
				UITextScaler.ClampScalePercent(UITextScaler.kMinScalePercent - 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ClampScalePercent_ValueAboveMaximum_ReturnsMaximum()
		{
			Assert.AreEqual(UITextScaler.kMaxScalePercent,
				UITextScaler.ClampScalePercent(UITextScaler.kMaxScalePercent + 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ClampScalePercent_RidiculouslyLargeValue_ReturnsMaximum()
		{
			Assert.AreEqual(UITextScaler.kMaxScalePercent, UITextScaler.ClampScalePercent(int.MaxValue));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRecommendedScalePercent_Tibetan_ReturnsLargerThanDefault()
		{
			var result = UITextScaler.GetRecommendedScalePercent("bo");
			Assert.IsTrue(result.HasValue);
			Assert.Greater(result.Value, 100);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRecommendedScalePercent_LanguageWithoutRecommendation_ReturnsNull()
		{
			Assert.IsNull(UITextScaler.GetRecommendedScalePercent("en"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRecommendedScalePercent_NullLanguageCode_ReturnsNull()
		{
			Assert.IsNull(UITextScaler.GetRecommendedScalePercent(null));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Guards against a Debug.Fail -- which pops a blocking dialog, even under an automated
		/// NUnit run -- when a control's cached original font no longer matches its current font
		/// family/style on a redundant scaling pass (e.g. a scale-changed notification firing
		/// even though the percentage didn't actually change, or a mixed-family Designer
		/// hierarchy). The fix: treat a mismatch as the control's font legitimately changing out
		/// from under us, not a bug to flag.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void GetOriginalFont_ControlsCurrentFontFamilyDiffersFromCachedOriginal_DoesNotThrowAndRecachesCurrentFont()
		{
			using (var control = new Label { Font = new Font(FontFamily.GenericSansSerif, 9f) })
			{
				var cached = UITextScaler.GetOriginalFont(control);
				Assert.AreEqual(FontFamily.GenericSansSerif, cached.FontFamily);

				control.Font = new Font(FontFamily.GenericSerif, 12f);

				Font recached = null;
				Assert.DoesNotThrow(() => recached = UITextScaler.GetOriginalFont(control));
				Assert.AreEqual(FontFamily.GenericSerif, recached.FontFamily);

				// A third call with the font unchanged should now hit the fast (cached) path.
				var thirdCall = UITextScaler.GetOriginalFont(control);
				Assert.AreEqual(FontFamily.GenericSerif, thirdCall.FontFamily);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Guards against reconstructing a font from its family, size, and style while
		/// discarding its GraphicsUnit. A control's baseline font can be declared in a unit
		/// other than Point (e.g. World, as the 3-argument Font constructor's default would
		/// silently convert to); losing that unit re-expresses the same numeric size in a
		/// different unit, inflating or shrinking the rendered text even at a neutral 100% scale.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ControlsBaselineFontUsesWorldUnits_PreservesUnitEvenAtNeutralScale()
		{
			Settings.Default.UITextScale = 100;

			using (var button = new Button
			{
				Text = "New",
				Font = new Font("Segoe UI", 11f, FontStyle.Regular, GraphicsUnit.World)
			})
			{
				button.ScaleUIFont();

				Assert.AreEqual(GraphicsUnit.World, button.Font.Unit,
					"Reconstructing the font must preserve the original GraphicsUnit, not silently convert it to Point.");
				Assert.AreEqual(11f, button.Font.Size, 0.01f,
					"At 100% scale the numeric size (in its original unit) should be unchanged.");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A control's Text is a mutable, independent property -- a Label with empty Text now
		/// can have text set into it later (e.g. a status message, or a field populated after a
		/// slower load). Skipping the Font assignment while Text happens to be empty would leave
		/// that control's Font un-set (ambient) only until the next time its own Font is ever
		/// explicitly assigned for any reason.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ControlHasEmptyText_FontIsStillScaled()
		{
			Settings.Default.UITextScale = 200;

			using (var label = new Label { Text = "", Font = new Font(FontFamily.GenericSansSerif, 9f) })
			{
				var originalSize = label.Font.Size;

				label.ScaleUIFont();

				Assert.Greater(label.Font.Size, originalSize);
			}
		}
	}
}
