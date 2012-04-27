using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using SilTools;
using SilTools.Controls;

namespace SayMore.Utilities.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	public class ViewTabGroup : Panel
	{
		public delegate void ViewTabChangedHandler(ViewTabGroup sender, ViewTab affectedTab);
		public event ViewTabChangedHandler ViewActivated;
		public event ViewTabChangedHandler ViewDeactivated;

		private const TextFormatFlags kTxtFmtFlags = TextFormatFlags.VerticalCenter |
			TextFormatFlags.SingleLine | TextFormatFlags.LeftAndRightPadding;

		private Panel _panelHdrBand;
		private Panel _panelTabs;
		private Panel _panelUndock;
		private Panel _panelScroll;
		private XButton _buttonLeft;
		private XButton _buttonRight;
		private readonly ToolTip _tooltip;

		/// ------------------------------------------------------------------------------------
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<ViewTab> Tabs { get; set; }

		/// ------------------------------------------------------------------------------------
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Font TabFont { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The panel m_pnlHdrBand owns both the m_pnlTabs and the m_pnlUndock panels.
		/// m_pnlUndock contains the close buttons and the arrow buttons that allow the user
		/// to scroll all the tabs left and right. m_pnlTabs contains all the tabs and is the
		/// panel that moves left and right (i.e. scrolls) when the number of tabs in the
		/// group exceeds the available space in which to display them all.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTabGroup()
		{
			Visible = true;
			base.DoubleBuffered = true;
			base.BackColor = SystemColors.Control;
			TabFont = new Font(SystemFonts.IconTitleFont.FontFamily, 9, FontStyle.Bold);
			_tooltip = new ToolTip();

			SetupOuterTabCollectionContainer();
			SetupInnerTabCollectionContainer();
			SetupUndockingControls();
			SetupScrollPanel();

			Tabs = new List<ViewTab>();
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (Tabs != null)
				{
					for (int i = Tabs.Count - 1; i >= 0; i--)
					{
						if (Tabs[i] != null && !Tabs[i].IsDisposed)
							Tabs[i].Dispose();
					}
				}
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the panel that holds the tab collection's panel and in which the tab
		/// collection's panel slides back and forth when it's not wide enough to see all
		/// the tabs at once.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupOuterTabCollectionContainer()
		{
			_panelHdrBand = new Panel();
			_panelHdrBand.Dock = DockStyle.Top;
			_panelHdrBand.Padding = new Padding(0, 0, 0, 5);
			_panelHdrBand.Paint += m_pnlHdrBand_Paint;
			_panelHdrBand.Resize += m_pnlHdrBand_Resize;

			using (var g = CreateGraphics())
			{
				_panelHdrBand.Height = TextRenderer.MeasureText(g, "X",
					TabFont, Size.Empty, kTxtFmtFlags).Height + 27;
			}

			Controls.Add(_panelHdrBand);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the panel to which the tabs are directly added.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupInnerTabCollectionContainer()
		{
			// Create the panel that holds all the tabs.
			_panelTabs = new Panel();
			_panelTabs.Visible = true;
			_panelTabs.Paint += HandleLinePaint;
			_panelTabs.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			_panelTabs.Padding = new Padding(3, 3, 0, 0);
			_panelTabs.Location = new Point(0, 0);
			_panelTabs.Height = _panelHdrBand.Height - 5;
			_panelTabs.Width = 0;
			_panelHdrBand.Controls.Add(_panelTabs);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the undocking button and the panel that owns it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupUndockingControls()
		{
			// Create the panel that will hold the undocking button
			_panelUndock = new Panel();

			// NOTE: Remove the following line and uncomment the rest of the code in this
			// method to display the undock view button to the right of the tabs.
			_panelUndock.Width = 5;
			//m_pnlUndock.Width = 27;
			_panelUndock.Visible = true;
			_panelUndock.Dock = DockStyle.Right;
			_panelUndock.Paint += HandleLinePaint;
			_panelHdrBand.Controls.Add(_panelUndock);
			_panelUndock.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		private void SetupScrollPanel()
		{
			// Create the panel that will hold the close button
			_panelScroll = new Panel();
			_panelScroll.Width = 40;
			_panelScroll.Visible = true;
			_panelScroll.Dock = DockStyle.Right;
			_panelScroll.Paint += HandleLinePaint;
			_panelHdrBand.Controls.Add(_panelScroll);
			_panelScroll.Visible = false;
			_panelScroll.BringToFront();

			int top = ((_panelHdrBand.Height - _panelHdrBand.Padding.Bottom - 18) / 2);

			// Create a left scrolling button.
			_buttonLeft = new XButton();
			_buttonLeft.DrawLeftArrowButton = true;
			_buttonLeft.Size = new Size(18, 18);
			_buttonLeft.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			_buttonLeft.Click += m_btnLeft_Click;
			_buttonLeft.Location = new Point(4, top);
			_panelScroll.Controls.Add(_buttonLeft);

			// Create a right scrolling button.
			_buttonRight = new XButton();
			_buttonRight.DrawRightArrowButton = true;
			_buttonRight.Size = new Size(18, 18);
			_buttonRight.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			_buttonRight.Click += m_btnRight_Click;
			_buttonRight.Location = new Point(22, top);
			_panelScroll.Controls.Add(_buttonRight);

			_tooltip.SetToolTip(_buttonLeft, LocalizationManager.GetString("MainWindow.ViewTabsScrollLeftToolTipText", "Scroll Left"));
			_tooltip.SetToolTip(_buttonRight, LocalizationManager.GetString("MainWindow.ViewTabsScrollRightToolTipText", "Scroll Right"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes all the views associated with the tabs in the tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CloseAllViews()
		{
			foreach (var tab in Tabs)
				tab.CloseView();

			AdjustTabContainerWidth(true);
		}

		/// ------------------------------------------------------------------------------------
		public bool IsOKToCloseGroup
		{
			get
			{
				foreach (var view in Tabs.Where(t => t.View is ISayMoreView).Select(t => t.View as ISayMoreView))
				{
					if (!view.IsOKToLeaveView(false))
						return false;
				}

				return true;
			}
		}

		#region Tab managment methods
		/// ------------------------------------------------------------------------------------
		public ViewTab AddTab(Control view)
		{
			if (_panelTabs.Left > 0)
				_panelTabs.Left = 0;

			var tab = new ViewTab(this, view, TabTextChanged);
			tab.Click += HandleTabClick;
			Tabs.Add(tab);
			_panelTabs.Controls.Add(tab);
			tab.BringToFront();
			return tab;
		}

		/// ------------------------------------------------------------------------------------
		private void TabTextChanged(ViewTab tab)
		{
			// Get the text's width.
			using (var g = CreateGraphics())
			{
				tab.Width = TextRenderer.MeasureText(g, tab.Text, tab.Font,
					Size.Empty, kTxtFmtFlags).Width;

				if (tab.View is ISayMoreView && ((ISayMoreView)tab.View).Image != null)
					tab.Width += (((ISayMoreView)tab.View).Image.Width + 5);
			}

			tab.Width += 6;
			AdjustTabContainerWidth(true);
		}

		/// ------------------------------------------------------------------------------------
		private void AdjustTabContainerWidth(bool includeInVisibleTabs)
		{
			int totalWidth = Tabs.Where(tab => tab.Visible || includeInVisibleTabs).Sum(tab => tab.Width);
			_panelTabs.Width = totalWidth + _panelTabs.Padding.Left + _panelTabs.Padding.Right;
			RefreshScrollButtonPanel();
		}

		/// ------------------------------------------------------------------------------------
		public void EnsureTabVisible(ViewTab tab)
		{
			// Make sure the tab isn't wider than the available width.
			// Just leave if there's no hope of making the tab fully visible.
			int availableWidth = _panelHdrBand.Width - (_panelUndock.Width +
				(_panelScroll.Visible ? _panelScroll.Width : 0));

			if (tab.Width > availableWidth)
				return;

			int maxRight = (_panelScroll.Visible ? _panelScroll.Left : _panelUndock.Left);

			// Get the tab's left and right edge relative to the header panel.
			int left = tab.Left + _panelTabs.Left;
			int right = left + tab.Width;

			// Check if it's already fully visible.
			if (left >= 0 && right < maxRight)
				return;

			// Slide the panel in the proper direction to make it visible.
			int dx = (left < 0 ? left : right - maxRight);
			SlideTabs(_panelTabs.Left - dx);
			RefreshScrollButtonPanel();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTabClick(object sender, EventArgs e)
		{
			SetActiveView(sender as ViewTab);
		}

		/// ------------------------------------------------------------------------------------
		public ViewTab GetSelectedTab()
		{
			return Tabs.FirstOrDefault(x => x.Selected);
		}

		/// ------------------------------------------------------------------------------------
		public Control GetSelectedView()
		{
			var tab = GetSelectedTab();
			return (tab == null ? null : tab.View);
		}

		/// ------------------------------------------------------------------------------------
		public void SetActiveView(Control newVw)
		{
			SetActiveView(Tabs.FirstOrDefault(x => x.View == newVw));
		}

		/// ------------------------------------------------------------------------------------
		public void SetActiveView(ViewTab tab)
		{
			if (tab == null)
				return;

			var currTab = GetSelectedTab();
			if (tab == currTab)
				return;

			Utils.SetWindowRedraw(this, false);

			if (currTab != null)
			{
				if (currTab.DeactivateView(true))
				{
					if (ViewDeactivated != null)
						ViewDeactivated(this, currTab);
				}
				else
				{
					Utils.SetWindowRedraw(this, true);
					return;
				}
			}

			EnsureTabVisible(tab);
			tab.ActivateView();

			if (ViewActivated != null)
				ViewActivated(this, tab);

			Utils.SetWindowRedraw(this, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the first visible tab to the left of the specified tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTab FindFirstVisibleTabToLeft(ViewTab tab)
		{
			int i = _panelTabs.Controls.IndexOf(tab);
			if (i == -1)
				return null;

			// Tabs are in the control collection in reverse order from how they appear
			// (i.e. Control[0] is the furthest right tab.
			while (++i < _panelTabs.Controls.Count && !_panelTabs.Controls[i].Visible) { }

			return (i == _panelTabs.Controls.Count ? null : _panelTabs.Controls[i] as ViewTab);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the first visible tab to the right of the specified tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTab FindFirstVisibleTabToRight(ViewTab tab)
		{
			int i = _panelTabs.Controls.IndexOf(tab);
			if (i == -1)
				return null;

			// Tabs are in the control collection in reverse order from how they appear
			// (i.e. Control[0] is the furthest right tab.
			while (--i >= 0 && !_panelTabs.Controls[i].Visible) { }

			return (i < 0 ? null : _panelTabs.Controls[i] as ViewTab);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the first visible tab to the right of the
		/// specified tab is selected.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsRightAdjacentTabSelected(ViewTab tab)
		{
			var adjacentTab = FindFirstVisibleTabToRight(tab);
			return (adjacentTab != null && adjacentTab.Selected);
		}

		#endregion

		#region Methods for managing scrolling of the tabs
		/// ------------------------------------------------------------------------------------
		void m_pnlHdrBand_Resize(object sender, EventArgs e)
		{
			RefreshScrollButtonPanel();
		}

		/// ------------------------------------------------------------------------------------
		private void RefreshScrollButtonPanel()
		{
			if (_panelTabs == null || _panelHdrBand == null || _panelUndock == null)
				return;

			// Determine whether or not the scroll button panel should
			// be visible and set its visible state accordingly.
			bool shouldBeVisible = (_panelTabs.Width > _panelHdrBand.Width - _panelUndock.Width);
			if (_panelScroll.Visible != shouldBeVisible)
				_panelScroll.Visible = shouldBeVisible;

			// Determine whether or not the tabs are scrolled to either left or right
			// extreme. If so, then the appropriate scroll buttons needs to be disabled.
			_buttonLeft.Enabled = (_panelTabs.Left < 0);
			_buttonRight.Enabled = (_panelTabs.Right > _panelUndock.Left ||
				(shouldBeVisible && _panelTabs.Right > _panelScroll.Left));

			// If the scroll buttons are hidden and the tab panel is
			// not all visible, then move it so all the tabs are visible.
			if (!shouldBeVisible && _panelTabs.Left < 0)
				_panelTabs.Left = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scroll the tabs to the right (i.e. move the tab's panel to the right) so user is
		/// able to see tabs obscured on the left side.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnLeft_Click(object sender, EventArgs e)
		{
			int left = _panelTabs.Left;

			// Find the furthest right tab that is partially
			// obscurred and needs to be scrolled into view.
			foreach (var tab in Tabs)
			{
				if (left < 0 && left + tab.Width >= 0)
				{
					SlideTabs(_panelTabs.Left + Math.Abs(left));
					break;
				}

				left += tab.Width;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scroll the tabs to the left (i.e. move the tab's panel to the left) so user is
		/// able to see tabs obscured on the right side.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnRight_Click(object sender, EventArgs e)
		{
			int left = _panelTabs.Left + _panelTabs.Padding.Left;

			// Find the furthest left tab that is partially
			// obscurred and needs to be scrolled into view.
			foreach (var tab in Tabs)
			{
				if (left <= _panelScroll.Left && left + tab.Width > _panelScroll.Left)
				{
					int dx = (left + tab.Width) - _panelScroll.Left;
					SlideTabs(_panelTabs.Left - dx);
					break;
				}

				left += tab.Width;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Slides the container for all the tab controls to the specified new left value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SlideTabs(int newLeft)
		{
			float dx = Math.Abs(_panelTabs.Left - newLeft);
			int pixelsPerIncrement = (int)Math.Ceiling(dx / 75f);
			var slidingLeft = (newLeft < _panelTabs.Left);

			while (_panelTabs.Left != newLeft)
			{
				if (slidingLeft)
				{
					if (_panelTabs.Left - pixelsPerIncrement < newLeft)
						_panelTabs.Left = newLeft;
					else
						_panelTabs.Left -= pixelsPerIncrement;
				}
				else
				{
					if (_panelTabs.Left + pixelsPerIncrement > newLeft)
						_panelTabs.Left = newLeft;
					else
						_panelTabs.Left += pixelsPerIncrement;
				}

				Utils.UpdateWindow(_panelTabs.Handle);
			}

			RefreshScrollButtonPanel();
		}

		#endregion

		#region Painting methods
		/// ------------------------------------------------------------------------------------
		void m_pnlHdrBand_Paint(object sender, PaintEventArgs e)
		{
			var rc = _panelHdrBand.ClientRectangle;
			int y = rc.Bottom - 6;
			e.Graphics.DrawLine(SystemPens.ControlDark, rc.Left, y, rc.Right, y);

			using (var br = new SolidBrush(Color.White))
				e.Graphics.FillRectangle(br, rc.Left, y + 1, rc.Right, rc.Bottom);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw a line that's the continuation of the line drawn underneath all the tabs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static void HandleLinePaint(object sender, PaintEventArgs e)
		{
			var pnl = sender as Panel;

			if (pnl == null)
				return;

			var rc = pnl.ClientRectangle;
			int y = rc.Bottom - 1;
			e.Graphics.DrawLine(SystemPens.ControlDark, rc.Left, y, rc.Right, y);
		}

		#endregion
	}
}
