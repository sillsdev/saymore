using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SayMore.Transcription.Model;
using SilTools;

namespace SayMore.Transcription.UI
{
	public class AudioWaveFormColumn : TierColumnBase
	{
		public OralAnnotationType PlaybackAnnotationType { get; private set; }

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

			PlaybackAnnotationType = OralAnnotationType.Careful;
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
			_grid.AnnotationPlaybackInfoProvider = (() => GetAnnotationMediaInfo());
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<AnnotationPlaybackInfo> GetAnnotationMediaInfo()
		{
			var carefulSpeechFile = GetCurrentSegment().GetPathToCarefulSpeechFile();

			if ((PlaybackAnnotationType & OralAnnotationType.Original) == OralAnnotationType.Original)
				yield return GetOriginalAnnotationPlaybackInfo();

			if ((PlaybackAnnotationType & OralAnnotationType.Careful) == OralAnnotationType.Careful &&
				File.Exists(carefulSpeechFile))
			{
				yield return new AnnotationPlaybackInfo { MediaFile = carefulSpeechFile };
			}
			else if (PlaybackAnnotationType == OralAnnotationType.Careful)
				yield return GetOriginalAnnotationPlaybackInfo();
		}

		/// ------------------------------------------------------------------------------------
		private AnnotationPlaybackInfo GetOriginalAnnotationPlaybackInfo()
		{
			var segment = GetCurrentSegment();

			return new AnnotationPlaybackInfo
			{
				MediaFile = ((TimeTier)Tier).MediaFileName,
				Start = segment.Start,
				End = segment.End,
				Length = segment.GetLength(1)
			};
		}

		/// ------------------------------------------------------------------------------------
		public Segment GetCurrentSegment()
		{
			return Tier.Segments[_grid.CurrentCellAddress.Y];
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			base.HandleGridCellValueNeeded(sender, e);

			if (e.ColumnIndex != Index)
				return;

			e.Value = Tier.Segments[e.RowIndex];
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

	/// ----------------------------------------------------------------------------------------
	public class AnnotationPlaybackInfo
	{
		public string MediaFile;
		public float Start;
		public float End;
		public float Length;
	}
}
