using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SilTools;

namespace SayMore.AudioUtils
{
	/// ----------------------------------------------------------------------------------------
	public class WaveControlBasic : UserControl
	{
		public delegate TimeSpan SetCurorAtTimeOnMouseClickHandler(WaveControlBasic ctrl, TimeSpan timeAtMouseX);
		public delegate void PlaybackEventHandler(WaveControlBasic ctrl, TimeSpan time1, TimeSpan time2);
		public delegate void CursorTimeChangedHandler(WaveControlBasic ctrl, TimeSpan cursorTime);
		public delegate void BoundaryMouseDownHandler(WaveControlBasic ctrl, int mouseX,
			TimeSpan boundary, int boundaryNumber);

		public event SetCurorAtTimeOnMouseClickHandler SetCurorAtTimeOnMouseClick;
		public event PlaybackEventHandler PlaybackStarted;
		public event PlaybackEventHandler PlaybackStopped;
		public event PlaybackEventHandler PlaybackUpdate;
		public event BoundaryMouseDownHandler BoundaryMouseDown;
		public event CursorTimeChangedHandler CursorTimeChanged;

		protected float _zoomPercentage;
		protected bool _wasStreamCreatedHere;
		protected WavePainterBasic _painter;
		protected WaveStream _waveStream;
		protected WaveOut _waveOut;

		protected TimeSpan _playbackStartTime;
		protected TimeSpan _playbackEndTime;
		protected TimeSpan _boundaryMouseOver;

