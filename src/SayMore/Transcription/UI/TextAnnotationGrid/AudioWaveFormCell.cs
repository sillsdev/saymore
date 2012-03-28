using System;
using System.Drawing;
using System.Windows.Forms;

namespace SayMore.Transcription.UI
{
	public class AudioWaveFormCell : DataGridViewTextBoxCell
	{
		private TextAnnotationEditorGrid _grid;

		public bool IsMouseIsOverButtonArea { get; private set; }

		/// ------------------------------------------------------------------------------------
		protected override void OnDataGridViewChanged()
		{
			base.OnDataGridViewChanged();

			if (_grid != null)
				HandleGridHandleDestoryed(null, null);

			_grid = DataGridView as TextAnnotationEditorGrid;

			if (_grid != null)
			{
				_grid.CellMouseLeave += HandleGridCellMouseLeave;
				_grid.HandleDestroyed += HandleGridHandleDestoryed;
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleGridHandleDestoryed(object sender, EventArgs e)
		{
			_grid.CellMouseLeave -= HandleGridCellMouseLeave;
			_grid.HandleDestroyed -= HandleGridHandleDestoryed;
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetButtonRectangle(Rectangle rcCell)
		{
			var rc = new Rectangle(new Point(0, 0), StandardAudioButtons.PlayButtonImage.Size);
			rc.X = rcCell.Right - rc.Width - 3;
			rc.Y = rcCell.Y + (int)Math.Floor((rcCell.Height - rc.Height) / 2d);
			return rc;
		}

		///// ------------------------------------------------------------------------------------
		//private Rectangle GetWaveFormRectangle(Rectangle rcCell)
		//{
		//    var rc = rcCell;
		//    rc.Width -= (_buttonSize.Width + 4);
		//    return rc;
		//}

		/// ------------------------------------------------------------------------------------
		private Image GetButtonImageToDraw()
		{
			if (!_grid.PlaybackInProgress)
			{
				//_playButtonVisible = true;
				return (IsMouseIsOverButtonArea ? StandardAudioButtons.HotPlayButtonImage : StandardAudioButtons.PlayButtonImage);
			}

			//_playButtonVisible = false;
			return (IsMouseIsOverButtonArea ? StandardAudioButtons.HotStopButtonImage : StandardAudioButtons.StopButtonImage);
		}

		/// ------------------------------------------------------------------------------------
		public override object DefaultNewRowValue
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		void HandleGridCellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			if (!_grid.Disposing && !_grid.IsDisposed && e.ColumnIndex == ColumnIndex && e.RowIndex == RowIndex)
			{
				IsMouseIsOverButtonArea = false;
				_grid.InvalidateCell(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (e.RowIndex != _grid.CurrentCellAddress.Y || !IsMouseIsOverButtonArea ||
				e.Button != MouseButtons.Left)
			{
				return;
			}

			if (_grid.PlaybackInProgress)
				_grid.Stop();
			else
				_grid.Play();

			_grid.InvalidateCell(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (e.RowIndex != _grid.CurrentCellAddress.Y)
				return;

			// Determine if mouse is over the part of the cell where the button is drawn.
			var rc = _grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
			rc.X = rc.Y = 0;
			rc.Width = _grid.Columns[ColumnIndex].Width;

			var overButtonArea = GetButtonRectangle(rc).Contains(e.Location);

			if (IsMouseIsOverButtonArea == overButtonArea)
				return;

			IsMouseIsOverButtonArea = overButtonArea;
			_grid.InvalidateCell(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
			DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
			DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
			DataGridViewPaintParts paintParts)
		{
			paintParts &= ~DataGridViewPaintParts.Focus;

			// If we're drawing the current row's cell, then don't draw the cell's text.
			if (_grid.CurrentCellAddress.Y == rowIndex && rowIndex > 0)
				paintParts &= ~DataGridViewPaintParts.ContentForeground;

			// Draw most or all of the cell's parts, depending on whether or not this is the current row.
			base.Paint(g, clipBounds, cellBounds, rowIndex, cellState, value,
				formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

			// If not drawing the current row, then we're done.
			if (_grid.CurrentCellAddress.Y != rowIndex || rowIndex < 0)
				return;

			_grid.DrawPlaybackProgressBar(g, cellBounds, cellStyle.SelectionBackColor);

			// Now draw the cell's text.
			paintParts = DataGridViewPaintParts.ContentForeground;

			base.Paint(g, clipBounds, cellBounds, rowIndex, cellState, value,
				formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

			// Draw the play or stop button.
			g.DrawImage(GetButtonImageToDraw(), GetButtonRectangle(cellBounds));
		}
	}
}
