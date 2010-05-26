using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SilUtils;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.UI.ElementListScreen
{
	public partial class ComponentFileGrid : SilUtils.Controls.SilPanel
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
			_grid.CurrentCell = _grid[e.ColumnIndex, e.RowIndex];
			var file = _files.ElementAt(e.RowIndex);
			file.HandleDoubleClick();
		}

		/// ------------------------------------------------------------------------------------
		void HandleFileGridCurrentRowChanged(object sender, EventArgs e)
		{
			if (null != ComponentSelectedCallback)
				ComponentSelectedCallback(_grid.CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		protected void HandleFileGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			var dataPropName = _grid.Columns[e.ColumnIndex].DataPropertyName;
			var currElementFile = _files.ToList().ElementAt(e.RowIndex);

			e.Value = (currElementFile == null ? null :
				ReflectionHelper.GetProperty(currElementFile, dataPropName));
		}

		/// ------------------------------------------------------------------------------------
		void HandleFileGridDragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;

			if (!e.Data.GetFormats().Contains(DataFormats.FileDrop))
				return;

			if (FilesBeingDraggedOverGrid != null)
				e.Effect = FilesBeingDraggedOverGrid(e.Data.GetData(DataFormats.FileDrop) as string[]);
		}

		/// ------------------------------------------------------------------------------------
		void HandleFileGridDragDrop(object sender, DragEventArgs e)
		{
			if (!e.Data.GetFormats().Contains(DataFormats.FileDrop) || FilesDroppedOnGrid == null)
				return;

			if (FilesDroppedOnGrid(e.Data.GetData(DataFormats.FileDrop) as string[]))
			{
				// Refresh the grid.
			}
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateComponentList(IEnumerable<ComponentFile> componentFiles)
		{
			_files = componentFiles;

			// I (David) think there's a bug in the grid that fires the cell value needed
			// event in the process of changing the row count but it fires it for rows that
			// are no longer supposed to exist. This tends to happen when the row count was
			// previously higher than the new value.

			_grid.CellValueNeeded -= HandleFileGridCellValueNeeded;
			_grid.RowCount = componentFiles.Count();
			_grid.CellValueNeeded += HandleFileGridCellValueNeeded;
			_grid.Invalidate();
		}

	}
}
