using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SayMore.Properties;
using SayMore.Transcription.Model;

namespace SayMore.Media.Audio
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
		public Action<Exception> PlaybackErrorAction { get; set; }

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

		protected TimeRange _playbackRange;
		protected TimeSpan _boundaryMouseOver;
		protected WaveControlScrollCalculator _scrollCalculator;
		protected Timer _slideTimer;
		protected DateTime _endSlideTime;
		protected int _slidingTargetScrollOffset;
		protected bool _ignoreMouseProcessing;

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
		public void Initialize(string audioFileName)
		{
			_wasStreamCreatedHere = true;
			try
			{
				Initialize(new WaveFileReader(audioFileName));
			}
			catch
			{
				_wasStreamCreatedHere = true;
				AllowDrawing = false;
				throw;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize(WaveFileReader stream)
		{
			if (Painter != null)
				Painter.Dispose();

			WaveStream = stream;

			_playbackStream = (PlaybackStreamProvider == null ?
				WaveStream : PlaybackStreamProvider(WaveStream));

			Painter = GetNewWavePainter(stream);

			Painter.BottomReservedAreaHeight = _savedBottomReservedAreaHeight;
			Painter.BottomReservedAreaColor = _bottomReservedAreaColor;
			Painter.BottomReservedAreaBorderColor = _bottomReservedAreaBorderColor;
			Painter.BottomReservedAreaPaintAction = _bottomReservedAreaPaintAction;
			Painter.AllowRedraw = _savedAllowDrawingValue;
			Painter.ForeColor = ForeColor;
			Painter.BackColor = BackColor;
			Painter.SetPixelsPerSecond(Settings.Default.SegmentingWaveViewPixelsPerSecond);
			AutoScrollMinSize = new Size(Painter.VirtualWidth, 0);

			if (Painter.AllowRedraw)
				Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		public void CloseStream()
		{
			KillSlideTimer();

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

		/// ------------------------------------------------------------------------------------
		public void InvalidateIfNeeded(Rectangle rc)
		{
			if (rc.Width * rc.Height != 0)
				Invalidate(rc);
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
		public bool AllowDrawing
		{
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
		public void InvalidateRegionBetweenTimes(TimeRange timeRange, int additionalPixelsToInvalidate)
		{
			if (TimeRange.IsNullOrZeroLength(timeRange))
				return;

			var x = Painter.ConvertTimeToXCoordinate(timeRange.Start);
			var rc = new Rectangle(x, 0,
				Painter.ConvertTimeToXCoordinate(timeRange.End) - x + additionalPixelsToInvalidate,
				ClientSize.Height);
			InvalidateIfNeeded(rc);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void InvalidateBottomReservedArea()
		{
			if (BottomReservedAreaHeight == 0)
				return;

			var rc = ClientRectangle;
			rc.Y = rc.Bottom - BottomReservedAreaHeight;
			rc.Height = BottomReservedAreaHeight;
			InvalidateIfNeeded(rc);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void IgnoreMouseProcessing(bool ignore)
		{
			_ignoreMouseProcessing = ignore;
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

		/// ------------------------------------------------------------------------------------
		/// <summary>Returns true if the requested time is already visible. False if a scroll will be
		/// done (using the slide timer) to get there.</summary>
		/// ------------------------------------------------------------------------------------
		public bool EnsureTimeIsVisible(TimeSpan time, TimeRange timeRange, bool scrollToCenter,
			bool prepareForPlayback)
		{
			bool discardCalculator = false;

			if (_scrollCalculator == null || _scrollCalculator.TimeRange != timeRange)
			{
				KillSlideTimer();
				_scrollCalculator = new WaveControlScrollCalculator(this, timeRange, scrollToCenter);
				discardCalculator = !prepareForPlayback;
			}

			var retVal = EnsureTimeIsVisible(time);

			if (discardCalculator)
				_scrollCalculator = null;

			return retVal;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>Returns true if the requested time is already visible. False if a scroll will be
		/// done (using the slide timer) to get there.</summary>
		/// ------------------------------------------------------------------------------------
		public bool EnsureTimeIsVisible(TimeSpan time)
		{
			return EnsureXIsVisible(Painter.ConvertTimeToXCoordinate(time));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>Returns true if the requested horizontal pixel location is already visible. False
		/// if a scroll will be done (using the slide timer) to get there.</summary>
		/// ------------------------------------------------------------------------------------
		public bool EnsureXIsVisible(int x)
		{
			bool discardCalculator = false;

			if (_scrollCalculator == null)
			{
				KillSlideTimer();
				_scrollCalculator = new WaveControlScrollCalculator(this,
					new TimeRange(TimeSpan.Zero, _playbackStream.TotalTime), true);
				discardCalculator = true;
			}

			_slidingTargetScrollOffset = _scrollCalculator.ComputeTargetScrollOffset(x);

			if (discardCalculator)
				_scrollCalculator = null;

			if (_slidingTargetScrollOffset == -AutoScrollPosition.X)
				return true;

			_endSlideTime = DateTime.Now.AddMilliseconds(250);
			if (_slideTimer == null || !_slideTimer.Enabled)
			{
				Invoke((Action)(() =>
				{
					_slideTimer = new Timer();
					_slideTimer.Interval = 1;
					_slideTimer.Tick += HandleSlideTimerTick;
					_slideTimer.Start();
				}));
			}
			return false;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSlideTimerTick(object sender, EventArgs e)
		{
			var currTime = DateTime.Now;
			var newTargetX = _slidingTargetScrollOffset;

			if (currTime < _endSlideTime)
				newTargetX = (int)Math.Ceiling((-AutoScrollPosition.X + newTargetX) / 2f);
			else
				KillSlideTimer();

			SetAutoScrollPosition(newTargetX);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void SetAutoScrollPosition(int newTargetX)
		{
			AutoScrollPosition = new Point(newTargetX, 0);
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
		public void DiscardScrollCalculator()
		{
			_scrollCalculator = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns an enumeration of rectangles representing the top portion of the wave
		/// control (i.e., excluding the bottom reserved area, if any) for each of the
		/// segments.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<Rectangle> GetSegmentRectangles()
		{
			var timeRange = new TimeRange(0, 0);

			foreach (var endTime in SegmentBoundaries)
			{
				timeRange.End = endTime;
				yield return Painter.GetUpperRectangleForTimeRange(timeRange);
				timeRange.Start = endTime;
			}
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Rectangle> GetChannelDisplayRectangles()
		{
			return (Painter == null ? new Rectangle[0] :
				Painter.GetChannelDisplayRectangles(ClientRectangle));
		}

		/// ------------------------------------------------------------------------------------
		public int GetIndexOfLastBoundaryBeforeTime(TimeSpan time)
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

		/// ------------------------------------------------------------------------------------
		public TimeRange GetTimeRangeEnclosingMouseX()
		{
			var pt = PointToClient(MousePosition);
			var time = Painter.ConvertXCoordinateToTime(pt.X);

			var startTime = TimeSpan.Zero;
			foreach (var boundary in Painter.SegmentBoundaries)
			{
				if (boundary >= time)
					return (new TimeRange(startTime, boundary));

				startTime = boundary;
			}

			return new TimeRange(0, 0);
		}

		#region Playback/stop methods
		/// ------------------------------------------------------------------------------------
		public virtual void Play(TimeSpan playbackStartTime)
		{
			Play(playbackStartTime, TimeSpan.Zero);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Play(TimeRange timeRange)
		{
			Play(timeRange.Start, timeRange.End);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Play(TimeSpan playbackStartTime, TimeSpan playbackEndTime)
		{
			if (_playbackStream == null || (_playbackStream.WaveFormat.BitsPerSample == 32 &&
				_playbackStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat))
			{
				return;
			}

			_playbackRange = new TimeRange(playbackStartTime, playbackEndTime);

			if (_playbackRange.Start < TimeSpan.Zero)
				_playbackRange.Start = TimeSpan.Zero;

			if (_playbackRange.DurationSeconds.Equals(0))
			{
				_playbackRange.End = WaveStream.TotalTime;
				EnsureTimeIsVisible(_playbackRange.Start);
			}
			else
				EnsureTimeIsVisible(_playbackRange.Start, _playbackRange, true, true);

			AudioUtils.NAudioExceptionThrown += HandleNAudioExceptionThrown;

			var waveOutProvider = new SampleChannel(_playbackRange.End == TimeSpan.Zero || _playbackRange.End == WaveStream.TotalTime ?
				new WaveSegmentStream(_playbackStream, playbackStartTime) :
				new WaveSegmentStream(_playbackStream, playbackStartTime, playbackEndTime - playbackStartTime));

			waveOutProvider.PreVolumeMeter += HandlePlaybackMetering;

			try
			{
				_waveOut = new WaveOut();
				_waveOut.DesiredLatency = 100;
				_waveOut.Init(new SampleToWaveProvider(waveOutProvider));
				_waveOut.PlaybackStopped += delegate { Stop(); };
				_waveOut.Play();
				OnPlaybackStarted(playbackStartTime, playbackEndTime);
			}
			catch (MmException)
			{
				_waveOut = null;
				throw;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleNAudioExceptionThrown(Exception exception)
		{
			try
			{
				Stop();
			}
			catch { }

			if (PlaybackErrorAction != null)
				PlaybackErrorAction(exception);
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
			if (_playbackStream.CurrentTime == (_playbackRange.End > TimeSpan.Zero ? _playbackRange.End : WaveStream.TotalTime))
			{
				SetCursor(_playbackRange.End);
				if (_waveOut != null)
				{
					// We're using a WaveSegmentStream which never gets a PlaybackStopped
					// event on the WaveOut, so we have to force it here.
					_waveOut.Stop();
				}
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
			AudioUtils.NAudioExceptionThrown -= HandleNAudioExceptionThrown;

			if (_waveOut == null)
				return;

			_waveOut.Stop();
			TimeSpan stopped;
			try
			{
				stopped = _playbackStream.CurrentTime;
			}
			catch
			{
				// Might have already stopped and been disposed
				stopped = _playbackRange.End;
			}

			_waveOut.Dispose();
			_waveOut = null;
			_scrollCalculator = null;
			OnPlaybackStopped(_playbackRange.Start, stopped);
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
				SetAutoScrollPosition(Painter.VirtualWidth - (ClientSize.Width + 10));
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
				_boundaryMouseOver = GetBoundaryNearX(e.X);
			else if (!_ignoreMouseProcessing)
				OnMouseMoveEx(e, GetBoundaryNearX(e.X));
		}

		/// ------------------------------------------------------------------------------------
		private TimeSpan GetBoundaryNearX(int x)
		{
			if (SegmentBoundaries == null)
				return default(TimeSpan);

			var timeAtMouseA = (x <= 4 ? TimeSpan.Zero : GetTimeFromX(x - 4));
			var timeAtMouseB = GetTimeFromX(x + 4);
			return SegmentBoundaries.FirstOrDefault(b => b >= timeAtMouseA && b <= timeAtMouseB);
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

			if (WaveStream == null)
				return;

			var boundaryMouseOver = GetBoundaryNearX(e.X);

			if (IsPlaying)
				Stop();

			var timeAtX = GetTimeFromX(e.X);
			if (timeAtX > WaveStream.TotalTime)
				timeAtX = WaveStream.TotalTime;

			if (boundaryMouseOver == default(TimeSpan))
			{
				if (SetCursorAtTimeOnMouseClick != null)
					timeAtX = SetCursorAtTimeOnMouseClick(this, timeAtX);

				OnSetCursorWhenMouseDown(timeAtX, false);
				return;
			}

			OnBoundaryMouseDown(e.X, boundaryMouseOver, GetIndexOfBoundary(boundaryMouseOver));

			timeAtX = (SetCursorAtTimeOnMouseClick == null ? boundaryMouseOver :
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
