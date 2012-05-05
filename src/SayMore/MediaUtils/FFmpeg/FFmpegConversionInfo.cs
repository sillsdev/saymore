using System.Collections.Generic;
using Nini.Ini;
using Palaso.IO;
using SayMore.Model.Files;
using SayMore.UI;

namespace SayMore.Media.FFmpeg
{
	/// ----------------------------------------------------------------------------------------
	public class FFmpegConversionInfo
	{
		public string Name { get; protected set; }
		public string OutputExtension { get; protected set; }
		public string CommandLine { get; protected set; }
		public string ApplicableFileType { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public static IEnumerable<FFmpegConversionInfo> GetConversions()
		{
			var iniFile = FileLocator.GetFileDistributedWithApplication("FFmpegConversions.ini");
			var ffmpegConversions = new IniDocument(iniFile);

			for (int i = 0; i < ffmpegConversions.Sections.Count; i++)
			{
				yield return new FFmpegConversionInfo
				{
					Name = ffmpegConversions.Sections[i].Name,
					OutputExtension = ffmpegConversions.Sections[i].GetValue("outputFileExtension"),
					CommandLine = ffmpegConversions.Sections[i].GetValue("commandLine"),
					ApplicableFileType = ffmpegConversions.Sections[i].GetValue("applicableFileType")
				};
			}

			yield return new FFmpegConversionInfo
			{
				Name = ConvertMediaDlg.GetFactoryMp4ConversionName(),
				OutputExtension = "mp4",
				CommandLine = "-vb {vb} -vcodec mpeg4 -ab {ab} -acodec aac -strict -2",
				ApplicableFileType = "video"
			};

			yield return new FFmpegConversionInfo
			{
				Name = VideoFileType.GetExtractToMp3AudioCommandString(),
				OutputExtension = "mp3",
				CommandLine = "-vn -acodec libmp3lame -ac 1",
				ApplicableFileType = "video"
			};
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}
	}
}
