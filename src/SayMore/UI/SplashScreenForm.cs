using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using SIL.Reporting;
using SIL.Windows.Forms;
using SIL.Windows.Forms.Extensions;
using SayMore.Utilities;
using Timer = System.Windows.Forms.Timer;

namespace SayMore.UI
{
	#region SplashScreenForm class
	/// ----------------------------------------------------------------------------------------
	public partial class SplashScreenForm : Form
	{
		#region Data members
		private delegate void UpdateOpacityDelegate();

		private const int kLogoTextImageTop = 18;

		// Gap kept between a label's measured text and whatever boundary (the window edge, the
		// next label, or pictureBox2) bounds its available space. This covers two things: a
		// small visual buffer so text never grows all the way to touch that boundary, and the
		// gap between TextRenderer.MeasureText's result (what GetLargestFontThatFits measures
		// against) and a Label's own, somewhat taller AutoSize-driven preferred size (what the
		// Label actually becomes once its Font is set) -- confirmed empirically to be up to
		// about 8px across these labels' font sizes.
		private const int kLabelBoundsMargin = 8;

		private readonly int _logoTextLeft;
		// NOTE: we use a Forms.Timer here (compared to Threading.Timer in FW)
		// because we can't set the stack size of a thread in the thread pool which the
		// Threading.Timer uses, so we'd get a stack overflow.
		private Timer m_timer;
		private EventWaitHandle m_waitHandle;
		protected Panel m_panel;
		protected PictureBox pictureBox1;
		protected Label lblMessage;
		protected Label lblCopyright;
		protected Label lblProductName;
		private bool m_useFading = true;
		private bool m_showStandardSILContent = true;
		protected Label lblBuildNumber;
		private readonly bool m_showBuildNum;
		private PictureBox picLoadingWheel;
		private readonly string m_buildFmt;
		private int _unscaledBottomLabelsTop;
		private int _unscaledClientHeight;
		#endregion

		#region Constructor
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Default Constructor for SplashScreen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SplashScreenForm()
		{
			Logger.WriteEvent("Starting to construct SplashScreenForm.");

			InitializeComponent();

			pictureBox2.Image = ResizeImage(SIL.Windows.Forms.Widgets.SilResources.SilLogoRandom, 
				pictureBox2.Image.Height);

			ShowStandardSILContent = false;

			_logoTextLeft = ResourceImageCache.LargeSayMoreLogo.Size.Width + 40;
			_labelLoading.Location = new Point(_logoTextLeft, kLogoTextImageTop + ResourceImageCache.SayMoreText.Height);

			Width = ResourceImageCache.LargeSayMoreLogo.Size.Width + ResourceImageCache.SayMoreText.Width + 80;
			Height = ResourceImageCache.LargeSayMoreLogo.Size.Height + kLogoTextImageTop + 30;

			_labelVersionInfo.Text = ApplicationContainer.GetVersionInfo(_labelVersionInfo.Text, BuildType.Current);

			m_buildFmt = lblBuildNumber.Text;
			lblCopyright.Font = SystemInformation.MenuFont;
			lblMessage.Font = SystemInformation.MenuFont;
			lblBuildNumber.Font = SystemInformation.MenuFont;
			Opacity = 0;

			// Captured here, before any scaling has run, as the floor ApplyLabelScaling grows
			// from and shrinks back to -- not the Designer's own values, since Width/Height
			// above already replaced those with sizes computed from the logo bitmaps.
			_unscaledBottomLabelsTop = label1.Top;
			_unscaledClientHeight = ClientSize.Height;

			ApplyLabelScaling();
			UITextScaler.ScaleChanged += HandleUiTextScaleChanged;
		}

		/// ------------------------------------------------------------------------------------
		private Bitmap ResizeImage(Bitmap originalImage, int maxHeight)
		{
			var scale = (float)maxHeight / originalImage.Height;
			var newWidth = (int)(originalImage.Width * scale);
    
			var resizedImage = new Bitmap(newWidth, maxHeight);
			using (var g = Graphics.FromImage(resizedImage))
			{
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.DrawImage(originalImage, 0, 0, newWidth, maxHeight);
			}
			return resizedImage;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SplashScreenForm(bool showBuildNum, BuildType.VersionType versionType) : this()
		{
			m_showBuildNum = showBuildNum;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Disposes of the resources (other than memory) used by the
		/// <see cref="T:System.Windows.Forms.Form"></see>.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false
		/// to release only unmanaged resources.</param>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();

				if (m_timer != null)
					m_timer.Dispose();

				UITextScaler.ScaleChanged -= HandleUiTextScaleChanged;
			}

			m_timer = null;
			m_waitHandle = null;
			base.Dispose(disposing);
		}

		#endregion

