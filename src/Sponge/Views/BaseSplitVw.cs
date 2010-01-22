using System.Windows.Forms;

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Used as the base class for all views needing slit panels
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class BaseSplitVw : UserControl
	{
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
	}
}
