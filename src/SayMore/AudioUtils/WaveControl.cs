using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SayMore.AudioUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for WaveControl.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WaveControl : UserControl
	{
		public Action<TimeSpan, TimeSpan> Stopped;
		public Action PlaybackStarted;
		public Action<TimeSpan, TimeSpan> PlaybackUpdate;

		/// <summary>
		/// This boolean value gets rid of the currently active region and also refreshes the wave
		/// </summary>
		private bool _resetRegion;

		private WavePainter _painter;
		private WaveStream _waveStream;
		private WaveOut _waveOut;
		private bool _wasStreamCreatedHere;
		private TimeSpan _playbackStartTime;
		private TimeSpan _playbackEndTime;
		private float _zoomPercentage;

		/// ------------------------------------------------------------------------------------
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

			ForeColor = SystemColors.WindowText;
			BackColor = SystemColors.Window;
			_zoomPercentage = 100f;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (_waveOut != null)
				Stop();

			if (_wasStreamCreatedHere && _waveStream != null)
			{
				_waveStream.Close();
				_waveStream.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize(string audioFileName)
		{
			_wasStreamCreatedHere = true;
			Initialize(new WaveFileReader(audioFileName));
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize(WaveStream stream)
		{
			_waveStream = stream;

			// We'll handle a file with any number of channels, but for now, we'll assume
			// all channels contain the same data. Therefore, we'll just store the samples
			// from the first channel.

			// REVIEW: Instead of guessing how big to make the list, calculate
			// the number of samples in the stream.
			var samples = new List<float>(100000);
			var provider = new SampleChannel(stream);
			var buffer = new float[provider.WaveFormat.Channels];

			// TODO: This assumes all channels contain the same data. Figure out how to
			// combine the samples from each channel into a single float value.
			while (provider.Read(buffer, 0, provider.WaveFormat.Channels) > 0)
				samples.Add(buffer[0]);

			Initialize(samples, stream.TotalTime);
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize(IEnumerable<float> samples, TimeSpan totalTime)
		{
			_painter = new WavePainter(this, samples, totalTime);
			_painter.ForeColor = ForeColor;
			_painter.BackColor = BackColor;
			_painter.SetVirtualWidth(Math.Max(ClientSize.Width, AutoScrollMinSize.Width));
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsPlaying
		{
			get { return _waveOut != null; }
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IEnumerable<TimeSpan> SegmentBoundaries
		{
			get { return (_painter != null ? _painter.SegmentBoundaries : null); }
			set
			{
				if (_painter != null)
					_painter.SegmentBoundaries = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool ShadePlaybackAreaDuringPlayback { get; set; }

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

		#endregion

		/// ------------------------------------------------------------------------------------
		public void SetVirtualWidth(int width)
		{
			var percentage = _zoomPercentage / 100;
			var adjustedWidth = (int)(Math.Round(width * percentage, MidpointRounding.AwayFromZero));
			AutoScrollMinSize = new Size(Math.Max(adjustedWidth, ClientSize.Width), ClientSize.Height);

			if (AutoScrollMinSize.Width == 0 || AutoScrollMinSize.Width == ClientSize.Width)
				_zoomPercentage = 100;
		}

		/// ------------------------------------------------------------------------------------
		public void SetCursor(TimeSpan cursorTime)
		{
			_painter.SetCursor(cursorTime);
		}

		/// ------------------------------------------------------------------------------------
		public void SetCursor(int cursorX)
		{
			_painter.SetCursor(cursorX);
		}

		/// ------------------------------------------------------------------------------------
		public void SetPlaybackCursor(TimeSpan cursorTime)
		{
			_painter.SetPlaybackCursor(cursorTime);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetCursorTime()
		{
			return _painter.CursorTime;
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectionTimes(TimeSpan selStartTime, TimeSpan selEndTime)
		{
			_painter.SetSelectionTimes(selStartTime, selEndTime);
		}

		/// ------------------------------------------------------------------------------------
		public void ClearSelectedRegion()
		{
			_painter.SetSelectionTimes(TimeSpan.Zero, TimeSpan.Zero);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetTimeFromX(int dx)
		{
			return _painter.ConvertXCoordinateToTime(dx);
		}

		/// ------------------------------------------------------------------------------------
		public void EnsureXIsVisible(int dx)
		{
			if (dx < 0 || dx > ClientSize.Width)
			{
				var newX = -AutoScrollPosition.X + (dx < 0 ? dx : (dx - ClientSize.Width));
				AutoScrollPosition = new Point(newX, AutoScrollPosition.Y);
				_painter.SetOffsetOfLeftEdge(-AutoScrollPosition.X);
				Invalidate();
			}
		}

		#region Playback methods
		/// ------------------------------------------------------------------------------------
		public void Play(TimeSpan playbackStartTime)
		{
			Play(playbackStartTime, TimeSpan.Zero);
		}

		/// ------------------------------------------------------------------------------------
		public void Play(TimeSpan playbackStartTime, TimeSpan playbackEndTime)
		{
			_playbackStartTime = playbackStartTime;
			_playbackEndTime = playbackEndTime;

			if (_playbackStartTime < TimeSpan.Zero)
				_playbackStartTime = TimeSpan.Zero;

			if (_playbackEndTime <= _playbackStartTime)
				_playbackEndTime = TimeSpan.Zero;

			var waveOutProvider = new SampleChannel(_playbackEndTime == TimeSpan.Zero ?
				new WaveSegmentStream(_waveStream, playbackStartTime) :
				new WaveSegmentStream(_waveStream, playbackStartTime, playbackEndTime - playbackStartTime));

			waveOutProvider.PreVolumeMeter += HandlePlaybackMetering;

			_waveOut = new WaveOut();
			_waveOut.DesiredLatency = 100;
			_waveOut.Init(new SampleToWaveProvider(waveOutProvider));
			_waveOut.PlaybackStopped += delegate { Stop(); };
			_waveOut.Play();

			if (PlaybackStarted != null)
				PlaybackStarted();
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackMetering(object sender, StreamVolumeEventArgs e)
		{
			// We're using a WaveSegmentStream which never gets a PlaybackStopped
			// event on the WaveOut, so we have to force it here.
			if (_waveStream.CurrentTime == (_playbackEndTime > TimeSpan.Zero ? _playbackEndTime : _waveStream.TotalTime))
			{
				_waveOut.Stop();
				return;
			}

			if (ShadePlaybackAreaDuringPlayback)
				SetSelectionTimes(_playbackStartTime, _waveStream.CurrentTime);

			SetCursor(_waveStream.CurrentTime);

			var dx = _painter.ConvertTimeToXCoordinate(_waveStream.CurrentTime);
			dx += (dx < 0 ? -10 : 10);
			EnsureXIsVisible(dx);

			if (PlaybackUpdate != null)
				PlaybackUpdate(_waveStream.CurrentTime, _waveStream.TotalTime);
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			if (_waveOut == null)
				return;

			_waveOut.Stop();
			_waveOut.Dispose();
			_waveOut = null;

			if (Stopped != null)
				Stopped(_playbackStartTime, _waveStream.CurrentTime);
		}

		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		protected override void OnScroll(ScrollEventArgs se)
		{
			if (se.OldValue != se.NewValue && _painter != null && se.ScrollOrientation == ScrollOrientation.HorizontalScroll)
				_painter.SetOffsetOfLeftEdge(se.NewValue);

			base.OnScroll(se);
		}

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

		#region Zooming property/methods
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public float ZoomPercentage
		{
			get { return _zoomPercentage; }
			set
			{
				if (value.Equals(_zoomPercentage) ||
					(value < _zoomPercentage && AutoScrollMinSize.Width == ClientSize.Width))
				{
					return;
				}

				_zoomPercentage = value;
				SetZoom();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void ZoomIn()
		{
			ZoomPercentage += 10;
			SetZoom();
		}

		/// ------------------------------------------------------------------------------------
		public void ZoomOut()
		{
			ZoomPercentage -= 10;
			SetZoom();
		}

		/// ------------------------------------------------------------------------------------
		private void SetZoom()
		{
			SetVirtualWidth(Math.Max(ClientSize.Width, AutoScrollPosition.X));
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
					//_selectedRegionStartX = e.X;
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

			//_selectedRegionEndX = e.X;
			_resetRegion = false;
			//_prevMouseX = e.X;
			Refresh();
		}

		private void WaveControl_MouseUp(object sender, MouseEventArgs e)
		{
			if (_resetRegion)
			{
				//_selectedRegionStartX = 0;
				//_selectedRegionEndX = 0;
				Refresh();
			}
			else
			{
				//_selectedRegionEndX = e.X;
			}
		}
	}
}
