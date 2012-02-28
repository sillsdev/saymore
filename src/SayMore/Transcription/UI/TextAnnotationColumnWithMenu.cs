using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public class TextAnnotationColumnWithMenu : TextAnnotationColumn
	{
		public OralAnnotationType PlaybackType { get; protected set; }
		public Action<OralAnnotationType> AnnotationPlaybackTypeChangedAction { get; set; }

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

			var menu = new ContextMenuStrip();
			menu.Items.AddRange(GetPlaybackOptionMenus().ToArray());
			menu.ShowCheckMargin = true;
			menu.ShowImageMargin = false;

			var rc = _grid.GetCellDisplayRectangle(Index, -1, false);
			var pt = _grid.PointToScreen(new Point(rc.Left, rc.Bottom));
			menu.Show(pt);
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
			PlaybackType = (OralAnnotationType)menuItem.Tag;

			if (AnnotationPlaybackTypeChangedAction != null)
				AnnotationPlaybackTypeChangedAction(PlaybackType);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			HeaderCell.SortGlyphDirection = SortOrder.Descending;
			_grid.CellPainting -= HandleGridCellPainting;
		}
	}
}
