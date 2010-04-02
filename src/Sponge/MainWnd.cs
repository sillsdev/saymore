using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
using SIL.Sponge.ConfigTools;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;
using SIL.Sponge.Views;
using SIL.Sponge.Views.Overview.Statistics;
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
		private OverviewVw _overviewView;
		private SessionsVw _sessionsView;
		private PeopleVw _peopleView;
		private SendReceiveVw _sendReceiveView;
		private SetupVw _setupView;
		private ViewButtonManager _viewManger;
		private BackgroundStatisticsMananager _backgroundStatisticsGather;

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
			_viewManger.SetView(tsbOverview);
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

			SpongeProject project = CurrentProject;
			_backgroundStatisticsGather = new BackgroundStatisticsMananager(project.SessionsFolder);
			_backgroundStatisticsGather.Start();

			var statisticsModel = new StatisticsViewModel(project, _backgroundStatisticsGather );

			_overviewView = new OverviewVw(project, statisticsModel);
			_sessionsView = new SessionsVw(CurrentProject, CurrentProject.GetPeopleNames);
			_peopleView = new PeopleVw(CurrentProject);
			_sendReceiveView = new SendReceiveVw();
			_setupView = new SetupVw();

			var views = new Control[] {_overviewView,  _sessionsView,
				_peopleView, _sendReceiveView, _setupView };

			Controls.AddRange(views);
			_viewManger = new ViewButtonManager(tsMain, views);

		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Remove and dispose of existing view controls.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CleanOutViews()
		{

			if (_overviewView != null)
			{
				Controls.Remove(_overviewView);
				Controls.Remove(_sessionsView);
				Controls.Remove(_peopleView);
				Controls.Remove(_sendReceiveView);
				Controls.Remove(_setupView);

				_overviewView.Dispose();
				_sessionsView.Dispose();
				_peopleView.Dispose();
				_sendReceiveView.Dispose();
				_setupView.Dispose();
			}

			if (_viewManger != null)
				_viewManger.Dispose();

			if (_backgroundStatisticsGather != null)
			{
				_backgroundStatisticsGather.Dispose();
				_backgroundStatisticsGather = null;
			}
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
			foreach (var vw in _viewManger.Views)
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
