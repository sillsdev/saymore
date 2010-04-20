using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using SIL.Localization;
using SIL.Sponge.Properties;
using SIL.Sponge.Utilities;


namespace SIL.Sponge.ConfigTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Incapsulates the welcome dialog box.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class WelcomeDialog : Form
	{
		public string ProjectPath { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="WelcomeDialog"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WelcomeDialog()
		{
			Font = SystemFonts.MessageBoxFont; //use the default OS UI font
			MruProjects.Initialize(Settings.Default.MRUList);

			InitializeComponent();

			var rc = Settings.Default.WelcomeDlgBounds;
			if (rc.Height < 0)
				StartPosition = FormStartPosition.CenterScreen;
			else
				Bounds = rc;

			var ver = Assembly.GetExecutingAssembly().GetName().Version;

			// The build number is just the number of days since 01/01/2000
			DateTime bldDate = new DateTime(2000, 1, 1).AddDays(ver.Build);
			lblVersionInfo.Text = string.Format(lblVersionInfo.Text, ver.Major,
				ver.Minor, ver.Revision, bldDate.ToString("dd-MMM-yyyy"));

			LoadMRUButtons();
			SetupLinkLabel();
			LocalizeItemDlg.StringsLocalized += SetupLinkLabel;

			tsOptions.BackColorBegin = Color.White;
			tsOptions.BackColorEnd = Color.White;

			DialogResult = DialogResult.Cancel;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads buttons for the most recently used projects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadMRUButtons()
		{
			tsbMru0.Visible = false;

			int i = 0;
			foreach (string prjName in MruProjects.Paths)
			{
				if (prjName == null)
					continue;

				ToolStripButton tsb;

				if (i++ == 0)
					tsb = tsbMru0;
				else
				{
					tsb = new ToolStripButton(tsbMru0.Image);
					tsb.Name = "tsbMru" + i;
					tsb.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
					tsb.ImageAlign = tsbMru0.ImageAlign;
					tsb.TextAlign = tsbMru0.TextAlign;
					tsb.Font = tsbMru0.Font;
					tsb.Margin = tsbMru0.Margin;
					tsb.Click += OnMru_Click;
					int index = tsOptions.Items.IndexOf(tsbBrowse);
					tsOptions.Items.Insert(index, tsb);
				}

				tsb.Visible = true;
				tsb.ToolTipText = prjName;

				// For the text, use only the project file's immediate parent
				// folder for the project name.
				var dir = Path.GetDirectoryName(prjName);
				int isep = dir.LastIndexOf(Path.DirectorySeparatorChar);
				tsb.Text = (isep >= 0 ? dir.Substring(isep + 1) : dir);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Setups the link label with proper localizations. This method gets called from the
		/// constructor and after strings are localized in the string localizing dialog box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupLinkLabel()
		{
			LocalizationManager.LocalizeObject(lnkWebSites, "WelcomeDialog.lnkWebSites",
				"Sponge is brought to you by SIL International.  Visit the Sponge web Site.",
				"Dialog Boxes");

			var entireLink = LocalizationManager.GetString(lnkWebSites);

			var silPortion = LocalizationManager.LocalizeString(
				"WelcomeDialog.lnkWebSites.SILLinkPortion", "SIL International",
				"This is the portion of the text that is underlined, indicating the link " +
				"to the SIL web site.", "Dialog Boxes");

			var spongePortion = LocalizationManager.LocalizeString(
				"WelcomeDialog.lnkWebSites.SpongeLinkPortion", "Sponge web site",
				"This is the portion of the text that is underlined, indicating the link " +
				"to the Sponge web site.", "Dialog Boxes");

			lnkWebSites.Links.Clear();

			// Add the underline and link for SIL's website.
			int i = entireLink.IndexOf(silPortion);
			if (i >= 0)
				lnkWebSites.Links.Add(i, silPortion.Length, "www.sil.org");

			// Add the underline and link for Sponge's website.
			i = entireLink.IndexOf(spongePortion);
			if (i >= 0)
				lnkWebSites.Links.Add(i, spongePortion.Length, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the LinkClicked event of the lnkWebSites control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lnkWebSites_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string tgt = e.Link.LinkData as string;

			if (!string.IsNullOrEmpty(tgt))
				System.Diagnostics.Process.Start(tgt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Browse for an existing project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OnBrowse_Click(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				var caption = LocalizationManager.LocalizeString(
					"WelcomeDialog.OpenFileDlgCaption", "Open Sponge Project", "Dialog Boxes");

				var prjFilterText = LocalizationManager.LocalizeString(
					"WelcomeDialog.SpongePrjFileType", "Sponge Project (*.sprj)", "Dialog Boxes");

				dlg.Title = caption;
				dlg.Filter = prjFilterText + "|*.sprj";//review: why allow these? +Sponge.OFDlgAllFileTypeText + "|*.*";
				dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //SpongeProject.ProjectsFolder;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				if (dlg.ShowDialog(this) == DialogResult.Cancel)
					return;

				ProjectPath = dlg.FileName;
			}

			DialogResult = DialogResult.OK;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create a new project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OnCreate_Click(object sender, EventArgs e)
		{
//			var viewModel = new NewProjectDlgViewModel();
//
//			using (var dlg = new NewProjectDlg(viewModel))
//			{
//				if (dlg.ShowDialog(this) != DialogResult.OK)
//					return;
//				ProjectPath = dlg.Path;
//			}

//			Project = SpongeProject.Create(viewModel.NewProjectFolderPath);
//
//			MruProjects.AddNewPath(Project.FullFilePath);
//			MruProjects.Save();
//			DialogResult = DialogResult.OK;
//			Close();

		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load one of the MRU projects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OnMru_Click(object sender, EventArgs e)
		{
			var tsb = sender as ToolStripButton;
			if (tsb == null)
				return;

			// The full path to the project file is in the tooltip of the button.
			ProjectPath = tsb.ToolTipText;//hack
			DialogResult = DialogResult.OK;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			Settings.Default.WelcomeDlgBounds = Bounds;
			Settings.Default.Save();
			LocalizeItemDlg.StringsLocalized -= SetupLinkLabel;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles painting some stuff.
		/// </summary>
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
