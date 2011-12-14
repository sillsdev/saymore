using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SayMore.AudioUtils
{
	/// ----------------------------------------------------------------------------------------
	public class WaveControl : UserControl
	{
		public Action<TimeSpan, TimeSpan> Stopped;
		public Action PlaybackStarted;
		public Action<TimeSpan, TimeSpan> PlaybackUpdate;

		public delegate void SegmentBoundaryMovedHandler(WaveControl ctrl, TimeSpan oldEndTime, TimeSpan newEndTime);
		public event SegmentBoundaryMovedHandler SegmentBoundaryMoved;

		public delegate void SegmentClickedHandler(WaveControl ctrl, int segmentNumber);
		public event SegmentClickedHandler SegmentClicked;

		private WavePainter _painter;
		private WaveStream _waveStream;
		private WaveOut _waveOut;
		private bool _wasStreamCreatedHere;
		private TimeSpan _playbackStartTime;
		private TimeSpan _playbackEndTime;
		private float _zoomPercentage;
		private TimeSpan _boundaryBeingMoved;
		private TimeSpan _leftBoundaryOfSegmentBeingResized;

		public bool IsSegmentMovingInProgress { get; private set; }

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

		/// ------------------------------------------------------------------------------------
		public int GetSegmentForX(int dx)
		{
			var timeAtX = GetTimeFromX(dx);
			int segNumber = 0;

			foreach (var seg in SegmentBoundaries)
			{
				if (timeAtX <= seg)
					return segNumber;

				segNumber++;
			}

			return -1;
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
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (IsSegmentMovingInProgress)
				SetSelectionTimes(_leftBoundaryOfSegmentBeingResized, GetTimeFromX(e.X));
			else if (e.Button == MouseButtons.None)
			{
				var timeAtMouseA = (e.X <= 2 ? TimeSpan.Zero : GetTimeFromX(e.X - 2));
				var timeAtMouseB = GetTimeFromX(e.X + 2);
				_boundaryBeingMoved = SegmentBoundaries.FirstOrDefault(b => b >= timeAtMouseA && b <= timeAtMouseB);
				Cursor = (_boundaryBeingMoved == default(TimeSpan) ? Cursors.Default : Cursors.SizeWE);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			int segmentClicked = -1;

			if (Cursor != Cursors.SizeWE || _boundaryBeingMoved == default(TimeSpan))
				segmentClicked = GetSegmentForX(e.X);
			else
			{
				IsSegmentMovingInProgress = true;
				var segs = SegmentBoundaries.ToArray();
				for (int i = 0; i < segs.Length - 1; i++)
				{
					if (segs[i + 1] == _boundaryBeingMoved)
					{
						_leftBoundaryOfSegmentBeingResized = segs[i];
						segmentClicked = i + 1;
						break;
					}
				}
			}

			if (SegmentClicked != null)
				SegmentClicked(this, segmentClicked);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (!IsSegmentMovingInProgress)
				return;

			IsSegmentMovingInProgress = false;

			if (SegmentBoundaryMoved != null)
			{
				SegmentBoundaryMoved(this, _boundaryBeingMoved, GetTimeFromX(e.X));
				Invalidate();
			}

			_leftBoundaryOfSegmentBeingResized = default(TimeSpan);
			_boundaryBeingMoved = default(TimeSpan);
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
