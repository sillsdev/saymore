using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class for the main window of the application.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class MainWnd : Form
	{
		private SetupVw m_setupView;
		private OverviewVw m_overviewView;
		private SessionsVw m_sessionsView;
		private PeopleVw m_peopleView;
		private SendReceiveVw m_sendReceiveView;
		private ViewButtonManager m_viewManger;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="MainWnd"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MainWnd()
		{
			InitializeComponent();
			SetupViews();
			m_viewManger.SetView(tsbSetup);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create and load all the views.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupViews()
		{
			m_setupView = new SetupVw();
			m_overviewView = new OverviewVw();
			m_sessionsView = new SessionsVw();
			m_peopleView = new PeopleVw();
			m_sendReceiveView = new SendReceiveVw();

			Controls.AddRange(new Control[] { m_setupView, m_overviewView,
				m_sessionsView, m_peopleView, m_sendReceiveView });

			m_viewManger = new ViewButtonManager(tsMain,
				new Control[] { m_setupView, m_overviewView, m_sessionsView, m_peopleView, m_sendReceiveView });
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
				m_viewManger.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint the background of the toolbar our own way.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tsMain_Paint(object sender, PaintEventArgs e)
		{
			Color clr1 = ColorHelper.CalculateColor(Color.LightSteelBlue, Color.White, 200);
			Color clr2 = Color.SteelBlue;

			using (var br = new LinearGradientBrush(tsMain.ClientRectangle, clr1, clr2, 20))
				e.Graphics.FillRectangle(br, tsMain.ClientRectangle);
		}
	}
}
