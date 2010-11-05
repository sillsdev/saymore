using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SayMore.Properties;
using SayMore.UI.Utilities;

namespace SayMore.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class SplashScreenForm : SilUtils.SplashScreenForm
	{
		private readonly int _logoTextLeft;

		/// ------------------------------------------------------------------------------------
		public SplashScreenForm()
		{
			InitializeComponent();

			ShowStandardSILContent = false;

			_logoTextLeft = Resources.LargeSayMoreLogo.Size.Width + 40;
			_labelLoading.Location = new Point(_logoTextLeft, Resources.SayMoreText.Height + 25);

			Width = Resources.LargeSayMoreLogo.Size.Width + Resources.SayMoreText.Width + 80;
			Height = Resources.LargeSayMoreLogo.Size.Height + 40;

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
			rc = new Rectangle(new Point(_logoTextLeft, 18), Resources.SayMoreText.Size);
			e.Graphics.DrawImage(Resources.SayMoreText, rc);

			// Draw border around window.
			rc = new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
			using (var pen = new Pen(AppColors.BarBorder))
				e.Graphics.DrawRectangle(pen, rc);
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class SplashScreen : SilUtils.SplashScreen
	{
		private SplashScreenForm _splashScreenForm;

		/// ------------------------------------------------------------------------------------
		protected override SilUtils.SplashScreenForm GetSplashScreenForm()
		{
			_splashScreenForm = new SplashScreenForm();
			return _splashScreenForm;
		}
	}
}
