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

			if (DiscourseType.AllTypes != null)
			{
				var discourseTypes = DiscourseType.AllTypes.ToArray();
				_eventType.Items.AddRange(discourseTypes);

				var currType = discourseTypes.FirstOrDefault(x => x.Id == _binder.GetValue("eventType"));
				_eventType.SelectedItem = (currType ?? DiscourseType.UnknownType);
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
		private bool HandleGetBoundControlValue(BindingHelper helper, Control boundControl, out string newValue)
		{
			newValue = null;

			if (boundControl != _eventType)
				return false;

			newValue = ((DiscourseType)_eventType.SelectedItem).Id;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleIdEnter(object sender, EventArgs e)
		{
			// Makes sure the id's label is also visible when the id field gains focus.
			AutoScrollPosition = new Point(0, 0);
		}
	}
}
