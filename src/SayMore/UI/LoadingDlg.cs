using System.Drawing;
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
		public LoadingDlg(string message) : this()
		{
			if (message != null)
				_labelLoading.Text = message;

			if (_labelLoading.Right - 20 > Right)
				Width += ((_labelLoading.Right + 20) - Right);
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
