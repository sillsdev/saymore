using System.Collections.Generic;
using SayMore.Transcription.UI;

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
		bool TryGetSegment(int index, out ISegment segment);
		TierColumnBase GridColumn { get; }
	}
}