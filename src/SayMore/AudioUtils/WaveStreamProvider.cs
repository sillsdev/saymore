using System;
using System.IO;
using Localization;
using NAudio.Wave;
using Palaso.Progress;
using SayMore.Transcription.Model;

namespace SayMore.Media
{
	public class WaveStreamProvider : IDisposable
	{
		private string _temporaryWavFile;

		public Exception Error { get; private set; }
		public string MediaFileName { get; private set; }
		public WaveFormat PreferredOutputFormat { get; private set; }
		public WaveStream Stream { get; private set; }

		/// ------------------------------------------------------------------------------------
		public static WaveStreamProvider Create(WaveFormat preferredOutputFormat, string mediaFilename)
		{
			var provider = new WaveStreamProvider(preferredOutputFormat, mediaFilename);
			provider.BuildWaveStream(mediaFilename);
			return provider;
		}

		/// ------------------------------------------------------------------------------------
		public WaveStreamProvider(WaveFormat outputFormat, string mediaFilename)
		{
			PreferredOutputFormat = outputFormat;
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
		private void BuildWaveStream(string mediaFile)
		{
			if (!File.Exists(MediaFileName))
			{
				Error = new FileNotFoundException(
					LocalizationManager.GetString("SoundFileUtils.MediaFileDoesNotExistErrorMsg",
					"Media file does not exist."), mediaFile);

				return;
			}

			try
			{
				// First, just try to open the file as a wave file. If that fails, use
				// ffmpeg or mplayer in an attempt to get wave audio out of the file.
				if (AudioUtils.GetIsFileStandardPcm(mediaFile))
				{
					Stream = new WaveFileReader(mediaFile);
					return;
				}
			}
			catch { }

			_temporaryWavFile = Path.ChangeExtension(Path.GetTempFileName(), ".wav");

			Exception error;
			WaitCursor.Show();

			Stream = AudioUtils.ConvertToStandardPcmStream(mediaFile,
				_temporaryWavFile, PreferredOutputFormat, out error);

			WaitCursor.Hide();
			Error = error;
		}

		/// ------------------------------------------------------------------------------------
		public WaveStream GetStreamSubset(Segment segment)
		{
			return (segment.TimeRange.DurationSeconds > 0 ? new WaveSegmentStream(Stream,
				segment.TimeRange.Start, segment.TimeRange.Duration) : Stream);
		}
	}
}
