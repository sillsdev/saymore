using System.Windows.Forms;

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Used as the base class for all views needing slit panels
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class BaseSplitVw : UserControl, ISpongeView
	{
		protected bool m_isViewActive;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseSplitVw"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public BaseSplitVw()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to show the left side panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowSidePanel
		{
			get { return !splitOuter.Panel1Collapsed; }
			set { splitOuter.Panel1Collapsed = !value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to show the panel at the bottom
		/// of the right side.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowRightBottomPanel
		{
			get { return !splitRightSide.Panel2Collapsed; }
			set { splitRightSide.Panel2Collapsed = !value; }
		}

		#region ISpongeView Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether the view is active.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual bool IsViewActive
		{
			get { return m_isViewActive; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when views is activated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void ViewActivated(bool firstTime)
		{
			m_isViewActive = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when the view is deactivated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void ViewDeactivated()
		{
			m_isViewActive = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Allows a subclass to cleans up its view (e.g. save outstanding changes).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual bool IsOKToLeaveView(bool showMsgWhenNotOK)
		{
			return true;
		}

		#endregion
	}
}
