using System.Linq;
using System.Windows.Forms;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class SessionBasicEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public SessionBasicEditor(ComponentFile file)
		{
			InitializeComponent();
			Name = "Basic";
			_binder.SetComponentFile(file);

			if (DiscourseType.AllTypes == null)
				return;

			var discourseTypes = DiscourseType.AllTypes.ToArray();
			_eventType.Items.AddRange(discourseTypes);

			var currType = discourseTypes.FirstOrDefault(x => x.Id == _binder.GetValue("eventType"));
			_eventType.SelectedItem = (currType ?? DiscourseType.UnknownType);
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
