using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class VideoComponentEditor : EditorBase
	{
		public delegate VideoComponentEditor Factory(ComponentFile file, string tabText, string imageKey);

		private FieldsValuesGrid _grid;
		private FieldsValuesGridViewModel _gridViewModel;
		private IEnumerable<string> _customFieldIds;

		/// ------------------------------------------------------------------------------------
		public VideoComponentEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Video File Information";

			_customFieldIds = fieldGatherer.GetFieldsForType(_file.FileType, AllDefaultFieldIds);
			InitializeGrid(autoCompleteProvider);

			fieldGatherer.NewDataAvailable += HandleNewDataFieldsAvailable;
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeGrid(IMultiListDataProvider autoCompleteProvider)
		{
			_gridViewModel = new FieldsValuesGridViewModel(_file,
				GetDefaultFieldIdsToDisplayInGrid(), _customFieldIds, autoCompleteProvider);

			_grid = new FieldsValuesGrid(_gridViewModel);
			_grid.Dock = DockStyle.Fill;
			Controls.Add(_grid);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			if (_gridViewModel != null)
			{
				_gridViewModel.SetComponentFile(file,
					GetDefaultFieldIdsToDisplayInGrid(), _customFieldIds);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<string> AllDefaultFieldIds
		{
			get
			{
				yield return "Recordist";
				yield return "Device";
				yield return "Microphone";
				yield return "Channels";
				yield return "Bit_Depth";
				yield return "Sample_Rate";
				yield return "Resolution";
				yield return "notes";
			}
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetDefaultFieldIdsToDisplayInGrid()
		{
			// Show all but the notes field in the grid.
			return from id in AllDefaultFieldIds
				   where id != "notes"
				   select id;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleNewDataFieldsAvailable(object sender, EventArgs e)
		{
			_customFieldIds = ((FieldGatherer)sender).GetFieldsForType(_file.FileType,
				AllDefaultFieldIds);
		}
	}
}
