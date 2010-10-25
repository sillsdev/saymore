using System;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Properties;

namespace SayMore.UI.Utilities
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
	public class AppColors
	{
		public static Color BarBegin = Settings.Default.BarColorBegin;
		public static Color BarEnd = Settings.Default.BarColorEnd;
		public static Color BarBorder = Settings.Default.BarColorBorder;
		public static Color DataEntryPanelBegin = Settings.Default.DataEntryPanelColorBegin;
		public static Color DataEntryPanelEnd = Settings.Default.DataEntryPanelColorEnd;
		public static Color DataEntryPanelBorder = Settings.Default.DataEntryPanelColorBorder;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Replaces one color with another in the specified image. This returns a new image
		/// so it's up to the caller to dispose of the original as necessary.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Image ReplaceColor(Image bmpOrig, Color clrOld, Color clrNew)
		{
			// This is a pretty brute-force (and slow) way to do this.
			// Perhaps there's a better way.
			var bmp = new Bitmap(bmpOrig);
			for (int y = 0; y < bmp.Height; y++)
			{
				for (int x = 0; x < bmp.Width; x++)
				{
					var clr = bmp.GetPixel(x, y);
					if (clr == clrOld)
						bmp.SetPixel(x, y, clrNew);
				}
			}

			return bmp;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Paints the background of the specified control using the data entry background
		///// color scheme.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public static void PaintDataEntryBackground(object sender, PaintEventArgs e)
		//{
		//    Rectangle rc = ((Control)sender).ClientRectangle;
		//    PaintDataEntryBackground(e.Graphics, rc, BorderSides.All);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Paints the background of the specified control using the data entry background
		///// color scheme.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public static void PaintDataEntryBackground(Graphics g, Rectangle rc, BorderSides sides)
		//{
		//    //using (var br = new LinearGradientBrush(rc, DataEntryPanelBegin, DataEntryPanelEnd, 45f))
		//    //    g.FillRectangle(br, rc);

		//    var clrDark = ColorHelper.CalculateColor(Settings.Default.PersonEditorsBorderColor, Color.White, 150);


		//    using (var br = new LinearGradientBrush(rc,
		//        Settings.Default.PersonEditorsBackgroundColor,
		//        clrDark, 135f))
		//    {
		//        g.FillRectangle(br, rc);
		//    }

		//    PaintBorder(g, Settings.Default.PersonEditorsBorderColor, rc, sides);
		//    //			PaintDataEntryBorder(g, rc, sides);
		//}

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
			if (sides == BorderSides.None)
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