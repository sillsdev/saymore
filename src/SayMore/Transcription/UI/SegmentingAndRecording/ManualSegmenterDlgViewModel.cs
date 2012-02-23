using System;
using SayMore.Model.Files;

namespace SayMore.Transcription.UI.SegmentingAndRecording
{
	public class ManualSegmenterDlgViewModel : SegmenterDlgBaseViewModel
	{
		/// ------------------------------------------------------------------------------------
		public ManualSegmenterDlgViewModel(ComponentFile file) : base(file)
		{
		}

		/// ------------------------------------------------------------------------------------
		public void SaveBoundaryPositionAfterMovedUsingArrowKeys(TimeSpan oldBoundary, TimeSpan newBoundary)
		{
			var oldbndry = (float)oldBoundary.TotalSeconds;
			var newbndry = (float)newBoundary.TotalSeconds;

			if (TimeTier.ChangeSegmentsEndBoundary(oldbndry, newbndry) == Model.BoundaryModificationResult.Success)
				SegmentBoundariesChanged = true;
		}
	}
}
