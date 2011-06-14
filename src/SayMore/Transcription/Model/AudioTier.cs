using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class AudioTier : TierBase
	{
		private readonly string _filename;

		/// ------------------------------------------------------------------------------------
		public AudioTier(string displayName, string filename) : base(displayName)
		{
			DataType = TierType.Audio;
			_filename = filename;
			GridColumn = new AudioWaveFormColumn(this);
		}

		/// ------------------------------------------------------------------------------------
		public AudioSegment AddSegment(float start, float length)
		{
			var segment = new AudioSegment(this, _filename, start, length);
			_segments.Add(segment);
			return segment;
		}
	}
}
