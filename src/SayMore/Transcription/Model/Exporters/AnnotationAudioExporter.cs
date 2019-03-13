using L10NSharp;
using NAudio.Wave;
using SayMore.UI;
using SIL.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SayMore.Transcription.Model.Exporters
{
	public class AnnotationAudioExporterException : Exception
	{
		public AnnotationAudioExporterException(string msg) : base(msg)
		{
		}
	}


	/// <summary>
	/// Exports the selected audio chunk files as a combined MP3 file. 
	/// </summary>
	public class AnnotationAudioExporter
	{
		public static void Export(string waveFileDirectoryName, string filenameFilter, string outputFilePath)
		{
			try
			{
				System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

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

				// delete old version of the MP3 file
				if (File.Exists(outputFilePath))
					File.Delete(outputFilePath);

				// if the file still exists, we can't continue
				if (File.Exists(outputFilePath))
				{
					var msg = LocalizationManager.GetString(
								"SessionsView.Transcription.TextAnnotation.ExportMenu.AudioExportFileLocked",
								"Audio export failed because the selected output file already exists and is locked.");
					throw new Exception(msg);
				}

				using (var tempFile = TempFile.WithExtension(".wav"))
				{
					// combine the chunks into a single wave file
					CombineChunks(sorted.Values, tempFile.Path);

					// convert to MP3
					ConvertMediaDlg.Show(tempFile.Path, "Extract audio to mono mp3 audio file (low quality)", outputFilePath);
				}
			}
            finally
			{
				System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
			}
        }

		private static void CombineChunks(IEnumerable<string> sorted, string tempFileName)
		{
			byte[] buffer = new byte[1024];
			WaveFileWriter waveFileWriter = null;
			string currentFileName = sorted.First();

			try
			{
				// get the audio format
				WaveFormat waveFormat;
				using (WaveFileReader reader = new WaveFileReader(sorted.First()))
				{
					waveFormat = reader.WaveFormat;
				}

				using (waveFileWriter = new WaveFileWriter(tempFileName, waveFormat))
				{
					foreach (var waveFile in sorted)
					{
						// remember for a possible error message
						currentFileName = waveFile;

						using (WaveFileReader reader = new WaveFileReader(waveFile))
						{
							if (!reader.WaveFormat.Equals(waveFormat))
							{
								var msg = "Can't concatenate WAV Files that don't share the same format.{3}{3}\"{0}\" must have the same format as \"{1}\".{3}{3}The files are located in \"{2}\".";
								throw new AnnotationAudioExporterException(string.Format(msg, Path.GetFileName(waveFile), Path.GetFileName(sorted.First()), Path.GetDirectoryName(waveFile), Environment.NewLine));
							}

							int read;
							while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
							{
								waveFileWriter.Write(buffer, 0, read);
							}
						}
					}
				}
			}
			catch (AnnotationAudioExporterException)
			{
				throw;
			}
			catch (FormatException formatError)
			{
				var msg = string.Format("At least one of the source files is not WAV format.{2}{2}File name: \"{0}\"{2}{2}The files are located in \"{1}\".",
					Path.GetFileName(currentFileName), Path.GetDirectoryName(currentFileName), Environment.NewLine);
				throw new FormatException(msg, formatError);
			}
			catch (Exception error)
			{
				Console.Out.WriteLine(error.Message);
			}
		}
	}
}
