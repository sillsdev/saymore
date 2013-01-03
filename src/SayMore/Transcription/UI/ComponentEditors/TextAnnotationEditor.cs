using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Localization;
using Palaso.Reporting;
using SayMore.Media.Audio;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Transcription.Model.Exporters;
using SayMore.UI.ComponentEditors;
using SayMore.Media.MPlayer;
using SilTools;
using SayMore.Model;

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

			Utils.SetWindowRedraw(this, false);
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
			Utils.SetWindowRedraw(this, true);
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
			Utils.SetWindowRedraw(this, false);

			_videoPanel.Size = new Size(_splitter.Panel1.ClientSize.Width,
				(int)(_splitter.Panel1.ClientSize.Width * Settings.Default.AnnotationEditorVideoWindowYtoXRatio));

			if (_videoPanel.Width > _splitter.Panel1.ClientSize.Width)
				_videoPanel.Width = _splitter.Panel1.ClientSize.Width;

			if (_videoPanel.Height > _splitter.Panel1.ClientSize.Height)
				_videoPanel.Height = _splitter.Panel1.ClientSize.Height;

			if (!_grid.PlayerViewModel.HasPlaybackStarted)
				_videoPanel.ShowVideoThumbnailNow();

			Utils.SetWindowRedraw(this, true);
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
			_grid.Stop();
			_grid.EndEdit();
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
						if (originallySelectedCell != _grid.CurrentCellAddress)
						{
							// User has already selected a different cell, so go ahead and
							// start playback after refreshing the grid.
							originallySelectedCell = _grid.CurrentCellAddress;
							startPlayBackWhenFinished = true;
						}
						SetComponentFile(_file);
						if (_grid.RowCount > originallySelectedCell.Y && _grid.ColumnCount > originallySelectedCell.X)
							_grid.CurrentCell = _grid.Rows[originallySelectedCell.Y].Cells[originallySelectedCell.X];
						else
							startPlayBackWhenFinished = false; // Oops, that cell is gone!
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

		private void DoExportSubtitleDialog(string fileNameSuffix, TimeTier timeTier, TextTier textTeir)
		{
			textTeir.AddTimeRangeData(timeTier);

			var filter =  "SRT Subtitle File (*.srt)|*.srt";
			var fileName =_file.ParentElement.Id + ComponentRole.kFileSuffixSeparator + fileNameSuffix + ".srt";
			var action = new Action<string>(path => SRTFormatSubTitleExporter.Export(path, textTeir));

			DoSimpleExportDialog(".srt", filter, fileName, action);
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
	}
}
