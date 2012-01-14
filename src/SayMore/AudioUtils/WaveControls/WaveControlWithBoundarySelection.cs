using System;
using System.Collections.Generic;

namespace SayMore.AudioUtils
{
	public class WaveControlWithBoundarySelection : WaveControlWithMovableBoundaries
	{
		/// ------------------------------------------------------------------------------------
		protected override WavePainterBasic GetNewWavePainter(IEnumerable<float> samples, TimeSpan totalTime)
		{
			return new WavePainterWithBoundarySelection(this, samples, totalTime);
		}

		/// ------------------------------------------------------------------------------------
		private WavePainterWithBoundarySelection Painter
		{
			get { return _painter as WavePainterWithBoundarySelection; }
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetSelectedBoundary()
		{
			return Painter.SelectedBoundaryTime;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			Painter.HighlightBoundaryMouseOver(TimeSpan.Zero);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMoveEx(System.Windows.Forms.MouseEventArgs e,
			TimeSpan boundaryMouseOver)
		{
			base.OnMouseMoveEx(e, boundaryMouseOver);

			if (IsBoundaryMovingInProgress)
				Painter.SetCursor(TimeSpan.Zero);
			else if (e.Button == System.Windows.Forms.MouseButtons.None)
				Painter.HighlightBoundaryMouseOver(boundaryMouseOver);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMouseDown(int mouseX, TimeSpan boundaryClicked,
			int indexOutOfBoundaryClicked)
		{
			Painter.SetSelectedBoundary(IsBoundaryMovingInProgress ? TimeSpan.Zero : boundaryClicked);
			base.OnBoundaryMouseDown(mouseX, boundaryClicked, indexOutOfBoundaryClicked);
		}

		/// ------------------------------------------------------------------------------------
		protected override void InitiatiateBoundaryMove(int mouseX, TimeSpan boundaryBeingMoved)
		{
			Painter.HighlightBoundaryMouseIsNear = false;
			Painter.SetSelectedBoundary(TimeSpan.Zero);
			base.InitiatiateBoundaryMove(mouseX, boundaryBeingMoved);
			Painter.SetMovedBoundaryTime(boundaryBeingMoved);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMovedToOrigin(TimeSpan originalBoundary)
		{
			base.OnBoundaryMovedToOrigin(originalBoundary);
			Painter.SetSelectedBoundary(originalBoundary);
			Painter.HighlightBoundaryMouseIsNear = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMoved(TimeSpan oldBoundary, TimeSpan newBoundary)
		{
			base.OnBoundaryMoved(oldBoundary, newBoundary);
			Painter.SetSelectedBoundary(newBoundary);
			Painter.HighlightBoundaryMouseIsNear = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSetCursorWhenMouseDown(TimeSpan timeAtMouseX, bool wasBoundaryClicked)
		{
			if (!wasBoundaryClicked)
				base.OnSetCursorWhenMouseDown(timeAtMouseX, false);
		}
	}
}
