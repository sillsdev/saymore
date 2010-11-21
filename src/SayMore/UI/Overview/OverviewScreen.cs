using System.Windows.Forms;
using SayMore.Properties;
using SayMore.UI.Overview.Statistics;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.Overview
{
	public partial class OverviewScreen : UserControl, ISayMoreView
	{
		protected StatisticsView _statsView;

		/// ------------------------------------------------------------------------------------
		public OverviewScreen(StatisticsViewModel statisticsModel)
		{
			InitializeComponent();
			_statsView = new StatisticsView(statisticsModel);
			_statsView.Dock = DockStyle.Fill;
			Controls.Add(_statsView);
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
			if (firstTime)
				_statsView.InitializeView();
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
