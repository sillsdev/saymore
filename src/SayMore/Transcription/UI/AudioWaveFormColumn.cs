using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SayMore.Transcription.Model;
using SayMore.UI.MediaPlayer;
using SilTools;

namespace SayMore.Transcription.UI
{
	public class AudioWaveFormColumn : TierColumnBase
	{
		protected bool _mediaFileNeedsLoading = true;

		public MediaPlayerViewModel PlayerViewModel { get; private set; }
		public ITimeOrderSegment CurrentSegment { get; private set; }

		//private DateTime _lastShiftKeyPress;
		//private Control _gridEditControl;

		/// ------------------------------------------------------------------------------------
		public AudioWaveFormColumn(ITier tier) : base(new AudioWaveFormCell(), tier)
		{
			Debug.Assert(tier.DataType == TierType.Audio || tier.DataType == TierType.TimeOrder);
			ReadOnly = true;

			DefaultCellStyle.Font = FontHelper.MakeFont(SystemFonts.IconTitleFont, 7f);
			DefaultCellStyle.ForeColor = ColorHelper.CalculateColor(Color.White, DefaultCellStyle.ForeColor, 85);

			PlayerViewModel = new MediaPlayerViewModel();
			PlayerViewModel.SetVolume(100);
			PlayerViewModel.Loop = true;

			if (Tier.GetAllSegments().Count() > 0)
				CurrentSegment = Tier.GetSegment(0) as ITimeOrderSegment;
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
			_grid.Leave += HandleGridLeave;
			_grid.Enter += HandleGridEnter;
			_grid.RowEnter += HandleGridRowEnter;

			if (_grid is TextAnnotationEditorGrid)
				((TextAnnotationEditorGrid)_grid).PlaybackSpeedChanged += HandlePlaybackSpeedChanged;

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
			_grid.Leave -= HandleGridLeave;
			_grid.RowEnter -= HandleGridRowEnter;

			if (_grid.FindForm() != null)
				_grid.FindForm().Deactivate -= HandleGridLeave;

			if (_grid is TextAnnotationEditorGrid)
				((TextAnnotationEditorGrid)_grid).PlaybackSpeedChanged -= HandlePlaybackSpeedChanged;

			base.UnsubscribeToGridEvents();
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackSpeedChanged(object sender, int newSpeed)
		{
			PlayerViewModel.Speed = newSpeed;
		}

		/// ------------------------------------------------------------------------------------
		void HandleGridEnter(object sender, EventArgs e)
		{
			if (_grid != null && _grid.FindForm() != null)
				_grid.FindForm().Deactivate += HandleGridLeave;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridLeave(object sender, EventArgs e)
		{
			Stop();
			RedrawPlayerCell();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridRowEnter(object sender, DataGridViewCellEventArgs e)
		{
			Stop();
			Application.Idle -= HandleStartPlaybackOnIdle;

			if (_grid.CurrentCellAddress.Y < 0)
				return;

			var segment = Tier.GetSegment(e.RowIndex) as ITimeOrderSegment;
			_mediaFileNeedsLoading = (segment != CurrentSegment);
			CurrentSegment = segment;
			//Application.Idle += HandleStartPlaybackOnIdle;
			Play();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleStartPlaybackOnIdle(object sender, EventArgs e)
		{
			Application.Idle -= HandleStartPlaybackOnIdle;

			if (_grid != null && (_grid.Focused || _grid.IsCurrentCellInEditMode))
				Play();
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			base.HandleGridCellValueNeeded(sender, e);

			if (e.ColumnIndex != Index)
				return;

			var segment = Tier.GetSegment(e.RowIndex) as ITimeOrderSegment;
			var length = segment.GetLength();

			e.Value = PlayerViewModel.GetRangeTimeDisplay(segment.Start, (length.Equals(0f) ? 0 :
				(float)((decimal)segment.Start + (decimal)length)));
		}

		/// ------------------------------------------------------------------------------------
		public void Play()
		{
			if (PlayerViewModel.HasPlaybackStarted)
				Stop();

			if (_mediaFileNeedsLoading)
			{
				PlayerViewModel.LoadFile(((TimeOrderTier)Tier).MediaFileName,
					CurrentSegment.Start, CurrentSegment.GetLength());
			}

			PlayerViewModel.PlaybackStarted = (() => _grid.Invoke((Action)RedrawPlayerCell));
			PlayerViewModel.PlaybackEnded = (() => _grid.Invoke((Action)RedrawPlayerCell));
			PlayerViewModel.PlaybackPositionChanged = (pos => _grid.Invoke((Action)RedrawPlayerCell));
			PlayerViewModel.Play();
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			PlayerViewModel.PlaybackStarted = null;
			PlayerViewModel.PlaybackEnded = null;
			PlayerViewModel.PlaybackPositionChanged = null;
			PlayerViewModel.Stop();
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
