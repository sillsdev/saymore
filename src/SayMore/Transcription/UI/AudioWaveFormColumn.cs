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
			base.OnDataGridViewChanged();
			_grid.Controls.Add(_player);
		}

		/// ------------------------------------------------------------------------------------
		protected override void UnsubscribeToGridEvents()
		{
			_grid.Leave -= HandleGridLeave;
			_grid.RowEnter -= HandleGridRowEnter;
			_grid.CellPainting -= HandleCellPainting;
			_grid.ColumnWidthChanged -= HandleGridColumnWidthChanged;
		}

		/// ------------------------------------------------------------------------------------
		protected override void SubscribeToGridEvents()
		{
			_grid.Leave += HandleGridLeave;
			_grid.RowEnter += HandleGridRowEnter;
			_grid.CellPainting += HandleCellPainting;
			_grid.ColumnWidthChanged += HandleGridColumnWidthChanged;

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
				LocatePlayer(DataGridView.CurrentCellAddress.Y, false);
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

			if (DataGridView != null && (DataGridView.Focused || DataGridView.IsCurrentCellInEditMode))
				_player.Play();
		}

		/// ------------------------------------------------------------------------------------
		void HandleCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.ColumnIndex != Index || e.RowIndex < 0)
				return;

			e.Handled = true;
			var rc = e.CellBounds;
			e.Paint(rc, DataGridViewPaintParts.Border);

			rc.Width--;
			rc.Height--;

			var segment = Tier.GetSegment(e.RowIndex) as IMediaSegment;

			_player.DrawTimeInfo(e.Graphics, segment.MediaStart, segment.MediaLength,
				rc, SystemColors.GrayText, e.CellStyle.BackColor);
		}

		/// ------------------------------------------------------------------------------------
		private void LocatePlayer(int rowIndex, bool stopPlayingFirst)
		{
			if (DataGridView == null)
			{
				_player.Visible = false;
				return;
			}

			//if (stopPlayingFirst)
			//    _player.Stop();

			var segment = Tier.GetSegment(rowIndex) as IMediaSegment;
			if (segment != _player.Segment)
				_player.LoadSegment(segment);

			var rc = DataGridView.GetCellDisplayRectangle(Index, rowIndex, false);
			rc.Width--;
			rc.Height--;

			if (_player.Bounds != rc)
			{
				_player.Bounds = rc;

				if (!_player.Visible)
					_player.Visible = true;
			}

			_player.ForeColor = DataGridView.DefaultCellStyle.SelectionForeColor;
			_player.BackColor = DataGridView.DefaultCellStyle.SelectionBackColor;
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