		/// ------------------------------------------------------------------------------------
		public WaveControlBasic()
		{
			// ENHANCE: This class assumes the audio data is in 16 bit samples.

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
		public virtual void Initialize(string audioFileName)
		{
			_wasStreamCreatedHere = true;
			Initialize(new WaveFileReader(audioFileName));
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Initialize(WaveStream stream)
		{
			_waveStream = stream;

			// We'll handle a file with any number of channels, but for now, we'll assume
			// all channels contain the same data. Therefore, we'll just store the samples
			// from the first channel.

			// REVIEW: Instead of guessing how big to make the list, calculate
			// the number of samples in the stream exactly.
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
		public virtual void Initialize(IEnumerable<float> samples, TimeSpan totalTime)
		{
			_painter = GetNewWavePainter(samples, totalTime);
			_painter.ForeColor = ForeColor;
			_painter.BackColor = BackColor;
			_painter.SetVirtualWidth(Math.Max(ClientSize.Width, AutoScrollMinSize.Width));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual WavePainterBasic GetNewWavePainter(IEnumerable<float> samples, TimeSpan totalTime)
		{
			return new WavePainterBasic(this, samples, totalTime);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool IsPlaying
		{
			get { return _waveOut != null; }
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IEnumerable<TimeSpan> SegmentBoundaries
		{
			get { return (_painter != null ? _painter.SegmentBoundaries : new TimeSpan[] { }); }
			set
			{
				if (_painter != null)
					_painter.SegmentBoundaries = value;
			}
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
					Utils.SetWindowRedraw(this, false);
					_painter.SetVirtualWidth(Math.Max(ClientSize.Width, value.Width));
					Invalidate();
					Utils.SetWindowRedraw(this, true);
				}
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public virtual void SetVirtualWidth(int width)
		{
			var percentage = _zoomPercentage / 100;
			var adjustedWidth = (int)(Math.Round(width * percentage, MidpointRounding.AwayFromZero));
			AutoScrollMinSize = new Size(Math.Max(adjustedWidth, ClientSize.Width), ClientSize.Height);

			if (AutoScrollMinSize.Width == 0 || AutoScrollMinSize.Width == ClientSize.Width)
				_zoomPercentage = 100;
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetCursor(int cursorX)
		{
			_painter.SetCursor(cursorX);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetCursor(TimeSpan cursorTime)
		{
			OnCursorTimeChanged(cursorTime);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnCursorTimeChanged(TimeSpan cursorTime)
		{
			_painter.SetCursor(cursorTime);

			if (CursorTimeChanged != null)
				CursorTimeChanged(this, cursorTime);
		}

		/// ------------------------------------------------------------------------------------
		public virtual TimeSpan GetCursorTime()
		{
			return _painter.CursorTime;
		}

		/// ------------------------------------------------------------------------------------
		public virtual TimeSpan GetTimeFromX(int dx)
		{
			return _painter.ConvertXCoordinateToTime(dx);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void EnsureXIsVisible(int dx)
		{
			if (dx < 0 || dx > ClientSize.Width)
			{
				var newX = -AutoScrollPosition.X + (dx < 0 ? dx : (dx - ClientSize.Width));
				AutoScrollPosition = new Point(newX, AutoScrollPosition.Y);
				_painter.SetOffsetOfLeftEdge(-AutoScrollPosition.X);
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Rectangle> GetSegmentRectangles()
		{
			var startTime = TimeSpan.Zero;

			foreach (var endTime in SegmentBoundaries)
			{
				yield return _painter.GetRectangleForTimeRange(startTime, endTime);
				startTime = endTime;
			}
		}

		/// ------------------------------------------------------------------------------------
		public int GetIndexOfFirstBoundaryBeforeTime(TimeSpan time)
		{
			var segs = SegmentBoundaries.ToArray();

			for (var i = segs.Length; i >= 0; i--)
			{
				if (time >= segs[i])
					return i;
			}

			return -1;
		}

		/// ------------------------------------------------------------------------------------
		public int GetIndexOfFirstBoundaryAfterTime(TimeSpan time)
		{
			int segNumber = 0;

			foreach (var boundary in SegmentBoundaries)
			{
				if (time <= boundary)
					return segNumber;

				segNumber++;
			}

			return -1;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual int GetIndexOfBoundary(TimeSpan boundary)
		{
			int i = 0;
			foreach (var b in SegmentBoundaries)
			{
				if (b == boundary)
					return i;

				i++;
			}

			return -1;
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsMouseOverBoundary()
		{
			return (_boundaryMouseOver != default(TimeSpan));
		}

		#region Playback/stop methods
		/// ------------------------------------------------------------------------------------
		public virtual void Play(TimeSpan playbackStartTime)
		{
			Play(playbackStartTime, TimeSpan.Zero);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Play(TimeSpan playbackStartTime, TimeSpan playbackEndTime)
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
			OnPlaybackStarted(playbackStartTime, playbackEndTime);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnPlaybackStarted(TimeSpan startTime, TimeSpan endTime)
		{
			if (PlaybackStarted != null)
				PlaybackStarted(this, startTime, endTime);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandlePlaybackMetering(object sender, StreamVolumeEventArgs e)
		{
			// We're using a WaveSegmentStream which never gets a PlaybackStopped
			// event on the WaveOut, so we have to force it here.
			if (_waveStream.CurrentTime == (_playbackEndTime > TimeSpan.Zero ? _playbackEndTime : _waveStream.TotalTime))
			{
				_waveOut.Stop();
				return;
			}
			var dx = _painter.ConvertTimeToXCoordinate(_waveStream.CurrentTime);
			dx += (dx < 0 ? -10 : 10);
			EnsureXIsVisible(dx);

			OnInternalPlaybackUpdate(_waveStream.CurrentTime, _waveStream.TotalTime);
			SetCursor(_waveStream.CurrentTime);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnInternalPlaybackUpdate(TimeSpan currentTimeInStream, TimeSpan streamLength)
		{
			if (PlaybackUpdate != null)
				PlaybackUpdate(this, currentTimeInStream, streamLength);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Stop()
		{
			if (_waveOut == null)
				return;

			_waveOut.Stop();
			_waveOut.Dispose();
			_waveOut = null;
			OnPlaybackStopped(_playbackStartTime, _waveStream.CurrentTime);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnPlaybackStopped(TimeSpan startTime, TimeSpan endTime)
		{
			if (PlaybackStopped != null)
				PlaybackStopped(this, startTime, endTime);
		}

		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		protected override void OnScroll(ScrollEventArgs e)
		{
			if (e.OldValue != e.NewValue && _painter != null && e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
				_painter.SetOffsetOfLeftEdge(e.NewValue);

			base.OnScroll(e);
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
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (SegmentBoundaries == null)
			{
				_boundaryMouseOver = default(TimeSpan);
				return;
			}

			var timeAtMouseA = (e.X <= 4 ? TimeSpan.Zero : GetTimeFromX(e.X - 4));
			var timeAtMouseB = GetTimeFromX(e.X + 4);
			OnMouseMoveEx(e, SegmentBoundaries.FirstOrDefault(b => b >= timeAtMouseA && b <= timeAtMouseB));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnMouseMoveEx(MouseEventArgs e, TimeSpan boundaryMouseOver)
		{
			_boundaryMouseOver = boundaryMouseOver;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (IsPlaying)
				Stop();

			var timeAtX = GetTimeFromX(e.X);

			if (_boundaryMouseOver == default(TimeSpan))
			{
				if (SetCurorAtTimeOnMouseClick != null)
					timeAtX = SetCurorAtTimeOnMouseClick(this, timeAtX);

				OnSetCursorWhenMouseDown(timeAtX, false);
				return;
			}

			OnBoundaryMouseDown(e.X, _boundaryMouseOver, GetIndexOfBoundary(_boundaryMouseOver));

			timeAtX = (SetCurorAtTimeOnMouseClick == null ? _boundaryMouseOver :
				timeAtX = SetCurorAtTimeOnMouseClick(this, timeAtX));

			OnSetCursorWhenMouseDown(timeAtX, true);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnBoundaryMouseDown(int mouseX, TimeSpan boundaryClicked,
			int indexOutOfBoundaryClicked)
		{
			if (BoundaryMouseDown != null)
				BoundaryMouseDown(this, mouseX, boundaryClicked, indexOutOfBoundaryClicked);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnSetCursorWhenMouseDown(TimeSpan timeAtMouseX, bool wasBoundaryClicked)
		{
			SetCursor(timeAtMouseX);
		}

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

		#endregion
	}
}
