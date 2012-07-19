using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Model.Fields;
using SIL.Localization;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public class CustomFieldsGrid : BetterGrid
	{
		/// ------------------------------------------------------------------------------------
		public CustomFieldsGrid()
		{
			Font = SystemFonts.IconTitleFont;
			AllowUserToAddRows = true;
			AllowUserToDeleteRows = true;
			MultiSelect = false;
			Height = 100;
			Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
			Margin = new Padding(0, Margin.Top, 0, Margin.Bottom);
			DefaultCellStyle.SelectionForeColor = DefaultCellStyle.ForeColor;
			DefaultCellStyle.SelectionBackColor = ColorHelper.CalculateColor(Color.White,
				 DefaultCellStyle.SelectionBackColor, 140);

			AddColumns();
			AutoResizeRows();
			AdjustHeight();
		}

		/// ------------------------------------------------------------------------------------
		private void AddColumns()
		{
			var col = CreateTextBoxColumn("Field");
			col.Width = 150;
			Columns.Add(col);
			LocalizationManager.LocalizeObject(Columns["Field"],
				"CustomFieldsGrid.FieldColumnHdg", "Field", "Dialog Boxes");

			col = CreateTextBoxColumn("Value");
			col.Width = 175;
			Columns.Add(col);
			LocalizationManager.LocalizeObject(Columns["Value"],
				"CustomFieldsGrid.ValueColumnHdg", "Value", "Dialog Boxes");
		}

		///// ------------------------------------------------------------------------------------
		//public IEnumerable<FieldValue> GetFieldsAndValues()
		//{
		//    return from row in Rows.Cast<DataGridViewRow>()
		//        where row.Index != NewRowIndex
		//           select new FieldValue(row.Cells["Field"].Value as string,
		//               row.Cells["Value"].Value as string);
		//}

		/// ------------------------------------------------------------------------------------
		public FieldValue GetFieldValueForIndex(int index)
		{
			return new FieldValue(this["Field", index].Value as string,
				this["Value", index].Value as string ?? string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public void SetFieldsAndValues(IEnumerable<FieldValue> fieldsAndValues)
		{
			Rows.Clear();

			if (fieldsAndValues == null)
				return;

			foreach (var fav in fieldsAndValues)
				Rows.Add(fav.FieldKey, fav.Value);
		}

		/// ------------------------------------------------------------------------------------
		private void AdjustHeight()
		{
			if (IsHandleCreated && !Disposing && RowCount > 0)
			{
				Height = ColumnHeadersHeight + (RowCount * Rows[0].Height) + 2 +
					(HorizontalScrollBar.Visible ? HorizontalScrollBar.Height : 0);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
		{
			base.OnRowsAdded(e);
			AdjustHeight();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnRowsRemoved(DataGridViewRowsRemovedEventArgs e)
		{
			base.OnRowsRemoved(e);
			AdjustHeight();
		}

		protected override void OnRowValidating(DataGridViewCellCancelEventArgs e)
		{
			// TODO: verify the user has entered a field name.

			base.OnRowValidating(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnUserDeletingRow(DataGridViewRowCancelEventArgs e)
		{
			base.OnUserDeletingRow(e);

			// TODO: verify the user really wants to do this.
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
		{
			base.OnCellEndEdit(e);

			// TODO: Handle case when user edits a Field cell so there's nothing left in
			// the cell and verify the user really wants to do this.
		}
	}
}
