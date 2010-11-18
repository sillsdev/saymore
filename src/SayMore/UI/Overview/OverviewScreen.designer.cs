namespace SayMore.UI.Overview
{
	partial class OverviewScreen
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OverviewScreen));
			this.tsbStatistics = new System.Windows.Forms.ToolStripButton();
			this.tsbGenre = new System.Windows.Forms.ToolStripButton();
			this.tsbContributor = new System.Windows.Forms.ToolStripButton();
			this.tsbTasks = new System.Windows.Forms.ToolStripButton();
			this._tabControl = new System.Windows.Forms.TabControl();
			this._tabPageOverview = new System.Windows.Forms.TabPage();
			this._tabPageCharts = new System.Windows.Forms.TabPage();
			this._webBrowser = new System.Windows.Forms.WebBrowser();
			this._tabControl.SuspendLayout();
			this._tabPageCharts.SuspendLayout();
			this.SuspendLayout();
			//
			// tsbStatistics
			//
			this.tsbStatistics.AutoSize = false;
			this.tsbStatistics.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbStatistics.Image = ((System.Drawing.Image)(resources.GetObject("tsbStatistics.Image")));
			this.tsbStatistics.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbStatistics.Margin = new System.Windows.Forms.Padding(10, 5, 10, 2);
			this.tsbStatistics.Name = "tsbStatistics";
			this.tsbStatistics.Size = new System.Drawing.Size(100, 24);
			this.tsbStatistics.Text = "Statistics";
			this.tsbStatistics.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// tsbGenre
			//
			this.tsbGenre.AutoSize = false;
			this.tsbGenre.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbGenre.Image = ((System.Drawing.Image)(resources.GetObject("tsbGenre.Image")));
			this.tsbGenre.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbGenre.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
			this.tsbGenre.Name = "tsbGenre";
			this.tsbGenre.Size = new System.Drawing.Size(100, 24);
			this.tsbGenre.Text = "By Genre";
			this.tsbGenre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// tsbContributor
			//
			this.tsbContributor.AutoSize = false;
			this.tsbContributor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbContributor.Image = ((System.Drawing.Image)(resources.GetObject("tsbContributor.Image")));
			this.tsbContributor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbContributor.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
			this.tsbContributor.Name = "tsbContributor";
			this.tsbContributor.Size = new System.Drawing.Size(100, 24);
			this.tsbContributor.Text = "By Contributor";
			this.tsbContributor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// tsbTasks
			//
			this.tsbTasks.AutoSize = false;
			this.tsbTasks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbTasks.Image = ((System.Drawing.Image)(resources.GetObject("tsbTasks.Image")));
			this.tsbTasks.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbTasks.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
			this.tsbTasks.Name = "tsbTasks";
			this.tsbTasks.Size = new System.Drawing.Size(100, 24);
			this.tsbTasks.Text = "Event Tasks";
			this.tsbTasks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// _tabControl
			//
			this._tabControl.Controls.Add(this._tabPageOverview);
			this._tabControl.Controls.Add(this._tabPageCharts);
			this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tabControl.Location = new System.Drawing.Point(10, 10);
			this._tabControl.Name = "_tabControl";
			this._tabControl.SelectedIndex = 0;
			this._tabControl.Size = new System.Drawing.Size(637, 331);
			this._tabControl.TabIndex = 0;
			//
			// _tabPageOverview
			//
			this._tabPageOverview.Location = new System.Drawing.Point(4, 22);
			this._tabPageOverview.Name = "_tabPageOverview";
			this._tabPageOverview.Padding = new System.Windows.Forms.Padding(3);
			this._tabPageOverview.Size = new System.Drawing.Size(629, 305);
			this._tabPageOverview.TabIndex = 0;
			this._tabPageOverview.Text = "Overview";
			this._tabPageOverview.UseVisualStyleBackColor = true;
			//
			// _tabPageCharts
			//
			this._tabPageCharts.Controls.Add(this._webBrowser);
			this._tabPageCharts.Location = new System.Drawing.Point(4, 22);
			this._tabPageCharts.Name = "_tabPageCharts";
			this._tabPageCharts.Padding = new System.Windows.Forms.Padding(7);
			this._tabPageCharts.Size = new System.Drawing.Size(629, 305);
			this._tabPageCharts.TabIndex = 1;
			this._tabPageCharts.Text = "Charts";
			this._tabPageCharts.UseVisualStyleBackColor = true;
			//
			// _webBrowser
			//
			this._webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._webBrowser.Location = new System.Drawing.Point(7, 7);
			this._webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this._webBrowser.Name = "_webBrowser";
			this._webBrowser.Size = new System.Drawing.Size(615, 291);
			this._webBrowser.TabIndex = 4;
			//
			// OverviewScreen
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tabControl);
			this.Name = "OverviewScreen";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.Size = new System.Drawing.Size(657, 351);
			this._tabControl.ResumeLayout(false);
			this._tabPageCharts.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripButton tsbStatistics;
		private System.Windows.Forms.ToolStripButton tsbGenre;
		private System.Windows.Forms.ToolStripButton tsbContributor;
		private System.Windows.Forms.ToolStripButton tsbTasks;
		private System.Windows.Forms.TabControl _tabControl;
		private System.Windows.Forms.TabPage _tabPageOverview;
		private System.Windows.Forms.TabPage _tabPageCharts;
		private System.Windows.Forms.WebBrowser _webBrowser;
	}
}
