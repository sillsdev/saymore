using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using Localization;
using Localization.UI;
using SayMore.Properties;
using SayMore.UI.LowLevelControls;
using SilTools;
using ColorHelper = SilTools.ColorHelper;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public class FieldsValuesGrid : SilGrid
	{
		private readonly FieldsValuesGridViewModel _model;
		private readonly Font _factoryFieldFont;
		private readonly Color _focusedSelectionBackColor;
		private bool _adjustHeightToFitRows = true;
		private readonly LocalizationExtender _locExtender;

		/// ------------------------------------------------------------------------------------
		public FieldsValuesGrid(FieldsValuesGridViewModel model)
		{
			_locExtender = new LocalizationExtender();
			_locExtender.LocalizationManagerId = "SayMore";
			_locExtender.SetLocalizingId(this, "FieldsAndValuesGrid");

			VirtualMode = true;
			Font = Program.DialogFont;
			_factoryFieldFont = new Font(Font, FontStyle.Bold);
			AllowUserToAddRows = true;
			AllowUserToDeleteRows = true;
			MultiSelect = false;
			Margin = new Padding(0, Margin.Top, 0, Margin.Bottom);
			RowHeadersVisible = false;
			DefaultCellStyle.SelectionForeColor = DefaultCellStyle.ForeColor;

			_focusedSelectionBackColor = ColorHelper.CalculateColor(Color.White,
				DefaultCellStyle.SelectionBackColor, 140);

			SetSelectionColors(false);

			AddColumns();

			_model = model;

			// Add one for new row.
			RowCount = _model.RowData.Count + 1;

			AutoResizeRows();

			_model.ComponentFileChanged = HandleComponentFileChanged;

			if (!string.IsNullOrEmpty(_model.GridSettingsName) &&
				Settings.Default[_model.GridSettingsName] != null)
			{
				((GridSettings)Settings.Default[_model.GridSettingsName]).InitializeGrid(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void SetSelectionColors(bool hasFocus)
		{
			// The reason the Focused property is not used is because when this method is
			// called in the Validated event (which is also true of the Leave and LostFocus
			// events) the Focused property is still true. Argh!
			DefaultCellStyle.SelectionBackColor = (hasFocus ?
				_focusedSelectionBackColor : BackgroundColor);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);

			// In addition to getting this event when coming from a control outside of the
			// grid, we'll also get this event when a cell goes out of the editing mode and
			// it's editing control loses focus to the grid itself. So if we're here because
			// one of the cell's is coming out of edit mode, then we don't need to do anything.
			if (EditingControl != null)
				return;

			SetSelectionColors(true);
			CurrentCell = (_model.GetIdForIndex(0) == null ? this[0, 0] : this[1, 0]);

			// This prevents the grid from stealing focus at startup when it shouldn't.
			// The problem arises in the following way: The OnCellFormatting gets called,
			// even when the grid does not have focus. In the CellFormatting event, cells
			// are made readonly or not. When the edit mode is EditOnEnter, setting a
			// cell's readonly property to false (I think) will cause the cell to go into
			// edit mode. When it goes into edit mode, the cell editor gets displayed and
			// if the grid does not have focus, it thinks it should, because the editor
			// just got shown. Therefore, the grid will steal the focus from another
			// control at startup.
			if (EditMode != DataGridViewEditMode.EditOnEnter)
				EditMode = DataGridViewEditMode.EditOnEnter;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnValidated(EventArgs e)
		{
			base.OnValidated(e);
			SetSelectionColors(false);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleComponentFileChanged()
		{
			var saveAdjustHeightToFitRows = _adjustHeightToFitRows;
			_adjustHeightToFitRows = false;
			RowCount = _model.RowData.Count + 1;
			CurrentCell = this[0, 0];
			_adjustHeightToFitRows = saveAdjustHeightToFitRows;

			if (_adjustHeightToFitRows)
				AdjustHeight();

			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void AddColumns()
		{
			var col = CreateTextBoxColumn("colField");
			col.HeaderText = "_L10N_:CommonToMultipleViews.FieldsAndValuesGrid.ColumnHeadings.Field!Field";
			col.Width = 125;
			Columns.Add(col);

			col = CreateTextBoxColumn("colValue");
			col.HeaderText = "_L10N_:CommonToMultipleViews.FieldsAndValuesGrid.ColumnHeadings.Value!Value";
			col.Width = 175;
			Columns.Add(col);

			_locExtender.EndInit();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnDockChanged(EventArgs e)
		{
			base.OnDockChanged(e);
			_adjustHeightToFitRows = (Dock != DockStyle.Fill && Dock != DockStyle.None);
		}

		/// ------------------------------------------------------------------------------------
		private void AdjustHeight()
		{
			if (_adjustHeightToFitRows && (Anchor & AnchorStyles.Bottom) != AnchorStyles.Bottom &&
				IsHandleCreated && !Disposing && RowCount > 0)
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
			// REVIEW: For some reason, when the grid does not have focus, sometimes
			// setting a cell's readonly property to true gives the grid focus.

			if (_model == null)
			{
				base.OnCellFormatting(e);
				return;
			}

			var isReadOnly = _model.IsIndexForReadOnlyField(e.RowIndex);
			var isCustom = _model.IsIndexForCustomField(e.RowIndex);

			if (string.IsNullOrEmpty(_model.GetIdForIndex(e.RowIndex)))
				this[1, e.RowIndex].ReadOnly = true;
			else if (e.RowIndex < NewRowIndex)
			{
				if (e.ColumnIndex == 0)
				{
					this[0, e.RowIndex].ReadOnly = !isCustom;
					if (!isCustom)
						e.CellStyle.Font = _factoryFieldFont;
				}
				else
				{
					this[1, e.RowIndex].ReadOnly = isReadOnly;
					if (isReadOnly)
						this[1, e.RowIndex].Style.ForeColor = Color.Gray;
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
				txtBox.AutoCompleteMode = AutoCompleteMode.None;
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
		protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
		{
			base.OnColumnWidthChanged(e);

			if (!string.IsNullOrEmpty(_model.GridSettingsName))
				Settings.Default[_model.GridSettingsName] = GridSettings.Create(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			// Gets rid of the annoying beep caused by pressing ESC when a cell is in
			// the edit mode. It also takes the user out of edit mode.
			if (keyData == Keys.Escape && IsCurrentCellInEditMode)
			{
				CancelEdit();
				return true;
			}

			// When the field name is not blank, cause tab and shift+tab to move
			// from value to value, rather than passing through the field name cells.
			if (CurrentCellAddress.X == 1 && msg.WParam.ToInt32() == (int)Keys.Tab)
			{
				var newRowIndex = -1;
				var skipFieldName = true;

				if ((keyData & Keys.Shift) == 0 && CurrentCellAddress.Y < NewRowIndex)
				{
					if (IsCurrentCellInEditMode)
						EndEdit();

					newRowIndex = CurrentCellAddress.Y + 1;
					skipFieldName = !string.IsNullOrEmpty(this[0, newRowIndex].Value as string);
				}
				else if ((keyData & Keys.Shift) > 0 && CurrentCellAddress.Y > 0)
				{
					if (IsCurrentCellInEditMode)
						EndEdit();

					newRowIndex = CurrentCellAddress.Y - 1;
					skipFieldName = !string.IsNullOrEmpty(this[0, CurrentCellAddress.Y].Value as string);
				}

				if (newRowIndex >= 0 && skipFieldName)
				{
					CurrentCell = this[1, newRowIndex];
					return true;
				}
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
					_model.GetDisplayableFieldName(e.RowIndex) :
					_model.GetValueForIndex(e.RowIndex);
			}

			base.OnCellValueNeeded(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValuePushed(DataGridViewCellValueEventArgs e)
		{
			if (e.ColumnIndex == 1)
				_model.SaveValueForIndex(e.RowIndex, e.Value as string);
			else
			{
				string oldId;
				if (!_model.GetShouldAskToRemoveFieldEverywhere(e.RowIndex, e.Value as string, out oldId))
					_model.SaveIdForIndex(e.RowIndex, e.Value as string);
				else
				{
					if (AskUserToVerifyRemovingFieldEverywhere(oldId))
					{
						_model.RemoveFieldFromEntireProject(e.RowIndex);
						RowCount--;
						Invalidate();
					}
				}
			}

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

		///// ------------------------------------------------------------------------------------
		//protected override void OnRowValidating(DataGridViewCellCancelEventArgs e)
		//{
		//    var fieldId = _model.GetIdForIndex(e.RowIndex);
		//    var fieldValue = _model.GetValueForIndex(e.RowIndex);

		//    if (e.RowIndex < NewRowIndex && string.IsNullOrEmpty(fieldId) && !string.IsNullOrEmpty(fieldValue))
		//    {
		//        Utils.MsgBox("You must enter a field name.");
		//        e.Cancel = true;
		//    }

		//    base.OnRowValidating(e);
		//}

		///// ------------------------------------------------------------------------------------
		//protected override void OnUserDeletingRow(DataGridViewRowCancelEventArgs e)
		//{
		//    int indexOfRowToDelete;

		//    if (_model.CanDeleteRow(e.Row.Index, out indexOfRowToDelete))
		//    {
		//        if (indexOfRowToDelete >= 0)
		//        {
		//            var idToDelete = e.Row.Cells[0].Value as string;
		//            if (AskUserToVerifyRemovingFieldEverywhere(idToDelete))
		//                _model.RemoveFieldFromEntireProject(indexOfRowToDelete);
		//            else
		//            {
		//                e.Cancel = true;
		//                SystemSounds.Beep.Play();
		//            }
		//        }
		//    }
		//    else
		//    {
		//        e.Cancel = true;
		//        SystemSounds.Beep.Play();
		//    }

		//    base.OnUserDeletingRow(e);
		//}

		/// ------------------------------------------------------------------------------------
		private static bool AskUserToVerifyRemovingFieldEverywhere(string id)
		{
			var msg = LocalizationManager.GetString("CommonToMultipleViews.FieldsAndValuesGrid.VerifyDeleteFieldQuestion",
				"Do you want to delete the field '{0}' and its contents from the entire project?");

			using (var dlg = new DeleteMessageBox(string.Format(msg, id)))
				return (dlg.ShowDialog() == DialogResult.OK);
		}
	}
}
