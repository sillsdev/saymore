using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using SayMore.AudioUtils;
using SayMore.Properties;
using SayMore.Transcription.UI;
using SayMore.UI;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class generates an oral annotation file by interleaving into a single, 3-channel
	/// audio file, original recording, careful speech and oral translation segments.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class OralAnnotationFileGenerator : IDisposable
	{
		private readonly TimeOrderTier _origRecordingTier;
		private readonly ITimeOrderSegment[] _origRecordingSegments;
		private WaveFileWriter _audioFileWriter;
		private readonly WaveFormat _output3ChannelAudioFormat;
		private readonly WaveFormat _output1ChannelAudioFormat;
		private WaveStreamProvider _origRecStreamProvider;
		private string _outputFileName;

		/// ------------------------------------------------------------------------------------
		public static string Generate(TimeOrderTier originalRecodingTier, Control parentControlForDialog)
		{
			using (var generator = new OralAnnotationFileGenerator(originalRecodingTier))
			{
				LoadingDlg dlg = null;

				if (parentControlForDialog != null)
				{
					dlg = new LoadingDlg("Generating Oral Annotation file...");
					dlg.Show(parentControlForDialog);
				}

				var worker = new BackgroundWorker();
				worker.DoWork += generator.CreateInterleavedAudioFile;
				worker.RunWorkerAsync();

				while (worker.IsBusy)
					Application.DoEvents();

				dlg.Close();
				dlg.Dispose();

				return generator._outputFileName;
			}
		}

		/// ------------------------------------------------------------------------------------
		private OralAnnotationFileGenerator(TimeOrderTier originalRecodingTier)
		{
			_origRecordingTier = originalRecodingTier;
			_origRecordingSegments = _origRecordingTier.GetAllSegments().Cast<ITimeOrderSegment>().ToArray();
			_output3ChannelAudioFormat = WaveFileUtils.GetDefaultWaveFormat(3);
			_output1ChannelAudioFormat = WaveFileUtils.GetDefaultWaveFormat(1);

			_origRecStreamProvider =
				WaveStreamProvider.Create(_output1ChannelAudioFormat, _origRecordingTier.MediaFileName);
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_audioFileWriter != null)
				_audioFileWriter.Close();

			if (_origRecStreamProvider != null)
				_origRecStreamProvider.Dispose();

			_audioFileWriter = null;
			_origRecStreamProvider = null;
		}

		/// ------------------------------------------------------------------------------------
		private void CreateInterleavedAudioFile(object sender, DoWorkEventArgs e)
		{
			if (_origRecStreamProvider.Error != null)
			{
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(
					"There was an error processing the original recording.", _origRecStreamProvider.Error);

				return;
			}

			_outputFileName = _origRecordingTier.MediaFileName +
				Settings.Default.OralAnnotationGeneratedFileAffix;

			using (_audioFileWriter = new WaveFileWriter(_outputFileName, _output3ChannelAudioFormat))
			{
				for (int i = 0; i < _origRecordingSegments.Length; i++)
					InterleaveSegments(i);
			}

			_audioFileWriter = null;
		}

		/// ------------------------------------------------------------------------------------
		private void InterleaveSegments(int segmentIndex)
		{
			var segment = _origRecordingSegments[segmentIndex];

			// Write a channel for the original recording segment
			var inputStream = _origRecStreamProvider.GetStreamSubset(segment.Start, segment.GetLength());
			if (inputStream != null)
				WriteAudioStreamToChannel(1, inputStream);

			// Write a channel for the careful speech segment
			using (var provider = GetWaveStreamForOralAnnotationSegment(segment, OralAnnotationType.Careful))
			{
				if (provider.Stream != null)
					WriteAudioStreamToChannel(2, provider.Stream);
			}

			// Write a channel for the oral translation segment
			using (var provider = GetWaveStreamForOralAnnotationSegment(segment, OralAnnotationType.Translation))
			{
				if (provider.Stream != null)
					WriteAudioStreamToChannel(3, provider.Stream);
			}
		}

		/// ------------------------------------------------------------------------------------
		private WaveStreamProvider GetWaveStreamForOralAnnotationSegment(ITimeOrderSegment segment,
			OralAnnotationType annotationType)
		{
			var pathToAnnotationsFolder = _origRecordingTier.MediaFileName + "_Annotations";

			var filename = Path.Combine(pathToAnnotationsFolder,
				string.Format(Settings.Default.OralAnnotationSegmentFileAffix,
					segment.Start, segment.Stop, annotationType));

			var provider = WaveStreamProvider.Create(_output1ChannelAudioFormat, filename);
			if (provider.Error != null && !(provider.Error is FileNotFoundException))
			{
				var msg = "There was an error processing a {0} annotation file.";
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(
					string.Format(msg, annotationType.ToString().ToLower()),
					_origRecStreamProvider.Error);
			}

			return provider;
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
