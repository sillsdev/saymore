using System;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Utilities.Utilities;

namespace SayMore.Utilities
{
	public partial class LoadingDlg : Form
	{
		public Action CancelClicked;

		/// ------------------------------------------------------------------------------------
		public LoadingDlg()
		{
			InitializeComponent();
			_linkCancel.Font = SystemFonts.IconTitleFont;
			BackColor = Color.White;
			_linkCancel.Visible = false;
		}

		/// ------------------------------------------------------------------------------------
		public LoadingDlg(string message) : this(message, false)
		{
		}

		/// ------------------------------------------------------------------------------------
		public LoadingDlg(bool showCancel) : this(null, showCancel)
		{
		}

		/// ------------------------------------------------------------------------------------
		public LoadingDlg(string message, bool showCancel) : this()
		{
			if (message != null)
				_labelLoading.Text = message;

			if (_labelLoading.Right - 20 > Right)
				Width += ((_labelLoading.Right + 20) - Right);

			_linkCancel.Visible = showCancel;
			_linkCancel.LinkClicked += delegate
			{
				if (CancelClicked != null)
					CancelClicked();
			};
		}

		/// ------------------------------------------------------------------------------------
		public void Show(Control parent)
		{
			if (parent == null || parent.Width < Width || parent.Height < Height)
				StartPosition = FormStartPosition.CenterScreen;
			else
			{
				// Center the loading dialog within the bounds of the specified control.
				StartPosition = FormStartPosition.Manual;
				var pt = parent.PointToScreen(new Point(0, 0));
				pt.X += (parent.Width - Width) / 2;
				pt.Y += (parent.Height - Height) / 2;
				Location = pt;
			}

			Show();
			Application.DoEvents();
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
