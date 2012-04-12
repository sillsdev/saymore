using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using SayMore.Transcription.Model;

namespace SayMore.Media
{
	public class WaveControlWithRangeSelection : WaveControlWithMovableBoundaries
	{
		public delegate void SelectedRegionChangedHandler(
			WaveControlWithRangeSelection wavCtrl, TimeRange newTimeRange);

		public bool SelectSegmentOnMouseOver { get; set; }

		private bool _saveStateOfSelectSegmentOnMouseOver;
		private Color[] _selectionColorsToLeftOfMovingBoundary;
		private Color[] _selectionColorsToRightOfMovingBoundary;
		private TimeSpan _boundaryToRightOfMovingBoundary;

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
		private void ClearSelection()
		{
			SetSelectionTimes(TimeSpan.Zero, TimeSpan.Zero);
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectionTimes(TimeSpan start, TimeSpan end)
		{
			SetSelectionTimes(new TimeRange(start, end), Color.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectionTimes(TimeRange newTimeRange, Color color)
		{
			MyPainter.SetSelectionTimes(newTimeRange, color);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (!SelectSegmentOnMouseOver)
				return;

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

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMouseDown(int mouseX, TimeSpan boundaryClicked,
			int indexOutOfBoundaryClicked)
		{
			var boundaries = SegmentBoundaries.ToArray();

			var startTime = (indexOutOfBoundaryClicked == 0 ? TimeSpan.Zero :
				boundaries.ElementAt(indexOutOfBoundaryClicked - 1));

			MyPainter.SetSelectionTimes(startTime, boundaryClicked);

			_selectionColorsToLeftOfMovingBoundary = MyPainter.GetColorsOfAreaEndingAtTime(boundaryClicked);
			System.Diagnostics.Debug.Write("OnBoundaryMouseDown: Left colors = ");
			foreach (var color in _selectionColorsToLeftOfMovingBoundary)
				System.Diagnostics.Debug.Write(color + "; ");
			System.Diagnostics.Debug.WriteLine("");

			if (indexOutOfBoundaryClicked >= boundaries.Length - 1)
				_selectionColorsToRightOfMovingBoundary = null;
			else
			{
				_selectionColorsToRightOfMovingBoundary = MyPainter.GetColorsOfAreaStartingAtTime(boundaryClicked);
				_boundaryToRightOfMovingBoundary = SegmentBoundaries.ElementAt(indexOutOfBoundaryClicked + 1);

				System.Diagnostics.Debug.Write("OnBoundaryMouseDown: Right colors = ");
				foreach (var color in _selectionColorsToRightOfMovingBoundary)
					System.Diagnostics.Debug.Write(color + "; ");
				System.Diagnostics.Debug.WriteLine("");
			}
			base.OnBoundaryMouseDown(mouseX, boundaryClicked, indexOutOfBoundaryClicked);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnInitiatiatingBoundaryMove(InitiatiatingBoundaryMoveEventArgs e)
		{
			_saveStateOfSelectSegmentOnMouseOver = SelectSegmentOnMouseOver;
			SelectSegmentOnMouseOver = false;
			base.OnInitiatiatingBoundaryMove(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMoving(TimeSpan newBoundary)
		{
			base.OnBoundaryMoving(newBoundary);
			var newTimeRange = new TimeRange(MyPainter.DefaultSelectedRange.Start, newBoundary);

			SetSelectionTimesForBoundaryMove(newTimeRange);
		}

		/// ------------------------------------------------------------------------------------
		private void SetSelectionTimesForBoundaryMove(TimeRange newTimeRangeOfLeftSegment)
		{
			foreach (var color in _selectionColorsToLeftOfMovingBoundary)
				MyPainter.SetSelectionTimes(newTimeRangeOfLeftSegment, color);

			if (_selectionColorsToRightOfMovingBoundary == null)
				return;

			var newTimeRangeOfRightSegment = new TimeRange(newTimeRangeOfLeftSegment.End,
				_boundaryToRightOfMovingBoundary);

			foreach (var color in _selectionColorsToRightOfMovingBoundary)
				MyPainter.SetSelectionTimes(newTimeRangeOfRightSegment, color);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnBoundaryMoved(TimeSpan oldBoundary, TimeSpan newBoundary)
		{
			SelectSegmentOnMouseOver = _saveStateOfSelectSegmentOnMouseOver;

			if (base.OnBoundaryMoved(oldBoundary, newBoundary))
				return true;

			SetSelectionTimesForBoundaryMove(new TimeRange(MyPainter.DefaultSelectedRange.Start, oldBoundary));
			return false;
		}
	}
}
