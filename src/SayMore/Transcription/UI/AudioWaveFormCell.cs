using System;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Transcription.Model;
using SilTools;

namespace SayMore.Transcription.UI
{
	public class AudioWaveFormCell : DataGridViewTextBoxCell
	{
		private readonly Size _buttonSize = Properties.Resources.PlaySegment.Size;
		private TextAnnotationEditorGrid _grid;
		private bool _mouseIsOverButtonArea;
		private bool _playButtonVisible;

		/// ------------------------------------------------------------------------------------
		protected override void OnDataGridViewChanged()
		{
			base.OnDataGridViewChanged();
			_grid = DataGridView as TextAnnotationEditorGrid;
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetButtonRectangle(Rectangle rcCell)
		{
			var rc = new Rectangle(new Point(0, 0), _buttonSize);
			rc.X = rcCell.Right - rc.Width - 3;
			rc.Y = rcCell.Y + (int)Math.Round((rcCell.Height - rc.Height) / 2d, MidpointRounding.AwayFromZero);
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
		private Image GetImageToDraw()
		{
			if (_grid.PlayerViewModel.IsPlayButtonVisible)
			{
				_playButtonVisible = true;
				return (_mouseIsOverButtonArea ? Properties.Resources.PlaySegment_Hot :
					Properties.Resources.PlaySegment);
			}

			_playButtonVisible = false;
			return (_mouseIsOverButtonArea ? Properties.Resources.StopSegment_Hot :
				Properties.Resources.StopSegment);
		}

		/// ------------------------------------------------------------------------------------
		public override object DefaultNewRowValue
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (e.RowIndex != _grid.CurrentCellAddress.Y || !_mouseIsOverButtonArea ||
				e.Button != MouseButtons.Left)
			{
				return;
			}

			if (_playButtonVisible)
				_grid.Play();
			else
				_grid.Stop();

			_grid.InvalidateCell(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (e.RowIndex != _grid.CurrentCellAddress.Y)
				return;

			var rc = _grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
			rc.X = rc.Y = 0;
			var overButtonArea = GetButtonRectangle(rc).Contains(e.Location);

			if (_mouseIsOverButtonArea == overButtonArea)
				return;

			_mouseIsOverButtonArea = overButtonArea;
			_grid.InvalidateCell(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
			DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
			DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
			DataGridViewPaintParts paintParts)
		{
			// If we're drawing the current row's cell, then don't draw the cell's text.
			if (_grid.CurrentCellAddress.Y == rowIndex && rowIndex > 0)
				paintParts &= ~DataGridViewPaintParts.ContentForeground;

			// Draw most or all of the cell's parts, depending on whether or not this is the current row.
			base.Paint(g, clipBounds, cellBounds, rowIndex, cellState, value,
				formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

			// If not drawing the current row, then we're done.
			if (_grid.CurrentCellAddress.Y != rowIndex || rowIndex < 0)
				return;

			DrawPlaybackProgressBar(g, value as ITimeOrderSegment, cellBounds, cellStyle);

			// Now draw the cell's text.
			paintParts = DataGridViewPaintParts.ContentForeground;

			base.Paint(g, clipBounds, cellBounds, rowIndex, cellState, value,
				formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

			// Draw the play or stop button.
			g.DrawImage(GetImageToDraw(), GetButtonRectangle(cellBounds));
		}

		/// ------------------------------------------------------------------------------------
		private void DrawPlaybackProgressBar(Graphics g, ITimeOrderSegment segment,
			Rectangle cellBounds, DataGridViewCellStyle cellStyle)
		{
			if (segment == null || _playButtonVisible || !_grid.PlayerViewModel.HasPlaybackStarted)
				return;

			// Draw bar indicating playback progress.
			var rcBar = cellBounds;
			var playbackPosition = _grid.PlayerViewModel.CurrentPosition;

			if (playbackPosition < Math.Round(segment.Stop, 1, MidpointRounding.AwayFromZero))
			{
				var pixelsPerSec = rcBar.Width / segment.GetLength(1);
				rcBar.Width = (int)Math.Round(pixelsPerSec * (playbackPosition - segment.Start), MidpointRounding.AwayFromZero);
			}

			if (rcBar.Width <= 0)
				return;

			rcBar.Height -= 6;
			rcBar.Y += 3;
			using (var br = new SolidBrush(ColorHelper.CalculateColor(Color.White, cellStyle.SelectionBackColor, 110)))
				g.FillRectangle(br, rcBar);
		}
	}
}
