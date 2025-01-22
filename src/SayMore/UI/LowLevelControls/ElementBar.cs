using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SayMore.Utilities;

namespace SayMore.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Implements a tool strip control used for making pretty blue gradient tool bars in
	/// the application.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ElementBar : ToolStrip
	{
		private ToolStripRenderer _prevRenderer;
		private Color _clrBegin = AppColors.BarBegin;
		private Color _clrEnd = AppColors.BarEnd;

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the gradient angle.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float GradientAngle { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the lighter color of the gradient bar color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Color BackColorBegin
		{
			get { return _clrBegin; }
			set { _clrBegin = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the darker color of the gradient bar color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Color BackColorEnd
		{
			get { return _clrEnd; }
			set { _clrEnd = value; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.ToolStrip.RendererChanged"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnRendererChanged(EventArgs e)
		{
			if (_prevRenderer != null)
				_prevRenderer.RenderToolStripBorder -= OverrideElementBarBorderPainting;

			base.OnRendererChanged(e);

			Renderer.RenderToolStripBorder += OverrideElementBarBorderPainting;
			_prevRenderer = Renderer;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint the background of the toolbar our own way.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			PaintElementBarBackground(e.Graphics, ClientRectangle);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint over the default toolstrip border with the same gradient used to paint the
		/// background of the rest of the main toolstrip.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OverrideElementBarBorderPainting(object sender, ToolStripRenderEventArgs e)
		{
			if (Dock == DockStyle.Top || Dock == DockStyle.Left)
			{
				// Paint over a couple of pixels at the left edge.
				var rc = e.ToolStrip.ClientRectangle;
				rc.Y = rc.Bottom - 3;
				rc.Height = 3;
				PaintElementBarBackground(e.Graphics, e.ToolStrip.ClientRectangle, rc);
			}

			if (Dock == DockStyle.Top)
			{
				// Paint over a couple of pixels at the left edge.
				var rc = e.ToolStrip.ClientRectangle;
				rc.Width = 2;
				PaintElementBarBackground(e.Graphics, e.ToolStrip.ClientRectangle, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint in the specified rectangle the gradient blue of a bar background.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PaintElementBarBackground(Graphics g, Rectangle rc)
		{
			PaintElementBarBackground(g, rc, rc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint in the specified rectangle the gradient blue of a bar background.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="rcGradient">The rectangle used for calculating the gradient (i.e.
		/// the rectangle used for constructing the LinearGradientBrush).</param>
		/// <param name="rcFill">The rectangle that's filled using the FillRectangle method.
		/// </param>
		/// ------------------------------------------------------------------------------------
		private void PaintElementBarBackground(Graphics g, Rectangle rcGradient, Rectangle rcFill)
		{
			using (var br = new LinearGradientBrush(rcGradient, BackColorBegin, BackColorEnd, GradientAngle))
				g.FillRectangle(br, rcFill);
		}
	}
}
