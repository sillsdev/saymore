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
		FieldsValuesGrid _gridCustomFields;
		public delegate SessionBasicEditor Factory(ComponentFile file, string tabText, string imageKey);

		/// ------------------------------------------------------------------------------------
		public SessionBasicEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			InitializeFieldsValuesGrid();
			Name = "SessionBasicEditor";
			SetBindingHelper(_binder);

			_autoCompleteHelper.SetAutoCompleteProvider(autoCompleteProvider);

			if (DiscourseType.AllTypes == null)
				return;

			var discourseTypes = DiscourseType.AllTypes.ToArray();
			_eventType.Items.AddRange(discourseTypes);

			var currType = discourseTypes.FirstOrDefault(x => x.Id == _binder.GetValue("eventType"));
			_eventType.SelectedItem = (currType ?? DiscourseType.UnknownType);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeFieldsValuesGrid()
		{
			var model = new FieldsValuesGridViewModel(_file, GetCustomFieldIdsToDisplayInGrid());
			_gridCustomFields = new FieldsValuesGrid(model);
			_tableLayout.Controls.Add(_gridCustomFields, 0, 11);
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
