using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Localization;
using Palaso.Reporting;
using Palaso.UI.WindowsForms.Miscellaneous;
using SayMore.Media.Audio;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI.SegmentingAndRecording;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class ManualSegmenterDlg : SegmenterDlgBase
	{
		private readonly string _origAddSegBoundaryButtonText;
		private WaveControlWithBoundarySelection _waveControl;
		private bool _justStoppedusingSpace;

		/// ------------------------------------------------------------------------------------
		public static string ShowDialog(ComponentFile file, Control parent, int segmentToHighlight)
		{
			Exception error;
			string msg;

			using (var viewModel = new ManualSegmenterDlgViewModel(file))
			using (var dlg = new ManualSegmenterDlg(viewModel, segmentToHighlight))
			{
				try
				{
					if (dlg.ShowDialog(parent) != DialogResult.OK || !viewModel.WereChangesMade)
					{
						viewModel.DiscardChanges();
						return null;
					}

					var annotationFile = file.GetAnnotationFile();

					var eafFile = AnnotationFileHelper.Save(file.PathToAnnotatedFile, viewModel.Tiers);

					if (annotationFile == null)
						return eafFile;

					error = annotationFile.TryLoadAndReturnException();
					if (error == null)
					{
						WaitCursor.Show();
						try
						{
							annotationFile.AssociatedComponentFile.GenerateOralAnnotationFile(viewModel.Tiers,
								parent, ComponentFile.GenerateOption.ClearAndRegenerateOnDemand);
						}
						finally
						{
							WaitCursor.Hide();
						}
						return eafFile;
					}

					msg = LocalizationManager.GetString(
						"DialogBoxes.Transcription.ManualSegmenterDlg.SavingSegmentsErrorMsg",
						"There was an error while trying to save segments for the file '{0}'.");
				}
				catch (Exception e)
				{
					error = e;
					msg = LocalizationManager.GetString(
						"DialogBoxes.Transcription.ManualSegmenterDlg.GeneralSegmentingErrorMsg",
						"There was an error segmenting the file '{0}'.");
				}
			}

			ErrorReport.NotifyUserOfProblem(error, msg, file.PathToAnnotatedFile);
			return null;
		}

		/// ------------------------------------------------------------------------------------
		public ManualSegmenterDlg(ManualSegmenterDlgViewModel viewModel, int segmentToHighlight)
			: base(viewModel)
		{
			InitializeComponent();
			_tableLayoutButtons.BackColor = Settings.Default.BarColorEnd;
			Opacity = 0D;

			Controls.Remove(toolStripButtons);
			_tableLayoutOuter.Controls.Add(toolStripButtons);
			_tableLayoutOuter.Resize += HandleTableLayoutOuterResize;

			_origAddSegBoundaryButtonText = _buttonAddSegmentBoundary.Text;

			_buttonStopOriginal.Click += delegate { _waveControl.Stop(); };

			if (segmentToHighlight > 0)
			{
				Shown += delegate {

					var endOfPreviousSegment = ViewModel.GetEndOfSegment(segmentToHighlight - 1);
					_waveControl.SetCursor(endOfPreviousSegment);
					_waveControl.EnsureTimeIsVisible(endOfPreviousSegment);
				};
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleTableLayoutOuterResize(object sender, EventArgs e)
		{
			if (toolStripButtons.PreferredSize.Width + _toolStripStatus.PreferredSize.Width > _tableLayoutOuter.Width)
			{
				_tableLayoutOuter.ColumnStyles[0].SizeType = SizeType.AutoSize;
				_tableLayoutOuter.ColumnStyles[1].SizeType = SizeType.Percent;
				_tableLayoutOuter.ColumnStyles[1].Width = 100;
			}
			else
			{
				_tableLayoutOuter.ColumnStyles[0].SizeType = SizeType.Percent;
				_tableLayoutOuter.ColumnStyles[0].Width = 100;
				_tableLayoutOuter.ColumnStyles[1].SizeType = SizeType.AutoSize;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleListenToOriginalClick(object sender, EventArgs e)
		{
			if (!_waveControl.IsPlaying)
			{
				_waveControl.Play(_waveControl.GetCursorTime());
				_newSegmentDefinedBy = SegmentDefinitionMode.Manual;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleAddSegmentBoundaryClick(object sender, EventArgs e)
		{
			var cursorTime = _waveControl.GetCursorTime();
			if (_viewModel.GetIsSegmentLongEnough(cursorTime))
			{
				var newsegmentBoundaries = ViewModel.InsertNewBoundary(cursorTime);
				_waveControl.SegmentBoundaries = newsegmentBoundaries;
				int i = 0;
				TimeSpan originalEnd = new TimeSpan();
				foreach (var boundary in newsegmentBoundaries)
				{
					if (boundary > cursorTime)
					{
						originalEnd = boundary;
						break;
					}
					i++;
				}
				if (originalEnd > cursorTime && ViewModel.GetIsSegmentIgnored(i))
					_waveControl.Painter.AddIgnoredRegion(new TimeRange(cursorTime, originalEnd));

				_waveControl.SetSelectedBoundary(_waveControl.GetCursorTime());
				UpdateDisplay();
			}
			else
			{
				_newSegmentDefinedBy = _waveControl.IsPlaying ? SegmentDefinitionMode.AddButtonWhileListening : SegmentDefinitionMode.Manual;
				StopAllMedia();
				_buttonAddSegmentBoundary.ForeColor = Color.Red;
				_buttonAddSegmentBoundary.Text = GetSegmentTooShortText();
				_clearWarningMessageTimer.Tick += ResetAddSegmentButton;
				_clearWarningMessageTimer.Start();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDeleteSegmentClick(object sender, EventArgs e)
		{
			_newSegmentDefinedBy = SegmentDefinitionMode.Manual;
			var boundary = _waveControl.GetSelectedBoundary();
			float boundarySeconds = (float)boundary.TotalSeconds;
			var ignoredBundaryToRemove = new TimeSpan();
			if (!ViewModel.GetIsSegmentIgnored(ViewModel.TimeTier.GetSegmentHavingEndBoundary(boundarySeconds)))
			{
				var segmentFollowingDeletedBreak = ViewModel.TimeTier.GetSegmentHavingStartBoundary(boundarySeconds);
				if (ViewModel.GetIsSegmentIgnored(segmentFollowingDeletedBreak))
					ignoredBundaryToRemove = segmentFollowingDeletedBreak.TimeRange.End;
			}

			_waveControl.ClearSelectedBoundary();
			if (!ViewModel.DeleteBoundary(boundary))
				return;

			_waveControl.SegmentBoundaries = _viewModel.GetSegmentEndBoundaries();
			_waveControl.Painter.RemoveIgnoredRegion(boundary);
			if (ignoredBundaryToRemove.Seconds > 0)
				_waveControl.Painter.RemoveIgnoredRegion(ignoredBundaryToRemove);

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void ResetAddSegmentButton(object sender, EventArgs e)
		{
			_clearWarningMessageTimer.Stop();
			_buttonAddSegmentBoundary.ForeColor = _buttonListenToOriginal.ForeColor;
			_buttonAddSegmentBoundary.Text = _origAddSegBoundaryButtonText;
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private ManualSegmenterDlgViewModel ViewModel
		{
			get { return _viewModel as ManualSegmenterDlgViewModel; }
		}

		/// ------------------------------------------------------------------------------------
		protected override WaveControlBasic CreateWaveControl()
		{
			_waveControl = new WaveControlWithBoundarySelection();

			_buttonCancel.Margin = new Padding(_buttonCancel.Margin.Left,
				_buttonCancel.Margin.Top, Padding.Right, _buttonCancel.Margin.Bottom);
			Padding = new Padding(0, 8, 0, 0);

			_waveControl.BoundaryMoved += HandleSegmentBoundaryMovedInWaveControl;
			_waveControl.BoundaryMouseDown += delegate { UpdateDisplay(); };
			_waveControl.CursorTimeChanged += delegate { UpdateDisplay(); };
			return _waveControl;
		}

		/// ------------------------------------------------------------------------------------
		protected override FormSettings FormSettings
		{
			get { return Settings.Default.ManualSegmenterDlg; }
			set { Settings.Default.ManualSegmenterDlg = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override float ZoomPercentage
		{
			get { return Settings.Default.ZoomPercentageInManualSegmenterDlg; }
			set { Settings.Default.ZoomPercentageInManualSegmenterDlg = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ShouldShadePlaybackAreaDuringPlayback
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int HeightOfTableLayoutButtonRow
		{
			get
			{
				return (_buttonListenToOriginal.Height * 3) + 5 +
					_buttonListenToOriginal.Margin.Top + _buttonListenToOriginal.Margin.Bottom +
					_buttonAddSegmentBoundary.Margin.Top + _buttonAddSegmentBoundary.Margin.Bottom +
					_buttonDeleteSegment.Margin.Top + _buttonDeleteSegment.Margin.Bottom +
					toolStripButtons.Margin.Top + toolStripButtons.Margin.Bottom;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void UpdateDisplay()
		{
			_buttonListenToOriginal.Visible = !_waveControl.IsPlaying;
			_buttonStopOriginal.Visible = _waveControl.IsPlaying;

			if (_newSegmentDefinedBy != SegmentDefinitionMode.AddButtonWhileListening)
				ResetAddSegmentButton(null, null);

			var cursorTime = _waveControl.GetCursorTime();
			_buttonAddSegmentBoundary.Enabled = cursorTime > TimeSpan.Zero;
			if (_buttonAddSegmentBoundary.Enabled)
			{
				var segmentContainingCursor = ViewModel.TimeTier.GetSegmentEnclosingTime((float)cursorTime.TotalSeconds);
				if (segmentContainingCursor != null)
				{
					_buttonAddSegmentBoundary.Enabled = segmentContainingCursor.TimeRange.End != cursorTime &&
						!segmentContainingCursor.GetHasAnyOralAnnotation();
				}
			}
			_buttonDeleteSegment.Enabled = _waveControl.GetSelectedBoundary() > TimeSpan.Zero &&
				!ViewModel.IsBoundaryPermanent(_waveControl.GetSelectedBoundary());

			base.UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected override TimeSpan GetCurrentTimeForTimeDisplay()
		{
			return (_waveControl.GetCursorTime() == TimeSpan.Zero ?
				_waveControl.GetSelectedBoundary() : base.GetCurrentTimeForTimeDisplay());
		}

		/// ------------------------------------------------------------------------------------
		protected override TimeSpan GetBoundaryToAdjustOnArrowKeys()
		{
			return _waveControl.GetSelectedBoundary();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnAdjustBoundaryUsingArrowKey(int milliseconds)
		{
			if (!base.OnAdjustBoundaryUsingArrowKey(milliseconds))
				return false;

			var currBoundary = _waveControl.GetSelectedBoundary() +
				TimeSpan.FromMilliseconds(milliseconds);

			_waveControl.SegmentBoundaries = _viewModel.GetSegmentEndBoundaries()
				.Select(b => b == _timeAtBeginningOfBoundaryMove ? currBoundary : b);

			_waveControl.SetSelectedBoundary(currBoundary);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void FinalizeBoundaryMovedUsingArrowKeys()
		{
			System.Diagnostics.Debug.WriteLine("In ManualSegmenterDlg.FinalizeBoundaryMovedUsingArrowKeys.");

			var newBoundary = _waveControl.GetSelectedBoundary();
			if (ViewModel.SaveBoundaryPositionAfterMovedUsingArrowKeys(_timeAtBeginningOfBoundaryMove, newBoundary))
			{
				base.FinalizeBoundaryMovedUsingArrowKeys();
				PlaybackShortPortionUpToBoundary(newBoundary);
			}
			else
				_waveControl.SetSelectedBoundary(_timeAtBeginningOfBoundaryMove);
		}

		/// ------------------------------------------------------------------------------------
		protected override void PlaybackShortPortionUpToBoundary(WaveControlBasic ctrl,
			TimeSpan time1, TimeSpan time2)
		{
			base.PlaybackShortPortionUpToBoundary(ctrl, time1, time2);
			_waveControl.SetCursor(TimeSpan.FromSeconds(1).Negate());
		}

		#region Low level keyboard handling
		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyDown(Keys key)
		{
			if (!ContainsFocus)
				return true;

			if (key == Keys.Delete)
				_buttonDeleteSegment.PerformClick();
			else if (key == Keys.Space && _waveControl.IsPlaying)
			{
				_justStoppedusingSpace = true;
				_buttonStopOriginal.PerformClick();
			}
			//else if (key == Keys.Left)
			//{
			//    if (_viewModel.MoveExistingSegmentBoundary(_waveControl.GetSelectedBoundary(),
			//        Settings.Default.MillisecondsToBackupSegmentBoundaryOnLeftArrow))
			//    {

			//    }
			//}

			return base.OnLowLevelKeyDown(key);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyUp(Keys key)
		{
			if (!ContainsFocus)
				return true;

			if (key == Keys.Space)
			{
				if (_justStoppedusingSpace)
					_justStoppedusingSpace = false;
				else if (!_waveControl.IsPlaying)
				{
					_buttonListenToOriginal.PerformClick();
					return true;
				}
				return false;
			}

			if (key == Keys.Enter)
			{
				_buttonAddSegmentBoundary.PerformClick();
				return true;
			}

			return base.OnLowLevelKeyUp(key);
		}

		#endregion

		private void toolStripButtons_MouseEnter(object sender, EventArgs e)
		{
			_currentSegmentMenuStrip.Visible = false;
		}
	}
}
