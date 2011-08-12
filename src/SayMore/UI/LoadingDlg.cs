using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SayMore.UI.Utilities;

namespace SayMore.UI
{
	public partial class LoadingDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		public LoadingDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var rc = ClientRectangle;
			rc.Width--;
			rc.Height--;

			using (var pen = new Pen(AppColors.BarBorder))
				e.Graphics.DrawRectangle(pen, rc);
		}
	}
}
