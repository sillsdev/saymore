using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Palaso.Reporting;
using SayMore.Media;
using SayMore.Properties;
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

		#region Static methods
		/// ------------------------------------------------------------------------------------
		private static string GetGenericErrorMsg()
		{
			return LocalizationManager.GetString(
				"EventsView.Transcription.GeneratedOralAnnotationView.GeneratingOralAnnotationFileGenericErrorMsg",
				"There was an error generating the oral annotation file.");

		}

		/// ------------------------------------------------------------------------------------
		public static string Generate(TimeTier originalRecodingTier, Control parentControlForDialog)
		{
			if (!CanGenerate(originalRecodingTier))
				return null;

			try
			{
				Program.SuspendBackgroundProcesses();
				ExceptionHandler.AddDelegate(HandleOralAnnotationFileGeneratorException);

				var msg = LocalizationManager.GetString(
					"EventsView.Transcription.GeneratedOralAnnotationView.GeneratingOralAnnotationFileMsg",
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
			catch (Exception error)
			{
				HandleOralAnnotationFileGeneratorException(null,
					new CancelExceptionHandlingEventArgs(error));
			}
			finally
			{
				Program.ResumeBackgroundProcesses(true);
				ExceptionHandler.RemoveDelegate(HandleOralAnnotationFileGeneratorException);
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		private static bool CanGenerate(TimeTier originalRecodingTier)
		{
			return Directory.Exists(originalRecodingTier.MediaFileName +
				Settings.Default.OralAnnotationsFolderSuffix);

			//var pathToAnnotationsFolder = originalRecodingTier.MediaFileName +
			//    Settings.Default.OralAnnotationsFolderSuffix;

			//// First, look for the folder that stores the oral annotation segment files.
			//if (!Directory.Exists(pathToAnnotationsFolder))
			//    return false;

			//// Now look in that folder to see if any segment files actually exist.
			//return (Directory.GetFiles(pathToAnnotationsFolder).Any(f =>
			//    f.ToLower().EndsWith(Settings.Default.OralAnnotationCarefulSegmentFileSuffix.ToLower()) ||
			//    f.ToLower().EndsWith(Settings.Default.OralAnnotationTranslationSegmentFileSuffix.ToLower())));
		}

		/// ------------------------------------------------------------------------------------
		private static void HandleOralAnnotationFileGeneratorException(object sender,
			CancelExceptionHandlingEventArgs e)
		{
			ErrorReport.NotifyUserOfProblem(e.Exception, GetGenericErrorMsg());
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private OralAnnotationFileGenerator(TimeTier originalRecodingTier)
		{
			_origRecordingTier = originalRecodingTier;
			_origRecordingSegments = _origRecordingTier.Segments.ToArray();

			_origRecStreamProvider = WaveStreamProvider.Create(
				AudioUtils.GetDefaultWaveFormat(1), _origRecordingTier.MediaFileName);

			var originalFormat = _origRecStreamProvider.Stream.WaveFormat;

			_outputAudioFormat = new WaveFormat(originalFormat.SampleRate,
				originalFormat.BitsPerSample, originalFormat.Channels + 2);

			_output1ChannelAudioFormat = new WaveFormat(originalFormat.SampleRate,
				originalFormat.BitsPerSample, 1);
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
				var msg = LocalizationManager.GetString(
					"EventsView.Transcription.GeneratedOralAnnotationView.ProcessingOriginalRecordingErrorMsg",
					"There was an error processing the original recording.");

				ErrorReport.NotifyUserOfProblem(msg, _origRecStreamProvider.Error);
				return;
			}

			_outputFileName = _origRecordingTier.MediaFileName +
				Settings.Default.OralAnnotationGeneratedFileSuffix;

			var tmpOutputFile = Path.GetFileName(_outputFileName);
			tmpOutputFile = Path.Combine(Path.GetTempPath(), tmpOutputFile);

			try
			{
				using (_audioFileWriter = new WaveFileWriter(tmpOutputFile, _outputAudioFormat))
				{
					foreach (var segment in _origRecordingSegments)
						InterleaveSegments(segment);
				}

				if (File.Exists(_outputFileName))
					File.Delete(_outputFileName);

				File.Move(tmpOutputFile, _outputFileName);
			}
			catch (Exception error)
			{
				ErrorReport.NotifyUserOfProblem(error, GetGenericErrorMsg());
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
		private void InterleaveSegments(Segment segment)
		{
			// Write a channel for the original recording segment
			var inputStream = _origRecStreamProvider.GetStreamSubset(segment);
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
			var pathToAnnotationsFolder = _origRecordingTier.MediaFileName +
				Settings.Default.OralAnnotationsFolderSuffix;

			var filename = Path.Combine(pathToAnnotationsFolder, (annotationType == OralAnnotationType.Careful ?
				TimeTier.ComputeFileNameForCarefulSpeechSegment(segment) :
				TimeTier.ComputeFileNameForOralTranslationSegment(segment)));

			var provider = WaveStreamProvider.Create(_output1ChannelAudioFormat, filename);
			if (provider.Error != null && !(provider.Error is FileNotFoundException))
			{
				var msg = LocalizationManager.GetString(
					"EventsView.Transcription.GeneratedOralAnnotationView.ProcessingAnnotationFileErrorMsg",
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
			var blocksRead = 0;
			var totalBlocks = inputStream.Length / inputStream.WaveFormat.BlockAlign;
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
