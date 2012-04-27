using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SayMore.Properties;
using SayMore.Utilities.Utilities;

namespace SayMore.Utilities
{
	/// ----------------------------------------------------------------------------------------
	public partial class SplashScreenForm : SilTools.SplashScreenForm
	{
		private const int kLogoTextImageTop = 18;

		private readonly int _logoTextLeft;

		/// ------------------------------------------------------------------------------------
		public SplashScreenForm()
		{
			InitializeComponent();

			ShowStandardSILContent = false;

			_logoTextLeft = Resources.LargeSayMoreLogo.Size.Width + 40;
			_labelLoading.Location = new Point(_logoTextLeft, kLogoTextImageTop + Resources.SayMoreText.Height);

			Width = Resources.LargeSayMoreLogo.Size.Width + Resources.SayMoreText.Width + 80;
			Height = Resources.LargeSayMoreLogo.Size.Height + kLogoTextImageTop + 30;

			_labelVersionInfo.Text = ApplicationContainer.GetVersionInfo(_labelVersionInfo.Text);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetMessage(string value)
		{
			_labelLoading.Text = string.Format("Loading: {0}...", value);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			// Draw a gradient from top to bottom of window.
			var rc = new Rectangle(0, 45, ClientSize.Width, ClientSize.Height - 45);
			using (var br = new LinearGradientBrush(rc, Color.White, AppColors.BarBegin, 90f))
				e.Graphics.FillRectangle(br, rc);

			// Draw a line at the bottom of the gradient blue bar.
			using (var pen = new Pen(AppColors.BarBorder))
				e.Graphics.DrawLine(pen, 0, rc.Bottom, rc.Right, rc.Bottom);

			rc = new Rectangle(0, 0, ClientSize.Width, 45);

			// Draw the gradient blue bar.
			using (var br = new LinearGradientBrush(rc, AppColors.BarBegin, AppColors.BarEnd, 0.0f))
				e.Graphics.FillRectangle(br, rc);

			// Draw a line at the bottom of the gradient blue bar.
			using (var pen = new Pen(AppColors.BarBorder))
				e.Graphics.DrawLine(pen, 0, rc.Bottom, rc.Right, rc.Bottom);

			// Draw the application's logo image.
			rc = new Rectangle(new Point(30, 0), Resources.LargeSayMoreLogo.Size);
			e.Graphics.DrawImage(Resources.LargeSayMoreLogo, rc);

			// Draw logo text.
			rc = new Rectangle(new Point(_logoTextLeft, kLogoTextImageTop), Resources.SayMoreText.Size);
			//rc = new Rectangle(new Point(_logoTextLeft, 18), Resources.SayMoreText.Size);
			e.Graphics.DrawImage(Resources.SayMoreText, rc);

			// Draw border around window.
			rc = new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
			using (var pen = new Pen(AppColors.BarBorder))
				e.Graphics.DrawRectangle(pen, rc);
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class SplashScreen : SilTools.SplashScreen
	{
		private SplashScreenForm _splashScreenForm;

		/// ------------------------------------------------------------------------------------
		protected override SilTools.SplashScreenForm GetSplashScreenForm()
		{
			_splashScreenForm = new SplashScreenForm();
			return _splashScreenForm;
		}
	}
}
