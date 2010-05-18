using System.Drawing;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.UI.Utilities;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class AudioComponentEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public AudioComponentEditor(ComponentFile file)
		{
			InitializeComponent();
			Name = "Audio File Information";
			_binder.SetComponentFile(file);

			_tableLayout.BackColor = SystemColors.Window;

			var labelFont = new Font(SystemFonts.IconTitleFont, FontStyle.Bold);

			foreach (Control ctrl in _tableLayout.Controls)
			{
				if (ctrl.Name.StartsWith("_label"))
					ctrl.Font = labelFont;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			// Fill with the windows background color, an area that is a
			// little smaller than the client area.
			var rc = ClientRectangle;
			rc.Width -= (Padding.Left + Padding.Right);
			rc.Height -= (Padding.Top + Padding.Bottom);
			rc.X = Padding.Left;
			rc.Y = Padding.Top;
			e.Graphics.FillRectangle(SystemBrushes.Window, rc);

			// Draw a border around the fill.
			rc.Inflate(1, 1);
			rc.Width--;

			if (rc.Height < _tableLayout.Height)
				rc.Height += Padding.Bottom;
			else
				rc.Height--;

			using (Pen pen = new Pen(SpongeColors.DataEntryPanelBorder))
				e.Graphics.DrawRectangle(pen, rc);
		}
	}
}
