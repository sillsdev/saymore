using System;
using System.IO;
using Localization;
using NAudio.Wave;

namespace SayMore.AudioUtils
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
				// ffmpeg in an attempt to get wave audio out of the file.
				if (WaveFileUtils.GetIsFilePlainPcm(mediaFile))
				{
					Stream = new WaveFileReader(mediaFile);
					return;
				}
			}
			catch { }

			Exception error;

			_temporaryWavFile = Path.ChangeExtension(Path.GetTempFileName(), ".wav");

			Stream = WaveFileUtils.GetPlainPcmStream(mediaFile, _temporaryWavFile,
				PreferredOutputFormat, out error);

			Error = error;
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
