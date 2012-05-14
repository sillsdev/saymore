using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SilTools;

namespace SayMore.UI.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	public class ViewTab : Label
	{
		private bool _mouseOver;
		private bool _selected;
		private readonly Image _image;
		private bool _hasBeenActivated;
		private readonly Action<ViewTab> _tabTextChangedAction;

		/// ------------------------------------------------------------------------------------
		public ViewTabGroup OwningTabGroup { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ViewTab(ViewTabGroup owningTabControl, Control viewControl,
			Action<ViewTab> tabTextChangedAction)
		{
			base.DoubleBuffered = true;
			base.AutoSize = false;
			base.Font = owningTabControl.TabFont;
			base.Dock = DockStyle.Left;
			base.Text = Utils.RemoveAcceleratorPrefix(viewControl.Text);
			Name = base.Text.Replace(" ", string.Empty) + "ViewTab";
			Text = "Set This Tab's Text";

			OwningTabGroup = owningTabControl;
			_tabTextChangedAction = tabTextChangedAction;

			if (viewControl is ISayMoreView)
				_image = ((ISayMoreView)viewControl).Image;

			View = viewControl;
			View.Dock = DockStyle.Fill;
			OwningTabGroup.Controls.Add(View);
			View.PerformLayout();
			View.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		public void ActivateView()
		{
			if (View == null)
				return;

			_selected = true;
			View.Visible = true;
			View.BringToFront();

			if (View is ISayMoreView)
				((ISayMoreView)View).ViewActivated(!_hasBeenActivated);

			_hasBeenActivated = true;
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		public bool DeactivateView(bool showMsgWhenNotOK)
		{
			if (View == null)
				return true;

			if (View is ISayMoreView)
			{
				if (!((ISayMoreView)View).IsOKToLeaveView(showMsgWhenNotOK))
					return false;

				((ISayMoreView)View).ViewDeactivated();
			}

			_selected = false;
			View.Visible = false;
			Invalidate();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public void CloseView()
		{
			if (View == null)
				return;

			Visible = true;

			if (!View.IsDisposed)
			{
				if (OwningTabGroup != null && OwningTabGroup.Controls.Contains(View))
					OwningTabGroup.Controls.Remove(View);

				View.Dispose();
				View = null;
			}
		}

		#region Properties

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab's control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control View { get; private set; }

		/// ------------------------------------------------------------------------------------
		public bool Selected
		{
			get { return _selected; }
			set
			{
				if (_selected == value)
					return;

				_selected = value;
				Invalidate();
				Utils.UpdateWindow(Handle);

				// Invalidate the tab to the left of this one in
				// case it needs to redraw its etched right border.
				var adjacentTab = OwningTabGroup.FindFirstVisibleTabToLeft(this);
				if (adjacentTab != null)
				{
					adjacentTab.Invalidate();
					Utils.UpdateWindow(adjacentTab.Handle);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get { return base.Text; }
			set
			{
				base.Text = value;
				if (_tabTextChangedAction != null)
					_tabTextChangedAction(this);
			}
		}

		#endregion

		#region Overridden methods and event handlers
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			_mouseOver = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			_mouseOver = false;
			Invalidate();
			base.OnMouseLeave(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			DrawBackground(e.Graphics);
			DrawImage(e.Graphics);
			DrawText(e.Graphics);
			DrawHoverIndicator(e.Graphics);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the tab's background.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawBackground(Graphics g)
		{
			Rectangle rc = ClientRectangle;

			// First, fill the entire background with the control color.
			g.FillRectangle(SystemBrushes.Control, rc);

			Point[] pts = new[] {new Point(0, rc.Bottom), new Point(0, rc.Top + 3),
				new Point(3, 0), new Point(rc.Right - 4, 0), new Point(rc.Right - 1, rc.Top + 3),
				new Point(rc.Right - 1, rc.Bottom)};

			if (_selected)
			{
				using (SolidBrush br = new SolidBrush(Color.White))
					g.FillPolygon(br, pts);

				g.DrawLines(SystemPens.ControlDark, pts);
			}
			else
			{
				// Draw the etched line on the right edge to act as a separator. But
				// only draw it when the tab to the right of this one is not selected.
				if (!OwningTabGroup.IsRightAdjacentTabSelected(this))
				{
					g.DrawLine(SystemPens.ControlDark, rc.Width - 2, 1, rc.Width - 2, rc.Height - 5);
					g.DrawLine(SystemPens.ControlLight, rc.Width - 1, 1, rc.Width - 1, rc.Height - 5);
				}

				// The tab is not selected tab, so draw a
				// line across the bottom of the tab.
				g.DrawLine(SystemPens.ControlDark, 0, rc.Bottom - 1, rc.Right, rc.Bottom - 1);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw's the tab's image.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawImage(Graphics g)
		{
			if (_image == null)
				return;

			Rectangle rc = ClientRectangle;
			rc.X = 7;
			rc.Y = (rc.Height - _image.Height) / 2;
			rc.Size = _image.Size;

			if (_selected)
				rc.Y++;

			g.DrawImage(_image, rc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the tab's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawText(IDeviceContext g)
		{
			const TextFormatFlags kFlags = TextFormatFlags.VerticalCenter |
				TextFormatFlags.HorizontalCenter | TextFormatFlags.WordEllipsis |
				TextFormatFlags.SingleLine | TextFormatFlags.NoPadding |
				TextFormatFlags.HidePrefix | TextFormatFlags.PreserveGraphicsClipping;

			Color clrText = (_selected ? Color.Black :
				ColorHelper.CalculateColor(SystemColors.ControlText,
				SystemColors.Control, 145));

			Rectangle rc = ClientRectangle;

			// Account for the image if there is one.
			if (_image != null)
			{
				rc.X += (5 + _image.Width);
				rc.Width -= (5 + _image.Width);
			}

			// When the tab is selected, then bump the text down a couple of pixels.
			if (_selected)
			{
				rc.Y += 2;
				rc.Height -= 2;
			}

			TextRenderer.DrawText(g, Text, Font, rc, clrText, kFlags);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the mouse is over the tab, draw a line across the top to hightlight the tab
		/// the mouse is over.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawHoverIndicator(Graphics g)
		{
			if (!_mouseOver)
				return;

			Rectangle rc = ClientRectangle;

			Color clr = (PaintingHelper.CanPaintVisualStyle() ?
				VisualStyleInformation.ControlHighlightHot : SystemColors.Highlight);

			// Draw the lines that only show when the mouse is over the tab.
			using (Pen pen = new Pen(clr))
			{
				if (_selected)
				{
					g.DrawLine(pen, 3, 1, rc.Right - 4, 1);
					g.DrawLine(pen, 2, 2, rc.Right - 3, 2);
				}
				else
				{
					g.DrawLine(pen, 0, 0, rc.Right - 3, 0);
					g.DrawLine(pen, 0, 1, rc.Right - 3, 1);
				}
			}
		}

		#endregion
	}
}
