using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Palaso.Reporting;
using SayMore.AudioUtils;
using SayMore.Properties;
using SayMore.Transcription.UI;
using SayMore.UI;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class generates an oral annotation file by interleaving into a single audio file
	/// having one channel for the careful speech, one for the oral translation and one or
	/// two for the original recording. Therefore, the result is an audio file containing
	/// 3 or 4 channels, depending on how many are in the original.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class OralAnnotationFileGenerator : IDisposable
	{
		private enum AnnotationChannel
		{
			Original,
			Careful,
			Translation
		}

		private readonly TimeTier _origRecordingTier;
		private readonly Segment[] _origRecordingSegments;
		private WaveFileWriter _audioFileWriter;
		private readonly WaveFormat _outputAudioFormat;
		private readonly WaveFormat _output1ChannelAudioFormat;
		private WaveStreamProvider _origRecStreamProvider;
		private string _outputFileName;

		/// ------------------------------------------------------------------------------------
		public static string Generate(TimeTier originalRecodingTier, Control parentControlForDialog)
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
		private static bool CanGenerate(TimeTier originalRecodingTier)
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
		private OralAnnotationFileGenerator(TimeTier originalRecodingTier)
		{
			_origRecordingTier = originalRecodingTier;
			_origRecordingSegments = _origRecordingTier.Segments.ToArray();
			_outputAudioFormat = WaveFileUtils.GetDefaultWaveFormat(3);
			_output1ChannelAudioFormat = WaveFileUtils.GetDefaultWaveFormat(1);

			_origRecStreamProvider =
				WaveStreamProvider.Create(_output1ChannelAudioFormat, _origRecordingTier.MediaFileName);

			if (_origRecStreamProvider.Stream.WaveFormat.Channels >= 2)
			{
				_outputAudioFormat = new WaveFormat(_outputAudioFormat.SampleRate,
					_outputAudioFormat.BitsPerSample, _origRecStreamProvider.Stream.WaveFormat.Channels + 2);
			}
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
				using (_audioFileWriter = new WaveFileWriter(tmpOutputFile, _outputAudioFormat))
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
				WriteAudioStreamToChannel(AnnotationChannel.Original, inputStream);

			// Write a channel for the careful speech segment
			using (var provider = GetWaveStreamForOralAnnotationSegment(segment, OralAnnotationType.Careful))
			{
				if (provider.Stream != null)
					WriteAudioStreamToChannel(AnnotationChannel.Careful, provider.Stream);
			}

			// Write a channel for the oral translation segment
			using (var provider = GetWaveStreamForOralAnnotationSegment(segment, OralAnnotationType.Translation))
			{
				if (provider.Stream != null)
					WriteAudioStreamToChannel(AnnotationChannel.Translation, provider.Stream);
			}
		}

		/// ------------------------------------------------------------------------------------
		private WaveStreamProvider GetWaveStreamForOralAnnotationSegment(Segment segment,
			OralAnnotationType annotationType)
		{
			var pathToAnnotationsFolder = _origRecordingTier.MediaFileName + Settings.Default.OralAnnotationsFolderAffix;

			var filename = Path.Combine(pathToAnnotationsFolder, (annotationType == OralAnnotationType.Careful ?
				TimeTier.ComputeFileNameForCarefulSpeechSegment(segment) :
				TimeTier.ComputeFileNameForOralTranslationSegment(segment)));

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
		private void WriteAudioStreamToChannel(AnnotationChannel channel, WaveStream inputStream)
		{
			var silentBlocksForOrig = new float[_origRecStreamProvider.Stream.WaveFormat.Channels];
			var totalBlocks = inputStream.Length / inputStream.WaveFormat.BlockAlign;
			var blocksRead = 0;
			var provider = new SampleChannel(inputStream);
			var buffer = new float[provider.WaveFormat.Channels];

			while (provider.Read(buffer, 0, provider.WaveFormat.Channels) > 0 && blocksRead < totalBlocks)
			{
				blocksRead += 1;

				switch (channel)
				{
					case AnnotationChannel.Original:
						_audioFileWriter.WriteSamples(buffer, 0, _origRecStreamProvider.Stream.WaveFormat.Channels);
						_audioFileWriter.WriteSample(0f);
						_audioFileWriter.WriteSample(0f);
						break;

					case AnnotationChannel.Careful:
						_audioFileWriter.WriteSamples(silentBlocksForOrig, 0, silentBlocksForOrig.Length);
						_audioFileWriter.WriteSample(buffer[0]);
						_audioFileWriter.WriteSample(0f);
						break;

					case AnnotationChannel.Translation:
						_audioFileWriter.WriteSamples(silentBlocksForOrig, 0, silentBlocksForOrig.Length);
						_audioFileWriter.WriteSample(0f);
						_audioFileWriter.WriteSample(buffer[0]);
						break;
				}
			}
		}
	}
}
