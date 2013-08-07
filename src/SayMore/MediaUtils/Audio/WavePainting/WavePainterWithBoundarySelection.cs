using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using NAudio.Wave;
using Palaso.UI.WindowsForms;

namespace SayMore.Media.Audio
{
	public class WavePainterWithBoundarySelection : WavePainterBasic
	{
		private const int kHighlightHalfWidth = 15;

		private TimeSpan _boundaryMouseOver;

		public TimeSpan SelectedBoundaryTime { get; private set; }
		public virtual Color BoundaryHighlightColor { get; set; }
		public virtual bool HighlightBoundaryWhenMouseIsNear { get; set; }
		private Func<TimeSpan, bool> CanBoundaryBeMoved;

		/// ------------------------------------------------------------------------------------
		public WavePainterWithBoundarySelection(WaveControlWithMovableBoundaries ctrl,
			WaveFileReader stream) :
			base(ctrl, stream)
		{
			Initialize(ctrl);
		}

		/// ------------------------------------------------------------------------------------
		public WavePainterWithBoundarySelection(WaveControlWithMovableBoundaries ctrl,
			IEnumerable<float> samples, TimeSpan totalTime) :
			base(ctrl, samples, totalTime)
		{
			Initialize(ctrl);
		}

		/// ------------------------------------------------------------------------------------
		private void Initialize(WaveControlWithMovableBoundaries ctrl)
		{
			BoundaryHighlightColor = Color.FromArgb(100, Color.DarkSlateBlue);
			HighlightBoundaryWhenMouseIsNear = true;
			CanBoundaryBeMoved = b => ctrl.CanBoundaryBeMoved(b, false);
		}

		/// ------------------------------------------------------------------------------------
		private Color ImmovableBoundaryColor
		{
			// ENHANCE: Get the inverse of the normal border color, rather than hard-coding blue.
			// See http://stackoverflow.com/questions/1165107/how-do-i-invert-a-colour-color-c-net
			get { return Color.Blue; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void CreateBorderPens()
		{
			base.CreateBorderPens();
			_solidBorderPens.Add(new Pen(ImmovableBoundaryColor));
			_translucentBorderPens.Add(new Pen(Color.FromArgb(60, ImmovableBoundaryColor)));
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
		protected override void DrawBoundary(Graphics g, int height, TimeSpan boundary, int penSelector)
		{
			if (_movedBoundarysOrigTime > TimeSpan.Zero && _movedBoundarysOrigTime == boundary)
			{
				var x = ConvertTimeToXCoordinate(boundary);

				_solidBorderPens[penSelector].DashStyle = DashStyle.Dot;
				g.DrawLine(_solidBorderPens[penSelector], x, 0, x, height);
				_solidBorderPens[penSelector].DashStyle = DashStyle.Solid;
				return;
			}

			if (CanBoundaryBeMoved != null && !CanBoundaryBeMoved(boundary))
				penSelector = 1;

			base.DrawBoundary(g, height, boundary, penSelector);
		}

		/// ------------------------------------------------------------------------------------
		public void HighlightBoundaryMouseOver(TimeSpan boundary)
		{
			if (_boundaryMouseOver == boundary)
				return;

			InvalidateBoundary(_boundaryMouseOver, kHighlightHalfWidth);
			if (boundary > TimeSpan.Zero && CanBoundaryBeMoved(boundary))
			{
				_boundaryMouseOver = boundary;
				InvalidateBoundary(_boundaryMouseOver, kHighlightHalfWidth);
			}
			else
				_boundaryMouseOver = TimeSpan.Zero;
		}

		/// ------------------------------------------------------------------------------------
		public override void Draw(PaintEventArgs e, Rectangle rc)
		{
			base.Draw(e, rc);
			if (SelectedBoundaryTime > TimeSpan.Zero)
			{
				var penSelector = (CanBoundaryBeMoved == null || CanBoundaryBeMoved(SelectedBoundaryTime)) ? 0 : 1;
				DrawBoundarySelectedAtX(e.Graphics, ConvertTimeToXCoordinate(SelectedBoundaryTime),
					penSelector);
			}
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
		private void DrawBoundarySelectedAtX(Graphics g, int dx, int penSelector = 0)
		{
			if (dx == 0)
				return;

			var pen = _solidBorderPens[penSelector];
			var baseColor = pen.Color;

			int clientHeight = (Control == null ? 0 : Control.ClientSize.Height);
			var fillColor = ColorHelper.CalculateColor(Color.White, baseColor, 200);

			int regionHeight = Math.Min(30, clientHeight / 6);
			int arrowHeight = kBoundaryHotZoneHalfWidth;

			var path = new GraphicsPath();
			path.AddLine(dx - kBoundaryHotZoneHalfWidth, 0, dx - kBoundaryHotZoneHalfWidth, regionHeight - arrowHeight);
			path.AddLine(dx - kBoundaryHotZoneHalfWidth, regionHeight - arrowHeight, dx, regionHeight);
			path.AddLine(dx, regionHeight, dx + kBoundaryHotZoneHalfWidth, regionHeight - arrowHeight);
			path.AddLine(dx + kBoundaryHotZoneHalfWidth, regionHeight - arrowHeight, dx + kBoundaryHotZoneHalfWidth, 0);

			using (var br = new LinearGradientBrush(path.GetBounds(), fillColor, baseColor, 90f))
			{
				g.FillRegion(br, new Region(path));
				g.DrawPath(pen, path);
			}

			path.Reset();

			path.AddLine(dx - kBoundaryHotZoneHalfWidth, clientHeight, dx - kBoundaryHotZoneHalfWidth, (clientHeight - 1) - (regionHeight - arrowHeight));
			path.AddLine(dx - kBoundaryHotZoneHalfWidth, (clientHeight - 1) - (regionHeight - arrowHeight), dx, (clientHeight - 1) - regionHeight);
			path.AddLine(dx, (clientHeight - 1) - regionHeight, dx + kBoundaryHotZoneHalfWidth, (clientHeight - 1) - (regionHeight - arrowHeight));
			path.AddLine(dx + kBoundaryHotZoneHalfWidth, (clientHeight - 1) - (regionHeight - arrowHeight), dx + kBoundaryHotZoneHalfWidth, clientHeight);

			using (var br = new LinearGradientBrush(path.GetBounds(), baseColor, fillColor, 90f))
			{
				g.FillRegion(br, new Region(path));
				g.DrawPath(pen, path);
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
