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
				// mplayer in an attempt to get wave audio out of the file.
				if (AudioUtils.GetIsFileStandardPcm(mediaFile))
				{
					Stream = new WaveFileReader(mediaFile);
					return;
				}
			}
			catch { }

			// REVIEW: I (TomB) don't think we should ever get here. If we do, we're probably in
			// trouble because this code does UI stuff but is not invoked on the UI thread.
			_temporaryWavFile = Path.ChangeExtension(Path.GetTempFileName(), ".wav");

			Exception error;
			WaitCursor.Show();
			Stream = AudioUtils.ConvertToStandardPcmStream(mediaFile, _temporaryWavFile, out error);
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
