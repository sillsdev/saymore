using System;
using System.Collections.ObjectModel;
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

	[FlagsAttribute]
	public enum OralAnnotationType
	{
		Original = 1,
		Careful = 2,
		Translation = 4
	}

	/// ----------------------------------------------------------------------------------------
	public class SegmentCollection : Collection<Segment>
	{
		/// ------------------------------------------------------------------------------------
		public Segment GetLast()
		{
			return (Count == 0 ? null : this[Count - 1]);
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Implements a generic tier used to derive other tier types.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public abstract class TierBase
	{
		public virtual SegmentCollection Segments { get; protected set; }
		public virtual string DisplayName { get; protected set; }
		public virtual string Locale { get; protected set; }
		public virtual TierColumnBase GridColumn { get; protected set; }
		public virtual TierType TierType { get; set; }
		public virtual string LinguisticType { get; set; }

		/// ------------------------------------------------------------------------------------
		protected TierBase()
		{
			Segments = new SegmentCollection();
		}

		/// ------------------------------------------------------------------------------------
		protected TierBase(string displayName, Func<TierBase, TierColumnBase> tierColumnProvider) : this()
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
			var copy = GetNewTierInstance();
			copy.TierType = TierType;
			copy.LinguisticType = LinguisticType;
			copy.Locale = Locale;

			foreach (var seg in Segments)
				copy.Segments.Add(seg.Copy(copy));

			return copy;
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
			if (index < 0 || index >= Segments.Count)
				return false;

			Segments.RemoveAt(index);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool TryGetSegment(int index, out Segment segment)
		{
			segment = null;

			if (index < 0 || index >= Segments.Count)
				return false;

			segment = Segments[index];
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
