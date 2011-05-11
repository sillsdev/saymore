using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Properties;
using SayMore.UI.Overview.Statistics;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.Overview
{
	public partial class ProgressScreen : UserControl, ISayMoreView
	{
		protected StatisticsView _statsView;
		protected ToolStripMenuItem _mnuProgress;

		/// ------------------------------------------------------------------------------------
		public ProgressScreen(StatisticsViewModel statisticsModel)
		{
			InitializeComponent();
			_statsView = new StatisticsView(statisticsModel);
			_statsView.Dock = DockStyle.Fill;
			Controls.Add(_statsView);

			_mnuProgress = new ToolStripMenuItem("Pr&ogress");

			var menu = new ToolStripMenuItem("&Copy", Resources.Copy,
				_statsView.HandleCopyToClipboardClick);
			menu.ToolTipText = "Copy entire view to clipboard";
			_mnuProgress.DropDown.Items.Add(menu);

			menu = new ToolStripMenuItem("&Save...", Resources.Save,
				_statsView.HandleSaveButtonClicked);
			menu.ToolTipText = "Save view to file";
			_mnuProgress.DropDown.Items.Add(menu);

			_mnuProgress.DropDown.Items.Add("&Print...", Resources.Print,
				_statsView.HandlePrintButtonClicked);
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
