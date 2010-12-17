using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SilUtils;
using SilUtils.Controls;

namespace SayMore.UI.LowLevelControls
{
	public class ColumnChooserButton : XButton
	{
		private DataGridView _grid;

		public MultiValuePickerPopup ColChooserPopup { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ColumnChooserButton()
		{
			AutoSize = false;
			Image = Properties.Resources.ColumnChooser;
			Size = new Size(Image.Width + 4, Image.Height + 4);
			Text = string.Empty;

			if (!DesignMode)
			{
				ColChooserPopup = new MultiValuePickerPopup();
				ColChooserPopup.ItemCheckChanged += HandleColChooserItemCheckChanged;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataGridView Grid
		{
			get { return _grid; }
			set
			{
				_grid = value;
				Enabled = (_grid != null);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);

			if (!Enabled)
				return;

			ColChooserPopup.Clear();

			ColChooserPopup.AddRange(from col in _grid.Columns.Cast<DataGridViewColumn>()
									 where !col.Name.EndsWith("*")
									 select new PickerPopupItem(col.HeaderText, col.Visible) { Tag = col });

			var pt = PointToScreen(new Point(0, Height));
			ColChooserPopup.ShowPopup(pt);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleColChooserItemCheckChanged(object sender, PickerPopupItem item)
		{
			var col = item.Tag as DataGridViewColumn;

			// If the column is being hidden and it's the current column, then change the
			// current column before hiding it. Otherwise, the current column becomes indeterminate.
			if (_grid is SilGrid && !item.Checked && _grid != null && _grid.CurrentCellAddress.X == col.Index)
				((SilGrid)_grid).SelectAdjacentVisibleColumn(true);

			col.Visible = item.Checked;
		}
	}
}
