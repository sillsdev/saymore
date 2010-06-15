using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SilUtils;

namespace SayMore.UI.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface ISayMoreView
	{
		void ViewActivated(bool firstTime);
		void ViewDeactivated();
		//bool IsViewActive { get; }
		bool IsOKToLeaveView(bool showMsgWhenNotOK);
		Image Image { get; }
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class manages the association between buttons on a ToolStrip control and a
	/// collection of controls. When one of the buttons on the ToolStrip is pressed, its
	/// associated view is displayed.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ViewButtonManager : IDisposable
	{
		private ToolStrip _toolStrip;
		private Control _toolStripOwner;
		private Dictionary<ToolStripButton, Control> _controls;
		private Dictionary<Control, bool> _hasBeenActivatedList;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewButtonManager"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewButtonManager(ToolStrip toolstrip, IEnumerable<Control> ctrls)
		{
			Debug.Assert(toolstrip.Items.Count >= ctrls.Count());

			_toolStripOwner = toolstrip.TopLevelControl;
			_toolStrip = toolstrip;
			_toolStrip.ItemClicked += toolstrip_ItemClicked;
			_controls = new Dictionary<ToolStripButton, Control>();
			_hasBeenActivatedList = new Dictionary<Control, bool>();

			int i = 0;
			foreach (Control ctrl in ctrls)
			{
				ctrl.Dock = DockStyle.Fill;
				ctrl.Visible = false;
				Debug.Assert(_toolStrip.Items[i] is ToolStripButton);
				_controls[_toolStrip.Items[i++] as ToolStripButton] = ctrl;

				if (ctrl is ISayMoreView)
					_hasBeenActivatedList[ctrl] = false;
			}
		}

		#region IDisposable Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or
		/// resetting unmanaged resources.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_toolStrip != null)
				_toolStrip.ItemClicked -= toolstrip_ItemClicked;

			_toolStrip = null;
			_toolStripOwner = null;
			_controls = null;
			_hasBeenActivatedList = null;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets all the controls associated with buttons that implement the ISayMoreView
		/// interface.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<ISayMoreView> Views
		{
			get
			{
				return from x in _controls.Values
					   where x is ISayMoreView
					   select x as ISayMoreView;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current button for the current view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripButton CurrentViewButton
		{
			get
			{
				foreach (ToolStripButton btn in _toolStrip.Items)
				{
					if (btn.Checked)
						return btn;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles one of the view buttons being clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void toolstrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			SetView(e.ClickedItem as ToolStripButton);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the view associated with the specified view button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetView(ToolStripButton btn)
		{
			if (!_controls.ContainsKey(btn))
				return;

			ToolStripButton currbtn = CurrentViewButton;
			if (btn == currbtn)
				return;

			Control newVw = (btn == null ? null : _controls[btn]);

			// Now make sure we can leave the current view.
			foreach (var vw in Views)
			{
				var ctrl = vw as Control;
				if (ctrl != null && !ctrl.IsHandleCreated)
					continue;

				if (vw != newVw && !vw.IsOKToLeaveView(true))
					return;
			}

			if (_toolStripOwner != null)
				Utils.SetWindowRedraw(_toolStripOwner, false);

			if (currbtn != null && _controls.ContainsKey(currbtn))
			{
				currbtn.Checked = false;
				_controls[currbtn].Visible = false;

				if (_controls[currbtn] is ISayMoreView)
					((ISayMoreView)_controls[currbtn]).ViewDeactivated();
			}

			if (btn != null)
			{
				btn.Checked = true;
				newVw.Visible = true;
				newVw.BringToFront();
			}

			if (_toolStripOwner != null)
				Utils.SetWindowRedraw(_toolStripOwner, true);

			if (newVw is ISayMoreView)
			{
				((ISayMoreView)newVw).ViewActivated(!_hasBeenActivatedList[newVw]);
				_hasBeenActivatedList[newVw] = true;
			}
		}
	}
}
