
namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class AudioSegment : SegmentBase, IMediaSegment
	{
		public string MediaFile { get; private set; }
		public float MediaStart { get; private set; }
		public float MediaLength { get; private set; }

		/// ------------------------------------------------------------------------------------
		public AudioSegment(ITier tier, string filename, float start, float length)
			: base(tier)
		{
			MediaFile = filename;
			MediaStart = start;
			MediaLength = length;
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return string.Format("{0};  Start={1};  Length={2}",
				MediaFile, MediaStart, MediaLength);
		}
	}
}
