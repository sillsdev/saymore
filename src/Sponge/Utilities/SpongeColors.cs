// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: SpongeColors.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Sponge.Utilities
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SpongeColors
	{
		//public static Color DefaultSpongeBarColorBegin =
		//    ColorHelper.CalculateColor(Color.LightSteelBlue, Color.White, 200);

		//public static Color DefaultSpongeBarColorEnd = Color.SteelBlue;

		public static Color BarBegin =
			ColorHelper.CalculateColor(Color.FromArgb(88, 126, 176), Color.White, 90);

		public static Color BarEnd =
			ColorHelper.CalculateColor(Color.FromArgb(88, 126, 176), Color.White, 200);

		public static Color BarBorder = Color.FromArgb(83, 110, 145);

		public static Color DataEntryPanelBegin =
			ColorHelper.CalculateColor(Color.FromArgb(188, 196, 153), Color.White, 175);

		public static Color DataEntryPanelEnd = Color.FromArgb(188, 196, 153);
		public static Color DataEntryPanelBorder = Color.FromArgb(167, 175, 138);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints the background of the specified control using the data entry background
		/// color scheme.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void PaintDataEntryBackground(object sender, PaintEventArgs e)
		{
			Rectangle rc = ((Control)sender).ClientRectangle;

			using (var br = new LinearGradientBrush(rc, DataEntryPanelBegin, DataEntryPanelEnd, 45f))
				e.Graphics.FillRectangle(br, rc);

			PaintDataEntryBorder(sender, e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints the border of the specified control using the data entry background
		/// color scheme.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void PaintDataEntryBorder(object sender, PaintEventArgs e)
		{
			Rectangle rc = ((Control)sender).ClientRectangle;
			rc.Width--;
			rc.Height--;
			using (var pen = new Pen(DataEntryPanelBorder))
				e.Graphics.DrawRectangle(pen, rc);
		}
	}
}