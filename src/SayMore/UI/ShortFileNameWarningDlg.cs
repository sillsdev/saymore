using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.UI;
using L10NSharp.XLiffUtils;
using SayMore.Utilities;
using SIL.Program;
using SIL.Windows.Forms.Widgets.BetterGrid;
using static System.String;
using Settings = SayMore.Properties.Settings;

namespace SayMore.UI
{
	public partial class ShortFileNameWarningDlg : Form
	{
		private readonly string _volume;
		private readonly string _extension;

		public bool SuppressFutureWarnings => _chkDoNotReportAnymoreThisSession.Checked || _chkDoNotReportEver.Checked;

		public ShortFileNameWarningDlg(string path, string failedAction)
		{
			if (IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			InitializeComponent();

			if (!File.Exists(path))
			{
				_lblMsg.Visible = false;
				_lblFilePath.Visible = false;
				_extension = null;
			}
			else
			{
				_lblFilePath.Text = path;
				_extension = Path.GetExtension(path).TrimStart('.');
			}

			_volume = FileSystemUtils.GetVolume(path);

			if (IsNullOrEmpty(failedAction))
				_lblFailedAction.Hide();
			else
				_lblFailedAction.Text = failedAction;

			_chkDoNotReportEver.Checked = Settings.Default.SuppressShortFilenameWarnings;

			// Initialize controls and checkboxes from settings
			InitializeVolumesChecklist();
			InitializeExtensionsChecklist();
			InitializeFoldersList();
			InitializeFilenameMatchList();

			LocalizeItemDlg<XLiffDocument>.StringsLocalized += HandleStringsLocalized;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeVolumesChecklist()
		{
			PopulateChecklist(_checkedListBoxVolumes, 
				FileSystemUtils.ShortFilenameWarningsToSuppressByVolume, _volume);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeExtensionsChecklist()
		{
			var extensions = FileSystemUtils.ShortFilenameWarningsToSuppressByExtension;
			if (IsNullOrEmpty(_extension) &&
			    extensions.Count == 0)
			{
				var i = _tableLayoutPanelMain.GetRow(_lblDoNotReportForExtensions);
				_lblDoNotReportForExtensions.Visible = false;
				_checkedListBoxExtensions.Visible = false;
				_tableLayoutPanelMain.RowStyles[i] = new RowStyle(SizeType.AutoSize);
			}

			PopulateChecklist(_checkedListBoxExtensions, extensions, _extension);
		}

		/// ------------------------------------------------------------------------------------
		private static void PopulateChecklist(CheckedListBox listbox,
			IReadOnlyList<string> existingCheckedValues, string newValue)
		{
			if (existingCheckedValues.Any())
			{
				listbox.Items.AddRange(existingCheckedValues.OfType<object>().ToArray());
				for (int i = 0; i < listbox.Items.Count; i++)
					listbox.SetItemChecked(i, true);
			}

			if (!IsNullOrEmpty(newValue))
			{
				if (!existingCheckedValues.Contains(newValue, StringComparer.OrdinalIgnoreCase))
					listbox.Items.Insert(0, newValue);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeFoldersList()
		{
			_chkDoNotReportForFolders.Checked = PopulateRegexMatchList(
				Settings.Default.ShortFilenameWarningsToSuppressByFolder,
				_gridFolders);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeFilenameMatchList()
		{
			_chkDoNotReportForFilesContaining.Checked = PopulateRegexMatchList(
				Settings.Default.ShortFilenameWarningsToSuppressByFilenameMatch,
				_gridFilenameContains);
		}

		/// ------------------------------------------------------------------------------------
		private static bool PopulateRegexMatchList(string setting, DataGridView grid)
		{
			var existingItems = setting.TrimStart('(').TrimEnd(')')
				.Split(new [] { ")|("}, StringSplitOptions.RemoveEmptyEntries)
				.Select(Regex.Unescape).ToList();

			grid.RowCount = existingItems.Count;

			if (!existingItems.Any())
				return false;

			grid.SuspendLayout();
			for (var index = 0; index < existingItems.Count; index++)
				grid.Rows[index].Cells[0].Value = existingItems[index];
			grid.ResumeLayout();

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected void HandleStringsLocalized(ILocalizationManager lm = null)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
			{
				if (_lblMsg.Visible)
					_lblMsg.Text = Format(_lblMsg.Text, Program.ProductName);
				else
				{
					Text = LocalizationManager.GetString(
						"MainWindow.ShortFileNameWarningSettingsDlgTitle",
						"Short Filename Warning Settings");
				}

				const string fsUtil8dot3 = "fsutil 8dot3name";

				if (_lblFilePath.Visible)
				{
					_linkLabelFsUtilMsg.Text = Format(_linkLabelFsUtilMsg.Text, fsUtil8dot3,
						_volume);
				}
				else
				{
					_linkLabelFsUtilMsg.Text = Format(LocalizationManager.GetString(
						"ShortFileNameWarningDlg.linkLabelFsUtilMsgForSettingsDlg",
						"If possible, you (or a system administrator) should use {0} to enable " +
						"creation of short \"8.3\" file names for file system volumes where " +
						"media files are located."), fsUtil8dot3);
				}

				_linkLabelFsUtilMsg.LinkArea = new LinkArea(
					_linkLabelFsUtilMsg.Text.IndexOf(fsUtil8dot3, StringComparison.Ordinal),
					fsUtil8dot3.Length);

				_chkDoNotReportAnymoreThisSession.Text =
					Format(_chkDoNotReportAnymoreThisSession.Text, Program.ProductName);
			}
		}
		
		/// ------------------------------------------------------------------------------------
		private void _linkLabelFsUtilMsg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.SafeStart("https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/fsutil-8dot3name");
		}

		/// ------------------------------------------------------------------------------------
		private static string GetSortedCheckedItems(CheckedListBox listbox, string separator = "")
		{
			var items = (from object item in listbox.CheckedItems select item.ToString()).ToList();
			items.Sort(StringComparer.OrdinalIgnoreCase);
			return Join(separator, items);
		}

		/// ------------------------------------------------------------------------------------
		private void _chkDoNotReportForFolders_CheckedChanged(object sender, EventArgs e)
		{
			_gridFolders.Enabled = _chkDoNotReportForFolders.Checked;
			_gridFolders.AllowUserToAddRows = true;
		}

		/// ------------------------------------------------------------------------------------
		private void _chkDoNotReportForFilesContaining_CheckedChanged(object sender, EventArgs e)
		{
			_gridFilenameContains.Enabled = _chkDoNotReportForFilesContaining.Checked;
			_gridFilenameContains.AllowUserToAddRows = true;
		}

		/// ------------------------------------------------------------------------------------
		private string InvalidCharactersMessage => LocalizationManager.GetString(
			"ShortFileNameWarningDlg.InvalidFilenameCharacters",
			"Value entered contains characters that are not valid: {0}");

		/// ------------------------------------------------------------------------------------
		private void RowValidating(object sender, DataGridViewCellCancelEventArgs e)
		{
			var grid = (BetterGrid)sender;

			if (grid.Rows[e.RowIndex].IsNewRow || !grid.IsCurrentRowDirty)
				return;

			var value = grid.Rows[e.RowIndex].Cells[0].Value?.ToString();
			if (IsNullOrWhiteSpace(value))
			{
				grid.CancelEdit();
				return;
			}

			var invalidChars = grid == _gridFolders ?
				Path.GetInvalidPathChars().Union(new[] { Path.VolumeSeparatorChar }) :
				Path.GetInvalidFileNameChars();

			var invalidCharsFound = value.Intersect(invalidChars).ToList();
			if (invalidCharsFound.Any())
			{
				e.Cancel = true;
				_gridFilenameContains.Rows[e.RowIndex].ErrorText =
					Format(InvalidCharactersMessage, Join(", ", invalidCharsFound));
			}
			else
			{
				foreach (var row in grid.GetRows())
				{
					if (row.Index != e.RowIndex && row.Cells[0].Value == value)
					{
						e.Cancel = true;
						// TODO: Localize
						row.ErrorText = "Duplicate";
						return;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		void CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			var grid = (BetterGrid)sender;
			if (grid.IsCurrentCellDirty)
				grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		/// ------------------------------------------------------------------------------------
		private void RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			//var grid = (BetterGrid)sender;
			//if (!(grid.Tag is int rowIndex) || rowIndex == e.RowIndex)
			//	return;
			//RemoveEmptyRow(grid, rowIndex);
		}

		/// ------------------------------------------------------------------------------------
		private void GridLeave(object sender, EventArgs e)
		{
			var grid = (BetterGrid)sender;
			RemoveEmptyRows(grid);
		}

		/// ------------------------------------------------------------------------------------
		private void RemoveEmptyRows(BetterGrid grid)
		{
			BeginInvoke(new Action(() =>
			{
				if (grid.IsDisposed || grid.Disposing || grid.ContainsFocus)
					return;
				var rowsToDelete = new List<int>();
				for (var i = 0; i < grid.RowCountLessNewRow; i++)
				{
					if (IsNullOrWhiteSpace((string)grid.Rows[i].Cells[0].Value))
						rowsToDelete.Insert(0, i);
				}

				foreach (var rowIndex in rowsToDelete)
					grid.Rows.RemoveAt(rowIndex);
			}));
		
		}

		/// ------------------------------------------------------------------------------------
		private void _chkDoNotReportEver_CheckedChanged(object sender, EventArgs e)
		{
			if (_chkDoNotReportEver.Checked)
			{
				_lblDoNotReportForVolumes.Enabled = false;
				_checkedListBoxVolumes.Enabled = false;
				_lblDoNotReportForExtensions.Enabled = false;
				_checkedListBoxExtensions.Enabled = false;
				_chkDoNotReportForFolders.Enabled = false;
				_gridFolders.Enabled = false;
				_chkDoNotReportForFilesContaining.Enabled = false;
				_gridFilenameContains.Enabled = false;
				_chkDoNotReportAnymoreThisSession.Enabled = false;
			}
			else
			{
				_lblDoNotReportForVolumes.Enabled = true;
				_checkedListBoxVolumes.Enabled = true;
				_lblDoNotReportForExtensions.Enabled = true;
				_checkedListBoxExtensions.Enabled = true;
				_chkDoNotReportForFolders.Enabled = true;
				_gridFolders.Enabled = _chkDoNotReportForFolders.Checked;
				_chkDoNotReportForFilesContaining.Enabled = true;
				_gridFilenameContains.Enabled = _chkDoNotReportForFilesContaining.Checked;
				_chkDoNotReportAnymoreThisSession.Enabled = true;		
			}
		}

		/// ------------------------------------------------------------------------------------
		private void _btnOK_Click(object sender, EventArgs e)
		{
			if (_chkDoNotReportEver.Checked)
			{
				Settings.Default.SuppressShortFilenameWarnings = true;
				return;
			}

			if (_gridFolders.IsCurrentCellDirty)
				_gridFolders.CommitEdit(DataGridViewDataErrorContexts.Commit);

			if (_gridFilenameContains.IsCurrentCellDirty)
				_gridFilenameContains.CommitEdit(DataGridViewDataErrorContexts.Commit);

			Settings.Default.SuppressShortFilenameWarnings = false;

			Settings.Default.ShortFilenameWarningsToSuppressByVolume = 
				GetSortedCheckedItems(_checkedListBoxVolumes);

			Settings.Default.ShortFilenameWarningsToSuppressByExtension =
				GetSortedCheckedItems(_checkedListBoxExtensions, ".");

			Settings.Default.ShortFilenameWarningsToSuppressByFolder = 
				GetRegexFromList(_chkDoNotReportForFolders, _gridFolders);
			
			Settings.Default.ShortFilenameWarningsToSuppressByFilenameMatch =
				GetRegexFromList(_chkDoNotReportForFilesContaining, _gridFilenameContains);
		}

		private static string GetRegexFromList(CheckBox checkBox, BetterGrid grid)
		{
			if (checkBox.Checked && grid.RowCountLessNewRow > 0)
			{
				var items = grid.GetRows().Select(r =>
						(string)r.Cells[0].Value).Where(v => !IsNullOrWhiteSpace(v))
					.Select(Regex.Escape).ToList();
				return "(" + Join(")|(", items) + ")";
			}
			return Empty;
		}
	}
}
