using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SilTools;

namespace SayMore.Transcription.UI
{
	public class TextAnnotationEditorGrid : SilGrid
	{
		private AnnotationComponentFile _annotationFile;

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
	}
}
