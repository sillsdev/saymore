using System.Windows.Forms;
using SayMore.Properties;
using SayMore.UI.Overview.Statistics;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.Overview
{
	public partial class OverviewScreen : UserControl, ISayMoreView
	{
		/// ------------------------------------------------------------------------------------
		public OverviewScreen(StatisticsViewModel statisticsModel)
		{
			InitializeComponent();
			var statisticsView = new StatisticsView(statisticsModel);
			statisticsView.Dock = DockStyle.Fill;
			Controls.Add(statisticsView);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
				components.Dispose();

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get { return "Progress"; }
			set { }
		}

		#region ISayMoreView Members
		/// ------------------------------------------------------------------------------------
		public void ViewActivated(bool firstTime)
		{
		}

		/// ------------------------------------------------------------------------------------
		public void ViewDeactivated()
		{
		}

		/// ------------------------------------------------------------------------------------
		public bool IsOKToLeaveView(bool showMsgWhenNotOK)
		{
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public System.Drawing.Image Image
		{
			get { return Resources.Progress; }
		}

		#endregion
	}
}
