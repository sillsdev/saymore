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
			this.tsOptions = new SIL.Sponge.Controls.SpongeBar();
			this.tslblCreate = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbCreate = new System.Windows.Forms.ToolStripButton();
			this.tslblOpen = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbMru0 = new System.Windows.Forms.ToolStripButton();
			this.tsbBrowse = new System.Windows.Forms.ToolStripButton();
			this.lnkSpongeWebSite = new System.Windows.Forms.LinkLabel();
			this.lnkSIL = new System.Windows.Forms.LinkLabel();
			this.lblProdInfo1 = new System.Windows.Forms.Label();
			this.lblVersionInfo = new System.Windows.Forms.Label();
			this.lblProdInfo2 = new System.Windows.Forms.Label();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
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
			this.tsOptions.BackColorBegin = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(208)))), ((int)(((byte)(229)))));
			this.tsOptions.BackColorEnd = System.Drawing.Color.SteelBlue;
			this.tsOptions.GradientAngle = 0F;
			this.tsOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblCreate,
            this.toolStripSeparator1,
            this.tsbCreate,
            this.tslblOpen,
            this.toolStripSeparator2,
            this.tsbMru0,
            this.tsbBrowse});
			this.tsOptions.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
			this.locExtender.SetLocalizableToolTip(this.tsOptions, null);
			this.locExtender.SetLocalizationComment(this.tsOptions, null);
			this.locExtender.SetLocalizingId(this.tsOptions, "WelcomeDlg.tsOptions");
			this.tsOptions.Name = "tsOptions";
			this.tsOptions.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
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
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
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
			// tslblOpen
			// 
			resources.ApplyResources(this.tslblOpen, "tslblOpen");
			this.tslblOpen.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.locExtender.SetLocalizableToolTip(this.tslblOpen, null);
			this.locExtender.SetLocalizationComment(this.tslblOpen, null);
			this.locExtender.SetLocalizingId(this.tslblOpen, "WelcomeDlg.tslblOpen");
			this.tslblOpen.Name = "tslblOpen";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
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
			// lnkSpongeWebSite
			// 
			resources.ApplyResources(this.lnkSpongeWebSite, "lnkSpongeWebSite");
			this.locExtender.SetLocalizableToolTip(this.lnkSpongeWebSite, null);
			this.locExtender.SetLocalizationComment(this.lnkSpongeWebSite, null);
			this.locExtender.SetLocalizingId(this.lnkSpongeWebSite, "WelcomeDlg.lnkSpongeWebSite");
			this.lnkSpongeWebSite.Name = "lnkSpongeWebSite";
			this.lnkSpongeWebSite.TabStop = true;
			this.lnkSpongeWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSpongeWebSite_LinkClicked);
			// 
			// lnkSIL
			// 
			resources.ApplyResources(this.lnkSIL, "lnkSIL");
			this.locExtender.SetLocalizableToolTip(this.lnkSIL, null);
			this.locExtender.SetLocalizationComment(this.lnkSIL, null);
			this.locExtender.SetLocalizingId(this.lnkSIL, "WelcomeDlg.lnkSIL");
			this.lnkSIL.Name = "lnkSIL";
			this.lnkSIL.TabStop = true;
			this.lnkSIL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSIL_LinkClicked);
			// 
			// lblProdInfo1
			// 
			resources.ApplyResources(this.lblProdInfo1, "lblProdInfo1");
			this.locExtender.SetLocalizableToolTip(this.lblProdInfo1, null);
			this.locExtender.SetLocalizationComment(this.lblProdInfo1, null);
			this.locExtender.SetLocalizingId(this.lblProdInfo1, "WelcomeDlg.lblProdInfo1");
			this.lblProdInfo1.Name = "lblProdInfo1";
			// 
			// lblVersionInfo
			// 
			resources.ApplyResources(this.lblVersionInfo, "lblVersionInfo");
			this.locExtender.SetLocalizableToolTip(this.lblVersionInfo, null);
			this.locExtender.SetLocalizationComment(this.lblVersionInfo, null);
			this.locExtender.SetLocalizingId(this.lblVersionInfo, "WelcomeDlg.lblVersionInfo");
			this.lblVersionInfo.Name = "lblVersionInfo";
			// 
			// lblProdInfo2
			// 
			resources.ApplyResources(this.lblProdInfo2, "lblProdInfo2");
			this.locExtender.SetLocalizableToolTip(this.lblProdInfo2, null);
			this.locExtender.SetLocalizationComment(this.lblProdInfo2, null);
			this.locExtender.SetLocalizingId(this.lblProdInfo2, "WelcomeDlg.lblProdInfo2");
			this.lblProdInfo2.Name = "lblProdInfo2";
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
			// WelcomeDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lnkSpongeWebSite);
			this.Controls.Add(this.lnkSIL);
			this.Controls.Add(this.lblProdInfo1);
			this.Controls.Add(this.lblVersionInfo);
			this.Controls.Add(this.lblProdInfo2);
			this.Controls.Add(this.pnlOptions);
			this.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "WelcomeDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WelcomeDlg";
			this.pnlOptions.ResumeLayout(false);
			this.tsOptions.ResumeLayout(false);
			this.tsOptions.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
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
		private System.Windows.Forms.LinkLabel lnkSpongeWebSite;
		private System.Windows.Forms.LinkLabel lnkSIL;
		private System.Windows.Forms.Label lblProdInfo1;
		private System.Windows.Forms.Label lblVersionInfo;
		private System.Windows.Forms.Label lblProdInfo2;
		private System.Windows.Forms.Label lblSubTitle;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;


	}
}