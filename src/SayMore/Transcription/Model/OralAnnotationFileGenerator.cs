using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Localization;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Palaso.Reporting;
using SayMore.Media.Audio;
using SayMore.Properties;
using SayMore.UI;
using SayMore.Utilities;

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
		private static string GetGeneratingMsg()
		{
			return LocalizationManager.GetString(
				"SessionsView.Transcription.GeneratedOralAnnotationView.GeneratingOralAnnotationFileMsg",
				"Generating Oral Annotation file...");
		}

		/// ------------------------------------------------------------------------------------
		public static bool Generate(TierCollection tierCollection, Control parentControlForDialog,
			bool justClearFileContents)
		{
			var timeTier = tierCollection.GetTimeTier();
			if (!CanGenerate(timeTier))
				return false;

			try
			{
				Program.SuspendBackgroundProcesses();

				if (justClearFileContents)
				{
					var oralAnnotationFile = timeTier.MediaFileName +
						Settings.Default.OralAnnotationGeneratedFileSuffix;
					File.Create(oralAnnotationFile);
					FileSystemUtils.WaitForFileRelease(oralAnnotationFile);
					return true;
				}

				using (var generator = new OralAnnotationFileGenerator(timeTier,
					tierCollection.GetIsSegmentIgnored, parentControlForDialog))
				using (var dlg = new LoadingDlg(GetGeneratingMsg()))
				{
					dlg.GenericErrorMessage = GetGenericErrorMsg();
					var worker = new BackgroundWorker();
					worker.DoWork += generator.CreateInterleavedAudioFile;
					worker.WorkerSupportsCancellation = true;
					dlg.BackgroundWorker = worker;
					dlg.Show(parentControlForDialog);
					return (dlg.DialogResult == DialogResult.OK);
				}
			}
			catch (Exception error)
			{
				ErrorReport.NotifyUserOfProblem(error, GetGenericErrorMsg());
				return false;
			}
			finally
			{
				Program.ResumeBackgroundProcesses(true);
			}
		}

		/// ------------------------------------------------------------------------------------
		private static bool CanGenerate(TimeTier sourceRecodingTier)
		{
			return Directory.Exists(sourceRecodingTier.MediaFileName +
				Settings.Default.OralAnnotationsFolderSuffix);
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
			BackgroundWorker worker = (BackgroundWorker)sender;
			var dlg = (LoadingDlg)e.Argument;

			if (_srcRecStreamProvider.Error != null)
			{
				var msg = LocalizationManager.GetString(
					"SessionsView.Transcription.GeneratedOralAnnotationView.ProcessingSourceRecordingErrorMsg",
					"There was an error processing the source recording.");
				throw new Exception(msg, _srcRecStreamProvider.Error);
			}

			_outputFileName = _srcRecordingTier.MediaFileName +
				Settings.Default.OralAnnotationGeneratedFileSuffix;

			var tmpOutputFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(_outputFileName));

			try
			{
				int retries = 0;
				while (e.Result == null && !worker.CancellationPending && !e.Cancel)
				{
					// REVIEW: Not sure why this should have to be inside the while-loop, but in at least one of my tests,
					// the temp file went AWOL.
					if (retries == 0 || !File.Exists(tmpOutputFile))
					{
						if (retries > 0)
							dlg.SetState(GetGeneratingMsg(), null);

						using (_audioFileWriter = new WaveFileWriter(tmpOutputFile, _outputAudioFormat))
						{
							foreach (var timeRange in _srcRecordingSegments)
							{
								if (worker.CancellationPending)
									return;
								InterleaveSegments(timeRange);
							}
						}
					}

					try
					{
						retries++;

						if (File.Exists(_outputFileName))
							File.Delete(_outputFileName);

						File.Move(tmpOutputFile, _outputFileName);
						e.Result = true;
					}
					catch (Exception failure)
					{
						var retriesMsg = string.Format(LocalizationManager.GetString(
								"CommonToMultipleViews.AttemptedRetriesMsg",
								"(Retries attempted: {0})",
								"Parameter is the number of times SayMore has attempted to move the temp file to the permanent location."), retries);

						string failureMsg;
						if (failure is UnauthorizedAccessException || failure is IOException)
						{
							failureMsg = failure.Message;
							if (!failureMsg.Contains(_outputFileName))
							{
								var detailsMsg = LocalizationManager.GetString(
									"SessionsView.Transcription.GeneratedOralAnnotationView.GeneratingOralAnnotationFileDetails",
									"Attempting to generate file:");
								failureMsg += Environment.NewLine + detailsMsg + Environment.NewLine + _outputFileName;
							}
						}
						else
							throw;
						dlg.SetState(failureMsg + Environment.NewLine + retriesMsg, failure);
						Thread.Sleep(1000);
					}
				}
			}
			finally
			{
				try { _audioFileWriter.Dispose(); }
				catch { }
				_audioFileWriter = null;

				if (File.Exists(tmpOutputFile))
					File.Delete(tmpOutputFile);

				if (worker.CancellationPending)
					e.Cancel = true;
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

				var type = annotationType.ToString().ToLower();
				if (_synchInvoke.InvokeRequired)
					_synchInvoke.Invoke((Action)(() => ErrorReport.NotifyUserOfProblem(_srcRecStreamProvider.Error, msg, type)), null);
				else
					ErrorReport.NotifyUserOfProblem(_srcRecStreamProvider.Error, msg, type);
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
