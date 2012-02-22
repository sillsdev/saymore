using System;
using System.Collections.Generic;
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
		public IEnumerable<TimeSpan> SaveNewBoundary(TimeSpan newBoundary)
		{
			if (TimeTier.InsertSegmentBoundary((float)newBoundary.TotalSeconds) == Model.BoundaryModificationResult.Success)
				SegmentBoundariesChanged = true;

			return GetSegmentEndBoundaries();
		}

		/// ------------------------------------------------------------------------------------
		public void SaveNewBoundaryPosition(TimeSpan oldBoundary, TimeSpan newBoundary)
		{
			var oldbndry = (float)oldBoundary.TotalSeconds;
			var newbndry = (float)newBoundary.TotalSeconds;

			if (TimeTier.ChangeSegmentsEndBoundary(oldbndry, newbndry) == Model.BoundaryModificationResult.Success)
				SegmentBoundariesChanged = true;
		}
	}
}
