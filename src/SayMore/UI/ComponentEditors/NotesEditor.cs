using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class NotesEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public NotesEditor(ComponentFile file)
		{
			InitializeComponent();
			Name = "Notes";
			_binder.SetComponentFile(file);
		}
	}
}
