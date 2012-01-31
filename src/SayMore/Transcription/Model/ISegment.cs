
namespace SayMore.Transcription.Model
{
	/// ------------------------------------------------------------------------------------
	public interface ISegment
	{
		ITier Tier { get; }

		/// <summary>Performs a deep copy of the segment</summary>
		ISegment Copy(ITier owningTier);
	}

	/// ------------------------------------------------------------------------------------
	public interface ITimeOrderSegment : ISegment
	{
		/// <summary>Segment's start time in seconds</summary>
		float Start { get; set; }

		/// <summary>Segment's stop time in seconds</summary>
		float Stop { get; set; }

		/// <summary>Segment's length in seconds</summary>
		float GetLength();

		/// <summary>Segment's length in seconds to a specified decimal place</summary>
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