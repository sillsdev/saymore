using System.Collections.Generic;
using System.IO;
using Nini.Ini;
using Palaso.IO;
using SayMore.Properties;
using SayMore.UI;

namespace SayMore.Media.FFmpeg
{
	/// ----------------------------------------------------------------------------------------
	public class FFmpegConversionInfo
	{
		public string Name { get; protected set; }
		public string OutputExtension { get; protected set; }
		public string Comments { get; protected set; }
		public string CommandLine { get; protected set; }
		public string ApplicableFileType { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public static IEnumerable<FFmpegConversionInfo> GetConversions(string fileToConvert)
		{
			var iniFile = FileLocator.GetFileDistributedWithApplication("FFmpegConversions.ini");
			var ffmpegConversions = new IniDocument(iniFile);
			var typeToShow = string.Empty;

			if (Settings.Default.AudioFileExtensions.Contains(Path.GetExtension(fileToConvert)))
				typeToShow = "audio";
			else if (Settings.Default.VideoFileExtensions.Contains(Path.GetExtension(fileToConvert)))
				typeToShow = "video";

			for (int i = 0; i < ffmpegConversions.Sections.Count; i++)
			{
				var applicableFileType = ffmpegConversions.Sections[i].GetValue("applicableFileType");
				if (applicableFileType != null && applicableFileType.Contains(typeToShow))
				{
					yield return new FFmpegConversionInfo
					{
						Name = ffmpegConversions.Sections[i].Name,
						OutputExtension = ffmpegConversions.Sections[i].GetValue("outputFileExtension"),
						CommandLine = ffmpegConversions.Sections[i].GetValue("commandLine"),
						Comments = ffmpegConversions.Sections[i].GetValue("comments"),
						ApplicableFileType = ffmpegConversions.Sections[i].GetValue("applicableFileType")
					};
				}
			}

			if (typeToShow == "video")
			{
				yield return new FFmpegConversionInfo
				{
					Name = ConvertMediaDlg.GetFactoryConvertToH263Mp4ConversionName(),
					OutputExtension = "mp4",
					CommandLine = "-vb {vb} -codec:v mpeg4 -ab {ab} -codec:a aac -strict -2",
					ApplicableFileType = "video"
				};
			}

			yield return new FFmpegConversionInfo
			{
				Name = ConvertMediaDlg.GetFactoryExtractToStandardPcmConversionName(),
				OutputExtension = "wav",
				CommandLine = "-vn -codec:a {pcm}",
				ApplicableFileType = "audio/video"
			};

			yield return new FFmpegConversionInfo
			{
				Name = ConvertMediaDlg.GetFactoryExtractToMp3AudioConversionName(),
				OutputExtension = "mp3",
				CommandLine = "-vn -codec:a libmp3lame -ac 1",
				ApplicableFileType = "audio/video"
			};
		}

		/// ------------------------------------------------------------------------------------
		public static FFmpegConversionInfo CreateForTest(string extension, string commandLine)
		{
			return new FFmpegConversionInfo
			{
				Name = "TestConversion",
				OutputExtension = extension,
				CommandLine = commandLine,
			};
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}
	}
}
