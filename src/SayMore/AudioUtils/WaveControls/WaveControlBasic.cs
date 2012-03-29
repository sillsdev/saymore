using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Palaso.Progress;
using SayMore.Properties;
using SilTools;

namespace SayMore.Media
{
	/// ----------------------------------------------------------------------------------------
	public class WaveControlBasic : UserControl
	{
		public Func<WaveStream, WaveStream> PlaybackStreamProvider;

		public delegate TimeSpan SetCurorAtTimeOnMouseClickHandler(WaveControlBasic ctrl, TimeSpan timeAtMouseX);
		public delegate void PlaybackEventHandler(WaveControlBasic ctrl, TimeSpan time1, TimeSpan time2);
		public delegate void CursorTimeChangedHandler(WaveControlBasic ctrl, TimeSpan cursorTime);
		public delegate void BoundaryMouseDownHandler(WaveControlBasic ctrl, int mouseX,
			TimeSpan boundary, int boundaryNumber);

		public event SetCurorAtTimeOnMouseClickHandler SetCursorAtTimeOnMouseClick;
		public event PlaybackEventHandler PlaybackStarted;
		public event PlaybackEventHandler PlaybackStopped;
		public event PlaybackEventHandler PlaybackUpdate;
		public event BoundaryMouseDownHandler BoundaryMouseDown;
		public event CursorTimeChangedHandler CursorTimeChanged;
		public Action<PaintEventArgs> PostPaintAction;

		protected float _zoomPercentage;
		protected bool _wasStreamCreatedHere;
		protected WavePainterBasic _painter;
		protected WaveStream _waveStream;
		protected WaveStream _playbackStream;
		protected WaveOut _waveOut;
		protected Size _prevClientSize;
		protected bool _savedAllowDrawingValue = true;
		protected int _savedBottomReservedAreaHeight;
		protected Form _owningForm;
		protected Color _bottomReservedAreaColor;
		protected Color _bottomReservedAreaBorderColor;
		protected Action<PaintEventArgs, Rectangle> _bottomReservedAreaPaintAction;

		protected TimeSpan _playbackStartTime;
		protected TimeSpan _playbackEndTime;
		protected TimeSpan _boundaryMouseOver;

