using System;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using SayMore.Transcription.Model;

namespace SayMore.Media.Audio
{
	public class WaveControlWithBoundarySelection : WaveControlWithMovableBoundaries
	{
		/// ------------------------------------------------------------------------------------
		protected override WavePainterBasic GetNewWavePainter(WaveFileReader stream, string source)
		{
			return new WavePainterWithBoundarySelection(this, stream, source);
		}

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
		protected override bool AllowMovingBoundariesWithAdjacentAnnotations => true;

		/// ------------------------------------------------------------------------------------
		public override void IgnoreMouseProcessing(bool ignore)
		{
			base.IgnoreMouseProcessing(ignore);

			if (ignore)
			{
				Cursor = Cursors.Default;
				MyPainter.HighlightBoundaryMouseOver(TimeSpan.Zero);
			}
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
		protected override void OnMouseMoveEx(MouseEventArgs e,
			TimeSpan boundaryMouseOver)
		{
			base.OnMouseMoveEx(e, boundaryMouseOver);

			if (IsBoundaryMovingInProgress)
				MyPainter.SetCursor(TimeSpan.Zero);
			else if (e.Button == MouseButtons.None)
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
		protected override bool OnInitiatingBoundaryMove(int mouseX, TimeSpan boundary)
		{
			MyPainter.HighlightBoundaryWhenMouseIsNear = false;
			MyPainter.SetSelectedBoundary(TimeSpan.Zero);
			var canMove = base.OnInitiatingBoundaryMove(mouseX, boundary);
			MyPainter.SetMovedBoundaryTime(boundary);

			if (!canMove)
			{
				// Can't move it, but still want to select it.
				MyPainter.SetSelectedBoundary(boundary);
				MyPainter.HighlightBoundaryWhenMouseIsNear = true;
			}

			return canMove;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnBoundaryMovedToOrigin(TimeSpan sourceBoundary)
		{
			base.OnBoundaryMovedToOrigin(sourceBoundary);
			MyPainter.SetSelectedBoundary(sourceBoundary);
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
