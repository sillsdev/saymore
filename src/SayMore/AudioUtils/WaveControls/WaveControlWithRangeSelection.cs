using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;

namespace SayMore.Media
{
	public class WaveControlWithRangeSelection : WaveControlWithMovableBoundaries
	{
		public delegate void SelectedRegionChangedHandler(WaveControlWithRangeSelection wavCtrl,
			TimeSpan newStart, TimeSpan newEnd);

		public event SelectedRegionChangedHandler SelectedRegionChanged;

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
			if (MyPainter.SelectedRegionStartTime < MyPainter.SelectedRegionEndTime)
				base.Play(MyPainter.SelectedRegionStartTime, MyPainter.SelectedRegionEndTime);
		}

		/// ------------------------------------------------------------------------------------
		public void InvalidateSelectedRegion()
		{
			if (MyPainter.SelectedRegionStartTime == MyPainter.SelectedRegionEndTime)
				return;

			Invalidate(new Rectangle(MyPainter.ConvertTimeToXCoordinate(MyPainter.SelectedRegionStartTime),
				0, MyPainter.ConvertTimeToXCoordinate(MyPainter.SelectedRegionEndTime), ClientSize.Height));
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
			return (MyPainter.SelectedRegionStartTime < MyPainter.SelectedRegionEndTime);
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectionTimes(TimeSpan start, TimeSpan end)
		{
			var regionChanged = (start != MyPainter.SelectedRegionStartTime ||
				end != MyPainter.SelectedRegionEndTime);

			MyPainter.SetSelectionTimes(start, end);

			if (regionChanged && SelectedRegionChanged != null)
				SelectedRegionChanged(this, start, end);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

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
