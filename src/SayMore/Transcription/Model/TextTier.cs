using System.Linq;
using System.Text;
using System.Windows.Forms;
using Localization;
using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class TextTier : TierBase
	{
		public const string TranscriptionTierName = "Transcription";
		public const string ElanFreeTranslationTierName = "Phrase Free Translation";

		public static string SayMoreFreeTranslationTierName =
			LocalizationManager.GetString("EventsView.Transcription.TierNames.FreeTranslation", "Free Translation");

		/// ------------------------------------------------------------------------------------
		public TextTier(string displayName) : base(displayName, tier => new TextAnnotationColumn(tier))
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override TierBase GetNewTierInstance()
		{
			return new TextTier(DisplayName);
		}

		/// ------------------------------------------------------------------------------------
		public Segment AddSegment(string id)
		{
			return AddSegment(id, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public Segment AddSegment(string id, string text)
		{
			var segment = new Segment(this, id, text);
			_segments.Add(segment);
			return segment;
		}

		/// ------------------------------------------------------------------------------------
		public override bool RemoveSegment(int index)
		{
			if (index >= 0 && index < _segments.Count && _segments.Count > 1)
			{
				int joinWithIndex = (index == _segments.Count - 1 ? index - 1 : index + 1);
				JoinSements(joinWithIndex, index);
			}

			base.RemoveSegment(index);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public void JoinSements(int fromIndex, int toIndex)
		{
			var fromSeg = _segments[fromIndex];
			var toSeg = _segments[toIndex];

			var fromText = (fromSeg.Text ?? string.Empty).Trim();
			var toText = (toSeg.Text ?? string.Empty).Trim();

			toSeg.Text = (fromIndex < toIndex ?
				(fromText + " " + toText).Trim() : (toText + " " + fromText).Trim());
		}

		/// ------------------------------------------------------------------------------------
		public override object GetTierClipboardData(out string dataFormat)
		{
			dataFormat = DataFormats.UnicodeText;
			var bldr = new StringBuilder();

			foreach (var seg in _segments)
				bldr.AppendLine(seg.Text);

			return bldr.ToString().TrimEnd();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns true if all the segments in the tier are not empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GetIsComplete()
		{
			return (_segments.Count > 0 && !_segments.Any(s => string.IsNullOrEmpty(s.Text)));
		}
	}
}