		#region Public Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the splash screen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void RealShow(EventWaitHandle waitHandle, bool useFading)
		{
			Logger.WriteEvent("Showing SplashScreenForm.");

			m_waitHandle = waitHandle;
			InitControlLabels();
			ApplyLabelScaling();
			m_useFading = useFading;

			if (!useFading)
			{
				Opacity = 1;
				Show();
			}
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Activates (brings back to the top) the splash screen (assuming it is already visible
		/// and the application showing it is the active application).
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public virtual void RealActivate()
		{
			BringToFront();
			Refresh();
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the splash screen
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public virtual void RealClose()
		{
			m_timer?.Stop();

			Close();
			Logger.WriteEvent("Closed SplashScreenForm.");
		}
		#endregion

		#region Public Properties needed for all clients
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The message to display to indicate startup activity on the splash screen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void SetMessage(string value)
		{
			_labelLoading.Text = string.Format("Loading: {0}...", value);
			ApplyLabelScaling();

			// In some rare cases, setting the text causes an exception which should just
			// be ignored.
			try
			{
				lblMessage.Text = value;
				picLoadingWheel.Visible = !string.IsNullOrEmpty(value);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public bool ShowStandardSILContent
		{
			get { return m_showStandardSILContent; }
			set
			{
				m_showStandardSILContent = value;
				m_panel.Visible = value;
				if (value)
				{
					pictureBox1.Image = ResizeImage(SIL.Windows.Forms.Widgets.SilResources.SilLogoRandom,
						pictureBox1.Image.Height);
				}
			}
		}

		#endregion

		#region Public properties set automatically in constructor for .Net apps
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The copyright info which appears in the Copyright label on the splash screen
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored. They should set the
		/// AssemblyCopyrightAttribute attribute in AssemblyInfo.cs of the executable.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		public virtual void SetCopyright(string value)
		{
			lblCopyright.Text = value.Replace("(C)", "©");
		}
		#endregion

		#region Non-public methods
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			this.CenterFormInScreen();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged"></see> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (Visible && m_useFading && m_waitHandle != null)
				m_waitHandle.Set();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tasks needing to be done when Window is being opened: Set window position.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (DesignMode)
				return;

			// set build label visibility
			lblBuildNumber.Visible = m_showBuildNum;

			// start a timer to ramp up the opacity of the window.
			m_timer = new Timer();
			m_timer.Interval = 50;
			m_timer.Tick += OnUpdateOpacity;
			m_timer.Start();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initialize text of controls prior to display
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void InitControlLabels()
		{
			try
			{
				// Set the Application label to the name of the app

				lblProductName.Text = Text = Program.ProductName;

				if (m_showBuildNum)
				{
					lblBuildNumber.Visible = true;
					var bldDate = File.GetCreationTime(Application.ExecutablePath);
					lblBuildNumber.Text = string.Format(m_buildFmt, bldDate.ToString("dd-MMM-yyyy"));
				}
				else
					lblBuildNumber.Visible = false;

				// Get copyright information from assembly info. By doing this we don't have to
				// update the splash screen each year.
				var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
				var attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
if (attributes.Length > 0)
					label1.Text = ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
			catch
			{
				// ignore errors
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleUiTextScaleChanged(object sender, EventArgs e)
		{
			ApplyLabelScaling();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Grows _labelLoading, label1 (the copyright notice), and _labelVersionInfo as large
		/// as the current UI text scale setting allows. label1 and _labelVersionInfo are
		/// allowed to grow this form's own ClientSize.Height to fit, so _labelLoading (which
		/// stops above label1's fixed position, not at the window edge) and the two bottom
		/// labels (constrained only by the available width, not a height tied to the window's
		/// current size) are each given the largest font that still fits its own space, even
		/// when that's less than the full requested scale.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ApplyLabelScaling()
		{
			var desiredScalePercent = UITextScaler.ClampScalePercent(Properties.Settings.Default.UITextScale);

			_labelLoading.Font = FontFitting.GetLargestFontThatFits(_labelLoading.Text,
				UITextScaler.GetOriginalFont(_labelLoading), desiredScalePercent,
				new Size(ClientSize.Width - _labelLoading.Left - kLabelBoundsMargin,
					_unscaledBottomLabelsTop - _labelLoading.Top - kLabelBoundsMargin));

			var bottomLabelMaxBounds = new Size(
				pictureBox2.Left - label1.Left - kLabelBoundsMargin, int.MaxValue / 2);

			label1.Font = FontFitting.GetLargestFontThatFits(label1.Text,
				UITextScaler.GetOriginalFont(label1), desiredScalePercent, bottomLabelMaxBounds);

			_labelVersionInfo.Font = FontFitting.GetLargestFontThatFits(_labelVersionInfo.Text,
				UITextScaler.GetOriginalFont(_labelVersionInfo), desiredScalePercent, bottomLabelMaxBounds);

			var neededHeight = PositionBottomLabelsAndGetRequiredClientHeight(label1,
				_labelVersionInfo, _unscaledBottomLabelsTop, _unscaledClientHeight, kLabelBoundsMargin);

			if (neededHeight != ClientSize.Height)
				ClientSize = new Size(ClientSize.Width, neededHeight);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// topLabel and bottomLabel are both Anchor = Bottom|Left, so simply growing
		/// ClientSize.Height shifts both down together by the same amount without changing the
		/// gap between them -- so this instead stacks bottomLabel directly below topLabel
		/// (pinning topLabel back at its unscaled position first) and returns the client
		/// height needed to fit both, never smaller than unscaledClientHeight.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static int PositionBottomLabelsAndGetRequiredClientHeight(Label topLabel,
			Label bottomLabel, int unscaledTopLabelTop, int unscaledClientHeight, int margin)
		{
			topLabel.Top = unscaledTopLabelTop;
			bottomLabel.Top = topLabel.Bottom + margin;

			return Math.Max(unscaledClientHeight, bottomLabel.Bottom + margin);
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
			rc = new Rectangle(new Point(30, 0), ResourceImageCache.LargeSayMoreLogo.Size);
			e.Graphics.DrawImage(ResourceImageCache.LargeSayMoreLogo, rc);

			// Draw logo text.
			rc = new Rectangle(new Point(_logoTextLeft, kLogoTextImageTop), ResourceImageCache.SayMoreText.Size);
			//rc = new Rectangle(new Point(_logoTextLeft, 18), ResourceImageCache.SayMoreText.Size);
			e.Graphics.DrawImage(ResourceImageCache.SayMoreText, rc);

			// Draw border around window.
			rc = new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
			using (var pen = new Pen(AppColors.BarBorder))
				e.Graphics.DrawRectangle(pen, rc);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleBackgroundPanelPaint(object sender, PaintEventArgs e)
		{
			var clr1 = m_panel.BackColor;
			var clr2 = ColorHelper.CalculateColor(Color.White, Color.DarkGray, 150);
			var rc = m_panel.ClientRectangle;
			using (LinearGradientBrush br = new LinearGradientBrush(rc, clr1, clr2, 45))
				e.Graphics.FillRectangle(br, rc);

			const int dypLineThickness = 2;
			int nTopOfGrayLine = lblCopyright.Bottom +
				(lblMessage.Top - lblCopyright.Bottom) / 2 -
				dypLineThickness;

			int x1 = 16;
			int x2 = m_panel.ClientSize.Width - (x1 + 1);

			using (Pen pen = new Pen(Color.FromArgb(128, 128, 128)))
			{
				e.Graphics.DrawLine(pen, x1, nTopOfGrayLine, x2, nTopOfGrayLine);
				e.Graphics.DrawLine(pen, x1, nTopOfGrayLine + 1, x2, nTopOfGrayLine + 1);
				pen.Color = Color.FromArgb(192, 192, 192);
				e.Graphics.DrawLine(pen, x1, nTopOfGrayLine + 2, x2, nTopOfGrayLine + 2);
				e.Graphics.DrawLine(pen, x1, nTopOfGrayLine + 3, x2, nTopOfGrayLine + 3);
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Timer event to increase the opacity of the splash screen over time. Since this
		/// event occurs in a different thread from the one in which the form exists, we
		/// cannot set the form's opacity property in this thread because it will generate
		/// a cross threading error. Calling the invoke method will invoke the method on
		/// the same thread in which the form was created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void OnUpdateOpacity(object sender, EventArgs e)
		{
			if (m_timer == null)
				return;

			// This callback might get called multiple times before the Invoke is finished,
			// which causes some problems. We just ignore any callbacks we get while we are
			// processing one, so we are using TryEnter/Exit(this) instead of lock(this).
			// We sync on "this" so that we're using the same flag as the SplashScreen class.
			if (Monitor.TryEnter(this))
			{
				try
				{
					// In some rare cases the splash screen is already disposed and the
					// timer is still running. It happened to me (EberhardB) when I stopped
					// debugging while starting up, but it might happen at other times too
					// - so just be safe.
					if (!IsDisposed && IsHandleCreated)
						Invoke(new UpdateOpacityDelegate(UpdateOpacity));
				}
				catch (Exception ex)
				{
					// just ignore any exceptions
					Debug.WriteLine("Got exception in OnUpdateOpacity: " + ex.Message);
				}
				finally
				{
					Monitor.Exit(this);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void UpdateOpacity()
		{
			try
			{
				double currentOpacity = Opacity;

				if (currentOpacity == 0.0)
					Refresh();

				if (currentOpacity < 1.0)
					Opacity = currentOpacity + 0.05;
				else if (m_timer != null)
				{
					m_timer.Stop();
					m_timer.Dispose();
					m_timer = null;
				}
			}
			catch
			{
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual void MakeFullyOpaque()
		{
			try
			{
				Refresh();
				Opacity = 1.0;
				Thread.Sleep(1200);
			}
			catch { }
		}
	}

	#endregion
}
