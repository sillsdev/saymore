using System;
using System.Windows.Forms;
using L10NSharp;
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
			UpdateNotesTabLabel();
		}

		private void UpdateNotesTabLabel()
		{
			if (_notes.Text.Contains("**"))
			{
				TabText = "**" + _origTabText;
			}
			else if (_notes.Text.Trim() == string.Empty)
			{
				TabText = string.Format("({0})", _origTabText);
			}
			else
			{
				TabText = _origTabText;
			}
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
		protected override void HandleStringsLocalized(ILocalizationManager lm)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
			{
				_origTabText = TabText = LocalizationManager.GetString(
					"CommonToMultipleViews.NotesEditor.TabText", "Notes");
			}

			base.HandleStringsLocalized(lm);
		}

		private void NotesEditor_Load(object sender, EventArgs e)
		{
			UpdateNotesTabLabel();
		}
	}
}
