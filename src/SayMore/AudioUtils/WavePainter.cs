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

		/// <summary>
		/// Each pixel value (X direction) represents this many samples in the wavefile
		/// Starting value is based on control width so that the .WAV will cover the entire width.
		/// </summary>
		private double _samplesPerPixel;

		private TimeSpan _cursor;
		private double _pixelPerMillisecond;
		private int _offsetOfLeftEdge;
		private readonly TimeSpan _totalTime;
		private readonly float[] _samples = new float[0];
		private KeyValuePair<float, float>[] _samplesToDraw = new KeyValuePair<float,float>[0];

		public Color ForeColor { get; set; }
		public Color BackColor { get; set; }
		public int SelectionStartX  { get; set; }
		public int SelectionEndX { get; set; }
		public Rectangle PreviousCursorRectangle { get; private set; }

		/// ------------------------------------------------------------------------------------
		public WavePainter(IEnumerable<float> samples, TimeSpan totalTime)
		{
			_samples = samples.ToArray();
			_totalTime = totalTime;

			BackColor = Color.CornflowerBlue;
			ForeColor = Color.LightGray;
		}

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
			_samplesPerPixel = value;
			LoadBufferOfSamplesToDraw();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadBufferOfSamplesToDraw()
		{
			var samplesToDraw = new List<KeyValuePair<float, float>>();
			int firstSampleInPixel = 0;

			while (firstSampleInPixel < _samples.Length)
			{
				float maxVal = float.MinValue;
				float minVal = float.MaxValue;

				// Find the max & min peaks for this pixel
				for (int i = 0; i < _samplesPerPixel && firstSampleInPixel + i< _samples.Length; i++)
				{
					maxVal = Math.Max(maxVal, _samples[firstSampleInPixel + i]);
					minVal = Math.Min(minVal, _samples[firstSampleInPixel + i]);
				}

				samplesToDraw.Add(new KeyValuePair<float, float>(maxVal, minVal));
				firstSampleInPixel += (int)_samplesPerPixel;
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
			if (_samplesPerPixel > 0.0000000001d)
				DrawWave(e.Graphics, rc);

			// Now draw the selected region, if there is one.
			DrawSelectedRegion(e.Graphics, rc);

			if (_cursor > TimeSpan.Zero)
				DrawCursor(e.Graphics, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawWave(Graphics g, Rectangle rc)
		{
			// Samples will span from some value between 0 and +1 to some value between 0 and -1.
			// The top of the client area represents +1 and the bottom represents -1.

			var blend = new Blend();
			blend.Positions = new[] { 0f, 0.15f, 0.5f, 0.85f, 1.0f };
			blend.Factors = new[] { 0.65f, 0.85f, 1.0f, 0.85f, 0.65f };

			int verticalMidPoint = rc.Y + (int)Math.Round(rc.Height / 2d, MidpointRounding.AwayFromZero);

			var clipRect = g.VisibleClipBounds;

			for (int x = (int)clipRect.X; x < clipRect.X + clipRect.Width; x++)
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
		private void DrawSelectedRegion(Graphics g, Rectangle rc)
		{
			int regionStartX = Math.Min(SelectionStartX, SelectionEndX);
			int regionEndX = Math.Max(SelectionStartX, SelectionEndX);

			if (regionStartX == regionEndX)
				return;

			using (var br = new SolidBrush(Color.FromArgb(128, 0, 0, 0)))
				g.FillRectangle(br, regionStartX, 0, regionEndX - regionStartX, rc.Height);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawCursor(Graphics g, Rectangle rc)
		{
			int cursorX = GetCursorX();

			using (var pen = new Pen(Color.FromArgb(200, ForeColor)))
				g.DrawLine(pen, cursorX, 0, cursorX, rc.Height);

			PreviousCursorRectangle = new Rectangle(cursorX, 0, 1, rc.Height);
		}

		/// ------------------------------------------------------------------------------------
		public void SetCursor(Control ctrl, TimeSpan cursorTime)
		{
			var rc = PreviousCursorRectangle;
			if (rc != Rectangle.Empty)
				ctrl.Invalidate(rc);

			_cursor = cursorTime;
			var cursorX = GetCursorX();
			ctrl.Invalidate(new Rectangle(cursorX, 0, cursorX, ctrl.ClientSize.Height));
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetCursor()
		{
			return _cursor;
		}

		/// ------------------------------------------------------------------------------------
		public int GetCursorX()
		{
			return (int)(_cursor.TotalMilliseconds * _pixelPerMillisecond) - _offsetOfLeftEdge;
		}
	}
}