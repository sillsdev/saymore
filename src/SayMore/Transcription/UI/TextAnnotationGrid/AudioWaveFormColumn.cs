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
		protected bool _mediaFileNeedsLoading = true;
		protected Control _gridEditControl;

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

		/// ------------------------------------------------------------------------------------
		protected override void SubscribeToGridEvents()
		{
			//_grid.Leave += HandleGridLeave;
			//_grid.Enter += HandleGridEnter;
			_grid.CellEnter += HandleCellEnter;
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
			_grid.CellEnter -= HandleCellEnter;
			_grid.SetPlaybackProgressReportAction(null);
			base.UnsubscribeToGridEvents();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCellEnter(object sender, DataGridViewCellEventArgs e)
		{
			_grid.AnnotationPlaybackInfoProvider = (t => GetAnnotationMediaInfo(t));
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<AnnotationPlaybackInfo> GetAnnotationMediaInfo(OralAnnotationType playbackType)
		{
			var carefulSpeechFile = GetCurrentSegment().GetPathToCarefulSpeechFile();
			var oralTranslationFile = GetCurrentSegment().GetPathToOralTranslationFile();

			if ((playbackType & OralAnnotationType.Original) == OralAnnotationType.Original)
				yield return GetOriginalAnnotationPlaybackInfo();

			if ((playbackType & OralAnnotationType.Careful) == OralAnnotationType.Careful)
			{
				if (File.Exists(carefulSpeechFile))
					yield return new AnnotationPlaybackInfo { MediaFile = carefulSpeechFile };
				else if (playbackType == OralAnnotationType.Careful)
					yield return GetOriginalAnnotationPlaybackInfo();
			}

			if ((playbackType & OralAnnotationType.Translation) == OralAnnotationType.Translation)
			{
				if (File.Exists(oralTranslationFile))
					yield return new AnnotationPlaybackInfo { MediaFile = oralTranslationFile };
				else if (playbackType == OralAnnotationType.Translation)
					yield return GetOriginalAnnotationPlaybackInfo();
			}
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
