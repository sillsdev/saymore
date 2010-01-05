using System;
using System.Drawing;
using SilUtils;

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Container for the sessions view.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class SessionsVw : BaseSplitVw
	{
		public SessionsVw()
		{
			InitializeComponent();

			gridFiles.Rows.Add(10);

			gridFiles.AlternatingRowsDefaultCellStyle.BackColor =
				ColorHelper.CalculateColor(Color.Black, gridFiles.DefaultCellStyle.BackColor, 10);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This fixes a paint error in .Net that manifests itself when tab controls are
		/// resized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tabSessions_SizeChanged(object sender, EventArgs e)
		{
			tabSessions.Invalidate();
		}
	}
}
