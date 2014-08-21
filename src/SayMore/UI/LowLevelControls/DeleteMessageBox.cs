using System.Drawing;
using System.Windows.Forms;
using Palaso.Reporting;

namespace SayMore.UI.LowLevelControls
{
	public partial class DeleteMessageBox : Form
	{
		/// ------------------------------------------------------------------------------------
		public static DialogResult Show(string msg)
		{
			return Show(null, msg);
		}

		/// ------------------------------------------------------------------------------------
		public static DialogResult Show(Control ctrl, string msg)
		{
			using (var dlg = new DeleteMessageBox(msg))
			{
				if (ctrl == null)
					dlg.StartPosition = FormStartPosition.CenterScreen;

				return dlg.ShowDialog(ctrl);
			}
		}

		/// ------------------------------------------------------------------------------------
		public DeleteMessageBox(string msg)
		{
			Logger.WriteEvent("DeleteMessageBox constructor");

			InitializeComponent();
			_labelMessage.Font = SystemFonts.MessageBoxFont;
			_labelMessage.Text = msg;
		}
	}
}
