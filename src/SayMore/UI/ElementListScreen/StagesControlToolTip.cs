using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Model;
using SayMore.Model.Files;

namespace SayMore.UI.ElementListScreen
{
	public partial class StagesControlToolTip : Form
	{
		private readonly IEnumerable<ComponentRole> _componentRoles;
		private readonly StagesImageMaker _stagesImageMaker;

		/// ------------------------------------------------------------------------------------
		public StagesControlToolTip(IEnumerable<ComponentRole> componentRoles, StagesImageMaker stagesImageMaker)
		{
			_componentRoles = componentRoles;
			_stagesImageMaker = stagesImageMaker;
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
		public void SetComponentStage(IEnumerable<ComponentRole> completedRoles)
		{
			int i = 0;

			foreach (var role in _componentRoles)
			{
				//TODO: make the number of stage rows sensitive to the actual number of roles, not preset to 4
				if (i > 4)
					break;
				_tableLayout.Controls["_labelComponent" + i].Text =
					role.Name +":";

				((PictureBox)_tableLayout.Controls["_picBox" + i]).Image =
					_stagesImageMaker.GetComponentStageColorBlock(role, completedRoles);

				_tableLayout.Controls["_labelComplete" + i].Text =
					(role.IsContainedIn(completedRoles)? "Complete" : "Incomplete");

				i++;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Show(Point pt, IEnumerable<ComponentRole> completedRoles)
		{
			SetComponentStage(completedRoles);
			Location = pt;
			Size = new Size(_tableLayout.Width + (_tableLayout.Left * 2),
				_tableLayout.Height + (_tableLayout.Top * 2));

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
