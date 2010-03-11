using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
using SIL.Sponge.ConfigTools;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;
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
		private OverviewVw m_overviewView;
		private SessionsVw m_sessionsView;
		private PeopleVw m_peopleView;
		private SendReceiveVw m_sendReceiveView;
		private SetupVw m_setupView;
		private ViewButtonManager m_viewManger;

		public static bool Resizing { get; private set; }

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

			if (Settings.Default.MainWndBounds.Height < 0)
				StartPosition = FormStartPosition.CenterScreen;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="MainWnd"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MainWnd(SpongeProject prj) : this()
		{
			Initialize(prj);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the main window for the specified project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Initialize(SpongeProject prj)
		{
			if (CurrentProject != null)
				CurrentProject.Dispose();

			CurrentProject = prj;
			CurrentProject.FileWatcherSynchronizingObject = this;
			SetupViews();
			m_viewManger.SetView(tsbOverview);
			SetWindowText();
			LocalizeItemDlg.StringsLocalized -= SetWindowText;
			LocalizeItemDlg.StringsLocalized += SetWindowText;
			Show();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create and load all the views.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupViews()
		{
			CleanOutViews();

			foreach (ToolStripButton btn in tsMain.Items)
				btn.Checked = false;

			m_overviewView = new OverviewVw(CurrentProject);
			m_sessionsView = new SessionsVw(CurrentProject, CurrentProject.GetPeopleNames);
			m_peopleView = new PeopleVw(CurrentProject);
			m_sendReceiveView = new SendReceiveVw();
			m_setupView = new SetupVw();

			var views = new Control[] { m_overviewView, m_sessionsView,
				m_peopleView, m_sendReceiveView, m_setupView };

			Controls.AddRange(views);
			m_viewManger = new ViewButtonManager(tsMain, views);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Remove and dispose of existing view controls.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CleanOutViews()
		{
			if (m_overviewView != null)
			{
				Controls.Remove(m_overviewView);
				Controls.Remove(m_sessionsView);
				Controls.Remove(m_peopleView);
				Controls.Remove(m_sendReceiveView);
				Controls.Remove(m_setupView);

				m_overviewView.Dispose();
				m_sessionsView.Dispose();
				m_peopleView.Dispose();
				m_sendReceiveView.Dispose();
				m_setupView.Dispose();
			}

			if (m_viewManger != null)
				m_viewManger.Dispose();
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
				CleanOutViews();
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the localized window title texts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetWindowText()
		{
			var fmt = LocalizationManager.GetString(this);
			Text = string.Format(fmt, CurrentProject.Name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);

			// Do this here because it doesn't work in the constructor.
			if (Settings.Default.MainWndBounds.Height >= 0)
				Bounds = Settings.Default.MainWndBounds;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			Settings.Default.MainWndBounds = Bounds;
			Settings.Default.Save();
			LocalizeItemDlg.StringsLocalized -= SetWindowText;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.ResizeBegin"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResizeBegin(System.EventArgs e)
		{
			if (!Settings.Default.RedrawAsMainWndResizes)
				Utils.SetWindowRedraw(this, false);
			else
				Resizing = true;

			base.OnResizeBegin(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.ResizeEnd"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResizeEnd(System.EventArgs e)
		{
			if (!Settings.Default.RedrawAsMainWndResizes)
				Utils.SetWindowRedraw(this, true);
			else
			{
				Resizing = false;
				Invalidate(true);
			}
			base.OnResizeEnd(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show the welcome dialog to allow the user to choose another project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tsbChangeProjects_Click(object sender, System.EventArgs e)
		{
			foreach (var vw in m_viewManger.Views)
			{
				if (!vw.IsOKToLeaveView(true))
					return;
			}

			Hide();

			using (var dlg = new WelcomeDlg())
			{
				if (dlg.ShowDialog() != DialogResult.OK || dlg.Project == null ||
					dlg.Project.Name == CurrentProject.Name)
				{
					Show();
				}
				else
				{
					Initialize(dlg.Project);
					Focus();
				}
			}
		}
	}
}
