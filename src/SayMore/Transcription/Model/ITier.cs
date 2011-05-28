using System.Collections.Generic;

namespace SayMore.Transcription.Model
{
	public enum TierType { Text, Audio, Video } ;

	/// ------------------------------------------------------------------------------------
	public interface ITier
	{
		string DisplayName { get; }
		TierType DataType { get; }
		IEnumerable<ISegment> GetAllSegments();
		ISegment GetSegment(int index);
	}
}