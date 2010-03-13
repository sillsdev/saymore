using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
using SIL.Sponge.Model;
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
		private readonly NewSessionsFromFileDlgModel _viewModel;
		private readonly NewSessionsFromFilesDlgFolderNotFoundMsg _folderMissingMsgCtrl;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFilesDlg()
		{
			InitializeComponent();

			_filesGrid.Font = SystemFonts.IconTitleFont;
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

			_viewModel = new NewSessionsFromFileDlgModel();
			UpdateDisplay();

			Application.Idle += HandleApplicationIdle;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Monitor the selected folder in case it disappears or reappears (e.g. when the user
		/// plug-in or unplugs the device containing the folder).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleApplicationIdle(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(_viewModel.SelectedFolder))
				return;

			if ((!Directory.Exists(_viewModel.SelectedFolder) && !_folderMissingMsgCtrl.Visible) ||
				(Directory.Exists(_viewModel.SelectedFolder) && _folderMissingMsgCtrl.Visible))
			{
				_viewModel.Refresh();
				UpdateDisplay();
			}
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

			Application.Idle -= HandleApplicationIdle;

			Settings.Default.NewSessionsFromFilesDlgCols =
				Sponge.StoreGridColumnWidthsInString(_filesGrid);

			Settings.Default.NewSessionsFromFilesDlgBounds = Bounds;
			Settings.Default.Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the Id of the first of the newly added sessions. If the user closes the
		/// the dialog box without adding any new sessions, then null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FirstNewSessionAdded
		{
			get { return _viewModel.FirstNewSessionAdded; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the state and content of the controls on the dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
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

			Utils.SetWindowRedraw(this, true);
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
			using (var dlg = new FolderBrowserDialog())
			{
				dlg.Description = LocalizationManager.LocalizeString(
					"NewSessionsFromFilesDlg.FolderBrowserDlgDescription",
					"Choose a Folder Medial Files.", "Dialog Boxes");

				if (_viewModel.SelectedFolder != null && Directory.Exists(_viewModel.SelectedFolder))
					dlg.SelectedPath = _viewModel.SelectedFolder;

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					_viewModel.SelectedFolder = dlg.SelectedPath;
					UpdateDisplay();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Feeds values about a file to the grid from the view model.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFileGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex >= 0 && e.RowIndex < _viewModel.Files.Count)
			{
				var field = _filesGrid.Columns[e.ColumnIndex].DataPropertyName;
				e.Value = ReflectionHelper.GetProperty(_viewModel.Files[e.RowIndex], field);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles toggling the selected state of a file in the file list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFilesGridCellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				_viewModel.Files[e.RowIndex].Selected = !_viewModel.Files[e.RowIndex].Selected;
				UpdateDisplay();
			}
		}
	}
}
