// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: SpongeBar.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Sponge2.UI.Utilities;

namespace Sponge2.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Implements a tool strip control used for making pretty blue gradient tool bars in
	/// the application.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SpongeBar : ToolStrip
	{
		private ToolStripRenderer _prevRenderer;
		private Color _clrBegin = SpongeColors.BarBegin;
		private Color _clrEnd = SpongeColors.BarEnd;

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the gradient angle.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float GradientAngle { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the lighter color of the gradient sponge bar color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color BackColorBegin
		{
			get { return _clrBegin; }
			set { _clrBegin = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the darker color of the gradient sponge bar color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
				_prevRenderer.RenderToolStripBorder -= OverrideSpongeBarBorderPainting;

			base.OnRendererChanged(e);

			Renderer.RenderToolStripBorder += OverrideSpongeBarBorderPainting;
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
			PaintSpongeBarBackground(e.Graphics, ClientRectangle);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint over the default toolstrip border with the same gradient used to paint the
		/// background of the rest of the main toolstrip.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OverrideSpongeBarBorderPainting(object sender, ToolStripRenderEventArgs e)
		{
			if (Dock == DockStyle.Top || Dock == DockStyle.Left)
			{
				// Paint over a couple of pixels at the left edge.
				var rc = e.ToolStrip.ClientRectangle;
				rc.Y = rc.Bottom - 3;
				rc.Height = 3;
				PaintSpongeBarBackground(e.Graphics, e.ToolStrip.ClientRectangle, rc);
			}

			if (Dock == DockStyle.Top)
			{
				// Paint over a couple of pixels at the left edge.
				var rc = e.ToolStrip.ClientRectangle;
				rc.Width = 2;
				PaintSpongeBarBackground(e.Graphics, e.ToolStrip.ClientRectangle, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint in the specified rectangle the gradient blue of a sponge bar background.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PaintSpongeBarBackground(Graphics g, Rectangle rc)
		{
			PaintSpongeBarBackground(g, rc, rc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint in the specified rectangle the gradient blue of a sponge bar background.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="rcGradient">The rectangle used for calculating the gradient (i.e.
		/// the rectangle used for constructing the LinearGradientBrush).</param>
		/// <param name="rcFill">The rectangle that's filled using the FillRectangle method.
		/// </param>
		/// ------------------------------------------------------------------------------------
		private void PaintSpongeBarBackground(Graphics g, Rectangle rcGradient, Rectangle rcFill)
		{
			using (var br = new LinearGradientBrush(rcGradient, BackColorBegin, BackColorEnd, GradientAngle))
				g.FillRectangle(br, rcFill);
		}
	}
}
