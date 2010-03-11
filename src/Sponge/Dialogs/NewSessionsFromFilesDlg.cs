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
		private readonly string m_noFilesSelectedCreateButtonText;
		private readonly string m_filesSelectedCreateButtonText;
		private readonly NewSessionsFromFileDlgModel m_viewModel;
		private readonly NewSessionsFromFilesDlgFolderNotFoundMsg m_folderMissingMsgCtrl;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NewSessionsFromFilesDlg"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFilesDlg()
		{
			InitializeComponent();

			m_instructionsLabel.Text = string.Format(m_instructionsLabel.Text, Application.ProductName);

			m_noFilesSelectedCreateButtonText = LocalizationManager.LocalizeString(
				"NewSessionsFromFilesDlg.NoFilesSelectedCreateButtonText",
				"Create Sessions", "Text in create button when no files are selected.",
				"Dialog Boxes", LocalizationCategory.Button, LocalizationPriority.High);

			m_filesSelectedCreateButtonText = LocalizationManager.LocalizeString(
				"NewSessionsFromFilesDlg.FilesSelectedCreateButtonText",
				"Create {0} Sessions", "Format text in create button when one or more files are selected.",
				"Dialog Boxes", LocalizationCategory.Button, LocalizationPriority.High);

			var rc = Settings.Default.NewSessionsFromFilesDlgBounds;
			if (rc.Height < 0)
				StartPosition = FormStartPosition.CenterScreen;
			else
				Bounds = rc;

			Sponge.SetGridColumnWidthsFromString(m_filesGrid,
				Settings.Default.NewSessionsFromFilesDlgCols);

			m_folderMissingMsgCtrl = new NewSessionsFromFilesDlgFolderNotFoundMsg();
			m_filesPanel.Controls.Add(m_folderMissingMsgCtrl);
			m_folderMissingMsgCtrl.BringToFront();

			m_viewModel = new NewSessionsFromFileDlgModel();
			UpdateDisplay();

			Application.Idle += HandleApplicationIdle;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the application idle.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleApplicationIdle(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(m_viewModel.SelectedFolder))
				return;

			if ((!Directory.Exists(m_viewModel.SelectedFolder) && !m_folderMissingMsgCtrl.Visible) ||
				(Directory.Exists(m_viewModel.SelectedFolder) && m_folderMissingMsgCtrl.Visible))
			{
				m_viewModel.Refresh();
				UpdateDisplay();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			Settings.Default.NewSessionsFromFilesDlgCols =
				Sponge.StoreGridColumnWidthsInString(m_filesGrid);

			Settings.Default.NewSessionsFromFilesDlgBounds = Bounds;
			Settings.Default.Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the state and content of the controls on the dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			Utils.SetWindowRedraw(this, false);

			m_filesPanel.Visible = !string.IsNullOrEmpty(m_viewModel.SelectedFolder);

			var showMissingFolderMsg =
				(!string.IsNullOrEmpty(m_viewModel.SelectedFolder) &&
				!Directory.Exists(m_viewModel.SelectedFolder));

			m_sourceFolderLabel.ForeColor = (showMissingFolderMsg ?
				Color.Red : SystemColors.ControlText);

			m_folderMissingMsgCtrl.Visible = showMissingFolderMsg;
			m_folderMissingMsgCtrl.SetDriveLetterFromPath(m_viewModel.SelectedFolder);
			m_sourceFolderLabel.Text = m_viewModel.SelectedFolder;

			int selectedFileCount = m_viewModel.NumberOfSelectedFiles;
			m_createSessionsButton.Enabled = m_viewModel.AnyFilesSelected;
			m_createSessionsButton.Text = (selectedFileCount == 0 ?
				m_noFilesSelectedCreateButtonText :
				string.Format(m_filesSelectedCreateButtonText, selectedFileCount));

			var fileCount = m_viewModel.Files.Count;
			if (m_filesGrid.RowCount != fileCount)
			{
				if (fileCount == 0)
				{
					m_filesGrid.CellValueNeeded -= HandleFileGridCellValueNeeded;
					m_filesGrid.RowCount = fileCount;
				}
				else
				{
					m_filesGrid.RowCount = fileCount;
					m_filesGrid.CellValueNeeded += HandleFileGridCellValueNeeded;
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

				if (m_viewModel.SelectedFolder != null && Directory.Exists(m_viewModel.SelectedFolder))
					dlg.SelectedPath = m_viewModel.SelectedFolder;

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					m_viewModel.SelectedFolder = dlg.SelectedPath;
					UpdateDisplay();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns values to the grid from the view model about a file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFileGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			int row = e.RowIndex;

			if (row < 0 || row >= m_viewModel.Files.Count)
				return;

			switch (e.ColumnIndex)
			{
				case 0: e.Value = m_viewModel.Files[row].Selected; break;
				case 1: e.Value = m_viewModel.Files[row].SmallIcon; break;
				case 2: e.Value = m_viewModel.Files[row].FileName; break;
				case 3: e.Value = m_viewModel.Files[row].FileType; break;
				case 4: e.Value = m_viewModel.Files[row].DateModified; break;
				case 5: e.Value = m_viewModel.Files[row].FileSize; break;
				case 6: e.Value = null; break;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the files grid cell content click.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFilesGridCellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				m_viewModel.Files[e.RowIndex].Selected = !m_viewModel.Files[e.RowIndex].Selected;
				UpdateDisplay();
			}
		}
	}
}
