using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			if (_segments.All(sb => sb.end != newBoundary))
			{
				SegmentBoundariesChanged = true;

				var segs = _segments.Select(s => s.end).ToList();
				segs.Add(newBoundary);
				segs.Sort();
				_segments.Clear();

				for (int i = 0; i < segs.Count; i++)
					_segments.Add(new SegmentBoundaries(i == 0 ? TimeSpan.Zero : segs[i - 1], segs[i]));
			}

			return _segments.Select(s => s.end);
		}
	}
}
