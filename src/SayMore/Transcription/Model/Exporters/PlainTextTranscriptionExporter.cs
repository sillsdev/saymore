using System.IO;
using Localization;


namespace SayMore.Transcription.Model
{
	/// <summary>
	/// Exports the text of a tier to an SRT format text file
	/// </summary>
	public class PlainTextTranscriptionExporter
	{
		public static void Export(string outputFilePath, TierCollection tiers)
		{
			using (var stream = File.CreateText(outputFilePath))
			{
				stream.WriteLine("-- " + LocalizationManager.GetString("SessionsView.Transcription.TierDisplayNames.Transcription", "Transcription") + " --");
				stream.WriteLine();
				foreach (var segment in tiers.GetTranscriptionTier().Segments)
				{
					stream.WriteLine(segment.Text);
				}

				stream.WriteLine();
				stream.WriteLine();
				stream.WriteLine("-- " + LocalizationManager.GetString("SessionsView.Transcription.TierDisplayNames.FreeTranslation", "Free Translation") + " --");
				stream.WriteLine();
				foreach (var segment in tiers.GetFreeTranslationTier().Segments)
				{
					stream.WriteLine(segment.Text);
				}
			}
		}
	}
}
