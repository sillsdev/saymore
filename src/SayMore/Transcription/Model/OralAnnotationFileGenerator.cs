using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using NAudio.Wave;
using Palaso.Reporting;
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
			if (!CanGenerate(originalRecodingTier))
				return null;

			var msg = LocalizationManager.GetString("EventsView.Transcription.GeneratedOralAnnotationView.GeneratingOralAnnotationFileMsg",
				"Generating Oral Annotation file...");

			using (var generator = new OralAnnotationFileGenerator(originalRecodingTier))
			using (var dlg = new LoadingDlg(msg))
			{
				if (parentControlForDialog != null)
					dlg.Show(parentControlForDialog);
				else
				{
					dlg.StartPosition = FormStartPosition.CenterScreen;
					dlg.Show();
				}

				var worker = new BackgroundWorker();
				worker.DoWork += generator.CreateInterleavedAudioFile;
				worker.RunWorkerAsync();

				while (worker.IsBusy)
					Application.DoEvents();

				dlg.Close();

				return generator._outputFileName;
			}
		}

		/// ------------------------------------------------------------------------------------
		private static bool CanGenerate(TimeOrderTier originalRecodingTier)
		{
			var pathToAnnotationsFolder = originalRecodingTier.MediaFileName +
				Settings.Default.OralAnnotationsFolderAffix;

			// First, look for the folder that stores the oral annotation segment files.
			if (!Directory.Exists(pathToAnnotationsFolder))
				return false;

			// Now look in that folder to see if any segment files actually exist.
			return (Directory.GetFiles(pathToAnnotationsFolder).Any(f =>
				f.ToLower().EndsWith(Settings.Default.OralAnnotationCarefulSegmentFileAffix.ToLower()) ||
				f.ToLower().EndsWith(Settings.Default.OralAnnotationTranslationSegmentFileAffix.ToLower())));
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
				var msg = LocalizationManager.GetString("EventsView.Transcription.GeneratedOralAnnotationView.ProcessingOriginalRecordingErrorMsg",
					"There was an error processing the original recording.");

				ErrorReport.NotifyUserOfProblem(msg, _origRecStreamProvider.Error);
				return;
			}

			_outputFileName = _origRecordingTier.MediaFileName +
				Settings.Default.OralAnnotationGeneratedFileAffix;

			var tmpOutputFile = Path.GetFileName(_outputFileName);
			tmpOutputFile = Path.Combine(Path.GetTempPath(), tmpOutputFile);

			try
			{
				using (_audioFileWriter = new WaveFileWriter(tmpOutputFile, _output3ChannelAudioFormat))
				{
					for (int i = 0; i < _origRecordingSegments.Length; i++)
						InterleaveSegments(i);
				}

				if (File.Exists(_outputFileName))
					File.Delete(_outputFileName);

				File.Move(tmpOutputFile, _outputFileName);
			}
			catch (Exception error)
			{
				var msg = LocalizationManager.GetString("EventsView.Transcription.GeneratedOralAnnotationView.GenericErrorCreatingOralAnnotationFileMsg",
					"There was an error generating the oral annotation file.");

				ErrorReport.NotifyUserOfProblem(error, msg);
			}
			finally
			{
				try { _audioFileWriter.Dispose(); }
				catch { }
				_audioFileWriter = null;

				if (File.Exists(tmpOutputFile))
					File.Delete(tmpOutputFile);
			}
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
			var pathToAnnotationsFolder = _origRecordingTier.MediaFileName + Settings.Default.OralAnnotationsFolderAffix;

			var affix = (annotationType == OralAnnotationType.Careful ?
				Settings.Default.OralAnnotationCarefulSegmentFileAffix :
				Settings.Default.OralAnnotationTranslationSegmentFileAffix);

			var filename = Path.Combine(pathToAnnotationsFolder,
				string.Format(Settings.Default.OralAnnotationSegmentFileFormat,
					segment.Start, segment.Stop, affix));

			var provider = WaveStreamProvider.Create(_output1ChannelAudioFormat, filename);
			if (provider.Error != null && !(provider.Error is FileNotFoundException))
			{
				var msg = LocalizationManager.GetString("EventsView.Transcription.GeneratedOralAnnotationView.ProcessingAnnotationFileErrorMsg",
					"There was an error processing a {0} annotation file.",
					"The parameter is the annotation type (i.e. careful, translation).");

				ErrorReport.NotifyUserOfProblem(_origRecStreamProvider.Error, msg,
					annotationType.ToString().ToLower());
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
