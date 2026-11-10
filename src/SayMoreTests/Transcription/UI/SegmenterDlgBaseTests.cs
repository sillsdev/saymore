using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Transcription.UI;

namespace SayMoreTests.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// SegmenterDlgBase's Undo/Ignored ToolStripMenuItems have AutoSize turned off and their
	/// Width/Height set explicitly (so the two items match each other), which means they never
	/// grow on their own when UITextScaler later scales their Font up -- this tests the pure
	/// resize-to-match helper directly, without constructing the whole dialog.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SegmenterDlgBaseTests
	{
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ResizeMenuItemsToMatchForCurrentTextScale_ItemsHaveStaleSmallSize_BothGrowToFitCurrentFont()
		{
			using (var shortItem = new ToolStripMenuItem("Undo"))
			using (var longItem = new ToolStripMenuItem("Ignored"))
			{
				shortItem.Font = longItem.Font = new Font(shortItem.Font.FontFamily, 24f, FontStyle.Bold);
				shortItem.AutoSize = false;
				longItem.AutoSize = false;
				// Simulate the bug: Width/Height locked at a small, pre-scale value.
				shortItem.Width = longItem.Width = 10;
				shortItem.Height = longItem.Height = 10;

				SegmenterDlgBase.ResizeMenuItemsToMatchForCurrentTextScale(shortItem, longItem);

				var expectedWidth = Math.Max(
					shortItem.GetPreferredSize(Size.Empty).Width,
					longItem.GetPreferredSize(Size.Empty).Width);
				var expectedHeight = Math.Max(
					shortItem.GetPreferredSize(Size.Empty).Height,
					longItem.GetPreferredSize(Size.Empty).Height) * 2;

				Assert.AreEqual(expectedWidth, shortItem.Width);
				Assert.AreEqual(expectedWidth, longItem.Width);
				Assert.AreEqual(expectedHeight, shortItem.Height);
				Assert.AreEqual(expectedHeight, longItem.Height);
				Assert.Greater(shortItem.Width, 10,
					"The stale pre-scale width must actually be replaced, not left alone.");
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ResizeMenuItemsToMatchForCurrentTextScale_ItemsHaveDifferentText_BothMatchTheWiderOne()
		{
			using (var shortItem = new ToolStripMenuItem("Undo"))
			using (var longItem = new ToolStripMenuItem("A Very Long Ignored Label"))
			{
				shortItem.Font = longItem.Font = new Font(shortItem.Font.FontFamily, 18f, FontStyle.Bold);
				shortItem.AutoSize = false;
				longItem.AutoSize = false;

				SegmenterDlgBase.ResizeMenuItemsToMatchForCurrentTextScale(shortItem, longItem);

				Assert.AreEqual(longItem.Width, shortItem.Width,
					"Both items must end up the same width -- the wider item's preferred width.");
				Assert.GreaterOrEqual(shortItem.Width, longItem.GetPreferredSize(Size.Empty).Width);
			}
		}
	}
}
