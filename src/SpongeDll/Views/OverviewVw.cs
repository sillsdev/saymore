using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
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
		public OverviewVw()
		{
			InitializeComponent();

			gridGenre.Rows.Add(10);
			gridTasks.Rows.Add(10);

			gridGenre.AlternatingRowsDefaultCellStyle.BackColor =
				ColorHelper.CalculateColor(Color.Black, gridGenre.DefaultCellStyle.BackColor, 10);

			gridTasks.AlternatingRowsDefaultCellStyle.BackColor =
				ColorHelper.CalculateColor(Color.Black, gridTasks.DefaultCellStyle.BackColor, 10);

			m_viewBtnManger = new ViewButtonManager(tsOverview,
				new Control[] { pnlStatistics, gridGenre, pnlContributor, gridTasks });

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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make background blue gradient.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tsOverview_Paint(object sender, PaintEventArgs e)
		{
			Color clr1 = ColorHelper.CalculateColor(Color.LightSteelBlue, Color.White, 200);
			Color clr2 = Color.SteelBlue;

			using (var br = new LinearGradientBrush(tsOverview.ClientRectangle, clr1, clr2, 90))
				e.Graphics.FillRectangle(br, tsOverview.ClientRectangle);
		}
	}
}
