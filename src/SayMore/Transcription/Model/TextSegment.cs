
namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class TextSegment : SegmentBase, ITextSegment
	{
		string _text;

		/// ------------------------------------------------------------------------------------
		public TextSegment(ITier tier, string text) : base(tier)
		{
			SetText(text);
		}

		/// ------------------------------------------------------------------------------------
		public string GetText()
		{
			return _text;
		}

		/// ------------------------------------------------------------------------------------
		public void SetText(string text)
		{
			_text = text;
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return GetText();
		}
	}
}
