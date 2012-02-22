using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class TierCollection : Collection<TierBase>
	{
		/// ------------------------------------------------------------------------------------
		public static TierCollection LoadFromAnnotationFile(string eafFilePath)
		{
			var helper = AnnotationFileHelper.Load(eafFilePath);
			var collection = helper.GetTierCollection();
			collection.AnnotatedMediaFile = helper.GetFullPathToMediaFile();

			return collection;
		}

		/// ------------------------------------------------------------------------------------
		public TierCollection()
		{
		}

		/// ------------------------------------------------------------------------------------
		public TierCollection(string annotatedMediaFile)
		{
			AnnotatedMediaFile = annotatedMediaFile;
			Add(new TimeTier(annotatedMediaFile));
			Add(new TextTier(TextTier.TranscriptionTierName) { TierType = TierType.Transcription });
			Add(new TextTier(TextTier.SayMoreFreeTranslationTierName) { TierType = TierType.FreeTranslation });
		}

		/// ------------------------------------------------------------------------------------
		public string AnnotatedMediaFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		public TierCollection Copy()
		{
			var copy = new TierCollection();

			copy.AnnotatedMediaFile = AnnotatedMediaFile;

			foreach (var tier in this)
				copy.Add(tier.Copy());

			return copy;
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoTimeSegmentsExist()
		{
			return (GetTimeTier() != null && GetTimeTier().Segments.Count > 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the segment in each tier at the specified index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool RemoveTierSegments(int index)
		{
			if (Count == 0 || index < 0 || index >= this[0].Segments.Count)
				return false;

			for (int i = 0; i < Count; i++)
				this[i].RemoveSegment(index);

			return true;
		}

		#region Methods for Saving tiers to EAF file
		/// ------------------------------------------------------------------------------------
		public string Save()
		{
			Debug.Assert(AnnotatedMediaFile != null);
			Debug.Assert(File.Exists(AnnotatedMediaFile));
			return Save(AnnotatedMediaFile);
		}

		/// ------------------------------------------------------------------------------------
		public string Save(string annotatedMediaFile)
		{
			return AnnotationFileHelper.Save(annotatedMediaFile, this);
		}

		#endregion

		#region Methods for getting various tiers
		/// ------------------------------------------------------------------------------------
		public TimeTier GetTimeTier()
		{
			return this.FirstOrDefault(t => t.TierType == TierType.Time) as TimeTier;
		}

		/// ------------------------------------------------------------------------------------
		public TextTier GetTranscriptionTier()
		{
			return this.FirstOrDefault(t => t.TierType == TierType.Transcription) as TextTier;
		}

		/// ------------------------------------------------------------------------------------
		public TextTier GetFreeTranslationTier()
		{
			return this.FirstOrDefault(t => t.TierType == TierType.FreeTranslation) as TextTier;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets all the text tiers other than the transcription tier.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<TextTier> GetDependentTextTiers()
		{
			return this.OfType<TextTier>().Where(t => t.TierType != TierType.Transcription);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets all the text tiers other than the transcription and free translation.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<TextTier> GetUserDefinedTextTiers()
		{
			return this.OfType<TextTier>().Where(t => t.TierType != TierType.Transcription &&
				t.TierType != TierType.FreeTranslation);
		}

		#endregion
	}
}
