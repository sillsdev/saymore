using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SayMore.Transcription.Model;
using SilTools;

namespace SayMore.Transcription.UI
{
	public class AudioWaveFormColumn : TierColumnBase
	{
		protected bool _mediaFileNeedsLoading = true;

		//private CheckBoxColumnHeaderHandler _chkBoxColHdrHandler;
		//private DateTime _lastShiftKeyPress;
		private Control _gridEditControl;

		/// ------------------------------------------------------------------------------------
		public AudioWaveFormColumn(TierBase tier) : base(new AudioWaveFormCell(), tier)
		{
			Debug.Assert(tier.TierType == TierType.Time);
			ReadOnly = true;

			DefaultCellStyle.Font = FontHelper.MakeFont(SystemFonts.IconTitleFont, 7f);
			DefaultCellStyle.ForeColor = ColorHelper.CalculateColor(Color.White, DefaultCellStyle.ForeColor, 85);
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

		///// ------------------------------------------------------------------------------------
		//public bool IsColumnChecked
		//{
		//    get { return _chkBoxColHdrHandler.HeadersCheckState == CheckState.Checked; }
		//}

		/// ------------------------------------------------------------------------------------
		protected override void SubscribeToGridEvents()
		{
			//_grid.Leave += HandleGridLeave;
			//_grid.Enter += HandleGridEnter;
			_grid.CurrentRowChanged += HandleCurrentRowChanged;
			_grid.SetPlaybackProgressReportAction(() => _grid.InvalidateCell(Index, _grid.CurrentCellAddress.Y));

			//_grid.PlaybackSpeedChanged += HandlePlaybackSpeedChanged;

			_grid.KeyDown += HandleKeyDown;

			_grid.EditingControlShowing += (s, e) =>
			{
				_gridEditControl = e.Control;
				_gridEditControl.KeyDown += HandleKeyDown;
			};

			_grid.CellEndEdit += (s, e) =>
			{
				_gridEditControl.KeyDown -= HandleKeyDown;
				_gridEditControl = null;
			};

			base.SubscribeToGridEvents();
		}

		/// ------------------------------------------------------------------------------------
		protected override void UnsubscribeToGridEvents()
		{
			_grid.KeyDown -= HandleKeyDown;
			_grid.CurrentRowChanged -= HandleCurrentRowChanged;
			_grid.SetPlaybackProgressReportAction(null);
			base.UnsubscribeToGridEvents();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCurrentRowChanged(object sender, EventArgs e)
		{
			_grid.SegmentProvider = GetCurrentSegment;
			_grid.MediaFileProvider = (() => ((TimeTier)Tier).MediaFileName);
		}

		/// ------------------------------------------------------------------------------------
		public Segment GetCurrentSegment()
		{
			return Tier.Segments.ElementAt(_grid.CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			base.HandleGridCellValueNeeded(sender, e);

			if (e.ColumnIndex != Index)
				return;

			e.Value = Tier.Segments.ElementAt(e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		protected void RedrawPlayerCell()
		{
			if (_grid != null)
				_grid.InvalidateCell(Index, _grid.CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		void HandleKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.F2)
				return;

			if (_grid.PlaybackInProgress)
				_grid.Stop();
			else
				_grid.Play();

			_grid.InvalidateCell(Index, _grid.CurrentCellAddress.Y);
		}
	}
}
