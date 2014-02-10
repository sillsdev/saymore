using System.IO;

namespace SayMore.Transcription.Model.Exporters
{
	/// <summary>
	/// Exports the text of a tier to an SRT format text file
	/// </summary>
	public class SRTFormatSubTitleExporter
	{
		public static void Export(string outputFilePath, TierBase tier)
		{
			const string ktimeFormat = "hh\\:mm\\:ss\\,ff";
			using (var stream = File.CreateText(outputFilePath))
			{
				int index = 1;
				foreach (var segment in tier.Segments)
				{
					stream.WriteLine(index);
					stream.WriteLine(segment.TimeRange.Start.ToString(ktimeFormat) + " --> " + segment.TimeRange.End.ToString(ktimeFormat));
					stream.WriteLine(segment.Text);
					stream.WriteLine();
					index++;
				}
			}
		}

		//public static void Export(string outputFilePath, TierBase tier)
		//{
		//    const string ktimeFormat = "hh\\:mm\\:ss\\.ff";
		//    using (var stream = File.CreateText(outputFilePath))
		//    {
		//        stream.WriteLine("[ScriptInfo]");
		//        stream.WriteLine("ScriptType: v4.00+");
		//        stream.WriteLine();
		//        stream.WriteLine("[V4+ Styles]");
		//        stream.WriteLine("Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, AlphaLevel, Encoding");
		//        stream.WriteLine("Style: Default,Arial,16,&Hffffff,&Hffffff,&H0,&H0,0,0,0,1,1,0,2,10,10,10,0,0");
		//        stream.WriteLine();
		//        stream.WriteLine("[Events");
		//        stream.WriteLine("Format: Layer, Start, End, Style, Text");

		//        foreach (var segment in tier.Segments)
		//            stream.WriteLine("Dialogue: 0,{0},{1},Default,{2}", segment.TimeRange.Start.ToString(ktimeFormat), segment.TimeRange.End.ToString(ktimeFormat), segment.Text);

		//        stream.Close();
		//    }
		//}
	}
}
