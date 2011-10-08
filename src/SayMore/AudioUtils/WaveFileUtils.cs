using System.Linq;
using NAudio.Wave;
using NAudio.Wave.Compression;
using Palaso.Reporting;

namespace SayMore.AudioUtils
{
	public class WaveFileUtils
	{
		/// ------------------------------------------------------------------------------------
		public static WaveFormat GetDefaultWaveFormat(int channels)
		{
			WaveFormat bestFormat = null;

			var pcmDriver = AcmDriver.EnumerateAcmDrivers().SingleOrDefault(d => d.ShortName == "MS-PCM");
			if (pcmDriver != null)
			{
				pcmDriver.Open();

				var formatTag = pcmDriver.FormatTags.SingleOrDefault(t => t.FormatTag == WaveFormatEncoding.Pcm);
				if (formatTag != null)
				{
					foreach (var fmt in pcmDriver.GetFormats(formatTag))
					{
						if (bestFormat == null ||
							fmt.WaveFormat.BitsPerSample > bestFormat.BitsPerSample ||
							fmt.WaveFormat.SampleRate > bestFormat.SampleRate)
						{
							bestFormat = fmt.WaveFormat;
						}
					}
				}
				else
				{
					var msg = Program.GetString("SoundFileUtils.ErrorFindingPcmConversionCapabilitiesMsg",
						"There was an error trying to find PCM audio conversion capabilities on this computer. Ensure that you have a PCM sound driver installed.");

					ErrorReport.NotifyUserOfProblem(msg);
				}

				pcmDriver.Close();
			}
			else
			{
				var msg = Program.GetString("SoundFileUtils.ErrorFindingPcmAudioDriverMsg",
					"There was an error trying to find a PCM audio driver on this computer. Ensure that you have a PCM sound driver installed.");

				ErrorReport.NotifyUserOfProblem(msg);
			}

			return new WaveFormat(bestFormat.SampleRate, bestFormat.BitsPerSample, channels);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Joins two or more wave files and writes them to the specified output wave file.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public static TimeSpan[] Join(IEnumerable<string> inputWaveFiles, string outputWaveFile)
		//{
		//    return Join(44100, 1, inputWaveFiles, outputWaveFile);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Joins two or more wave files and writes them to the specified output wave file.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public static TimeSpan[] Join(int ouputFileSampleRate, int ouputFileChannels,
		//    IEnumerable<string> inputWaveFiles, string outputWaveFile)
		//{
		//    if (inputWaveFiles == null)
		//        throw new ArgumentNullException("inputWaveFiles");

		//    var inputFiles = inputWaveFiles.ToArray();

		//    if (inputFiles.Length == 0)
		//        throw new Exception("List of wave files to join must contain at least one item.");

		//    var writer = new WaveFileWriter(outputWaveFile,
		//        new WaveFormat(ouputFileSampleRate, ouputFileChannels));

		//    try
		//    {
		//        var fileLengths = inputFiles.Select(f => WriteSingleWaveFile(f, ref writer)).ToArray();
		//        return fileLengths;
		//    }
		//    finally
		//    {
		//        writer.Close();
		//        writer.Dispose();
		//    }
		//}

		///// ------------------------------------------------------------------------------------
		//private static TimeSpan WriteSingleWaveFile(string inputWaveFile, ref WaveFileWriter writer)
		//{
		//    var reader = new WaveFileReader(inputWaveFile);
		//    var totalTime = reader.TotalTime;
		//    var buffer = new byte[4096];
		//    var count = int.MaxValue;

		//    while (count >= 4096)
		//    {
		//        count = reader.Read(buffer, 0, 4096);
		//        if (count > 0)
		//            writer.Write(buffer, 0, count);
		//    }

		//    reader.Close();
		//    reader.Dispose();
		//    return totalTime;
		//}
	}
}
