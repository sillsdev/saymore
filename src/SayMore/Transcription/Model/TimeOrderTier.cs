using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	public class TimeOrderTier : TierBase
	{
		public string MediaFileName { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public TimeOrderTier(string filename) :
			this(Program.GetString("TierNames.OriginalRecording", "Original"), filename)
		{
		}

		/// ------------------------------------------------------------------------------------
		public TimeOrderTier(string displayName, string filename) : base(displayName)
		{
			DataType = TierType.TimeOrder;
			MediaFileName = filename;
			GridColumn = new AudioWaveFormColumn(this);
		}

		/// ------------------------------------------------------------------------------------
		public TimeOrderSegment AddSegment(float start, float stop)
		{
			var segment = new TimeOrderSegment(this, start, stop);
			_segments.Add(segment);
			return segment;
		}
	}
}
