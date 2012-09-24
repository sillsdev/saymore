using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Localization;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public class TextAnnotationColumnWithMenu : TextAnnotationColumn
	{
		public AudioRecordingType PlaybackType { get; protected set; }
		//public Action<OralAnnotationType> AnnotationPlaybackTypeChangedAction { get; set; }

		private ContextMenuStrip _columnMenu;

		/// ------------------------------------------------------------------------------------
		public TextAnnotationColumnWithMenu(TierBase tier) : base(tier)
		{
			SortMode = DataGridViewColumnSortMode.Programmatic;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnDataGridViewChanged()
		{
			if (_grid != null)
				_grid.ColumnHeaderMouseClick -= HandleColumnHeaderMouseClick;

			base.OnDataGridViewChanged();

			if (_grid != null && !_grid.IsDisposed && !_grid.Disposing)
			{
				_grid.CellPainting += HandleGridCellPainting;
				_grid.ColumnHeaderMouseClick += HandleColumnHeaderMouseClick;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex != Index)
				return;

			_grid.Stop();
			var audioCol = _grid.Columns.Cast<DataGridViewColumn>().FirstOrDefault(c => c is AudioWaveFormColumn);
			if (audioCol != null)
				_grid.InvalidateCell(audioCol.Index, _grid.CurrentCellAddress.Y);

			foreach (var menuItem in _columnMenu.Items.OfType<ToolStripMenuItem>().Where(m => m.Tag is AudioRecordingType))
				menuItem.Checked = ((AudioRecordingType)menuItem.Tag == PlaybackType);

			var rc = _grid.GetCellDisplayRectangle(Index, -1, false);
			var pt = _grid.PointToScreen(new Point(rc.Left, rc.Bottom));
			_columnMenu.Show(pt);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual IEnumerable<ToolStripMenuItem> GetPlaybackOptionMenus()
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandlePlaybackTypeMenuItemClicked(object sender, EventArgs e)
		{
			var menuItem = sender as ToolStripMenuItem;
			PlaybackType = (AudioRecordingType)menuItem.Tag;
			_grid.Play();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (DataGridView == null)
				return;

			HeaderCell.SortGlyphDirection = SortOrder.Descending;
			_grid.CellPainting -= HandleGridCellPainting;
		}

		/// ------------------------------------------------------------------------------------
		public override void InitializeColumnContextMenu()
		{
			if (_columnMenu != null)
				throw new InvalidOperationException("InitializeColumnContextMenu should only be called once");
			_columnMenu = new ContextMenuStrip();
			var playbackOptionMenus = GetPlaybackOptionMenus();
			_columnMenu.Items.AddRange(playbackOptionMenus.Cast<ToolStripItem>().ToArray());
			_columnMenu.Items.Add(new ToolStripSeparator());
			_columnMenu.Items.Add(new ToolStripMenuItem(LocalizationManager.GetString("SessionsView.Transcription.FontsMenu", "&Fonts..."),
				null, HandleFontMenuItemClicked));
			_columnMenu.ShowCheckMargin = true;
			_columnMenu.ShowImageMargin = false;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFontMenuItemClicked(object sender, EventArgs e)
		{
			using (var dlg = new FontDialog())
			{
				dlg.Font = (DefaultCellStyle.Font);
				if (dlg.ShowDialog() != DialogResult.OK)
					return;

				SetFont(dlg.Font);
				if (_grid.EditingControl != null)
					_grid.EditingControl.Font = dlg.Font;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void SetFont(Font newFont)
		{
			DefaultCellStyle.Font = newFont;
		}
	}
}
