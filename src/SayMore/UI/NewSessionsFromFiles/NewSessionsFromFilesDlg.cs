using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;
using SIL.Windows.Forms.Extensions;
using SIL.Windows.Forms.PortableSettingsProvider;
using SIL.Windows.Forms.Widgets.BetterGrid;
using SayMore.Properties;
using SayMore.Media.MPlayer;

namespace SayMore.UI.NewSessionsFromFiles
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Dialog class in which the user may create many sessions based on files they choose,
	/// most likely from a recorder or flash memory.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class NewSessionsFromFilesDlg : Form
	{
		//private readonly AxWindowsMediaPlayer _winMediaPlayer;
		private readonly NewSessionsFromFileDlgViewModel _viewModel;
		private readonly NewSessionsFromFilesDlgFolderNotFoundMsg _folderMissingMsgCtrl;
		private readonly CheckBoxColumnHeaderHandler _chkBoxColHdrHandler;
		private readonly MediaPlayerViewModel _mediaPlayerViewModel;
		private readonly MediaPlayer _mediaPlayer;

		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFilesDlg(NewSessionsFromFileDlgViewModel viewModel)
		{
			Logger.WriteEvent("NewSessionsFromFilesDlg constructor");

			InitializeComponent();

			var selectedCol = new DataGridViewCheckBoxColumn();
			selectedCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			selectedCol.DataPropertyName = "Selected";
			selectedCol.HeaderText = string.Empty;
			selectedCol.Name = "selectedCol";
			selectedCol.Resizable = DataGridViewTriState.False;
			selectedCol.SortMode = DataGridViewColumnSortMode.Automatic;
			_gridFiles.Grid.Columns.Insert(0, selectedCol);
			_chkBoxColHdrHandler = new CheckBoxColumnHeaderHandler(selectedCol);

			_gridFiles.InitializeGrid("NewSessionsFromFilesDlg");
			_gridFiles.AfterComponentSelectionChanged += HandleComponentFileSelected;

			Controls.Add(_panelProgress);

			_labelIncomingFiles.Font = Program.DialogFont;
			_labelInstructions.Font = Program.DialogFont;
			_linkFindFiles.Font = Program.DialogFont;
			_labelSourceFolder.Font = new Font(Program.DialogFont, FontStyle.Bold);
			_panelProgress.Visible = false;

			if (Settings.Default.NewSessionsFromFilesDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.NewSessionsFromFilesDlg = FormSettings.Create(this);
			}

			_folderMissingMsgCtrl = new NewSessionsFromFilesDlgFolderNotFoundMsg();
			_gridFiles.Controls.Add(_folderMissingMsgCtrl);
			_folderMissingMsgCtrl.BringToFront();

			_mediaPlayerPanel.BorderStyle = BorderStyle.None;

			_mediaPlayerViewModel = new MediaPlayerViewModel();

			_mediaPlayerViewModel.SetVolume(Settings.Default.MediaPlayerVolume);
			_mediaPlayer = new MediaPlayer(_mediaPlayerViewModel);
			_mediaPlayer.Dock = DockStyle.Fill;
			_mediaPlayerPanel.Controls.Add(_mediaPlayer);

			DialogResult = DialogResult.Cancel;
			_viewModel = viewModel;
			_viewModel.FilesChanged += UpdateDisplay;
			_viewModel.FileLoadingStarted += InitializeProgressIndicatorForFileLoading;
			_viewModel.FilesLoaded += UpdateFileLoadingProgress;
			_viewModel.FileLoadingCompleted += FileLoadingProgressComplete;
			_viewModel.Initialize(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			Settings.Default.NewSessionsFromFilesDlg.InitializeForm(this);
			base.OnShown(e);
			_mediaPlayerViewModel.VolumeChanged = () => Invoke((Action) HandleMediaPlayerVolumeChanged);
			_viewModel.SelectedFolder = Settings.Default.NewSessionsFromFilesLastFolder;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save misc. info. about the state of the dialog (e.g. widths of columns in the
		/// files grid, size and location of the dialog box).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.Cancel && _panelProgress.Visible)
				_viewModel.CancelLoad();

			_viewModel.FilesChanged -= UpdateDisplay;
			_viewModel.FileLoadingStarted -= InitializeProgressIndicatorForFileLoading;
			_viewModel.FilesLoaded -= UpdateFileLoadingProgress;
			_viewModel.FileLoadingCompleted -= FileLoadingProgressComplete;

			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the state and content of the controls on the dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay(ReadOnlyCollection<NewComponentFile> files, bool isFolderMissing)
		{
			this.SetWindowRedraw(false);

			_gridFiles.Visible = !string.IsNullOrEmpty(_viewModel.SelectedFolder);

			_labelIncomingFiles.Visible = !isFolderMissing;
			_labelInstructions.Visible = !isFolderMissing;

			_labelSourceFolder.ForeColor = (isFolderMissing ?
				Color.Red : SystemColors.ControlText);

			_folderMissingMsgCtrl.Visible = isFolderMissing;
			_folderMissingMsgCtrl.SetDriveLetterFromPath(_viewModel.SelectedFolder);
			_labelSourceFolder.Text = _viewModel.SelectedFolder;

			UpdateCreateButton();

			var fileCount = files.Count;

			if (_gridFiles.Grid.RowCount == fileCount)
				_mediaPlayerPanel.Enabled = (_gridFiles.Grid.RowCount > 0);
			else
			{
				_gridFiles.UpdateComponentFileList(files);

				if (fileCount == 0)
				{
					_gridFiles.Grid.CellValuePushed -= HandleFileGridCellValuePushed;
					_mediaPlayerPanel.Enabled = false;
				}
				else
				{
					_gridFiles.Grid.CellValuePushed += HandleFileGridCellValuePushed;
					_mediaPlayerPanel.Enabled = true;
					QueueMediaFile(_gridFiles.Grid.CurrentCellAddress.Y);
				}

				var selectedCount = files.Count(x => x.Selected);
				if (selectedCount == files.Count)
					_chkBoxColHdrHandler.HeadersCheckState = CheckState.Checked;
				else
				{
					_chkBoxColHdrHandler.HeadersCheckState = (selectedCount == 0 ?
						CheckState.Unchecked : CheckState.Indeterminate);
				}
			}

			this.SetWindowRedraw(true);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateCreateButton()
		{
			var selectedCount = _viewModel.NumberOfNewSessions;
			_buttonCreateSessions.Enabled = (selectedCount > 0 && !_panelProgress.Visible);

			if (selectedCount == 0)
			{
				var text = LocalizationManager.GetString("DialogBoxes.NewSessionsFromFilesDlg.NoFilesSelectedCreateButtonText",
					"Create Sessions", "Text in create button when no files are selected.");

				_buttonCreateSessions.Text = text;
			}
			else if (selectedCount == 1)
			{
				var fmt = LocalizationManager.GetString("DialogBoxes.NewSessionsFromFilesDlg.OneFileSelectedCreateButtonText",
					"Create 1 Session", "Text in create button when one file is selected.");

				_buttonCreateSessions.Text = string.Format(fmt, selectedCount);
			}
			else
			{
				var fmt = LocalizationManager.GetString("DialogBoxes.NewSessionsFromFilesDlg._buttonCreateSessions",
					"Create {0} Sessions", "Format text in create button when more than one file is selected. Parameter is the number of sessions to be created.");

				_buttonCreateSessions.Text = string.Format(fmt, selectedCount);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleComponentFileSelected(int index)
		{
			QueueMediaFile(index);
		}

		/// ------------------------------------------------------------------------------------
		private void QueueMediaFile(int rowIndex)
		{
			try
			{
				var filePath = _viewModel.GetFullFilePath(rowIndex);
				if (string.IsNullOrEmpty(filePath))
					return;
				_mediaPlayerViewModel.LoadFile(filePath);
			}
			catch (ObjectDisposedException)
			{
				// Safe to ignore
			}
			catch (Exception e)
			{
				if (InvokeRequired)
					Invoke((Action)(() => ErrorReport.NotifyUserOfProblem(e.Message)));
				else
					ErrorReport.NotifyUserOfProblem(e.Message);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeProgressIndicatorForFileLoading(int fileCount)
		{
			_progressBar.Maximum = fileCount;
			_progressBar.Value = 0;

			SizeAndLocateProgressPanel();
			_panelProgress.BackColor = _gridFiles.Grid.BackgroundColor;
			_panelProgress.BringToFront();
			_panelProgress.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateFileLoadingProgress(int numberOfFilesLoaded)
		{
			_progressBar.Increment(numberOfFilesLoaded);
		}

		/// ------------------------------------------------------------------------------------
		private void FileLoadingProgressComplete(ReadOnlyCollection<NewComponentFile> files, bool isFolderMissing)
		{
			_panelProgress.Visible = false;
			UpdateDisplay(files, isFolderMissing);

			// This is to fix a very frustrating problem. When the list is first loaded
			// when the dialog first gets displayed and when the first item is selected,
			// the checkbox value in the row doesn't reflect that. This will force the
			// selected cell to show the correct value.
			if (_gridFiles.Grid.RowCount > 0)
				_gridFiles.Grid.CurrentCell = _gridFiles.Grid[1, 0];
		}

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		void HandleMediaPlayerVolumeChanged()
		{
			Settings.Default.MediaPlayerVolume = _mediaPlayerViewModel.Volume;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles creating new sessions from the selected files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCreateSessionsButtonClick(object sender, EventArgs e)
		{
			Hide();

			_mediaPlayerViewModel.Stop(true);

			var pairs = _viewModel.GetUniqueSourceAndDestinationPairs().ToArray();

			if (pairs.Length == 0)
				return;

			var model = new CopyFilesViewModel(pairs);
			model.BeforeFileCopiedAction = _viewModel.CreateSingleSession;

			model.FileCopyFailedAction = (srcFile, dstFile) =>
			{
				if (File.Exists(dstFile))
					File.Delete(dstFile);
			};

			var caption = LocalizationManager.GetString(
				"DialogBoxes.NewSessionsFromFilesDlg.CreatingSessions.ProgressDlg.Caption",
				"Creating Sessions");

			using (var dlg = new ProgressDlg(model, caption))
			{
				dlg.StartPosition = FormStartPosition.CenterScreen;
				dlg.ShowDialog();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Displays an open file dialog box in which the user may specify files from which
		/// to make new sessions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFindFilesLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_viewModel.LetUserChangeSelectedFolder();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Selected value changed in cell, so update the view model's file list accordingly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleFileGridCellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				_viewModel.SetFileSelectionState(e.RowIndex, (bool) e.Value);
				UpdateCreateButton();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleOuterTableLayoutSizeChanged(object sender, EventArgs e)
		{
			SizeAndLocateProgressPanel();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Centers the progress bar panel in the file list grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SizeAndLocateProgressPanel()
		{
			_panelProgress.Width = (int)(_gridFiles.Width * .66);
			var pt = new Point((_gridFiles.Width - _panelProgress.Width) / 2,
				(_gridFiles.Height - _panelProgress.Height) / 2);

			_panelProgress.Location = PointToClient(_gridFiles.PointToScreen(pt));
		}

		#endregion
	}
}
