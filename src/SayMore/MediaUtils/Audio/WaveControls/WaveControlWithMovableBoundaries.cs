using System;
using System.Linq;
using System.Windows.Forms;

namespace SayMore.Media.Audio
{
	public abstract class WaveControlWithMovableBoundaries : WaveControlBasic
	{
		public delegate bool BoundaryMovedHandler(WaveControlWithMovableBoundaries ctrl, TimeSpan oldTime, TimeSpan newTime);
		public event BoundaryMovedHandler BoundaryMoved;
		public Func<TimeSpan, bool, bool> CanBoundaryBeMoved;

		protected int _mouseXAtBeginningOfSegmentMove;
		protected int _minXForBoundaryMove;
		protected int _maxXForBoundaryMove;

		public bool IsBoundaryMovingInProgress { get; protected set; }

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMouseDown(int mouseX, TimeSpan boundaryClicked,
			int indexOutOfBoundaryClicked)
		{
			base.OnBoundaryMouseDown(mouseX, boundaryClicked, indexOutOfBoundaryClicked);
			OnInitiatingBoundaryMove(mouseX, boundaryClicked);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool AllowMovingBoundariesWithAdjacentAnnotations => false;

		/// ------------------------------------------------------------------------------------
		protected virtual bool OnInitiatingBoundaryMove(int mouseX, TimeSpan boundary)
		{
			if (CanBoundaryBeMoved != null && !CanBoundaryBeMoved(boundary, AllowMovingBoundariesWithAdjacentAnnotations))
				return false;

			ScrollCalculator = new WaveControlScrollCalculator(this);

			// Figure out the limits within which the boundary may be moved. It's not allowed
			// to be moved to the left of the previous boundary or to the right of the next
			// boundary.
			_minXForBoundaryMove = Painter.ConvertTimeToXCoordinate(SegmentBoundaries.LastOrDefault(b => b < boundary));

			var nextBoundary = SegmentBoundaries.FirstOrDefault(b => b > boundary);
			bool limitedByEndOfStream = nextBoundary == default(TimeSpan);
			if (limitedByEndOfStream)
				nextBoundary = WaveStream.TotalTime;

			_maxXForBoundaryMove = Painter.ConvertTimeToXCoordinate(nextBoundary);

			if (_minXForBoundaryMove > 0)
				_minXForBoundaryMove += WavePainterBasic.kBoundaryHotZoneHalfWidth;

			if (_maxXForBoundaryMove == 0)
				_maxXForBoundaryMove = ClientSize.Width - WavePainterBasic.kRightDisplayPadding + 1;
			else if (!limitedByEndOfStream)
				_maxXForBoundaryMove -= WavePainterBasic.kBoundaryHotZoneHalfWidth;

			Painter.SetMovingAnchorTime(boundary);
			IsBoundaryMovingInProgress = true;
			_mouseXAtBeginningOfSegmentMove = mouseX;

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetAutoScrollPosition(int newTargetX)
		{
			if (IsBoundaryMovingInProgress)
			{
				_maxXForBoundaryMove -= newTargetX + AutoScrollPosition.X;
				_minXForBoundaryMove -= newTargetX + AutoScrollPosition.X;
			}

			base.SetAutoScrollPosition(newTargetX);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMoveEx(MouseEventArgs e, TimeSpan boundaryMouseOver)
		{
			if (!IsBoundaryMovingInProgress)
			{
				base.OnMouseMoveEx(e, boundaryMouseOver);
				Cursor = boundaryMouseOver == default(TimeSpan) ||
					(CanBoundaryBeMoved != null && !CanBoundaryBeMoved(boundaryMouseOver, AllowMovingBoundariesWithAdjacentAnnotations)) ?
					Cursors.Default : Cursors.SizeWE;
				return;
			}

			// If moving a boundary has been initiated, a mouse movement is ignored if
			// the boundary has not moved more than 2 pixels from its origin. This is to
			// prevent an unintended boundary movement when the user just wants to click
			// a boundary to select it.
			var x = e.X;
			if ((_mouseXAtBeginningOfSegmentMove > -1 && x >= _mouseXAtBeginningOfSegmentMove - 2 &&
				x <= _mouseXAtBeginningOfSegmentMove + 2))
			{
				return;
			}

			if (x < _minXForBoundaryMove)
				x = _minXForBoundaryMove;
			else if (x > _maxXForBoundaryMove)
				x = _maxXForBoundaryMove;

			_mouseXAtBeginningOfSegmentMove = -1;
			OnBoundaryMoving(GetTimeFromX(x));
			EnsureXIsVisible(x);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnBoundaryMoving(TimeSpan newBoundary)
		{
			Painter.SetMovedBoundaryTime(newBoundary);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			Painter.SetMovedBoundaryTime(TimeSpan.Zero);
			var boundaryReallyMoved = (_mouseXAtBeginningOfSegmentMove == -1);
			_mouseXAtBeginningOfSegmentMove = -1;

			if (!IsBoundaryMovingInProgress)
				return;

			ScrollCalculator = null;

			IsBoundaryMovingInProgress = false;
			Painter.SetMovingAnchorTime(TimeSpan.Zero);

			if (BoundaryMoved == null || !boundaryReallyMoved)
				OnBoundaryMovedToOrigin(BoundaryMouseOver);
			else
			{
				var dx = e.X;
				if (dx < _minXForBoundaryMove)
					dx = _minXForBoundaryMove;
				else if (dx > _maxXForBoundaryMove)
					dx = _maxXForBoundaryMove;

				OnBoundaryMoved(BoundaryMouseOver, GetTimeFromX(dx));
			}

			BoundaryMouseOver = default(TimeSpan);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnBoundaryMovedToOrigin(TimeSpan sourceBoundary)
		{
			Painter.InvalidateBoundary(sourceBoundary);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool OnBoundaryMoved(TimeSpan oldBoundary, TimeSpan newBoundary)
		{
			var success = BoundaryMoved(this, oldBoundary, newBoundary);
			Painter.InvalidateBoundary(oldBoundary);
			return success;
		}
	}
}
