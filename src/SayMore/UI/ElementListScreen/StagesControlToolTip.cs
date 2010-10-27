using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model;
using SayMore.Model.Files;

namespace SayMore.UI.ElementListScreen
{
	public partial class StagesControlToolTip : Form
	{
		private readonly IEnumerable<ComponentRole> _componentRoles;
		private readonly StagesImageMaker _stagesImageMaker;

		private readonly PictureBox _imageTemplate;
		private readonly Label _nameTemplate;
		private readonly Label _compltedTemplate;

		/// ------------------------------------------------------------------------------------
		public StagesControlToolTip(IEnumerable<ComponentRole> componentRoles, StagesImageMaker stagesImageMaker)
		{
			_componentRoles = componentRoles;
			_stagesImageMaker = stagesImageMaker;
			InitializeComponent();

			_imageTemplate = _picBoxTemplate;
			_nameTemplate = _labelComponentTemplate;
			_compltedTemplate = _labelCompleteTemplate;
		}

		/// --------------------------------------------------------------------------------
		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		public void SetComponentStage(IEnumerable<ComponentRole> completedRoles)
		{
			int row = 0;
			_tableLayout.Controls.Clear();
			_tableLayout.RowStyles.Clear();
			_tableLayout.RowCount = _componentRoles.Count();

			foreach (var role in _componentRoles)
			{
				_tableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				_tableLayout.Controls.Add(GetNewRolePictureBox(role), 0, row);

				var text = role.Name + ":";
				_tableLayout.Controls.Add(GetNewRoleLabel(_nameTemplate, text), 1, row);

				text = (role.IsContainedIn(completedRoles) ? "Complete" : "Incomplete");
				_tableLayout.Controls.Add(GetNewRoleLabel(_compltedTemplate, text), 2, row);

				row++;
			}
		}

		/// ------------------------------------------------------------------------------------
		private PictureBox GetNewRolePictureBox(ComponentRole role)
		{
			var pb = new PictureBox
			{
				Image = _stagesImageMaker.GetComponentStageColorBlock(role),
				SizeMode = _imageTemplate.SizeMode,
				Anchor = _imageTemplate.Anchor,
				Margin = _imageTemplate.Margin,
			};

			return pb;
		}

		/// ------------------------------------------------------------------------------------
		private static Label GetNewRoleLabel(Label lblTemplate, string text)
		{
			var lbl = new Label
			{
				Font = SystemFonts.IconTitleFont,
				Text = text,
				Margin = lblTemplate.Margin,
				AutoSize = true
			};

			return lbl;
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
