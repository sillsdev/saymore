using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SIL.Localization;
using SayMore.Properties;
using SayMore.UI.ProjectChoosingAndCreating.NewProjectDialog;
using SayMore.UI.Utilities;

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
//		/// ------------------------------------------------------------------------------------
//		public WelcomeDialog()
//		{
//			Font = SystemFonts.MessageBoxFont; //use the default OS UI font
//			InitializeComponent();
//		}

		/// ------------------------------------------------------------------------------------
		public WelcomeDialog(WelcomeDialogViewModel viewModel)
		{
			InitializeComponent();
			Model = viewModel;

			var rc = Settings.Default.WelcomeDialogBounds;
			if (rc.Height < 0)
				StartPosition = FormStartPosition.CenterScreen;
			else
				Bounds = rc;

			tsOptions.BackColorBegin = Color.White;
			tsOptions.BackColorEnd = Color.White;
			DialogResult = DialogResult.Cancel;

			LoadMRUButtons();
			if(LocalizationManager.Enabled)//review: to make work with dumb unit tests
				LocalizationInitiated();
			LocalizeItemDlg.StringsLocalized += LocalizationInitiated;
		}

		public WelcomeDialogViewModel Model { get; set; }

		/// ------------------------------------------------------------------------------------
		private void LoadMRUButtons()
		{
			tsbMru0.Visible = false;

			int i = 0;
			foreach (var recentProjectInfo in Model.RecentlyUsedProjects)
			{
				ToolStripButton tsb;

				if (i++ == 0)
					tsb = tsbMru0;
				else
				{
					tsb = new ToolStripButton(tsbMru0.Image);
					//tsb.Name = "tsbMru" + i;
					tsb.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
					tsb.ImageAlign = tsbMru0.ImageAlign;
					tsb.TextAlign = tsbMru0.TextAlign;
					tsb.Font = tsbMru0.Font;
					tsb.Margin = tsbMru0.Margin;
					tsb.Click += HandleMruClick;
					int index = tsOptions.Items.IndexOf(tsbBrowse);
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
			lblVersionInfo.Text = Model.GetVersionInfo(lblVersionInfo.Text);

			LocalizationManager.LocalizeObject(lnkWebSites, "WelcomeDialog.lnkWebSites",
											   "Sponge is brought to you by SIL International.  Visit the Sponge web site.",
											   locExtender.LocalizationGroup);

			var entireLink = LocalizationManager.GetString((object) lnkWebSites);

			var silPortion = LocalizationManager.LocalizeString(
				"WelcomeDialog.lnkWebSites.SILLinkPortion", "SIL International",
				"This is the portion of the text that is underlined, indicating the link " +
				"to the SIL web site.", locExtender.LocalizationGroup);

			var spongePortion = LocalizationManager.LocalizeString(
				"WelcomeDialog.lnkWebSites.SpongeLinkPortion", "Sponge web site",
				"This is the portion of the text that is underlined, indicating the link " +
				"to the Sponge web site.", locExtender.LocalizationGroup);

			lnkWebSites.Links.Clear();

			// Add the underline and link for SIL's website.
			int i = entireLink.IndexOf(silPortion);
			if (i >= 0)
				lnkWebSites.Links.Add(i, silPortion.Length, Settings.Default.SilWebSite);

			// Add the underline and link for Sponge's website.
			i = entireLink.IndexOf(spongePortion);
			if (i >= 0)
				lnkWebSites.Links.Add(i, spongePortion.Length, Settings.Default.ProgramsWebSite);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			Settings.Default.WelcomeDialogBounds = Bounds;
			Settings.Default.Save();
			LocalizeItemDlg.StringsLocalized -= LocalizationInitiated;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
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
				dlg.Title = LocalizationManager.LocalizeString(
					"WelcomeDialog.OpenFileDlgCaption", "Open Sponge Project",
					locExtender.LocalizationGroup);

				var prjFilterText = LocalizationManager.LocalizeString(
					"WelcomeDialog.ProjectFileType", "Sponge Project (*.sprj)",
					locExtender.LocalizationGroup);

				dlg.Filter = prjFilterText + "|*.sprj";
				dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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
//			using (var dlg = new FolderBrowserDialog())
//			{
//				dlg.Description = LocalizationManager.LocalizeString(
//					"WelcomeDialog.CreateProjectFolderBrowserMsg",
//					"Choose the folder in which to create a project.",
//					locExtender.LocalizationGroup);
//
//				dlg.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
//
//				if (DialogResult.Cancel == dlg.ShowDialog())
//					return;
//
//				if (Model.CreateNewProject(dlg.SelectedPath))
//				{
//					DialogResult = DialogResult.OK;
//					Close();
//				}
//			}
				var viewModel = new NewProjectDlgViewModel();

				using (var dlg = new NewProjectDlg(viewModel))
				{
					if (dlg.ShowDialog() == DialogResult.OK)
					{
//						if (Model.CreateNewProject(viewModel.ParentFolderPathForNewProject, viewModel.NewProjectName))
//						{
						Model.SetRequestedPath(viewModel.ParentFolderPathForNewProject, viewModel.NewProjectName);
						DialogResult = DialogResult.OK;
							Close();
//						}
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
			using (var br = new LinearGradientBrush(rc, SpongeColors.BarBegin,
													SpongeColors.BarEnd, 0.0f))
			{
				e.Graphics.FillRectangle(br, rc);
			}

			// Draw a line at the bottom of the gradient blue bar.
			using (var pen = new Pen(SpongeColors.BarBorder))
				e.Graphics.DrawLine(pen, 0, rc.Bottom, rc.Right, rc.Bottom);

			rc = new Rectangle(new Point(lblSubTitle.Left - 6, 4), Resources.kimidSpongeText.Size);
			rc.Inflate(-4, -4);
			e.Graphics.DrawImage(Resources.kimidSpongeText, rc);

			// Draw the Sponge logo image.
			rc = new Rectangle(new Point(pnlOptions.Left - 10, 16), Resources.kimidSponge.Size);
			rc.Inflate(-8, -8);
			e.Graphics.DrawImage(Resources.kimidSponge, rc);
		}
	}
}