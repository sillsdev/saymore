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
			this._menuExportEvents = new System.Windows.Forms.ToolStripMenuItem();
			this._menuExportPeople = new System.Windows.Forms.ToolStripMenuItem();
			this._toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._menuChangeUILanguage = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this._menuExit = new System.Windows.Forms.ToolStripMenuItem();
			this._mainMenuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this._menuReleaseNotes = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._menuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this._menuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this._viewTabGroup = new SayMore.UI.ProjectWindow.ViewTabGroup();
			this._menuShowMPlayerDebugWindow = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this._mainMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// _mainMenuStrip
			// 
			this._mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuProject,
            this._mainMenuHelp});
			this.locExtender.SetLocalizableToolTip(this._mainMenuStrip, null);
			this.locExtender.SetLocalizationComment(this._mainMenuStrip, null);
			this.locExtender.SetLocalizationPriority(this._mainMenuStrip, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._mainMenuStrip, "MainWindow._mainMenuStrip");
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
            this._menuExportEvents,
            this._menuExportPeople,
            this._toolStripSeparator1,
            this._menuChangeUILanguage,
            this._menuShowMPlayerDebugWindow,
            this.toolStripMenuItem1,
            this._menuExit});
			this.locExtender.SetLocalizableToolTip(this._menuProject, null);
			this.locExtender.SetLocalizationComment(this._menuProject, null);
			this.locExtender.SetLocalizingId(this._menuProject, "MainWindow._menuProject");
			this._menuProject.Name = "_menuProject";
			this._menuProject.Size = new System.Drawing.Size(56, 20);
			this._menuProject.Text = "&Project";
			this._menuProject.DropDownOpening += new System.EventHandler(this.HandleProjectMenuOpening);
			// 
			// _menuOpenProject
			// 
			this.locExtender.SetLocalizableToolTip(this._menuOpenProject, null);
			this.locExtender.SetLocalizationComment(this._menuOpenProject, null);
			this.locExtender.SetLocalizingId(this._menuOpenProject, "MainWindow._menuOpenProject");
			this._menuOpenProject.Name = "_menuOpenProject";
			this._menuOpenProject.Size = new System.Drawing.Size(254, 22);
			this._menuOpenProject.Text = "&Open Project...";
			this._menuOpenProject.Click += new System.EventHandler(this.HandleOpenProjectClick);
			// 
			// _toolStripSeparator0
			// 
			this._toolStripSeparator0.Name = "_toolStripSeparator0";
			this._toolStripSeparator0.Size = new System.Drawing.Size(251, 6);
			// 
			// _menuExportEvents
			// 
			this.locExtender.SetLocalizableToolTip(this._menuExportEvents, null);
			this.locExtender.SetLocalizationComment(this._menuExportEvents, null);
			this.locExtender.SetLocalizingId(this._menuExportEvents, "MainWindow._menuExportEvents");
			this._menuExportEvents.Name = "_menuExportEvents";
			this._menuExportEvents.Size = new System.Drawing.Size(254, 22);
			this._menuExportEvents.Tag = "exportEvents";
			this._menuExportEvents.Text = "Export Events...";
			this._menuExportEvents.Click += new System.EventHandler(this.HandleCommandMenuItemClick);
			// 
			// _menuExportPeople
			// 
			this.locExtender.SetLocalizableToolTip(this._menuExportPeople, null);
			this.locExtender.SetLocalizationComment(this._menuExportPeople, null);
			this.locExtender.SetLocalizingId(this._menuExportPeople, "MainWindow._menuExportPeople");
			this._menuExportPeople.Name = "_menuExportPeople";
			this._menuExportPeople.Size = new System.Drawing.Size(254, 22);
			this._menuExportPeople.Tag = "exportPeople";
			this._menuExportPeople.Text = "Export People...";
			this._menuExportPeople.Click += new System.EventHandler(this.HandleCommandMenuItemClick);
			// 
			// _toolStripSeparator1
			// 
			this._toolStripSeparator1.Name = "_toolStripSeparator1";
			this._toolStripSeparator1.Size = new System.Drawing.Size(251, 6);
			// 
			// _menuChangeUILanguage
			// 
			this.locExtender.SetLocalizableToolTip(this._menuChangeUILanguage, null);
			this.locExtender.SetLocalizationComment(this._menuChangeUILanguage, null);
			this.locExtender.SetLocalizingId(this._menuChangeUILanguage, "MainWindow._menuChangeUILanguage");
			this._menuChangeUILanguage.Name = "_menuChangeUILanguage";
			this._menuChangeUILanguage.Size = new System.Drawing.Size(254, 22);
			this._menuChangeUILanguage.Text = "Change User Interface Language...";
			this._menuChangeUILanguage.Click += new System.EventHandler(this.HandleChangeUILanguageMenuClick);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(251, 6);
			// 
			// _menuExit
			// 
			this.locExtender.SetLocalizableToolTip(this._menuExit, null);
			this.locExtender.SetLocalizationComment(this._menuExit, null);
			this.locExtender.SetLocalizingId(this._menuExit, "MainWindow._menuExit");
			this._menuExit.Name = "_menuExit";
			this._menuExit.Size = new System.Drawing.Size(254, 22);
			this._menuExit.Text = "E&xit";
			this._menuExit.Click += new System.EventHandler(this.HandleExitClick);
			// 
			// _mainMenuHelp
			// 
			this._mainMenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuReleaseNotes,
            this.toolStripSeparator1,
            this._menuHelp,
            this._menuAbout});
			this.locExtender.SetLocalizableToolTip(this._mainMenuHelp, null);
			this.locExtender.SetLocalizationComment(this._mainMenuHelp, null);
			this.locExtender.SetLocalizingId(this._mainMenuHelp, "MainWindow._mainMenuHelp");
			this._mainMenuHelp.Name = "_mainMenuHelp";
			this._mainMenuHelp.Size = new System.Drawing.Size(44, 20);
			this._mainMenuHelp.Text = "&Help";
			// 
			// _menuReleaseNotes
			// 
			this.locExtender.SetLocalizableToolTip(this._menuReleaseNotes, null);
			this.locExtender.SetLocalizationComment(this._menuReleaseNotes, null);
			this.locExtender.SetLocalizingId(this._menuReleaseNotes, "MainWindow._menuReleaseNotes");
			this._menuReleaseNotes.Name = "_menuReleaseNotes";
			this._menuReleaseNotes.Size = new System.Drawing.Size(156, 22);
			this._menuReleaseNotes.Tag = "releaseNotes";
			this._menuReleaseNotes.Text = "Release Notes...";
			this._menuReleaseNotes.Click += new System.EventHandler(this.HandleCommandMenuItemClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
			// 
			// _menuHelp
			// 
			this.locExtender.SetLocalizableToolTip(this._menuHelp, null);
			this.locExtender.SetLocalizationComment(this._menuHelp, null);
			this.locExtender.SetLocalizingId(this._menuHelp, "MainWindow._menuHelp");
			this._menuHelp.Name = "_menuHelp";
			this._menuHelp.Size = new System.Drawing.Size(156, 22);
			this._menuHelp.Text = "&Help...";
			this._menuHelp.Click += new System.EventHandler(this.HandleHelpClick);
			// 
			// _menuAbout
			// 
			this.locExtender.SetLocalizableToolTip(this._menuAbout, null);
			this.locExtender.SetLocalizationComment(this._menuAbout, null);
			this.locExtender.SetLocalizingId(this._menuAbout, "MainWindow._menuAbout");
			this._menuAbout.Name = "_menuAbout";
			this._menuAbout.Size = new System.Drawing.Size(156, 22);
			this._menuAbout.Text = "&About...";
			this._menuAbout.Click += new System.EventHandler(this.HandleHelpAboutClick);
			// 
			// _viewTabGroup
			// 
			this._viewTabGroup.BackColor = System.Drawing.SystemColors.Control;
			this._viewTabGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this._viewTabGroup, null);
			this.locExtender.SetLocalizationComment(this._viewTabGroup, null);
			this.locExtender.SetLocalizingId(this._viewTabGroup, "MainWindow._viewTabGroup");
			this._viewTabGroup.Location = new System.Drawing.Point(0, 24);
			this._viewTabGroup.Name = "_viewTabGroup";
			this._viewTabGroup.Size = new System.Drawing.Size(697, 445);
			this._viewTabGroup.TabIndex = 2;
			this._viewTabGroup.ViewActivated += new SayMore.UI.ProjectWindow.ViewTabGroup.ViewTabChangedHandler(this.HandleViewActivated);
			this._viewTabGroup.ViewDeactivated += new SayMore.UI.ProjectWindow.ViewTabGroup.ViewTabChangedHandler(this.HandleViewDeactivated);
			// 
			// _menuShowMPlayerDebugWindow
			// 
			this.locExtender.SetLocalizableToolTip(this._menuShowMPlayerDebugWindow, null);
			this.locExtender.SetLocalizationComment(this._menuShowMPlayerDebugWindow, null);
			this.locExtender.SetLocalizationPriority(this._menuShowMPlayerDebugWindow, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._menuShowMPlayerDebugWindow, "ProjectWindow.showMPlayerDebugWindowToolStripMenuItem");
			this._menuShowMPlayerDebugWindow.Name = "_menuShowMPlayerDebugWindow";
			this._menuShowMPlayerDebugWindow.Size = new System.Drawing.Size(254, 22);
			this._menuShowMPlayerDebugWindow.Text = "Show MPlayer Debug Window";
			this._menuShowMPlayerDebugWindow.Click += new System.EventHandler(this.HandleShowMPlayerDebugWindowMenuClick);
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
			this.locExtender.SetLocalizingId(this, "MainWindow.WindowTitle");
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
		private System.Windows.Forms.ToolStripMenuItem _menuProject;
		private System.Windows.Forms.ToolStripMenuItem _menuOpenProject;
		private System.Windows.Forms.ToolStripSeparator _toolStripSeparator0;
		private System.Windows.Forms.ToolStripMenuItem _menuExit;
		private System.Windows.Forms.ToolStripMenuItem _menuExportEvents;
	    private System.Windows.Forms.ToolStripMenuItem _menuExportPeople;
        private System.Windows.Forms.ToolStripSeparator _toolStripSeparator1;
		private ViewTabGroup _viewTabGroup;
		private System.Windows.Forms.ToolStripMenuItem _mainMenuHelp;
		private System.Windows.Forms.ToolStripMenuItem _menuAbout;
		private System.Windows.Forms.ToolStripMenuItem _menuReleaseNotes;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem _menuHelp;
		private System.Windows.Forms.ToolStripMenuItem _menuChangeUILanguage;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem _menuShowMPlayerDebugWindow;
	}
}

