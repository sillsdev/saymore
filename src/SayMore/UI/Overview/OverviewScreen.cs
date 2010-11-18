using System.Windows.Forms;
using SayMore.Properties;
using SayMore.UI.Charts;
using SayMore.UI.Overview.Statistics;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.Overview
{
	public partial class OverviewScreen : UserControl, ISayMoreView
	{
		protected HTMLChartBuilder _chartBuilder;

		/// ------------------------------------------------------------------------------------
		public OverviewScreen(StatisticsViewModel statisticsModel, HTMLChartBuilder chartBuilder)
		{
			InitializeComponent();
			var statisticsView = new StatisticsView(statisticsModel);
			statisticsView.Dock = DockStyle.Fill;
			_tabPageOverview.Controls.Add(statisticsView);

			_chartBuilder = chartBuilder;
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
			_webBrowser.DocumentText = _chartBuilder.GetGenreChart();
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
