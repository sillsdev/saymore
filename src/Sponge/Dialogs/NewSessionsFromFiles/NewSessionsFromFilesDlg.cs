using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AxWMPLib;
using SIL.Localization;
using SIL.Sponge.Dialogs.NewSessionsFromFiles.CopyFiles;
using SIL.Sponge.Properties;
using SilUtils;

namespace SIL.Sponge.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Dialog class in which the user may create many sessions based on files they choose,
	/// most likely from a recorder or flash memory.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class NewSessionsFromFilesDlg : Form
	{
		private readonly string _noFilesSelectedCreateButtonText;
		private readonly string _filesSelectedCreateButtonText;
		private readonly AxWindowsMediaPlayer _winMediaPlayer;
		private readonly NewSessionsFromFileDlgViewModel _viewModel;
		private readonly NewSessionsFromFilesDlgFolderNotFoundMsg _folderMissingMsgCtrl;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFilesDlg(NewSessionsFromFileDlgViewModel viewModel)
		{
			InitializeComponent();

			_instructionsLabel.Text = string.Format(_instructionsLabel.Text, Application.ProductName);
			_progressPanel.Visible = false;

			_noFilesSelectedCreateButtonText = LocalizationManager.LocalizeString(
				"NewSessionsFromFilesDlg.NoFilesSelectedCreateButtonText",
				"Create Sessions", "Text in create button when no files are selected.",
				"Dialog Boxes", LocalizationCategory.Button, LocalizationPriority.High);

			_filesSelectedCreateButtonText = LocalizationManager.LocalizeString(
				"NewSessionsFromFilesDlg.FilesSelectedCreateButtonText",
				"Create {0} Sessions", "Format text in create button when one or more files are selected.",
				"Dialog Boxes", LocalizationCategory.Button, LocalizationPriority.High);

			var rc = Settings.Default.NewSessionsFromFilesDlgBounds;
			if (rc.Height < 0)
				StartPosition = FormStartPosition.CenterScreen;
			else
				Bounds = rc;

			Sponge.SetGridColumnWidthsFromString(_filesGrid,
				Settings.Default.NewSessionsFromFilesDlgCols);

			_folderMissingMsgCtrl = new NewSessionsFromFilesDlgFolderNotFoundMsg();
			_filesPanel.Controls.Add(_folderMissingMsgCtrl);
			_folderMissingMsgCtrl.BringToFront();

			_mediaPlayerPanel.BorderStyle = BorderStyle.None;

#if !MONO
			_winMediaPlayer = new AxWindowsMediaPlayer();
			((ISupportInitialize)(_winMediaPlayer)).BeginInit();
			_winMediaPlayer.Dock = DockStyle.Fill;
			_winMediaPlayer.Name = "_winMediaPlayer";
			_mediaPlayerPanel.Controls.Add(_winMediaPlayer);
			((ISupportInitialize)(_winMediaPlayer)).EndInit();
			_winMediaPlayer.settings.autoStart = false;
#endif

			DialogResult = DialogResult.Cancel;
			new CheckBoxColumnHeaderHandler(_selectedCol);
			_viewModel = viewModel;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			_viewModel.SelectedFolder = Settings.Default.NewSessionsFromFilesLastFolder;
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save misc. info. about the state of the dialog (e.g. widths of columns in the
		/// files grid, size and location of the dialog box).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.Cancel && _progressPanel.Visible)
				_viewModel.CancelLoad();

			base.OnFormClosing(e);

			Settings.Default.NewSessionsFromFilesDlgCols =
				Sponge.StoreGridColumnWidthsInString(_filesGrid);

			Settings.Default.NewSessionsFromFilesDlgBounds = Bounds;
			Settings.Default.Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsMissingFolderMessageVisible
		{
			get { return _folderMissingMsgCtrl.Visible; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the state and content of the controls on the dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateDisplay()
		{
			Utils.SetWindowRedraw(this, false);

			_filesPanel.Visible = !string.IsNullOrEmpty(_viewModel.SelectedFolder);

			var showMissingFolderMsg =
				(!string.IsNullOrEmpty(_viewModel.SelectedFolder) &&
				!Directory.Exists(_viewModel.SelectedFolder));

			_sourceFolderLabel.ForeColor = (showMissingFolderMsg ?
				Color.Red : SystemColors.ControlText);

			_folderMissingMsgCtrl.Visible = showMissingFolderMsg;
			_folderMissingMsgCtrl.SetDriveLetterFromPath(_viewModel.SelectedFolder);
			_sourceFolderLabel.Text = _viewModel.SelectedFolder;

			int selectedFileCount = _viewModel.NumberOfSelectedFiles;
			_createSessionsButton.Enabled = _viewModel.AnyFilesSelected && !_progressPanel.Visible;
			_createSessionsButton.Text = (selectedFileCount == 0 ?
				_noFilesSelectedCreateButtonText :
				string.Format(_filesSelectedCreateButtonText, selectedFileCount));

			var fileCount = _viewModel.Files.Count;
			if (_filesGrid.RowCount != fileCount)
			{
				if (fileCount == 0)
				{
					_filesGrid.CellValueNeeded -= HandleFileGridCellValueNeeded;
					_filesGrid.CellValuePushed -= HandleFileGridCellValuePushed;
					_filesGrid.RowCount = fileCount;
					_mediaPlayerPanel.Enabled = false;
				}
				else
				{
					_filesGrid.CellValueNeeded += HandleFileGridCellValueNeeded;
					_filesGrid.RowCount = fileCount;
					_filesGrid.CellValuePushed += HandleFileGridCellValuePushed;
					_mediaPlayerPanel.Enabled = true;
					QueueMediaFile(_filesGrid.CurrentCellAddress.Y);
				}
			}

			Utils.SetWindowRedraw(this, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void QueueMediaFile(int rowIndex)
		{
#if !MONO
			_winMediaPlayer.Ctlcontrols.stop();
			_winMediaPlayer.URL = _viewModel.GetFullFilePath(rowIndex);
#endif
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Centers the progress bar panel in the file list grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SizeAndLocateProgressPanel()
		{
			_progressPanel.Width = (int)(_filesGrid.Width * .66);
			var pt = new Point((_filesGrid.Width - _progressPanel.Width) / 2,
				(_filesGrid.Height - _progressPanel.Height) / 2);

			_progressPanel.Location = PointToClient(_filesGrid.PointToScreen(pt));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InitializeProgressIndicatorForFileLoading(int fileCount)
		{
			_progressBar.Maximum = fileCount;
			_progressBar.Value = 0;

			SizeAndLocateProgressPanel();
			_progressPanel.BackColor = _filesGrid.BackgroundColor;
			_progressPanel.BringToFront();
			_progressPanel.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateFileLoadingProgress()
		{
			_progressBar.Increment(1);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void FileLoadingProgressComplele()
		{
			_progressPanel.Visible = false;
			UpdateDisplay();
		}

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles creating new sessions from the selected files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCreateSessionsButtonClick(object sender, EventArgs e)
		{
			Hide();

			using (var dialog = new MakeSessionsFromFileProgressDialog(_viewModel.GetSourceAndDestinationPairs(), _viewModel.CreateSingleSession))
			{
				dialog.ShowDialog(Form.ActiveForm);
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
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFilesGridRowEnter(object sender, DataGridViewCellEventArgs e)
		{
			QueueMediaFile(e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Feeds values about a file to the grid from the view model.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFileGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			e.Value = _viewModel.GetPropertyValueForFile(e.RowIndex,
				_filesGrid.Columns[e.ColumnIndex].DataPropertyName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Selected value changed in cell, so update the view model's file list accordingly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleFileGridCellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.ColumnIndex == 0)
				_viewModel.Files[e.RowIndex].Selected = (bool)e.Value;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleOuterTableLayoutSizeChanged(object sender, EventArgs e)
		{
			SizeAndLocateProgressPanel();
		}

		#endregion
	}
}
