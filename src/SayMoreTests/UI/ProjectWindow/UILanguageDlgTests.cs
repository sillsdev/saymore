using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Properties;
using SayMore.UI.ProjectWindow;
using SayMore.Utilities;

namespace SayMoreTests.UI.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Covers the live-preview font-size computation UILanguageDlg.ScaleFonts relies on.
	/// Extracted as a pure function because the spinner's raw Value can transiently be out of
	/// its own configured [Minimum, Maximum] range while the user is actively typing into it
	/// (e.g. after clearing the field to type a new value), which previously crashed
	/// System.Drawing.Font's constructor (SP: "Value of '0' is not valid for 'emSize'").
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class UILanguageDlgTests
	{
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPreviewFontSize_ValueWithinRange_ScalesBaseline()
		{
			Assert.AreEqual(15f, UILanguageDlg.GetPreviewFontSize(10f, 150m));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPreviewFontSize_ValueIsZero_ClampsToMinimumScale()
		{
			var result = UILanguageDlg.GetPreviewFontSize(10f, 0m);
			Assert.Greater(result, 0f);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPreviewFontSize_NegativeValue_ClampsToMinimumScale()
		{
			var result = UILanguageDlg.GetPreviewFontSize(10f, -40m);
			Assert.Greater(result, 0f);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPreviewFontSize_FractionalValueBelowOne_ClampsToMinimumScale()
		{
			// (int) truncation of a value like 0.4 would otherwise produce a scale of exactly 0.
			var result = UILanguageDlg.GetPreviewFontSize(10f, 0.4m);
			Assert.Greater(result, 0f);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPreviewFontSize_RidiculouslyLargeValue_ClampsToMaximumScale()
		{
			var result = UILanguageDlg.GetPreviewFontSize(10f, 999999m);
			Assert.AreEqual(10f * UITextScaler.kMaxScalePercent / 100f, result);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// At 100% scale, a small fixed-pixel gap between rows (e.g. 3-6px) is enough to read
		/// as visual breathing room. But that gap doesn't grow with the live preview the way
		/// font-driven control sizes do, so at a large scale (e.g. 180%, where a combo box's
		/// height can roughly double) the same fixed few pixels become visually negligible
		/// next to the now much taller controls, reading as touching/overlapping even though
		/// nothing is technically broken in the layout math. Margins need to scale too.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ScaleMargin_ScaleGreaterThanOne_ScalesAllFourSides()
		{
			var result = UILanguageDlg.ScaleMargin(new Padding(1, 2, 3, 4), 2f);

			Assert.AreEqual(new Padding(2, 4, 6, 8), result);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ScaleMargin_BaselineSideIsZero_StaysZero()
		{
			// A zero baseline margin has nothing to scale from; this is why the dialog's
			// Designer-configured baseline margins themselves also need to be non-zero on the
			// sides where inter-row breathing room actually matters, not just scaled at runtime.
			var result = UILanguageDlg.ScaleMargin(new Padding(0, 5, 0, 5), 2f);

			Assert.AreEqual(0, result.Left);
			Assert.AreEqual(0, result.Right);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ScaleMargin_ScaleAtBaseline_ReturnsUnchanged()
		{
			var baseline = new Padding(2, 4, 6, 8);

			Assert.AreEqual(baseline, UILanguageDlg.ScaleMargin(baseline, 1f));
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// If the initial UITextScale setting is non-100% (e.g. the saved setting, or a language
	/// whose recommendation differs from the default), setting _numUpDownScale.Value in the
	/// constructor fires ValueChanged -> ScaleFonts() before OnLoad has ever run. ScaleFonts
	/// needs a real baseline font size at that point, not whatever _baseLineFontSize defaults
	/// to before OnLoad sets it.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class UILanguageDlgConstructionTests
	{
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Constructor_InitialScaleNotOneHundredPercent_InitializesBaselineFontSizeBeforeScaling()
		{
			Settings.Default.UITextScale = 150;

			using (var dlg = new UILanguageDlg())
			{
				var field = typeof(UILanguageDlg).GetField("_baseLineFontSize",
					BindingFlags.NonPublic | BindingFlags.Instance);
				var baseline = (float)field.GetValue(dlg);

				Assert.Greater(baseline, 0f);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// _numUpDownScale.Minimum/Maximum are set in the Designer, independently of
		/// UITextScaler.kMinScalePercent/kMaxScalePercent. Rather than trust a comment to keep
		/// them in sync, the constructor overwrites the Designer values with the constants, so
		/// the constants are the actually-enforced source of truth.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Constructor_AlwaysEnforcesSpinnerBoundsFromUITextScalerConstants()
		{
			using (var dlg = new UILanguageDlg())
			{
				var numUpDown = typeof(UILanguageDlg).GetField("_numUpDownScale",
					BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dlg) as NumericUpDown;

				Assert.AreEqual(UITextScaler.kMinScalePercent, numUpDown.Minimum);
				Assert.AreEqual(UITextScaler.kMaxScalePercent, numUpDown.Maximum);
			}
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Covers the decision logic behind "recommend a larger scale for certain languages that 
	/// need it": when to auto-bump the scale value, when to undo a bump we made earlier
	/// (only if the user hasn't touched the value since), and when the Reset button's target/
	/// enabled state should reflect the selected language's own recommended default rather
	/// than a flat 100%.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class UILanguageDlgScaleRecommendationTests
	{
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ResolveScaleForLanguageChange_RecommendedLanguageSelectedAtDefaultValue_BumpsValue()
		{
			var result = UILanguageDlg.ResolveScaleForLanguageChange(100, false, 130);

			Assert.AreEqual(130, result.NewValue);
			Assert.IsTrue(result.AutoBumped);
			Assert.IsTrue(result.ShowHint);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ResolveScaleForLanguageChange_RecommendedLanguageSelectedButValueAlreadyCustomized_LeavesValueAlone()
		{
			var result = UILanguageDlg.ResolveScaleForLanguageChange(150, false, 130);

			Assert.AreEqual(150, result.NewValue);
			Assert.IsFalse(result.AutoBumped);
			Assert.IsTrue(result.ShowHint, "Hint is informational about the language, regardless of the current value.");
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ResolveScaleForLanguageChange_SwitchingAwayFromAutoBumpedLanguage_UndoesBump()
		{
			var result = UILanguageDlg.ResolveScaleForLanguageChange(130, true, null);

			Assert.AreEqual(100, result.NewValue);
			Assert.IsFalse(result.AutoBumped);
			Assert.IsFalse(result.ShowHint);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ResolveScaleForLanguageChange_SwitchingAwayAfterUserManuallyChangedValue_LeavesValueAlone()
		{
			// _scaleWasAutoBumped would already be false here because the ValueChanged handler
			// clears it as soon as the user (not our own code) edits the spinner.
			var result = UILanguageDlg.ResolveScaleForLanguageChange(150, false, null);

			Assert.AreEqual(150, result.NewValue);
			Assert.IsFalse(result.AutoBumped);
			Assert.IsFalse(result.ShowHint);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ResolveScaleForLanguageChange_SwitchingBetweenTwoRecommendedLanguages_UndoesThenReapplies()
		{
			var result = UILanguageDlg.ResolveScaleForLanguageChange(130, true, 140);

			Assert.AreEqual(140, result.NewValue);
			Assert.IsTrue(result.AutoBumped);
			Assert.IsTrue(result.ShowHint);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ResolveScaleForLanguageChange_NeitherLanguageHasRecommendation_NoOp()
		{
			var result = UILanguageDlg.ResolveScaleForLanguageChange(100, false, null);

			Assert.AreEqual(100, result.NewValue);
			Assert.IsFalse(result.AutoBumped);
			Assert.IsFalse(result.ShowHint);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetResetTargetValue_LanguageHasRecommendation_ReturnsRecommendedValue()
		{
			Assert.AreEqual(130, UILanguageDlg.GetResetTargetValue(130));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetResetTargetValue_LanguageHasNoRecommendation_ReturnsOneHundred()
		{
			Assert.AreEqual(100, UILanguageDlg.GetResetTargetValue(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsResetEnabled_ValueDiffersFromTarget_ReturnsTrue()
		{
			Assert.IsTrue(UILanguageDlg.IsResetEnabled(150, 130));
			Assert.IsTrue(UILanguageDlg.IsResetEnabled(130, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsResetEnabled_ValueMatchesTarget_ReturnsFalse()
		{
			Assert.IsFalse(UILanguageDlg.IsResetEnabled(130, 130));
			Assert.IsFalse(UILanguageDlg.IsResetEnabled(100, null));
		}
	}
}
