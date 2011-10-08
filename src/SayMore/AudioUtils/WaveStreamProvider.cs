using System;
using System.IO;
using NAudio.Wave;
using Palaso.CommandLineProcessing;
using Palaso.Progress.LogBox;

namespace SayMore.AudioUtils
{
	public class WaveStreamProvider : IDisposable
	{
		private string _temporaryWavFile;

		public Exception Error { get; private set; }
		public string MediaFileName { get; private set; }
		public WaveFormat OutputFormat { get; private set; }
		public WaveStream Stream { get; private set; }

		/// ------------------------------------------------------------------------------------
		public static WaveStreamProvider Create(WaveFormat outputFormat, string mediaFilename)
		{
			var provider = new WaveStreamProvider(outputFormat, mediaFilename);
			provider.BuildWaveStream();
			return provider;
		}

		/// ------------------------------------------------------------------------------------
		public WaveStreamProvider(WaveFormat outputFormat, string mediaFilename)
		{
			OutputFormat = outputFormat;
			MediaFileName = mediaFilename;
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (Stream != null)
				Stream.Dispose();

			if (_temporaryWavFile != null && File.Exists(_temporaryWavFile))
				File.Delete(_temporaryWavFile);

			Stream = null;
			_temporaryWavFile = null;
		}

		/// ------------------------------------------------------------------------------------
		private void BuildWaveStream()
		{
			if (!File.Exists(MediaFileName))
			{
				Error = new FileNotFoundException(
					Program.GetString("SoundFileUtils.MediaFileDoesNotExistErrorMsg", "Media file does not exist."),
					MediaFileName);

				return;
			}

			WaveStream stream;

			try
			{
				// First, just try to open the file as a wave file. If that fails, use
				// ffmpeg in an attempt to get wave audio out of the file.
				stream = new WaveFileReader(MediaFileName);
			}
			catch
			{
				var execResult = CreateFFmpegGeneratedAudioFile();
				if (execResult.ExitCode == 0)
					Stream = new WaveFileReader(_temporaryWavFile);
				else
				{
					var msg = Program.GetString("SoundFileUtils.ExtractingAudioError",
						"There was an error extracting audio from the media file '{0}'\n\n{1}",
						"Second parameter is the error message.");

					Error = new Exception(string.Format(msg, execResult.StandardError));
				}

				return;
			}

			if (stream.WaveFormat.BitsPerSample != OutputFormat.BitsPerSample ||
				stream.WaveFormat.SampleRate != OutputFormat.SampleRate ||
				stream.WaveFormat.Channels != OutputFormat.Channels)
			{
				try
				{
					stream = new WaveFormatConversionStream(OutputFormat, stream);
				}
				catch (Exception e)
				{
					var execResult = CreateFFmpegGeneratedAudioFile();
					if (execResult.ExitCode == 0)
						stream = new WaveFileReader(_temporaryWavFile);
					else
					{
						stream = null;
						var msg = Program.GetString("SoundFileUtils.ConvertingAudioError",
							"There was an error converting the audio file '{0}' to the correct format.\n\n{1}",
							"Second parameter is the error message.");

						Error = new Exception(string.Format(msg, execResult.StandardError), e);
						return;
					}
				}
			}

			Stream = stream;
		}

		/// ------------------------------------------------------------------------------------
		private ExecutionResult CreateFFmpegGeneratedAudioFile()
		{
			_temporaryWavFile = Path.ChangeExtension(Path.GetTempFileName(), ".wav");

			return Palaso.Media.FFmpegRunner.ExtractPcmAudio(MediaFileName,
				_temporaryWavFile, OutputFormat.BitsPerSample, OutputFormat.SampleRate,
				OutputFormat.Channels, new NullProgress());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// <param name="startOffsetInSource">Number of seconds into media where to begin.</param>
		/// <param name="returnedStreamLength">Number of seconds of media to return in stream.</param>
		/// ------------------------------------------------------------------------------------
		public WaveStream GetStreamSubset(double startOffsetInSource, double returnedStreamLength)
		{
			return (returnedStreamLength > 0d ? new WaveSegmentStream(Stream,
				TimeSpan.FromSeconds(startOffsetInSource), TimeSpan.FromSeconds(returnedStreamLength)) : Stream);
		}
	}
}
