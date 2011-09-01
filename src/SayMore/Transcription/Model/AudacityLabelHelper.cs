using System.Collections.Generic;
using System.Linq;
using SayMore.UI.MediaPlayer;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class AudacityLabelInfo
	{
		public float Start = -1;
		public float Stop = -1;
		public string Text;
	}

	/// ----------------------------------------------------------------------------------------
	public class AudacityLabelHelper
	{
		private readonly string _mediaFile;

		public IEnumerable<AudacityLabelInfo> LabelInfo { get; private set; }

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
			var labelInfo = lines.Select(l => CreateSingleLabelInfo(l)).Where(ali => ali.Start > -1).ToList();

			LabelInfo = FixUpLabelInfo(labelInfo);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<AudacityLabelInfo> FixUpLabelInfo(List<AudacityLabelInfo> labelInfo)
		{
			for (int i = 0; i < labelInfo.Count; i++)
			{
				if (labelInfo[i].Stop > labelInfo[i].Start)
					continue;

				// At this point, we know the stop location is zero. For all labels but the
				// last label, the stop position will be the start position of the next label.
				// For the last label, the stop position is assumed to be the end of the file.
				if (i < labelInfo.Count - 1)
					labelInfo[i].Stop = labelInfo[i + 1].Start;
				else if (i == labelInfo.Count - 1 && _mediaFile != null)
				{
					var mediaInfo = new MPlayerMediaInfo(_mediaFile);
					labelInfo[i].Stop = mediaInfo.Duration;
				}
			}

			// If the label file didn't have a label at the beginning of the
			// file (i.e. offset zero), treat offset zero as an implicit label.
			if (labelInfo.Count > 0 && labelInfo[0].Start > 0)
				labelInfo.Insert(0, new AudacityLabelInfo { Start = 0, Stop = labelInfo[0].Start });

			return labelInfo;
		}

		/// ------------------------------------------------------------------------------------
		public AudacityLabelInfo CreateSingleLabelInfo(string[] labelInfo)
		{
			var ali = new AudacityLabelInfo();

			if (labelInfo.Length >= 3)
				ali.Text = labelInfo[2];

			float value;

			if (float.TryParse(labelInfo[0].Trim(), out value))
				ali.Start = value;

			if (labelInfo.Length > 1 && float.TryParse(labelInfo[1].Trim(), out value))
				ali.Stop = value;
			else
				ali.Stop = ali.Start;

			return ali;
		}
	}
}
