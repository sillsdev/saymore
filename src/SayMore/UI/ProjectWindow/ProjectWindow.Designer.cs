using Localization.UI;

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
            this.locExtender = new Localization.UI.LocalizationExtender(this.components);
            this._mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this._menuProject = new System.Windows.Forms.ToolStripMenuItem();
            this._menuOpenProject = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this._exportEventsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this._menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.releaseNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._viewTabGroup = new SayMore.UI.ProjectWindow.ViewTabGroup();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this._menuProject,
            this._menuHelp});
            this._mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this._mainMenuStrip.Name = "_mainMenuStrip";
            this._mainMenuStrip.Size = new System.Drawing.Size(697, 24);
            this._mainMenuStrip.TabIndex = 1;
            this._mainMenuStrip.Text = "menuStrip1";
            this._mainMenuStrip.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleMainMenuPaint);
            // 
            // _menuProject
            // 
            this._menuProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuOpenProject,
            this._toolStripSeparator0,
            this._exportEventsMenuItem,
            this._toolStripSeparator1,
            this._menuExit});
            this._menuProject.Name = "_menuProject";
            this._menuProject.Size = new System.Drawing.Size(56, 20);
            this._menuProject.Text = "&Project";
            // 
            // _menuOpenProject
            // 
            this._menuOpenProject.Name = "_menuOpenProject";
            this._menuOpenProject.Size = new System.Drawing.Size(153, 22);
            this._menuOpenProject.Text = "&Open Project...";
            this._menuOpenProject.Click += new System.EventHandler(this.HandleOpenProjectClick);
            // 
            // _toolStripSeparator0
            // 
            this._toolStripSeparator0.Name = "_toolStripSeparator0";
            this._toolStripSeparator0.Size = new System.Drawing.Size(150, 6);
            // 
            // _exportEventsMenuItem
            // 
            this._exportEventsMenuItem.Name = "_exportEventsMenuItem";
            this._exportEventsMenuItem.Size = new System.Drawing.Size(153, 22);
            this._exportEventsMenuItem.Tag = "export";
            this._exportEventsMenuItem.Text = "Export Events...";
            this._exportEventsMenuItem.Click += new System.EventHandler(this.HandleCommandMenuItemClick);
            // 
            // _toolStripSeparator1
            // 
            this._toolStripSeparator1.Name = "_toolStripSeparator1";
            this._toolStripSeparator1.Size = new System.Drawing.Size(150, 6);
            // 
            // _menuExit
            // 
            this._menuExit.Name = "_menuExit";
            this._menuExit.Size = new System.Drawing.Size(153, 22);
            this._menuExit.Text = "E&xit";
            this._menuExit.Click += new System.EventHandler(this.HandleExitClick);
            // 
            // _menuHelp
            // 
            this._menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.releaseNotesToolStripMenuItem,
            this.toolStripSeparator1,
            this.helpToolStripMenuItem,
            this._menuAbout});
            this._menuHelp.Name = "_menuHelp";
            this._menuHelp.Size = new System.Drawing.Size(44, 20);
            this._menuHelp.Text = "&Help";
            // 
            // releaseNotesToolStripMenuItem
            // 
            this.releaseNotesToolStripMenuItem.Name = "releaseNotesToolStripMenuItem";
            this.releaseNotesToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.releaseNotesToolStripMenuItem.Tag = "releaseNotes";
            this.releaseNotesToolStripMenuItem.Text = "Release Notes...";
            this.releaseNotesToolStripMenuItem.Click += new System.EventHandler(this.HandleCommandMenuItemClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
            // 
            // _menuAbout
            // 
            this._menuAbout.Name = "_menuAbout";
            this._menuAbout.Size = new System.Drawing.Size(156, 22);
            this._menuAbout.Text = "&About...";
            this._menuAbout.Click += new System.EventHandler(this.HandleHelpAboutClick);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // _viewTabGroup
            // 
            this._viewTabGroup.BackColor = System.Drawing.SystemColors.Control;
            this._viewTabGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewTabGroup.Location = new System.Drawing.Point(0, 24);
            this._viewTabGroup.Name = "_viewTabGroup";
            this._viewTabGroup.Size = new System.Drawing.Size(697, 445);
            this._viewTabGroup.TabIndex = 2;
            this._viewTabGroup.ViewActivated += new SayMore.UI.ProjectWindow.ViewTabGroup.ViewTabChangedHandler(this.HandleViewActivated);
            this._viewTabGroup.ViewDeactivated += new SayMore.UI.ProjectWindow.ViewTabGroup.ViewTabChangedHandler(this.HandleViewDeactivated);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.helpToolStripMenuItem.Text = "&Help...";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.HandleHelpClick);
            // 
            // ProjectWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 469);
            this.Controls.Add(this._viewTabGroup);
            this.Controls.Add(this._mainMenuStrip);
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
		private System.Windows.Forms.ToolStripMenuItem _menuProject;
		private System.Windows.Forms.ToolStripMenuItem _menuOpenProject;
		private System.Windows.Forms.ToolStripSeparator _toolStripSeparator0;
		private System.Windows.Forms.ToolStripMenuItem _menuExit;
		private System.Windows.Forms.ToolStripMenuItem _exportEventsMenuItem;
		private System.Windows.Forms.ToolStripSeparator _toolStripSeparator1;
		private ViewTabGroup _viewTabGroup;
		private System.Windows.Forms.ToolStripMenuItem _menuHelp;
		private System.Windows.Forms.ToolStripMenuItem _menuAbout;
		private System.Windows.Forms.ToolStripMenuItem releaseNotesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
	}
}

