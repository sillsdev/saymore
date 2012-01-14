using System;
using System.Linq;
using System.Windows.Forms;

namespace SayMore.AudioUtils
{
	public abstract class WaveControlWithMovableBoundaries : WaveControlBasic
	{
		public delegate void BoundaryMovedHandler(WaveControlWithMovableBoundaries ctrl, TimeSpan oldTime, TimeSpan newTime);
		public event BoundaryMovedHandler BoundaryMoved;

		protected int _mouseXAtBeginningOfSegmentMove;
		protected int _minXForBoundaryMove;
		protected int _maxXForBoundaryMove;

		public bool IsBoundaryMovingInProgress { get; protected set; }

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMouseDown(int mouseX, TimeSpan boundaryClicked,
			int indexOutOfBoundaryClicked)
		{
			base.OnBoundaryMouseDown(mouseX, boundaryClicked, indexOutOfBoundaryClicked);
			InitiatiateBoundaryMove(mouseX, boundaryClicked);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void InitiatiateBoundaryMove(int mouseX, TimeSpan boundaryBeingMoved)
		{
			// Figure out the limits within which the boundary may be moved. It's not allowed
			// to be moved to the left of the previous boundary or to the right of the next
			// boundary.
			_minXForBoundaryMove =
				_painter.ConvertTimeToXCoordinate(SegmentBoundaries.LastOrDefault(b => b < boundaryBeingMoved));

			_maxXForBoundaryMove =
				_painter.ConvertTimeToXCoordinate(SegmentBoundaries.FirstOrDefault(b => b > boundaryBeingMoved));

			if (_minXForBoundaryMove > 0)
				_minXForBoundaryMove += WavePainterBasic.kBoundaryHotZoneHalfWidth;

			if (_maxXForBoundaryMove == 0)
				_maxXForBoundaryMove = ClientSize.Width - 1;
			else
				_maxXForBoundaryMove -= WavePainterBasic.kBoundaryHotZoneHalfWidth;

			_painter.SetMovingAnchorTime(boundaryBeingMoved);
			IsBoundaryMovingInProgress = true;
			_mouseXAtBeginningOfSegmentMove = mouseX;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMoveEx(MouseEventArgs e, TimeSpan boundaryMouseOver)
		{
			if (!IsBoundaryMovingInProgress)
			{
				base.OnMouseMoveEx(e, boundaryMouseOver);
				Cursor = (boundaryMouseOver == default(TimeSpan) ? Cursors.Default : Cursors.SizeWE);
				return;
			}

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
			OnBoundaryMoving(GetTimeFromX(e.X));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnBoundaryMoving(TimeSpan newBoundary)
		{
			_painter.SetMovedBoundaryTime(newBoundary);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			_painter.SetMovedBoundaryTime(TimeSpan.Zero);
			var boundaryReallyMoved = (_mouseXAtBeginningOfSegmentMove == -1);
			_mouseXAtBeginningOfSegmentMove = -1;

			if (!IsBoundaryMovingInProgress)
				return;

			IsBoundaryMovingInProgress = false;
			_painter.SetMovingAnchorTime(TimeSpan.Zero);

			if (BoundaryMoved == null || !boundaryReallyMoved)
				OnBoundaryMovedToOrigin(_boundaryMouseOver);
			else
			{
				var dx = e.X;
				if (dx < _minXForBoundaryMove)
					dx = _minXForBoundaryMove;
				else if (dx > _maxXForBoundaryMove)
					dx = _maxXForBoundaryMove;

				OnBoundaryMoved(_boundaryMouseOver, GetTimeFromX(dx));
			}

			_boundaryMouseOver = default(TimeSpan);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnBoundaryMovedToOrigin(TimeSpan originalBoundary)
		{
			_painter.InvalidateBoundary(originalBoundary);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnBoundaryMoved(TimeSpan oldBoundary, TimeSpan newBoundary)
		{
			BoundaryMoved(this, oldBoundary, newBoundary);
			_painter.InvalidateBoundary(oldBoundary);
		}
	}
}
