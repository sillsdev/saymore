using System.Collections.Generic;
using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	public enum TierType { Text, Audio, Video, TimeOrder } ;

	/// ------------------------------------------------------------------------------------
	public interface ITier
	{
		string DisplayName { get; }
		TierType DataType { get; }
		string Locale { get; }
		IEnumerable<ITier> DependentTiers { get; }
		TierColumnBase GridColumn { get; }

		ITier Copy();
		IEnumerable<ISegment> GetAllSegments();
		ISegment GetSegment(int index);
		bool TryGetSegment(int index, out ISegment segment);
		object GetTierClipboardData(out string dataFormat);
		bool RemoveSegment(int index);
	}
}