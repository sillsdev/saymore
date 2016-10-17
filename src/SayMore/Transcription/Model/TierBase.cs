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
	public enum AudioRecordingType
	{
		Source = 1,
		Careful = 2,
		Translation = 4
	}

	public enum OralAnnotationType
	{
		CarefulSpeech,
		Translation,
	}

	public enum TextAnnotationType
	{
		Transcription,
		Translation,
	}

	/// ----------------------------------------------------------------------------------------
	public class SegmentCollection : Collection<AnnotationSegment>
	{
		/// ------------------------------------------------------------------------------------
		public AnnotationSegment GetLast()
		{
			return (Count == 0 ? null : this[Count - 1]);
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Implements a generic tier from which to derive other tier types.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public abstract class TierBase
	{
		public virtual SegmentCollection Segments { get; protected set; }
		public virtual string Id { get; protected set; }
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
		protected TierBase(string id, Func<TierBase, TierColumnBase> tierColumnProvider) : this()
		{
			Id = id;
			TierType = TierType.Other;
			Locale = "ipa-ext";

			if (tierColumnProvider != null)
				GridColumn = tierColumnProvider(this);
		}

		/// ------------------------------------------------------------------------------------
		public virtual string DisplayName
		{
			get { return Id; }
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

		/// ------------------------------------------------------------------------------------
		public virtual bool RemoveSegment(int index)
		{
			if (index < 0 || index >= Segments.Count)
				return false;

			Segments.RemoveAt(index);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool TryGetSegment(int index, out AnnotationSegment segment)
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
