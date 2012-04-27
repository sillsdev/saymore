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
	public class WaveControl : UserControl
	{
		public enum SegmentSelectionType
		{
			Segments,
			Boundaries
		}

		public Action<TimeSpan, TimeSpan> Stopped;
		public Action PlaybackStarted;
		public Action<TimeSpan, TimeSpan> PlaybackUpdate;

		public delegate void BoundaryMovedHandler(WaveControl ctrl, TimeSpan oldTime, TimeSpan newTime);
		public event BoundaryMovedHandler BoundaryMoved;

		public delegate void BoundaryClickedHandler(WaveControl ctrl, int segmentNumber);
		public event BoundaryClickedHandler SegmentClicked;

		protected float _zoomPercentage;
		protected bool _wasStreamCreatedHere;
		protected WavePainter _painter;
		protected WaveStream _waveStream;
		protected WaveOut _waveOut;
		protected TimeSpan _playbackStartTime;
		protected TimeSpan _playbackEndTime;
		protected TimeSpan _boundaryMouseOver;
		//protected int _mouseXAtBeginningOfSegmentMove;
		//protected int _minXForBoundaryMove;
		//protected int _maxXForBoundaryMove;

		//public bool IsBoundaryMovingInProgress { get; protected set; }
		public SegmentSelectionType SelectSegmentType { get; set; }

		/// ------------------------------------------------------------------------------------
		public WaveControl()
		{
			// ENHANCE: This class assumes the audio data is in 16 bit samples.

			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			ResizeRedraw = true;

			AutoScroll = true;

			ForeColor = SystemColors.WindowText;
			BackColor = SystemColors.Window;
			SelectSegmentType = SegmentSelectionType.Segments;
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
			get { return (_painter != null ? _painter.SegmentBoundaries : new TimeSpan[] { }); }
			set
			{
				if (_painter != null)
					_painter.SegmentBoundaries = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool HighlightDuringPlayback { get; set; }

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
		public void SetSelectedBoundary(TimeSpan selectedBoundary)
		{
			SetSelectionTimes(TimeSpan.Zero, selectedBoundary);
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectionTimes(TimeSpan selStartTime, TimeSpan selEndTime)
		{
			if (SelectSegmentType == SegmentSelectionType.Boundaries)
				_painter.SetSelectedBoundary(selEndTime);
			else
				_painter.SetSelectionTimes(selStartTime, selEndTime);
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectedSegment(int segmentNumber)
		{
			var segs = SegmentBoundaries.ToArray();

			if (segmentNumber >= segs.Length - 1)
				SetSelectionTimes(TimeSpan.Zero, segs[segmentNumber - 1]);
			else
				SetSelectionTimes(segs[segmentNumber], segs[segmentNumber + 1]);
		}

		/// ------------------------------------------------------------------------------------
		private void SelectSegment(int segNumber)
		{
			if (segNumber == -1)
				return;

			var segs = SegmentBoundaries.ToArray();

			if (SelectSegmentType == SegmentSelectionType.Boundaries)
				SetSelectedBoundary(segs[segNumber]);
			else
				SetSelectionTimes(segNumber == 0 ? TimeSpan.Zero : segs[segNumber - 1], segs[segNumber]);
		}

		/// ------------------------------------------------------------------------------------
		public void ClearSelection()
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

		/// ------------------------------------------------------------------------------------
		public int GetSegmentForTime(TimeSpan time)
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
		public int GetSegmentForX(int dx)
		{
			return (SelectSegmentType == SegmentSelectionType.Boundaries ?
				_painter.GetBoundaryNearX(dx) : GetSegmentForTime(GetTimeFromX(dx)));
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

			if (HighlightDuringPlayback)
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
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (IsBoundaryMovingInProgress)
			{
				// If moving a boundary has been initiated, there are two conditions in which
				// a mouse movement is ignored. They are: 1) the boundary has not moved moved
				// more than 2 pixels from it's origin (this is to prevent an unintended
				// boundary movement when the user just wants to click a boundary to select it;
				// 2) The mouse has been moved too far to the left or right (i.e. beyond an
				// adjacent boundary).
				if ((_mouseXAtBeginningOfSegmentMove > -1 && e.X >= _mouseXAtBeginningOfSegmentMove - 2 &&
					e.X <= _mouseXAtBeginningOfSegmentMove + 2) || (e.X < _minXForBoundaryMove ||
					e.X > _maxXForBoundaryMove))
				{
					return;
				}

				_mouseXAtBeginningOfSegmentMove = -1;

				if (SelectSegmentType == SegmentSelectionType.Boundaries)
					SetSelectedBoundary(GetTimeFromX(e.X));
				else
					SetSelectionTimes(_painter.SelectedRegionStartTime, GetTimeFromX(e.X));
			}
			else if (e.Button == MouseButtons.None)
			{
				var timeAtMouseA = (e.X <= 4 ? TimeSpan.Zero : GetTimeFromX(e.X - 4));
				var timeAtMouseB = GetTimeFromX(e.X + 4);
				_boundaryMouseOver = SegmentBoundaries.FirstOrDefault(b => b >= timeAtMouseA && b <= timeAtMouseB);
				Cursor = (_boundaryMouseOver == default(TimeSpan) ? Cursors.Default : Cursors.SizeWE);
				_painter.HighlightBoundaryMouseOver(_boundaryMouseOver);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			int segmentClicked = (_boundaryMouseOver == TimeSpan.Zero ?
				GetSegmentForX(e.X) : GetSegmentForTime(_boundaryMouseOver));

			if (segmentClicked == -1)
			{
				SetPlaybackCursor(GetTimeFromX(e.X));
				return;
			}

			SelectSegment(segmentClicked);

			if (IsPlaying)
				Stop();

			if (SelectSegmentType == SegmentSelectionType.Boundaries)
				SetPlaybackCursor(SegmentBoundaries.ElementAt(segmentClicked));
			else
				SetPlaybackCursor(segmentClicked == 0 ? TimeSpan.Zero : SegmentBoundaries.ElementAt(segmentClicked));

			if (SegmentClicked != null)
				SegmentClicked(this, segmentClicked);

			if (_boundaryMouseOver != default(TimeSpan))
				InitiatiateBoundaryMove(e.X);
		}

		/// ------------------------------------------------------------------------------------
		private void InitiatiateBoundaryMove(int dx)
		{
			// Figure out the limits within which the boundary may be moved. It's not allowed
			// to be moved to the left of the previous boundary or to the right of the next
			// boundary.
			_minXForBoundaryMove =
				_painter.ConvertTimeToXCoordinate(SegmentBoundaries.LastOrDefault(b => b < _boundaryMouseOver));

			_maxXForBoundaryMove =
				_painter.ConvertTimeToXCoordinate(SegmentBoundaries.FirstOrDefault(b => b > _boundaryMouseOver));

			if (_minXForBoundaryMove > 0)
				_minXForBoundaryMove += WavePainter.kMarkerHalfWidth;

			if (_maxXForBoundaryMove == 0)
				_maxXForBoundaryMove = ClientSize.Width - 1;
			else
				_maxXForBoundaryMove -= WavePainter.kMarkerHalfWidth;

			_painter.BeginBoundaryMove(_boundaryMouseOver);
			IsBoundaryMovingInProgress = true;
			_mouseXAtBeginningOfSegmentMove = dx;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			var boundaryReallyMoved = (_mouseXAtBeginningOfSegmentMove == -1);
			_mouseXAtBeginningOfSegmentMove = -1;

			if (!IsBoundaryMovingInProgress)
				return;

			IsBoundaryMovingInProgress = false;
			_painter.EndBoundaryMove();

			if (BoundaryMoved != null && boundaryReallyMoved)
			{
				var dx = e.X;
				if (dx < _minXForBoundaryMove)
					dx = _minXForBoundaryMove;
				else if (dx > _maxXForBoundaryMove)
					dx = _maxXForBoundaryMove;

				BoundaryMoved(this, _boundaryMouseOver, GetTimeFromX(dx));
				Invalidate();
			}

			_boundaryMouseOver = default(TimeSpan);
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

		#endregion
	}
}
