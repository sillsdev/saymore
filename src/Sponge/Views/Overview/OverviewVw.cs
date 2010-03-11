using System.Drawing;
using System.Windows.Forms;
using SIL.Sponge.Model;
using SIL.Sponge.Views;
using SIL.Sponge.Views.Overview.Statistics;
using SilUtils;

namespace SIL.Sponge
{
	public partial class OverviewVw : UserControl
	{
		private readonly ViewButtonManager m_viewBtnManger;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="OverviewVw"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public OverviewVw(SpongeProject project)
		{
			InitializeComponent();

			gridGenre.Rows.Add(10);
			gridTasks.Rows.Add(10);

			gridGenre.AlternatingRowsDefaultCellStyle.BackColor =
				ColorHelper.CalculateColor(Color.Black, gridGenre.DefaultCellStyle.BackColor, 10);

			gridTasks.AlternatingRowsDefaultCellStyle.BackColor =
				ColorHelper.CalculateColor(Color.Black, gridTasks.DefaultCellStyle.BackColor, 10);

			var statisticsView = new StatisticsView(new StatisticsViewModel(project));
			this.Controls.Add(statisticsView);

			m_viewBtnManger = new ViewButtonManager(tsOverview,
				new Control[] { statisticsView, gridGenre, pnlContributor, gridTasks });

			m_viewBtnManger.SetView(tsbStatistics);
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
				m_viewBtnManger.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
