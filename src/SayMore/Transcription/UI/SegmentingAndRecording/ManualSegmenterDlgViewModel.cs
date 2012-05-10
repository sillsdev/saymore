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
		public bool SaveBoundaryPositionAfterMovedUsingArrowKeys(TimeSpan oldBoundary, TimeSpan newBoundary)
		{
			var oldbndry = TimeSpan.FromSeconds((float)oldBoundary.TotalSeconds);
			var newbndry = TimeSpan.FromSeconds((float)newBoundary.TotalSeconds);

			return UpdateSegmentBoundary(oldbndry, newbndry);
		}
	}
}
