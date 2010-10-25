using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace SayMore.UI.LowLevelControls
{
	public partial class PopupControl : UserControl
	{
		public CancelEventHandler PopupOpening;
		public ToolStripDropDownClosingEventHandler PopupClosing;

		protected ToolStripControlHost _controlHost;
		protected ToolStripDropDown _dropDown;

		/// ------------------------------------------------------------------------------------
		public PopupControl()
		{
			if (!DesignMode)
				SetupPopup();

			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		private void SetupPopup()
		{
			_controlHost = new ToolStripControlHost(this)
			{
				AutoSize = false,
				Padding = Padding.Empty,
				Margin = Padding.Empty
			};

			_dropDown = new ToolStripDropDown
			{
				AutoSize = false,
				Padding = Padding.Empty,
				LayoutStyle = ToolStripLayoutStyle.Table
			};

			_dropDown.Closing += HandlePopupClosing;
			_dropDown.Opening += HandlePopupOpening;
			_dropDown.Items.Add(_controlHost);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the control is currently in design mode.
		/// I have had some problems with the base class' DesignMode property being true
		/// when in design mode. I'm not sure why, but adding a couple more checks fixes the
		/// problem.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected new bool DesignMode
		{
			get
			{
				return (base.DesignMode || GetService(typeof(IDesignerHost)) != null) ||
					(LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool IsShowing
		{
			get { return _dropDown.IsDropDown; }
		}

		#endregion

		#region Methods for showing, closing, loading and sizing popup
		/// ------------------------------------------------------------------------------------
		public virtual void ShowPopup(Point pt)
		{
			_dropDown.Show(pt);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void ClosePopup()
		{
			_dropDown.Close();
		}

		#endregion

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(System.EventArgs e)
		{
			base.OnSizeChanged(e);

			if (!DesignMode)
				_controlHost.Size = _dropDown.Size = Size;
		}

		/// ------------------------------------------------------------------------------------
		void HandlePopupOpening(object sender, CancelEventArgs e)
		{
			if (PopupOpening != null)
				PopupOpening(this, e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePopupClosing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (PopupClosing != null)
				PopupClosing(this, e);
		}

		#endregion
	}
}
