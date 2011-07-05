
namespace SayMore.Transcription.Model
{
	/// ------------------------------------------------------------------------------------
	public interface ISegment
	{
		ITier Tier { get; }
	}

	/// ------------------------------------------------------------------------------------
	public interface ITimeOrderSegment : ISegment
	{
		float Start { get; }
		float Stop { get; }
		float GetLength();
		float GetLength(int decimalPlaces);
	}

	/// ------------------------------------------------------------------------------------
	public interface IMediaSegment : ITimeOrderSegment
	{
		string MediaFile { get; }
	}

	/// ------------------------------------------------------------------------------------
	public interface ITextSegment : ISegment
	{
		string Id { get; set; }

		string GetText();

		/// <summary>
		/// nb: this is not a property because it may trigger non-trivial things, like saving to disk
		/// </summary>
		void SetText(string text);

		string ToString();
	}
}