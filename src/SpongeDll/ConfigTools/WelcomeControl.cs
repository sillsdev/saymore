using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Palaso.I8N;
using SIL.Sponge.Controls;

namespace SIL.Sponge.ConfigTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class WelcomeControl : UserControl
	{
		public event EventHandler NewProjectClicked;
		public Action<string> OpenSpecifiedProject;
		public event EventHandler ChooseProjectClicked;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="WelcomeControl"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WelcomeControl()
		{
			Font = SystemFonts.MessageBoxFont;//use the default OS UI font
			InitializeComponent();

			var ver = Assembly.GetExecutingAssembly().GetName().Version;

			// The build number is just the number of days since 01/01/2000
			DateTime bldDate = new DateTime(2000, 1, 1).AddDays(ver.Build);
			lblVersionInfo.Text = "Version " + ver.Major + "." + ver.Minor + "." +
				ver.Revision + "    Built on " + bldDate.ToString("dd-MMM-yyyy");

			LoadButtons();

			tsOptions.BackColorBegin = Color.White;
			tsOptions.BackColorEnd = Color.White;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadButtons()
		{
			flwPanel.Controls.Clear();
			var createAndGetGroup = new TableLayoutPanel();
			createAndGetGroup.AutoSize = true;
			AddCreateChoices(createAndGetGroup);
			AddGetChoices(createAndGetGroup);

			var openChoices = new TableLayoutPanel();
			openChoices.AutoSize = true;
			AddSection("Open", openChoices);
			AddOpenProjectChoices(openChoices);
			flwPanel.Controls.AddRange(new Control[] { createAndGetGroup, openChoices });
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the section.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddSection(string sectionName, TableLayoutPanel panel)
		{
			panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			panel.RowCount++;
			var label = new Label();
			label.Font = new Font(StringCatalog.LabelFont.FontFamily, lblTemplate.Font.Size, lblTemplate.Font.Style);
			label.ForeColor = lblTemplate.ForeColor;
			label.Text = sectionName;
			label.Margin = new Padding(0, 20, 0, 0);
			panel.Controls.Add(label);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the file choice.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddFileChoice(string path, TableLayoutPanel panel)
		{
			var button = AddChoice(Path.GetFileNameWithoutExtension(path), path,
				"wesayProject", true, openRecentProject_LinkClicked, panel);

			button.Tag = path;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the choice.
		/// </summary>
		/// <param name="localizedLabel">The localized label.</param>
		/// <param name="localizedTooltip">The localized tooltip.</param>
		/// <param name="imageKey">The image key.</param>
		/// <param name="enabled">if set to <c>true</c> [enabled].</param>
		/// <param name="clickHandler">The click handler.</param>
		/// <param name="panel">The panel.</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private Button AddChoice(string localizedLabel, string localizedTooltip,
			string imageKey, bool enabled, EventHandler clickHandler, TableLayoutPanel panel)
		{
			panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			panel.RowCount++;
			var button = new Button();
			button.Anchor = AnchorStyles.Top | AnchorStyles.Left;

			button.Size = btnTemplate.Size;//review
			button.Font = new Font(StringCatalog.LabelFont.FontFamily, btnTemplate.Font.Size, btnTemplate.Font.Style);
			button.ImageKey = imageKey;
			button.ImageList = _imageList;
			button.ImageAlign = ContentAlignment.MiddleLeft;
			button.Click += clickHandler;
			button.Text = "  " + localizedLabel;

			button.FlatAppearance.BorderSize = btnTemplate.FlatAppearance.BorderSize;
			button.FlatAppearance.BorderColor = btnTemplate.FlatAppearance.BorderColor;
			button.FlatAppearance.MouseDownBackColor = btnTemplate.FlatAppearance.MouseDownBackColor;
			button.FlatAppearance.MouseOverBackColor = btnTemplate.FlatAppearance.MouseOverBackColor;

			button.FlatStyle = btnTemplate.FlatStyle;
			button.ImageAlign = btnTemplate.ImageAlign;
			button.TextImageRelation = btnTemplate.TextImageRelation;
			button.UseVisualStyleBackColor = btnTemplate.UseVisualStyleBackColor;
			button.Enabled = enabled;

			toolTip1.SetToolTip(button, localizedTooltip);
			panel.Controls.Add(button);
			return button;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the create choices.
		/// </summary>
		/// <param name="panel">The panel.</param>
		/// ------------------------------------------------------------------------------------
		private void AddCreateChoices(TableLayoutPanel panel)
		{
			AddSection("Create", panel);

			AddChoice("Create new blank project", string.Empty, "newProject", true,
				createNewProject_LinkClicked, panel);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the get choices.
		/// </summary>
		/// <param name="panel">The panel.</param>
		/// ------------------------------------------------------------------------------------
		private void AddGetChoices(TableLayoutPanel panel)
		{
			//AddSection("Get", panel);
			////nb: we want these always enabled, so that we can give a message explaining about hg if needed
			//AddChoice("Get From USB drive", "Get a project from a Chorus repository on a USB flash drive", "getFromUsb", true, OnGetFromUsb, panel);
			//AddChoice("Get from Internet", "Get a project from a Chorus repository which is hosted on the internet (e.g. public.languagedepot.org) and put it on this computer",
			//"getFromInternet", true, OnGetFromInternet, panel);
		}

		private void OnGetFromInternet(object sender, EventArgs e)
		{
			//if (!Chorus.UI.Misc.ReadinessDialog.ChorusIsReady)
			//{
			//using (var dlg = new Chorus.UI.Misc.ReadinessDialog())
			//{
			//dlg.ShowDialog();
			//return;
			//}
			//}
			//if (!Directory.Exists(Project.WeSayWordsProject.NewProjectDirectory))
			//{
			////e.g. mydocuments/wesay
			//Directory.CreateDirectory(Project.WeSayWordsProject.NewProjectDirectory);
			//}
			//using (var dlg = new Chorus.UI.Clone.GetCloneFromInternetDialog(Project.WeSayWordsProject.NewProjectDirectory))
			//{
			//if (DialogResult.Cancel == dlg.ShowDialog())
			//return;
			//OpenSpecifiedProject(dlg.PathToNewProject);
			//}
		}

		private void OnGetFromUsb(object sender, EventArgs e)
		{
			//if(!Chorus.UI.Misc.ReadinessDialog.ChorusIsReady)
			//{
			//using (var dlg = new Chorus.UI.Misc.ReadinessDialog())
			//{
			//dlg.ShowDialog();
			//return;
			//}
			//}
			//if (!Directory.Exists(Project.WeSayWordsProject.NewProjectDirectory))
			//{
			////e.g. mydocuments/wesay
			//Directory.CreateDirectory(Project.WeSayWordsProject.NewProjectDirectory);
			//}
			//using (var dlg = new Chorus.UI.Clone.GetCloneFromUsbDialog(Project.WeSayWordsProject.NewProjectDirectory))
			//{
			//dlg.Model.ProjectFilter = dir => GetLooksLikeWeSayProject(dir);
			//if (DialogResult.Cancel == dlg.ShowDialog())
			//return;
			//OpenSpecifiedProject(dlg.PathToNewProject);
			//}
		}

		private static bool GetLooksLikeWeSayProject(string directoryPath)
		{
			return Directory.GetFiles(directoryPath, "*.WeSayConfig").Length > 0;
		}

		private void AddOpenProjectChoices(TableLayoutPanel panel)
		{
			//int count = 0;
			//foreach (string path in Settings.Default.MruConfigFilePaths.Paths)
			//{
			//AddFileChoice(path, panel);
			//++count;
			//if (count > 2)
			//break;

			//}
			//AddChoice("Browse for other projects...", string.Empty, "browse", true, openDifferentProject_LinkClicked, panel);
		}

		private void openRecentProject_LinkClicked(object sender, EventArgs e)
		{
			if (OpenSpecifiedProject != null)
				OpenSpecifiedProject.Invoke(((Button)sender).Tag as string);
		}

		private void openDifferentProject_LinkClicked(object sender, EventArgs e)
		{
			if (ChooseProjectClicked != null)
				ChooseProjectClicked.Invoke(this, null);
		}

		private void createNewProject_LinkClicked(object sender, EventArgs e)
		{
			if (NewProjectClicked != null)
				NewProjectClicked.Invoke(this, null);
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
			using (var br = new LinearGradientBrush(rc, SpongeBar.DefaultSpongeBarColorBegin,
				SpongeBar.DefaultSpongeBarColorEnd, 0.0))
			{
				e.Graphics.FillRectangle(br, rc);
			}

			// Draw a line at the bottom of the gradient blue bar.
			using (var pen = new Pen(SpongeBar.DefaultSpongeBarColorEnd))
				e.Graphics.DrawLine(pen, 0, rc.Bottom, rc.Right, rc.Bottom);

			// Draw the sponge text, half in the blue bar and half below.
			//using (var fnt = new Font(lblSubTitle.Font.FontFamily, 28, FontStyle.Bold))
			//{
			//    TextRenderer.DrawText(e.Graphics, "Sponge", fnt,
			//        new Point(lblSubTitle.Left - 6, 16), lblSubTitle.ForeColor);
			//}

			rc = new Rectangle(new Point(lblSubTitle.Left - 6, 4),
				Properties.Resources.kimidSpongeText.Size);
			rc.Inflate(-4, -4);
			e.Graphics.DrawImage(Properties.Resources.kimidSpongeText, rc);

			// Draw the Sponge logo image.
			rc = new Rectangle(pnlOptions.Left, 16, 80, 80);
			e.Graphics.DrawImage(Properties.Resources.kimidSponge, rc);
		}

		private void tsOptions_Paint(object sender, PaintEventArgs e)
		{

		}
	}
}