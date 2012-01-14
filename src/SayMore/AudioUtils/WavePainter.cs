using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SayMore.AudioUtils
{
	public class WavePainter
	{
		///// <summary>
		///// This value is the amount to increase/decrease the m_SamplesPerPixel. This creates a 'Zoom' effect.
		///// Starting value is _samplesPerPixel / 25 so that it is scaled for the size of the .WAV
		///// </summary>
		//private double _zoomFactor;

		//public const int kMarkerHalfWidth = 4;

		/// <summary>
		/// Each pixel value (X direction) represents this many samples in the wavefile
		/// Starting value is based on control width so that the .WAV will cover the entire width.
		/// </summary>
		public double SamplesPerPixel { get; private set; }

		private IEnumerable<TimeSpan> _segmentBoundaries;
		private TimeSpan _achoredSegment;
		//private TimeSpan _boundaryMouseOver;
		//private Rectangle _highlightedBoundaryRectMouseOver;
		private double _pixelPerMillisecond;
		private int _offsetOfLeftEdge;
		private readonly TimeSpan _totalTime;
		private readonly float[] _samples = new float[0];
		private KeyValuePair<float, float>[] _samplesToDraw = new KeyValuePair<float, float>[0];

		public Control Control { get; set; }
		public Color ForeColor { get; set; }
		public Color BackColor { get; set; }

		public Rectangle PreviousCursorRectangle { get; private set; }
		public Rectangle PreviousPlaybackCursorRectangle { get; private set; }
		public Rectangle PreviousSelectedRegionRectangle { get; private set; }

		public TimeSpan CursorTime { get; private set; }
		public TimeSpan PlaybackCursorTime { get; private set; }
		public TimeSpan SelectedRegionStartTime { get; private set; }
		public TimeSpan SelectedRegionEndTime { get; private set; }

		/// ------------------------------------------------------------------------------------
		public WavePainter(IEnumerable<float> samples, TimeSpan totalTime) : this(null, samples, totalTime)
		{
		}

		/// ------------------------------------------------------------------------------------
		public WavePainter(Control ctrl, IEnumerable<float> samples, TimeSpan totalTime)
		{
			_segmentBoundaries = new TimeSpan[0];

			Control = ctrl;
			ctrl.Paint += (s, e) => Draw(e, ctrl.ClientRectangle);

			_samples = samples.ToArray();
			_totalTime = totalTime;

			BackColor = Color.CornflowerBlue;
			ForeColor = Color.LightGray;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<TimeSpan> SegmentBoundaries
		{
			get { return _segmentBoundaries; }
			set
			{
				_segmentBoundaries = (value ?? new TimeSpan[0]);
				if (Control != null)
					Control.Invalidate();
			}
		}

		///// ------------------------------------------------------------------------------------
		//public void HighlightBoundaryMouseOver(TimeSpan boundary)
		//{
		//    if (_boundaryMouseOver == boundary)
		//        return;

		//    _boundaryMouseOver = boundary;

		//    if (_highlightedBoundaryRectMouseOver != Rectangle.Empty)
		//        Control.Invalidate(_highlightedBoundaryRectMouseOver);

		//    if (Control == null || boundary == TimeSpan.Zero)
		//        return;

		//    int dx = ConvertTimeToXCoordinate(_boundaryMouseOver);
		//    _highlightedBoundaryRectMouseOver = new Rectangle(dx - kMarkerHalfWidth, 0,
		//        kMarkerHalfWidth * 2 + 1, Control.ClientSize.Height);

		//    Control.Invalidate(_highlightedBoundaryRectMouseOver);
		//}

		/// ------------------------------------------------------------------------------------
		public void SetVirtualWidth(int width)
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
		public void SetOffsetOfLeftEdge(int offset)
		{
			_offsetOfLeftEdge = offset;
		}

		/// ------------------------------------------------------------------------------------
		private void SetSamplesPerPixel(double value)
		{
			SamplesPerPixel = value;
			LoadBufferOfSamplesToDraw();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadBufferOfSamplesToDraw()
		{
			var samplesToDraw = new List<KeyValuePair<float, float>>();
			int firstSampleInPixel = 0;

			while (firstSampleInPixel < _samples.Length)
			{
				var maxVal = float.MinValue;
				var minVal = float.MaxValue;

				// Find the max and min peaks for this pixel
				for (int i = 0; i < SamplesPerPixel && firstSampleInPixel + i< _samples.Length; i++)
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
		public void Draw(PaintEventArgs e, Rectangle rc)
		{
			if (_samplesToDraw.Length == 0 && _samples.Length > 0)
				SetVirtualWidth(rc.Width);

			// Draw the X axis through middle of the graph area.
			using (var pen = new Pen(ForeColor))
			{
				int dy = (int)Math.Round(rc.Height / 2d, MidpointRounding.AwayFromZero);
				e.Graphics.DrawLine(pen, 0, dy, rc.Width, dy);
			}

			// If samples per pixel is small or less than zero,
			// we are out of zoom range, so don't display anything
			if (SamplesPerPixel > 0.0000000001d)
				DrawWave(e.Graphics, rc);

			if (_segmentBoundaries != null)
				DrawSegmentBoundaries(e.Graphics, rc.Height);

			// Now draw the selected region, if there is one.
			if (_segmentBoundaries == null || SelectedRegionStartTime >= TimeSpan.Zero)
				DrawSelectedRegion(e.Graphics);
			else
			{
				//DrawSelectedBoundary(e.Graphics);
				//DrawHighlightedSegmentBoundaryMouseIsOver(e.Graphics, rc.Height);
			}

			DrawCursor(e.Graphics, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawWave(Graphics g, Rectangle rc)
		{
			// Samples will span from some value <= +1 to >= -1.
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
		private void DrawSelectedRegion(Graphics g)
		{
			var regionRect = GetRectangleForTimeRange(SelectedRegionStartTime, SelectedRegionEndTime);
			if (regionRect == Rectangle.Empty)
				return;

			using (var br = new SolidBrush(Color.FromArgb(110, SystemColors.Highlight)))
				g.FillRectangle(br, regionRect);

			PreviousSelectedRegionRectangle = regionRect;
		}

		///// ------------------------------------------------------------------------------------
		//private void DrawSelectedBoundary(Graphics g)
		//{
		//    var dx = ConvertTimeToXCoordinate(SelectedRegionEndTime);

		//    if (dx == 0)
		//        return;

		//    int clientHeight = (Control == null ? 0 : Control.ClientSize.Height);
		//    //var lineColor = Color.MidnightBlue;
		//    //var fillColor = Color.CornflowerBlue;
		//    var lineColor = Color.FromArgb(220, 121, 0);
		//    var fillColor = Color.Orange;

		//    using (var pen = new Pen(lineColor))
		//    {
		//        g.DrawLine(pen, dx, 0, dx, clientHeight + 1);

		//        int regionHeight = Math.Min(30, clientHeight / 6);
		//        int arrowHeight = kMarkerHalfWidth;

		//        var path = new GraphicsPath();
		//        path.AddLine(dx - kMarkerHalfWidth, 0, dx - kMarkerHalfWidth, regionHeight - arrowHeight);
		//        path.AddLine(dx - kMarkerHalfWidth, regionHeight - arrowHeight, dx, regionHeight);
		//        path.AddLine(dx, regionHeight, dx + kMarkerHalfWidth, regionHeight - arrowHeight);
		//        path.AddLine(dx + kMarkerHalfWidth, regionHeight - arrowHeight, dx + kMarkerHalfWidth, 0);

		//        using (var br = new LinearGradientBrush(path.GetBounds(), fillColor, lineColor, 90f))
		//        {
		//            g.FillRegion(br, new Region(path));
		//            g.DrawPath(pen, path);
		//        }

		//        path.Reset();

		//        path.AddLine(dx - kMarkerHalfWidth, clientHeight, dx - kMarkerHalfWidth, (clientHeight - 1) - (regionHeight - arrowHeight));
		//        path.AddLine(dx - kMarkerHalfWidth, (clientHeight - 1) - (regionHeight - arrowHeight), dx, (clientHeight - 1) - regionHeight);
		//        path.AddLine(dx, (clientHeight - 1) - regionHeight, dx + kMarkerHalfWidth, (clientHeight - 1) - (regionHeight - arrowHeight));
		//        path.AddLine(dx + kMarkerHalfWidth, (clientHeight - 1) - (regionHeight - arrowHeight), dx + kMarkerHalfWidth, clientHeight);

		//        using (var br = new LinearGradientBrush(path.GetBounds(), lineColor, fillColor, 90f))
		//        {
		//            g.FillRegion(br, new Region(path));
		//            g.DrawPath(pen, path);
		//        }
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		public Rectangle GetRectangleForTimeRange(TimeSpan startTime, TimeSpan endTime)
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
		private void DrawSegmentBoundaries(Graphics g, int clientHeight)
		{
			//using (var pen = new Pen(Color.MidnightBlue))
			using (var pen = new Pen(Color.FromArgb(220, 121, 0)))
			{
				foreach (var dx in _segmentBoundaries.Where(b => b != _achoredSegment).Select(b => ConvertTimeToXCoordinate(b)))
					g.DrawLine(pen, dx, 0, dx, clientHeight);
			}

			if (_segmentBoundaries.All(b => b != _achoredSegment))
				return;

			using (var pen = new Pen(ForeColor))
			{
				pen.DashStyle = DashStyle.Dot;
				var dx = ConvertTimeToXCoordinate(_achoredSegment);
				g.DrawLine(pen, dx, 0, dx, clientHeight);
			}
		}

		///// ------------------------------------------------------------------------------------
		//private void DrawHighlightedSegmentBoundaryMouseIsOver(Graphics g, int clientHeight)
		//{
		//    if (_boundaryMouseOver == TimeSpan.Zero || _segmentBoundaries == null ||
		//        SelectedRegionStartTime >= TimeSpan.Zero || SelectedRegionEndTime == _boundaryMouseOver)
		//    {
		//        return;
		//    }

		//    int dx = ConvertTimeToXCoordinate(_boundaryMouseOver);

		//    var rcHighlight = new Rectangle(dx - (kMarkerHalfWidth + 15), 0, kMarkerHalfWidth * 2 + 30, clientHeight);
		//    using (var br = new SolidBrush(Color.FromArgb(100, Color.DarkSlateBlue)))
		//        g.FillRectangle(br, rcHighlight);

		//    //g.DrawLine(Pens.DarkSlateBlue, rcHighlight.Left, 0, rcHighlight.Left, clientHeight);
		//    //g.DrawLine(Pens.DarkSlateBlue, rcHighlight.Right, 0, rcHighlight.Right, clientHeight);
		//}

		/// ------------------------------------------------------------------------------------
		private void DrawCursor(Graphics g, Rectangle rc)
		{
			var dx = ConvertTimeToXCoordinate(CursorTime);

			using (var pen = new Pen(Color.Green))
				g.DrawLine(pen, dx, 0, dx, rc.Height);

			PreviousCursorRectangle = new Rectangle(dx, 0, 1, rc.Height);
		}

		///// ------------------------------------------------------------------------------------
		//private void DrawPlaybackCursor(Graphics g, Rectangle rc)
		//{
		//    var dx = ConvertTimeToXCoordinate(PlaybackCursorTime);

		//    using (var pen = new Pen(Color.FromArgb(255, Color.DarkBlue)))
		//        g.DrawLine(pen, dx, 0, dx, rc.Height);

		//    PreviousPlaybackCursorRectangle = new Rectangle(dx, 0, 1, rc.Height);
		//}

		///// ------------------------------------------------------------------------------------
		//private void DrawPlaybackProgressShading(Graphics g, Rectangle rc)
		//{
		//    rc.X = _cursorX;
		//    rc.Width = _playbackCursorX - _cursorX;

		//    using (var br = new SolidBrush(Color.FromArgb(110, SystemColors.Highlight)))
		//        g.FillRectangle(br, rc);
		//}

		/// ------------------------------------------------------------------------------------
		public void BeginBoundaryMove(TimeSpan boundary)
		{
			_achoredSegment = boundary;
		}

		/// ------------------------------------------------------------------------------------
		public void EndBoundaryMove()
		{
			_achoredSegment = TimeSpan.Zero;
		}

		///// ------------------------------------------------------------------------------------
		//public void SetSelectedBoundary(TimeSpan selTime)
		//{
		//    InvalidatePreviouslySelectedRegion();

		//    SelectedRegionStartTime = TimeSpan.FromSeconds(1).Negate();
		//    SelectedRegionEndTime = selTime;

		//    var dx = ConvertTimeToXCoordinate(selTime);

		//    if (dx == 0)
		//        PreviousSelectedRegionRectangle = Rectangle.Empty;
		//    else
		//    {
		//        PreviousSelectedRegionRectangle = new Rectangle(dx - kMarkerHalfWidth, 0,
		//            kMarkerHalfWidth * 2 + 1, Control.ClientSize.Height);
		//    }

		//    Control.Invalidate(PreviousSelectedRegionRectangle);
		//}

		/// ------------------------------------------------------------------------------------
		public void SetSelectionTimes(TimeSpan selStartTime, TimeSpan selEndTime)
		{
			InvalidatePreviouslySelectedRegion();

			SelectedRegionStartTime = selStartTime;
			SelectedRegionEndTime = selEndTime;

			var endX = ConvertTimeToXCoordinate(selEndTime);
			var startX = ConvertTimeToXCoordinate(selStartTime);

			PreviousSelectedRegionRectangle = new Rectangle(startX, 0,
				endX - startX, Control.ClientSize.Height);

			Control.Invalidate(PreviousSelectedRegionRectangle);
		}

		/// ------------------------------------------------------------------------------------
		private void InvalidatePreviouslySelectedRegion()
		{
			if (PreviousSelectedRegionRectangle != Rectangle.Empty)
				Control.Invalidate(PreviousSelectedRegionRectangle);
		}

		/// ------------------------------------------------------------------------------------
		public void SetCursor(TimeSpan cursorTime)
		{
			CursorTime = cursorTime;
			RepaintAfterCursorIsSet();
		}

		/// ------------------------------------------------------------------------------------
		public void SetCursor(int cursorX)
		{
			CursorTime = ConvertXCoordinateToTime(cursorX);
			RepaintAfterCursorIsSet();
		}

		/// ------------------------------------------------------------------------------------
		private void RepaintAfterCursorIsSet()
		{
			var rc = PreviousCursorRectangle;
			if (rc != Rectangle.Empty)
				Control.Invalidate(rc);

			var dx = ConvertTimeToXCoordinate(CursorTime);
			Control.Invalidate(new Rectangle(dx, 0, dx, Control.ClientSize.Height));
		}

		/// ------------------------------------------------------------------------------------
		public void SetPlaybackCursor(TimeSpan cursorTime)
		{
			PlaybackCursorTime = cursorTime;
			SetPlaybackCursor(ConvertTimeToXCoordinate(cursorTime));
		}

		/// ------------------------------------------------------------------------------------
		public void SetPlaybackCursor(int cursorX)
		{
			var rc = PreviousPlaybackCursorRectangle;
			if (rc != Rectangle.Empty)
				Control.Invalidate(rc);


			PlaybackCursorTime = ConvertXCoordinateToTime(cursorX);
			var dx = ConvertTimeToXCoordinate(PlaybackCursorTime);
			Control.Invalidate(new Rectangle(dx, 0, dx, Control.ClientSize.Height));
		}

		/// ------------------------------------------------------------------------------------
		public int ConvertTimeToXCoordinate(TimeSpan time)
		{
			return (int)(time.TotalMilliseconds * _pixelPerMillisecond) - _offsetOfLeftEdge;
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan ConvertXCoordinateToTime(int x)
		{
			return TimeSpan.FromMilliseconds((x + _offsetOfLeftEdge) / _pixelPerMillisecond);
		}
	}
}