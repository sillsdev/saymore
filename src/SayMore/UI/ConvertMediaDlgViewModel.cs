using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SayMore.Media.FFmpeg;

namespace SayMore.UI
{
	public class ConvertMediaDlgViewModel
	{
		public string InputFile { get; private set; }
		public FFmpegConversionInfo SelectedConversion { get; set; }
		public FFmpegConversionInfo[] AvailableConversions { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ConvertMediaDlgViewModel(string inputFile, string initialConversionName)
		{
			InputFile = inputFile;

			AvailableConversions = FFmpegConversionInfo.GetConversions().OrderBy(c => c.Name).ToArray();
			SelectedConversion = AvailableConversions.FirstOrDefault(c => c.Name == initialConversionName) ?? AvailableConversions[0];
		}

	}
}
