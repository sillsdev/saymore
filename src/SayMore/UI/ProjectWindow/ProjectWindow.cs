// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2014, SIL International. All Rights Reserved.
// <copyright from='2011' to='2014' company='SIL International'>
//		Copyright (c) 2014, SIL International. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (http://sil.mit-license.org/)
// </copyright>
#endregion
// --------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Windows.Forms;
using DesktopAnalytics;
using L10NSharp;
using L10NSharp.UI;
using SIL.IO;
using SIL.Reporting;
using SIL.Windows.Forms.PortableSettingsProvider;
using SayMore.Media.Audio;
using SayMore.Properties;
using SayMore.Media.MPlayer;
using SayMore.UI.Overview;

namespace SayMore.UI.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class for the main window of the application.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ProjectWindow : Form
	{
		public delegate ProjectWindow Factory(string projectPath, IEnumerable<ISayMoreView> views); //autofac uses this

		private readonly string _projectPath;
		private readonly IEnumerable<ICommand> _commands;
		private readonly UILanguageDlg.Factory _uiLanguageDialogFactory;
		private MPlayerDebuggingOutputWindow _outputDebuggingWindow;
		private string _titleFmt;

		public bool UserWantsToOpenADifferentProject { get; set; }

		/// ------------------------------------------------------------------------------------
		private string ProjectName
		{
			get { return Path.GetFileNameWithoutExtension(_projectPath); }
		}

		/// ------------------------------------------------------------------------------------
		private ProjectWindow()
		{
			Logger.WriteEvent("ProjectWindow constructor");

			ExceptionHandler.AddDelegate(AudioUtils.HandleGlobalNAudioException);

			InitializeComponent();
			_titleFmt = Text;
			_menuShowMPlayerDebugWindow.Tag = _menuProject.DropDownItems.IndexOf(_menuShowMPlayerDebugWindow);
			_menuProject.DropDownItems.Remove(_menuShowMPlayerDebugWindow);
		}

		/// ------------------------------------------------------------------------------------
		public ProjectWindow(string projectPath, IEnumerable<ISayMoreView> views,
			IEnumerable<ICommand> commands, UILanguageDlg.Factory uiLanguageDialogFactory) : this()
		{
			if (Settings.Default.ProjectWindow == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.ProjectWindow = FormSettings.Create(this);
			}

			var asm = Assembly.GetExecutingAssembly();
			var stream = asm.GetManifestResourceStream("SayMore.SayMore.ico");
			if (stream != null) Icon = new Icon(stream);

			_projectPath = projectPath;
			_commands = commands;
			_uiLanguageDialogFactory = uiLanguageDialogFactory;

			_viewTabGroup.Visible = false;

			foreach (var vw in views)
			{
				vw.AddTabToTabGroup(_viewTabGroup);

				if (vw.MainMenuItem != null)
				{
					vw.MainMenuItem.Enabled = false;
					_mainMenuStrip.Items.Insert(_mainMenuStrip.Items.IndexOf(_mainMenuHelp), vw.MainMenuItem);
				}

				//------------------------------------------------------------------------------------
				// 19 FEB 2014, Phil Hopper: Disable the hidden tabs because Alt+Key is activating
				// buttons that are not on the active (visible) tab.  Use vw.ViewActivated() and
				// vw.ViewDeactivated() to enable and disable tabs at the appropriate time.
				//------------------------------------------------------------------------------------
				((UserControl)vw).Enabled = false;
			}

			SetWindowText();
			LocalizeItemDlg.StringsLocalized += SetWindowText;

			foreach (var tab in _viewTabGroup.Tabs.Where(tab => tab.View is ProjectScreen))
				_viewTabGroup.SetActiveView(tab);

			Application.DoEvents();
			_viewTabGroup.Visible = true;
		}

		internal List<Control> Views
		{
			get
			{
				return _viewTabGroup.Tabs.Select(item => item.View).ToList();
			}
		}

		internal int SelectedTabIndex()
		{
			for (var i = 0; i < _viewTabGroup.Tabs.Count; i++)
				if (_viewTabGroup.Tabs[i].Selected) return i;

			return -1;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				LocalizeItemDlg.StringsLocalized -= SetWindowText;

				ExceptionHandler.RemoveDelegate(AudioUtils.HandleGlobalNAudioException);

				if (components != null)
					components.Dispose();

				if (_viewTabGroup != null)
					_viewTabGroup.Tabs.Clear();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the localized window title texts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetWindowText()
		{
			var ver = Assembly.GetExecutingAssembly().GetName().Version;
			Text = string.Format(_titleFmt, ProjectName, ver.Major, ver.Minor, ver.Build,
				ApplicationContainer.GetBuildTypeDescriptor(BuildType.Current));
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.ProjectWindow.InitializeForm(this);
			base.OnLoad(e);

			_viewTabGroup.SetActiveView(_viewTabGroup.Tabs[0]);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			ReportAnyFileLoadErrors();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (!_viewTabGroup.IsOKToCloseGroup)
			{
				e.Cancel = true;
				SystemSounds.Beep.Play();
			}

			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show the welcome dialog to allow the user to choose another project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleOpenProjectClick(object sender, EventArgs e)
		{
			UserWantsToOpenADifferentProject = true;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleExitClick(object sender, EventArgs e)
		{
			UserWantsToOpenADifferentProject = false;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleHelpAboutClick(object sender, EventArgs e)
		{
			using (var dlg = new SIL.Windows.Forms.Miscellaneous.SILAboutBox(FileLocator.GetFileDistributedWithApplication("aboutbox.htm")))
				dlg.ShowDialog();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleHelpClick(object sender, EventArgs e)
		{
			//nb: when the file is in our source code, and not in the program directory, windows security will squawk and then not show content.
			var path = FileLocator.GetFileDistributedWithApplication(false,"SayMore.chm");
			try
			{
				Process.Start(path);
			}
			catch (Exception ex)
			{
				//user cancelling a security warning here shouldn't lead to a crash
				Debug.Print(ex.Message);
			}
			Analytics.Track("Show Help from main menu");
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCommandMenuItemClick(object sender, EventArgs e)
		{
			var handler = _commands.First(c => c.Id == (string)((ToolStripMenuItem)sender).Tag);
			handler.Execute();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleArchiveProjectMenuItemClick(object sender, EventArgs e)
		{
			Program.ArchiveProjectUsingIMDI(this);
			ReportAnyFileLoadErrors();
		}

		/// ------------------------------------------------------------------------------------
		private void ReportAnyFileLoadErrors()
		{
			var loadErrors = Program.FileLoadErrors;
			if (loadErrors.Any())
			{
				using (var dlg = new FileLoadErrorsReportDlg(loadErrors))
					dlg.ShowDialog(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleChangeUILanguageMenuClick(object sender, EventArgs e)
		{
			using (var dlg = _uiLanguageDialogFactory())
			{
				if (dlg.ShowDialog(this) != DialogResult.OK)
					return;

				Settings.Default.UserInterfaceLanguage = dlg.UILanguage;
				LocalizationManager.SetUILanguage(dlg.UILanguage, true);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleProjectMenuClick(object sender, EventArgs e)
		{
			if (_outputDebuggingWindow == null && (int)(ModifierKeys & Keys.Shift) > 0)
			{
				_menuProject.DropDownItems.Insert((int)_menuShowMPlayerDebugWindow.Tag,
					_menuShowMPlayerDebugWindow);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleProjectMenuDropDownClosed(object sender, EventArgs e)
		{
			if (_menuProject.DropDownItems.Contains(_menuShowMPlayerDebugWindow))
				_menuProject.DropDownItems.Remove(_menuShowMPlayerDebugWindow);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleShowMPlayerDebugWindowMenuClick(object sender, EventArgs e)
		{
			_outputDebuggingWindow = new MPlayerDebuggingOutputWindow();
			_outputDebuggingWindow.FormClosed += delegate { _outputDebuggingWindow = null; };
			_outputDebuggingWindow.Disposed += delegate { _outputDebuggingWindow = null; };
			_outputDebuggingWindow.Show();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw a subtle line at the bottom of the main menu to visually separate
		/// the main tabs from the main menu.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleMainMenuPaint(object sender, PaintEventArgs e)
		{
			var clr = Color.FromArgb(30, Color.Black);
			using (var pen = new Pen(clr))
			{
				var rc = _mainMenuStrip.ClientRectangle;
				e.Graphics.DrawLine(pen, 0, rc.Bottom - 1, rc.Right, rc.Bottom - 1);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleViewActivated(ViewTabGroup sender, ViewTab activatedTab)
		{
			var view = activatedTab.View as ISayMoreView;
			if (view == null)
				return;

			if (view.MainMenuItem != null)
				view.MainMenuItem.Enabled = true;

			Analytics.Track(view.NameForUsageReporting + "View Activated");
		}

		/// ------------------------------------------------------------------------------------
		private void HandleViewDeactivated(ViewTabGroup sender, ViewTab deactivatedTab)
		{
			var view = deactivatedTab.View as ISayMoreView;
			if (view != null && view.MainMenuItem != null)
				view.MainMenuItem.Enabled = false;
		}
	}
}
