using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SayMore.AudioUtils
{
	public class WavePainterBasic : IDisposable
	{
		public const int kBoundaryHotZoneHalfWidth = 4;

		/// <summary>
		/// Each pixel value (X direction) represents this many samples in the wavefile
		/// Starting value is based on control width so that the .WAV will cover the entire width.
		/// </summary>
		public virtual double SamplesPerPixel { get; private set; }
		public virtual TimeSpan CursorTime { get; private set; }
		public virtual Control Control { get; set; }
		public virtual Color ForeColor { get; set; }
		public virtual Color BackColor { get; set; }
		public virtual Color BoundaryColor { get; set; }
		public virtual Color CursorColor { get; set; }

		protected IEnumerable<TimeSpan> _segmentBoundaries;
		protected double _pixelPerMillisecond;
		protected int _offsetOfLeftEdge;
		protected readonly TimeSpan _totalTime;

		protected float[] _samples = new float[0];
		protected List<float[]>[] _samplesToDraw;
		protected int _channels = 1;
		protected long _numberOfSamples;
		protected WaveFileReader _waveStream;
		protected TimeSpan _movedBoundary;
		protected TimeSpan _movedBoundarysOrigTime;
		protected bool _allowDrawing = true;

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
		public WavePainterBasic(Control ctrl, IEnumerable<float> samples, TimeSpan totalTime)
		{
			//BoundaryColor = Color.MidnightBlue;
			BoundaryColor = Color.FromArgb(220, 121, 0);
			CursorColor = Color.Green;
			BackColor = Color.CornflowerBlue;
			ForeColor = Color.LightGray;

			_segmentBoundaries = new TimeSpan[0];

			Control = ctrl;
			ctrl.Paint += DrawControl;

			_samples = samples.ToArray();
			_totalTime = totalTime;
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (Control != null)
				Control.Paint -= DrawControl;

			_waveStream = null;
		}

		/// ------------------------------------------------------------------------------------
		private void DrawControl(object sender, PaintEventArgs e)
		{
			var ctrl = sender as Control;
			if (ctrl != null)
				Draw(e, ctrl.ClientRectangle);
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
			_pixelPerMillisecond = width / _totalTime.TotalMilliseconds;
			SetSamplesPerPixel(_numberOfSamples / (double)width);
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
				GetSamplesToDrawFromStream();
				return;
			}

			_numberOfSamples = _samples.Length;
			_samplesToDraw = new[] { new List<float[]>((int)(_numberOfSamples / (long)SamplesPerPixel)) };

			for (int sample = 0; sample < _numberOfSamples; sample += (int)SamplesPerPixel)
			{
				var biggestSample = float.MinValue;
				var smallestSample = float.MaxValue;

				// Find the max and min peaks for this pixel
				for (int i = 0; i < SamplesPerPixel && sample + i < _numberOfSamples; i++)
				{
					biggestSample = Math.Max(biggestSample, _samples[sample + i]);
					smallestSample = Math.Min(smallestSample, _samples[sample + i]);
				}

				_samplesToDraw[0].Add(new[] { biggestSample, smallestSample });
			}
		}

		/// ------------------------------------------------------------------------------------
		private void GetSamplesToDrawFromStream()
		{
			_waveStream.Seek(0, SeekOrigin.Begin);
			var provider = new SampleChannel(_waveStream);

			_samplesToDraw = new List<float[]>[_channels];
			for (int c = 0; c < _channels; c++)
				_samplesToDraw[c] = new List<float[]>(Control == null ? 400 : Control.ClientSize.Width);

			int numberSamplesInOnePixel = (int)SamplesPerPixel * _channels;
			var buffer = new float[numberSamplesInOnePixel];

			while (provider.Read(buffer, 0, numberSamplesInOnePixel) > 0)
			{
				for (var c = 0; c < _channels; c++)
				{
					var biggestSample = float.MinValue;
					var smallestSample = float.MaxValue;

					for (int i = 0; i < numberSamplesInOnePixel; i += _channels)
					{
						biggestSample = Math.Max(biggestSample, buffer[i + c]);
						smallestSample = Math.Min(smallestSample, buffer[i + c]);
					}

					_samplesToDraw[c].Add(new[] { biggestSample, smallestSample });
				}
			}
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
			if (!_allowDrawing)
				return;

			if ((_samplesToDraw == null || _samplesToDraw[0].Count == 0) && _numberOfSamples > 0)
			{
				if (SamplesPerPixel.Equals(0))
					SetVirtualWidth(rc.Width);
				else
					LoadBufferOfSamplesToDraw();
			}

			DrawWave(e.Graphics, rc);
			DrawSegmentBoundaries(e.Graphics, rc.Height);
			DrawCursor(e.Graphics, rc);

			if (_movedBoundary > TimeSpan.Zero)
				DrawMovedBoundary(e.Graphics, rc.Height, ConvertTimeToXCoordinate(_movedBoundary));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void DrawWave(Graphics g, Rectangle rc)
		{
			var channelRects = new Rectangle[_channels];
			var dyChannelXAxes = new int[_channels];

			// Calculate the rectangle for each channel.
			int dy = 0;
			for (int c = 0; c < _channels; c++)
			{
				int height = (int)Math.Floor((float)rc.Height / _channels);
				channelRects[c] = new Rectangle(rc.X, dy, rc.Width, height);
				dyChannelXAxes[c] = dy + (int)Math.Ceiling(height / 2f);
				dy += height;
			}

			// Draw the X axis through middle of each channel's rectangle.
			using (var pen = new Pen(ForeColor))
			{
				foreach (var xAxis in dyChannelXAxes)
					g.DrawLine(pen, 0, xAxis, rc.Width, xAxis);
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

			for (int x = (int)clipRect.X; x < clipRect.X + clipRect.Width && x + _offsetOfLeftEdge < _samplesToDraw[0].Count; x++)
			{
				for (int channel = 0; channel < _channels; channel++)
				{
					var sampleAmplitudes = _samplesToDraw[channel][x + _offsetOfLeftEdge];

					if (sampleAmplitudes[0].Equals(0f) && sampleAmplitudes[1].Equals(0f))
						continue;

					int y1 = dyChannelXAxes[channel] - (int)Math.Ceiling(channelRects[channel].Height * (sampleAmplitudes[0] / 2f));
					int y2 = dyChannelXAxes[channel] - (int)Math.Ceiling(channelRects[channel].Height * (sampleAmplitudes[1] / 2f));

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