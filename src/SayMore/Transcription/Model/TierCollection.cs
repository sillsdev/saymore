using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class TierCollection : Collection<TierBase>
	{
		/// ------------------------------------------------------------------------------------
		public static TierCollection LoadFromAnnotationFile(string annotationFile)
		{
			var helper = AnnotationFileHelper.Load(annotationFile);
			return helper.GetTierCollection();
		}

		/// ------------------------------------------------------------------------------------
		public TierCollection Copy()
		{
			var copy = new TierCollection();

			foreach (var tier in this)
				copy.Add(tier.Copy());

			return copy;
		}

		///// ------------------------------------------------------------------------------------
		//public string GetPathToCarefulSpeechFileForSegment(TimeSpan start, TimeSpan end)
		//{
		//    var segment = this.FirstOrDefault(s => s.Start == start && s.End == end);

		//    return Path.Combine(_tempOralAnnotationsFolder,
		//        string.Format(Settings.Default.OralAnnotationSegmentFileFormat,
		//        (float)start.TotalSeconds, (float)end.TotalSeconds, affix));
		//}

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

		/// ------------------------------------------------------------------------------------
		public string Save(string annotatedMediaFile)
		{
			return AnnotationFileHelper.Save(annotatedMediaFile, this);
		}
	}
}
