using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SayMore.Transcription.Model;

namespace SayMore.Media
{
	public abstract class WaveControlWithMovableBoundaries : WaveControlBasic
	{
		public delegate bool BoundaryMovedHandler(WaveControlWithMovableBoundaries ctrl, TimeSpan oldTime, TimeSpan newTime);
		public event BoundaryMovedHandler BoundaryMoved;
		public delegate void InitiatiatingBoundaryMoveHandler(WaveControlWithMovableBoundaries ctrl, InitiatiatingBoundaryMoveEventArgs e);
		public event InitiatiatingBoundaryMoveHandler InitiatiatingBoundaryMove;


		protected int _mouseXAtBeginningOfSegmentMove;
		protected int _minXForBoundaryMove;
		protected int _maxXForBoundaryMove;

		public bool IsBoundaryMovingInProgress { get; protected set; }

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMouseDown(int mouseX, TimeSpan boundaryClicked,
			int indexOutOfBoundaryClicked)
		{
			base.OnBoundaryMouseDown(mouseX, boundaryClicked, indexOutOfBoundaryClicked);
			OnInitiatiatingBoundaryMove(new InitiatiatingBoundaryMoveEventArgs(mouseX, boundaryClicked));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnInitiatiatingBoundaryMove(InitiatiatingBoundaryMoveEventArgs e)
		{
			if (InitiatiatingBoundaryMove != null)
			{
				InitiatiatingBoundaryMove(this, e);
				if (e.Cancel)
					return;
			}

			_scrollCalculator = new WaveControlScrollCalculator(this);

			// Figure out the limits within which the boundary may be moved. It's not allowed
			// to be moved to the left of the previous boundary or to the right of the next
			// boundary.
			_minXForBoundaryMove = Math.Max(1,
				Painter.ConvertTimeToXCoordinate(SegmentBoundaries.LastOrDefault(b => b < e.BoundaryBeingMoved)));

			var nextBoundary = SegmentBoundaries.FirstOrDefault(b => b > e.BoundaryBeingMoved);
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

			Painter.SetMovingAnchorTime(e.BoundaryBeingMoved);
			IsBoundaryMovingInProgress = true;
			_mouseXAtBeginningOfSegmentMove = e.MouseX;
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
			// a mouse movement is ignored. They are: 1) the boundary has not moved
			// more than 2 pixels from it's origin (this is to prevent an unintended
			// boundary movement when the user just wants to click a boundary to select it;
			// 2) The mouse has been moved too far to the left or right (i.e. beyond an
			// adjacent boundary or the end).
			if ((_mouseXAtBeginningOfSegmentMove > -1 && e.X >= _mouseXAtBeginningOfSegmentMove - 2 &&
				e.X <= _mouseXAtBeginningOfSegmentMove + 2) || e.X < _minXForBoundaryMove ||
				e.X > _maxXForBoundaryMove)
			{
				return;
			}

			_mouseXAtBeginningOfSegmentMove = -1;
			OnBoundaryMoving(GetTimeFromX(e.X));
			EnsureXIsVisible(e.X);
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

			_scrollCalculator = null;

			IsBoundaryMovingInProgress = false;
			Painter.SetMovingAnchorTime(TimeSpan.Zero);

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
			Painter.InvalidateBoundary(originalBoundary);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool OnBoundaryMoved(TimeSpan oldBoundary, TimeSpan newBoundary)
		{
			var success = BoundaryMoved(this, oldBoundary, newBoundary);
			Painter.InvalidateBoundary(oldBoundary);
			return success;
		}
	}

	#region InitiatiatingBoundaryMoveEventArgs class
	/// ------------------------------------------------------------------------------------
	public class InitiatiatingBoundaryMoveEventArgs : CancelEventArgs
	{
		public int MouseX { get; private set; }
		public TimeSpan BoundaryBeingMoved { get; private set; }

		/// ------------------------------------------------------------------------------------
		public InitiatiatingBoundaryMoveEventArgs(int mouseX, TimeSpan boundary)
		{
			MouseX = mouseX;
			BoundaryBeingMoved = boundary;
		}
	}

	#endregion
}
