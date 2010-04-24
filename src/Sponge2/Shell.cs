using System;
using System.Windows.Forms;
using SIL.Localization;
using SIL.Sponge.ConfigTools;
using SIL.Sponge.Properties;
using SilUtils;
using Sponge2;

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class for the main window of the application.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class Shell : Form
	{
		public delegate Shell Factory(string projectName); //autofac uses this

		private ViewButtonManager _viewManger;
		private string _projectName;

		public static bool Resizing { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current Sponge project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		//public static SpongeProject CurrentProject { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Shell"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Shell()
		{
			InitializeComponent();

			if (Settings.Default.MainWindowBounds.Height < 0)
				StartPosition = FormStartPosition.CenterScreen;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Shell"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Shell(string projectName, SessionsControl sessionsControl) : this()
		{
			_projectName = projectName;
			var views = new Control[] { sessionsControl };

			Controls.AddRange(views);
			_viewManger = new ViewButtonManager(tsMain, views);

			_viewManger.SetView(tsbSessions);
			SetWindowText();

			//only one life to live, nowadays: LocalizeItemDlg.StringsLocalized -= SetWindowText;
			LocalizeItemDlg.StringsLocalized += SetWindowText;
			Show();
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
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// Do this here because it doesn't work in the constructor.
			if (Settings.Default.MainWindowBounds.Height >= 0)
				Bounds = Settings.Default.MainWindowBounds;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			Settings.Default.MainWindowBounds = Bounds;
			Settings.Default.Save();
			LocalizeItemDlg.StringsLocalized -= SetWindowText;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.ResizeBegin"/> event.
		/// </summary>
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
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.ResizeEnd"/> event.
		/// </summary>
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
		private void tsbChangeProjects_Click(object sender, EventArgs e)
		{
			UserWantsToOpenADifferentProject = true;
			this.Close();
		}

		public bool UserWantsToOpenADifferentProject { get; set; }
	}
}
