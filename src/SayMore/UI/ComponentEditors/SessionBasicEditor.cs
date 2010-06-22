using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Fields;
using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class SessionBasicEditor : EditorBase
	{
		CustomFieldsGrid _gridCustomFields;

		/// ------------------------------------------------------------------------------------
		public SessionBasicEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			InitializeCustomFieldsGrid();
			Name = "SessionBasicEditor";
			SetBindingHelper(_binder);

			if (DiscourseType.AllTypes == null)
				return;

			var discourseTypes = DiscourseType.AllTypes.ToArray();
			_eventType.Items.AddRange(discourseTypes);

			var currType = discourseTypes.FirstOrDefault(x => x.Id == _binder.GetValue("eventType"));
			_eventType.SelectedItem = (currType ?? DiscourseType.UnknownType);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeCustomFieldsGrid()
		{
			_gridCustomFields = new CustomFieldsGrid();
			_tableLayout.Controls.Add(_gridCustomFields, 0, 11);
			_tableLayout.SetColumnSpan(_gridCustomFields, 2);
			_binder.SetIsBound(_gridCustomFields, true);
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
