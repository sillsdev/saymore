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
		public TextTier(string displayName) : base(displayName)
		{
			DataType = TierType.Text;
			GridColumn = new TextAnnotationColumn(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override TierBase GetNewTierInstance()
		{
			return new TextTier(DisplayName);
		}

		/// ------------------------------------------------------------------------------------
		public TextSegment AddSegment(string id)
		{
			return AddSegment(id, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public TextSegment AddSegment(string id, string text)
		{
			var segment = new TextSegment(this, id, text);
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

			return base.RemoveSegment(index);
		}

		/// ------------------------------------------------------------------------------------
		public void JoinSements(int fromIndex, int toIndex)
		{
			var fromSeg = GetSegment(fromIndex) as ITextSegment;
			var toSeg = GetSegment(toIndex) as ITextSegment;

			var fromText = (fromSeg.GetText() ?? string.Empty).Trim();
			var toText = (toSeg.GetText() ?? string.Empty).Trim();

			toSeg.SetText(fromIndex < toIndex ?
				(fromText + " " + toText).Trim() : (toText + " " + fromText).Trim());
		}

		/// ------------------------------------------------------------------------------------
		public override object GetTierClipboardData(out string dataFormat)
		{
			dataFormat = DataFormats.UnicodeText;
			var bldr = new StringBuilder();
			foreach (var seg in GetAllSegments().Cast<ITextSegment>())
				bldr.AppendLine(seg.GetText());

			return bldr.ToString().TrimEnd();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns true if all the segments in the tier are not empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GetIsComplete()
		{
			var segments = GetAllSegments().Cast<ITextSegment>().ToArray();
			return (segments.Length > 0 && !segments.Any(s => string.IsNullOrEmpty(s.GetText())));
		}
	}
}
