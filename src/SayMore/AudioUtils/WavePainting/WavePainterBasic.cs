using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SilTools;

namespace SayMore.Media
{
	public class WavePainterBasic : IDisposable
	{
		public const int kBoundaryHotZoneHalfWidth = 4;
		public const int kRightDisplayPadding = 15;

		/// <summary>
		/// Each pixel value (X direction) represents this many samples in the wavefile
		/// Starting value is based on control width so that the .WAV will cover the entire width.
		/// </summary>
		public virtual double SamplesPerPixel { get; private set; }
		public virtual TimeSpan CursorTime { get; private set; }
		public virtual Control Control { get; set; }
		public virtual Color ForeColor { get; set; }
		public virtual Color BackColor { get; set; }

		public virtual Color CursorColor { get; set; }
		public virtual Func<WaveFormat, string> FormatNotSupportedMessageProvider { get; set; }
		public virtual int BottomReservedAreaHeight { get; set; }
		public virtual Color BottomReservedAreaColor { get; set; }
		public virtual Color BottomReservedAreaBorderColor { get; set; }
		public Action<PaintEventArgs, Rectangle> BottomReservedAreaPaintAction { get; set; }

		protected IEnumerable<TimeSpan> _segmentBoundaries;
		protected double _pixelPerMillisecond;
		protected int _offsetOfLeftEdge;
		protected int _virtualWidth;
		protected readonly TimeSpan _totalTime;

		protected float[] _samples = new float[0];
		protected Tuple<float, float>[,] _samplesToDraw;
		protected int _channels = 1;
		protected long _numberOfSamples;
		protected WaveFileReader _waveStream;
		protected TimeSpan _movedBoundary;
		protected TimeSpan _movedBoundarysOrigTime;
		protected bool _allowDrawing = true;
		protected Color _boundaryColor;
		protected Pen _solidBorderPen;
		protected Pen _translucentBorderPen;

		/// ------------------------------------------------------------------------------------
		public WavePainterBasic(WaveFileReader stream) : this(null, stream)
		{
		}

		/// ------------------------------------------------------------------------------------
		public WavePainterBasic(Control ctrl, WaveFileReader stream) :
			this(ctrl, new float[0], stream.TotalTime)
		{
			_waveStream = stream;
			_channels = _waveStream.WaveFormat.Channels;
			_numberOfSamples = _waveStream.SampleCount;
		}

		/// ------------------------------------------------------------------------------------
		public WavePainterBasic(IEnumerable<float> samples, TimeSpan totalTime)
			: this(null, samples, totalTime)
		{
		}

		/// ------------------------------------------------------------------------------------
		public WavePainterBasic(Control ctrl, IEnumerable<float> samples, TimeSpan totalTime) : this()
		{
			Control = ctrl;

			if (ctrl != null)
				ctrl.Paint += DrawControl;

			_samples = samples.ToArray();
			_totalTime = totalTime;
		}

		/// ------------------------------------------------------------------------------------
		public WavePainterBasic()
		{
			BottomReservedAreaHeight = 0;
			BottomReservedAreaColor = Color.LightGray;
			BottomReservedAreaBorderColor = Color.Gray;

			//BoundaryColor = Color.MidnightBlue;
			BoundaryColor = Color.FromArgb(220, 121, 0);
			CursorColor = Color.Green;
			BackColor = Color.CornflowerBlue;
			ForeColor = Color.LightGray;

			_segmentBoundaries = new TimeSpan[0];
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (Control != null)
				Control.Paint -= DrawControl;

			DisposeOfPens();

			_waveStream = null;
		}

