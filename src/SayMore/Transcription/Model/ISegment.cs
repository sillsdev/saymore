
namespace SayMore.Transcription.Model
{
	/// ------------------------------------------------------------------------------------
	public interface ISegment
	{
		ITier Tier { get; }
	}

	/// ------------------------------------------------------------------------------------
	public interface IMediaSegment : ISegment
	{
		string MediaFile { get; }
		float MediaStart { get; }
		float MediaLength { get; }
	}

	/// ------------------------------------------------------------------------------------
	public interface ITextSegment : ISegment
	{
		string GetText();

		/// <summary>
		/// nb: this is not a property because it may trigger non-trivial things, like saving to disk
		/// </summary>
		void SetText(string text);

		string ToString();
	}
}