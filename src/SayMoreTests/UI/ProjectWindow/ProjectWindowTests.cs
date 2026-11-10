using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.UI.ProjectWindow;

namespace SayMoreTests.UI.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// SILAboutBox's "Check for Updates" button is a third-party control with AutoSize left at
	/// its default false and a fixed Designer Size, so ScaleUIFont's font-only scaling pass
	/// overflows its box at large scale (same symptom as item 2d's "Ignored" button, but here
	/// the box can't be grown since it belongs to a NuGet-provided Form) -- this tests the pure
	/// shrink-to-fit helper directly, without constructing ProjectWindow or SILAboutBox.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class ProjectWindowTests
	{
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ShrinkButtonFontToFit_ScaledFontWouldOverflowButton_ShrinksFontToFitExistingBox()
		{
			const int margin = 8;

			using (var host = new Panel())
			using (var button = new Button
			{
				Text = "Check For Updates",
				Font = new Font("Segoe UI", 8.25f),
				Size = new Size(128, 23)
			})
			{
				host.Controls.Add(button);
				var unscaledFont = button.Font;

				SayMore.UI.ProjectWindow.ProjectWindow.ShrinkButtonFontToFit(button, 200, margin);

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

				SayMore.UI.ProjectWindow.ProjectWindow.ShrinkButtonFontToFit(button, 150, margin);

				Assert.That(button.Font.Size, Is.EqualTo(unscaledFont.Size * 1.5f));
			}
		}
	}
}
