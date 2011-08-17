using System;
using System.IO;
using NAudio.Wave;
using Palaso.CommandLineProcessing;
using Palaso.Progress.LogBox;
using SayMore.Properties;

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
				Error = new FileNotFoundException("Media file does not exist.", MediaFileName);
				return;
			}

			// First, check if media file is a video file. If it is, then, using ffmpeg,
			// extract the audio in the desired format.
			if (Settings.Default.VideoFileExtensions.Contains(Path.GetExtension(MediaFileName).ToLower()))
			{
				var execResult = CreateFFmpegGeneratedAudioFile();
				if (execResult.ExitCode == 0)
					Stream = new WaveFileReader(_temporaryWavFile);
				else
				{
					var msg = "There was an error extracting audio from the video file '{0}'\n\n{1}";
					Error = new Exception(string.Format(msg, execResult.StandardError));
				}

				return;
			}

			WaveStream stream = new WaveFileReader(MediaFileName);

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
						var msg = "There was an error converting the audio file '{0}' to the correct format.\n\n{1}";
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
