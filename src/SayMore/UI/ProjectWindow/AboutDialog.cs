using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Localization;
using Localization.UI;
using Palaso.Reporting;
using SayMore.Properties;
using SayMore.UI.Utilities;

namespace SayMore.UI.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Incapsulates the welcome dialog box, in which users may create new projects, or open
	/// existing projects via browsing the file systsem or by choosing a recently used project.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class AboutDialog : Form
	{
		private const int kLogoTextImageTop = 10;

		/// ------------------------------------------------------------------------------------
		public AboutDialog()
		{
			InitializeComponent();

			Font = SystemFonts.MessageBoxFont; //use the default OS UI font
			_linkSayMoreWebSite.Font = new Font(Font.FontFamily, 9, FontStyle.Bold, GraphicsUnit.Point);
			_linkSiLWebSite.Font = new Font(Font.FontFamily, 9, FontStyle.Regular, GraphicsUnit.Point);
			_labelVersionInfo.Font = _linkSiLWebSite.Font;

			LocalizeItemDlg.StringsLocalized += LocalizationInitiated;
			LocalizationInitiated();
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

			LocalizationManager.LocalizeObject(_linkSayMoreWebSite, "AboutDialog.lnkSayMoreWebSite",
			   "Visit the SayMore web site.",
			   locExtender.LocalizationGroup);

			LocalizationManager.LocalizeObject(_linkSiLWebSite, "AboutDialog.lnkSilWebSite",
			   "SayMore is brought to you by SIL International.",
			   locExtender.LocalizationGroup);

			var entireSayMoreLink = LocalizationManager.GetString(_linkSayMoreWebSite);
			var entireSilLink = LocalizationManager.GetString(_linkSiLWebSite);

			var silWebSiteUnderlinedPortion = LocalizationManager.LocalizeString(
				"AboutDialog.lnkSilWebSite.LinkPortion", "SIL International",
				"This is the portion of the text that is underlined, indicating the link " +
				"to the SIL web site.", locExtender.LocalizationGroup);

			var appWebSiteUnderlinedPortion = LocalizationManager.LocalizeString(
				"AboutDialog.lnkSayMoreWebSite.LinkPortion", "SayMore web site",
				"This is the portion of the text that is underlined, indicating the link " +
				"to the application's web site.", locExtender.LocalizationGroup);

			_linkSayMoreWebSite.Links.Clear();
			_linkSiLWebSite.Links.Clear();

			int i = entireSayMoreLink.IndexOf(appWebSiteUnderlinedPortion);
			if (i >= 0)
				_linkSayMoreWebSite.Links.Add(i, appWebSiteUnderlinedPortion.Length, Settings.Default.ProgramsWebSite);

			i = entireSilLink.IndexOf(silWebSiteUnderlinedPortion);
			if (i >= 0)
				_linkSiLWebSite.Links.Add(i, silWebSiteUnderlinedPortion.Length, Settings.Default.SilWebSite);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);
			UsageReporter.SendNavigationNotice("AboutBox");
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
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
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			// Draw a gradient from top to bottom of window.
			var rc = new Rectangle(0, 45, ClientSize.Width, ClientSize.Height - 45);
			using (var br = new LinearGradientBrush(rc, Color.White, AppColors.BarBegin, 90f))
				e.Graphics.FillRectangle(br, rc);

			// Draw the gradient blue bar.
			rc = new Rectangle(0, 0, ClientSize.Width, 45);
			using (var br = new LinearGradientBrush(rc, AppColors.BarBegin, AppColors.BarEnd, 0.0f))
				e.Graphics.FillRectangle(br, rc);

			// Draw a line at the bottom of the gradient blue bar.
			using (var pen = new Pen(AppColors.BarBorder))
				e.Graphics.DrawLine(pen, 0, rc.Bottom, rc.Right, rc.Bottom);

			rc = new Rectangle(new Point(lblSubTitle.Left - 6, 10), Resources.SayMoreText.Size);
			//rc = new Rectangle(new Point(lblSubTitle.Left - 6, 15), Resources.SayMoreText.Size);
			//rc.Inflate(-4, -4);
			e.Graphics.DrawImage(Resources.SayMoreText, rc);

			// Draw the application's logo image.
			rc = new Rectangle(new Point(10, 5), Resources.LargeSayMoreLogo.Size);
			e.Graphics.DrawImage(Resources.LargeSayMoreLogo, rc);
		}
	}
}