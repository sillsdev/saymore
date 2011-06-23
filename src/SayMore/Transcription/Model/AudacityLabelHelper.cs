using System.Collections.Generic;
using System.Linq;
using SayMore.UI.MediaPlayer;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class AudacityLabelInfo
	{
		public float Start = -1;
		public float Length = -1;
		public string Text;

		/// ------------------------------------------------------------------------------------
		public void SetLengthToHere(float stopPoint)
		{
			Length = (float)((decimal)stopPoint - (decimal)Start);
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class AudacityLabelHelper
	{
		private readonly string _mediaFile;

		public IEnumerable<AudacityLabelInfo> LabelInfo { get; private set; }

		/// ------------------------------------------------------------------------------------
		public AudacityLabelHelper(IEnumerable<string> allLabelLines, string mediaFile)
		{
			_mediaFile = mediaFile;

			// Parse each line (using tabs as the delimiter) into an array of strings.
			// Only keep lines having two or more pieces.
			var lines = allLabelLines.Select(ln => ln.Split('\t')).Where(p => p.Length >= 2).ToList();

			// Create an easier to use (i.e. than string arrays) list of objects for each label.
			var labelInfo = lines.Select(CreateSingleLabelInfo).Where(ali => ali.Start > -1).ToList();

			LabelInfo = FixUpLabelInfo(labelInfo);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<AudacityLabelInfo> FixUpLabelInfo(List<AudacityLabelInfo> labelInfo)
		{
			for (int i = 0; i < labelInfo.Count; i++)
			{
				// If the length is not zero, we know the labels refer to segments
				// with lengths, rather than just positions in the media stream.
				if (labelInfo[i].Length > 0)
					continue;

				// At this point, we know the stop location is zero. For all labels but the
				// last label, the stop position will be the start position of the next label.
				// For the last label, the stop position to be the end of the file.
				if (i < labelInfo.Count - 1)
					labelInfo[i].SetLengthToHere(labelInfo[i + 1].Start);
				else if (i == labelInfo.Count - 1 && _mediaFile != null)
				{
					var mediaInfo = new MPlayerMediaInfo(_mediaFile);
					labelInfo[i].SetLengthToHere(mediaInfo.Duration);
				}
			}

			// If the label file didn't have a label at the beginning of the
			// file (i.e. offset zero), treat offset zero as an implicit label.
			if (labelInfo.Count > 0 && labelInfo[0].Start > 0)
				labelInfo.Insert(0, new AudacityLabelInfo { Start = 0, Length = labelInfo[0].Start });

			return labelInfo;
		}

		/// ------------------------------------------------------------------------------------
		public AudacityLabelInfo CreateSingleLabelInfo(string[] labelInfo)
		{
			var ali = new AudacityLabelInfo();

			if (labelInfo.Length >= 3)
				ali.Text = labelInfo[2];

			float start;
			float stop;

			if (float.TryParse(labelInfo[0], out start) && float.TryParse(labelInfo[1], out stop))
			{
				ali.Start = start;
				ali.SetLengthToHere(stop);
			}

			return ali;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ITier> GetTiers()
		{
			var audioTier = new AudioTier("Original", _mediaFile);
			var textTier = new TextTier("Transcription");

			foreach (var label in LabelInfo)
			{
				audioTier.AddSegment(label.Start, label.Length);
				textTier.AddSegment(label.Text);
			}

			return new[] { audioTier as ITier, textTier };
		}
	}
}
