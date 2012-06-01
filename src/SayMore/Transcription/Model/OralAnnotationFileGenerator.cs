using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Palaso.Reporting;
using SayMore.Media.Audio;
using SayMore.Properties;
using SayMore.UI;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class generates an oral annotation file by interleaving into a single audio file
	/// having one channel for the careful speech, one for the oral translation and one or
	/// two for the source recording. Therefore, the result is an audio file containing
	/// 3 or 4 channels, depending on how many are in the source.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class OralAnnotationFileGenerator : IDisposable
	{
		private enum AnnotationChannel
		{
			Source,
			Careful,
			Translation
		}

		private readonly TimeTier _srcRecordingTier;
		private readonly Segment[] _srcRecordingSegments;
		private WaveFileWriter _audioFileWriter;
		private readonly WaveFormat _outputAudioFormat;
		private readonly WaveFormat _output1ChannelAudioFormat;
		private WaveStreamProvider _srcRecStreamProvider;
		private string _outputFileName;

		#region Static methods
		/// ------------------------------------------------------------------------------------
		private static string GetGenericErrorMsg()
		{
			return LocalizationManager.GetString(
				"SessionsView.Transcription.GeneratedOralAnnotationView.GeneratingOralAnnotationFileGenericErrorMsg",
				"There was an error generating the oral annotation file.");
		}

		/// ------------------------------------------------------------------------------------
		public static string Generate(TimeTier sourceRecodingTier, Control parentControlForDialog)
		{
			if (!CanGenerate(sourceRecodingTier))
				return null;

			try
			{
				Program.SuspendBackgroundProcesses();
				ExceptionHandler.AddDelegate(HandleOralAnnotationFileGeneratorException);

				var msg = LocalizationManager.GetString(
					"SessionsView.Transcription.GeneratedOralAnnotationView.GeneratingOralAnnotationFileMsg",
					"Generating Oral Annotation file...");

				using (var generator = new OralAnnotationFileGenerator(sourceRecodingTier))
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
		private static bool CanGenerate(TimeTier sourceRecodingTier)
		{
			return Directory.Exists(sourceRecodingTier.MediaFileName +
				Settings.Default.OralAnnotationsFolderSuffix);
		}

		/// ------------------------------------------------------------------------------------
		private static void HandleOralAnnotationFileGeneratorException(object sender,
			CancelExceptionHandlingEventArgs e)
		{
			ErrorReport.NotifyUserOfProblem(e.Exception, GetGenericErrorMsg());
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private OralAnnotationFileGenerator(TimeTier sourceRecodingTier)
		{
			_srcRecordingTier = sourceRecodingTier;
			_srcRecordingSegments = _srcRecordingTier.Segments.ToArray();

			_srcRecStreamProvider = WaveStreamProvider.Create(
				AudioUtils.GetDefaultWaveFormat(1), _srcRecordingTier.MediaFileName);

			var sourceFormat = _srcRecStreamProvider.Stream.WaveFormat;

			_outputAudioFormat = new WaveFormat(sourceFormat.SampleRate,
				sourceFormat.BitsPerSample, sourceFormat.Channels + 2);

			_output1ChannelAudioFormat = new WaveFormat(sourceFormat.SampleRate,
				sourceFormat.BitsPerSample, 1);
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_audioFileWriter != null)
				_audioFileWriter.Close();

			if (_srcRecStreamProvider != null)
				_srcRecStreamProvider.Dispose();

			_audioFileWriter = null;
			_srcRecStreamProvider = null;
		}

		/// ------------------------------------------------------------------------------------
		private void CreateInterleavedAudioFile(object sender, DoWorkEventArgs e)
		{
			if (_srcRecStreamProvider.Error != null)
			{
				var msg = LocalizationManager.GetString(
					"SessionsView.Transcription.GeneratedOralAnnotationView.ProcessingSourceRecordingErrorMsg",
					"There was an error processing the source recording.");

				ErrorReport.NotifyUserOfProblem(msg, _srcRecStreamProvider.Error);
				return;
			}

			_outputFileName = _srcRecordingTier.MediaFileName +
				Settings.Default.OralAnnotationGeneratedFileSuffix;

			var tmpOutputFile = Path.GetFileName(_outputFileName);
			tmpOutputFile = Path.Combine(Path.GetTempPath(), tmpOutputFile);

			try
			{
				using (_audioFileWriter = new WaveFileWriter(tmpOutputFile, _outputAudioFormat))
				{
					foreach (var segment in _srcRecordingSegments)
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
			// Write a channel for the source recording segment
			var inputStream = _srcRecStreamProvider.GetStreamSubset(segment);
			if (inputStream != null)
				WriteAudioStreamToChannel(AnnotationChannel.Source, inputStream);

			// Write a channel for the careful speech segment
			using (var provider = GetWaveStreamForOralAnnotationSegment(segment, AudioRecordingType.Careful))
			{
				if (provider.Stream != null)
					WriteAudioStreamToChannel(AnnotationChannel.Careful, provider.Stream);
			}

			// Write a channel for the oral translation segment
			using (var provider = GetWaveStreamForOralAnnotationSegment(segment, AudioRecordingType.Translation))
			{
				if (provider.Stream != null)
					WriteAudioStreamToChannel(AnnotationChannel.Translation, provider.Stream);
			}
		}

		/// ------------------------------------------------------------------------------------
		private WaveStreamProvider GetWaveStreamForOralAnnotationSegment(Segment segment,
			AudioRecordingType annotationType)
		{
			var pathToAnnotationsFolder = _srcRecordingTier.MediaFileName +
				Settings.Default.OralAnnotationsFolderSuffix;

			var filename = Path.Combine(pathToAnnotationsFolder, (annotationType == AudioRecordingType.Careful ?
				TimeTier.ComputeFileNameForCarefulSpeechSegment(segment) :
				TimeTier.ComputeFileNameForOralTranslationSegment(segment)));

			var provider = WaveStreamProvider.Create(_output1ChannelAudioFormat, filename);
			if (provider.Error != null && !(provider.Error is FileNotFoundException))
			{
				var msg = LocalizationManager.GetString(
					"SessionsView.Transcription.GeneratedOralAnnotationView.ProcessingAnnotationFileErrorMsg",
					"There was an error processing a {0} annotation file.",
					"The parameter is the annotation type (i.e. careful, translation).");

				ErrorReport.NotifyUserOfProblem(_srcRecStreamProvider.Error, msg,
					annotationType.ToString().ToLower());
			}

			return provider;
		}

		/// ------------------------------------------------------------------------------------
		private void WriteAudioStreamToChannel(AnnotationChannel channel, WaveStream inputStream)
		{
			var silentBlocksForOrig = new float[_srcRecStreamProvider.Stream.WaveFormat.Channels];
			var blocksRead = 0;
			var totalBlocks = inputStream.Length / inputStream.WaveFormat.BlockAlign;
			var provider = new SampleChannel(inputStream);
			var buffer = new float[provider.WaveFormat.Channels];

			while (provider.Read(buffer, 0, provider.WaveFormat.Channels) > 0 && blocksRead < totalBlocks)
			{
				blocksRead += 1;

				switch (channel)
				{
					case AnnotationChannel.Source:
						_audioFileWriter.WriteSamples(buffer, 0, _srcRecStreamProvider.Stream.WaveFormat.Channels);
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
