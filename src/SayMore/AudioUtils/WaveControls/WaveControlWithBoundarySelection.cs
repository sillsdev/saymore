using System;
using System.Drawing;
using System.Linq;
using NAudio.Wave;

namespace SayMore.Media
{
	public class WaveControlWithBoundarySelection : WaveControlWithMovableBoundaries
	{
		/// ------------------------------------------------------------------------------------
		protected override WavePainterBasic GetNewWavePainter(WaveFileReader stream)
		{
			return new WavePainterWithBoundarySelection(this, stream);
		}

		//protected override WavePainterBasic GetNewWavePainter(IEnumerable<float> samples, TimeSpan totalTime)
		//{
		//    return new WavePainterWithBoundarySelection(this, samples, totalTime);
		//}

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
		public void ClearSelectedBoundary()
		{
			Painter.SetSelectedBoundary(TimeSpan.Zero);
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectedBoundary(TimeSpan boundary)
		{
			if (boundary == TimeSpan.Zero || SegmentBoundaries.Any(b => b == boundary))
			{
				Painter.SetSelectedBoundary(boundary);
				var dx = Painter.ConvertTimeToXCoordinate(boundary);
				EnsureXIsVisible(dx);
			}
		}

		/// ------------------------------------------------------------------------------------
		public Rectangle GetSelectedBoundaryRectangle()
		{
			var boundary = GetSelectedBoundary();
			if (boundary == TimeSpan.Zero)
				return Rectangle.Empty;

			return _painter.GetRectangleForTimeRange(
				boundary.Subtract(TimeSpan.FromSeconds(1)),
				boundary.Add(TimeSpan.FromSeconds(1)));
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
			Painter.HighlightBoundaryWhenMouseIsNear = false;
			Painter.SetSelectedBoundary(TimeSpan.Zero);
			base.InitiatiateBoundaryMove(mouseX, boundaryBeingMoved);
			Painter.SetMovedBoundaryTime(boundaryBeingMoved);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMovedToOrigin(TimeSpan originalBoundary)
		{
			base.OnBoundaryMovedToOrigin(originalBoundary);
			Painter.SetSelectedBoundary(originalBoundary);
			Painter.HighlightBoundaryWhenMouseIsNear = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnBoundaryMoved(TimeSpan oldBoundary, TimeSpan newBoundary)
		{
			var success = base.OnBoundaryMoved(oldBoundary, newBoundary);

			if (success)
				Painter.SetSelectedBoundary(newBoundary);
			else
				SetSelectedBoundary(oldBoundary);

			Painter.HighlightBoundaryWhenMouseIsNear = true;
			return success;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSetCursorWhenMouseDown(TimeSpan timeAtMouseX, bool wasBoundaryClicked)
		{
			if (!wasBoundaryClicked)
				base.OnSetCursorWhenMouseDown(timeAtMouseX, false);
		}
	}
}
