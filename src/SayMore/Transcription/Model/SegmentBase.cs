
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

		/// ------------------------------------------------------------------------------------
		public virtual ISegment Copy(ITier owningTier)
		{
			return GetNewSegmentInstance(owningTier);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual ISegment GetNewSegmentInstance(ITier owningTier)
		{
			throw new System.NotImplementedException();
		}
	}
}
