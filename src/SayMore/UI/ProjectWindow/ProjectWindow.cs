// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2025, SIL Global. All Rights Reserved.
// <copyright from='2011' to='2025' company='SIL Global'>
//		Copyright (c) 2025, SIL Global. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (https://sil.mit-license.org/)
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
using L10NSharp.XLiffUtils;
using L10NSharp.UI;
using NetSparkle;
using SIL.IO;
using SIL.Reporting;
using SIL.Windows.Forms.PortableSettingsProvider;
using SayMore.Media.Audio;
using SayMore.Properties;
using SayMore.Media.MPlayer;
using SayMore.UI.Overview;
using SayMore.Utilities;
using SIL.Windows.Forms.Extensions;
using SIL.Windows.Forms.Miscellaneous;
using static System.String;
using static SayMore.Utilities.FileSystemUtils;

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

		private static Sparkle UpdateChecker;

		private readonly string _projectPath;
		private readonly IEnumerable<ICommand> _commands;
		private readonly UILanguageDlg.Factory _uiLanguageDialogFactory;
		private MPlayerDebuggingOutputWindow _outputDebuggingWindow;
		private string _titleFmt;

		public bool UserWantsToOpenADifferentProject { get; set; }

		/// ------------------------------------------------------------------------------------
		private string ProjectName => Path.GetFileNameWithoutExtension(_projectPath);

		/// ------------------------------------------------------------------------------------
		private ProjectWindow()
		{
			Logger.WriteEvent("ProjectWindow constructor");

			ExceptionHandler.AddDelegate(AudioUtils.HandleGlobalNAudioException);

			InitializeComponent();

			archiveRAMPProjectToolStripMenuItem.Image = ResourceImageCache.RampIcon;

			// ReSharper disable once VirtualMemberCallInConstructor
			_titleFmt = Text;
			_menuShowMPlayerDebugWindow.Tag = _menuProject.DropDownItems.IndexOf(_menuShowMPlayerDebugWindow);
			_menuProject.DropDownItems.Remove(_menuShowMPlayerDebugWindow);

			_menuShortFileNameWarningSettings.Visible = 
				Settings.Default.SuppressAllShortFilenameWarnings ||
				!IsNullOrEmpty(Settings.Default.ShortFilenameWarningsToSuppressByVolume) ||
				!IsNullOrEmpty(Settings.Default.ShortFilenameWarningsToSuppress);
		}

		private static Control FindFocusedControl(Control control)
		{
			var container = control as IContainerControl;
			while (container != null)
			{
				control = container.ActiveControl;
				container = control as IContainerControl;
			}
			return control;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData != Keys.Enter)
				return base.ProcessDialogKey(keyData);

			var control = FindFocusedControl(this);

			if (!control.ShouldTreatEnterAsTab())
				return base.ProcessDialogKey(keyData);

			// Go to the next control in the tab order.
			// The purpose for this to move to the next input field, so we are purposely
			// skipping over buttons and hyperlink controls.
			var next = GetNextControl(control, true);
			while (next != null)
			{
				if (next.ShouldTabToMe())
					break;

				next = GetNextControl(next, true);
			}

			// If no suitable control was found, perhaps we need to wrap back to the first
			// control. SelectNextControl() will do that.
			if (next == null)
				SelectNextControl(control, true, true, true, true);
			else
				next.Focus();

			return true;
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
			LocalizeItemDlg<XLiffDocument>.StringsLocalized += SetWindowText;

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

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				FailedToGetShortName -= HandleFailureToGetShortName;

				LocalizeItemDlg<XLiffDocument>.StringsLocalized -= SetWindowText;

				ExceptionHandler.RemoveDelegate(AudioUtils.HandleGlobalNAudioException);

				components?.Dispose();

				_viewTabGroup?.Tabs.Clear();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the localized window title texts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetWindowText(ILocalizationManager lm = null)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
			{
				var ver = Assembly.GetExecutingAssembly().GetName().Version;
				_titleFmt = Text;
				Text = Format(_titleFmt, ProjectName, ver.Major, ver.Minor, ver.Build,
					ApplicationContainer.GetBuildTypeDescriptor(BuildType.Current));
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.ProjectWindow.InitializeForm(this);
			base.OnLoad(e);

			_viewTabGroup.SetActiveView(_viewTabGroup.Tabs[0]);

			UpdateChecker = new Sparkle(@"https://software.sil.org/downloads/r/saymore/appcast.xml",
				Icon);
			// The SayMore installer already takes care of launching.
			UpdateChecker.DoLaunchAfterUpdate = false;
			// We don't want to do this until the main window is loaded because:
			// (a) it's very easy for the user to overlook, and
			// (b) more importantly, when the toast notifier closes, it can sometimes clobber an
			// error message being displayed for the user.
			UpdateChecker.CheckOnFirstApplicationIdle();
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			ReportAnyFileLoadErrors();
			FailedToGetShortName += HandleFailureToGetShortName;
		}

		private void HandleFailureToGetShortName(string path, string failedActionDescription)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			var filename = Path.GetFileName(path);
			if (filename == Empty)
			{
				throw new ArgumentException("Must be an actual file, not merely a volume or folder.",
					nameof(path));
			}

			this.SafeInvoke(() => { AlertUserToFailureToGetShortName(path, failedActionDescription); },
				"handling failure to get short name");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This does the UI-related stuff for <see cref="HandleFailureToGetShortName"/>, so it
		/// needs to be invoked on the UI thread.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AlertUserToFailureToGetShortName(string path, string failedActionDescription)
		{
			ShortFileNameWarningDlg.NoteFailure(this, path, failedActionDescription);
			_menuShortFileNameWarningSettings.Visible = true;
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
			using (var dlg = new SILAboutBox(FileLocationUtilities.GetFileDistributedWithApplication("aboutbox.htm")))
			{
				dlg.Text += "\u2122";
				dlg.CheckForUpdatesClicked += HandleAboutDialogCheckForUpdatesClick;
				dlg.ShowDialog(this);
			}
		}

		private static void HandleAboutDialogCheckForUpdatesClick(object sender, EventArgs e)
		{
			Analytics.Track("CheckForUpdates");
			var updateStatus = UpdateChecker.CheckForUpdatesAtUserRequest();
			if (sender is SILAboutBox aboutBox && updateStatus == Sparkle.UpdateStatus.UpdateNotAvailable)
				aboutBox.NotifyNoUpdatesAvailable();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleHelpClick(object sender, EventArgs e)
		{
			//nb: when the file is in our source code, and not in the program directory, windows security will squawk and then not show content.
			var path = FileLocationUtilities.GetFileDistributedWithApplication(false,"SayMore.chm");
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
		private void HandleArchiveWithRAMPProjectMenuItemClick(object sender, EventArgs e)
		{
			Program.ArchiveProjectUsingRAMP(this);
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

                Program.UpdateUiLanguageForUser(dlg.UILanguage);
            }
		}

		/// ------------------------------------------------------------------------------------
		private void HandleShortFileNameWarningSettingsMenuClick(object sender, EventArgs e)
		{
			Analytics.Track("Accessed short filename warning settings via menu");
			ShortFileNameWarningDlg.ViewSettings(this, _projectPath);
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
			if (!(activatedTab.View is ISayMoreView view))
				return;

			if (view.MainMenuItem != null)
				view.MainMenuItem.Enabled = true;

			Analytics.Track(view.NameForUsageReporting + "View Activated");
		}

		/// ------------------------------------------------------------------------------------
		private void HandleViewDeactivated(ViewTabGroup sender, ViewTab deactivatedTab)
		{
			if (deactivatedTab.View is ISayMoreView view && view.MainMenuItem != null)
				view.MainMenuItem.Enabled = false;
		}
	}
}
