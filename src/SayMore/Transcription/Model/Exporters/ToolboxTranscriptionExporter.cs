using System.IO;

namespace SayMore.Transcription.Model.Exporters
{
	/// <summary>
	/// Exports the text of a tier to a text file with \tx and \ft lines for Toolbox
	/// This output format based on Margetts, Language Documenatation & Conservation Vol. 3, No. 1 June 2009
	/// </summary>
	public class ToolboxTranscriptionExporter
	{
		public static void Export(string referencePrefix, string mediaFileName, string outputFilePath, TierCollection tiers)
		{
			const string ktimeFormat = "00.00###";
			using (var stream = File.CreateText(outputFilePath))
			{
				int count = tiers.GetTimeTier().Segments.Count;
				var timeSegments = tiers.GetTimeTier().Segments;
				var transcriptionSegments = tiers.GetTranscriptionTier(true).Segments;
				var freeTranslationSegments = tiers.GetFreeTranslationTier(true).Segments;

				for (int i = 0; i < count; i++)
				{
					stream.WriteLine("\\ref "+referencePrefix+"_"+(1+i).ToString("0000"));
					var begin = timeSegments[i].TimeRange.StartSeconds.ToString(ktimeFormat);
					var end = timeSegments[i].TimeRange.EndSeconds.ToString(ktimeFormat);
					stream.WriteLine("\\begin " + begin);
					stream.WriteLine("\\end " + end);
					stream.WriteLine("\\media " + mediaFileName + " " + begin + " " + end);
					stream.WriteLine("\\t "+transcriptionSegments[i].Text);
					stream.WriteLine("\\f "+freeTranslationSegments[i].Text);
					stream.WriteLine();
				}
			}
		}
	}
}
