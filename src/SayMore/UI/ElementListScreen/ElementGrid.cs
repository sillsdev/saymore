using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model;
using SayMore.Model.Files;
using SilUtils;

namespace SayMore.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ElementGrid : SilGrid
	{
		public delegate void SelectedElementChangedHandler(object sender,
			ProjectElement oldElement, ProjectElement newElement);

		public event SelectedElementChangedHandler SelectedElementChanged;

		protected IEnumerable<ProjectElement> _items = new ProjectElement[] { };

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
			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

			var clr = ColorHelper.CalculateColor(Color.White,
				 DefaultCellStyle.SelectionBackColor, 140);

			FullRowFocusRectangleColor = DefaultCellStyle.SelectionBackColor;
			DefaultCellStyle.SelectionBackColor = clr;
			DefaultCellStyle.SelectionForeColor = DefaultCellStyle.ForeColor;
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
		public void SetFileType(FileType type)
		{
			Columns.Clear();

			foreach (var col in type.GetFieldsShownInGrid())
			{
				col.HeaderText = col.Name;
				Columns.Add(col);
			}

			if (!DesignMode && GridSettings != null)
				GridSettings.InitializeGrid(this);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ProjectElement> Items
		{
			get { return _items; }
			set
			{
				_items = (value ?? new ProjectElement[] { });
				RowCount = _items.Count();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SelectElement(int index)
		{
			if (index < 0 || index >= RowCount)
			{
				var msg = string.Format(
					"{0} must be greater than or equal to 0 and less than {1}.", index, RowCount);
				throw new IndexOutOfRangeException(msg);
			}

			foreach (DataGridViewRow row in Rows)
				row.Selected = false;

			if (index >= 0 && index < _items.Count())
			{
				CurrentCell = this[0, index];
				Rows[index].Selected = true;
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
				var msg = string.Format("'{0}' doesn't exist in elements collection.", element.Id);
				throw new ArgumentException(msg);
			}
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
