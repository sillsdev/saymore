using Localization;
using Localization.UI;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ProjectChoosingAndCreating
{
	partial class WelcomeDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeDialog));
			this.pnlOptions = new System.Windows.Forms.Panel();
			this.tsOptions = new SayMore.UI.LowLevelControls.ElementBar();
			this.tslblOpen = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._buttonMru0 = new System.Windows.Forms.ToolStripButton();
			this._buttonBrowse = new System.Windows.Forms.ToolStripButton();
			this.tslblCreate = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this._buttonCreate = new System.Windows.Forms.ToolStripButton();
			this.lnkWebSites = new System.Windows.Forms.LinkLabel();
			this.lblVersionInfo = new System.Windows.Forms.Label();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.pnlOptions.SuspendLayout();
			this.tsOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlOptions
			// 
			resources.ApplyResources(this.pnlOptions, "pnlOptions");
			this.pnlOptions.BackColor = System.Drawing.Color.LightGray;
			this.pnlOptions.Controls.Add(this.tsOptions);
			this.pnlOptions.Name = "pnlOptions";
			// 
			// tsOptions
			// 
			resources.ApplyResources(this.tsOptions, "tsOptions");
			this.tsOptions.BackColor = System.Drawing.Color.White;
			this.tsOptions.GradientAngle = 0F;
			this.tsOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblOpen,
            this.toolStripSeparator1,
            this._buttonMru0,
            this._buttonBrowse,
            this.tslblCreate,
            this.toolStripSeparator2,
            this._buttonCreate});
			this.tsOptions.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
			this.tsOptions.Name = "tsOptions";
			// 
			// tslblOpen
			// 
			resources.ApplyResources(this.tslblOpen, "tslblOpen");
			this.tslblOpen.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.tslblOpen.Name = "tslblOpen";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// _buttonMru0
			// 
			resources.ApplyResources(this._buttonMru0, "_buttonMru0");
			this._buttonMru0.Image = global::SayMore.Properties.Resources.SmallSayMoreLogo;
			this._buttonMru0.Margin = new System.Windows.Forms.Padding(4, 1, 4, 2);
			this._buttonMru0.Name = "_buttonMru0";
			this._buttonMru0.Click += new System.EventHandler(this.HandleMruClick);
			// 
			// _buttonBrowse
			// 
			resources.ApplyResources(this._buttonBrowse, "_buttonBrowse");
			this._buttonBrowse.Image = global::SayMore.Properties.Resources.kimidBrowseForProject;
			this._buttonBrowse.Margin = new System.Windows.Forms.Padding(4, 1, 4, 15);
			this._buttonBrowse.Name = "_buttonBrowse";
			this._buttonBrowse.Click += new System.EventHandler(this.HandleBrowseForExistingProjectClick);
			// 
			// tslblCreate
			// 
			resources.ApplyResources(this.tslblCreate, "tslblCreate");
			this.tslblCreate.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.tslblCreate.Name = "tslblCreate";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// _buttonCreate
			// 
			resources.ApplyResources(this._buttonCreate, "_buttonCreate");
			this._buttonCreate.Image = global::SayMore.Properties.Resources.kimidNewProject;
			this._buttonCreate.Margin = new System.Windows.Forms.Padding(4, 1, 4, 15);
			this._buttonCreate.Name = "_buttonCreate";
			this._buttonCreate.Click += new System.EventHandler(this.HandleCreateProjectClick);
			// 
			// lnkWebSites
			// 
			this.lnkWebSites.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			resources.ApplyResources(this.lnkWebSites, "lnkWebSites");
			this.lnkWebSites.Name = "lnkWebSites";
			this.lnkWebSites.UseCompatibleTextRendering = true;
			this.lnkWebSites.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleWebSiteLinkClicked);
			// 
			// lblVersionInfo
			// 
			resources.ApplyResources(this.lblVersionInfo, "lblVersionInfo");
			this.lblVersionInfo.Name = "lblVersionInfo";
			// 
			// lblSubTitle
			// 
			resources.ApplyResources(this.lblSubTitle, "lblSubTitle");
			this.lblSubTitle.AutoEllipsis = true;
			this.lblSubTitle.Name = "lblSubTitle";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// WelcomeDialog
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lnkWebSites);
			this.Controls.Add(this.lblVersionInfo);
			this.Controls.Add(this.pnlOptions);
			this.DoubleBuffered = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WelcomeDialog";
			this.ShowIcon = false;
			this.pnlOptions.ResumeLayout(false);
			this.tsOptions.ResumeLayout(false);
			this.tsOptions.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlOptions;
		private ElementBar tsOptions;
		private System.Windows.Forms.ToolStripLabel tslblCreate;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton _buttonCreate;
		private System.Windows.Forms.ToolStripLabel tslblOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton _buttonMru0;
		private System.Windows.Forms.ToolStripButton _buttonBrowse;
		private System.Windows.Forms.LinkLabel lnkWebSites;
		private System.Windows.Forms.Label lblVersionInfo;
		private System.Windows.Forms.Label lblSubTitle;
		private LocalizationExtender locExtender;
	}
}