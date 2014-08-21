using System.Collections.Generic;
using System.Drawing;
using L10NSharp;
using System.Windows.Forms;
using Palaso.Reporting;
using SayMore.Properties;
using SayMore.UI.Overview.Statistics;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.Overview
{
	public partial class ProgressScreen : UserControl, ISayMoreView
	{
		private StatisticsView _statsView;
		private readonly ToolStripMenuItem _mnuProgress;

		/// ------------------------------------------------------------------------------------
		public ProgressScreen(StatisticsViewModel statisticsModel)
		{
			Logger.WriteEvent("ProgressScreen constructor");

			InitializeComponent();
			_statsView = new StatisticsView(statisticsModel) {Dock = DockStyle.Fill};
			Controls.Add(_statsView);

			_mnuProgress = new ToolStripMenuItem
			{
				Text = LocalizationManager.GetString("ProgressView.ProgressMainMenuItemText",
					"Pr&ogress", null, MainMenuItem)
			};

			var menu = new ToolStripMenuItem(null, Resources.Copy, _statsView.HandleCopyToClipboardClick);
			menu.Text = LocalizationManager.GetString("ProgressView.CopyMenuItemText",
				"&Copy", null, "Copy entire view to clipboard", null, menu);

			_mnuProgress.DropDown.Items.Add(menu);

			menu = new ToolStripMenuItem(null, Resources.Save, _statsView.HandleSaveButtonClicked);
			menu.Text = LocalizationManager.GetString("ProgressView.SaveMenuItemText",
				"&Save...", null, "Save view to file", null, menu);

			_mnuProgress.DropDown.Items.Add(menu);

			menu = new ToolStripMenuItem(null, Resources.Print, _statsView.HandlePrintButtonClicked);
			menu.Text = LocalizationManager.GetString("ProgressView.PrintMenuItemText",
				"&Print...", null, null, null, menu);

			_mnuProgress.DropDown.Items.Add(menu);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();
				if (_statsView != null)
				{
					_statsView.Dispose();
					_statsView = null;
				}
				if (_mnuProgress != null)
					_mnuProgress.Dispose();
			}

			base.Dispose(disposing);
		}

		#region ISayMoreView Members
		/// ------------------------------------------------------------------------------------
		public void AddTabToTabGroup(ViewTabGroup viewTabGroup)
		{
			//var tab = viewTabGroup.AddTab(this);
			//tab.Text = LocalizationManager.GetString("ProgressView.TabText", "Progress", null, "Progress View", null, tab);
			//Text = tab.Text;
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ToolStripMenuItem> GetMainMenuCommands()
		{
			return null;
		}

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
		public Image Image
		{
			get { return Resources.Progress; }
		}

		/// ------------------------------------------------------------------------------------
		public ToolStripMenuItem MainMenuItem
		{
			get { return _mnuProgress; }
		}

		#endregion
	}
}
