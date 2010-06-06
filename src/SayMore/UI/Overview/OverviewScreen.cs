using System.Windows.Forms;
using SayMore.UI.Overview.Statistics;


namespace SayMore.UI.Overview
{
	public partial class OverviewScreen : UserControl
	{

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="OverviewScreen"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public OverviewScreen(StatisticsViewModel statisticsModel)
		{
			InitializeComponent();
			var statisticsView = new StatisticsView(statisticsModel);
			statisticsView.Dock = DockStyle.Fill;
			this.Controls.Add(statisticsView);

		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
