using System.Windows.Forms;
using L10NSharp;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class BasicFieldGridEditor : EditorBase
	{
		public delegate BasicFieldGridEditor Factory(ComponentFile file, string imageKey);

		private FieldsValuesGrid _grid;
		private FieldsValuesGridViewModel _gridViewModel;

		/// ------------------------------------------------------------------------------------
		public BasicFieldGridEditor(ComponentFile file, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "BasicFieldGridEditor";
			InitializeGrid(autoCompleteProvider,fieldGatherer);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void InitializeGrid(IMultiListDataProvider autoCompleteProvider,
			FieldGatherer fieldGatherer)
		{
			_gridViewModel = new FieldsValuesGridViewModel(_file, autoCompleteProvider,
				fieldGatherer, key => key != "notes");

			_grid = new FieldsValuesGrid(_gridViewModel, "BasicFieldGridEditor._grid");
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized(ILocalizationManager lm)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
				TabText = GetPropertiesTabText();

			base.HandleStringsLocalized(lm);
		}
	}
}
