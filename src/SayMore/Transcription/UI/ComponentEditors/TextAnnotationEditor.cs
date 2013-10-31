using System;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using L10NSharp;
using Palaso.Reporting;
using Palaso.UI.WindowsForms.Extensions;
using SayMore.Media.Audio;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Transcription.Model.Exporters;
using SayMore.UI.ComponentEditors;
using SayMore.Media.MPlayer;
using SayMore.Model;
using Palaso.UI.WindowsForms;
using SayMore.Media;
using SayMore.Media.FFmpeg;
using System.Collections.Generic;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class TextAnnotationEditor : EditorBase
	{
		public delegate TextAnnotationEditor Factory(ComponentFile file, string imageKey);

		private readonly TextAnnotationEditorGrid _grid;
		private readonly VideoPanel _videoPanel;
		private FileSystemWatcher _watcher;
		private bool _isFirstTimeActivated = true;
		private Project _project;

		/// ------------------------------------------------------------------------------------
		public TextAnnotationEditor(ComponentFile file, string imageKey, Project project)
			: base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "Annotations";
			_toolStrip.Renderer = new NoToolStripBorderRenderer();

			_comboPlaybackSpeed.Font = Program.DialogFont;

			_project = project;
			_grid = new TextAnnotationEditorGrid(project.TranscriptionFont, project.FreeTranslationFont);
			_grid.TranscriptionFontChanged += font =>
			{
				_project.TranscriptionFont = font;
				_project.Save();
			};
			_grid.Dock = DockStyle.Fill;
			_splitter.Panel2.Controls.Add(_grid);

			LoadPlaybackSpeedCombo();
			_comboPlaybackSpeed.SelectedIndexChanged += HandlePlaybackSpeedValueChanged;
			SetSpeedPercentage(Settings.Default.AnnotationEditorPlaybackSpeedIndex);

			_videoPanel = new VideoPanel();
			_videoPanel.BackColor = Color.Black;
			_videoPanel.SetPlayerViewModel(_grid.PlayerViewModel);
			_splitter.Panel1.Controls.Add(_videoPanel);

			SetComponentFile(file);
			_splitter.Panel1.ClientSizeChanged += HandleSplitterPanel1ClientSizeChanged;

			_buttonHelp.Click += delegate
			{
				Program.ShowHelpTopic("/Using_Tools/Sessions_tab/Annotations_tab/Working_with_annotations.htm");
			};
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();

				if (_watcher != null)
				{
					_watcher.Changed -= HandleAnnotationFileChanged;
					_watcher.Dispose();
					_watcher = null;
				}
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadPlaybackSpeedCombo()
		{
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.100Pct", "100%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.90Pct", "90%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.80Pct", "80%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.70Pct", "70%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.60Pct", "60%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.50Pct", "50%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.40Pct", "40%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.30Pct", "30%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.20Pct", "20%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.10Pct", "10%"));
		}

		/// ------------------------------------------------------------------------------------
		private ComponentFile AssociatedComponentFile
		{
			get
			{
				return _file == null ? null :
					((AnnotationComponentFile)_file).AssociatedComponentFile;
			}
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			Deactivated();

			this.SetWindowRedraw(false);
			base.SetComponentFile(file);

			var annotationFile = file as AnnotationComponentFile;
			_splitter.Panel1Collapsed = annotationFile.GetIsAnnotatingAudioFile();

			var exception = annotationFile.TryLoadAndReturnException();
			if (exception != null)
			{
				var msg = LocalizationManager.GetString("SessionsView.Transcription.TextAnnotationEditor.LoadingAnnotationFileErrorMsg",
					"There was an error loading the annotation file '{0}'.");

				ErrorReport.NotifyUserOfProblem(exception, msg, file.PathToAnnotatedFile);
			}

			_grid.Load(annotationFile);
			_grid.SetColumnFonts(_project.TranscriptionFont, _project.FreeTranslationFont);

			_exportMenu.Enabled = (_grid.RowCount > 0);

			SetupWatchingForFileChanges();
			this.SetWindowRedraw(true);
			_videoPanel.ShowVideoThumbnailNow();
		}

		/// ------------------------------------------------------------------------------------
		public override void Activated()
		{
			base.Activated();

			if (!_isFirstTimeActivated)
				return;

			_isFirstTimeActivated = false;

			_grid.FirstTimeColumnInitialization();

			if (Settings.Default.AnnotationEditorSpiltterPos > 0)
				_splitter.SplitterDistance = Settings.Default.AnnotationEditorSpiltterPos;

			_splitter.SplitterMoved += delegate
			{
				Settings.Default.AnnotationEditorSpiltterPos = _splitter.SplitterDistance;
			};
		}

		/// ------------------------------------------------------------------------------------
		public override void Deactivated()
		{
			if (_grid != null)
				_grid.Stop();

			if (_file != null)
			{
				_file.BeforeSave -= HandleBeforeAnnotationFileSaved;
				_file.AfterSave -= HandleAfterAnnotationFileSaved;
			}

			if (_watcher != null)
			{
				_watcher.Changed -= HandleAnnotationFileChanged;
				_watcher.Dispose();
				_watcher = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSplitterPanel1ClientSizeChanged(object sender, EventArgs e)
		{
			this.SetWindowRedraw(false);

			_videoPanel.Size = new Size(_splitter.Panel1.ClientSize.Width,
				(int)(_splitter.Panel1.ClientSize.Width * Settings.Default.AnnotationEditorVideoWindowYtoXRatio));

			if (_videoPanel.Width > _splitter.Panel1.ClientSize.Width)
				_videoPanel.Width = _splitter.Panel1.ClientSize.Width;

			if (_videoPanel.Height > _splitter.Panel1.ClientSize.Height)
				_videoPanel.Height = _splitter.Panel1.ClientSize.Height;

			if (!_grid.PlayerViewModel.HasPlaybackStarted)
				_videoPanel.ShowVideoThumbnailNow();

			this.SetWindowRedraw(true);
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackSpeedValueChanged(object sender, EventArgs e)
		{
			Settings.Default.AnnotationEditorPlaybackSpeedIndex = _comboPlaybackSpeed.SelectedIndex;
			_grid.SetPlaybackSpeed(Math.Abs(_comboPlaybackSpeed.SelectedIndex - 10) * 10);
		}

		/// ------------------------------------------------------------------------------------
		private void SetSpeedPercentage(int index)
		{
			_comboPlaybackSpeed.SelectedIndex = (index >= 10 ? 0 : index);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormLostFocus()
		{
			base.OnFormLostFocus();
			OnEditorAndChildrenLostFocus();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEditorAndChildrenLostFocus()
		{
			base.OnEditorAndChildrenLostFocus();
			bool fInvalidate = _grid.PlaybackInProgress || _grid.IsCurrentCellInEditMode;
			_grid.Stop();
			_grid.EndEdit();
			if (fInvalidate)
				_grid.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void OnFLexTextExportClick(object sender, EventArgs e)
		{
			var file = (AnnotationComponentFile)_file;
			var mediaFileName = Path.GetFileName(file.GetPathToAssociatedMediaFile());

			using (var dlg = new ExportToFieldWorksInterlinearDlg(mediaFileName))
			{
				if (dlg.ShowDialog() == DialogResult.Cancel)
					return;

				FLExTextExporter.Save(dlg.FileName, mediaFileName,
					file.Tiers, dlg.TranscriptionWs.Id, dlg.FreeTranslationWs.Id);
			}
		}

		#region Methods for tracking changes to the EAF file outside of SayMore
		/// ------------------------------------------------------------------------------------
		void SetupWatchingForFileChanges()
		{
			_watcher = new FileSystemWatcher(
				Path.GetDirectoryName(_file.PathToAnnotatedFile),
				Path.GetFileName(_file.PathToAnnotatedFile));

			_watcher.IncludeSubdirectories = false;
			_watcher.EnableRaisingEvents = true;
			_watcher.Changed += HandleAnnotationFileChanged;

			_file.BeforeSave += HandleBeforeAnnotationFileSaved;
			_file.AfterSave += HandleAfterAnnotationFileSaved;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBeforeAnnotationFileSaved(object sender, EventArgs e)
		{
			if (_watcher != null)
				_watcher.EnableRaisingEvents = false;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleAfterAnnotationFileSaved(object sender, EventArgs e)
		{
			if (_watcher != null)
				_watcher.EnableRaisingEvents = true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleAnnotationFileChanged(object sender, FileSystemEventArgs e)
		{
			Invoke(new EventHandler((s, args) =>
			{
				_file.Load();
				_grid.Load(_file as AnnotationComponentFile);
				_grid.SetColumnFonts(_project.TranscriptionFont, _project.FreeTranslationFont);
			}));
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void ShowSegmentationDialog(Action showDialog)
		{
			_grid.PreventPlayback = true;
			try
			{
				_grid.EndEdit();
				HandleBeforeAnnotationFileSaved(null, null);
				try
				{
					((AnnotationComponentFile)_file).Tiers.Save(AssociatedComponentFile.PathToAnnotatedFile);
					showDialog();
				}
				finally
				{
					HandleAfterAnnotationFileSaved(null, null);
				}
			}
			finally
			{
				_grid.PreventPlayback = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordedAnnotationButtonClick(object sender, EventArgs e)
		{
			if (!AudioUtils.GetCanRecordAudio())
				return;

			ShowSegmentationDialog(delegate
			{
				var annotationType = (sender == _buttonCarefulSpeech
										? AudioRecordingType.Careful
										: AudioRecordingType.Translation);

				if (AssociatedComponentFile.RecordAnnotations(FindForm(), annotationType) != null &&
					ComponentFileListRefreshAction != null)
				{
					ComponentFileListRefreshAction(_file.PathToAnnotatedFile, null);
				}
			});
		}

		/// ------------------------------------------------------------------------------------
		private void HandleResegmentButtonClick(object sender, EventArgs e)
		{
			var originallySelectedCell = _grid.CurrentCellAddress;
			bool startPlayBackWhenFinished = false;
			ShowSegmentationDialog(delegate
				{
					if (ManualSegmenterDlg.ShowDialog(AssociatedComponentFile, this, _grid.CurrentCellAddress.Y) != null)
					{
						if (!_grid.IsDisposed)
						{
							if (originallySelectedCell != _grid.CurrentCellAddress)
							{
								// User has already selected a different cell, so go ahead and
								// start playback after refreshing the grid.
								originallySelectedCell = _grid.CurrentCellAddress;
								startPlayBackWhenFinished = true;
							}
							SetComponentFile(_file);
							if (_grid.RowCount > originallySelectedCell.Y && originallySelectedCell.Y >= 0 &&
								_grid.ColumnCount > originallySelectedCell.X && originallySelectedCell.X >= 0)
							{
								_grid.CurrentCell = _grid.Rows[originallySelectedCell.Y].Cells[originallySelectedCell.X];
							}
							else
								startPlayBackWhenFinished = false; // Oops, that cell is gone!
						}
					}
				});
			if (startPlayBackWhenFinished)
				_grid.Play();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.TabText", "Annotations");

			base.HandleStringsLocalized();
		}

		private void OnExportElanMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Actually, SayMore already stores this information in ELAN format (.eaf). Simply double click the annotations file to edit it in ELAN.");
		}

		private void OnExportSubtitlesFreeTranslation(object sender, EventArgs e)
		{
			var timeTier = (((AnnotationComponentFile)_file).Tiers.GetTimeTier());
			var contentTier = ((AnnotationComponentFile) _file).Tiers.GetFreeTranslationTier();
			DoExportSubtitleDialog(LocalizationManager.GetString("SessionsView.Transcription.TextAnnotation.ExportMenu.srtSubtitlesFreeTranslationExport.freeTranslationFilenameSuffix", "freeTranslation_subtitle"), timeTier, contentTier);
		}

		private void OnExportSubtitlesVernacular(object sender, EventArgs e)
		{
			var timeTier = (((AnnotationComponentFile)_file).Tiers.GetTimeTier());
			var contentTier = ((AnnotationComponentFile)_file).Tiers.GetTranscriptionTier();
			DoExportSubtitleDialog(LocalizationManager.GetString("SessionsView.Transcription.TextAnnotation.ExportMenu.srtSubtitlesTranscriptionExport.transcriptionFilenameSuffix", "transcription_subtitle"), timeTier, contentTier);
		}

		private void OnExportVideoFreeTranslation(object sender, EventArgs e)
		{
			// The string has to be worked out
			string fileNameSuffix = "freeTranslation";
			DoExportVideoDialog(LocalizationManager.GetString("SessionsView.Transcription.TextAnnotation.ExportMenu.srtSubtitlesFreeTranslationExport.freeTranslationFilenameSuffix", fileNameSuffix));
		}

		private void OnExportVideoTranscription(object sender, EventArgs e)
		{
			// The string has to be worked out
			string fileNameSuffix = "transcription";
			DoExportVideoDialog(LocalizationManager.GetString("SessionsView.Transcription.TextAnnotation.ExportMenu.srtSubtitlesTranscriptionExport.transcriptionFilenameSuffix", fileNameSuffix));
		}

		private void DoExportSubtitleDialog(string fileNameSuffix, TimeTier timeTier, TextTier textTeir)
		{
			textTeir.AddTimeRangeData(timeTier);

			var filter =  "SRT Subtitle File (*.srt)|*.srt";
			var fileName =_file.ParentElement.Id + ComponentRole.kFileSuffixSeparator + fileNameSuffix + ".srt";
			var action = new Action<string>(path => SRTFormatSubTitleExporter.Export(path, textTeir));

			DoSimpleExportDialog(".srt", filter, fileName, action);
		}

		private void DoExportVideoDialog(string fileNameSuffix)
		{
			// check whether FFmpeg is installed, if not, give a chance to install
			// this routine can be potentially moved to an earlier spot
			if (!FFmpegDownloadHelper.DoesFFmpegForSayMoreExist)
			{
				// TODO: Display an explanatory MessageBox
				MessageBox.Show("FFMPEG is required to export video with subtitle. The software can be downloaded now.");
				using (var dlg = new FFmpegDownloadDlg())
					dlg.ShowDialog(this);
				if (!FFmpegDownloadHelper.DoesFFmpegForSayMoreExist)
					return;
			}

			// ffmpeg requires FONTCONFIG_PATH environment variable set.
			// this routine can be potentially moved to an earlier spot
			var fontConfigPath = Palaso.IO.FileLocator.GetDirectoryDistributedWithApplication("mplayer", "fonts");
			System.Environment.SetEnvironmentVariable("FONTCONFIG_PATH ", fontConfigPath);

			/*
			Enumerate the video files in the session to try to
			determine which one to add subtitles to. The assumption is that if there is exactly one
			video in the session whose name matches the “base name” of the standard audio WAV file
			being annotated, then we can just use it without prompting the user.
			However, there are a couple edge cases I’m not sure about:

			1) If there are two or more videos with the same name but having different
			video formats (AVI, MP4, etc.), I’m assuming we need to ask the user which one to use?
			Or is there one particular format that we can safely assume to be the preferred one?
			(Presumably all the different videos would be the same content, so the end result will be essentially the same no matter which one we use, but the quality could be different.)

			2) If there are no videos at all in the session, do we disable these menu commands, detect and
			report that situation after they choose the command, or allow them to navigate to a video
			anywhere on their computer to marry with the subtitles?

			3) If there is exactly one video in the session, but it has a different name, do we use it without
			prompting, or ask the user? (If the latter, do we give them the option of navigating to a
			video somewhere else?)

			4) If there are multiple videos in the session, but none has a matching name, do we display them
			as a list and let the user choose? (Do we give them the option of navigating to a
			video somewhere else?)

			// Enumerate the video files in the session to try to determine which one to add subtitles to.
			List<string> videoFiles = new List<string>();
			foreach (var file in _file.ParentElement.GetComponentFiles())
			{
				var info = MediaFileInfo.GetInfo(file.PathToAnnotatedFile);
				if (info != null && info.IsVideo)
				{
					videoFiles.Add(file.PathToAnnotatedFile);
					//System.Console.Write(file.PathToAnnotatedFile);
				}
			}
			// The assumption is that there is exactly one video file
			if (videoFiles.Count != 1)
			{
				//TODO message
				System.Console.Write("There are either no video or more than one video file");
				return;
			}
			// compare against Settings.Default.StandardAudioFileSuffix;
			if (!videoFiles[0].StartsWith(baseName))
				return;
			// the input video file to which the subtitle is added
			string inVideoPath = videoFiles[0];
			*/

			// the input video file to which the subtitle is added
			int index = _file.PathToAnnotatedFile.IndexOf(Settings.Default.StandardAudioFileSuffix);
			string inVideoPath = _file.PathToAnnotatedFile.Substring(0, index) + "_Source.mp4";
			if (!File.Exists(inVideoPath))
				return;

			// produce a subtitle file to be merged into a video
			// this file will be created in a temp folder and later will be deleted
			var timeTier = (((AnnotationComponentFile)_file).Tiers.GetTimeTier());
			var contentTier = ((AnnotationComponentFile)_file).Tiers.GetFreeTranslationTier();
			contentTier.AddTimeRangeData(timeTier);
			var tempDir = Path.GetTempPath();
			var subtitleFilePath = tempDir + "\\" + _file.ParentElement.Id + "_tmp.srt";
			SRTFormatSubTitleExporter.Export(subtitleFilePath, contentTier);
			// in the future, consider ass format for better quality subtitle

			// the output video file
			var outVideoPath = _file.ParentElement.Id + ComponentRole.kFileSuffixSeparator + fileNameSuffix;
			var filter = "Video (*.mp4;*.mkv)|*.mp4;*mkv";

			var action = new Action<string>(path => TestFormatVideoExporter.Export(path, inVideoPath, subtitleFilePath));
			string extension = Path.GetExtension(inVideoPath);
			DoSimpleVideoExportDialog(extension, filter, outVideoPath, action);

			// delete the temporary subtitle file
			File.Delete(subtitleFilePath);
		}

		private void OnAudacityExportFreeTranslation(object sender, EventArgs e)
		{
			var timeTier = (((AnnotationComponentFile)_file).Tiers.GetTimeTier());
			var textTeir = ((AnnotationComponentFile)_file).Tiers.GetFreeTranslationTier();
			textTeir.AddTimeRangeData(timeTier);
			var filter = "Text File (*.txt)|*.txt";
			var suffix = LocalizationManager.GetString("SessionsView.Transcription.TextAnnotation.ExportMenu.AudacityFreeTranslationFilenameSuffix", "audacity_freeTranslation");
			var fileName = _file.ParentElement.Id + "_" + suffix + ".txt";
			var action = new Action<string>(path => AudacityExporter.Export(path, textTeir));

			DoSimpleExportDialog(".txt", filter, fileName, action);
		}

		private void OnAudacityExportTranscription(object sender, EventArgs e)
		{
			var timeTier = (((AnnotationComponentFile)_file).Tiers.GetTimeTier());
			var textTeir = ((AnnotationComponentFile)_file).Tiers.GetTranscriptionTier();
			textTeir.AddTimeRangeData(timeTier);

			var filter = "Text File (*.txt)|*.txt";
			var suffix = LocalizationManager.GetString("SessionsView.Transcription.TextAnnotation.ExportMenu.AudacityTranscriptionFilenameSuffix", "audacity_transcription");
			var fileName = _file.ParentElement.Id + "_"+suffix+".txt";
			var action = new Action<string>(path => AudacityExporter.Export(path, textTeir));

			DoSimpleExportDialog(".txt", filter, fileName, action);
		}

		private void OnPlainTextExportMenuItem_Click(object sender, EventArgs e)
		{
			var filter = "Text File (*.txt)|*.txt";
			var fileName = _file.ParentElement.Id + "_transcription.txt";
			var action = new Action<string>(path => PlainTextTranscriptionExporter.Export(path, (((AnnotationComponentFile) _file).Tiers)));

			DoSimpleExportDialog(".txt", filter, fileName, action);
		}

		private void OnCsvExportMenuItem_Click(object sender, EventArgs e)
		{
			var filter = "Comma Separated Values File (*.csv)|*.csv";
			var fileName = _file.ParentElement.Id + "_transcription.csv";
			var action = new Action<string>(path => CSVTranscriptionExporter.Export(path, (((AnnotationComponentFile) _file).Tiers)));

			DoSimpleExportDialog(".csv", filter, fileName, action);
		}

		private void OnToolboxInterlinearExportMenuItem_Click(object sender, EventArgs e)
		{
			var filter = "Toolbox Standard Format File (*.txt)|*.txt";
			var fileName = _file.ParentElement.Id + "_interlinear.txt";
			var mediaFileName = Path.GetFileName(AssociatedComponentFile.PathToAnnotatedFile);
			var action = new Action<string>(path => ToolboxTranscriptionExporter.Export(_file.ParentElement.Id, mediaFileName, path, (((AnnotationComponentFile)_file).Tiers)));

			DoSimpleExportDialog(".txt", filter, fileName, action);
		}

		private static void DoSimpleExportDialog(string defaultExt, string filter, string fileName, Action<string> action)
		{
			try
			{
				using (var dlg = new SaveFileDialog())
				{
					dlg.AddExtension = true;
					dlg.CheckPathExists = true;
					dlg.AutoUpgradeEnabled = true;
					dlg.DefaultExt = defaultExt;
					dlg.Filter = filter;
					dlg.FileName = fileName;
					dlg.RestoreDirectory = true;
					dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
					if (DialogResult.OK != dlg.ShowDialog())
						return;

					action(dlg.FileName);
					Process.Start("Explorer", "/select, \"" + dlg.FileName + "\"");
				}
			}
			catch (Exception error)
			{
				// Got a null refeence somwhere in here on 12/12/12. Not sure where/why, but if it happens
				// again, we want to be able to get a call stack so we can fix the problem.
				if (error is NullReferenceException)
					throw;
				ErrorReport.NotifyUserOfProblem(error, "There was a problem creating that file.\r\n\r\n" + error.Message);
			}
		}

		private static void DoSimpleVideoExportDialog(string defaultExt, string filter, string fileName, Action<string> action)
		{
			try
			{
				using (var dlg = new SaveFileDialog())
				{
					dlg.AddExtension = true;
					dlg.CheckPathExists = true;
					dlg.AutoUpgradeEnabled = true;
					dlg.DefaultExt = defaultExt;
					dlg.Filter = filter;
					dlg.FileName = fileName;
					dlg.RestoreDirectory = true;
					dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

					if (DialogResult.OK != dlg.ShowDialog())
						return;

					action(dlg.FileName);
					Process.Start("Explorer", "/select, \"" + dlg.FileName + "\"");
				}
			}
			catch (Exception error)
			{
				// Got a null refeence somwhere in here on 12/12/12. Not sure where/why, but if it happens
				// again, we want to be able to get a call stack so we can fix the problem.
				if (error is NullReferenceException)
					throw;
				ErrorReport.NotifyUserOfProblem(error, "There was a problem creating that file.\r\n\r\n" + error.Message);
			}
		}
	}
}
