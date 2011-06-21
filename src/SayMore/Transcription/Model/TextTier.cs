using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class TextTier : TierBase
	{
		/// ------------------------------------------------------------------------------------
		public TextTier(string displayName) : base(displayName)
		{
			DataType = TierType.Text;
			GridColumn = new TextTranscriptionColumn(this);
		}

		/// ------------------------------------------------------------------------------------
		public TextSegment AddSegment(string text)
		{
			var segment = new TextSegment(this, text);
			_segments.Add(segment);
			return segment;
		}
	}
}
