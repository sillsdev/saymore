using System;
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
		public const string ElanTranscriptionTierId = "Transcription";
		public const string ElanTranslationTierId = "Phrase Free Translation";

		public static string TranscriptionTierDisplayName =
			LocalizationManager.GetString("EventsView.Transcription.TierDisplayNames.Transcription", "Transcription");

		public static string FreeTranslationTierDisplayName =
			LocalizationManager.GetString("EventsView.Transcription.TierDisplayNames.FreeTranslation", "Free Translation");

		protected TierColumnBase _gridColumn;

		/// ------------------------------------------------------------------------------------
		public TextTier(string id) : base(id, tier => AnnotationColumnProvider(id, tier))
		{
			LinguisticType = "Translation";
		}

		/// ------------------------------------------------------------------------------------
		private static TierColumnBase AnnotationColumnProvider(string id, TierBase tier)
		{
			if (id == ElanTranscriptionTierId)
				return new TranscriptionAnnotationColumn(tier);

			return id == ElanTranslationTierId ?
				new TranslationAnnotationColumn(tier) :
				new TextAnnotationColumn(tier);
		}

		/// ------------------------------------------------------------------------------------
		public override string DisplayName
		{
			get
			{
				if (Id == ElanTranscriptionTierId)
					return TranscriptionTierDisplayName;

				if (Id == ElanTranslationTierId)
					return FreeTranslationTierDisplayName;

				return base.DisplayName;
			}
		}

		/// ------------------------------------------------------------------------------------
		public override TierType TierType
		{
			get
			{
				if (Id == ElanTranscriptionTierId)
					return TierType.Transcription;

				if (Id == ElanTranslationTierId)
					return TierType.FreeTranslation;

				return base.TierType;
			}
			set
			{
				base.TierType = value;
				if (value == TierType.Transcription)
					LinguisticType = "Transcription";
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override TierBase GetNewTierInstance()
		{
			return new TextTier(Id);
		}

		/// ------------------------------------------------------------------------------------
		public Segment AddSegment(string text)
		{
			var segment = new Segment(this, text ?? string.Empty);
			Segments.Add(segment);
			return segment;
		}

		/// ------------------------------------------------------------------------------------
		public override bool RemoveSegment(int index)
		{
			if (index >= 0 && index < Segments.Count && Segments.Count > 1)
			{
				// If the segment being removed is the first, then join it with the
				// next segment. Otherwise, join it with the preceding segment.
				int joinToIndex = (index == Segments.Count - 1 ? index - 1 : index + 1);
				JoinSements(index, joinToIndex);
			}

			return base.RemoveSegment(index);
		}

		/// ------------------------------------------------------------------------------------
		public void JoinSements(int fromIndex, int toIndex)
		{
			if (Math.Abs(fromIndex - toIndex) != 1)
			{
				throw new ArgumentException(string.Format("The 'from' index ({0}) " +
					"must be adjacent to the 'to' index ({1}). In other words, the absolute " +
					"value of the difference between the two indexes must be 1.", fromIndex, toIndex));
			}

			var fromSeg = Segments[fromIndex];
			var toSeg = Segments[toIndex];

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

			foreach (var seg in Segments)
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
			return (Segments.Count > 0 && !Segments.Any(s => string.IsNullOrEmpty(s.Text)));
		}
	}
}
