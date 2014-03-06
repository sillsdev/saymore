using System;
using System.Drawing;
using System.Windows.Forms;

// SP-848: Testing another possible fix.
namespace SayMore.UI.LowLevelControls
{
	public class CustomDataGridTextBoxColumn : DataGridViewColumn
	{
		public CustomDataGridTextBoxColumn()
			: base(new CustomDataGridTextBoxCell())
		{
			HeaderCell.Style.Font = SystemFonts.MenuFont;
		}

		public override DataGridViewCell CellTemplate
		{
			get { return base.CellTemplate; }
			set
			{
				// Ensure that the cell used for the template is a CustomDataGridTextBoxCell.
				if (value != null &&
					!value.GetType().IsAssignableFrom(typeof(CustomDataGridTextBoxCell)))
				{
					throw new InvalidCastException("Must be a CustomDataGridTextBoxCell");
				}
				base.CellTemplate = value;
			}
		}
	}

	public class CustomDataGridTextBoxCell : DataGridViewTextBoxCell
	{
		public CustomDataGridTextBoxCell()
		{
			Style.Font = SystemFonts.MenuFont;
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

			var ctl = DataGridView.EditingControl as CustomDataGridTextBox;
			if (ctl == null) return;

			// select the current text
			if (ctl.Text.Length <= 0) return;     // nothing to select
			if (ctl.SelectionLength != 0) return; // already selected

			ctl.SelectionStart = 0;
			ctl.SelectionLength = ctl.TextLength;
		}

		public override Type EditType
		{
			get { return typeof(CustomDataGridTextBox); }
		}

		public override Type ValueType
		{
			get { return typeof(string); }
		}

		public override object DefaultNewRowValue
		{
			get { return string.Empty; }
		}
	}

	public class CustomDataGridTextBox : TextBox, IDataGridViewEditingControl
	{
		public DataGridView EditingControlDataGridView { get; set; }
		public int EditingControlRowIndex { get; set; }
		public bool EditingControlValueChanged { get; set; }

		public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
		{
			Font = dataGridViewCellStyle.Font;
			ForeColor = dataGridViewCellStyle.ForeColor;
			BackColor = dataGridViewCellStyle.BackColor;
		}

		public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
		{
			return !dataGridViewWantsInputKey;
		}

		public object EditingControlFormattedValue
		{
			get { return Text; }
			set
			{
				if (!(value is string)) return;
				Text = (string)value;
			}
		}

		public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return EditingControlFormattedValue;
		}

		public void PrepareEditingControlForEdit(bool selectAll)
		{
			// nothing needs done
		}

		public Cursor EditingPanelCursor
		{
			get { return base.Cursor; }
		}

		public bool RepositionEditingControlOnValueChange
		{
			get { return false; }
		}

		protected override void OnTextChanged(EventArgs e)
		{
			EditingControlValueChanged = true;
			EditingControlDataGridView.NotifyCurrentCellDirty(true);
			base.OnTextChanged(e);
		}
	}
}
