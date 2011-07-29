using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Transcription.Model;
using SilTools;

namespace SayMore.Transcription.UI
{
	public class AudioWaveFormColumn : TierColumnBase
	{
		protected bool _mediaFileNeedsLoading = true;

		//private DateTime _lastShiftKeyPress;
		//private Control _gridEditControl;

		/// ------------------------------------------------------------------------------------
		public AudioWaveFormColumn(ITier tier) : base(new AudioWaveFormCell(), tier)
		{
			Debug.Assert(tier.DataType == TierType.Audio || tier.DataType == TierType.TimeOrder);
			ReadOnly = true;

			DefaultCellStyle.Font = FontHelper.MakeFont(SystemFonts.IconTitleFont, 7f);
			DefaultCellStyle.ForeColor = ColorHelper.CalculateColor(Color.White, DefaultCellStyle.ForeColor, 85);

			//if (Tier.GetAllSegments().Count() > 0)
			//    CurrentSegment = Tier.GetSegment(0) as ITimeOrderSegment;
		}

		/// ------------------------------------------------------------------------------------
		public override DataGridViewCell CellTemplate
		{
			get { return base.CellTemplate; }
			set
			{
				// Ensure that the cell used for the template is a CalendarCell.
				if (value != null && !value.GetType().IsAssignableFrom(typeof(AudioWaveFormCell)))
					throw new InvalidCastException("Cell template must be an AudioWaveFormCell");

				base.CellTemplate = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void SubscribeToGridEvents()
		{
			//_grid.Leave += HandleGridLeave;
			//_grid.Enter += HandleGridEnter;
			_grid.CurrentRowChanged += HandleCurrentRowChanged;
			_grid.SetPlaybackProgressReportAction(() => _grid.InvalidateCell(Index, _grid.CurrentCellAddress.Y));

			//_grid.PlaybackSpeedChanged += HandlePlaybackSpeedChanged;

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
		protected override void UnsubscribeToGridEvents()
		{
			_grid.CurrentRowChanged -= HandleCurrentRowChanged;
			_grid.SetPlaybackProgressReportAction(null);
			base.UnsubscribeToGridEvents();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCurrentRowChanged(object sender, EventArgs e)
		{
			_grid.SegmentProvider = GetCurrentSegment;
			_grid.MediaFileProvider = (() => ((TimeOrderTier)Tier).MediaFileName);
		}

		/// ------------------------------------------------------------------------------------
		public ITimeOrderSegment GetCurrentSegment()
		{
			return Tier.GetSegment(_grid.CurrentCellAddress.Y) as ITimeOrderSegment;
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			base.HandleGridCellValueNeeded(sender, e);

			if (e.ColumnIndex != Index)
				return;

			e.Value = Tier.GetSegment(e.RowIndex) as ITimeOrderSegment;
		}

		/// ------------------------------------------------------------------------------------
		protected void RedrawPlayerCell()
		{
			if (_grid != null)
				_grid.InvalidateCell(Index, _grid.CurrentCellAddress.Y);
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
