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
	/// Reproduces a reported "Open dropdown gets cut off at 130% scaling" bug in
	/// SessionBasicEditor's _access ComboBox, which sits in the last row of an AutoSize
	/// TableLayoutPanel. The initial hypothesis was a margin-scaling gap, then a nested-AutoSize-
	/// TableLayoutPanel caching bug; neither held up. The real cause: WinForms' TableLayoutPanel
	/// under-measures an AutoSize row containing a ComboBox, because ComboBox.AutoSize defaults
	/// to false and the panel's row-AutoSize calculation doesn't fall back to GetPreferredSize()
	/// for non-AutoSize children the way it does for e.g. Labels. This reproduces even with a
	/// single (non-nested) TableLayoutPanel and a ComboBox whose large font was never the result
	/// of scaling at all -- it's a pre-existing WinForms quirk, not something our scaling code
	/// introduced. ComboBox's own rendered height is always correct (the OS enforces
	/// PreferredHeight regardless of AutoSize); only the PARENT's row-height bookkeeping is wrong.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class UITextScalerNestedAutoSizeTableLayoutPanelTests
	{
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		private static TableLayoutPanel BuildAccessLikeRow(out ComboBox combo)
		{
			var font = new Font(FontFamily.GenericSansSerif, 8.25f);
			combo = new ComboBox { Dock = DockStyle.Top, Font = font, Text = "Not Restricted" };
			var label = new Label { AutoSize = true, Text = "Access", Font = font };

			var panel = new TableLayoutPanel
			{
				Dock = DockStyle.Top,
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				ColumnCount = 1,
				RowCount = 2
			};
			panel.RowStyles.Add(new RowStyle());
			panel.RowStyles.Add(new RowStyle());
			panel.Controls.Add(label, 0, 0);
			panel.Controls.Add(combo, 0, 1);
			return panel;
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ComboBoxDockedInAutoSizeTableLayoutPanelRow_RowGrowsToFitAfterScaling()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new Panel { Size = new Size(400, 400) })
			{
				var panel = BuildAccessLikeRow(out var combo);
				host.Controls.Add(panel);
				host.CreateControl();
				panel.PerformLayout();

				Settings.Default.UITextScale = 180;
				host.ScaleUIFont();

				Assert.GreaterOrEqual(panel.Height, combo.Bottom,
					"The combo box's own row/panel should be tall enough to fully contain it after scaling, not clip it.");
			}
		}
	}
}
