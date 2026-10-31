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
		/// Reproduces a real crash: closing UILanguageDlg with OK fires NotifyScaleChanged
		/// unconditionally, even when the scale percentage didn't actually change, re-triggering
		/// a scaling pass over controls already scaled in a prior pass. Combined with a control
		/// whose cached original font family no longer matches its current one (e.g. redundant
		/// passes, or a mixed-family Designer hierarchy like SessionBasicEditor's _tableLayout
		/// vs. its children), this used to hit a Debug.Fail that could pop a real blocking
		/// dialog -- even under an automated NUnit run. The fix: treat a family/style mismatch
		/// as the control's font legitimately changing out from under us, not a bug to flag.
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
		/// Reproduces a real bug: ListPanel's _outerPanel.Font is declared in Designer as
		/// ("Segoe UI", 11F, Regular, GraphicsUnit.World) -- not the GraphicsUnit.Point used
		/// almost everywhere else in this codebase (11 World units is roughly equivalent to the
		/// app's usual ~8.25pt text, just expressed in different units). Every control that
		/// ambient-inherits this font (e.g. SessionsListScreen's "New"/"New From Recording..."/
		/// "New From Files..." buttons, once reparented into ListPanel's button flow panel) was
		/// getting visibly enlarged -- even at a neutral 100% scale -- because every font
		/// reconstruction in UITextScaler used the 3-argument Font constructor, which always
		/// creates a GraphicsUnit.Point font, silently discarding the original's unit.
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
	}
}
