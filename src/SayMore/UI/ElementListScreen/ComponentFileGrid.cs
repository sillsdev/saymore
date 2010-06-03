using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SIL.Localization;
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
		/// John: this is really just an event. Is there any reason not to name it like
		/// that? i.e. Does "Callback" make it more understandable? It's not very .Net-like.
		public Action<int> ComponentSelectedCallback;

		public Func<string[], DragDropEffects> FilesBeingDraggedOverGrid;
		public Func<string[], bool> FilesDroppedOnGrid;
		public Func<string[], bool> FilesAdded;

		/// ------------------------------------------------------------------------------------
		public ComponentFileGrid()
		{
			InitializeComponent();

			try
			{
				// Sigh. Setting AllowDrop when tests are running throws an exception and
				// I'm not quite sure why, even after using reflector to look at the
				// code behind setting the property. Nonetheless, I've seen this before
				// so I'm just going to ignore any exception thrown when enabling drag
				// and drop. The worst that could happen is that the end user will not
				// have that feature.
				_grid.AllowDrop = true;
			}
			catch { }

			_grid.DragEnter += HandleFileGridDragEnter;
			_grid.DragDrop += HandleFileGridDragDrop;
			_grid.CellMouseClick += HandleMouseClick;
			_grid.CellValueNeeded += HandleFileGridCellValueNeeded;
			_grid.CellDoubleClick += HandleFileGridCellDoubleClick;
			_grid.CurrentRowChanged += HandleFileGridCurrentRowChanged;
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
			Settings.Default[_gridColSettingPrefix + "ComponentGrid"] = GridSettings.Create(_grid);
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		public bool AddButtonVisible
		{
			get { return _buttonAdd.Visible; }
			set
			{
				if (_buttonAdd.Visible != value)
					_buttonAdd.Visible = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public DataGridView Grid
		{
			get { return _grid; }
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				_grid.CurrentCell = _grid[e.ColumnIndex, e.RowIndex];

				Point pt = _grid.PointToClient(MousePosition);
				var file = _files.ElementAt(e.RowIndex);
				_contextMenuStrip.Items.Clear();
				_contextMenuStrip.Items.AddRange(file.GetContextMenuItems(_grid.Invalidate).ToArray());
				_contextMenuStrip.Show(_grid, pt);
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
			if (null != ComponentSelectedCallback)
				ComponentSelectedCallback(_grid.CurrentCellAddress.Y);
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
		public void UpdateComponentList(IEnumerable<ComponentFile> componentFiles)
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
		/// Draw a line between the add button and the grid. Use the same color as the grid
		/// lines. Then fill the area below that line with a gradient fill.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePaintingGridButtonSeparatorLine(object sender, PaintEventArgs e)
		{
			if (!_buttonAdd.Visible)
				return;

			// Get the Y coordinate of the bottom of the grid, relative to the outer panel.
			var pt = _tableLayout.PointToScreen(new Point(0, _grid.Bottom));
			var dy = _panelOuter.PointToClient(pt).Y;

			using (var pen = new Pen(_grid.GridColor))
				e.Graphics.DrawLine(pen, 0, dy, _panelOuter.Width, dy);

			var rc = new Rectangle(0, dy + 1, _panelOuter.Width, _panelOuter.ClientSize.Height - dy - 1);
			var clr1 = Color.FromArgb(95, _panelOuter.BorderColor);
			var clr2 = Color.FromArgb(40, _panelOuter.BorderColor);

			using (var br = new LinearGradientBrush(rc, clr1, clr2, LinearGradientMode.Horizontal))
				e.Graphics.FillRectangle(br, rc);
		}
	}
}
