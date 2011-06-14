using System;
using System.Linq;
using System.Windows.Forms;
using Localization;
using SayMore.Model.Files;
using SayMore.Transcription.Model;
using SayMore.UI.ComponentEditors;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class SegmentEditor : EditorBase
	{
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

			if (!file.GetCanHaveTranscriptionFile())
				return;

		// do something to read file, if there is one.
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsOKSToShow
		{
			get { return _file.GetCanHaveTranscriptionFile(); }
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLoadSegmentFileClick(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				var caption = LocalizationManager.LocalizeString(
					"SegmentEditor.LoadSegmentFileDlgCaption", "Select Segment File");

				dlg.Title = caption;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;

				// TODO: Add ELAN .eaf files.

				dlg.Filter = "Audacity Label File (*.txt)|*.txt|All Files (*.*)|*.*";

				if (dlg.ShowDialog(this) != DialogResult.OK)
					return;

				CreateTierColumns(new AudacityLabelFile(dlg.FileName, _file.PathToAnnotatedFile));
			}
		}

		/// ------------------------------------------------------------------------------------
		private void CreateTierColumns(AudacityLabelFile alf)
		{
			Utils.SetWindowRedraw(_grid, false);
			_grid.RowCount = 0;
			_grid.Columns.Clear();
			int rowCount = 0;

			foreach (var tier in alf.GetTiers(_file.PathToAnnotatedFile))
			{
				_grid.Columns.Add(tier.GridColumn);
				rowCount = Math.Max(rowCount, tier.GetAllSegments().Count());
			}

			_grid.RowCount = rowCount;
			Utils.SetWindowRedraw(_grid, true);
		}
	}
}
