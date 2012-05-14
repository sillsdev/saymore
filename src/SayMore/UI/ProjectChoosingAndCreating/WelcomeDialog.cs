using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using Localization;
using Localization.UI;
using SayMore.Properties;
using SayMore.UI.ProjectChoosingAndCreating.NewProjectDialog;
using SayMore.Utilities;
using SilTools;
using NoToolStripBorderRenderer = SilTools.NoToolStripBorderRenderer;

namespace SayMore.UI.ProjectChoosingAndCreating
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Incapsulates the welcome dialog box, in which users may create new projects, or open
	/// existing projects via browsing the file systsem or by choosing a recently used project.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class WelcomeDialog : Form
	{
		public WelcomeDialogViewModel Model { get; set; }

		/// ------------------------------------------------------------------------------------
		public WelcomeDialog(WelcomeDialogViewModel viewModel)
		{
			InitializeComponent();

			Font = SystemFonts.MessageBoxFont; //use the default OS UI font

			Model = viewModel;

			if (Settings.Default.WelcomeDialog == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.WelcomeDialog = FormSettings.Create(this);
			}

			tsOptions.Renderer = new NoToolStripBorderRenderer();
			tsOptions.BackColorBegin = Color.White;
			tsOptions.BackColorEnd = Color.White;
			DialogResult = DialogResult.Cancel;

			LoadMRUButtons();

			LocalizeItemDlg.StringsLocalized += LocalizationInitiated;
			LocalizationInitiated();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadMRUButtons()
		{
			_buttonMru0.Visible = false;

			int i = 0;
			foreach (var recentProjectInfo in Model.RecentlyUsedProjects)
			{
				ToolStripButton tsb;

				if (i++ == 0)
					tsb = _buttonMru0;
				else
				{
					tsb = new ToolStripButton(_buttonMru0.Image);
					//tsb.Name = "tsbMru" + i;
					tsb.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
					tsb.ImageAlign = _buttonMru0.ImageAlign;
					tsb.TextAlign = _buttonMru0.TextAlign;
					tsb.Font = _buttonMru0.Font;
					tsb.Margin = _buttonMru0.Margin;
					tsb.Click += HandleMruClick;
					int index = tsOptions.Items.IndexOf(_buttonBrowse);
					tsOptions.Items.Insert(index, tsb);
				}

				tsb.Visible = true;
				tsb.Name = recentProjectInfo.Key;
				tsb.ToolTipText = recentProjectInfo.Key;
				tsb.Text = recentProjectInfo.Value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Setups the link label with proper localizations. This method gets called from the
		/// constructor and after strings are localized in the string localizing dialog box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LocalizationInitiated()
		{
			_labelVersionInfo.Text = ApplicationContainer.GetVersionInfo(_labelVersionInfo.Text);

			var entireLink = _linkWebSites.Text;

			var silPortion = LocalizationManager.GetString("DialogBoxes.WelcomeDlg._linkWebSites_SILPortion",
				"SIL International", "This is the portion of the text that is underlined, indicating the link to the SIL web site.");

			var appPortion = LocalizationManager.GetString("DialogBoxes.WelcomeDlg._linkWebSites_ApplicationPortion", "SayMore web site",
				"This is the portion of the text that is underlined, indicating the link to the application's web site.");

			_linkWebSites.Links.Clear();

			// Add the underline and link for SIL's website.
			int i = entireLink.IndexOf(silPortion);
			if (i >= 0)
				_linkWebSites.Links.Add(i, silPortion.Length, Settings.Default.SilWebSite);

			// Add the underline and link for application's website.
			i = entireLink.IndexOf(appPortion);
			if (i >= 0)
				_linkWebSites.Links.Add(i, appPortion.Length, Settings.Default.ProgramsWebSite);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.WelcomeDialog.InitializeForm(this);
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			Settings.Default.Save();
			LocalizeItemDlg.StringsLocalized -= LocalizationInitiated;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWebSiteLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string tgt = e.Link.LinkData as string;

			if (!string.IsNullOrEmpty(tgt))
				System.Diagnostics.Process.Start(tgt);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBrowseForExistingProjectClick(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				dlg.Title = LocalizationManager.GetString("DialogBoxes.WelcomeDlg.OpenFileDlgCaption", "Open SayMore Project");
				var prjFilterText = LocalizationManager.GetString("DialogBoxes.WelcomeDlg.ProjectFileType", "SayMore Project (*.sprj)");

				// TODO: This should really be a static or at least in a class that is accessible
				// from anywhere because this is the second place it's used. I'm hesitant to use
				// a static, because I don't understand the DI, app. container and project
				// context stuff well enough.

				// JH says: The di approach is to inject, not reach out.
				// I.e., it should be a parameter to the contructor of this class.
				var projPath = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SayMore");

				dlg.Filter = prjFilterText + "|*.sprj";
				dlg.InitialDirectory = projPath;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				if (dlg.ShowDialog(this) == DialogResult.Cancel)
					return;

				Model.ProjectSettingsFilePath = dlg.FileName;
			}

			DialogResult = DialogResult.OK;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCreateProjectClick(object sender, EventArgs e)
		{
			var viewModel = new NewProjectDlgViewModel();

			using (var dlg = new NewProjectDlg(viewModel))
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					Model.SetRequestedPath(viewModel.ParentFolderPathForNewProject, viewModel.NewProjectName);
					DialogResult = DialogResult.OK;
					Close();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMruClick(object sender, EventArgs e)
		{
			var tsb = sender as ToolStripButton;
			if (tsb != null)
			{
				Model.ProjectSettingsFilePath = tsb.Name;
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			var rc = new Rectangle(0, 0, ClientSize.Width, 45);

			// Draw the gradient blue bar.
			using (var br = new LinearGradientBrush(rc, AppColors.BarBegin, AppColors.BarEnd, 0.0f))
				e.Graphics.FillRectangle(br, rc);

			// Draw a line at the bottom of the gradient blue bar.
			using (var pen = new Pen(AppColors.BarBorder))
				e.Graphics.DrawLine(pen, 0, rc.Bottom, rc.Right, rc.Bottom);

			rc = new Rectangle(new Point(_labelSubTitle.Left - 6, 18), Resources.SayMoreText.Size);
			//rc.Inflate(-4, -4);
			e.Graphics.DrawImage(Resources.SayMoreText, rc);

			// Draw the application's logo image.
			rc = new Rectangle(new Point(pnlOptions.Left - 10, 0), Resources.LargeSayMoreLogo.Size);
			e.Graphics.DrawImage(Resources.LargeSayMoreLogo, rc);
		}
	}
}