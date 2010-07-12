using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class SessionBasicEditor : EditorBase
	{
		public delegate SessionBasicEditor Factory(ComponentFile file, string tabText, string imageKey);

		private FieldsValuesGrid _gridCustomFields;
		private FieldsValuesGridViewModel _gridViewModel;

		/// ------------------------------------------------------------------------------------
		public SessionBasicEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "SessionBasicEditor";

			InitializeGrid(autoCompleteProvider,fieldGatherer);
			SetBindingHelper(_binder);
			_autoCompleteHelper.SetAutoCompleteProvider(autoCompleteProvider);

			if (GenreDefinition.FactoryGenreDefinitions != null)
			{
				//add the ones in use, factory or otherwise
				_genre.Items.AddRange(autoCompleteProvider.GetValueLists(false)["genre"].ToArray());
				_genre.Items.Add("-----");
				//add the rest of the factory defaults
				_genre.Items.AddRange(GenreDefinition.FactoryGenreDefinitions.ToArray());

				var genre = GenreDefinition.FactoryGenreDefinitions.ToArray().FirstOrDefault(x => x.Id == _binder.GetValue("genre"));
				_genre.SelectedItem = (genre ?? GenreDefinition.UnknownType);

			}
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeGrid(IMultiListDataProvider autoCompleteProvider,
			FieldGatherer fieldGatherer)
		{
			_gridViewModel = new FieldsValuesGridViewModel(_file, autoCompleteProvider,
				fieldGatherer, key => _file.FileType.GetIsCustomFieldId(key));

			_gridCustomFields = new FieldsValuesGrid(_gridViewModel);
			_gridCustomFields.Dock = DockStyle.Top;
			_panelGrid.AutoSize = true;
			_panelGrid.Controls.Add(_gridCustomFields);
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
		/// Provide special handling for persisting the value of the event type combo.
		/// </summary>
		/// ------------------------------------------------------------------------------------
//		private bool GetComboBoxValue(BindingHelper helper, Control boundControl, out string newValue)
//		{
//			newValue = null;
//
//			if (boundControl != _genre)
//				return false;
//
			//newValue = ((DiscourseType)_genre.SelectedItem).Id;
//			newValue = _genre.Text;
//			return true;
//		}

		/// ------------------------------------------------------------------------------------
		private void HandleIdEnter(object sender, EventArgs e)
		{
			// Makes sure the id's label is also visible when the id field gains focus.
			AutoScrollPosition = new Point(0, 0);
		}

	}
}
