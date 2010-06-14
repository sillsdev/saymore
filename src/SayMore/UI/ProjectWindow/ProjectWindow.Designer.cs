using SIL.Localization;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ProjectWindow
{
	partial class ProjectWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.locExtender = new SIL.Localization.LocalizationExtender(this.components);
			this._mainToolStrip = new SayMore.UI.LowLevelControls.ElementBar();
			this._toolStripButtonOpenProject = new System.Windows.Forms.ToolStripButton();
			this._mainMenuStrip = new System.Windows.Forms.MenuStrip();
			this._menuFile = new System.Windows.Forms.ToolStripMenuItem();
			this._menuOpenProject = new System.Windows.Forms.ToolStripMenuItem();
			this._toolStripSeparator0 = new System.Windows.Forms.ToolStripSeparator();
			this._exportSessionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._menuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._toolStripButtonOverview = new System.Windows.Forms.ToolStripButton();
			this._toolStripButtonSessions = new System.Windows.Forms.ToolStripButton();
			this._toolStripButtonPeople = new System.Windows.Forms.ToolStripButton();
			this._toolStripButtonSendReceive = new System.Windows.Forms.ToolStripButton();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this._mainToolStrip.SuspendLayout();
			this._mainMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Main Window";
			// 
			// _mainToolStrip
			// 
			this._mainToolStrip.GradientAngle = 0F;
			this._mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._mainToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
			this._mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripButtonOverview,
            this._toolStripButtonSessions,
            this._toolStripButtonPeople,
            this._toolStripButtonSendReceive,
            this._toolStripButtonOpenProject});
			this.locExtender.SetLocalizableToolTip(this._mainToolStrip, null);
			this.locExtender.SetLocalizationComment(this._mainToolStrip, null);
			this.locExtender.SetLocalizingId(this._mainToolStrip, "MainWnd.tsMain");
			this._mainToolStrip.Location = new System.Drawing.Point(0, 24);
			this._mainToolStrip.Name = "_mainToolStrip";
			this._mainToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this._mainToolStrip.Size = new System.Drawing.Size(697, 38);
			this._mainToolStrip.TabIndex = 0;
			// 
			// _toolStripButtonOpenProject
			// 
			this._toolStripButtonOpenProject.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._toolStripButtonOpenProject.AutoSize = false;
			this._toolStripButtonOpenProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._toolStripButtonOpenProject.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._toolStripButtonOpenProject.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._toolStripButtonOpenProject, "Open a Different Project");
			this.locExtender.SetLocalizationComment(this._toolStripButtonOpenProject, null);
			this.locExtender.SetLocalizationPriority(this._toolStripButtonOpenProject, SIL.Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._toolStripButtonOpenProject, "MainWnd.tsbChangeProjects");
			this._toolStripButtonOpenProject.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
			this._toolStripButtonOpenProject.Name = "_toolStripButtonOpenProject";
			this._toolStripButtonOpenProject.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this._toolStripButtonOpenProject.Size = new System.Drawing.Size(50, 30);
			this._toolStripButtonOpenProject.Text = "Change Projects...";
			this._toolStripButtonOpenProject.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this._toolStripButtonOpenProject.ToolTipText = "Open a Different Project";
			// 
			// _mainMenuStrip
			// 
			this._mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuFile});
			this.locExtender.SetLocalizableToolTip(this._mainMenuStrip, null);
			this.locExtender.SetLocalizationComment(this._mainMenuStrip, null);
			this.locExtender.SetLocalizingId(this._mainMenuStrip, "menuStrip1.menuStrip1");
			this._mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			this._mainMenuStrip.Name = "_mainMenuStrip";
			this._mainMenuStrip.Size = new System.Drawing.Size(697, 24);
			this._mainMenuStrip.TabIndex = 1;
			this._mainMenuStrip.Text = "menuStrip1";
			// 
			// _menuFile
			// 
			this._menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuOpenProject,
            this._toolStripSeparator0,
            this._exportSessionsMenuItem,
            this._toolStripSeparator1,
            this._menuExit});
			this.locExtender.SetLocalizableToolTip(this._menuFile, null);
			this.locExtender.SetLocalizationComment(this._menuFile, null);
			this.locExtender.SetLocalizingId(this._menuFile, "ProjectWindow._menuFile");
			this._menuFile.Name = "_menuFile";
			this._menuFile.Size = new System.Drawing.Size(37, 20);
			this._menuFile.Text = "&File";
			// 
			// _menuOpenProject
			// 
			this.locExtender.SetLocalizableToolTip(this._menuOpenProject, null);
			this.locExtender.SetLocalizationComment(this._menuOpenProject, null);
			this.locExtender.SetLocalizingId(this._menuOpenProject, "ProjectWindow._menuOpenProject");
			this._menuOpenProject.Name = "_menuOpenProject";
			this._menuOpenProject.Size = new System.Drawing.Size(163, 22);
			this._menuOpenProject.Text = "&Open Project...";
			this._menuOpenProject.Click += new System.EventHandler(this.HandleOpenProjectClick);
			// 
			// _toolStripSeparator0
			// 
			this._toolStripSeparator0.Name = "_toolStripSeparator0";
			this._toolStripSeparator0.Size = new System.Drawing.Size(160, 6);
			// 
			// _exportSessionsMenuItem
			// 
			this.locExtender.SetLocalizableToolTip(this._exportSessionsMenuItem, null);
			this.locExtender.SetLocalizationComment(this._exportSessionsMenuItem, null);
			this.locExtender.SetLocalizingId(this._exportSessionsMenuItem, "ProjectWindow.exportSessionsToolStripMenuItem");
			this._exportSessionsMenuItem.Name = "_exportSessionsMenuItem";
			this._exportSessionsMenuItem.Size = new System.Drawing.Size(163, 22);
			this._exportSessionsMenuItem.Tag = "export";
			this._exportSessionsMenuItem.Text = "Export Sessions...";
			this._exportSessionsMenuItem.Click += new System.EventHandler(this.OnCommandMenuItem_Click);
			// 
			// _toolStripSeparator1
			// 
			this._toolStripSeparator1.Name = "_toolStripSeparator1";
			this._toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
			// 
			// _menuExit
			// 
			this.locExtender.SetLocalizableToolTip(this._menuExit, null);
			this.locExtender.SetLocalizationComment(this._menuExit, null);
			this.locExtender.SetLocalizingId(this._menuExit, "ProjectWindow._menuExit");
			this._menuExit.Name = "_menuExit";
			this._menuExit.Size = new System.Drawing.Size(163, 22);
			this._menuExit.Text = "E&xit";
			// 
			// fileToolStripMenuItem
			// 
			this.locExtender.SetLocalizableToolTip(this.fileToolStripMenuItem, null);
			this.locExtender.SetLocalizationComment(this.fileToolStripMenuItem, null);
			this.locExtender.SetLocalizingId(this.fileToolStripMenuItem, ".fileToolStripMenuItem");
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// _toolStripButtonOverview
			// 
			this._toolStripButtonOverview.AutoSize = false;
			this._toolStripButtonOverview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._toolStripButtonOverview.Image = global::SayMore.Properties.Resources.Overview;
			this._toolStripButtonOverview.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._toolStripButtonOverview.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._toolStripButtonOverview, "Project Overview");
			this.locExtender.SetLocalizationComment(this._toolStripButtonOverview, null);
			this.locExtender.SetLocalizingId(this._toolStripButtonOverview, "MainWnd.tsbOverview");
			this._toolStripButtonOverview.Margin = new System.Windows.Forms.Padding(10, 4, 0, 4);
			this._toolStripButtonOverview.Name = "_toolStripButtonOverview";
			this._toolStripButtonOverview.Size = new System.Drawing.Size(40, 30);
			this._toolStripButtonOverview.Text = "Overview";
			this._toolStripButtonOverview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// _toolStripButtonSessions
			// 
			this._toolStripButtonSessions.AutoSize = false;
			this._toolStripButtonSessions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._toolStripButtonSessions.Image = global::SayMore.Properties.Resources.Sessions;
			this._toolStripButtonSessions.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._toolStripButtonSessions.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._toolStripButtonSessions, "Manage Sessions");
			this.locExtender.SetLocalizationComment(this._toolStripButtonSessions, null);
			this.locExtender.SetLocalizingId(this._toolStripButtonSessions, "MainWnd.tsbSessions");
			this._toolStripButtonSessions.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this._toolStripButtonSessions.Name = "_toolStripButtonSessions";
			this._toolStripButtonSessions.Size = new System.Drawing.Size(40, 30);
			this._toolStripButtonSessions.Text = "Sessions";
			this._toolStripButtonSessions.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// _toolStripButtonPeople
			// 
			this._toolStripButtonPeople.AutoSize = false;
			this._toolStripButtonPeople.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._toolStripButtonPeople.Image = global::SayMore.Properties.Resources.People;
			this._toolStripButtonPeople.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._toolStripButtonPeople.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._toolStripButtonPeople, "Manage List of People");
			this.locExtender.SetLocalizationComment(this._toolStripButtonPeople, null);
			this.locExtender.SetLocalizingId(this._toolStripButtonPeople, "MainWnd.tsbPeople");
			this._toolStripButtonPeople.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this._toolStripButtonPeople.Name = "_toolStripButtonPeople";
			this._toolStripButtonPeople.Size = new System.Drawing.Size(40, 30);
			this._toolStripButtonPeople.Text = "People";
			this._toolStripButtonPeople.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// _toolStripButtonSendReceive
			// 
			this._toolStripButtonSendReceive.AutoSize = false;
			this._toolStripButtonSendReceive.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._toolStripButtonSendReceive.Image = global::SayMore.Properties.Resources.SendReceive;
			this._toolStripButtonSendReceive.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._toolStripButtonSendReceive.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._toolStripButtonSendReceive, "Send/Receive");
			this.locExtender.SetLocalizationComment(this._toolStripButtonSendReceive, null);
			this.locExtender.SetLocalizingId(this._toolStripButtonSendReceive, "MainWnd.tsbSendReceive");
			this._toolStripButtonSendReceive.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this._toolStripButtonSendReceive.Name = "_toolStripButtonSendReceive";
			this._toolStripButtonSendReceive.Size = new System.Drawing.Size(40, 30);
			this._toolStripButtonSendReceive.Text = "Send/Receive";
			this._toolStripButtonSendReceive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// ProjectWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(697, 469);
			this.Controls.Add(this._mainToolStrip);
			this.Controls.Add(this._mainMenuStrip);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "MainWnd.WindowTitle");
			this.MainMenuStrip = this._mainMenuStrip;
			this.MinimumSize = new System.Drawing.Size(600, 450);
			this.Name = "ProjectWindow";
			this.Text = "{0} - SayMore";
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this._mainToolStrip.ResumeLayout(false);
			this._mainToolStrip.PerformLayout();
			this._mainMenuStrip.ResumeLayout(false);
			this._mainMenuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ElementBar _mainToolStrip;
		private System.Windows.Forms.ToolStripButton _toolStripButtonPeople;
		private System.Windows.Forms.ToolStripButton _toolStripButtonOverview;
		private System.Windows.Forms.ToolStripButton _toolStripButtonSendReceive;
		private System.Windows.Forms.ToolStripButton _toolStripButtonSessions;
		private LocalizationExtender locExtender;
		private System.Windows.Forms.ToolStripButton _toolStripButtonOpenProject;
		private System.Windows.Forms.MenuStrip _mainMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _menuFile;
		private System.Windows.Forms.ToolStripMenuItem _menuOpenProject;
		private System.Windows.Forms.ToolStripSeparator _toolStripSeparator0;
		private System.Windows.Forms.ToolStripMenuItem _menuExit;
		private System.Windows.Forms.ToolStripMenuItem _exportSessionsMenuItem;
		private System.Windows.Forms.ToolStripSeparator _toolStripSeparator1;
	}
}

