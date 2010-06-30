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
	public partial class SessionBasicEditor : EditorBase
	{
		public delegate SessionBasicEditor Factory(ComponentFile file, string tabText, string imageKey);

		private FieldsValuesGrid _gridCustomFields;
		private FieldsValuesGridViewModel _gridViewModel;
		private IEnumerable<string> _customFieldIds;

		/// ------------------------------------------------------------------------------------
		public SessionBasicEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "SessionBasicEditor";

			_customFieldIds = fieldGatherer.GetFieldsForType(_file.FileType, GetAllDefaultFieldIds());
			InitializeGrid(autoCompleteProvider);
			SetBindingHelper(_binder);
			_autoCompleteHelper.SetAutoCompleteProvider(autoCompleteProvider);
			fieldGatherer.NewDataAvailable += HandleNewDataFieldsAvailable;

			if (DiscourseType.AllTypes != null)
			{
				var discourseTypes = DiscourseType.AllTypes.ToArray();
				_eventType.Items.AddRange(discourseTypes);

				var currType = discourseTypes.FirstOrDefault(x => x.Id == _binder.GetValue("eventType"));
				_eventType.SelectedItem = (currType ?? DiscourseType.UnknownType);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeGrid(IMultiListDataProvider autoCompleteProvider)
		{
			_gridViewModel = new FieldsValuesGridViewModel(_file, new List<string>(0),
				_customFieldIds, autoCompleteProvider);

			_gridCustomFields = new FieldsValuesGrid(_gridViewModel);
			_gridCustomFields.Dock = DockStyle.Top;
			_gridCustomFields.Margin = _situation.Margin;
			_tableLayout.Controls.Add(_gridCustomFields, 0, 11);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			if (_gridViewModel != null)
				_gridViewModel.SetComponentFile(file, _customFieldIds);
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<string> GetAllDefaultFieldIds()
		{
			yield return "date";
			yield return "synopsis";
			yield return "access";
			yield return "location";
			yield return "setting";
			yield return "situation";
			yield return "eventType";
			yield return "participants";
			yield return "title";
			yield return "notes";
		}

		/// ------------------------------------------------------------------------------------
		private void HandleNewDataFieldsAvailable(object sender, EventArgs e)
		{
			_customFieldIds = ((FieldGatherer)sender).GetFieldsForType(_file.FileType,
				GetAllDefaultFieldIds());
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
	}
}
