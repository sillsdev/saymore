using System;
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
		private readonly NewSessionsFromFileModel m_viewModel;

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

			m_viewModel = new NewSessionsFromFileModel();
			UpdateDisplay();
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
		/// Updates the controls on the dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			Utils.SetWindowRedraw(this, false);

			m_filesPanel.Visible = (m_viewModel.Files.Count > 0);

			int selectedFileCount = m_viewModel.NumberOfSelectedFiles;
			m_createSessionsButton.Enabled = m_viewModel.AnyFilesSelected;
			m_createSessionsButton.Text = (selectedFileCount == 0 ?
				m_noFilesSelectedCreateButtonText :
				string.Format(m_filesSelectedCreateButtonText, selectedFileCount));

			if (m_filesGrid.RowCount != m_viewModel.Files.Count)
				m_filesGrid.RowCount = m_viewModel.Files.Count;

			m_sourceFolderLabel.Text = (m_filesGrid.CurrentCellAddress.Y < 0 ? string.Empty :
				m_viewModel.Files[m_filesGrid.CurrentCellAddress.Y].Folder);

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
			var caption = LocalizationManager.LocalizeString(
				"NewSessionsFromFilesDlg.OpenFileDlgCaption", "Choose Sessions Files", "Dialog Boxes");

			var fileNames = Sponge.GetFilesOfAnyType(caption);

			if (fileNames != null)
			{
				foreach (string file in fileNames)
					m_viewModel.AddFile(file);

				UpdateDisplay();
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
		/// Update the display when a row is entered.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFilesGridRowEnter(object sender, DataGridViewCellEventArgs e)
		{
			UpdateDisplay();
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
