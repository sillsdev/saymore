using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
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
		private readonly NewSessionsFromFileDlgViewModel _viewModel;
		private readonly NewSessionsFromFilesDlgFolderNotFoundMsg _folderMissingMsgCtrl;
		private readonly CheckBoxColumnHeaderHandler _selectedColCheckHandler;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFilesDlg(NewSessionsFromFileDlgViewModel viewModel)
		{
			InitializeComponent();

			_instructionsLabel.Text = string.Format(_instructionsLabel.Text, Application.ProductName);

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

			_viewModel = viewModel;

			_selectedColCheckHandler = new CheckBoxColumnHeaderHandler(_selectedCol);
			_selectedColCheckHandler.CheckChanged += HandleSelectAllChanged;

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
			base.OnFormClosing(e);

			Settings.Default.NewSessionsFromFilesDlgCols =
				Sponge.StoreGridColumnWidthsInString(_filesGrid);

			Settings.Default.NewSessionsFromFilesDlgBounds = Bounds;
			Settings.Default.Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the folder selected is missing. When this
		/// dialog first loads and this is true, it means the selected folder chosen the last
		/// time the user opened this dialog box is now missing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsSelectedFolderMissing
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
			_createSessionsButton.Enabled = _viewModel.AnyFilesSelected;
			_createSessionsButton.Text = (selectedFileCount == 0 ?
				_noFilesSelectedCreateButtonText :
				string.Format(_filesSelectedCreateButtonText, selectedFileCount));

			var fileCount = _viewModel.Files.Count;
			if (_filesGrid.RowCount != fileCount)
			{
				if (fileCount == 0)
				{
					_filesGrid.CellValueNeeded -= HandleFileGridCellValueNeeded;
					_filesGrid.RowCount = fileCount;
				}
				else
				{
					_filesGrid.RowCount = fileCount;
					_filesGrid.CellValueNeeded += HandleFileGridCellValueNeeded;
				}
			}

			SetSelectedColumnHeaderCheckBoxState();

			Utils.SetWindowRedraw(this, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetSelectedColumnHeaderCheckBoxState()
		{
			if (_viewModel.AllFilesSelected)
				_selectedColCheckHandler.CheckState = CheckState.Checked;
			else if (_viewModel.AnyFilesSelected)
				_selectedColCheckHandler.CheckState = CheckState.Indeterminate;
			else
				_selectedColCheckHandler.CheckState = CheckState.Unchecked;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleSelectAllChanged(object sender, EventArgs e)
		{
			_viewModel.SelectAllFiles(_selectedColCheckHandler.CheckState == CheckState.Checked);

			if (_filesGrid.IsCurrentCellInEditMode)
				_filesGrid.EndEdit();

			_filesGrid.InvalidateColumn(_selectedCol.Index);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles creating new sessions from the selected files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCreateSessionsButtonClick(object sender, EventArgs e)
		{
			Hide();
			_viewModel.CreateSessions();
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
		/// Feeds values about a file to the grid from the view model.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFileGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			e.Value = _viewModel.GetPropertyValueForFile(e.RowIndex,
				_filesGrid.Columns[e.ColumnIndex].DataPropertyName);

			SetSelectedColumnHeaderCheckBoxState();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles toggling the selected state of a file in the file list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFilesGridCellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 0)
				_viewModel.ToggleFilesSelectedState(e.RowIndex);
		}
	}
}
