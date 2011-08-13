using System;
using System.IO;
using System.Linq;
using NAudio.Wave;
using SayMore.AudioUtils;
using SayMore.Properties;
using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class generates an oral annotation file by interleaving into a single, 3-channel
	/// audio file, original recording, careful speech and oral translation segments.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class OralAnnotationFileGenerator
	{
		private readonly TimeOrderTier _origRecordingTier;
		private readonly ITimeOrderSegment[] _origRecordingSegments;
		private WaveFileWriter _audioFileWriter;

		/// ------------------------------------------------------------------------------------
		public static void Generate(TimeOrderTier originalRecodingTier)
		{
			var generator = new OralAnnotationFileGenerator(originalRecodingTier);
			generator.CreateInterleavedAudioFile();
		}

		/// ------------------------------------------------------------------------------------
		private OralAnnotationFileGenerator(TimeOrderTier originalRecodingTier)
		{
			_origRecordingTier = originalRecodingTier;
			_origRecordingSegments = _origRecordingTier.GetAllSegments().Cast<ITimeOrderSegment>().ToArray();
		}

		/// ------------------------------------------------------------------------------------
		private void CreateInterleavedAudioFile()
		{
			var outputFilename = _origRecordingTier.MediaFileName +
				Settings.Default.OralAnnotationGeneratedFileAffix;

			using (_audioFileWriter = new WaveFileWriter(outputFilename, WaveFileUtils.GetDefaultWaveFormat(3)))
			{
				for (int i = 0; i < _origRecordingSegments.Length; i++)
					InterleaveSegments(i);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void InterleaveSegments(int segmentIndex)
		{
			WaveStream inputStream;

			// Write a channel for the original recording segment
			using (inputStream = GetWaveStreamForOriginalSegment(segmentIndex))
				WriteAudioStreamToChannel(1, inputStream);

			// Write a channel for the careful speech segment
			inputStream = GetWaveStreamForOralAnnotationSegment(segmentIndex,
				OralAnnotationType.Careful);

			if (inputStream != null)
			{
				WriteAudioStreamToChannel(2, inputStream);
				inputStream.Dispose();
			}

			// Write a channel for the oral translation segment
			inputStream = GetWaveStreamForOralAnnotationSegment(segmentIndex,
				OralAnnotationType.Translation);

			if (inputStream != null)
			{
				WriteAudioStreamToChannel(3, inputStream);
				inputStream.Dispose();
			}
		}

		/// ------------------------------------------------------------------------------------
		private WaveStream GetWaveStreamForOriginalSegment(int segmentIndex)
		{
			var segment = _origRecordingSegments[segmentIndex];
			var origRecStream = new WaveFileReader(_origRecordingTier.MediaFileName);

			return new WaveSegmentStream(origRecStream,
					TimeSpan.FromSeconds(segment.Start), TimeSpan.FromSeconds(segment.GetLength()));
		}

		/// ------------------------------------------------------------------------------------
		private WaveStream GetWaveStreamForOralAnnotationSegment(int segmentIndex,
			OralAnnotationType annotationType)
		{
			var pathToAnnotationsFolder = _origRecordingTier.MediaFileName + "_Annotations";
			var segment = _origRecordingSegments[segmentIndex];

			var filename = Path.Combine(pathToAnnotationsFolder,
				string.Format(Settings.Default.OralAnnotationSegmentFileAffix,
					segment.Start, segment.Stop, annotationType));

			return (File.Exists(filename) ? new WaveFileReader(filename) : null);
		}

		/// ------------------------------------------------------------------------------------
		private void WriteAudioStreamToChannel(int channel, WaveStream inputStream)
		{
			int bytesPerBlock = (inputStream.WaveFormat.BitsPerSample / 8) * inputStream.WaveFormat.Channels;
			var oneBlock = new byte[bytesPerBlock];
			var silentBlock = new byte[bytesPerBlock];

			for (long bytesRead = 0; bytesRead < inputStream.Length; bytesRead += bytesPerBlock)
			{
				inputStream.Read(oneBlock, 0, bytesPerBlock);

				switch (channel)
				{
					case 1:
						_audioFileWriter.Write(oneBlock, 0, bytesPerBlock);
						_audioFileWriter.Write(silentBlock, 0, bytesPerBlock);
						_audioFileWriter.Write(silentBlock, 0, bytesPerBlock);
						break;

					case 2:
						_audioFileWriter.Write(silentBlock, 0, bytesPerBlock);
						_audioFileWriter.Write(oneBlock, 0, bytesPerBlock);
						_audioFileWriter.Write(silentBlock, 0, bytesPerBlock);
						break;

					case 3:
						_audioFileWriter.Write(silentBlock, 0, bytesPerBlock);
						_audioFileWriter.Write(silentBlock, 0, bytesPerBlock);
						_audioFileWriter.Write(oneBlock, 0, bytesPerBlock);
						break;
				}
			}
		}
	}
}
