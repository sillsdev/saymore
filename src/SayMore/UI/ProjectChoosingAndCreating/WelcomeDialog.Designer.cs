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
			this.pnlOptions = new System.Windows.Forms.Panel();
			this.tsOptions = new SayMore.UI.LowLevelControls.ElementBar();
			this._labelOpen = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._buttonMru0 = new System.Windows.Forms.ToolStripButton();
			this._buttonBrowse = new System.Windows.Forms.ToolStripButton();
			this._labelCreate = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this._buttonCreate = new System.Windows.Forms.ToolStripButton();
			this._linkWebSites = new System.Windows.Forms.LinkLabel();
			this._labelVersionInfo = new System.Windows.Forms.Label();
			this._labelSubTitle = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.pnlOptions.SuspendLayout();
			this.tsOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
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
			this.locExtender.SetLocalizingId(this.tsOptions, "WelcomeDialog.tsOptions");
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
			this.locExtender.SetLocalizingId(this._labelOpen, "WelcomeDialog._labelOpen");
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
			this.locExtender.SetLocalizationPriority(this._buttonMru0, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonMru0, "WelcomeDialog._buttonMru0");
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
			this.locExtender.SetLocalizableToolTip(this._buttonBrowse, null);
			this.locExtender.SetLocalizationComment(this._buttonBrowse, null);
			this.locExtender.SetLocalizingId(this._buttonBrowse, "WelcomeDialog._buttonBrowse");
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
			this.locExtender.SetLocalizingId(this._labelCreate, "WelcomeDialog._labelCreate");
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
			this.locExtender.SetLocalizableToolTip(this._buttonCreate, null);
			this.locExtender.SetLocalizationComment(this._buttonCreate, null);
			this.locExtender.SetLocalizingId(this._buttonCreate, "WelcomeDialog._buttonCreate");
			this._buttonCreate.Margin = new System.Windows.Forms.Padding(4, 1, 4, 15);
			this._buttonCreate.Name = "_buttonCreate";
			this._buttonCreate.Size = new System.Drawing.Size(408, 21);
			this._buttonCreate.Text = "Create new, blank project...";
			this._buttonCreate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonCreate.Click += new System.EventHandler(this.HandleCreateProjectClick);
			// 
			// _linkWebSites
			// 
			this._linkWebSites.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this._linkWebSites.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._linkWebSites.AutoSize = true;
			this._linkWebSites.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this._linkWebSites.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this._linkWebSites.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
			this.locExtender.SetLocalizableToolTip(this._linkWebSites, null);
			this.locExtender.SetLocalizationComment(this._linkWebSites, null);
			this.locExtender.SetLocalizingId(this._linkWebSites, "WelcomeDialog._linkWebSites");
			this._linkWebSites.Location = new System.Drawing.Point(31, 357);
			this._linkWebSites.Name = "_linkWebSites";
			this._linkWebSites.Size = new System.Drawing.Size(393, 13);
			this._linkWebSites.TabIndex = 2;
			this._linkWebSites.Text = "SayMore is brought to you by SIL International.  Visit the SayMore web site.";
			this._linkWebSites.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleWebSiteLinkClicked);
			// 
			// _labelVersionInfo
			// 
			this._labelVersionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelVersionInfo.AutoSize = true;
			this._labelVersionInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this._labelVersionInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._labelVersionInfo, null);
			this.locExtender.SetLocalizationComment(this._labelVersionInfo, null);
			this.locExtender.SetLocalizingId(this._labelVersionInfo, "WelcomeDialog._labelVersionInfo");
			this._labelVersionInfo.Location = new System.Drawing.Point(31, 341);
			this._labelVersionInfo.Name = "_labelVersionInfo";
			this._labelVersionInfo.Size = new System.Drawing.Size(159, 13);
			this._labelVersionInfo.TabIndex = 1;
			this._labelVersionInfo.Text = "Version {0}.{1}.{2}    Built on {3}";
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
			this.locExtender.SetLocalizingId(this._labelSubTitle, "WelcomeDialog._labelSubTitle");
			this._labelSubTitle.Location = new System.Drawing.Point(141, 68);
			this._labelSubTitle.Name = "_labelSubTitle";
			this._labelSubTitle.Size = new System.Drawing.Size(307, 21);
			this._labelSubTitle.TabIndex = 0;
			this._labelSubTitle.Text = "Language Documentation Project Management";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// WelcomeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(460, 379);
			this.Controls.Add(this._labelSubTitle);
			this.Controls.Add(this._linkWebSites);
			this.Controls.Add(this._labelVersionInfo);
			this.Controls.Add(this.pnlOptions);
			this.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "WelcomeDialog.WindowTitle");
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
		private System.Windows.Forms.LinkLabel _linkWebSites;
		private System.Windows.Forms.Label _labelVersionInfo;
		private System.Windows.Forms.Label _labelSubTitle;
		private LocalizationExtender locExtender;
	}
}