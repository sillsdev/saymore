using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
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
		protected WaveStream _playbackStream;
		protected WaveOut _waveOut;
		protected int _virtualWidth;
		protected Size _prevClientSize;
		protected bool _savedAllowDrawingValue = true;

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
			_painter.AllowRedraw = _savedAllowDrawingValue;
			_painter.ForeColor = ForeColor;
			_painter.BackColor = BackColor;
			_painter.SetVirtualWidth(Math.Max(ClientSize.Width, AutoScrollMinSize.Width));
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
		public virtual bool AllowDrawing
		{
			get { return (_painter == null ? _savedAllowDrawingValue : _painter.AllowRedraw); }
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
			_virtualWidth = width;

			var percentage = _zoomPercentage / 100;
			var adjustedWidth = (int)(Math.Round(width * percentage, MidpointRounding.AwayFromZero));
			AutoScrollMinSize = new Size(Math.Max(adjustedWidth, ClientSize.Width), ClientSize.Height);

			if (AutoScrollMinSize.Width == 0 || AutoScrollMinSize.Width == ClientSize.Width)
				_zoomPercentage = 100;
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
		public virtual void EnsureXIsVisible(int dx)
		{
			if (dx < 0 || dx > ClientSize.Width)
			{
				var newX = -AutoScrollPosition.X + (dx < 0 ? dx : (dx - ClientSize.Width));
				AutoScrollPosition = new Point(newX, AutoScrollPosition.Y);
				_painter.SetOffsetOfLeftEdge(-AutoScrollPosition.X);
			}

			//var newX = -AutoScrollPosition.X;

			//if (dx < 10)
			//    newX -= 10;
			//else if (dx > (ClientSize.Width - 10))
			//    newX += (dx - (ClientSize.Width - 10));
			//else
			//    return;

			//if (newX < 0)
			//    newX = 0;
			//else if (newX > AutoScrollMinSize.Width)
			//    newX = AutoScrollMinSize.Width;

			//if (newX != -AutoScrollPosition.X)
			//{
			//    AutoScrollPosition = new Point(newX, AutoScrollPosition.Y);

			//    if (_painter != null)
			//        _painter.SetOffsetOfLeftEdge(-AutoScrollPosition.X);

			//    Invalidate();
			//}
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
				_playbackEndTime = TimeSpan.Zero;

			var waveOutProvider = new SampleChannel(_playbackEndTime == TimeSpan.Zero ?
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
			if (_prevClientSize != ClientSize)
			{
				if (AutoScroll)
					AutoScrollMinSize = new Size(AutoScrollMinSize.Width, ClientSize.Height);
			}

			base.OnResize(e);

			if (_prevClientSize != ClientSize)
			{
				if (_painter != null)
					_painter.SetVirtualWidth(Math.Max(AutoScrollMinSize.Width, ClientSize.Width));

				Invalidate();
			}

			_prevClientSize = ClientSize;
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
			SetVirtualWidth(Math.Max(ClientSize.Width, AutoScrollPosition.X));
		}

		#endregion
	}
}
