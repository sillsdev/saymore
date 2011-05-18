using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SayMore.Transcription.Model
{
	public interface ISegmentRepository
	{
		IEnumerable<ISegment> GetAllSegments();
		IEnumerable<ITier> GetAllTiers();
		ISegment GetSegment(int i);
	}
}
