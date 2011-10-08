using System;
using System.Windows.Forms;
using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class NotesEditor : EditorBase
	{
		private string _origTabText;

		/// ------------------------------------------------------------------------------------
		public NotesEditor(ComponentFile file) : base(file, null, "Notes")
		{
			InitializeComponent();
			Name = "Notes";
			_binder.SetComponentFile(file);
			SetBindingHelper(_binder);

			_notes.KeyDown += HandleNotesTextBoxKeyDown;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleNotesTextChanged(object sender, EventArgs e)
		{
			TabText = (_notes.Text.Trim() == string.Empty ?
				string.Format("({0})", _origTabText) : _origTabText);
		}

		/// ------------------------------------------------------------------------------------
		private static void HandleNotesTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Pause && e.Alt)
				throw new ApplicationException("User-invoked test crash.");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			_origTabText = TabText = Program.GetString("CommonToMultipleViews.NotesEditor.TabText", "Notes");
			base.HandleStringsLocalized();
		}
	}
}
