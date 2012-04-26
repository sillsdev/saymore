using System.Collections.Generic;
using System.Linq;
using SayMore.Media.UI;
using SayMore.Model.Files;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class AudacityLabelHelper
	{
		private readonly string _mediaFile;

		public IEnumerable<Segment> Segments { get; private set; }

		/// ------------------------------------------------------------------------------------
		public AudacityLabelHelper(IEnumerable<string> allLabelLines, string mediaFile) :
			this(allLabelLines, '\t', mediaFile)
		{
		}

		/// ------------------------------------------------------------------------------------
		public AudacityLabelHelper(IEnumerable<string> allLabelLines, char delimiter, string mediaFile)
		{
			_mediaFile = mediaFile;

			// Parse each line (using the specified delimiter) into an array of strings.
			var lines = allLabelLines.Select(ln => ln.Split(delimiter)).Where(p => p.Length >= 1).ToList();

			// Create an easier to use (i.e. than string arrays) list of objects for each label.
			var segments = lines.Select(l => CreateSingleSegment(l)).Where(ali => ali.Start > -1).ToList();

			Segments = FixUpLabelInfo(segments);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Segment> FixUpLabelInfo(List<Segment> segments)
		{
			for (int i = 0; i < segments.Count; i++)
			{
				if (segments[i].End > segments[i].Start)
					continue;

				// At this point, we know the stop location is zero. For all labels but the
				// last label, the stop position will be the start position of the next label.
				// For the last label, the stop position is assumed to be the end of the file.
				if (i < segments.Count - 1)
					segments[i].End = segments[i + 1].Start;
				else if (i == segments.Count - 1 && _mediaFile != null)
				{
					var mediaInfo = MediaFileInfo.GetInfo(_mediaFile);

					if (segments[i].Start.Equals(mediaInfo.DurationSeconds))
						segments.RemoveAt(i);
					else
						segments[i].End = mediaInfo.DurationSeconds;
				}
			}

			// If the label file didn't have a label at the beginning of the
			// file (i.e. offset zero), treat offset zero as an implicit label.
			if (segments.Count > 0 && segments[0].Start > 0)
				segments.Insert(0, new Segment { Start = 0, End = segments[0].Start });

			return segments;
		}

		/// ------------------------------------------------------------------------------------
		public Segment CreateSingleSegment(string[] labelInfo)
		{
			var seg = new Segment(null);

			if (labelInfo.Length >= 3)
				seg.Text = labelInfo[2];

			float value;

			if (float.TryParse(labelInfo[0].Trim(), out value))
				seg.Start = value;

			if (labelInfo.Length > 1 && float.TryParse(labelInfo[1].Trim(), out value))
				seg.End = value;
			else
				seg.End = seg.Start;

			return seg;
		}
	}
}
