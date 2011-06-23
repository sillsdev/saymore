using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class TextAnnotationEditor : EditorBase
	{
		public delegate TextAnnotationEditor Factory(ComponentFile file, string tabText, string imageKey);

		private readonly TextAnnotationEditorGrid _grid;

		/// ------------------------------------------------------------------------------------
		public TextAnnotationEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Segments";
			_grid = new TextAnnotationEditorGrid();
			_tableLayout.Controls.Add(_grid, 0, 0);
			SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			//file.PostFileCommandAction = (() => CheckIfElanHasFile(file as AnnotationComponentFile));
			file.Load();
			LoadGrid(file as AnnotationComponentFile);
		}

		//[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		//internal static extern int GetWindowText (IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount );

		//private void CheckIfElanHasFile(AnnotationComponentFile file)
		//{
		//    var elan = Process.GetProcesses().SingleOrDefault(p => p.ProcessName.ToLower() == "elan");
		//    if (elan == null)
		//        return;

		//    var sb = new StringBuilder(256);
		//    GetWindowText(elan.MainWindowHandle, sb, sb.Capacity);
		//}

		/// ------------------------------------------------------------------------------------
		private void LoadGrid(AnnotationComponentFile file)
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

				var col = tier.GridColumn as TextAnnotationColumn;
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
