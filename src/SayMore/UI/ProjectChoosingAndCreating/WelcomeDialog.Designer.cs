using L10NSharp;
using L10NSharp.UI;
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
			this.pnlOptions = new System.Windows.Forms.Panel();
			this.tsOptions = new SayMore.UI.LowLevelControls.ElementBar();
			this._labelOpen = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._buttonMru0 = new System.Windows.Forms.ToolStripButton();
			this._buttonBrowse = new System.Windows.Forms.ToolStripButton();
			this._labelCreate = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this._buttonCreate = new System.Windows.Forms.ToolStripButton();
			this._linkSILWebsite = new System.Windows.Forms.LinkLabel();
			this._labelVersionInfo = new System.Windows.Forms.Label();
			this._labelSubTitle = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._linkSayMoreWebsite = new System.Windows.Forms.LinkLabel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.pnlOptions.SuspendLayout();
			this.tsOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlOptions
			// 
			this.pnlOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlOptions.BackColor = System.Drawing.Color.LightGray;
			this.pnlOptions.Controls.Add(this.tsOptions);
			this.pnlOptions.Location = new System.Drawing.Point(30, 107);
			this.pnlOptions.Name = "pnlOptions";
			this.pnlOptions.Size = new System.Drawing.Size(418, 231);
			this.pnlOptions.TabIndex = 17;
			// 
			// tsOptions
			// 
			this.tsOptions.AutoSize = false;
			this.tsOptions.BackColor = System.Drawing.Color.White;
			this.tsOptions.BackColorBegin = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(209)))), ((int)(((byte)(227)))));
			this.tsOptions.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(153)))), ((int)(((byte)(193)))));
			this.tsOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tsOptions.GradientAngle = 0F;
			this.tsOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._labelOpen,
            this.toolStripSeparator1,
            this._buttonMru0,
            this._buttonBrowse,
            this._labelCreate,
            this.toolStripSeparator2,
            this._buttonCreate});
			this.tsOptions.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
			this.locExtender.SetLocalizableToolTip(this.tsOptions, null);
			this.locExtender.SetLocalizationComment(this.tsOptions, null);
			this.locExtender.SetLocalizingId(this.tsOptions, "DialogBoxes.WelcomeDlg.tsOptions");
			this.tsOptions.Location = new System.Drawing.Point(0, 0);
			this.tsOptions.Name = "tsOptions";
			this.tsOptions.Size = new System.Drawing.Size(418, 231);
			this.tsOptions.TabIndex = 0;
			// 
			// _labelOpen
			// 
			this._labelOpen.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
			this._labelOpen.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.locExtender.SetLocalizableToolTip(this._labelOpen, null);
			this.locExtender.SetLocalizationComment(this._labelOpen, null);
			this.locExtender.SetLocalizingId(this._labelOpen, "DialogBoxes.WelcomeDlg._labelOpen");
			this._labelOpen.Name = "_labelOpen";
			this._labelOpen.Size = new System.Drawing.Size(416, 20);
			this._labelOpen.Text = "Open";
			this._labelOpen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(416, 6);
			// 
			// _buttonMru0
			// 
			this._buttonMru0.Font = new System.Drawing.Font("Segoe UI", 9.75F);
			this._buttonMru0.Image = global::SayMore.Properties.Resources.SmallSayMoreLogo;
			this._buttonMru0.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonMru0.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonMru0, null);
			this.locExtender.SetLocalizationComment(this._buttonMru0, null);
			this.locExtender.SetLocalizationPriority(this._buttonMru0, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonMru0, "DialogBoxes.WelcomeDlg._buttonMru0");
			this._buttonMru0.Margin = new System.Windows.Forms.Padding(4, 1, 4, 2);
			this._buttonMru0.Name = "_buttonMru0";
			this._buttonMru0.Size = new System.Drawing.Size(408, 21);
			this._buttonMru0.Text = "#";
			this._buttonMru0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonMru0.Click += new System.EventHandler(this.HandleMruClick);
			// 
			// _buttonBrowse
			// 
			this._buttonBrowse.Font = new System.Drawing.Font("Segoe UI", 9.75F);
			this._buttonBrowse.Image = global::SayMore.Properties.Resources.kimidBrowseForProject;
			this._buttonBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonBrowse.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonBrowse, "Browse the file system for a project");
			this.locExtender.SetLocalizationComment(this._buttonBrowse, null);
			this.locExtender.SetLocalizingId(this._buttonBrowse, "DialogBoxes.WelcomeDlg._buttonBrowse");
			this._buttonBrowse.Margin = new System.Windows.Forms.Padding(4, 1, 4, 15);
			this._buttonBrowse.Name = "_buttonBrowse";
			this._buttonBrowse.Size = new System.Drawing.Size(408, 21);
			this._buttonBrowse.Text = "Browse for project...";
			this._buttonBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonBrowse.Click += new System.EventHandler(this.HandleBrowseForExistingProjectClick);
			// 
			// _labelCreate
			// 
			this._labelCreate.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
			this._labelCreate.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.locExtender.SetLocalizableToolTip(this._labelCreate, null);
			this.locExtender.SetLocalizationComment(this._labelCreate, null);
			this.locExtender.SetLocalizingId(this._labelCreate, "DialogBoxes.WelcomeDlg._labelCreate");
			this._labelCreate.Name = "_labelCreate";
			this._labelCreate.Size = new System.Drawing.Size(416, 20);
			this._labelCreate.Text = "Create";
			this._labelCreate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(416, 6);
			// 
			// _buttonCreate
			// 
			this._buttonCreate.Font = new System.Drawing.Font("Segoe UI", 9.75F);
			this._buttonCreate.Image = global::SayMore.Properties.Resources.kimidNewProject;
			this._buttonCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonCreate, "Create a blank project");
			this.locExtender.SetLocalizationComment(this._buttonCreate, null);
			this.locExtender.SetLocalizingId(this._buttonCreate, "DialogBoxes.WelcomeDlg._buttonCreate");
			this._buttonCreate.Margin = new System.Windows.Forms.Padding(4, 1, 4, 15);
			this._buttonCreate.Name = "_buttonCreate";
			this._buttonCreate.Size = new System.Drawing.Size(408, 21);
			this._buttonCreate.Text = "Create new, blank project...";
			this._buttonCreate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonCreate.Click += new System.EventHandler(this.HandleCreateProjectClick);
			// 
			// _linkSILWebsite
			// 
			this._linkSILWebsite.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this._linkSILWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._linkSILWebsite.AutoSize = true;
			this._linkSILWebsite.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this._linkSILWebsite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this._linkSILWebsite.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
			this.locExtender.SetLocalizableToolTip(this._linkSILWebsite, null);
			this.locExtender.SetLocalizationComment(this._linkSILWebsite, "Parameter is the publisher of SayMore, \"SIL International\"");
			this.locExtender.SetLocalizingId(this._linkSILWebsite, "DialogBoxes.WelcomeDlg._linkSILWebsite");
			this._linkSILWebsite.Location = new System.Drawing.Point(0, 0);
			this._linkSILWebsite.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._linkSILWebsite.Name = "_linkSILWebsite";
			this._linkSILWebsite.Size = new System.Drawing.Size(197, 13);
			this._linkSILWebsite.TabIndex = 2;
			this._linkSILWebsite.Text = "This free software is published by {0}.";
			this._linkSILWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleWebSiteLinkClicked);
			// 
			// _labelVersionInfo
			// 
			this._labelVersionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelVersionInfo.AutoSize = true;
			this._labelVersionInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this._labelVersionInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._labelVersionInfo, null);
			this.locExtender.SetLocalizationComment(this._labelVersionInfo, null);
			this.locExtender.SetLocalizingId(this._labelVersionInfo, "DialogBoxes.WelcomeDlg._labelVersionInfo");
			this._labelVersionInfo.Location = new System.Drawing.Point(31, 341);
			this._labelVersionInfo.Name = "_labelVersionInfo";
			this._labelVersionInfo.Size = new System.Drawing.Size(191, 13);
			this._labelVersionInfo.TabIndex = 1;
			this._labelVersionInfo.Text = "Version {0}.{1}.{2} (Beta)    Built on {3}";
			// 
			// _labelSubTitle
			// 
			this._labelSubTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelSubTitle.AutoEllipsis = true;
			this._labelSubTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
			this._labelSubTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._labelSubTitle, null);
			this.locExtender.SetLocalizationComment(this._labelSubTitle, null);
			this.locExtender.SetLocalizingId(this._labelSubTitle, "DialogBoxes.WelcomeDlg._labelSubTitle");
			this._labelSubTitle.Location = new System.Drawing.Point(141, 68);
			this._labelSubTitle.Name = "_labelSubTitle";
			this._labelSubTitle.Size = new System.Drawing.Size(307, 21);
			this._labelSubTitle.TabIndex = 0;
			this._labelSubTitle.Text = "Language Documentation Project Management";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// _linkSayMoreWebsite
			// 
			this._linkSayMoreWebsite.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this._linkSayMoreWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._linkSayMoreWebsite.AutoSize = true;
			this._linkSayMoreWebsite.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this._linkSayMoreWebsite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this._linkSayMoreWebsite.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
			this.locExtender.SetLocalizableToolTip(this._linkSayMoreWebsite, null);
			this.locExtender.SetLocalizationComment(this._linkSayMoreWebsite, "Parameter is the program name, \"SayMore\"");
			this.locExtender.SetLocalizingId(this._linkSayMoreWebsite, "DialogBoxes.WelcomeDlg._linkSayMoreWebsite");
			this._linkSayMoreWebsite.Location = new System.Drawing.Point(200, 0);
			this._linkSayMoreWebsite.Margin = new System.Windows.Forms.Padding(0);
			this._linkSayMoreWebsite.Name = "_linkSayMoreWebsite";
			this._linkSayMoreWebsite.Size = new System.Drawing.Size(111, 13);
			this._linkSayMoreWebsite.TabIndex = 3;
			this._linkSayMoreWebsite.Text = "Visit {0} on the Web.";
			this._linkSayMoreWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleWebSiteLinkClicked);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this._linkSILWebsite);
			this.flowLayoutPanel1.Controls.Add(this._linkSayMoreWebsite);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(30, 357);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(418, 20);
			this.flowLayoutPanel1.TabIndex = 18;
			// 
			// WelcomeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(460, 379);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this._labelSubTitle);
			this.Controls.Add(this._labelVersionInfo);
			this.Controls.Add(this.pnlOptions);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.WelcomeDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(450, 400);
			this.Name = "WelcomeDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "SayMore";
			this.pnlOptions.ResumeLayout(false);
			this.tsOptions.ResumeLayout(false);
			this.tsOptions.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlOptions;
		private ElementBar tsOptions;
		private System.Windows.Forms.ToolStripLabel _labelCreate;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton _buttonCreate;
		private System.Windows.Forms.ToolStripLabel _labelOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton _buttonMru0;
		private System.Windows.Forms.ToolStripButton _buttonBrowse;
		private System.Windows.Forms.LinkLabel _linkSILWebsite;
		private System.Windows.Forms.Label _labelVersionInfo;
		private System.Windows.Forms.Label _labelSubTitle;
		private L10NSharpExtender locExtender;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.LinkLabel _linkSayMoreWebsite;
	}
}