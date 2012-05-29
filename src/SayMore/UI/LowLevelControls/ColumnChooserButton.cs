using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using Localization;
using Palaso.UI.WindowsForms.ClearShare.WinFormsUI;
using SilTools;
using SilTools.Controls;

namespace SayMore.UI.LowLevelControls
{
	public class ColumnChooserButton : XButton
	{
		private DataGridView _grid;
		private FadingMessageWindow _msgWindow;

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

			if (_msgWindow != null)
				_msgWindow.Close();

			ColChooserPopup.Clear();

			ColChooserPopup.AddRange(from col in _grid.Columns.Cast<DataGridViewColumn>()
									 orderby col.DisplayIndex
									 select new PickerPopupItem(col.HeaderText, col.Visible) { Tag = col });

			var pt = PointToScreen(new Point(0, Height));
			ColChooserPopup.ShowPopup(pt);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleColChooserItemCheckChanged(object sender, PickerPopupItem item)
		{
			var col = item.Tag as DataGridViewColumn;

			// Check if the column is being hidden.
			if (!item.Checked)
			{
				if (_grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).Count() == 1)
				{
					ColChooserPopup.ItemCheckChanged -= HandleColChooserItemCheckChanged;
					item.Checked = true;
					ColChooserPopup.ItemCheckChanged += HandleColChooserItemCheckChanged;

					if (_msgWindow == null)
						_msgWindow = new FadingMessageWindow();

					var msg = LocalizationManager.GetString("CommonToMultipleViews.ElementList.OneColumnMustBeVisibleMsg",
						"One column must be visible.",
						"Displayed when user unchecks all columns for display in sessions or people list");

					var pt = PointToScreen(new Point(Width / 2, Height / 3));
					_msgWindow.Show(msg, pt);

					SystemSounds.Beep.Play();
					return;
				}

				// If the column is the current column, then change the current column
				// before hiding it. Otherwise, the current column becomes indeterminate.
				if (_grid is SilGrid && _grid.CurrentCellAddress.X == col.Index)
					((SilGrid)_grid).SelectAdjacentVisibleColumn(true);
			}

			_grid.SuspendLayout();
			col.Visible = item.Checked;
			_grid.ResumeLayout();
		}
	}
}
