using System;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using SayMore.Transcription.Model;

namespace SayMore.Media
{
	public class WaveControlWithRangeSelection : WaveControlWithMovableBoundaries
	{
		public delegate void SelectedRegionChangedHandler(WaveControlWithRangeSelection wavCtrl,
			TimeSpan newStart, TimeSpan newEnd);

		public event SelectedRegionChangedHandler SelectedRegionChanged;
		public bool SelectSegmentOnMouseOver { get; set; }

		/// ------------------------------------------------------------------------------------
		public WaveControlWithRangeSelection()
		{
			SelectSegmentOnMouseOver = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override WavePainterBasic GetNewWavePainter(WaveFileReader stream)
		{
			return new WavePainterWithRangeSelection(this, stream);
		}

		/// ------------------------------------------------------------------------------------
		private WavePainterWithRangeSelection MyPainter
		{
			get { return Painter as WavePainterWithRangeSelection; }
		}

		/// ------------------------------------------------------------------------------------
		public void PlaySelectedRegion()
		{
			var timeRange = MyPainter.DefaultSelectedRange;

			if (!TimeRange.IsNullOrZeroLength(timeRange))
				base.Play(timeRange);
		}

		/// ------------------------------------------------------------------------------------
		public void InvalidateSelectedRegion()
		{
			InvalidateRegionBetweenTimes(MyPainter.DefaultSelectedRange);
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
		public void ClearSelection()
		{
			SetSelectionTimes(TimeSpan.Zero, TimeSpan.Zero);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetHasSelection()
		{
			return (!TimeRange.IsNullOrZeroLength(MyPainter.DefaultSelectedRange));
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectionTimes(TimeSpan start, TimeSpan end)
		{
			var newTimeRange = new TimeRange(start, end);

			if (MyPainter.DefaultSelectedRange != newTimeRange)
				return;

			MyPainter.SetSelectionTimes(newTimeRange);

			if (SelectedRegionChanged != null)
				SelectedRegionChanged(this, start, end);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (!SelectSegmentOnMouseOver)
				return;

			if (_boundaryMouseOver != default(TimeSpan))
			{
				ClearSelection();
				return;
			}

			var segNumber = GetSegmentForX(e.X);
			if (segNumber < 0)
			{
				ClearSelection();
				return;
			}

			var start = (segNumber == 0 ? TimeSpan.Zero : SegmentBoundaries.ElementAt(segNumber - 1));
			SetSelectionTimes(start, SegmentBoundaries.ElementAt(segNumber));
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			// Haven't really left if we're still somewhere within the bounds of the client area,
			// which can happen if the mouse passes over a hosted control (e.g., the busy wheel).
			if (!ClientRectangle.Contains(PointToClient(MousePosition)))
				ClearSelection();
		}

		///// ------------------------------------------------------------------------------------
		//protected override void OnMouseDown(MouseEventArgs e)
		//{
		//    base.OnMouseDown(e);

		//    if (_boundaryMouseOver != default(TimeSpan))
		//        return;

		//    var segNumber = GetSegmentForX(e.X);
		//    if (segNumber < 0)
		//        return;

		//    var start = (segNumber == 0 ? TimeSpan.Zero : SegmentBoundaries.ElementAt(segNumber - 1));
		//    SetSelectionTimes(start, SegmentBoundaries.ElementAt(segNumber));
		//}

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMouseDown(int mouseX, TimeSpan boundaryClicked,
			int indexOutOfBoundaryClicked)
		{
			var startTime = (indexOutOfBoundaryClicked == 0 ? TimeSpan.Zero :
				SegmentBoundaries.ElementAt(indexOutOfBoundaryClicked - 1));

			MyPainter.SetSelectionTimes(startTime, boundaryClicked);
			base.OnBoundaryMouseDown(mouseX, boundaryClicked, indexOutOfBoundaryClicked);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMoving(TimeSpan newBoundary)
		{
			base.OnBoundaryMoving(newBoundary);
			MyPainter.SetSelectionTimes(MyPainter.SelectedRegionStartTime, newBoundary);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnBoundaryMoved(TimeSpan oldBoundary, TimeSpan newBoundary)
		{
			if (base.OnBoundaryMoved(oldBoundary, newBoundary))
			{
				SetSelectionTimes(MyPainter.SelectedRegionStartTime, newBoundary);
				return true;
			}

			SetSelectionTimes(MyPainter.SelectedRegionStartTime, oldBoundary);
			return false;
		}
	}
}
