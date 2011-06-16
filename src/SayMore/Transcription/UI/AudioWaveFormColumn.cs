using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	public class AudioWaveFormColumn : TierColumnBase
	{
		private readonly TinyMediaPlayer _player;
		//private DateTime _lastShiftKeyPress;
		//private Control _gridEditControl;

		/// ------------------------------------------------------------------------------------
		public AudioWaveFormColumn(ITier tier) : base(tier)
		{
			Debug.Assert(tier.DataType == TierType.Audio);
			_player = new TinyMediaPlayer();
			_player.Visible = false;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnDataGridViewChanged()
		{
			if (_grid != null)
				_grid.Controls.Remove(_player);

			base.OnDataGridViewChanged();

			if (_grid != null)
				_grid.Controls.Add(_player);
		}

		/// ------------------------------------------------------------------------------------
		protected override void UnsubscribeToGridEvents()
		{
			_grid.Leave -= HandleGridLeave;
			_grid.RowEnter -= HandleGridRowEnter;
			_grid.CellFormatting -= HandleGridCellFormatting;
			_grid.ColumnWidthChanged -= HandleGridColumnWidthChanged;
			_grid.Scroll -= HandleGridScroll;

			base.UnsubscribeToGridEvents();
		}

		/// ------------------------------------------------------------------------------------
		protected override void SubscribeToGridEvents()
		{
			_grid.Leave += HandleGridLeave;
			_grid.RowEnter += HandleGridRowEnter;
			_grid.CellFormatting += HandleGridCellFormatting;
			_grid.ColumnWidthChanged += HandleGridColumnWidthChanged;
			_grid.Scroll += HandleGridScroll;

			//_grid.KeyDown += HandleKeyDown;

			//_grid.EditingControlShowing += (s, e) =>
			//{
			//    _gridEditControl = e.Control;
			//    _gridEditControl.KeyDown += HandleKeyDown;
			//};

			//_grid.CellEndEdit += (s, e) =>
			//{
			//    _gridEditControl.KeyDown -= HandleKeyDown;
			//    _gridEditControl = null;
			//};

			base.SubscribeToGridEvents();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridLeave(object sender, EventArgs e)
		{
			_player.Stop();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			if (e.Column.Index == Index)
				LocatePlayer(_grid.CurrentCellAddress.Y, false);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridRowEnter(object sender, DataGridViewCellEventArgs e)
		{
			LocatePlayer(e.RowIndex, true);
			Application.Idle -= HandleStartPlaybackOnIdle;
			Application.Idle += HandleStartPlaybackOnIdle;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleStartPlaybackOnIdle(object sender, EventArgs e)
		{
			Application.Idle -= HandleStartPlaybackOnIdle;

			if (_grid != null && (_grid.Focused || _grid.IsCurrentCellInEditMode))
				_player.Play();
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			base.HandleGridCellValueNeeded(sender, e);

			if (e.ColumnIndex != Index)
				return;

			var segment = Tier.GetSegment(e.RowIndex) as IMediaSegment;
			e.Value = _player.GetTimeInfoDisplayText(segment.MediaStart, segment.MediaLength);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex != Index)
				return;

			e.CellStyle.ForeColor = SystemColors.GrayText;
			e.CellStyle.Font = _player.Font;
		}

		/// ------------------------------------------------------------------------------------
		void HandleGridScroll(object sender, ScrollEventArgs e)
		{
			LocatePlayer(_grid.CurrentCellAddress.Y, false);
		}

		/// ------------------------------------------------------------------------------------
		private void LocatePlayer(int rowIndex, bool stopPlayingFirst)
		{
			if (_grid == null || rowIndex < _grid.FirstDisplayedScrollingRowIndex ||
				rowIndex > _grid.FirstDisplayedScrollingRowIndex + _grid.DisplayedRowCount(false) ||
				Index < _grid.FirstDisplayedScrollingColumnIndex ||
				Index > _grid.FirstDisplayedScrollingColumnIndex + _grid.DisplayedColumnCount(false))
			{
				_player.Visible = false;
				return;
			}

			//if (stopPlayingFirst)
			//    _player.Stop();

			var segment = Tier.GetSegment(rowIndex) as IMediaSegment;
			if (segment != _player.Segment)
				_player.LoadSegment(segment);

			var rc = GetPlayerRectangle(rowIndex);

			if (_player.Bounds != rc)
				_player.Bounds = rc;

			if (!_player.Visible)
				_player.Visible = true;

			_player.ForeColor = _grid.DefaultCellStyle.SelectionForeColor;
			_player.BackColor = _grid.DefaultCellStyle.SelectionBackColor;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// In most cases, getting the player's rectangle is simply done making a call to the
		/// grid's GetCellDisplayRectangle. However, when the grid's scroll event is fired,
		/// and the cell is just scrolling into view, calling GetCellDisplayRectangle to get
		/// its rectangle returns an empty rectangle. I think that's a bug in the grid, but
		/// whether or not it is, it has to be worked around. That's what most of this method
		/// does.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Rectangle GetPlayerRectangle(int rowIndex)
		{
			var rect = _grid.GetCellDisplayRectangle(Index, rowIndex, false);

			if (rect.Height > 0)
			{
				rect.Width--;
				rect.Height--;
				return rect;
			}

			// At this point, we know the current cell (i.e. the one containing the player)
			// has just been scrolled into view, whether horizontally or vertically.
			rect.Height = _grid.Rows[rowIndex].Height - 1;

			var rc = _grid.GetColumnDisplayRectangle(Index, true);

			if (rc.Width > 0)
			{
				rect.X = rc.X;
				rect.Width = rc.Width - 1;
			}
			{
				rect.X = _grid.RowHeadersWidth + 1;
				rect.Width = Width - 1;
			}

			if (rowIndex == _grid.FirstDisplayedScrollingRowIndex)
				rect.Y = _grid.ColumnHeadersHeight + 1;
			else
			{
				rc = _grid.GetRowDisplayRectangle(rowIndex - 1, false);
				rect.Y = (rc.Y + rc.Height);
			}

			return rect;
		}

		///// ------------------------------------------------------------------------------------
		//void HandleKeyDown(object sender, KeyEventArgs e)
		//{
		//    //if (!e.Shift)
		//    //    return;

		//    //if (DateTime.Now.Subtract(_lastShiftKeyPress).Milliseconds > 250)
		//    //    _lastShiftKeyPress = DateTime.Now;
		//    //else
		//    //{
		//    //    _player.Play();
		//    //}
		//}
	}
}
