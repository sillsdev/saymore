using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SayMore.Transcription.Model
{
	public interface ISegmentRepository
	{
		IEnumerable<ISegment> GetAllSegments();
		ISegment GetSegment(int i);
	}

	public interface ITierRepository
	{
		IEnumerable<ITier> GetAllTiers();
		ITier GetTier(int i);
	}
}
