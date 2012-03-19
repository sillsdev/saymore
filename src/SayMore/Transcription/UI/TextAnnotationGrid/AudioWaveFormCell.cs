using System;
using System.Drawing;
using System.Windows.Forms;
using SilTools;

namespace SayMore.Transcription.UI
{
	public class AudioWaveFormCell : DataGridViewTextBoxCell
	{
		private static Image s_hotPlayButtonImage;
		private static Image s_hotStopButtonImage;
		private static Image s_playButtonImage;
		private static Image s_stopButtonImage;

		private TextAnnotationEditorGrid _grid;

		public bool IsMouseIsOverButtonArea { get; private set; }

		/// ------------------------------------------------------------------------------------
		public AudioWaveFormCell()
		{
			if (s_playButtonImage == null)
				s_playButtonImage = CreateMediaControlImage(Properties.Resources.PlaySegment);

			if (s_stopButtonImage == null)
				s_stopButtonImage = CreateMediaControlImage(Properties.Resources.StopSegment);

			if (s_hotPlayButtonImage == null)
				s_hotPlayButtonImage = PaintingHelper.MakeHotImage(s_playButtonImage);

			if (s_hotStopButtonImage == null)
				s_hotStopButtonImage = PaintingHelper.MakeHotImage(s_stopButtonImage);
		}

		/// ------------------------------------------------------------------------------------
		private Image CreateMediaControlImage(Image img)
		{
			var bmp = new Bitmap(img.Width + 6, img.Height + 6);

			using (var br = new SolidBrush(Color.FromArgb(255, Color.White)))
			using (var g = Graphics.FromImage(bmp))
			{
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				g.FillEllipse(br, 0, 0, img.Width + 5, img.Width + 5);
				g.DrawImage(img, 3, 3, img.Width, img.Height);
			}

			return bmp;
		}

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
			var rc = new Rectangle(new Point(0, 0), s_playButtonImage.Size);
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
				return (IsMouseIsOverButtonArea ? s_hotPlayButtonImage : s_playButtonImage);
			}

			//_playButtonVisible = false;
			return (IsMouseIsOverButtonArea ? s_hotStopButtonImage : s_stopButtonImage);
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
