using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.UI.MediaPlayer;
using SilTools;

namespace SayMore.Transcription.UI
{
	public class TextAnnotationEditorGrid : SilGrid
	{
		public Func<ITimeOrderSegment> SegmentProvider;
		public Func<string> MediaFileProvider;
		public MediaPlayerViewModel PlayerViewModel { get; private set; }

		private AnnotationComponentFile _annotationFile;
		private bool _mediaFileNeedsLoading = true;
		private Action _playbackProgressReportingAction;

		/// ------------------------------------------------------------------------------------
		public TextAnnotationEditorGrid()
		{
			Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			Margin = new Padding(0);
			VirtualMode = true;
			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			AllowUserToResizeRows = false;
			EditMode = DataGridViewEditMode.EditOnEnter;
			FullRowFocusRectangleColor = DefaultCellStyle.SelectionBackColor;
			DefaultCellStyle.SelectionForeColor = DefaultCellStyle.ForeColor;
			DefaultCellStyle.SelectionBackColor =
				ColorHelper.CalculateColor(Color.White, DefaultCellStyle.SelectionBackColor, 140);

			PlayerViewModel = new MediaPlayerViewModel();
			PlayerViewModel.SetVolume(100);
			PlayerViewModel.SetSpeed(Settings.Default.AnnotationEditorPlaybackSpeed);
			PlayerViewModel.Loop = true;

			SetPlaybackProgressReportAction(null);
		}

		/// ------------------------------------------------------------------------------------
		public void Load(AnnotationComponentFile file)
		{
			_annotationFile = file;

			Utils.SetWindowRedraw(this, false);
			RowCount = 0;
			Columns.Clear();

			if (_annotationFile == null)
				return;

			int rowCount = 0;

			foreach (var tier in _annotationFile.Tiers)
				rowCount = Math.Max(rowCount, AddColumnForTier(tier));

			RowCount = rowCount;
			Utils.SetWindowRedraw(this, true);
			Invalidate();

			if (Settings.Default.SegmentGrid != null)
				Settings.Default.SegmentGrid.InitializeGrid(this);

			AutoResizeColumnHeadersHeight();
			ColumnHeadersHeight += 8;

			if (MediaFileProvider != null)
				PlayerViewModel.LoadFile(MediaFileProvider());
		}

		/// ------------------------------------------------------------------------------------
		private int AddColumnForTier(ITier tier)
		{
			Columns.Add(tier.GridColumn);

			var col = tier.GridColumn as TextAnnotationColumn;
			if (col != null)
				col.SegmentChangedAction = _annotationFile.Save;

			foreach (var dependentTier in tier.DependentTiers)
				AddColumnForTier(dependentTier);

			return tier.GetAllSegments().Count();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the user is in a transcription cell, this will intercept the tab and shift+tab
		/// keys so they move to the next transcription cell or previous transcription cell
		/// respectively.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (IsCurrentCellInEditMode && msg.WParam.ToInt32() == (int)Keys.Tab)
			{
				int newRowIndex = CurrentCellAddress.Y + (ModifierKeys == Keys.Shift ? -1 : 1);

				if (newRowIndex >= 0 && newRowIndex < RowCount)
				{
					EndEdit();
					CurrentCell = this[CurrentCell.ColumnIndex, newRowIndex];
				}

				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
		{
			base.OnColumnWidthChanged(e);
			AutoResizeColumnHeadersHeight();
			ColumnHeadersHeight += 8;
			Settings.Default.SegmentGrid = GridSettings.Create(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (ColumnCount > 0)
				return;

			var hint = "There are no transcription annotations found in\n'{0}'";
			DrawMessageInCenterOfGrid(e.Graphics, string.Format(hint,
				Path.GetFileName(_annotationFile.PathToAnnotatedFile)), 0);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
		{
			base.OnCellMouseClick(e);

			var col = Columns[e.ColumnIndex] as TierColumnBase;

			if (e.RowIndex < 0 || e.Button != MouseButtons.Right || col == null)
				return;

			var menuItems = col.GetContextMenuCommands().ToArray();
			if (menuItems.Length == 0)
				return;

			var menu = new ContextMenuStrip();
			menu.Items.AddRange(menuItems.ToArray());
			menu.Show(MousePosition);
		}

		#region Playback methods
		/// ------------------------------------------------------------------------------------
		public void SetPlaybackSpeed(int playbackSpeed)
		{
			if (PlayerViewModel.Speed != playbackSpeed)
				PlayerViewModel.SetSpeed(playbackSpeed);
		}

		/// ------------------------------------------------------------------------------------
		public void SetPlaybackProgressReportAction(Action action)
		{
			_playbackProgressReportingAction = (action ?? (() => { }));
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCurrentRowChanged(EventArgs e)
		{
			Stop();

			if (CurrentCellAddress.Y >= 0)
				_mediaFileNeedsLoading = true;

			base.OnCurrentRowChanged(e);

			if (CurrentCellAddress.Y >= 0 && Focused)
				Play();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);

			if (!PlayerViewModel.HasPlaybackStarted)
				Play();
		}

		/// ------------------------------------------------------------------------------------
		public void Play()
		{
			if (PlayerViewModel.HasPlaybackStarted)
				Stop();

			if (_mediaFileNeedsLoading && MediaFileProvider != null && SegmentProvider != null)
			{
				PlayerViewModel.LoadFile(MediaFileProvider(),
					SegmentProvider().Start, SegmentProvider().GetLength());
			}

			PlayerViewModel.PlaybackStarted -= HandleMediaPlayStarted;
			PlayerViewModel.PlaybackStarted += HandleMediaPlayStarted;
			PlayerViewModel.PlaybackEnded -= HandleMediaPlaybackEnded;
			PlayerViewModel.PlaybackEnded += HandleMediaPlaybackEnded;
			PlayerViewModel.PlaybackPositionChanged = (pos => Invoke(_playbackProgressReportingAction));
			PlayerViewModel.Play();
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			PlayerViewModel.PlaybackStarted -= HandleMediaPlayStarted;
			PlayerViewModel.PlaybackEnded -= HandleMediaPlaybackEnded;
			PlayerViewModel.PlaybackPositionChanged = null;
			PlayerViewModel.Stop();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlaybackEnded(object sender, bool EndedBecauseEOF)
		{
			if (InvokeRequired)
				Invoke(_playbackProgressReportingAction);
			else
				_playbackProgressReportingAction();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlayStarted(object sender, EventArgs e)
		{
			if (InvokeRequired)
				Invoke(_playbackProgressReportingAction);
			else
				_playbackProgressReportingAction();
		}

		#endregion
	}
}
