using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using L10NSharp;
using SayMore.Model.Files;

namespace SayMore.UI.ElementListScreen
{
	public partial class StagesControlToolTip : Form
	{
#if !__MonoCS__
		[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
		public static extern IntPtr SetWindowPos(IntPtr hWnd,
			int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
#endif
		private readonly IEnumerable<ComponentRole> _componentRoles;
		private readonly StagesDataProvider _stagesImageMaker;

		private readonly PictureBox _imageTemplate;
		private readonly Label _nameTemplate;
		private readonly Label _compltedTemplate;

		/// ------------------------------------------------------------------------------------
		public StagesControlToolTip(IEnumerable<ComponentRole> componentRoles, StagesDataProvider stagesImageMaker)
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

			var compRoles = completedRoles.ToArray();

			foreach (var role in _componentRoles)
			{
				_tableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				_tableLayout.Controls.Add(GetNewRolePictureBox(role), 0, row);

				var text = role.Name + ":";
				_tableLayout.Controls.Add(GetNewRoleLabel(_nameTemplate, text), 1, row);

				text = (role.IsContainedIn(compRoles) ?
					LocalizationManager.GetString("SessionsView.SessionsList.Stages.CompleteToolTipText", "Complete") :
					LocalizationManager.GetString("SessionsView.SessionsList.Stages.IncompleteToolTipText", "Incomplete"));

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
				Font = Program.DialogFont,
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
#if !__MonoCS__
			// There are some cases when showing the window doesn't bring it to the
			// front. e.g. after dropping down a menu, this tooltip form no longer
			// get's displayed on top of the main window's form. Therefore, we need
			// to force it without making the tooltip active. Perhaps, one day, I
			// just need to put all this on an owner drawn tooltip instead of a
			// form. (SWP_NOACTIVATE = 0x10, SWP_NOMOVE = 0x2,
			// SWP_NOREPOSITION = 0x200, SWP_NOSIZE = 0x1.)
			SetWindowPos(Handle, 0, 0, 0, 0, 0, 0x10 | 0x2 | 0x200 | 0x1);
#else
#endif
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
