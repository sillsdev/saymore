using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Localization;
using Localization.UI;
using SayMore.Model;
using SayMore.Model.Files;
using SilTools;
using ColorHelper = SilTools.ColorHelper;

namespace SayMore.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	public class ElementGrid : LmGrid
	{
		public delegate void SelectedElementChangedHandler(object sender,
			ProjectElement oldElement, ProjectElement newElement);

		public event SelectedElementChangedHandler SelectedElementChanged;

		public Func<bool> IsOKToSelectDifferentElement;
		public Action DeleteAction;

		protected FileType _fileType;
		protected IEnumerable<ProjectElement> _items = new ProjectElement[] { };
		protected ContextMenuStrip _contextMenuStrip = new ContextMenuStrip();
		protected readonly LocalizationExtender _locExtender;

		/// ------------------------------------------------------------------------------------
		public ElementGrid()
		{
			CellBorderStyle = DataGridViewCellBorderStyle.None;
			VirtualMode = true;
			RowHeadersVisible = false;
			BorderStyle = BorderStyle.None;
			StandardTab = true;
			Font = SystemFonts.IconTitleFont;
			MultiSelect = true;
			PaintFullRowFocusRectangle = true;
			ExtendFullRowSelectRectangleToEdge = true;
			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

			var clr = ColorHelper.CalculateColor(Color.White,
				 DefaultCellStyle.SelectionBackColor, 140);

			FullRowFocusRectangleColor = DefaultCellStyle.SelectionBackColor;
			DefaultCellStyle.SelectionBackColor = clr;
			DefaultCellStyle.SelectionForeColor = DefaultCellStyle.ForeColor;

			_locExtender = new LocalizationExtender();
			_locExtender.LocalizationManagerId = "SayMore";
			_locExtender.SetLocalizingId(this, "ElementGrid");
		}

		/// ------------------------------------------------------------------------------------
		public virtual GridSettings GridSettings
		{
			get { return null; }
			set {  }
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (!DesignMode)
				GridSettings = GridSettings.Create(this);

			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		public void SetFileType(FileType fileType)
		{
			_fileType = fileType;

			Columns.Clear();

			foreach (var col in _fileType.GetFieldsShownInGrid())
			{
				if (string.IsNullOrEmpty(col.HeaderText))
					col.HeaderText = col.Name;

				Columns.Add(col);
			}

			_locExtender.EndInit();

			if (!DesignMode && GridSettings != null)
				GridSettings.InitializeGrid(this);

			AutoResizeColumnHeadersHeight();
			ColumnHeadersHeight += 8;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ProjectElement> Items
		{
			get { return _items; }
			set
			{
				_items = (value != null ? value.ToList() : new List<ProjectElement>(0));
				RowCount = _items.Count();

				if (!DesignMode && GridSettings != null)
					Sort(GridSettings.SortedColumn, GridSettings.SortOrder);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SelectElement(int index)
		{
			if (index < 0 || index >= RowCount)
			{
				var msg = string.Format("{0} must be greater than or equal to 0 and less than {1}.", index, RowCount);
				throw new IndexOutOfRangeException(msg);
			}

			foreach (DataGridViewRow row in Rows)
				row.Selected = false;

			if (index >= 0 && index < _items.Count())
			{
				var forceRowChangeEvent = (CurrentCellAddress.Y == index);
				CurrentCell = this[FirstDisplayedCell.ColumnIndex, index];
				Rows[index].Selected = true;
				if (forceRowChangeEvent)
					OnCurrentRowChanged(EventArgs.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SelectElement(ProjectElement element)
		{
			if (element == null)
				return;

			int i = 0;
			foreach (var pe in _items)
			{
				if (element == pe)
				{
					SelectElement(i);
					return;
				}

				i++;
			}

			if (i == _items.Count())
			{
				var msg = LocalizationManager.GetString("MainWindow.ElementNoLongerExistsMsg",
					"'{0}' doesn't exist in elements collection.");

				throw new ArgumentException(string.Format(msg, element.Id));
			}
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshCurrentRow()
		{
			InvalidateRowInFullRowSelectMode(CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		public void SelectElement(string elementId)
		{
			SelectElement(_items.FirstOrDefault(x => x.Id == elementId));
		}

		/// ------------------------------------------------------------------------------------
		public ProjectElement GetCurrentElement()
		{
			int rowIndex = CurrentCellAddress.Y;

			return (rowIndex >= 0 && rowIndex < _items.Count() ?
				_items.ElementAt(rowIndex) : null);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ProjectElement> GetSelectedElements()
		{
			return from row in SelectedRows.Cast<DataGridViewRow>()
				   select _items.ElementAt(row.Index);
		}

		/// ------------------------------------------------------------------------------------
		public void Sort(int colIndex)
		{
			Sort(Columns.Cast<DataGridViewColumn>().FirstOrDefault(c => c.Index == colIndex));
		}

		/// ------------------------------------------------------------------------------------
		public void Sort(string colName)
		{
			Sort(Columns.Cast<DataGridViewColumn>().FirstOrDefault(c => c.Name == colName));
		}

		/// ------------------------------------------------------------------------------------
		public void Sort(string colName, SortOrder direction)
		{
			Sort(Columns.Cast<DataGridViewColumn>().FirstOrDefault(c => c.Name == colName), direction);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sorts the grid by the specified column, toggling the direction if the grid is
		/// already sorted by that column. If not, the sort direction is ascending.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Sort(DataGridViewColumn col)
		{
			if (col != null)
			{
				Sort(col, (col.HeaderCell.SortGlyphDirection != SortOrder.Ascending ?
					SortOrder.Ascending : SortOrder.Descending));
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Sort(DataGridViewColumn col, SortOrder direction)
		{
			if (col == null)
				return;

			var prevElement = GetCurrentElement();
			var prevId = (prevElement == null ? null : prevElement.Id);

			var fieldId = col.DataPropertyName;

			((List<ProjectElement>)_items).Sort(
				new ProjectElementComparer(fieldId, direction, GetSortValueForField));

			foreach (DataGridViewColumn c in Columns)
				c.HeaderCell.SortGlyphDirection = SortOrder.None;

			col.HeaderCell.SortGlyphDirection = (direction == SortOrder.Ascending ?
				SortOrder.Ascending : SortOrder.Descending);

			Refresh();

			if (RowCount == 0)
				return;

			if (prevId == null)
				SelectElement(0);
			else
				SelectElement(prevId);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
		{
			base.OnCellMouseDown(e);

			if (e.Button != MouseButtons.Right || e.RowIndex < 0)
				return;

			if (e.RowIndex != CurrentCellAddress.Y)
				SelectElement(e.RowIndex);

			Select();
			_contextMenuStrip.Items.Clear();
			_contextMenuStrip.Items.AddRange(GetMenuCommands().ToArray());
			_contextMenuStrip.Show(MousePosition);
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ToolStripMenuItem> GetMenuCommands()
		{
			if (DeleteAction == null)
				yield return null;

			var menu = new ToolStripMenuItem(string.Empty, null, (s, e) => DeleteAction());
			menu.Text = LocalizationManager.GetString("MainWindow.DeleteElementMenuText", "Delete", null, menu);
			yield return menu;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left || _fileType != null)
				Sort(e.ColumnIndex);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
		{
			base.OnColumnWidthChanged(e);

			AutoResizeColumnHeadersHeight();
			ColumnHeadersHeight += 8;

			if (!DesignMode)
				GridSettings = GridSettings.Create(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnRowValidating(DataGridViewCellCancelEventArgs e)
		{
			e.Cancel = (IsOKToSelectDifferentElement != null && !IsOKToSelectDifferentElement());
			base.OnRowValidating(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCurrentRowChanged(EventArgs e)
		{
			int itemCount = _items.Count();

			var oldElement = (_prevRowIndex >= 0 && _prevRowIndex < itemCount ?
				_items.ElementAt(_prevRowIndex) : null);

			base.OnCurrentRowChanged(e);

			var newElement = (CurrentCellAddress.Y >= 0 && CurrentCellAddress.Y < itemCount ?
				_items.ElementAt(CurrentCellAddress.Y) : null);

			if (SelectedElementChanged != null)
				SelectedElementChanged(this, oldElement, newElement);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex >= 0 && e.RowIndex < _items.Count())
			{
				var element = _items.ElementAt(e.RowIndex);
				e.Value = GetValueForField(element, Columns[e.ColumnIndex].DataPropertyName);
			}

			base.OnCellValueNeeded(e);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual object GetValueForField(ProjectElement element, string fieldName)
		{
			return element.MetaDataFile.GetStringValue(fieldName, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual object GetSortValueForField(ProjectElement element, string fieldName)
		{
			return (fieldName == "id" ? element.Id : GetValueForField(element, fieldName));
		}

		///// ------------------------------------------------------------------------------------
		//protected override void OnCellValuePushed(DataGridViewCellValueEventArgs e)
		//{
		//    var item = _items.ElementAt(e.RowIndex);
		//    var fieldName = Columns[e.ColumnIndex].DataPropertyName;
		//    string errMsg;
		//    item.MetaDataFile.SetValue(fieldName, e.Value as string, out errMsg);

		//    base.OnCellValuePushed(e);
		//    item.MetaDataFile.Save();
		//}
	}
}
