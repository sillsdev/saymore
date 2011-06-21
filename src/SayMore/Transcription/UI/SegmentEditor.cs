using System;
using System.Linq;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class SegmentEditor : EditorBase
	{
		public delegate SegmentEditor Factory(ComponentFile file, string tabText, string imageKey);

		private readonly SegmentEditorGrid _grid;

		/// ------------------------------------------------------------------------------------
		public SegmentEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Segments";
			_grid = new SegmentEditorGrid();
			_tableLayout.Controls.Add(_grid, 0, 0);
			SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);
			file.Load();
			LoadGrid(file as TranscriptionComponentFile);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadGrid(TranscriptionComponentFile file)
		{
			Utils.SetWindowRedraw(_grid, false);
			_grid.RowCount = 0;
			_grid.Columns.Clear();

			if (file == null)
				return;

			int rowCount = 0;

			foreach (var tier in file.Tiers)
			{
				_grid.Columns.Add(tier.GridColumn);
				rowCount = Math.Max(rowCount, tier.GetAllSegments().Count());

				var col = tier.GridColumn as TextTranscriptionColumn;
				if (col != null)
					col.SegmentChangedAction = file.Save;
			}

			_grid.RowCount = rowCount;
			Utils.SetWindowRedraw(_grid, true);
			_grid.Invalidate();

			if (Settings.Default.SegmentGrid != null)
				Settings.Default.SegmentGrid.InitializeGrid(_grid);
		}
	}
}
