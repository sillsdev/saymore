using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class NotesEditor : EditorBase
	{
		private readonly string _origTabText;

		/// ------------------------------------------------------------------------------------
		public NotesEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			_origTabText = tabText;

			InitializeComponent();
			Name = "Notes";
			_binder.SetComponentFile(file);
			SetBindingHelper(_binder);

		}

		/// ------------------------------------------------------------------------------------
		private void HandleNotesTextChanged(object sender, System.EventArgs e)
		{
			TabText = (_notes.Text.Trim() == string.Empty ?
				string.Format("({0})", _origTabText) : _origTabText);
		}
	}
}