		/// ------------------------------------------------------------------------------------
		public WaveControlBasic()
		{
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			ResizeRedraw = true;
			AutoScroll = true;
			ForeColor = SystemColors.WindowText;
			BackColor = SystemColors.Window;
			_zoomPercentage = 100f;
			_prevClientSize = ClientSize;
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
		public virtual void Initialize(WaveFileReader stream)
		{
			if (_painter != null)
				_painter.Dispose();

			_waveStream = stream;

			_playbackStream = (PlaybackStreamProvider == null ?
				_waveStream : PlaybackStreamProvider(_waveStream));

			_painter = GetNewWavePainter(stream);
			InternalInitialize();
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Initialize(IEnumerable<float> samples, TimeSpan totalTime)
		{
			if (_painter != null)
				_painter.Dispose();

			_painter = GetNewWavePainter(samples, totalTime);
			InternalInitialize();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void InternalInitialize()
		{
			_painter.BottomReservedAreaHeight = _savedBottomReservedAreaHeight;
			_painter.BottomReservedAreaColor = _bottomReservedAreaColor;
			_painter.BottomReservedAreaBorderColor = _bottomReservedAreaBorderColor;
			_painter.BottomReservedAreaPaintAction = _bottomReservedAreaPaintAction;
			_painter.AllowRedraw = _savedAllowDrawingValue;
			_painter.ForeColor = ForeColor;
			_painter.BackColor = BackColor;
			_painter.SetPixelsPerSecond(Settings.Default.SegmentingWaveViewPixelsPerSecond);
			AutoScrollMinSize = new Size(_painter.VirtualWidth, 0);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void CloseStream()
		{
			if (_wasStreamCreatedHere && _waveStream != null)
			{
				_painter.Dispose();
				_painter = null;
				_waveStream.Close();
				_waveStream.Dispose();
				_waveStream = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual WavePainterBasic GetNewWavePainter(IEnumerable<float> samples, TimeSpan totalTime)
		{
			return new WavePainterBasic(this, samples, totalTime);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual WavePainterBasic GetNewWavePainter(WaveFileReader stream)
		{
			return new WavePainterBasic(this, stream);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int BottomReservedAreaHeight
		{
			get { return _savedBottomReservedAreaHeight; }
			set
			{
				_savedBottomReservedAreaHeight = value;
				if (_painter != null)
					_painter.BottomReservedAreaHeight = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public Rectangle BottomReservedAreaRectangle
		{
			get
			{
				return new Rectangle(0, ClientRectangle.Bottom - BottomReservedAreaHeight,
					ClientSize.Width, BottomReservedAreaHeight);
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual Color BottomReservedAreaColor
		{
			get { return _bottomReservedAreaColor; }
			set
			{
				_bottomReservedAreaColor = value;
				if (_painter != null)
					_painter.BottomReservedAreaColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual Color BottomReservedAreaBorderColor
		{
			get { return _bottomReservedAreaBorderColor; }
			set
			{
				_bottomReservedAreaBorderColor = value;
				if (_painter != null)
					_painter.BottomReservedAreaBorderColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Action<PaintEventArgs, Rectangle> BottomReservedAreaPaintAction
		{
			get { return _bottomReservedAreaPaintAction; }
			set
			{
				_bottomReservedAreaPaintAction = value;
				if (_painter != null)
					_painter.BottomReservedAreaPaintAction = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool AllowDrawing
		{
			get { return _savedAllowDrawingValue; }
			set
			{
				_savedAllowDrawingValue = value;
				if (_painter != null)
					_painter.AllowRedraw = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual Func<WaveFormat, string> FormatNotSupportedMessageProvider
		{
			get { return (_painter == null ? null : _painter.FormatNotSupportedMessageProvider); }
			set
			{
				if (_painter != null)
					_painter.FormatNotSupportedMessageProvider = value;
			}
		}

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

		///// ------------------------------------------------------------------------------------
		//public new Size AutoScrollMinSize
		//{
		//    get { return base.AutoScrollMinSize; }
		//    set
		//    {
		//        base.AutoScrollMinSize = new Size(value.Width, value.Height);

		//        if (_painter != null)
		//        {
		//            Utils.SetWindowRedraw(this, false);
		//            _painter.SetVirtualWidth(Math.Max(ClientSize.Width, value.Width));
		//            Invalidate();
		//            Utils.SetWindowRedraw(this, true);
		//        }
		//    }
		//}

		#endregion

		/// ------------------------------------------------------------------------------------
		public virtual void InvalidateBottomReservedArea()
		{
			if (BottomReservedAreaHeight == 0)
				return;

			var rc = ClientRectangle;
			rc.Y = rc.Bottom - BottomReservedAreaHeight;
			rc.Height = BottomReservedAreaHeight;
			Invalidate(rc);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetCursor(int cursorX)
		{
			if (_painter == null)
				return;

			_painter.SetCursor(cursorX);
			EnsureXIsVisible(cursorX);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetCursor(TimeSpan cursorTime)
		{
			SetCursor(cursorTime, true);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetCursor(TimeSpan cursorTime, bool ensureCursorIsVisible)
		{
			OnCursorTimeChanged(cursorTime < TimeSpan.Zero ? TimeSpan.Zero : cursorTime);

			if (cursorTime >= TimeSpan.Zero && ensureCursorIsVisible)
				EnsureXIsVisible(_painter.ConvertTimeToXCoordinate(cursorTime));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnCursorTimeChanged(TimeSpan cursorTime)
		{
			if (_painter == null)
				return;

			_painter.SetCursor(cursorTime);

			if (CursorTimeChanged != null)
				CursorTimeChanged(this, cursorTime);
		}

		/// ------------------------------------------------------------------------------------
		public virtual TimeSpan GetCursorTime()
		{
			return (_painter == null ? TimeSpan.Zero : _painter.CursorTime);
		}

		/// ------------------------------------------------------------------------------------
		public virtual TimeSpan GetTimeFromX(int dx)
		{
			return (_painter == null ? TimeSpan.Zero : _painter.ConvertXCoordinateToTime(dx));
		}

		/// ------------------------------------------------------------------------------------
		public void EnsureTimeIsMoreOrLessCentered(TimeSpan time)
		{
			var currentPosition = _painter.ConvertTimeToXCoordinate(time);
			if (currentPosition > ClientSize.Width * 0.15 && currentPosition < ClientSize.Width * 0.85)
				return;

			var secondsInVisibleArea = (ClientSize.Width / _painter.PixelPerMillisecond) / 1000;
			var halfTheVisibleTime = TimeSpan.FromSeconds(secondsInVisibleArea / 2);
			EnsureRangeIsVisible(time - halfTheVisibleTime, time + halfTheVisibleTime);
		}

		/// ------------------------------------------------------------------------------------
		public void EnsureRangeIsVisible(TimeSpan start, TimeSpan end)
		{
			Utils.SetWindowRedraw(this, false);
			EnsureXIsVisible(_painter.ConvertTimeToXCoordinate(end) + 3, true);
			EnsureXIsVisible(_painter.ConvertTimeToXCoordinate(start) - 3);
			Utils.SetWindowRedraw(this, true);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void EnsureXIsVisible(int dx)
		{
			EnsureXIsVisible(dx, true);
		}

		private Timer _slideTimer;

		/// ------------------------------------------------------------------------------------
		public virtual void EnsureXIsVisible(int dx, bool useSlideEffect)
		{
			if (dx >= 0 && dx <= ClientSize.Width || _slideTimer != null)
				return;

			var newX = -AutoScrollPosition.X + (dx < 0 ? dx : (dx - ClientSize.Width));

			if (!useSlideEffect)
			{
				AutoScrollPosition = new Point(newX, AutoScrollPosition.Y);
				_painter.SetOffsetOfLeftEdge(-AutoScrollPosition.X);
				return;
			}

			//var interval = (350f / Math.Abs(newX - AutoScrollPosition.X)) * 1000;

			_slideTimer = new Timer();
			_slideTimer.Tag = newX;
			_slideTimer.Interval = 1;
			_slideTimer.Tick += _slideTimer_Tick;
			_slideTimer.Start();
		}

		/// ------------------------------------------------------------------------------------
		void _slideTimer_Tick(object sender, EventArgs e)
		{
			int newX = (int)_slideTimer.Tag;
			int incAmount = (newX < -AutoScrollPosition.X ? -35 : 35);

			int x = Math.Min(-AutoScrollPosition.X + incAmount, newX);
			AutoScrollPosition = new Point(x, AutoScrollPosition.Y);
			_painter.SetOffsetOfLeftEdge(-AutoScrollPosition.X);

			if (x != newX)
				return;

			_slideTimer.Stop();
			_slideTimer.Tick -= _slideTimer_Tick;
			_slideTimer.Dispose();
			_slideTimer = null;
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
		public IEnumerable<Rectangle> GetChannelDisplayRectangles()
		{
			return (_painter == null ? new Rectangle[0] :
				_painter.GetChannelDisplayRectangles(ClientRectangle));
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
			if (_playbackStream == null || (_playbackStream.WaveFormat.BitsPerSample == 32 &&
				_playbackStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat))
			{
				return;
			}

			_playbackStartTime = playbackStartTime;
			_playbackEndTime = playbackEndTime;

			if (_playbackStartTime < TimeSpan.Zero)
				_playbackStartTime = TimeSpan.Zero;

			if (_playbackEndTime <= _playbackStartTime)
				_playbackEndTime = _waveStream.TotalTime;

			var waveOutProvider = new SampleChannel(_playbackEndTime == TimeSpan.Zero || _playbackEndTime == _waveStream.TotalTime ?
				new WaveSegmentStream(_playbackStream, playbackStartTime) :
				new WaveSegmentStream(_playbackStream, playbackStartTime, playbackEndTime - playbackStartTime));

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
			if (_playbackStream.CurrentTime == (_playbackEndTime > TimeSpan.Zero ? _playbackEndTime : _waveStream.TotalTime))
			{
				SetCursor(_playbackEndTime);
				_waveOut.Stop();
				return;
			}

			var dx = _painter.ConvertTimeToXCoordinate(_playbackStream.CurrentTime);
			dx += (dx < 0 ? -10 : 10);
			EnsureXIsVisible(dx);

			OnInternalPlaybackUpdate(_playbackStream.CurrentTime, _playbackStream.TotalTime);
			SetCursor(_playbackStream.CurrentTime);
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
			OnPlaybackStopped(_playbackStartTime, _playbackStream.CurrentTime);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected virtual void OnPlaybackStopped(TimeSpan startTime, TimeSpan endTime)
		{
			if (PlaybackStopped != null)
				PlaybackStopped(this, startTime, endTime);
		}

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (_painter != null && _painter.ConvertTimeToXCoordinate(_waveStream.TotalTime) +
				WavePainterBasic.kRightDisplayPadding < ClientSize.Width)
			{
				AutoScrollPosition = new Point(_painter.VirtualWidth - (ClientSize.Width + 10), 0);
				_painter.SetOffsetOfLeftEdge(-AutoScrollPosition.X);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (PostPaintAction != null)
				PostPaintAction(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnScroll(ScrollEventArgs e)
		{
			if (e.OldValue != e.NewValue && _painter != null && e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
				_painter.SetOffsetOfLeftEdge(e.NewValue);

			base.OnScroll(e);
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
			if (timeAtX > _waveStream.TotalTime)
				timeAtX = _waveStream.TotalTime;

			if (_boundaryMouseOver == default(TimeSpan))
			{
				if (SetCursorAtTimeOnMouseClick != null)
					timeAtX = SetCursorAtTimeOnMouseClick(this, timeAtX);

				OnSetCursorWhenMouseDown(timeAtX, false);
				return;
			}

			OnBoundaryMouseDown(e.X, _boundaryMouseOver, GetIndexOfBoundary(_boundaryMouseOver));

			timeAtX = (SetCursorAtTimeOnMouseClick == null ? _boundaryMouseOver :
				SetCursorAtTimeOnMouseClick(this, timeAtX));

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
			SetCursor(timeAtMouseX, false);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void DrawBoundary(Graphics g, int x, int y, int height)
		{
			if (_painter != null)
				_painter.DrawBoundary(g, x, y, height);
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
			var width = Math.Max(ClientSize.Width, AutoScrollPosition.X);
			var percentage = _zoomPercentage / 100;
			var adjustedWidth = (int)(Math.Round(width * percentage, MidpointRounding.AwayFromZero));
			AutoScrollMinSize = new Size(Math.Max(adjustedWidth, ClientSize.Width), ClientSize.Height);
			//HandleResize(null, null);
		}

		#endregion
	}
}
