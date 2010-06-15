// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: ViewTab.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SayMore.UI.ProjectWindow;
using SilUtils;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ViewTab : Label
	{
		private bool _mouseOver;
		private bool _selected;
		private readonly Image _image;
		private Control _viewControl;
		private bool _hasBeenActivated;

		/// ------------------------------------------------------------------------------------
		public ViewTabGroup OwningTabGroup { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTab(ViewTabGroup owningTabControl, Control viewControl)
		{
			base.DoubleBuffered = true;
			base.AutoSize = false;
			base.Font = owningTabControl.TabFont;
			base.Text = Utils.RemoveAcceleratorPrefix(viewControl.Text);
			base.Dock = DockStyle.Left;

			OwningTabGroup = owningTabControl;

			if (viewControl is ISayMoreView)
				_image = ((ISayMoreView)viewControl).Image;

			_viewControl = viewControl;
			_viewControl.Dock = DockStyle.Fill;
			OwningTabGroup.Controls.Add(_viewControl);
			_viewControl.PerformLayout();
			_viewControl.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		public void ActivateView()
		{
			if (_viewControl == null)
				return;

			_selected = true;
			_viewControl.Visible = true;
			_viewControl.BringToFront();

			if (_viewControl is ISayMoreView)
				((ISayMoreView)_viewControl).ViewActivated(_hasBeenActivated);

			_hasBeenActivated = true;
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		public bool DeactivateView(bool showMsgWhenNotOK)
		{
			if (_viewControl == null)
				return true;

			if (_viewControl is ISayMoreView)
			{
				if (!((ISayMoreView)_viewControl).IsOKToLeaveView(showMsgWhenNotOK))
					return false;

				((ISayMoreView)_viewControl).ViewDeactivated();
			}

			_selected = false;
			_viewControl.Visible = false;
			Invalidate();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public void CloseView()
		{
			if (_viewControl == null)
				return;

			Visible = true;

			if (!_viewControl.IsDisposed)
			{
				if (OwningTabGroup != null && OwningTabGroup.Controls.Contains(_viewControl))
					OwningTabGroup.Controls.Remove(_viewControl);

				_viewControl.Dispose();
				_viewControl = null;
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab's control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control View
		{
			get { return _viewControl; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
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
				ViewTab adjacentTab = OwningTabGroup.FindFirstVisibleTabToLeft(this);
				if (adjacentTab != null)
				{
					adjacentTab.Invalidate();
					Utils.UpdateWindow(adjacentTab.Handle);
				}
			}
		}

		#endregion

		#region Overridden methods and event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			_mouseOver = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			_mouseOver = false;
			Invalidate();
			base.OnMouseLeave(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
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
