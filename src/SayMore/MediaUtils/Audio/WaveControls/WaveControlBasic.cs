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

		protected float _zoomPercentage = 100f;
		protected WaveStream _playbackStream;
		protected WaveOutEvent _waveOut;
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
		private bool _stopping;

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
			_prevClientSize = ClientSize;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_waveOut != null)
					Stop();

				CloseAndDisposeWaveStreamIfNeeded();

				PostPaintAction = null;
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		private void CloseAndDisposeWaveStreamIfNeeded()
		{
			if (WaveStream != null)
			{
				WaveStream.Close();
				WaveStream.Dispose();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void LoadFile(string audioFileName)
		{
			try
			{
				LoadFile(new WaveFileReader(audioFileName));
			}
			catch
			{
				AllowDrawing = false;
				throw;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void LoadFile(WaveFileReader stream)
		{
			if (Painter != null)
				Painter.Dispose();

			CloseAndDisposeWaveStreamIfNeeded();

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

			SetZoom();

			if (Painter.AllowRedraw)
				Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		public void CloseStream()
		{
			KillSlideTimer();

			if (Painter != null)
			{
				Painter.BottomReservedAreaPaintAction = null;
				Painter.Dispose();
				Painter = null;
			}

			CloseAndDisposeWaveStreamIfNeeded();
			WaveStream = null;

			_playbackStream = null;
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
		public Func<WaveFormat, string> FormatNotSupportedMessageProvider
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
		public bool IsPlaying
		{
			get { return _waveOut != null; }
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsPlayingToEndOfMedia
		{
			get { return IsPlaying && _playbackRange.End == WaveStream.TotalTime; }
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
		//	get { return base.AutoScrollMinSize; }
		//	set
		//	{
		//		base.AutoScrollMinSize = new Size(value.Width, value.Height);

		//		if (_painter != null)
		//		{
		//			this.SetWindowRedraw(false);
		//			_painter.SetVirtualWidth(Math.Max(ClientSize.Width, value.Width));
		//			Invalidate();
		//			this.SetWindowRedraw(true);
		//		}
		//	}
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
		public void SetCursor(int cursorX)
		{
			if (Painter == null)
				return;

			SetCursorInternal(Painter.ConvertXCoordinateToTime(cursorX), cursorX, true);
		}

		/// ------------------------------------------------------------------------------------
		public void SetCursor(TimeSpan cursorTime, bool ensureCursorIsVisible = true)
		{
			if (Painter == null)
				return;

			SetCursorInternal(cursorTime, Painter.ConvertTimeToXCoordinate(cursorTime), ensureCursorIsVisible);
		}

		/// ------------------------------------------------------------------------------------
		private void SetCursorInternal(TimeSpan cursorTime, int cursorX, bool ensureCursorIsVisible)
		{
			cursorTime = cursorTime < TimeSpan.Zero ? TimeSpan.Zero : cursorTime;
			Painter.SetCursor(cursorTime);

			if (CursorTimeChanged != null)
				CursorTimeChanged(this, cursorTime);

			if (ensureCursorIsVisible)
				EnsureXIsVisible(cursorX);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetCursorTime()
		{
			return (Painter == null ? TimeSpan.Zero : Painter.CursorTime);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetTimeFromX(int dx)
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
			if (Painter == null)
			{
				// This can happen when regenerating the OralAnnotation file while playing.
				KillSlideTimer();
				return;
			}

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
		public int GetSegmentForX(int dx)
		{
			var timeAtX = GetTimeFromX(dx);

			int segNumber = 0;

			foreach (var boundary in SegmentBoundaries)
			{
				if (timeAtX <= boundary)
					return segNumber;

				segNumber++;
			}

			return -1;
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
		public void Play(TimeSpan playbackStartTime)
		{
			Play(playbackStartTime, TimeSpan.Zero);
		}

		/// ------------------------------------------------------------------------------------
		public void Play(TimeRange timeRange)
		{
			Play(timeRange.Start, timeRange.End);
		}

		/// ------------------------------------------------------------------------------------
		public void Play(TimeSpan playbackStartTime, TimeSpan playbackEndTime)
		{
			if (_waveOut != null && _waveOut.PlaybackState == PlaybackState.Playing)
				throw new InvalidOperationException("Can't call play while playing.");

			if (_playbackStream == null || (_playbackStream.WaveFormat.BitsPerSample == 32 &&
				_playbackStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat))
			{
				return;
			}

			SetCursor(playbackStartTime);

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
				_waveOut = new WaveOutEvent();
				_waveOut.DesiredLatency = 100;
				_waveOut.NumberOfBuffers = 20;
				_waveOut.Init(new SampleToWaveProvider(waveOutProvider));
				_waveOut.PlaybackStopped += WaveOutOnPlaybackStopped;
				_waveOut.Play();
				OnPlaybackStarted(playbackStartTime, playbackEndTime);
			}
			catch (MmException exception)
			{
				if (_waveOut != null)
					_waveOut.Dispose();
				_waveOut = null;
				DesktopAnalytics.Analytics.ReportException(exception);
				//  throw;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void WaveOutOnPlaybackStopped(object sender, StoppedEventArgs stoppedEventArgs)
		{
			Stop();
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
		protected void HandlePlaybackMetering(object sender, StreamVolumeEventArgs e)
		{
			if (PlaybackUpdate != null)
				PlaybackUpdate(this, _playbackStream.CurrentTime, _playbackStream.TotalTime);

			if (_playbackStream.CurrentTime >= (_playbackRange.End > TimeSpan.Zero ? _playbackRange.End : WaveStream.TotalTime))
				SetCursor(_playbackRange.End);
			else
				SetCursor(_playbackStream.CurrentTime);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Stop()
		{
			AudioUtils.NAudioExceptionThrown -= HandleNAudioExceptionThrown;

			if (_waveOut == null || _stopping)
				return;

			_stopping = true;

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

			_waveOut.PlaybackStopped += (sender, args) => OnPlaybackStopped((WaveOutEvent)sender, _playbackRange.Start, stopped);
			// If _waveOut.PlaybackStopped fires right here, we'll actually get both event handlers.
			// We'll ignore the one that gets us back into this method because _stopping == true.

			try
			{
				_waveOut.PlaybackStopped -= WaveOutOnPlaybackStopped;

				bool wasPlaying = _waveOut.PlaybackState == PlaybackState.Playing;

				_waveOut.Stop();

				if (!wasPlaying)
					OnPlaybackStopped(_waveOut, _playbackRange.Start, stopped);
			}
			catch (NullReferenceException)
			{
			}
			catch (ObjectDisposedException)
			{
			}

			_stopping = false;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected virtual void OnPlaybackStopped(WaveOutEvent sender, TimeSpan startTime, TimeSpan endTime)
		{
			sender.Dispose();

			if (_waveOut == sender)
			{
				SetCursor(_playbackStream.CurrentTime);
				_waveOut = null;
				_scrollCalculator = null;

				if (PlaybackStopped != null)
					PlaybackStopped(this, startTime, endTime);
			}
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
				if (value.Equals(_zoomPercentage) || value < 100f)
					return;

				_zoomPercentage = value;
				SetZoom();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void SetZoom()
		{
			// SP-907: Object reference not set (ZoomPercentage is being set before Initialize is called)
			if (_playbackStream == null)
				return;

			var percentage = _zoomPercentage / 100;
			var defaultWidth = _playbackStream.TotalTime.TotalSeconds * Settings.Default.SegmentingWaveViewPixelsPerSecond;
			var adjustedWidth = (int)(Math.Round(defaultWidth * percentage, MidpointRounding.AwayFromZero));
			adjustedWidth = Math.Max(adjustedWidth, ClientSize.Width);
			AutoScrollMinSize = new Size(adjustedWidth, ClientSize.Height);
			Painter.SetPixelsPerSecond(adjustedWidth / _playbackStream.TotalTime.TotalSeconds);
			Invalidate();
		}
		#endregion
	}
}
