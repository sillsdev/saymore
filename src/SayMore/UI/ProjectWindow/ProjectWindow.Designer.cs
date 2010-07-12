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
			this._mainMenuStrip = new System.Windows.Forms.MenuStrip();
			this._menuFile = new System.Windows.Forms.ToolStripMenuItem();
			this._menuOpenProject = new System.Windows.Forms.ToolStripMenuItem();
			this._toolStripSeparator0 = new System.Windows.Forms.ToolStripSeparator();
			this._exportSessionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._menuExit = new System.Windows.Forms.ToolStripMenuItem();
			this._menuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this._menuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.releaseNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._viewTabGroup = new SIL.Pa.UI.Controls.ViewTabGroup();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this._mainMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Main Window";
			// 
			// _mainMenuStrip
			// 
			this._mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuFile,
            this._menuHelp});
			this.locExtender.SetLocalizableToolTip(this._mainMenuStrip, null);
			this.locExtender.SetLocalizationComment(this._mainMenuStrip, null);
			this.locExtender.SetLocalizingId(this._mainMenuStrip, "menuStrip1.menuStrip1");
			this._mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			this._mainMenuStrip.Name = "_mainMenuStrip";
			this._mainMenuStrip.Size = new System.Drawing.Size(697, 24);
			this._mainMenuStrip.TabIndex = 1;
			this._mainMenuStrip.Text = "menuStrip1";
			this._mainMenuStrip.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleMainMenuPaint);
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
			this._menuExit.Click += new System.EventHandler(this.HandleExitClick);
			// 
			// _menuHelp
			// 
			this._menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.releaseNotesToolStripMenuItem,
            this.toolStripSeparator1,
            this._menuAbout});
			this.locExtender.SetLocalizableToolTip(this._menuHelp, null);
			this.locExtender.SetLocalizationComment(this._menuHelp, null);
			this.locExtender.SetLocalizingId(this._menuHelp, "ProjectWindow.helpToolStripMenuItem");
			this._menuHelp.Name = "_menuHelp";
			this._menuHelp.Size = new System.Drawing.Size(44, 20);
			this._menuHelp.Text = "&Help";
			// 
			// _menuAbout
			// 
			this.locExtender.SetLocalizableToolTip(this._menuAbout, null);
			this.locExtender.SetLocalizationComment(this._menuAbout, null);
			this.locExtender.SetLocalizingId(this._menuAbout, ".aboutToolStripMenuItem");
			this._menuAbout.Name = "_menuAbout";
			this._menuAbout.Size = new System.Drawing.Size(156, 22);
			this._menuAbout.Text = "&About...";
			this._menuAbout.Click += new System.EventHandler(this.HandleHelpAboutClick);
			// 
			// releaseNotesToolStripMenuItem
			// 
			this.locExtender.SetLocalizableToolTip(this.releaseNotesToolStripMenuItem, null);
			this.locExtender.SetLocalizationComment(this.releaseNotesToolStripMenuItem, null);
			this.locExtender.SetLocalizingId(this.releaseNotesToolStripMenuItem, "ProjectWindow.releaseNotesToolStripMenuItem");
			this.releaseNotesToolStripMenuItem.Name = "releaseNotesToolStripMenuItem";
			this.releaseNotesToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.releaseNotesToolStripMenuItem.Tag = "releaseNotes";
			this.releaseNotesToolStripMenuItem.Text = "Release Notes...";
			this.releaseNotesToolStripMenuItem.Click += new System.EventHandler(this.OnCommandMenuItem_Click);
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
			// _viewTabGroup
			// 
			this._viewTabGroup.BackColor = System.Drawing.SystemColors.Control;
			this._viewTabGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this._viewTabGroup, null);
			this.locExtender.SetLocalizationComment(this._viewTabGroup, null);
			this.locExtender.SetLocalizingId(this._viewTabGroup, "viewTabGroup1.viewTabGroup1");
			this._viewTabGroup.Location = new System.Drawing.Point(0, 24);
			this._viewTabGroup.Name = "_viewTabGroup";
			this._viewTabGroup.Size = new System.Drawing.Size(697, 445);
			this._viewTabGroup.TabIndex = 2;
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
			// 
			// ProjectWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(697, 469);
			this.Controls.Add(this._viewTabGroup);
			this.Controls.Add(this._mainMenuStrip);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "MainWnd.WindowTitle");
			this.MainMenuStrip = this._mainMenuStrip;
			this.MinimumSize = new System.Drawing.Size(600, 450);
			this.Name = "ProjectWindow";
			this.Text = "{0} - SayMore {1}.{2}.{3}";
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this._mainMenuStrip.ResumeLayout(false);
			this._mainMenuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private LocalizationExtender locExtender;
		private System.Windows.Forms.MenuStrip _mainMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _menuFile;
		private System.Windows.Forms.ToolStripMenuItem _menuOpenProject;
		private System.Windows.Forms.ToolStripSeparator _toolStripSeparator0;
		private System.Windows.Forms.ToolStripMenuItem _menuExit;
		private System.Windows.Forms.ToolStripMenuItem _exportSessionsMenuItem;
		private System.Windows.Forms.ToolStripSeparator _toolStripSeparator1;
		private SIL.Pa.UI.Controls.ViewTabGroup _viewTabGroup;
		private System.Windows.Forms.ToolStripMenuItem _menuHelp;
		private System.Windows.Forms.ToolStripMenuItem _menuAbout;
		private System.Windows.Forms.ToolStripMenuItem releaseNotesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
	}
}

