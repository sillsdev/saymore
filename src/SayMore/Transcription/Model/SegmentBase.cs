
namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class SegmentBase : ISegment
	{
		public ITier Tier { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public SegmentBase(ITier tier)
		{
			Tier = tier;
		}
	}
}
