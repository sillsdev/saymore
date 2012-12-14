using System.IO;

namespace SayMore.Transcription.Model.Exporters
{
	/// <summary>
	/// Exports the text of a tier to an SRT format text file
	/// </summary>
	public class AudacityExporter
	{
		public static void Export(string outputFilePath, TierBase tier)
		{
			const string ktimeFormat = "0.000000";
			using (var stream = File.CreateText(outputFilePath))
			{
				int index = 1;
				foreach (var segment in tier.Segments)
				{
					var start = (segment.TimeRange.Start.TotalMilliseconds/1000.0).ToString(ktimeFormat);
					var end = (segment.TimeRange.End.TotalMilliseconds / 1000.0).ToString(ktimeFormat);

					stream.WriteLine(start + "\t" + end+"\t"+(segment.Text ?? ""));
					index++;
				}
				stream.WriteLine();
			}
		}
	}
}
