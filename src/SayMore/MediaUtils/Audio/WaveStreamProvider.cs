using System;
using System.IO;
using Localization;
using NAudio.Wave;
using Palaso.UI.WindowsForms.Miscellaneous;
using SayMore.Transcription.Model;

namespace SayMore.Media.Audio
{
	public class WaveStreamProvider : IDisposable
	{
		private string _temporaryWavFile;

		public Exception Error { get; private set; }
		public string MediaFileName { get; private set; }
		public WaveFormat PreferredOutputFormat { get; private set; }
		public WaveStream Stream { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// Gets an object that can provide a stream for the given media file. If necessary,
		/// the data will be up-sampled to meet the specs in the preferredOutputFormat.
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
				// First, just try to open the file as a wave file. If that fails or has a
				// format that is not as good as we want, convert it.
				if (AudioUtils.GetIsFileStandardPcm(mediaFile))
				{
					Stream = new WaveFileReader(mediaFile);
					var format = Stream.WaveFormat;
					if (format.BitsPerSample >= PreferredOutputFormat.BitsPerSample &&
						format.Channels >= PreferredOutputFormat.Channels &&
						format.SampleRate >= PreferredOutputFormat.SampleRate)
					{
						return;
					}
				}
			}
			catch { }

			_temporaryWavFile = Path.ChangeExtension(Path.GetTempFileName(), ".wav");

			Exception error;
			WaitCursor.Show();
			Stream = AudioUtils.ConvertToStandardPcmStream(mediaFile, _temporaryWavFile, PreferredOutputFormat, out error);
			WaitCursor.Hide();
			Error = error;
		}

		/// ------------------------------------------------------------------------------------
		public WaveStream GetStreamSubset(TimeRange segmentRange)
		{
			return (segmentRange.DurationSeconds > 0 ? new WaveSegmentStream(Stream,
				segmentRange.Start, segmentRange.Duration) : Stream);
		}
	}
}
