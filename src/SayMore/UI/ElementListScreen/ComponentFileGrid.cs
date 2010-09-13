using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using SilUtils;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	public partial class ComponentFileGrid : UserControl
	{
		private IEnumerable<ComponentFile> _files;
		private string _gridColSettingPrefix;

		/// <summary>
		/// When the user selects a different component, this is called
		/// </summary>
		public Action<int> AfterComponentSelected;

		/// <summary>
		/// When the user chooses a menu command, this is called after the command is issued.
		/// </summary>
		public Action PostMenuCommandRefreshAction;

		public Func<string[], DragDropEffects> FilesBeingDraggedOverGrid;
		public Func<string[], bool> FilesDroppedOnGrid;
		public Func<string[], bool> FilesAdded;

		public bool ShowContextMenu { get; set; }

		/// ------------------------------------------------------------------------------------
		public ComponentFileGrid()
		{
			ShowContextMenu = true;

			InitializeComponent();

			try
			{
				// Setting AllowDrop when tests are running throws an exception and
				// I'm not quite sure why, even after using reflector to look at the
				// code behind setting the property. Nonetheless, I've seen this before
				// so I'm just going to ignore any exception thrown when enabling drag
				// and drop. The worst that could happen by ignorning an exception
				// when the user runs the program (which should never happen), is that
				// they won't have the drag/drop feature.
				_grid.AllowDrop = true;
			}
			catch { }

			_grid.DragEnter += HandleFileGridDragEnter;
			_grid.DragDrop += HandleFileGridDragDrop;
			_grid.CellMouseClick += HandleFileGridCellMouseClick;
			_grid.CellValueNeeded += HandleFileGridCellValueNeeded;
			_grid.CellDoubleClick += HandleFileGridCellDoubleClick;
			_grid.CurrentRowChanged += HandleFileGridCurrentRowChanged;
			_grid.Paint += HandleFileGridPaint;
			_grid.ClientSizeChanged += HandleFileGridClientSizeChanged;
			_grid.Font = SystemFonts.IconTitleFont;
			_grid.DefaultCellStyle.SelectionForeColor = _grid.DefaultCellStyle.ForeColor;

			var clr = ColorHelper.CalculateColor(Color.White,
				 _grid.DefaultCellStyle.SelectionBackColor, 140);

			_grid.DefaultCellStyle.SelectionBackColor = clr;
		}

		/// ------------------------------------------------------------------------------------
		public void InitializeGrid(string settingPrefix)
		{
			_gridColSettingPrefix = settingPrefix;

			if (Settings.Default[_gridColSettingPrefix + "ComponentGrid"] != null)
				((GridSettings)Settings.Default[_gridColSettingPrefix + "ComponentGrid"]).InitializeGrid(_grid);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (!DesignMode)
				Settings.Default[_gridColSettingPrefix + "ComponentGrid"] = GridSettings.Create(_grid);

			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		public bool AddButtonEnabled
		{
			get { return _buttonAddFiles.Enabled; }
			set { _buttonAddFiles.Enabled = value; }
		}

		/// ------------------------------------------------------------------------------------
		public bool AddButtonVisible
		{
			get { return _buttonAddFiles.Visible; }
			set
			{
				if (_buttonAddFiles.Visible != value)
					_buttonAddFiles.Visible = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool RenameButtonVisible
		{
			get { return _buttonRename.Visible; }
			set
			{
				if (_buttonRename.Visible != value)
					_buttonRename.Visible = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool ConvertButtonVisible
		{
			get { return _buttonConvert.Visible; }
			set
			{
				if (_buttonConvert.Visible != value)
					_buttonConvert.Visible = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public DataGridView Grid
		{
			get { return _grid; }
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public ToolStripButton AddButton
		{
			get { return _buttonAddFiles; }
		}

		/// ------------------------------------------------------------------------------------
		void HandleFileGridClientSizeChanged(object sender, EventArgs e)
		{
			// This will recenter the grid's hint message.
			if (_grid.RowCount == 1)
				_grid.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		void HandleFileGridPaint(object sender, PaintEventArgs e)
		{
			if (_grid.RowCount != 1)
				return;

			var rcRow = _grid.GetRowDisplayRectangle(0, false);
			var rc = _grid.ClientRectangle;
			rc.Height -= (rcRow.Height + _grid.ColumnHeadersHeight);
			rc.Y += rcRow.Bottom;

			// Strangely, the grid's client size doesn't change when the scroll bars
			// are visible. Therefore, we have to explicitly allow for them.
			var hscroll = _grid.HScrollBar;
			if (hscroll != null && hscroll.Visible)
				rc.Height -= hscroll.Height;

			var hint = "Add additional files related to this event by\n" +
				"dragging them here or clicking the 'Add' button.";

			const TextFormatFlags flags =
				TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

			using (var fnt = new Font(_grid.Font.FontFamily, 12, FontStyle.Regular))
				TextRenderer.DrawText(e.Graphics, hint, fnt, rc, SystemColors.GrayText, flags);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFileGridCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (e.RowIndex != _grid.CurrentCellAddress.Y)
					SelectComponent(e.RowIndex);

				if (ShowContextMenu)
				{
					var file = _files.ElementAt(e.RowIndex);
					_contextMenuStrip.Items.Clear();
					_contextMenuStrip.Items.AddRange(file.GetMenuCommands(PostMenuCommandRefreshAction).ToArray());
					_contextMenuStrip.Show(MousePosition);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFileGridCellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			// Checking for greater-than-or-equal to zero, throws out double-clicks
			// on the column headings, in which case the row index is -1.
			if (e.RowIndex >= 0)
			{
				_grid.CurrentCell = _grid[e.ColumnIndex, e.RowIndex];
				var file = _files.ElementAt(e.RowIndex);
				file.HandleDoubleClick();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleFileGridCurrentRowChanged(object sender, EventArgs e)
		{
			if (Disposing)
				return;

			BuildMenuCommands(_grid.CurrentCellAddress.Y);

			if (null != AfterComponentSelected)
				AfterComponentSelected(_grid.CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleFileGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			var dataPropName = _grid.Columns[e.ColumnIndex].DataPropertyName;
			var currElementFile = _files.ElementAt(e.RowIndex);

			e.Value = (currElementFile == null ? null :
				ReflectionHelper.GetProperty(currElementFile, dataPropName));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Highlights the component file at the specified index. If the index is out of
		/// range, then it is ignored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SelectComponent(int index)
		{
			_grid.CurrentCell = (index >= 0 && index < _files.Count() ? _grid[0, index] : null);
			BuildMenuCommands(index);
		}

		/// ------------------------------------------------------------------------------------
		private void BuildMenuCommands(int index)
		{
			_buttonOpen.DropDown.Items.Clear();
			_buttonConvert.DropDown.Items.Clear();
			_buttonRename.DropDown.Items.Clear();

			var file = (index >= 0 && index < _files.Count() ? _files.ElementAt(index) : null);

			if (file != null)
			{
				foreach (var item in file.GetMenuCommands(PostMenuCommandRefreshAction))
				{
					switch (item.Tag as string)
					{
						case "open": _buttonOpen.DropDown.Items.Add(item); break;
						case "rename": _buttonRename.DropDown.Items.Add(item); break;
						case "convert": _buttonConvert.DropDown.Items.Add(item); break;
					}
				}
			}

			_buttonOpen.Enabled = (_buttonOpen.DropDown.Items.Count > 0);
			_buttonConvert.Enabled = (_buttonConvert.DropDown.Items.Count > 0);
			_buttonRename.Enabled = (_buttonRename.DropDown.Items.Count > 0);
		}

		/// ------------------------------------------------------------------------------------
		public void TrySetComponent(string file)
		{
			file = Path.GetFileName(file);
			int i = 0;
			foreach (var f in _files)
			{
				if (Path.GetFileName(f.PathToAnnotatedFile) == file)
				{
					SelectComponent(i);
					return;
				}

				i++;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateComponentFileList(IEnumerable<ComponentFile> componentFiles)
		{
			var currFile = (_grid.CurrentCellAddress.Y >= 0 && _files.Count() > 0 ?
				_files.ElementAt(_grid.CurrentCellAddress.Y).PathToAnnotatedFile : null);

			_files = componentFiles;

			// I (David) think there's a bug in the grid that fires the cell value needed
			// event in the process of changing the row count but it fires it for rows that
			// are no longer supposed to exist. This tends to happen when the row count was
			// previously higher than the new value.

			_grid.CellValueNeeded -= HandleFileGridCellValueNeeded;
			_grid.RowCount = componentFiles.Count();
			_grid.CellValueNeeded += HandleFileGridCellValueNeeded;
			_grid.Invalidate();

			if (currFile == null)
				return;

			// Try to select the same file that was selected before updating the list.
			int i = 0;
			foreach (var file in _files)
			{
				if (file.PathToAnnotatedFile == currFile)
				{
					_grid.CurrentCell = _grid[0, i];
					break;
				}

				i++;
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleFileGridDragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;

			if (e.Data.GetFormats().Contains(DataFormats.FileDrop) && FilesBeingDraggedOverGrid != null)
				e.Effect = FilesBeingDraggedOverGrid(e.Data.GetData(DataFormats.FileDrop) as string[]);
		}

		/// ------------------------------------------------------------------------------------
		void HandleFileGridDragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetFormats().Contains(DataFormats.FileDrop) && FilesDroppedOnGrid != null)
				FilesDroppedOnGrid(e.Data.GetData(DataFormats.FileDrop) as string[]);
		}

		/// ------------------------------------------------------------------------------------
		void HandleAddButtonClick(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				dlg.Title = LocalizationManager.LocalizeString(
					"ComponentFileGrid.AddFilesDlgCaption", "Add Files");

				var prjFilterText = LocalizationManager.LocalizeString(
					"ComponentFileGrid.AddFilesFileType", "All Files (*.*)");

				var folder = Settings.Default.LastFolderForComponentFileAdd;
				if (folder == null || !Directory.Exists(folder))
					folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

				dlg.Filter = prjFilterText + "|*.*";
				dlg.InitialDirectory = folder;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				if (dlg.ShowDialog(this) == DialogResult.OK && FilesAdded != null)
					FilesAdded(dlg.FileNames);

				if (!string.IsNullOrEmpty(dlg.FileName))
					Settings.Default.LastFolderForComponentFileAdd = Path.GetDirectoryName(dlg.FileName);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw a line between the buttons and the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _toolStripActions_Paint(object sender, PaintEventArgs e)
		{
			var dy = _toolStripActions.ClientSize.Height - _toolStripActions.Padding.Bottom - 1;

			using (var pen = new Pen(_panelOuter.BorderColor))
				e.Graphics.DrawLine(pen, 0, dy, _toolStripActions.ClientSize.Width, dy);
		}
	}
}
