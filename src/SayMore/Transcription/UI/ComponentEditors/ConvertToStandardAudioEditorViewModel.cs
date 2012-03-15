using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using SayMore.AudioUtils;
using SayMore.Model.Files;
using SayMore.UI;
using SayMore.UI.MediaPlayer;

namespace SayMore.Transcription.UI
{
	public class ConvertToStandardAudioEditorViewModel
	{
		/// ------------------------------------------------------------------------------------
		public string GetAudioFileEncoding(string mediaFilePath)
		{
			var encoding = WaveFileUtils.GetFileAudioFormat(mediaFilePath);
			return (encoding != WaveFormatEncoding.Unknown ?
				encoding.ToString().Replace("WAVE_FORMAT", "WAV").Replace('_', ' ').ToUpperInvariant() :
				MPlayerHelper.GetAudioEncoding(mediaFilePath));
		}

		/// ------------------------------------------------------------------------------------
		private int GetChannelsFromOriginalMediaFile(string mediaFilePath)
		{
			var mediaInfo = new MediaFileInfo(mediaFilePath);

			// There are some media files (e.g. MTS) for which ffmpeg -- which is what the
			// MediaFileInfo class uses -- returns 0 channels. Hence the check. When that
			// happens, then get the information using mplayer.
			return (mediaInfo.Channels > 0 ? mediaInfo.Channels :
				MPlayerHelper.GetAudioChannels(mediaFilePath));
		}

		/// ------------------------------------------------------------------------------------
		public Exception Convert(Control parent, ComponentFile file, string waitMessage)
		{
			var pathToStdAudioFile = file.GetSuggestedPathToStandardAudioFile();

			using (var dlg = new LoadingDlg(waitMessage))
			{
				dlg.Show(parent);
				Exception error = null;

				var worker = new BackgroundWorker();
				worker.DoWork += delegate
				{
					int channels = GetChannelsFromOriginalMediaFile(file.PathToAnnotatedFile);
					var format = WaveFileUtils.GetDefaultWaveFormat(channels);

					WaveFileUtils.GetPlainPcmStream(file.PathToAnnotatedFile,
						pathToStdAudioFile, format, out error);

					if (error != null && File.Exists(pathToStdAudioFile))
						File.Delete(pathToStdAudioFile);
				};

				worker.RunWorkerAsync();
				while (worker.IsBusy) { Application.DoEvents(); }
				dlg.Close();
				return error;
			}
		}
	}
}
