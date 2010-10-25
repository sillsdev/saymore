using System;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Model;

namespace SayMore.UI.ElementListScreen
{
	public partial class EventComponentToolTip : Form
	{
		/// ------------------------------------------------------------------------------------
		public EventComponentToolTip()
		{
			InitializeComponent();

			foreach (Control ctrl in _tableLayout.Controls)
			{
				if (ctrl.Name.StartsWith("_label"))
					ctrl.Font = SystemFonts.IconTitleFont;
			}
		}

		/// --------------------------------------------------------------------------------
		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		public void SetComponentStage(Event.ComponentStage componentStage)
		{
			int i = 0;

			foreach (Event.ComponentStage cs in Enum.GetValues(typeof(Event.ComponentStage)))
			{
				if (cs == Event.ComponentStage.None)
					continue;

				_tableLayout.Controls["_labelComponent" + i].Text =
					Event.GetComponentStageText(cs) +":";

				((PictureBox)_tableLayout.Controls["_picBox" + i]).Image =
					Event.GetImageForComponentStage(cs);

				_tableLayout.Controls["_labelComplete" + i].Text =
					((componentStage & cs) == cs ? "Complete" : "Incomplete");

				i++;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Show(Point pt, Event.ComponentStage componentStage)
		{
			SetComponentStage(componentStage);
			Location = pt;
			Size = new Size(_tableLayout.Width + 2, _tableLayout.Height + 2);
			Show();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw a subtle border around the window.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var rc = ClientRectangle;
			rc.Width--;
			rc.Height--;

			using (var pen = new Pen(Color.FromArgb(120, SystemColors.InfoText)))
				e.Graphics.DrawRectangle(pen, rc);
		}
	}
}
