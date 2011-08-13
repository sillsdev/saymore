using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SilTools;

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
		public double SamplesPerPixel { get; private set; }

		private int _cursorX;
		private int _playbackCursorX;
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
				float maxVal = float.MinValue;
				float minVal = float.MaxValue;

				// Find the max & min peaks for this pixel
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

			// Now draw the selected region, if there is one.
			DrawSelectedRegion(e.Graphics, rc);

			if (_cursorX > 0)
				DrawCursor(e.Graphics, rc);

			if (_playbackCursorX > 0)
				DrawPlaybackProgressShading(e.Graphics, rc);
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
			using (var pen = new Pen(Color.FromArgb(255, Color.Red)))
				g.DrawLine(pen, _cursorX, 0, _cursorX, rc.Height);

			PreviousCursorRectangle = new Rectangle(_cursorX, 0, 1, rc.Height);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawPlaybackProgressShading(Graphics g, Rectangle rc)
		{
			rc.X = _cursorX;
			rc.Width = _playbackCursorX - _cursorX;

			using (var br = new SolidBrush(Color.FromArgb(110, SystemColors.Highlight)))
				g.FillRectangle(br, rc);
		}

		/// ------------------------------------------------------------------------------------
		public void SetCursor(Control ctrl, TimeSpan cursorTime)
		{
			SetCursor(ctrl, ConvertTimeToXCoordinate(cursorTime));
		}

		/// ------------------------------------------------------------------------------------
		public void SetCursor(Control ctrl, int cursorX)
		{
			if (_playbackCursorX > 0)
				SetPlaybackCursor(ctrl, cursorX);
			else
			{
				var rc = PreviousCursorRectangle;
				if (rc != Rectangle.Empty)
					ctrl.Invalidate(rc);

				_cursorX = cursorX;
				ctrl.Invalidate(new Rectangle(_cursorX, 0, _cursorX, ctrl.ClientSize.Height));
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetPlaybackCursor(Control ctrl, TimeSpan cursorTime)
		{
			SetPlaybackCursor(ctrl, ConvertTimeToXCoordinate(cursorTime));
		}

		/// ------------------------------------------------------------------------------------
		public void SetPlaybackCursor(Control ctrl, int cursorX)
		{
			_playbackCursorX = cursorX;

			if (cursorX > 0)
			{
				ctrl.Invalidate(new Rectangle(_playbackCursorX - 10, 0,
					_playbackCursorX + 10, ctrl.ClientSize.Height));
			}
			else
			{
				Utils.SetWindowRedraw(ctrl, false);
				ctrl.Invalidate();
				Utils.SetWindowRedraw(ctrl, true);
			}
		}

		/// ------------------------------------------------------------------------------------
		private int ConvertTimeToXCoordinate(TimeSpan time)
		{
			return (int)(time.TotalMilliseconds * _pixelPerMillisecond) - _offsetOfLeftEdge;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the x-coordinate of the cursor to a time. The calculation is approximate.
		/// This *does* factors in whether or not the auto scroll position of the client area
		/// is not zero.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TimeSpan GetCursorTime()
		{
			return TimeSpan.FromMilliseconds((_cursorX + _offsetOfLeftEdge) / _pixelPerMillisecond);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the x-coordinate of the cursor within the client rectangle. Therefore, this
		/// does not factor in whether or not the auto scroll position of the client area
		/// is not zero.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int GetCursor()
		{
			return _cursorX;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the x-coordinate of the playback cursor within the client rectangle.
		/// Therefore, this does not factor in whether or not the auto scroll position of
		/// the client area is not zero.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int GetPlaybackCursor()
		{
			return _playbackCursorX;
		}
	}
}