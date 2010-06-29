using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
			Margin = new Padding(0, Margin.Top, 0, Margin.Bottom);
			DefaultCellStyle.SelectionForeColor = DefaultCellStyle.ForeColor;
			DefaultCellStyle.SelectionBackColor = ColorHelper.CalculateColor(Color.White,
				 DefaultCellStyle.SelectionBackColor, 140);

			AddColumns();

			_model = model;

			// Add one for new row.
			RowCount = _model.RowData.Count + 1;

			AutoResizeRows();

			_model.ComponentFileChanged = new Action(() =>
			{
				RowCount = _model.RowData.Count + 1;
				Invalidate();
				CurrentCell = this[0, 0];
			});
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
			if (Dock != DockStyle.Fill && Dock != DockStyle.None && IsHandleCreated &&
				!Disposing && RowCount > 0)
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
			if (e.RowIndex < NewRowIndex && e.ColumnIndex == 0)
			{
				var val = _model.GetIdForIndex(e.RowIndex);
				e.Value = val.Replace('_', ' ');

				if (!_model.IsIndexForCustomField(e.RowIndex))
				{
					e.CellStyle.Font = _defaultFieldFont;
					this[e.ColumnIndex, e.RowIndex].ReadOnly = true;
				}
			}

			base.OnCellFormatting(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
		{
			base.OnEditingControlShowing(e);

			var txtBox = e.Control as TextBox;

			if (CurrentCellAddress.X == 0)
			{
				txtBox.KeyPress += HandleCellEditBoxKeyPress;
				txtBox.HandleDestroyed += HandleCellEditBoxHandleDestroyed;
			}
			else
			{
				txtBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
				txtBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
				txtBox.AutoCompleteCustomSource = _model.GetAutoCompleteListForIndex(CurrentCellAddress.Y);
			}
		}

		/// ------------------------------------------------------------------------------------
		private static void HandleCellEditBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			// Prevent characters that are invalid as xml tags. There's probably more,
			// but this will do for now.
			if ("<>{}()[]/'\"\\.,;:?|!@#$%^&*=+`~".IndexOf(e.KeyChar) >= 0)
			{
				e.KeyChar = (char)0;
				e.Handled = true;
				SystemSounds.Beep.Play();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This gets rid of the annoying beep caused by pressing ESC when a cell is in
		/// the edit mode. It rightly takes the user out of edit mode, but without this code,
		/// it beeps too.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape && IsCurrentCellInEditMode)
			{
				EndEdit();
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// ------------------------------------------------------------------------------------
		private static void HandleCellEditBoxHandleDestroyed(object sender, EventArgs e)
		{
			((TextBox)sender).KeyPress -= HandleCellEditBoxKeyPress;
			((TextBox)sender).HandleDestroyed -= HandleCellEditBoxHandleDestroyed;
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
