using System;
using System.Linq;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AxWMPLib;
using SayMore.Model.Files;
using SayMore.Properties;
using SIL.Localization;
using SilUtils;

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
		private readonly AxWindowsMediaPlayer _winMediaPlayer;
		private readonly NewSessionsFromFileDlgViewModel _viewModel;
		private readonly NewSessionsFromFilesDlgFolderNotFoundMsg _folderMissingMsgCtrl;
		private readonly CheckBoxColumnHeaderHandler _chkBoxColHdrHandler;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFilesDlg(NewSessionsFromFileDlgViewModel viewModel)
		{
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
			_gridFiles.ComponentSelectedCallback = HandleComponentFileSelected;

			Controls.Add(_panelProgress);

			_labelIncomingFiles.Font = SystemFonts.IconTitleFont;
			_labelInstructions.Font = SystemFonts.IconTitleFont;
			_linkFindFiles.Font = SystemFonts.IconTitleFont;
			_labelSourceFolder.Font = new Font(SystemFonts.IconTitleFont, FontStyle.Bold);

			_labelInstructions.Text = string.Format(_labelInstructions.Text, Application.ProductName);
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
			_viewModel = viewModel;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			Settings.Default.NewSessionsFromFilesDlg.InitializeForm(this);
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
			if (DialogResult == DialogResult.Cancel && _panelProgress.Visible)
				_viewModel.CancelLoad();

			base.OnFormClosing(e);
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

			_gridFiles.Visible = !string.IsNullOrEmpty(_viewModel.SelectedFolder);

			var showMissingFolderMsg =
				(!string.IsNullOrEmpty(_viewModel.SelectedFolder) &&
				!Directory.Exists(_viewModel.SelectedFolder));

			_labelIncomingFiles.Visible = !showMissingFolderMsg;
			_labelInstructions.Visible = !showMissingFolderMsg;

			_labelSourceFolder.ForeColor = (showMissingFolderMsg ?
				Color.Red : SystemColors.ControlText);

			_folderMissingMsgCtrl.Visible = showMissingFolderMsg;
			_folderMissingMsgCtrl.SetDriveLetterFromPath(_viewModel.SelectedFolder);
			_labelSourceFolder.Text = _viewModel.SelectedFolder;

			UpdateCreateButton();

			var fileCount = _viewModel.Files.Count;

			if (_gridFiles.Grid.RowCount != fileCount)
			{
				_gridFiles.UpdateComponentList(_viewModel.Files.Cast<ComponentFile>());

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

				var selectedCount = _viewModel.Files.Where(x => x.Selected).Count();
				if (selectedCount == _viewModel.Files.Count)
					_chkBoxColHdrHandler.HeadersCheckState = CheckState.Checked;
				else
				{
					_chkBoxColHdrHandler.HeadersCheckState = (selectedCount == 0 ?
						CheckState.Unchecked : CheckState.Indeterminate);
				}
			}

			Utils.SetWindowRedraw(this, true);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateCreateButton()
		{
			var selectedCount = _viewModel.NumberOfSelectedFiles;
			_createSessionsButton.Enabled = (selectedCount > 0 && !_panelProgress.Visible);

			if (selectedCount == 0)
			{
				var text = LocalizationManager.LocalizeString(
					"NewSessionsFromFilesDlg.NoFilesSelectedCreateButtonText",
					"Create Sessions", "Text in create button when no files are selected.",
					"Dialog Boxes", LocalizationCategory.Button, LocalizationPriority.High);

				_createSessionsButton.Text = text;
			}
			else
			{
				var fmt = LocalizationManager.LocalizeString(
					"NewSessionsFromFilesDlg.FilesSelectedCreateButtonText",
					"Create {0} Sessions", "Format text in create button when one or more files are selected.",
					"Dialog Boxes", LocalizationCategory.Button, LocalizationPriority.High);

				_createSessionsButton.Text = string.Format(fmt, selectedCount);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleComponentFileSelected(int index)
		{
			QueueMediaFile(index);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void QueueMediaFile(int rowIndex)
		{
#if !MONO
			if (rowIndex >= 0)
			{
				_winMediaPlayer.Ctlcontrols.stop();
				_winMediaPlayer.URL = _viewModel.GetFullFilePath(rowIndex);
			}
#endif
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
			_panelProgress.BackColor = _gridFiles.Grid.BackgroundColor;
			_panelProgress.BringToFront();
			_panelProgress.Visible = true;
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
			_panelProgress.Visible = false;
			UpdateDisplay();

			// This is to fix a very frustrating problem. When the list is first loaded
			// when the dialog first gets displayed and when the first item is selected,
			// the checkbox value in the row doesn't reflect that. This will force the
			// selected cell to show the correct value.
			if (_gridFiles.Grid.RowCount > 0)
				_gridFiles.Grid.CurrentCell = _gridFiles.Grid[1, 0];
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

			using (var dlg = new MakeSessionsFromFileProgressDialog(
				_viewModel.GetSourceAndDestinationPairs(), _viewModel.CreateSingleSession))
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
				_viewModel.Files[e.RowIndex].Selected = (bool)e.Value;
				UpdateCreateButton();
			}
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
