using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
		private readonly ISynchronizeInvoke _synchInvoke;
		private readonly List<TimeRange> _srcRecordingSegments;
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
		public static string Generate(TierCollection tierCollection, Control parentControlForDialog)
		{
			var timeTier = tierCollection.GetTimeTier();
			if (!CanGenerate(timeTier))
				return null;

			try
			{
				Program.SuspendBackgroundProcesses();
				ExceptionHandler.AddDelegate(HandleOralAnnotationFileGeneratorException);

				var msg = LocalizationManager.GetString(
					"SessionsView.Transcription.GeneratedOralAnnotationView.GeneratingOralAnnotationFileMsg",
					"Generating Oral Annotation file...");

				using (var generator = new OralAnnotationFileGenerator(timeTier,
					tierCollection.GetIsSegmentIgnored, parentControlForDialog))
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
		private OralAnnotationFileGenerator(TimeTier sourceTier, Func<int, bool> ignoreSegment,
			ISynchronizeInvoke synchInvoke)
		{
			_srcRecordingTier = sourceTier;
			_synchInvoke = synchInvoke;

			bool fullySegmented = sourceTier.IsFullySegmented;
			_srcRecordingSegments = new List<TimeRange>();
			for (int i = 0; i < sourceTier.Segments.Count; i++)
			{
				// Per JohnH's request via e-mail (8-12-2012), exclude ignored segments
				if (!ignoreSegment(i))
					_srcRecordingSegments.Add(sourceTier.Segments[i].TimeRange);
			}
			if (!fullySegmented)
				_srcRecordingSegments.Add(new TimeRange(sourceTier.EndOfLastSegment, sourceTier.TotalTime));

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
			Action<Action> Invoke = actionToInvoke => {
				if (_synchInvoke.InvokeRequired)
					_synchInvoke.Invoke(actionToInvoke, null);
				else
					actionToInvoke();
			};

			if (_srcRecStreamProvider.Error != null)
			{
				var msg = LocalizationManager.GetString(
					"SessionsView.Transcription.GeneratedOralAnnotationView.ProcessingSourceRecordingErrorMsg",
					"There was an error processing the source recording.");


				Invoke(() => ErrorReport.NotifyUserOfProblem(msg, _srcRecStreamProvider.Error));
				return;
			}

			_outputFileName = _srcRecordingTier.MediaFileName +
				Settings.Default.OralAnnotationGeneratedFileSuffix;

			var tmpOutputFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(_outputFileName));

			try
			{
				int retry = 0;
				do
				{
					// REVIEW: Not sure why this should have to be inside the do-loop, but in at least one of my tests,
					// the temp file went AWOL.
					if (retry == 0 || !File.Exists(tmpOutputFile))
					{

						using (_audioFileWriter = new WaveFileWriter(tmpOutputFile, _outputAudioFormat))
						{
							foreach (var timeRange in _srcRecordingSegments)
								InterleaveSegments(timeRange);
						}
					}

					try
					{
						if (File.Exists(_outputFileName))
							File.Delete(_outputFileName);

						File.Move(tmpOutputFile, _outputFileName);

						retry = 0;
					}
					catch (Exception failure)
					{
						string failureMsg;
						if (failure is UnauthorizedAccessException)
						{
							var retryMsg = LocalizationManager.GetString(
								"CommonToMultipleViews.RetryAfterUnauthorizedAccessExceptionMsg",
								"If you can determine which program is using this file, close it and click Retry.");
							failureMsg = failure.Message + Environment.NewLine + retryMsg;
						}
						else if (failure is IOException)
						{
							var detailsMsg = LocalizationManager.GetString(
								"SessionsView.Transcription.GeneratedOralAnnotationView.GeneratingOralAnnotationFileDetails",
								"Attempting to generate file:");
							failureMsg = failure.Message + Environment.NewLine + detailsMsg + Environment.NewLine + _outputFileName;
						}
						else
							throw;
						if (retry++ > 0)
						{
							Invoke(() =>
							{
								var userMessage = GetGenericErrorMsg() + Environment.NewLine + failureMsg;
								if (MessageBox.Show(userMessage,
									Application.ProductName, MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
								{
									UsageReporter.ReportException(false,
										"Cancelled by user after 1 automatic retry and " + (retry - 1) + "retries requested by the user",
										failure, userMessage);
									retry = 0;
								}
							});
						}
					}
				} while (retry > 0);
			}
			catch (Exception error)
			{
				Invoke(() => ErrorReport.NotifyUserOfProblem(error, GetGenericErrorMsg()));
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
		private void InterleaveSegments(TimeRange segmentRange)
		{
			// Write a channel for the source recording segment
			var inputStream = _srcRecStreamProvider.GetStreamSubset(segmentRange);
			if (inputStream != null)
				WriteAudioStreamToChannel(AnnotationChannel.Source, inputStream);

			var pathToAnnotationsFolder = _srcRecordingTier.MediaFileName +
				Settings.Default.OralAnnotationsFolderSuffix;

			// Write a channel for the careful speech segment
			var filename = Path.Combine(pathToAnnotationsFolder, TimeTier.ComputeFileNameForCarefulSpeechSegment(segmentRange));
			using (var provider = GetWaveStreamForOralAnnotationSegment(filename, AudioRecordingType.Careful))
			{
				if (provider.Stream != null)
					WriteAudioStreamToChannel(AnnotationChannel.Careful, provider.Stream);
			}

			// Write a channel for the oral translation segment
			filename = Path.Combine(pathToAnnotationsFolder, TimeTier.ComputeFileNameForOralTranslationSegment(segmentRange));
			using (var provider = GetWaveStreamForOralAnnotationSegment(filename, AudioRecordingType.Translation))
			{
				if (provider.Stream != null)
					WriteAudioStreamToChannel(AnnotationChannel.Translation, provider.Stream);
			}
		}

		/// ------------------------------------------------------------------------------------
		private WaveStreamProvider GetWaveStreamForOralAnnotationSegment(string filename,
			AudioRecordingType annotationType)
		{
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
