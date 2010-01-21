using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using SIL.Sponge.Controls;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;

namespace SIL.Sponge.ConfigTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Incapsulates the welcome dialog box.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class WelcomeDlg : Form
	{
		public SpongeProject SpongeProject { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="WelcomeDlg"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WelcomeDlg()
		{
			Font = SystemFonts.MessageBoxFont;//use the default OS UI font
			MruProjects.Initialize(Settings.Default.MRUList);

			InitializeComponent();

			Size sz = Settings.Default.WelcomeDlgSize;
			Point pt = Settings.Default.WelcomeDlgLocation;

			if (sz.IsEmpty && pt.IsEmpty)
				StartPosition = FormStartPosition.CenterScreen;

			if (!sz.IsEmpty)
				Size = sz;

			if (!pt.IsEmpty)
				Location = pt;

			var ver = Assembly.GetExecutingAssembly().GetName().Version;

			// The build number is just the number of days since 01/01/2000
			DateTime bldDate = new DateTime(2000, 1, 1).AddDays(ver.Build);
			lblVersionInfo.Text = "Version " + ver.Major + "." + ver.Minor + "." +
				ver.Revision + "    Built on " + bldDate.ToString("dd-MMM-yyyy");

			LoadMRUButtons();

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
					tsb.Click += tsbMru_Click;
					int index = tsOptions.Items.IndexOf(tsbBrowse);
					tsOptions.Items.Insert(index, tsb);
				}

				tsb.Visible = true;
				tsb.Text = Path.GetFileNameWithoutExtension(prjName);
				tsb.ToolTipText = prjName;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the LinkClicked event of the lnkSIL control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lnkSIL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the LinkClicked event of the lnkSpongeWebSite control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lnkSpongeWebSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Browse for an existing project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tsbBrowse_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create a new project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tsbCreate_Click(object sender, System.EventArgs e)
		{
			SpongeProject = SpongeProject.Create(this);
			if (SpongeProject != null)
			{
				MruProjects.AddNewPath(SpongeProject.ProjectPath);
				MruProjects.Save();
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load one of the MRU projects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tsbMru_Click(object sender, System.EventArgs e)
		{
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
			Settings.Default.WelcomeDlgSize = Size;
			Settings.Default.WelcomeDlgLocation = Location;
			Settings.Default.Save();
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
