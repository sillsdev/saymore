using System.IO;

namespace SayMore.Transcription.Model.Exporters
{
	/// <summary>
	/// Exports the text of a tier to a comma-separated format text file
	/// </summary>
	public class CSVTranscriptionExporter
	{
		public static void Export(string outputFilePath, TierCollection tiers)
		{
			const string ktimeFormat = "hh\\:mm\\:ss\\:ff";
			using (var stream = File.CreateText(outputFilePath))
			{
				int count = tiers.GetTimeTier().Segments.Count;
				var timeSegments = tiers.GetTimeTier().Segments;
				var transcriptionSegments = tiers.GetTranscriptionTier(true).Segments;
				var freeTranslationSegments = tiers.GetFreeTranslationTier(true).Segments;

				for (int i = 0; i < count; i++)
				{
					stream.Write(i+1+",");
					stream.Write(timeSegments[i].TimeRange.Start.ToString(ktimeFormat));
					stream.Write(",");
					stream.Write(timeSegments[i].TimeRange.End.ToString(ktimeFormat) + ",");
					stream.Write(Escape(transcriptionSegments[i].Text));
					stream.Write(",");
					stream.Write(Escape(freeTranslationSegments[i].Text));
					stream.WriteLine();
				}
			}
		}
		public static string Escape(string s)
		{
			if (s == null) //I can't see how this would happen, but it's my only clue for SP-688
				return string.Empty;

			if (s.Contains(QUOTE))
				s = s.Replace(QUOTE, ESCAPED_QUOTE);

			if (s.IndexOfAny(CHARACTERS_THAT_MUST_BE_QUOTED) > -1)
				s = QUOTE + s + QUOTE;

			return s;
		}
		private const string QUOTE = "\"";
		private const string ESCAPED_QUOTE = "\"\"";
		private static char[] CHARACTERS_THAT_MUST_BE_QUOTED = { ',', '"', '\n' };
	}
}
