using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SayMore.Properties;

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

		public virtual WavePainterBasic Painter { get; private set; }
		public WaveStream WaveStream { get; private set; }

		protected float _zoomPercentage;
		protected bool _wasStreamCreatedHere;
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
		protected WaveControlScrollCalculator _scrollCalculator;
		protected Timer _slideTimer;
		protected DateTime _endSlideTime;
		protected int _slidingTargetScrollOffset;

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

			if (_wasStreamCreatedHere && WaveStream != null)
			{
				WaveStream.Close();
				WaveStream.Dispose();
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
			if (Painter != null)
				Painter.Dispose();

			WaveStream = stream;

			_playbackStream = (PlaybackStreamProvider == null ?
				WaveStream : PlaybackStreamProvider(WaveStream));

			Painter = GetNewWavePainter(stream);
			InternalInitialize();
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Initialize(IEnumerable<float> samples, TimeSpan totalTime)
		{
			if (Painter != null)
				Painter.Dispose();

			Painter = GetNewWavePainter(samples, totalTime);
			InternalInitialize();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void InternalInitialize()
		{
			Painter.BottomReservedAreaHeight = _savedBottomReservedAreaHeight;
			Painter.BottomReservedAreaColor = _bottomReservedAreaColor;
			Painter.BottomReservedAreaBorderColor = _bottomReservedAreaBorderColor;
			Painter.BottomReservedAreaPaintAction = _bottomReservedAreaPaintAction;
			Painter.AllowRedraw = _savedAllowDrawingValue;
			Painter.ForeColor = ForeColor;
			Painter.BackColor = BackColor;
			Painter.SetPixelsPerSecond(Settings.Default.SegmentingWaveViewPixelsPerSecond);
			AutoScrollMinSize = new Size(Painter.VirtualWidth, 0);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void CloseStream()
		{
			if (_wasStreamCreatedHere && WaveStream != null)
			{
				Painter.Dispose();
				Painter = null;
				WaveStream.Close();
				WaveStream.Dispose();
				WaveStream = null;
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
				if (Painter != null)
					Painter.BottomReservedAreaHeight = value;
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
				if (Painter != null)
					Painter.BottomReservedAreaColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual Color BottomReservedAreaBorderColor
		{
			get { return _bottomReservedAreaBorderColor; }
			set
			{
				_bottomReservedAreaBorderColor = value;
				if (Painter != null)
					Painter.BottomReservedAreaBorderColor = value;
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
				if (Painter != null)
					Painter.BottomReservedAreaPaintAction = value;
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
				if (Painter != null)
					Painter.AllowRedraw = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual Func<WaveFormat, string> FormatNotSupportedMessageProvider
		{
			get { return (Painter == null ? null : Painter.FormatNotSupportedMessageProvider); }
			set
			{
				if (Painter != null)
					Painter.FormatNotSupportedMessageProvider = value;
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
			get { return (Painter != null ? Painter.SegmentBoundaries : new TimeSpan[] { }); }
			set
			{
				if (Painter != null)
					Painter.SegmentBoundaries = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set
			{
				if (Painter != null)
					Painter.ForeColor = value;

				base.ForeColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				if (Painter != null)
					Painter.BackColor = value;

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
		public void InvalidateRegionBetweenTimes(TimeSpan start, TimeSpan end)
		{
			if (start == end)
				return;

			Invalidate(new Rectangle(Painter.ConvertTimeToXCoordinate(start),
				0, Painter.ConvertTimeToXCoordinate(end), ClientSize.Height));
		}

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
			if (Painter == null)
				return;

			Painter.SetCursor(cursorX);
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
				EnsureXIsVisible(Painter.ConvertTimeToXCoordinate(cursorTime));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnCursorTimeChanged(TimeSpan cursorTime)
		{
			if (Painter == null)
				return;

			Painter.SetCursor(cursorTime);

			if (CursorTimeChanged != null)
				CursorTimeChanged(this, cursorTime);
		}

		/// ------------------------------------------------------------------------------------
		public virtual TimeSpan GetCursorTime()
		{
			return (Painter == null ? TimeSpan.Zero : Painter.CursorTime);
		}

		/// ------------------------------------------------------------------------------------
		public virtual TimeSpan GetTimeFromX(int dx)
		{
			return (Painter == null ? TimeSpan.Zero : Painter.ConvertXCoordinateToTime(dx));
		}

//        /// ------------------------------------------------------------------------------------
//        /// <summary>
//        /// Ensure that the view is scrolled so that both the start and end times are visible
//        /// (and not jammed right up against the edge of the view) if possible. If they are not
//        /// both visible and the requested range is narrow enough to fit in the view, then
//        /// scroll so that the start is near the left edge of the view. If the requested range
//        /// is too wide to fit, then the favorStart parameter determines whether the start is
//        /// visible (true) or the end is visible (false).
//        /// </summary>
//        /// ------------------------------------------------------------------------------------
//        public void EnsureRangeIsVisible(TimeSpan start, TimeSpan end, bool center,
//            bool favorStart = true)
//        {
//            Debug.Assert(start <= end);
//            var startX = Painter.ConvertTimeToXCoordinate(start) - 3;
//            var endX = Painter.ConvertTimeToXCoordinate(end) + 3;
//            if (startX >= 0 && endX <= ClientRectangle.Right)
//                return;

//            if (endX - startX <= ClientRectangle.Width || favorStart)
//            {
//                if (center)
//                    EnsureXIsVisible((startX + endX) / 2);
//                else if (startX <
//                    EnsureXIsVisible((favorStart ? startX : endX), false);
//            }
//            else
//                EnsureXIsVisible(endX - ClientRectangle.Width / 2 + 3, center);
//        }

//        /// ------------------------------------------------------------------------------------
//        public virtual void EnsureXIsVisible(int x, bool scrollXToMiddle = true,
//            float startMarginPercent = 15, float endMarginPercent = 85, bool useSlideEffect = true)
//        {
//            int minX = (int)(ClientSize.Width * startMarginPercent / 100);
//            int maxX = (int)(ClientSize.Width * endMarginPercent / 100);

//            if (x >= minX && x <= maxX)
//                return;

//            useSlideEffect = true;

//            if (_slideTimer != null)
//                KillSlideTimer();

////			_slidingTargetScrollOffset = -AutoScrollPosition.X + (dx < 0 ? dx : (dx - ClientSize.Width));
//            _slidingTargetScrollOffset = -AutoScrollPosition.X + (x - (scrollXToMiddle ? ClientSize.Width / 2 : (x > maxX ? maxX : minX)));

//            if (!useSlideEffect)
//            {
//                AutoScrollPosition = new Point(_slidingTargetScrollOffset, AutoScrollPosition.Y);
//                Painter.SetOffsetOfLeftEdge(-AutoScrollPosition.X);
//                return;
//            }

//            _endSlideTime = DateTime.Now.AddMilliseconds(250);
//            _slideTimer = new Timer();
//            _slideTimer.Interval = 1;
//            _slideTimer.Tick += _slideTimer_Tick;
//            _slideTimer.Start();
//        }

		/// ------------------------------------------------------------------------------------
		public void EnsureTimeIsVisible(TimeSpan time, TimeSpan rangeStartTime,
			TimeSpan rangeEndTime, bool scrollToCenter, bool prepareForPlayback)
		{
			bool discardCalculator = false;
			if (_scrollCalculator == null)
			{
				_scrollCalculator = new WaveControlScrollCalculator(this, rangeStartTime, rangeEndTime, scrollToCenter);
				discardCalculator = !prepareForPlayback;
			}
			EnsureTimeIsVisible(time);
			if (discardCalculator)
				_scrollCalculator = null;
		}

		/// ------------------------------------------------------------------------------------
		public void EnsureTimeIsVisible(TimeSpan time)
		{
			EnsureXIsVisible(Painter.ConvertTimeToXCoordinate(time));
		}

		/// ------------------------------------------------------------------------------------
		public void EnsureXIsVisible(int x)
		{
			bool discardCalculator = false;
			if (_scrollCalculator == null)
			{
				_scrollCalculator = new WaveControlScrollCalculator(this, TimeSpan.Zero,
					_playbackStream.TotalTime, true);
				discardCalculator = true;
			}

			_slidingTargetScrollOffset = _scrollCalculator.ComputeTargetScrollOffset(x);

			if (discardCalculator)
				_scrollCalculator = null;

			if (_slidingTargetScrollOffset == -AutoScrollPosition.X)
				return;

			_endSlideTime = DateTime.Now.AddMilliseconds(250);
			KillSlideTimer();
			_slideTimer = new Timer();
			_slideTimer.Interval = 1;
			_slideTimer.Tick += HandleSlideTimerTick;
			_slideTimer.Start();
		}

		/// ------------------------------------------------------------------------------------
		void HandleSlideTimerTick(object sender, EventArgs e)
		{
			var currTime = DateTime.Now;
			var newTargetX = _slidingTargetScrollOffset;

			if (currTime < _endSlideTime)
				newTargetX = (int)Math.Ceiling((-AutoScrollPosition.X + newTargetX) / 2f);
			else
				KillSlideTimer();

			AutoScrollPosition = new Point(newTargetX, AutoScrollPosition.Y);
			Painter.SetOffsetOfLeftEdge(-AutoScrollPosition.X);
		}

		/// ------------------------------------------------------------------------------------
		private void KillSlideTimer()
		{
			if (_slideTimer == null)
				return;
			_slideTimer.Stop();
			_slideTimer.Dispose();
			_slideTimer = null;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Rectangle> GetSegmentRectangles()
		{
			var startTime = TimeSpan.Zero;

			foreach (var endTime in SegmentBoundaries)
			{
				yield return Painter.GetRectangleForTimeRange(startTime, endTime);
				startTime = endTime;
			}
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Rectangle> GetChannelDisplayRectangles()
		{
			return (Painter == null ? new Rectangle[0] :
				Painter.GetChannelDisplayRectangles(ClientRectangle));
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
			{
				_playbackEndTime = WaveStream.TotalTime;
				EnsureTimeIsVisible(_playbackStartTime);
			}
			else
				EnsureTimeIsVisible(_playbackStartTime, _playbackStartTime, _playbackEndTime, true, true);

			var waveOutProvider = new SampleChannel(_playbackEndTime == TimeSpan.Zero || _playbackEndTime == WaveStream.TotalTime ?
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
			if (_playbackStream.CurrentTime == (_playbackEndTime > TimeSpan.Zero ? _playbackEndTime : WaveStream.TotalTime))
			{
				SetCursor(_playbackEndTime);
				_waveOut.Stop();
				return;
			}

			OnInternalPlaybackUpdate(_playbackStream.CurrentTime, _playbackStream.TotalTime);
			SetCursor(_playbackStream.CurrentTime, true);
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
			_scrollCalculator = null;
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

			if (Painter != null && Painter.ConvertTimeToXCoordinate(WaveStream.TotalTime) +
				WavePainterBasic.kRightDisplayPadding < ClientSize.Width)
			{
				AutoScrollPosition = new Point(Painter.VirtualWidth - (ClientSize.Width + 10), 0);
				Painter.SetOffsetOfLeftEdge(-AutoScrollPosition.X);
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
			if (e.OldValue != e.NewValue && Painter != null && e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
				Painter.SetOffsetOfLeftEdge(e.NewValue);

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
			if (timeAtX > WaveStream.TotalTime)
				timeAtX = WaveStream.TotalTime;

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
			if (Painter != null)
				Painter.DrawBoundary(g, x, y, height);
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
