using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SilUtils;
using Sponge2.Model.Files;

namespace Sponge2.UI.ElementListScreen
{
	public partial class ComponentFileGrid : UserControl
	{
		private IEnumerable<ComponentFile> _files;

		/// <summary>
		/// When the user selects a different component, this is called
		/// </summary>
		public Action<int> ComponentSelectedCallback;

		public ComponentFileGrid()
		{
			InitializeComponent();

			_grid.CellValueNeeded += HandleFileGridCellValueNeeded;
			_grid.RowEnter += HandleFileGridRowEnter;
			_grid.Font = SystemFonts.IconTitleFont;

		}

		/// ------------------------------------------------------------------------------------
		protected void HandleFileGridRowEnter(object sender, DataGridViewCellEventArgs e)
		{
			// This event is fired even when the grid gains focus without the row actually
			// changing, therefore we should just ignore the event when the row hasn't changed.
			if (e.RowIndex != _grid.CurrentCellAddress.Y)
			{
				//Model.SetSelectedComponentFile(e.RowIndex);
				if(null!=ComponentSelectedCallback)
				{
					ComponentSelectedCallback(e.RowIndex);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		protected void HandleFileGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			var dataPropName = _grid.Columns[e.ColumnIndex].DataPropertyName;
			var currElementFile = _files.ToList().ElementAt(e.RowIndex);

			e.Value = (currElementFile == null ? null :
				ReflectionHelper.GetProperty(currElementFile, dataPropName));
		}


		public void UpdateComponentList(IEnumerable<ComponentFile> componentFiles)
		{
			_files = componentFiles;
			// I (David) think there's a bug in the grid that fires the cell value needed event in
			// the process of changing the row count but it fires it for rows that are
			// no longer supposed to exist. This tends to happen when the row count was
			// previously higher than the new value.

			_grid.CellValueNeeded -= HandleFileGridCellValueNeeded;
			_grid.RowCount = componentFiles.Count();
			_grid.CellValueNeeded += HandleFileGridCellValueNeeded;
			_grid.Invalidate();
		}
	}
}
