using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Properties;
using SayMore.Utilities;

namespace SayMoreTests.Utilities
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A Control with both AutoSize=true and an explicit MinimumSize (common for buttons
	/// throughout the app, e.g. SessionsListScreen's "New From Recording..." button) grows
	/// correctly as its font grows, but once its preferred size has exceeded MinimumSize,
	/// shrinking the font back down doesn't reliably shrink the control back down on its own --
	/// a known WinForms quirk. This needs to be fixed in the shared recursive scaling pass
	/// itself (ApplyFontRecursively), not per-dialog, since every scaled Form/UserControl in
	/// the app goes through it.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class UITextScalerAutoSizeMinimumSizeTests
	{
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ButtonWithAutoSizeAndMinimumSizeScaledUpThenBackDown_ShrinksBackToOriginalSize()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new Panel())
			{
				var button = new Button
				{
					AutoSize = true,
					AutoSizeMode = AutoSizeMode.GrowAndShrink,
					MinimumSize = new Size(117, 26),
					Text = "New From Recording...",
					Font = new Font(FontFamily.GenericSansSerif, 8.25f)
				};
				host.Controls.Add(button);

				var originalSize = button.Size;

				Settings.Default.UITextScale = 180;
				host.ScaleUIFont();
				Assert.Greater(button.Height, originalSize.Height, "Sanity check: scaling up should have grown the button.");

				Settings.Default.UITextScale = 100;
				host.ScaleUIFont();

				Assert.AreEqual(originalSize, button.Size);
			}
		}
	}
}
