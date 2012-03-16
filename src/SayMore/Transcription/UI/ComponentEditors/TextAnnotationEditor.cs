using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Localization;
using Palaso.Reporting;
using SayMore.Media;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.UI.ComponentEditors;
using SayMore.Media.UI;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class TextAnnotationEditor : EditorBase
	{
		private readonly TextAnnotationEditorGrid _grid;
		private readonly VideoPanel _videoPanel;
		private FileSystemWatcher _watcher;
		private bool _isFirstTimeActivated = true;

		/// ------------------------------------------------------------------------------------
		public TextAnnotationEditor(ComponentFile file, string imageKey) : base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "Annotations";
			_toolStrip.Renderer = new NoToolStripBorderRenderer();

			_comboPlaybackSpeed.Font = SystemFonts.IconTitleFont;

			_grid = new TextAnnotationEditorGrid();
			_grid.Dock = DockStyle.Fill;
			_splitter.Panel2.Controls.Add(_grid);

			LoadPlaybackSpeedCombo();
			_comboPlaybackSpeed.SelectedValueChanged += HandlePlaybackSpeedValueChanged;
			SetSpeedPercentage(Settings.Default.AnnotationEditorPlaybackSpeedIndex);

			_videoPanel = new VideoPanel();
			_videoPanel.BackColor = Color.Black;
			_videoPanel.SetPlayerViewModel(_grid.PlayerViewModel);
			_splitter.Panel1.Controls.Add(_videoPanel);

			SetComponentFile(file);
			_splitter.Panel1.ClientSizeChanged += HandleSplitterPanel1ClientSizeChanged;

			_buttonHelp.Click += delegate
			{
				Program.ShowHelpTopic("/Using_Tools/Events_tab/Working_with_annotations.htm");
			};
		}

		/// ------------------------------------------------------------------------------------
		private void LoadPlaybackSpeedCombo()
		{
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.100Pct", "100% (Normal)"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.90Pct", "90%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.80Pct", "80%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.70Pct", "70%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.60Pct", "60%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.50Pct", "50%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.40Pct", "40%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.30Pct", "30%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.20Pct", "20%"));
			_comboPlaybackSpeed.Items.Add(LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.PlaybackSpeeds.10Pct", "10%"));
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
				var msg = LocalizationManager.GetString("EventsView.Transcription.TextAnnotationEditor.LoadingAnnotationFileErrorMsg",
					"There was an error loading the annotation file '{0}'.");

				ErrorReport.NotifyUserOfProblem(exception, msg, file.PathToAnnotatedFile);
			}

			_grid.Load(annotationFile);

			_buttonRecordings.Enabled = (_grid.RowCount > 0);
			_buttonResegment.Enabled = (_grid.RowCount > 0);
			_buttonExport.Enabled = (_grid.RowCount > 0);

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
			_grid.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleExportButtonClick(object sender, EventArgs e)
		{
			var file = (AnnotationComponentFile)_file;
			var mediaFileName = Path.GetFileName(file.GetPathToAssociatedMediaFile());

			using (var dlg = new ExportToFieldWorksInterlinearDlg(mediaFileName))
			{
				if (dlg.ShowDialog() == DialogResult.Cancel)
					return;

				InterlinearXmlHelper.Save(dlg.FileName, mediaFileName,
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
			}));
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void HandleRecordedAnnotationButtonClick(object sender, EventArgs e)
		{
			if (!AudioUtils.WarnUserIfOSCannotRecord())
				return;

			var annotationType = (sender == _buttonCarefulSpeech ?
				OralAnnotationType.Careful : OralAnnotationType.Translation);

			if (AssociatedComponentFile.RecordAnnotations(annotationType) != null &&
				ComponentFileListRefreshAction != null)
			{
				ComponentFileListRefreshAction(_file.PathToAnnotatedFile, null);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleResegmentButtonClick(object sender, EventArgs e)
		{
			HandleBeforeAnnotationFileSaved(null, null);

			if (ManualSegmenterDlg.ShowDialog(AssociatedComponentFile, this) != null)
				SetComponentFile(_file);

			HandleAfterAnnotationFileSaved(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.TabText", "Annotations");

			base.HandleStringsLocalized();
		}
	}
}
