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
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SIL.Sponge.Properties;

namespace SIL.Sponge.Utilities
{
	[Flags]
	public enum BorderSides
	{
		None = 0,
		Left = 1,
		Right = 2,
		Top = 4,
		Bottom = 8,
		All = 15
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SpongeColors
	{
		public static Color BarBegin = Settings.Default.BarColorBegin;
		public static Color BarEnd = Settings.Default.BarColorEnd;
		public static Color BarBorder = Settings.Default.BarColorBorder;
		public static Color DataEntryPanelBegin = Settings.Default.DataEntryPanelColorBegin;
		public static Color DataEntryPanelEnd = Settings.Default.DataEntryPanelColorEnd;
		public static Color DataEntryPanelBorder = Settings.Default.DataEntryPanelColorBorder;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints the background of the specified control using the data entry background
		/// color scheme.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void PaintDataEntryBackground(object sender, PaintEventArgs e)
		{
			Rectangle rc = ((Control)sender).ClientRectangle;
			PaintDataEntryBackground(e.Graphics, rc, BorderSides.All);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints the background of the specified control using the data entry background
		/// color scheme.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void PaintDataEntryBackground(Graphics g, Rectangle rc, BorderSides sides)
		{
//TODO: can this be done without a static?
			//if (MainWnd.Resizing)
//				return;

			using (var br = new LinearGradientBrush(rc, DataEntryPanelBegin, DataEntryPanelEnd, 45f))
				g.FillRectangle(br, rc);

			PaintDataEntryBorder(g, rc, sides);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints the border of the specified control using the data entry background
		/// color scheme.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void PaintDataEntryBorder(object sender, PaintEventArgs e)
		{
			PaintDataEntryBorder(e.Graphics, ((Control)sender).ClientRectangle, BorderSides.All);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint borders in the specified rectangle using the border color for the data
		/// entry color scheme.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void PaintDataEntryBorder(Graphics g, Rectangle rc, BorderSides sides)
		{
			PaintBorder(g, DataEntryPanelBorder, rc, sides);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint borders in the specified rectangle using the specified color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void PaintBorder(Graphics g, Color clr, Rectangle rc, BorderSides sides)
		{
			if (sides == BorderSides.None /*|| TODO: can this be done without a static?
					MainWnd.Resizing*/
									  )
				return;

			using (var pen = new Pen(clr))
			{
				rc.Width--;
				rc.Height--;

				if (sides == BorderSides.All)
				{
					g.DrawRectangle(pen, rc);
					return;
				}

				if ((sides & BorderSides.Left) == BorderSides.Left)
					g.DrawLine(pen, rc.Location, new Point(rc.Left, rc.Bottom));

				if ((sides & BorderSides.Top) == BorderSides.Top)
					g.DrawLine(pen, rc.Location, new Point(rc.Right, rc.Top));

				if ((sides & BorderSides.Right) == BorderSides.Right)
					g.DrawLine(pen, new Point(rc.Right, rc.Top), new Point(rc.Right, rc.Bottom ));

				if ((sides & BorderSides.Bottom) == BorderSides.Bottom)
					g.DrawLine(pen, new Point(rc.Left, rc.Bottom), new Point(rc.Right, rc.Bottom));
			}
		}
	}
}