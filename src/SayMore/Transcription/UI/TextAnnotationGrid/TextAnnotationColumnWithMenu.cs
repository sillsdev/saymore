using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using Palaso.Reporting;
using SayMore.Transcription.Model;

// ReSharper disable once CheckNamespace
namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public class TextAnnotationColumnWithMenu : TextAnnotationColumn
	{
		public AudioRecordingType PlaybackType { get; protected set; }

		private ContextMenuStrip _columnMenu;

		/// ------------------------------------------------------------------------------------
		public TextAnnotationColumnWithMenu(TierBase tier) : base(tier)
		{
			SortMode = DataGridViewColumnSortMode.Programmatic;
			HeaderCell.Style.Font = new Font(DefaultCellStyle.Font, FontStyle.Bold);
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

			_columnMenu.PerformLayout();

			var rc = _grid.GetCellDisplayRectangle(Index, -1, false);
			var pt = _grid.PointToScreen(new Point(rc.Right - _columnMenu.Width, rc.Bottom));

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
			if (menuItem != null) PlaybackType = (AudioRecordingType)menuItem.Tag;
			_grid.Play();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (DataGridView == null || e.RowIndex >= 0 || e.ColumnIndex != Index)
				return;

			var headerTextWidth = TextRenderer.MeasureText(e.Graphics, HeaderText, e.CellStyle.Font).Width;
			//HeaderCell.SortGlyphDirection = SortOrder.Descending;
			e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

			var rect = e.CellBounds;
			rect.X += headerTextWidth;
			rect.Width -= headerTextWidth;

			var arrow = Properties.Resources.DropDownArrow;
			var glyphWidth = arrow.Width;
			if (rect.Width > glyphWidth)
			{
				e.Graphics.DrawImage(arrow, new Rectangle(rect.Right - glyphWidth - 1, rect.Top + (rect.Height - arrow.Height) / 2, glyphWidth, arrow.Height));
				rect.Width -= (glyphWidth + 4);
			}
			using (var optionsFont = new Font(e.CellStyle.Font, FontStyle.Regular))
			{
				var optionsText = LocalizationManager.GetString("SessionsView.Transcription.OptionsMenu", "Options");
				if (rect.Width > TextRenderer.MeasureText(e.Graphics, optionsText, optionsFont).Width)
				{
					rect.Height--;
					TextRenderer.DrawText(e.Graphics, optionsText, optionsFont, rect,
						e.CellStyle.ForeColor, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
				}
			}
			e.Handled = true;
		}

		/// ------------------------------------------------------------------------------------
		public override void InitializeColumnContextMenu()
		{
			// SP-755: Crash reloading file after editing it in an external editor
			//if (_columnMenu != null)
			//    throw new InvalidOperationException("InitializeColumnContextMenu should only be called once");
			if (_columnMenu != null) return;

			_columnMenu = new ContextMenuStrip();
			var playbackOptionMenus = GetPlaybackOptionMenus();
			_columnMenu.Items.AddRange(playbackOptionMenus.Cast<ToolStripItem>().ToArray());
			_columnMenu.Items.Add(new ToolStripSeparator());

			// add "Fonts..." as the last menu item
			var menuItem = new ToolStripMenuItem(null, null, HandleFontMenuItemClicked);
			menuItem.Text = LocalizationManager.GetString(
			   "SessionsView.Transcription.FontsMenu",
			   "&Fonts...", null, menuItem);

			_columnMenu.Items.Add(menuItem);
			_columnMenu.ShowCheckMargin = true;
			_columnMenu.ShowImageMargin = false;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFontMenuItemClicked(object sender, EventArgs e)
		{
			using (var dlg = new FontDialog())
			{
				dlg.Font = (DefaultCellStyle.Font);

				try //strange, but twice we've found situations where ShowDialog crashes on windows
				{
					if (DialogResult.OK != dlg.ShowDialog())
					{
						return;
					}
				}
				catch (Exception)
				{
					ErrorReport.NotifyUserOfProblem(LocalizationManager.GetString("SessionsView.Transcription.FontDialogProblem",
						"There was some problem with choosing that font. If you just installed it, you might try restarting the program or even your computer."));
					return;
				}

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
