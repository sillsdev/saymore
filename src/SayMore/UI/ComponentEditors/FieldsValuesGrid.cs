using System.Drawing;
using System.Media;
using System.Windows.Forms;
using SIL.Localization;
using SilUtils;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public class FieldsValuesGrid : SilGrid
	{
		private readonly FieldsValuesGridViewModel _model;
		private readonly Font _defaultFieldFont;

		/// ------------------------------------------------------------------------------------
		public FieldsValuesGrid(FieldsValuesGridViewModel model)
		{
			VirtualMode = true;
			Font = SystemFonts.IconTitleFont;
			_defaultFieldFont = new Font(Font, FontStyle.Bold);
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

			_model = model;

			// Add one for new row.
			RowCount = _model.RowData.Count + 1;

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
				"FieldsAndValuesGrid.FieldColumnHdg", "Field", "Views");

			col = CreateTextBoxColumn("Value");
			col.Width = 175;
			Columns.Add(col);
			LocalizationManager.LocalizeObject(Columns["Value"],
				"FieldsAndValuesGrid.ValueColumnHdg", "Value", "Views");
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
		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);
			AdjustHeight();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex < NewRowIndex && e.ColumnIndex == 0 && !_model.IsIndexForCustomField(e.RowIndex))
			{
				e.CellStyle.Font = _defaultFieldFont;
				this[e.ColumnIndex, e.RowIndex].ReadOnly = true;
			}

			base.OnCellFormatting(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
		{
			base.OnEditingControlShowing(e);

			if (CurrentCellAddress.X == 1)
			{
				var txtBox = e.Control as TextBox;

				// TODO: Hook up real lists.
				txtBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
				txtBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
				var list = new AutoCompleteStringCollection();
				list.AddRange(new[] { "Dingos", "Parrots", "Dogs", "Pigs", "Poultry", "Ducks" });
				txtBox.AutoCompleteCustomSource = list;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
		{
			e.Value = null;

			if (e.RowIndex != NewRowIndex && e.RowIndex < _model.RowData.Count)
			{
				e.Value = e.ColumnIndex == 0 ?
					_model.GetIdForIndex(e.RowIndex) : _model.GetValueForIndex(e.RowIndex);
			}

			base.OnCellValueNeeded(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValuePushed(DataGridViewCellValueEventArgs e)
		{
			if (e.ColumnIndex == 0)
				_model.SetIdForIndex(e.Value as string, e.RowIndex);
			else
				_model.SetValueForIndex(e.Value as string, e.RowIndex);

			base.OnCellValuePushed(e);
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

		/// ------------------------------------------------------------------------------------
		protected override void OnRowValidated(DataGridViewCellEventArgs e)
		{
			base.OnRowValidated(e);

			if (NewRowIndex != e.RowIndex)
				_model.SaveFieldForIndex(e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnRowValidating(DataGridViewCellCancelEventArgs e)
		{
			if (e.RowIndex < NewRowIndex && string.IsNullOrEmpty(_model.GetIdForIndex(e.RowIndex)))
			{
				Utils.MsgBox("You must enter a field name.");
				e.Cancel = true;
			}

			base.OnRowValidating(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnUserDeletingRow(DataGridViewRowCancelEventArgs e)
		{
			if (_model.IsIndexForCustomField(e.Row.Index))
				_model.RemoveFieldForIndex(e.Row.Index);
			else
			{
				e.Cancel = true;
				SystemSounds.Beep.Play();
			}

			base.OnUserDeletingRow(e);
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
