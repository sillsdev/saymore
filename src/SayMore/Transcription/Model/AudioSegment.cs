
namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class AudioSegment : TimeOrderSegment, IMediaSegment
	{
		public string MediaFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		public AudioSegment(ITier tier, string filename, float start, float stop)
			: base(tier, start, stop)
		{
			MediaFile = filename;
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return string.Format("{0};  {1}", MediaFile, base.ToString());
		}
	}
}
