using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SayMore.AudioUtils
{
	/// <summary>
	/// Summary description for WaveControl.
	/// </summary>
	public class WaveControl : UserControl
	{
		/// <summary>This is the starting x value of a mouse drag</summary>
		private int _selectedRegionStartX = 0;

		/// <summary>This is the ending x value of a mouse drag</summary>
		private int _selectedRegionEndX;

		/// <summary>This is the value of the previous mouse move event</summary>
		private int _prevMouseX;

		/// <summary>
		/// This boolean value gets rid of the currently active region and also refreshes the wave
		/// </summary>
		private bool _resetRegion;

		private WavePainter _painter;

		public WaveControl()
		{
			// REVIEW: This class assumes the audio data is in 16 bit samples.

			//this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WaveControl_MouseUp);
			//this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WaveControl_MouseMove);
			//this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WaveControl_MouseDown);

			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			ResizeRedraw = true;

			AutoScroll = true;
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize(IEnumerable<float> samples, TimeSpan totalTime)
		{
			_painter = new WavePainter(samples, totalTime);
			_painter.ForeColor = ForeColor;
			_painter.BackColor = BackColor;
		}

		/// ------------------------------------------------------------------------------------
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set
			{
				if (_painter != null)
					_painter.ForeColor = value;

				base.ForeColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				if (_painter != null)
					_painter.BackColor = value;

				base.BackColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public new Size AutoScrollMinSize
		{
			get { return base.AutoScrollMinSize; }
			set
			{
				base.AutoScrollMinSize = value;

				if (_painter != null)
				{
					_painter.SetVirtualWidth(Math.Max(ClientSize.Width, value.Width));
					Invalidate();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public new Point AutoScrollPosition
		{
			get { return base.AutoScrollPosition; }
			set
			{
				base.AutoScrollPosition = value;

				if (_painter != null)
				{
					_painter.SetOffsetOfLeftEdge(-AutoScrollPosition.X);
					Invalidate();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetVirtualWidth(int width)
		{
			AutoScrollMinSize = new Size(Math.Max(width, ClientSize.Width), ClientSize.Height);
		}

		/// ------------------------------------------------------------------------------------
		public void SetCursor(int cursorTimeInMilliseconds)
		{
			SetCursor(TimeSpan.FromMilliseconds(cursorTimeInMilliseconds));
		}

		/// ------------------------------------------------------------------------------------
		public void SetCursor(TimeSpan cursorTime)
		{
			_painter.SetCursor(this, cursorTime);
		}

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			if (AutoScroll)
				AutoScrollMinSize = new Size(AutoScrollMinSize.Width, ClientSize.Height);

			base.OnResize(e);

			if (_painter != null)
				_painter.SetVirtualWidth(Math.Max(AutoScrollMinSize.Width, ClientSize.Width));

			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (_painter != null)
				_painter.Draw(e, ClientRectangle);
		}

		///// ------------------------------------------------------------------------------------
		//protected override void OnMouseWheel(MouseEventArgs e)
		//{
		//    if (e.Delta * SystemInformation.MouseWheelScrollLines / 120 > 0)
		//        ZoomIn();
		//    else
		//        ZoomOut();

		//    Refresh();
		//}

		#endregion

		#region Zooming methods
		/// ------------------------------------------------------------------------------------
		public void ZoomIn()
		{
			// Zoom in by 10%
			Zoom(0.1f);
		}

		/// ------------------------------------------------------------------------------------
		public void ZoomIn(float percentage)
		{
			Zoom(percentage);
		}

		/// ------------------------------------------------------------------------------------
		public void ZoomOut()
		{
			// Zoom out by 10%
			ZoomOut(-0.1f);
		}

		/// ------------------------------------------------------------------------------------
		public void ZoomOut(float percentage)
		{
			if (AutoScrollMinSize.Width != ClientSize.Width)
				Zoom(-percentage);
		}

		/// ------------------------------------------------------------------------------------
		private void Zoom(float percentage)
		{
			if (_painter != null)
				_painter.SetVirtualWidth((int)(AutoScrollMinSize.Width + (ClientSize.Width * percentage)));
		}

		//private void ZoomToRegion()
		//{
		//    int regionStartX = Math.Min(_selectedRegionStartX, _selectedRegionEndX);
		//    int regionEndX = Math.Max(_selectedRegionStartX, _selectedRegionEndX);

		//    // if they are negative, make them zero
		//    regionStartX = Math.Max(0, regionStartX);
		//    regionEndX = Math.Max(0, regionEndX);

		//    _offsetInSamples += (int)(regionStartX * _samplesPerPixel);

		//    int numSamplesToShow = (int)((regionEndX - regionStartX) * _samplesPerPixel);

		//    if (numSamplesToShow > 0)
		//    {
		//        SamplesPerPixel = (double)numSamplesToShow / ClientSize.Width;
		//        _resetRegion = true;
		//    }
		//}

		//private void ZoomOutFull()
		//{
		//    SamplesPerPixel = (_numSamples / (double)ClientSize.Width);
		//    _offsetInSamples = 0;
		//    _resetRegion = true;
		//}

		#endregion

		//private void Scroll(int newXValue)
		//{
		//    _offsetInSamples -= (int)((newXValue - _prevMouseX) * _samplesPerPixel);

		//    if (_offsetInSamples < 0)
		//        _offsetInSamples = 0;
		//}

		private void WaveControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				//if (m_AltKeyDown)
				//{
				//    _prevMouseX = e.X;
				//}
				//else
				{
					_selectedRegionStartX = e.X;
					_resetRegion = true;
				}
			}
			//else if (e.Button == MouseButtons.Right)
			//{
			//    if (e.Clicks == 2)
			//        ZoomOutFull();
			//    else
			//        ZoomToRegion();
			//}
		}

		private void WaveControl_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			_selectedRegionEndX = e.X;
			_resetRegion = false;
			_prevMouseX = e.X;
			Refresh();
		}

		private void WaveControl_MouseUp(object sender, MouseEventArgs e)
		{
			if (_resetRegion)
			{
				_selectedRegionStartX = 0;
				_selectedRegionEndX = 0;
				Refresh();
			}
			else
			{
				_selectedRegionEndX = e.X;
			}
		}
	}
}
