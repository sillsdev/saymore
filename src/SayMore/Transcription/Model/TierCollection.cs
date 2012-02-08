using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class TierCollection : Collection<TierBase>
	{
		/// ------------------------------------------------------------------------------------
		public static TierCollection Create(string annotationFile)
		{
			var helper = AnnotationFileHelper.Load(annotationFile);
			return helper.GetTiers();
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
			return this.FirstOrDefault(t => t is TimeTier) as TimeTier;
		}

		/// ------------------------------------------------------------------------------------
		public TextTier GetFirstTextTier()
		{
			return this.FirstOrDefault(t => t is TextTier) as TextTier;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets all the text tiers other than the first. The first is assumed to be the one
		/// upon whom all others are depend.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<TextTier> GetDependentTextTiers()
		{
			var firstTextTier = GetFirstTextTier();
			return this.OfType<TextTier>().Where(t => t != firstTextTier);
		}

		/// ------------------------------------------------------------------------------------
		public string Save(string annotatedMediaFile)
		{
			return AnnotationFileHelper.Save(annotatedMediaFile, this);
		}
	}
}
