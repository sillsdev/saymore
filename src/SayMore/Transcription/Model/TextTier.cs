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
			GridColumn = new TextAnnotationColumn(this);
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
	}
}
