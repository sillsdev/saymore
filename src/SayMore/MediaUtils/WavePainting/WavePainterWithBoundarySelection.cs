using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using NAudio.Wave;
using SilTools;

namespace SayMore.Media
{
	public class WavePainterWithBoundarySelection : WavePainterBasic
	{
		private const int kHighlightHalfWidth = 15;

		private TimeSpan _boundaryMouseOver;

		public TimeSpan SelectedBoundaryTime { get; private set; }
		public virtual Color BoundaryHighlightColor { get; set; }
		public virtual bool HighlightBoundaryWhenMouseIsNear { get; set; }

		/// ------------------------------------------------------------------------------------
		public WavePainterWithBoundarySelection(Control ctrl, WaveFileReader stream)
			: base(ctrl, stream)
		{
			BoundaryHighlightColor = Color.FromArgb(100, Color.DarkSlateBlue);
			HighlightBoundaryWhenMouseIsNear = true;
		}

		/// ------------------------------------------------------------------------------------
		public WavePainterWithBoundarySelection(Control ctrl, IEnumerable<float> samples, TimeSpan totalTime) :
			base(ctrl, samples, totalTime)
		{
			BoundaryHighlightColor = Color.FromArgb(100, Color.DarkSlateBlue);
			HighlightBoundaryWhenMouseIsNear = true;
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectedBoundary(TimeSpan selTime)
		{
			InvalidateBoundary(SelectedBoundaryTime, kHighlightHalfWidth);
			System.Diagnostics.Debug.WriteLine("SelectedBoundaryTime changing from " + SelectedBoundaryTime + " to " + selTime);
			SelectedBoundaryTime = selTime;
			InvalidateBoundary(SelectedBoundaryTime, kHighlightHalfWidth);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetMovedBoundaryTime(TimeSpan newBoundary)
		{
			InvalidateBoundary(_movedBoundary, kBoundaryHotZoneHalfWidth);
			_movedBoundary = newBoundary;
			InvalidateBoundary(_movedBoundary, kBoundaryHotZoneHalfWidth);
		}

		/// ------------------------------------------------------------------------------------
		public void HighlightBoundaryMouseOver(TimeSpan boundary)
		{
			if (_boundaryMouseOver == boundary)
				return;

			InvalidateBoundary(_boundaryMouseOver, kHighlightHalfWidth);
			_boundaryMouseOver = boundary;
			InvalidateBoundary(_boundaryMouseOver, kHighlightHalfWidth);
		}

		/// ------------------------------------------------------------------------------------
		public override void Draw(PaintEventArgs e, Rectangle rc)
		{
			base.Draw(e, rc);
			DrawBoundarySelectedAtX(e.Graphics, ConvertTimeToXCoordinate(SelectedBoundaryTime));
			DrawHighlightedBoundaryMouseIsOver(e.Graphics, rc.Height);
			DrawCursor(e.Graphics, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawHighlightedBoundaryMouseIsOver(Graphics g, int clientHeight)
		{
			if (_boundaryMouseOver == TimeSpan.Zero || _segmentBoundaries == null ||
				SelectedBoundaryTime == _boundaryMouseOver || !HighlightBoundaryWhenMouseIsNear)
			{
				return;
			}

			int dx = ConvertTimeToXCoordinate(_boundaryMouseOver);

			var rcHighlight = new Rectangle(dx - kHighlightHalfWidth, 0, kHighlightHalfWidth * 2 + 1, clientHeight);
			using (var br = new LinearGradientBrush(rcHighlight, Color.Transparent, BoundaryHighlightColor, 0f))
			{
				var blend = new Blend();
				blend.Positions = new[] { 0f, 0.5f, 1.0f };
				blend.Factors = new[] { 0.0f, 1.0f, 0.0f };
				br.Blend = blend;
				g.FillRectangle(br, rcHighlight);
			}

			//g.DrawLine(Pens.DarkSlateBlue, rcHighlight.Left, 0, rcHighlight.Left, clientHeight);
			//g.DrawLine(Pens.DarkSlateBlue, rcHighlight.Right, 0, rcHighlight.Right, clientHeight);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawBoundarySelectedAtX(Graphics g, int dx)
		{
			if (dx == 0)
				return;

			int clientHeight = (Control == null ? 0 : Control.ClientSize.Height);
			var fillColor = ColorHelper.CalculateColor(Color.White, BoundaryColor, 200);

			using (var pen = new Pen(BoundaryColor))
			{
				int regionHeight = Math.Min(30, clientHeight / 6);
				int arrowHeight = kBoundaryHotZoneHalfWidth;

				var path = new GraphicsPath();
				path.AddLine(dx - kBoundaryHotZoneHalfWidth, 0, dx - kBoundaryHotZoneHalfWidth, regionHeight - arrowHeight);
				path.AddLine(dx - kBoundaryHotZoneHalfWidth, regionHeight - arrowHeight, dx, regionHeight);
				path.AddLine(dx, regionHeight, dx + kBoundaryHotZoneHalfWidth, regionHeight - arrowHeight);
				path.AddLine(dx + kBoundaryHotZoneHalfWidth, regionHeight - arrowHeight, dx + kBoundaryHotZoneHalfWidth, 0);

				using (var br = new LinearGradientBrush(path.GetBounds(), fillColor, BoundaryColor, 90f))
				{
					g.FillRegion(br, new Region(path));
					g.DrawPath(pen, path);
				}

				path.Reset();

				path.AddLine(dx - kBoundaryHotZoneHalfWidth, clientHeight, dx - kBoundaryHotZoneHalfWidth, (clientHeight - 1) - (regionHeight - arrowHeight));
				path.AddLine(dx - kBoundaryHotZoneHalfWidth, (clientHeight - 1) - (regionHeight - arrowHeight), dx, (clientHeight - 1) - regionHeight);
				path.AddLine(dx, (clientHeight - 1) - regionHeight, dx + kBoundaryHotZoneHalfWidth, (clientHeight - 1) - (regionHeight - arrowHeight));
				path.AddLine(dx + kBoundaryHotZoneHalfWidth, (clientHeight - 1) - (regionHeight - arrowHeight), dx + kBoundaryHotZoneHalfWidth, clientHeight);

				using (var br = new LinearGradientBrush(path.GetBounds(), BoundaryColor, fillColor, 90f))
				{
					g.FillRegion(br, new Region(path));
					g.DrawPath(pen, path);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void DrawMovedBoundary(Graphics g, int clientHeight, int dx)
		{
			base.DrawMovedBoundary(g, clientHeight, dx);
			DrawBoundarySelectedAtX(g, dx);
		}
	}
}
