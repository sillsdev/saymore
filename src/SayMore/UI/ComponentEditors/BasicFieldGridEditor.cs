using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class BasicFieldGridEditor : EditorBase
	{
		public delegate BasicFieldGridEditor Factory(ComponentFile file, string tabText, string imageKey);

		private FieldsValuesGrid _grid;
		private FieldsValuesGridViewModel _gridViewModel;

		/// ------------------------------------------------------------------------------------
		public BasicFieldGridEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "BasicFieldGridEditor";
			InitializeGrid(autoCompleteProvider,fieldGatherer);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeGrid(IMultiListDataProvider autoCompleteProvider, FieldGatherer fieldGatherer)
		{
			_gridViewModel = new FieldsValuesGridViewModel(_file,autoCompleteProvider,fieldGatherer);

			_grid = new FieldsValuesGrid(_gridViewModel);
			_grid.Dock = DockStyle.Fill;
			Controls.Add(_grid);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			if (_gridViewModel != null)
				_gridViewModel.SetComponentFile(file);
		}
	}
}
