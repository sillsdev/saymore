using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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
		private FileSystemWatcher _watcher;

		/// ------------------------------------------------------------------------------------
		public TextAnnotationEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Annotations";
			_grid = new TextAnnotationEditorGrid();
			_grid.Dock = DockStyle.Fill;
			Controls.Add(_grid);
			SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			file.Load();
			LoadGrid();
			SetupWatchingForFileChanges();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadGrid()
		{
			Utils.SetWindowRedraw(_grid, false);
			_grid.RowCount = 0;
			_grid.Columns.Clear();

			var file = _file as AnnotationComponentFile;

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

		#region Methods for tracking changes to the EAF file outside of SayMore
		/// ------------------------------------------------------------------------------------
		void SetupWatchingForFileChanges()
		{
			_watcher = new FileSystemWatcher(
				Path.GetDirectoryName(_file.PathToAnnotatedFile),
				Path.GetFileName(_file.PathToAnnotatedFile));

			_watcher.IncludeSubdirectories = false;
			_watcher.EnableRaisingEvents = true;
			_watcher.Changed += HandleAnnotationFileChanged;

			((AnnotationComponentFile)_file).PreSaveAction = () =>
			{
				if (_watcher != null)
					_watcher.EnableRaisingEvents = false;
			};

			((AnnotationComponentFile)_file).PostSaveAction = () =>
			{
				if (_watcher != null)
					_watcher.EnableRaisingEvents = true;
			};
		}

		/// ------------------------------------------------------------------------------------
		void HandleAnnotationFileChanged(object sender, FileSystemEventArgs e)
		{
			Invoke(new EventHandler((s, args) =>
			{
				_file.Load();
				LoadGrid();
			}));
		}

		/// ------------------------------------------------------------------------------------
		public override void Deactivate()
		{
			((AnnotationComponentFile)_file).PostSaveAction = null;
			((AnnotationComponentFile)_file).PostSaveAction = null;

			if (_watcher != null)
			{
				_watcher.Changed -= HandleAnnotationFileChanged;
				_watcher.Dispose();
				_watcher = null;
			}
		}

		#endregion
	}
}
