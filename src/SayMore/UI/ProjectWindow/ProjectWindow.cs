using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SIL.Localization;
using SayMore.Properties;
using SayMore.UI.ElementListScreen;
using SilUtils;

namespace SayMore.UI.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class for the main window of the application.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ProjectWindow : Form
	{
		public delegate ProjectWindow Factory(string projectName); //autofac uses this

		private readonly ViewButtonManager _viewManger;
		private readonly string _projectName;
		private readonly IEnumerable<ICommand> _commands;

		public static bool Resizing { get; private set; }
		public bool UserWantsToOpenADifferentProject { get; set; }

		/// ------------------------------------------------------------------------------------
		private ProjectWindow()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public ProjectWindow(string projectName, SessionScreen sessionsScreen,
			PersonListScreen personsScreen, IEnumerable<ICommand> commands)
			: this()
		{
			if (Settings.Default.ProjectWindow == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.ProjectWindow = FormSettings.Create(this);
			}

			_projectName = projectName;
			_commands = commands;
			var views = new Control[] { new TextBox(), sessionsScreen, personsScreen };

			Controls.AddRange(views);
			_viewManger = new ViewButtonManager(_mainToolStrip, views);

			SetWindowText();
			LocalizeItemDlg.StringsLocalized += SetWindowText;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Remove and dispose of existing view controls.
		/// </summary>
		/// ------------------------------------------------------------------------------------
//		private void CleanOutViews()
//		{
//			Controls.Clear();
//			//the lifetime management of the controls is handles by the DI container
//		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
//			if (disposing && (components != null))
//			{
//				CleanOutViews();
//				components.Dispose();
//			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the localized window title texts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetWindowText()
		{
			var fmt = LocalizationManager.GetString(this);
			Text = string.Format(fmt, _projectName);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.ProjectWindow.InitializeForm(this);
			base.OnLoad(e);

			_viewManger.SetView(_toolStripButtonSessions);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			LocalizeItemDlg.StringsLocalized -= SetWindowText;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnResizeBegin(System.EventArgs e)
		{
//			if (!Settings.Default.RedrawAsMainWindowResizes)
//				Utils.SetWindowRedraw(this, false);
//			else
//				Resizing = true;

			base.OnResizeBegin(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnResizeEnd(EventArgs e)
		{
//			if (!Settings.Default.RedrawAsMainWindowResizes)
//				Utils.SetWindowRedraw(this, true);
//			else
			{
				Resizing = false;
				Invalidate(true);
			}

			base.OnResizeEnd(e);
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

		private void OnCommandMenuItem_Click(object sender, EventArgs e)
		{
			var handler = _commands.First(c => c.Id == (string) ((ToolStripMenuItem) sender).Tag);
			handler.Execute();
		}
	}

}
