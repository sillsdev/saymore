using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SayMore.AudioUtils
{
	public class WavePainterBasic
	{
		public const int kBoundaryHotZoneHalfWidth = 4;

		/// <summary>
		/// Each pixel value (X direction) represents this many samples in the wavefile
		/// Starting value is based on control width so that the .WAV will cover the entire width.
		/// </summary>
		public virtual double SamplesPerPixel { get; private set; }
		public virtual TimeSpan CursorTime { get; private set; }

		protected IEnumerable<TimeSpan> _segmentBoundaries;
		protected double _pixelPerMillisecond;
		protected int _offsetOfLeftEdge;
		protected readonly TimeSpan _totalTime;
		protected readonly float[] _samples = new float[0];
		protected KeyValuePair<float, float>[] _samplesToDraw = new KeyValuePair<float, float>[0];

		protected TimeSpan _movedBoundary;
		protected TimeSpan _movedBoundarysOrigTime;

		public virtual Control Control { get; set; }
		public virtual Color ForeColor { get; set; }
		public virtual Color BackColor { get; set; }
		public virtual Color BoundaryColor { get; set; }
		public virtual Color CursorColor { get; set; }

		/// ------------------------------------------------------------------------------------
		public WavePainterBasic(IEnumerable<float> samples, TimeSpan totalTime)
			: this(null, samples, totalTime)
		{
		}

		/// ------------------------------------------------------------------------------------
		public WavePainterBasic(Control ctrl, IEnumerable<float> samples, TimeSpan totalTime)
		{
			//BoundaryColor = Color.MidnightBlue;
			BoundaryColor = Color.FromArgb(220, 121, 0);
			CursorColor = Color.Green;
			BackColor = Color.CornflowerBlue;
			ForeColor = Color.LightGray;

			_segmentBoundaries = new TimeSpan[0];

			Control = ctrl;
			ctrl.Paint += (s, e) => Draw(e, ctrl.ClientRectangle);

			_samples = samples.ToArray();
			_totalTime = totalTime;
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<TimeSpan> SegmentBoundaries
		{
			get { return _segmentBoundaries; }
			set
			{
				_segmentBoundaries = (value ?? new TimeSpan[0]);
				if (Control != null)
					Control.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetVirtualWidth(int width)
		{
			_pixelPerMillisecond = width / _totalTime.TotalMilliseconds;
			SetSamplesPerPixel(_samples.Length / (double)width);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the offset into the sample buffer that represents the left edge of the
		/// painting surface. This is often the absolute value of the X coordinate of the
		/// control's AutoScrollPosition.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void SetOffsetOfLeftEdge(int offset)
		{
			_offsetOfLeftEdge = offset;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void SetSamplesPerPixel(double value)
		{
			SamplesPerPixel = value;
			LoadBufferOfSamplesToDraw();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void LoadBufferOfSamplesToDraw()
		{
			var samplesToDraw = new List<KeyValuePair<float, float>>();
			int firstSampleInPixel = 0;

			while (firstSampleInPixel < _samples.Length)
			{
				var maxVal = float.MinValue;
				var minVal = float.MaxValue;

				// Find the max and min peaks for this pixel
				for (int i = 0; i < SamplesPerPixel && firstSampleInPixel + i < _samples.Length; i++)
				{
					maxVal = Math.Max(maxVal, _samples[firstSampleInPixel + i]);
					minVal = Math.Min(minVal, _samples[firstSampleInPixel + i]);
				}

				samplesToDraw.Add(new KeyValuePair<float, float>(maxVal, minVal));
				firstSampleInPixel += (int)SamplesPerPixel;
			}

			_samplesToDraw = samplesToDraw.ToArray();
		}

		/// ------------------------------------------------------------------------------------
		public virtual int GetBoundaryNearX(int x)
		{
			int i = 0;
			foreach (var boundaryX in SegmentBoundaries.Select(b => ConvertTimeToXCoordinate(b)))
			{
				if (x >= boundaryX - kBoundaryHotZoneHalfWidth && x <= boundaryX + kBoundaryHotZoneHalfWidth)
					return i;

				i++;
			}

			return -1;
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetCursor(int cursorX)
		{
			SetCursor(ConvertXCoordinateToTime(cursorX));
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetCursor(TimeSpan cursorTime)
		{
			InvalidateBoundary(CursorTime, 0);
			CursorTime = cursorTime;
			InvalidateBoundary(CursorTime, 0);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetMovingAnchorTime(TimeSpan movingAnchorTime)
		{
			InvalidateBoundary(_movedBoundarysOrigTime, 1);
			_movedBoundarysOrigTime = movingAnchorTime;
			InvalidateBoundary(_movedBoundarysOrigTime, 1);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetMovedBoundaryTime(TimeSpan newBoundary)
		{
			InvalidateBoundary(_movedBoundary, 1);
			_movedBoundary = newBoundary;
			InvalidateBoundary(_movedBoundary, 1);
		}

		/// ------------------------------------------------------------------------------------
		public virtual int ConvertTimeToXCoordinate(TimeSpan time)
		{
			return (int)(time.TotalMilliseconds * _pixelPerMillisecond) - _offsetOfLeftEdge;
		}

		/// ------------------------------------------------------------------------------------
		public virtual TimeSpan ConvertXCoordinateToTime(int x)
		{
			return TimeSpan.FromMilliseconds((x + _offsetOfLeftEdge) / _pixelPerMillisecond);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void InvalidateBoundary(TimeSpan boundary)
		{
			InvalidateBoundary(boundary, kBoundaryHotZoneHalfWidth);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void InvalidateBoundary(TimeSpan boundary, int pixelsOnEitherSide)
		{
			if (boundary == TimeSpan.Zero || Control == null)
				return;

			var dx = ConvertTimeToXCoordinate(boundary);
			Control.Invalidate(new Rectangle(dx - pixelsOnEitherSide, 0,
				pixelsOnEitherSide * 2 + 1, Control.ClientSize.Height));
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Draw(PaintEventArgs e, Rectangle rc)
		{
			if (_samplesToDraw.Length == 0 && _samples.Length > 0)
				SetVirtualWidth(rc.Width);

			DrawWave(e.Graphics, rc);
			DrawSegmentBoundaries(e.Graphics, rc.Height);
			DrawCursor(e.Graphics, rc);

			if (_movedBoundary > TimeSpan.Zero)
				DrawMovedBoundary(e.Graphics, rc.Height, ConvertTimeToXCoordinate(_movedBoundary));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void DrawWave(Graphics g, Rectangle rc)
		{
			// Draw the X axis through middle of the graph area.
			using (var pen = new Pen(ForeColor))
			{
				int dy = (int)Math.Round(rc.Height / 2d, MidpointRounding.AwayFromZero);
				g.DrawLine(pen, 0, dy, rc.Width, dy);
			}

			// If samples per pixel is small or less than zero,
			// we are out of zoom range, so don't display anything
			if (SamplesPerPixel <= 0.0000000001d)
				return;

			// Samples will span from some value between -1 and +1, inclusively.
			// The top of the client area represents +1 and the bottom represents -1.

			var blend = new Blend();
			blend.Positions = new[] { 0f, 0.15f, 0.5f, 0.85f, 1.0f };
			blend.Factors = new[] { 0.65f, 0.85f, 1.0f, 0.85f, 0.65f };

			int verticalMidPoint = rc.Y + (int)Math.Round(rc.Height / 2d, MidpointRounding.AwayFromZero);

			var clipRect = g.VisibleClipBounds;

			for (int x = (int)clipRect.X; x < clipRect.X + clipRect.Width && x + _offsetOfLeftEdge < _samplesToDraw.Length; x++)
			{
				var sampleAmplitudes = _samplesToDraw[x + _offsetOfLeftEdge];

				if (sampleAmplitudes.Key.Equals(0f) && sampleAmplitudes.Value.Equals(0f))
					continue;

				int y1 = verticalMidPoint -
					(int)Math.Round(rc.Height * (sampleAmplitudes.Key / 2f), MidpointRounding.AwayFromZero);

				int y2 = verticalMidPoint -
					(int)Math.Round(rc.Height * (sampleAmplitudes.Value / 2f), MidpointRounding.AwayFromZero);

				if (y2 - y1 <= 1)
					continue;

				var pt1 = new Point(x, y1);
				var pt2 = new Point(x, y2);

				using (var br = new LinearGradientBrush(pt1, pt2, BackColor, ForeColor))
				{
					br.Blend = blend;
					using (var pen = new Pen(br))
						g.DrawLine(pen, pt1, pt2);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual Rectangle GetRectangleForTimeRange(TimeSpan startTime, TimeSpan endTime)
		{
			var endX = ConvertTimeToXCoordinate(endTime);
			var startX = ConvertTimeToXCoordinate(startTime);

			var x1 = Math.Min(startX, endX);
			var x2 = Math.Max(startX, endX);

			if (x1 >= x2)
				return Rectangle.Empty;

			return new Rectangle(x1, 0, x2 - x1 + 1,
				(Control == null ? 0 : Control.ClientRectangle.Height));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void DrawSegmentBoundaries(Graphics g, int clientHeight)
		{
			if (_segmentBoundaries == null)
				return;

			using (var solidPen = new Pen(BoundaryColor))
			using (var translucentPen = new Pen(Color.FromArgb(60, BoundaryColor)))
			{
				foreach (var boundary in _segmentBoundaries)
				{
					DrawBoundary(g, clientHeight, translucentPen, solidPen,
						ConvertTimeToXCoordinate(boundary), boundary);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void DrawBoundary(Graphics g, int clientHeight, Pen edgePen,
			Pen centerPen, int dx, TimeSpan boundary)
		{
			if (_movedBoundarysOrigTime > TimeSpan.Zero && _movedBoundarysOrigTime == boundary)
			{
				centerPen.DashStyle = DashStyle.Dot;
				g.DrawLine(centerPen, dx, 0, dx, clientHeight);
				centerPen.DashStyle = DashStyle.Solid;
			}
			else
			{
				g.DrawLine(edgePen, dx - 1, 0, dx - 1, clientHeight);
				g.DrawLine(centerPen, dx, 0, dx, clientHeight);
				g.DrawLine(edgePen, dx + 1, 0, dx + 1, clientHeight);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void DrawCursor(Graphics g, Rectangle rc)
		{
			var dx = ConvertTimeToXCoordinate(CursorTime);

			if (dx > 0)
			{
				using (var pen = new Pen(CursorColor))
					g.DrawLine(pen, dx, 0, dx, rc.Height);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void DrawMovedBoundary(Graphics g, int clientHeight, int dx)
		{
			using (var pen = new Pen(BoundaryColor))
				g.DrawLine(pen, dx, 0, dx, clientHeight);
		}
	}
}