		/// ------------------------------------------------------------------------------------
		private void DisposeOfPens()
		{
			if (_translucentBorderPen != null)
			{
				_translucentBorderPen.Dispose();
				_translucentBorderPen = null;
			}

			if (_solidBorderPen != null)
			{
				_solidBorderPen.Dispose();
				_solidBorderPen = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DrawControl(object sender, PaintEventArgs e)
		{
			var ctrl = sender as Control;
			if (ctrl != null)
			{
				var rc = ctrl.ClientRectangle;
				rc.Height -= BottomReservedAreaHeight;
				Draw(e, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<TimeSpan> SegmentBoundaries
		{
			get { return _segmentBoundaries; }
			set
			{
				_segmentBoundaries = (value ?? new TimeSpan[0]);
				if (_allowDrawing && Control != null)
					Control.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual Color BoundaryColor
		{
			get { return _boundaryColor; }
			set
			{
				_boundaryColor = value;
				DisposeOfPens();
				_solidBorderPen = new Pen(_boundaryColor);
				_translucentBorderPen = new Pen(Color.FromArgb(60, _boundaryColor));
				if (_allowDrawing && Control != null)
					Control.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool AllowRedraw
		{
			get { return _allowDrawing; }
			set
			{
				if (_allowDrawing == value)
					return;

				_allowDrawing = value;

				if (_allowDrawing && Control != null)
					Control.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetVirtualWidth(int width)
		{
			_virtualWidth = width;
			_pixelPerMillisecond = (width - kRightDisplayPadding) / _totalTime.TotalMilliseconds;
			SetSamplesPerPixel(_numberOfSamples / ((double)width - kRightDisplayPadding));
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
			if (!_allowDrawing)
				return;

			if (_waveStream != null)
			{
				_samplesToDraw = AudioFileHelper.GetSamples(_waveStream,
					(uint)(_virtualWidth - kRightDisplayPadding));

				return;
			}

			//_numberOfSamples = _samples.Length;
			//_samplesToDraw = new[] { new List<float[]>((int)(_numberOfSamples / (long)SamplesPerPixel)) };

			//for (int sample = 0; sample < _numberOfSamples; sample += (int)SamplesPerPixel)
			//{
			//    var biggestSample = float.MinValue;
			//    var smallestSample = float.MaxValue;

			//    // Find the max and min peaks for this pixel
			//    for (int i = 0; i < SamplesPerPixel && sample + i < _numberOfSamples; i++)
			//    {
			//        biggestSample = Math.Max(biggestSample, _samples[sample + i]);
			//        smallestSample = Math.Min(smallestSample, _samples[sample + i]);
			//    }

			//    _samplesToDraw[0].Add(new[] { biggestSample, smallestSample });
			//}
		}

		/// ------------------------------------------------------------------------------------
		public void SetSamplesToDraw(Tuple<float, float>[,] samplesToDraw)
		{
			_samplesToDraw = samplesToDraw;
			_channels = samplesToDraw.GetLength(1);
			SamplesPerPixel = 1d;
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
			return (int)(Math.Ceiling(time.TotalMilliseconds * _pixelPerMillisecond)) - _offsetOfLeftEdge;
		}

		/// ------------------------------------------------------------------------------------
		public virtual TimeSpan ConvertXCoordinateToTime(int x)
		{
			return TimeSpan.FromMilliseconds(Math.Ceiling((x + _offsetOfLeftEdge) / _pixelPerMillisecond));
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
				pixelsOnEitherSide * 2 + 1, Control.ClientSize.Height - BottomReservedAreaHeight));
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Draw(PaintEventArgs e, Rectangle rc)
		{
			if (!_allowDrawing)
				return;

			if (_waveStream != null && (/*_waveStream.WaveFormat.Channels > 2 || */
				_waveStream.WaveFormat.BitsPerSample == 32 && _waveStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat))
			{
				var msg = FormatNotSupportedMessageProvider == null ?
					string.Format("{0} bit, {1} channel, {2} audio files are not supported.",
						_waveStream.WaveFormat.BitsPerSample, _waveStream.WaveFormat.Channels, _waveStream.WaveFormat.Encoding) :
					FormatNotSupportedMessageProvider(_waveStream.WaveFormat);

				DrawFormatNotSupportedMessage(e.Graphics, rc, msg);
				return;
			}

			if ((_samplesToDraw == null || _samplesToDraw.Length == 0) && _numberOfSamples > 0)
			{
				if (SamplesPerPixel.Equals(0))
					SetVirtualWidth(rc.Width);
				else
					LoadBufferOfSamplesToDraw();
			}

			DrawWave(e.Graphics, rc);
			DrawBottomArea(e, rc);
			DrawSegmentBoundaries(e.Graphics, rc.Height + BottomReservedAreaHeight);
			DrawCursor(e.Graphics, rc);

			if (_movedBoundary > TimeSpan.Zero)
				DrawMovedBoundary(e.Graphics, rc.Height, ConvertTimeToXCoordinate(_movedBoundary));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void DrawBottomArea(PaintEventArgs e, Rectangle rc)
		{
			if (BottomReservedAreaHeight == 0)
				return;

			rc.X = e.ClipRectangle.X;
			rc.Width = e.ClipRectangle.Width;
			rc.Y = rc.Bottom;
			rc.Height = BottomReservedAreaHeight;

			using (var br = new SolidBrush(BottomReservedAreaColor))
				e.Graphics.FillRectangle(br, rc);

			using (var pen = new Pen(BottomReservedAreaBorderColor))
				e.Graphics.DrawLine(pen, rc.X, rc.Y, rc.Right, rc.Y);

			if (BottomReservedAreaPaintAction != null)
				BottomReservedAreaPaintAction(e, rc);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void DrawFormatNotSupportedMessage(Graphics g, Rectangle rc, string msg)
		{
			if (msg == null)
			{
			}

			using (var fnt = FontHelper.MakeFont(SystemFonts.MenuFont, 10, FontStyle.Bold))
			{
				const TextFormatFlags fmt = TextFormatFlags.VerticalCenter |
					TextFormatFlags.HorizontalCenter | TextFormatFlags.WordBreak |
					TextFormatFlags.EndEllipsis;

				TextRenderer.DrawText(g, msg, fnt, rc, SystemColors.GrayText, fmt);
			}
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Rectangle> GetChannelDisplayRectangles(Rectangle rectForAllChannels)
		{
			if (rectForAllChannels != Rectangle.Empty)
			{
				// Calculate the rectangle for each channel.
				int dy = rectForAllChannels.Y;
				for (int c = 0; c < _channels; c++)
				{
					int height = (int)Math.Floor((float)rectForAllChannels.Height / _channels);
					var rc = new Rectangle(rectForAllChannels.X, dy, rectForAllChannels.Width, height);
					dy += height;
					yield return rc;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void DrawWave(Graphics g, Rectangle rc)
		{
			if (!g.VisibleClipBounds.IntersectsWith(rc))
				return;

			var channelRects = GetChannelDisplayRectangles(rc).ToArray();
			var dyChannelXAxes = new int[_channels];

			// Calculate the midpoint of each rectangle.
			for (int c = 0; c < _channels; c++)
				dyChannelXAxes[c] = channelRects[c].Y + (int)Math.Ceiling(channelRects[c].Height / 2f);

			// Draw the X axis through middle of each channel's rectangle.
			using (var pen = new Pen(ForeColor))
			{
				foreach (var xAxis in dyChannelXAxes)
					g.DrawLine(pen, rc.X, xAxis, rc.Right, xAxis);
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

			var clipRect = g.VisibleClipBounds;

			for (int x = (int)clipRect.X; x < clipRect.X + clipRect.Width; x++)
			{
				if (x < rc.X)
					continue;

				var sampleToDraw = x + _offsetOfLeftEdge - rc.X;
				if (sampleToDraw >= _samplesToDraw.GetLength(0))
					break;

				for (int channel = 0; channel < _channels; channel++)
				{
					var sampleAmplitudes = _samplesToDraw[sampleToDraw, channel];

					if (sampleAmplitudes.Item1.Equals(0f) && sampleAmplitudes.Item2.Equals(0f))
						continue;

					int y1 = dyChannelXAxes[channel] - (int)Math.Ceiling(channelRects[channel].Height * (sampleAmplitudes.Item1 / 2f));
					int y2 = dyChannelXAxes[channel] - (int)Math.Ceiling(channelRects[channel].Height * (sampleAmplitudes.Item2 / 2f));

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
				(Control == null ? 0 : Control.ClientRectangle.Height - BottomReservedAreaHeight));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void DrawSegmentBoundaries(Graphics g, int clientHeight)
		{
			if (_segmentBoundaries == null)
				return;

			foreach (var boundary in _segmentBoundaries)
				DrawBoundary(g, clientHeight, ConvertTimeToXCoordinate(boundary), boundary);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void DrawBoundary(Graphics g, int clientHeight, int x, TimeSpan boundary)
		{
			if (_movedBoundarysOrigTime > TimeSpan.Zero && _movedBoundarysOrigTime == boundary)
			{
				_solidBorderPen.DashStyle = DashStyle.Dot;
				g.DrawLine(_solidBorderPen, x, 0, x, clientHeight);
				_solidBorderPen.DashStyle = DashStyle.Solid;
			}
			else
				DrawBoundary(g, x, 0, clientHeight);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void DrawBoundary(Graphics g, int x, int y, int height)
		{
			g.DrawLine(_translucentBorderPen, x - 1, y, x - 1, y + height);
			g.DrawLine(_solidBorderPen, x, y, x, y + height);
			g.DrawLine(_translucentBorderPen, x + 1, y, x + 1, y + height);
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
		protected virtual void DrawMovedBoundary(Graphics g, int rectHeight, int dx)
		{
			using (var pen = new Pen(BoundaryColor))
				g.DrawLine(pen, dx, 0, dx, rectHeight);
		}
	}
}