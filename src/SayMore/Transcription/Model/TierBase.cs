using System;
using System.Collections.Generic;
using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	public enum TierType
	{
		Other,
		Time,
		Transcription,
		FreeTranslation,
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Implements a generic tier used to derive other tier types.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public abstract class TierBase
	{
		protected List<Segment> _segments = new List<Segment>();

		public virtual string DisplayName { get; protected set; }
		public virtual string Locale { get; protected set; }
		public virtual TierColumnBase GridColumn { get; protected set; }
		public virtual TierType TierType { get; set; }
		public virtual string LinguisticType { get; set; }

		/// ------------------------------------------------------------------------------------
		protected TierBase(string displayName, Func<TierBase, TierColumnBase> tierColumnProvider)
		{
			DisplayName = displayName;
			TierType = TierType.Other;
			Locale = "ipa-ext";

			if (tierColumnProvider != null)
				GridColumn = tierColumnProvider(this);
		}

		/// ------------------------------------------------------------------------------------
		public virtual TierBase Copy()
		{
			var copiedTier = GetNewTierInstance();

			foreach (var seg in _segments)
				copiedTier._segments.Add(seg.Copy(copiedTier));

			return copiedTier;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual TierBase GetNewTierInstance()
		{
			throw new NotImplementedException();
		}

		///// ------------------------------------------------------------------------------------
		//public virtual bool IsTimeTier
		//{
		//    get { return false; }
		//    set { }
		//}

		///// ------------------------------------------------------------------------------------
		//public virtual bool IsTextTranscriptionTier
		//{
		//    get { return false; }
		//    set { }
		//}

		/// ------------------------------------------------------------------------------------
		//public virtual void AddDependentTier(ITier tier)
		//{
		//    _dependentTiers.Add(tier);
		//}

		///// ------------------------------------------------------------------------------------
		//public virtual void AddDependentTierRange(IEnumerable<ITier> tiers)
		//{
		//    _dependentTiers.AddRange(tiers);
		//}

		/// ------------------------------------------------------------------------------------
		public virtual bool RemoveSegment(int index)
		{
			if (index < 0 || index >= _segments.Count)
				return false;

			_segments.RemoveAt(index);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<Segment> Segments
		{
			get { return _segments; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool TryGetSegment(int index, out Segment segment)
		{
			segment = null;

			if (index < 0 || index >= _segments.Count)
				return false;

			segment = _segments[index];
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public virtual object GetTierClipboardData(out string dataFormat)
		{
			dataFormat = null;
			return null;
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return DisplayName;
		}
	}
}
