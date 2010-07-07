using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class NotesEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public NotesEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Notes";
			_binder.SetComponentFile(file);
			SetBindingHelper(_binder);
		}
	}
}
