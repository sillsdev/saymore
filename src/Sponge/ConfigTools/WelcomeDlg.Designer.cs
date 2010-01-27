namespace SIL.Sponge.ConfigTools
{
	partial class WelcomeDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeDlg));
			this.pnlOptions = new System.Windows.Forms.Panel();
			this.lnkWebSites = new System.Windows.Forms.LinkLabel();
			this.lblVersionInfo = new System.Windows.Forms.Label();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.tsOptions = new SIL.Sponge.Controls.SpongeBar();
			this.tslblOpen = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbMru0 = new System.Windows.Forms.ToolStripButton();
			this.tsbBrowse = new System.Windows.Forms.ToolStripButton();
			this.tslblCreate = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbCreate = new System.Windows.Forms.ToolStripButton();
			this.pnlOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.tsOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlOptions
			// 
			resources.ApplyResources(this.pnlOptions, "pnlOptions");
			this.pnlOptions.BackColor = System.Drawing.Color.LightGray;
			this.pnlOptions.Controls.Add(this.tsOptions);
			this.pnlOptions.Name = "pnlOptions";
			// 
			// lnkWebSites
			// 
			this.lnkWebSites.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			resources.ApplyResources(this.lnkWebSites, "lnkWebSites");
			this.locExtender.SetLocalizableToolTip(this.lnkWebSites, null);
			this.locExtender.SetLocalizationComment(this.lnkWebSites, null);
			this.locExtender.SetLocalizingId(this.lnkWebSites, "WelcomeDlg.lnkSIL");
			this.lnkWebSites.Name = "lnkWebSites";
			this.lnkWebSites.UseCompatibleTextRendering = true;
			this.lnkWebSites.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWebSites_LinkClicked);
			// 
			// lblVersionInfo
			// 
			resources.ApplyResources(this.lblVersionInfo, "lblVersionInfo");
			this.locExtender.SetLocalizableToolTip(this.lblVersionInfo, null);
			this.locExtender.SetLocalizationComment(this.lblVersionInfo, null);
			this.locExtender.SetLocalizingId(this.lblVersionInfo, "WelcomeDlg.lblVersionInfo");
			this.lblVersionInfo.Name = "lblVersionInfo";
			// 
			// lblSubTitle
			// 
			resources.ApplyResources(this.lblSubTitle, "lblSubTitle");
			this.lblSubTitle.AutoEllipsis = true;
			this.locExtender.SetLocalizableToolTip(this.lblSubTitle, null);
			this.locExtender.SetLocalizationComment(this.lblSubTitle, null);
			this.locExtender.SetLocalizingId(this.lblSubTitle, "WelcomeDlg.lblSubTitle");
			this.lblSubTitle.Name = "lblSubTitle";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// tsOptions
			// 
			resources.ApplyResources(this.tsOptions, "tsOptions");
			this.tsOptions.BackColor = System.Drawing.Color.White;
			this.tsOptions.BackColorBegin = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(208)))), ((int)(((byte)(229)))));
			this.tsOptions.BackColorEnd = System.Drawing.Color.SteelBlue;
			this.tsOptions.GradientAngle = 0F;
			this.tsOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblOpen,
            this.toolStripSeparator1,
            this.tsbMru0,
            this.tsbBrowse,
            this.tslblCreate,
            this.toolStripSeparator2,
            this.tsbCreate});
			this.tsOptions.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
			this.locExtender.SetLocalizableToolTip(this.tsOptions, null);
			this.locExtender.SetLocalizationComment(this.tsOptions, null);
			this.locExtender.SetLocalizingId(this.tsOptions, "WelcomeDlg.tsOptions");
			this.tsOptions.Name = "tsOptions";
			this.tsOptions.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			// 
			// tslblOpen
			// 
			resources.ApplyResources(this.tslblOpen, "tslblOpen");
			this.tslblOpen.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.locExtender.SetLocalizableToolTip(this.tslblOpen, null);
			this.locExtender.SetLocalizationComment(this.tslblOpen, null);
			this.locExtender.SetLocalizingId(this.tslblOpen, "WelcomeDlg.tslblOpen");
			this.tslblOpen.Name = "tslblOpen";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// tsbMru0
			// 
			resources.ApplyResources(this.tsbMru0, "tsbMru0");
			this.tsbMru0.Image = global::SIL.Sponge.Properties.Resources.kimidSpongeSmall;
			this.locExtender.SetLocalizableToolTip(this.tsbMru0, null);
			this.locExtender.SetLocalizationComment(this.tsbMru0, null);
			this.locExtender.SetLocalizationPriority(this.tsbMru0, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tsbMru0, "WelcomeDlg.tsbMru0");
			this.tsbMru0.Margin = new System.Windows.Forms.Padding(4, 1, 4, 2);
			this.tsbMru0.Name = "tsbMru0";
			this.tsbMru0.Click += new System.EventHandler(this.tsbMru_Click);
			// 
			// tsbBrowse
			// 
			resources.ApplyResources(this.tsbBrowse, "tsbBrowse");
			this.tsbBrowse.Image = global::SIL.Sponge.Properties.Resources.kimidBrowseForProject;
			this.locExtender.SetLocalizableToolTip(this.tsbBrowse, null);
			this.locExtender.SetLocalizationComment(this.tsbBrowse, null);
			this.locExtender.SetLocalizingId(this.tsbBrowse, "WelcomeDlg.tsbBrowse");
			this.tsbBrowse.Margin = new System.Windows.Forms.Padding(4, 1, 4, 15);
			this.tsbBrowse.Name = "tsbBrowse";
			this.tsbBrowse.Click += new System.EventHandler(this.tsbBrowse_Click);
			// 
			// tslblCreate
			// 
			resources.ApplyResources(this.tslblCreate, "tslblCreate");
			this.tslblCreate.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.locExtender.SetLocalizableToolTip(this.tslblCreate, null);
			this.locExtender.SetLocalizationComment(this.tslblCreate, null);
			this.locExtender.SetLocalizingId(this.tslblCreate, "WelcomeDlg.tslblCreate");
			this.tslblCreate.Name = "tslblCreate";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// tsbCreate
			// 
			resources.ApplyResources(this.tsbCreate, "tsbCreate");
			this.tsbCreate.Image = global::SIL.Sponge.Properties.Resources.kimidNewProject;
			this.locExtender.SetLocalizableToolTip(this.tsbCreate, null);
			this.locExtender.SetLocalizationComment(this.tsbCreate, null);
			this.locExtender.SetLocalizingId(this.tsbCreate, "WelcomeDlg.tsbCreate");
			this.tsbCreate.Margin = new System.Windows.Forms.Padding(4, 1, 4, 15);
			this.tsbCreate.Name = "tsbCreate";
			this.tsbCreate.Click += new System.EventHandler(this.tsbCreate_Click);
			// 
			// WelcomeDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lnkWebSites);
			this.Controls.Add(this.lblVersionInfo);
			this.Controls.Add(this.pnlOptions);
			this.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "WelcomeDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WelcomeDlg";
			this.pnlOptions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.tsOptions.ResumeLayout(false);
			this.tsOptions.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlOptions;
		private SIL.Sponge.Controls.SpongeBar tsOptions;
		private System.Windows.Forms.ToolStripLabel tslblCreate;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tsbCreate;
		private System.Windows.Forms.ToolStripLabel tslblOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton tsbMru0;
		private System.Windows.Forms.ToolStripButton tsbBrowse;
		private System.Windows.Forms.LinkLabel lnkWebSites;
		private System.Windows.Forms.Label lblVersionInfo;
		private System.Windows.Forms.Label lblSubTitle;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;


	}
}