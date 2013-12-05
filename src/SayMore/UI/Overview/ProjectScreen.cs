using System.Collections.Generic;
using System.Drawing;
using L10NSharp;
using System.Windows.Forms;
using SayMore.Properties;
using SayMore.UI.Overview.Statistics;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.Overview
{
	public partial class ProjectScreen : UserControl, ISayMoreView
	{
		private readonly ProgressScreen _progressView;

		/// ------------------------------------------------------------------------------------
		public ProjectScreen(ProgressScreen progressView)
		{
			_progressView = progressView;
			InitializeComponent();

			_projectPages.AddRow(new object[] { Resources.Progress, LocalizationManager.GetString("ProjectView.ProgressViewTitle", "About This Project") });
			_projectPages.AddRow(new object[] { Resources.Progress, LocalizationManager.GetString("ProjectView.ProgressViewTitle", "Access Protocol") });
			_projectPages.AddRow(new object[] { Resources.Progress, LocalizationManager.GetString("ProjectView.ProgressViewTitle", "Progress") });
			_splitter.Panel2.BackColor = Color.FromArgb(230, 150, 100);
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

		#region ISayMoreView Members
		/// ------------------------------------------------------------------------------------
		public void AddTabToTabGroup(ViewTabGroup viewTabGroup)
		{
			var tab = viewTabGroup.AddTab(this);
			tab.Text = LocalizationManager.GetString("ProjectView.TabText", "Project", null, "Project View", null, tab);
			Text = tab.Text;
		}

		/// ------------------------------------------------------------------------------------
		public void ViewActivated(bool firstTime)
		{
		}

		/// ------------------------------------------------------------------------------------
		public void ViewDeactivated()
		{
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ToolStripMenuItem> GetMainMenuCommands()
		{
			return null;
		}

		/// ------------------------------------------------------------------------------------
		public bool IsOKToLeaveView(bool showMsgWhenNotOK)
		{
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public Image Image
		{
			get { return Resources.project; }
		}

		public ToolStripMenuItem MainMenuItem { get; private set; }
		#endregion

		private void _projectPages_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == 2)
				_splitter.Panel2.Controls.Add(_progressView);
		}
	}
}
