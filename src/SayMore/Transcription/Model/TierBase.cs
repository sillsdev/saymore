using System.Collections.Generic;
using System.Linq;
using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Implements a generic tier used to derive other tier types.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TierBase : ITier
	{
		protected List<ISegment> _segments = new List<ISegment>();

		public virtual string DisplayName { get; protected set; }
		public virtual TierType DataType { get; protected set; }
		public virtual string Locale { get; protected set; }
		public virtual TierColumnBase GridColumn { get; protected set; }

		protected readonly List<ITier> _dependentTiers = new List<ITier>();

		/// ------------------------------------------------------------------------------------
		public TierBase(string displayName)
		{
			DisplayName = displayName;
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ITier> DependentTiers
		{
			get { return _dependentTiers; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual void AddDependentTier(ITier tier)
		{
			_dependentTiers.Add(tier);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void AddDependentTierRange(IEnumerable<ITier> tiers)
		{
			_dependentTiers.AddRange(tiers);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ISegment> GetAllSegments()
		{
			return _segments;
		}

		/// ------------------------------------------------------------------------------------
		public virtual ISegment GetSegment(int index)
		{
			return _segments.ElementAt(index);
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool TryGetSegment(int index, out ISegment segment)
		{
			segment = null;

			if (index < 0 || index >= _segments.Count)
				return false;

			segment = GetSegment(index);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public virtual object GetTierClipboardData(out string dataFormat)
		{
			dataFormat = null;
			return null;
		}
	}
}
