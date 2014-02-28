using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.UI;
using Palaso.Reporting;
using Palaso.UI.WindowsForms.Widgets.BetterGrid;
using SayMore.Properties;
using SayMore.UI.LowLevelControls;
using Palaso.UI.WindowsForms;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public class FieldsValuesGrid : BetterGrid
	{
		private readonly FieldsValuesGridViewModel _model;
		private readonly Font _factoryFieldFont;
		private readonly Color _focusedSelectionBackColor;
		private bool _adjustHeightToFitRows = true;
		private readonly L10NSharpExtender _locExtender;

		/// ------------------------------------------------------------------------------------
		public FieldsValuesGrid(FieldsValuesGridViewModel model, string name)
		{
			Name = name;
			Logger.WriteEvent("Entering FieldValuesGrid Constructor: {0}", Name);
			if (SystemColors.WindowText.IsKnownColor)
				Logger.WriteEvent("    Window Text Color = {0}", SystemColors.WindowText.ToKnownColor());
			Logger.WriteEvent("    Window Text Color ARGB = {0}", FormatColorAsString(SystemColors.WindowText));
			if (SystemColors.Window.IsKnownColor)
				Logger.WriteEvent("    Window Color = {0}", SystemColors.Window.ToKnownColor());
			Logger.WriteEvent("    Window Color ARGB = {0}", FormatColorAsString(SystemColors.Window));
			if (SystemColors.HighlightText.IsKnownColor)
				Logger.WriteEvent("    Highlight Text Color = {0}", SystemColors.HighlightText.ToKnownColor());
			Logger.WriteEvent("    Highlight Text Color ARGB = {0}", FormatColorAsString(SystemColors.HighlightText));
			if (SystemColors.Highlight.IsKnownColor)
				Logger.WriteEvent("    Highlight Color = {0}", SystemColors.Highlight.ToKnownColor());
			Logger.WriteEvent("    Highlight Color ARGB = {0}", FormatColorAsString(SystemColors.Highlight));

			_locExtender = new L10NSharpExtender();
			_locExtender.LocalizationManagerId = "SayMore";
			_locExtender.SetLocalizingId(this, "FieldsAndValuesGrid");

			VirtualMode = true;
			Font = Program.DialogFont;
			_factoryFieldFont = new Font(Font, FontStyle.Bold);

			AllowUserToDeleteRows = true;
			MultiSelect = false;
			Margin = new Padding(0, Margin.Top, 0, Margin.Bottom);
			RowHeadersVisible = false;
			Logger.WriteEvent("  About to set DefaultCellStyle.SelectionForeColor");
			Logger.WriteEvent("    DefaultCellStyle.SelectionForeColor = {0}", DefaultCellStyle.SelectionForeColor);
			Logger.WriteEvent("    DefaultCellStyle.ForeColor = {0}", DefaultCellStyle.ForeColor);
			DefaultCellStyle.SelectionForeColor = DefaultCellStyle.ForeColor;
			Logger.WriteEvent("  Finished setting DefaultCellStyle.SelectionForeColor");
			Logger.WriteEvent("    DefaultCellStyle.SelectionForeColor = {0}", DefaultCellStyle.SelectionForeColor);

			_focusedSelectionBackColor = ColorHelper.CalculateColor(Color.White,
				DefaultCellStyle.SelectionBackColor, 140);

			SetSelectionColors(false);

			_model = model;

			AddColumns();

			RowCount = _model.RowData.Count;

			// setting AllowUserToAddRows=True will add a blank line
			AllowUserToAddRows = _model.AllowUserToAddRows;

			AutoResizeRows();

			_model.ComponentFileChanged = HandleComponentFileChanged;

			if (!string.IsNullOrEmpty(_model.GridSettingsName) &&
				Settings.Default[_model.GridSettingsName] != null)
			{
				((GridSettings)Settings.Default[_model.GridSettingsName]).InitializeGrid(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		private string FormatColorAsString(Color color)
		{
			var argb = color.ToArgb();
			var namedColor = GetNamedColors().FirstOrDefault(c => color.ToArgb() == c.ToArgb());
			string knownColorName = namedColor == default(Color) ? (((uint)argb == 0xFF3399FF) ? "Blue highlight" : "????") : namedColor.Name;

			return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2} ({4})", color.A, color.R, color.G, color.B, knownColorName);
		}

		/// ------------------------------------------------------------------------------------
		static IEnumerable<Color> GetNamedColors()
		{
			Type type = typeof(Color);
			return type.GetProperties().Where(info => info.PropertyType == type).Select(info => (Color)info.GetValue(null, null)).Where(c => !c.IsSystemColor);
		}

		/// ------------------------------------------------------------------------------------
		private void SetSelectionColors(bool hasFocus)
		{
			Logger.WriteEvent("Entering \"FieldValuesGrid.SetSelectionColors\" ({0})", Name);
			Logger.WriteEvent("    hasFocus = {0}", hasFocus);
			Logger.WriteEvent("    DefaultCellStyle.SelectionBackColor = {0}", DefaultCellStyle.SelectionBackColor);
			Logger.WriteEvent("    DefaultCellStyle.SelectionForeColor = {0}", DefaultCellStyle.SelectionForeColor);
			Logger.WriteEvent("    DefaultCellStyle.BackColor = {0}", DefaultCellStyle.BackColor);
			Logger.WriteEvent("    DefaultCellStyle.ForeColor = {0}", DefaultCellStyle.ForeColor);
			Logger.WriteEvent("    _focusedSelectionBackColor = {0}", FormatColorAsString(_focusedSelectionBackColor));
			Logger.WriteEvent("    Grid BackgroundColor = {0}", BackgroundColor);
			Logger.WriteEvent("    Grid ForeColor = {0}", ForeColor);

			// The reason the Focused property is not used is because when this method is
			// called in the Validated event (which is also true of the Leave and LostFocus
			// events) the Focused property is still true. Argh!
			DefaultCellStyle.SelectionBackColor = (hasFocus ?
				_focusedSelectionBackColor : BackgroundColor);
			Logger.WriteEvent("  Finished setting DefaultCellStyle.SelectionBackColor");
			Logger.WriteEvent("    DefaultCellStyle.SelectionBackColor = {0}", DefaultCellStyle.SelectionBackColor);

			Logger.WriteEvent("<Leaving \"FieldValuesGrid.SetSelectionColors\"");
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnGotFocus(EventArgs e)
		{
			Logger.WriteEvent("In \"FieldValuesGrid.OnGotFocus\"");
			if (CurrentCell == null)
				Logger.WriteEvent("    CurrentCell is null");
			else
				Logger.WriteEvent("    CurrentCell: Row = {0}; Column = {1}; Value = {2}", CurrentCell.RowIndex, CurrentCell.ColumnIndex, CurrentCell.Value ?? "Null");

			base.OnGotFocus(e);

			// In addition to getting this event when coming from a control outside of the
			// grid, we'll also get this event when a cell goes out of the editing mode and
			// it's editing control loses focus to the grid itself. So if we're here because
			// one of the cell's is coming out of edit mode, then we don't need to do anything.
			if (EditingControl != null)
				return;

			SetSelectionColors(true);
			var cell = CurrentCell;
			CurrentCell = (_model.GetIdForIndex(0) == null ? this[0, 0] : this[1, 0]);
			if (cell == CurrentCell)
				Logger.WriteEvent("  CurrentCell NOT changed");
			else
			{
				Logger.WriteEvent("  Finished setting CurrentCell");
				if (CurrentCell == null)
					Logger.WriteEvent("    CurrentCell is null");
				else
					Logger.WriteEvent("    CurrentCell: Row = {0}; Column = {1}; Value = {2}", CurrentCell.RowIndex,
						CurrentCell.ColumnIndex, CurrentCell.Value ?? "Null");
			}
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
			Logger.WriteEvent("In \"FieldValuesGrid.OnValidated\"");
			base.OnValidated(e);
			SetSelectionColors(false);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleComponentFileChanged()
		{
			var saveAdjustHeightToFitRows = _adjustHeightToFitRows;
			_adjustHeightToFitRows = false;
			RowCount = _model.RowData.Count + (AllowUserToAddRows ? 1 : 0);
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
			col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Columns.Add(col);

			col = CreateTextBoxColumn("colValue");
			col.HeaderText = "_L10N_:CommonToMultipleViews.FieldsAndValuesGrid.ColumnHeadings.Value!Value";
			col.Width = 175;
			col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
			if (e.ColumnIndex == 1)
			{
				Logger.WriteEvent("Entering \"FieldValuesGrid.OnCellFormatting\" ({0})", Name);
				Logger.WriteEvent("    e.RowIndex = {0}; e.ColumnIndex = {1}; Value = {2}", e.RowIndex, e.ColumnIndex,
					this[e.ColumnIndex, e.RowIndex].Value ?? "Null");
				Logger.WriteEvent("    e.CellStyle.BackColor = {0}", e.CellStyle.BackColor);
				Logger.WriteEvent("    e.CellStyle.ForeColor = {0}", e.CellStyle.ForeColor);
				Logger.WriteEvent("    e.CellStyle.SelectionBackColor = {0}", e.CellStyle.SelectionBackColor);
				Logger.WriteEvent("    e.CellStyle.SelectionForeColor = {0}", e.CellStyle.SelectionForeColor);
				if (e.ColumnIndex < ColumnCount && e.RowIndex >= 0 && e.RowIndex < RowCount)
				{
					Logger.WriteEvent("    this[e.ColumnIndex, e.RowIndex].Style.BackColor = {0}",
						this[e.ColumnIndex, e.RowIndex].Style.BackColor);
					Logger.WriteEvent("    this[e.ColumnIndex, e.RowIndex].Style.ForeColor = {0}",
						this[e.ColumnIndex, e.RowIndex].Style.ForeColor);
					Logger.WriteEvent("    this[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = {0}",
						this[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor);
					Logger.WriteEvent("    this[e.ColumnIndex, e.RowIndex].Style.SelectionForeColor = {0}",
						this[e.ColumnIndex, e.RowIndex].Style.SelectionForeColor);
				}
			}

			// REVIEW: For some reason, when the grid does not have focus, sometimes
			// setting a cell's readonly property to true gives the grid focus.

			if (_model != null)
			{
				if (e.ColumnIndex == 1)
				{
					Logger.WriteEvent("    Focused = {0}", Focused);
					Logger.WriteEvent("    DefaultCellStyle.SelectionBackColor = {0}", DefaultCellStyle.SelectionBackColor);
					Logger.WriteEvent("    DefaultCellStyle.SelectionForeColor = {0}", DefaultCellStyle.SelectionForeColor);
					Logger.WriteEvent("    DefaultCellStyle.BackColor = {0}", DefaultCellStyle.BackColor);
					Logger.WriteEvent("    DefaultCellStyle.ForeColor = {0}", DefaultCellStyle.ForeColor);
					Logger.WriteEvent("    Grid BackgroundColor = {0}", BackgroundColor);
					Logger.WriteEvent("    Grid ForeColor = {0}", ForeColor);
				}

				var isReadOnly = _model.IsIndexForReadOnlyField(e.RowIndex);
				var isCustom = _model.IsIndexForCustomField(e.RowIndex);

				if (string.IsNullOrEmpty(_model.GetIdForIndex(e.RowIndex)) && ColumnCount > 1)
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
						{
							this[1, e.RowIndex].Style.ForeColor = Color.Gray;
							Logger.WriteEvent("  After setting this[1, e.RowIndex].Style.ForeColor = Color.Gray");
							Logger.WriteEvent("    this[1, e.RowIndex].Style.ForeColor = {0}", this[1, e.RowIndex].Style.ForeColor);
						}
					}
				}
			}
			base.OnCellFormatting(e);
			if (e.ColumnIndex == 1)
			{
				Logger.WriteEvent("  After calling \"base.OnCellFormatting(e)\"");

				if (e.ColumnIndex < ColumnCount && e.RowIndex >= 0 && e.RowIndex < RowCount)
				{
					Logger.WriteEvent("    this[e.ColumnIndex, e.RowIndex].Style.BackColor = {0}",
						this[e.ColumnIndex, e.RowIndex].Style.BackColor);
					Logger.WriteEvent("    this[e.ColumnIndex, e.RowIndex].Style.ForeColor = {0}",
						this[e.ColumnIndex, e.RowIndex].Style.ForeColor);
					Logger.WriteEvent("    this[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = {0}",
						this[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor);
					Logger.WriteEvent("    this[e.ColumnIndex, e.RowIndex].Style.SelectionForeColor = {0}",
						this[e.ColumnIndex, e.RowIndex].Style.SelectionForeColor);
				}
				Logger.WriteEvent("< Leaving \"FieldValuesGrid.OnCellFormatting\"");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
		{
			Logger.WriteEvent("Entering \"FieldValuesGrid.OnEditingControlShowing\" ({0})", Name);
			Logger.WriteEvent("    Control Type = {0}, Name = {1}", e.Control.GetType().ToString(), e.Control.Name);
			if (CurrentCell != null)
			{
				Logger.WriteEvent("    CurrentCell.RowIndex = {0}; CurrentCell.ColumnIndex = {1}; Value = {2}", CurrentCell.RowIndex, CurrentCell.ColumnIndex, CurrentCell.Value ?? "Null");
				Logger.WriteEvent("    CurrentCell.Style.BackColor = {0}", CurrentCell.Style.BackColor);
				Logger.WriteEvent("    CurrentCell.Style.ForeColor = {0}", CurrentCell.Style.ForeColor);
				Logger.WriteEvent("    CurrentCell.Style.SelectionBackColor = {0}", CurrentCell.Style.SelectionBackColor);
				Logger.WriteEvent("    CurrentCell.Style.SelectionForeColor = {0}", CurrentCell.Style.SelectionForeColor);
			}
			Logger.WriteEvent("    e.CellStyle.BackColor = {0}", e.CellStyle.BackColor);
			Logger.WriteEvent("    e.CellStyle.ForeColor = ", e.CellStyle.ForeColor);
			Logger.WriteEvent("    e.CellStyle.SelectionBackColor = {0}", e.CellStyle.SelectionBackColor);
			Logger.WriteEvent("    e.CellStyle.SelectionForeColor = {0}", e.CellStyle.SelectionForeColor);

			if (e.Control != null)
			{
				Logger.WriteEvent("    e.Control.BackColor = {0}", e.Control.BackColor);
				Logger.WriteEvent("    e.Control.ForeColor = {0}", e.Control.ForeColor);
			}

			base.OnEditingControlShowing(e);

			var txtBox = e.Control as TextBox;

			// if not a text box, return now
			if (txtBox == null) return;

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

			Logger.WriteEvent("  Completed existing logic in \"FieldValuesGrid.OnEditingControlShowing\"");
			if (CurrentCell != null)
			{
				Logger.WriteEvent("    CurrentCell.Style.BackColor = {0}", CurrentCell.Style.BackColor);
				Logger.WriteEvent("    CurrentCell.Style.ForeColor = {0}", CurrentCell.Style.ForeColor);
				Logger.WriteEvent("    CurrentCell.Style.SelectionBackColor = {0}", CurrentCell.Style.SelectionBackColor);
				Logger.WriteEvent("    CurrentCell.Style.SelectionForeColor = {0}", CurrentCell.Style.SelectionForeColor);
			}

			if (e.Control != null && e.Control.ForeColor == e.Control.BackColor)
			{
				Logger.WriteEvent(">>>>>>>>>>>>> Forcing control colors to red and green!");
				e.Control.BackColor = Color.Red;
				e.Control.ForeColor = Color.GreenYellow;
			}
			Logger.WriteEvent("< Leaving \"FieldValuesGrid.OnEditingControlShowing\"");
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
