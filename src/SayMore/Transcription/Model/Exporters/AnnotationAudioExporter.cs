using L10NSharp;
using NAudio.Wave;
using SayMore.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace SayMore.Transcription.Model.Exporters
{
	/// <summary>
	/// Exports the selected audio chunk files as a combined MP3 file. 
	/// </summary>
	public class AnnotationAudioExporter
	{
		public static void Export(string waveFileDirectoryName, string filenameFilter, string outputFilePath)
		{
            var files = Directory.GetFiles(waveFileDirectoryName, filenameFilter);

            // sort the files 
            var sorted = new SortedList<float, string>();
            foreach (var file in files)
            {
                var fileFloat = -1f;
                if (float.TryParse(Regex.Match(Path.GetFileName(file), @"^[0-9.]+").Value, out fileFloat))
                {
                    sorted.Add(fileFloat, file);
                }
            }

			string waveFile = null;
			try
			{
				// combine the chunks into a single wave file
				waveFile = CombineChunks(sorted);

				// delete old version of the MP3 file
				if (File.Exists(outputFilePath))
					File.Delete(outputFilePath);

				// if the file still exists, we can't continue
				if (File.Exists(outputFilePath))
				{
					var msg = LocalizationManager.GetString(
								"SessionsView.Transcription.TextAnnotation.ExportMenu.AudioExportFileLocked",
								"Audio export failed because the selected output file already exists and is locked.");
					MessageBox.Show(msg, Program.ProductName);

					return;
				}
				var converter = new ConvertMediaDlgViewModel(waveFile, "Extract audio to mono mp3 audio file (low quality)");
				converter.BeginConversion(null, outputFilePath);
			}
			finally
			{
				if (!string.IsNullOrEmpty(waveFile) && File.Exists(waveFile))
				{
					File.Delete(waveFile);
				}
			}
        }

		private static string CombineChunks(SortedList<float, string> sorted)
		{
			var tempFileName = Path.ChangeExtension(Path.GetTempFileName(), ".wav"); ;

			byte[] buffer = new byte[1024];
			WaveFileWriter waveFileWriter = null;

			// get the audio format
			WaveFormat waveFormat;
			using (WaveFileReader reader = new WaveFileReader(sorted.Values[0]))
			{
				waveFormat = reader.WaveFormat;
			}

			using (waveFileWriter = new WaveFileWriter(tempFileName, waveFormat))
			{
				foreach (KeyValuePair<float, string> kvp in sorted)
				{
					using (WaveFileReader reader = new WaveFileReader(kvp.Value))
					{
						if (!reader.WaveFormat.Equals(waveFormat))
						{
							throw new InvalidOperationException("Can't concatenate WAV Files that don't share the same format");
						}

						int read;
						while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
						{
							waveFileWriter.Write(buffer, 0, read);
						}
					}
				}
			}

			return tempFileName;
		}
	}
}
