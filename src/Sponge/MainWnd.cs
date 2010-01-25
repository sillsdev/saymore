using System.Drawing;
using System.Windows.Forms;
using SIL.Sponge.ConfigTools;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;

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
		/// Gets the current Sponge project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SpongeProject CurrentProject { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="MainWnd"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MainWnd()
		{
			InitializeComponent();

			if (Settings.Default.MainWndSize.IsEmpty && Settings.Default.MainWndLocation.IsEmpty)
				StartPosition = FormStartPosition.CenterScreen;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="MainWnd"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MainWnd(SpongeProject prj) : this()
		{
			CurrentProject = prj;
			SetupViews();
			m_viewManger.SetView(tsbSetup);
			Text = string.Format(Text, prj.ProjectName);
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
		/// Raises the <see cref="E:System.Windows.Forms.Form.Shown"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(System.EventArgs e)
		{
			base.OnShown(e);

			if (!Settings.Default.MainWndSize.IsEmpty && !Settings.Default.MainWndLocation.IsEmpty)
			{
				// It works better to set these values here rather than in the constructor.
				Size = Settings.Default.MainWndSize;
				Location = Settings.Default.MainWndLocation;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			Settings.Default.MainWndSize = Size;
			Settings.Default.MainWndLocation = Location;
			Settings.Default.Save();
		}
	}
}
