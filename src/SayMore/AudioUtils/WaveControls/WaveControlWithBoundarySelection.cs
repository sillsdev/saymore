using System;
using System.Drawing;
using System.Linq;
using NAudio.Wave;
using SayMore.Transcription.Model;

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
		protected WavePainterWithBoundarySelection MyPainter
		{
			get { return Painter as WavePainterWithBoundarySelection; }
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetSelectedBoundary()
		{
			return MyPainter.SelectedBoundaryTime;
		}

		/// ------------------------------------------------------------------------------------
		public void ClearSelectedBoundary()
		{
			MyPainter.SetSelectedBoundary(TimeSpan.Zero);
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectedBoundary(TimeSpan boundary)
		{
			if (boundary == TimeSpan.Zero || SegmentBoundaries.Any(b => b == boundary))
			{
				MyPainter.SetSelectedBoundary(boundary);
				EnsureTimeIsVisible(boundary, new TimeRange(boundary, boundary), false, false);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			MyPainter.HighlightBoundaryMouseOver(TimeSpan.Zero);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMoveEx(System.Windows.Forms.MouseEventArgs e,
			TimeSpan boundaryMouseOver)
		{
			base.OnMouseMoveEx(e, boundaryMouseOver);

			if (IsBoundaryMovingInProgress)
				MyPainter.SetCursor(TimeSpan.Zero);
			else if (e.Button == System.Windows.Forms.MouseButtons.None)
				MyPainter.HighlightBoundaryMouseOver(boundaryMouseOver);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMouseDown(int mouseX, TimeSpan boundaryClicked,
			int indexOutOfBoundaryClicked)
		{
			MyPainter.SetSelectedBoundary(IsBoundaryMovingInProgress ? TimeSpan.Zero : boundaryClicked);
			base.OnBoundaryMouseDown(mouseX, boundaryClicked, indexOutOfBoundaryClicked);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnInitiatiatingBoundaryMove(InitiatiatingBoundaryMoveEventArgs e)
		{
			MyPainter.HighlightBoundaryWhenMouseIsNear = false;
			MyPainter.SetSelectedBoundary(TimeSpan.Zero);
			base.OnInitiatiatingBoundaryMove(e);
			MyPainter.SetMovedBoundaryTime(e.BoundaryBeingMoved);

			if (e.Cancel)
			{
				// Can't move it, but still want to select it.
				MyPainter.SetSelectedBoundary(e.BoundaryBeingMoved);
				MyPainter.HighlightBoundaryWhenMouseIsNear = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMovedToOrigin(TimeSpan originalBoundary)
		{
			base.OnBoundaryMovedToOrigin(originalBoundary);
			MyPainter.SetSelectedBoundary(originalBoundary);
			MyPainter.HighlightBoundaryWhenMouseIsNear = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnBoundaryMoved(TimeSpan oldBoundary, TimeSpan newBoundary)
		{
			var success = base.OnBoundaryMoved(oldBoundary, newBoundary);

			if (success)
				MyPainter.SetSelectedBoundary(newBoundary);
			else
				SetSelectedBoundary(oldBoundary);

			MyPainter.HighlightBoundaryWhenMouseIsNear = true;
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
