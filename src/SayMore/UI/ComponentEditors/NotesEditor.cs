using L10NSharp;
using SayMore.Model.Files;
using SayMore.Utilities;
using SIL.Windows.Forms.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;
using static SIL.Windows.Forms.Extensions.ControlExtensions.ErrorHandlingAction;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class NotesEditor : EditorBase
	{
		private string _origTabText;

		/// ------------------------------------------------------------------------------------
		public NotesEditor(ComponentFile file) : base(file, "Notes")
		{
			InitializeComponent();
			Name = "Notes";

			// _notes holds working-language content; its font is owned by
			// SetWorkingLanguageFont, which keeps it at the current UI text scale.
			UITextScaler.ExcludeFromUIFontScaling(_notes);

			_binder.SetComponentFile(file);
			SetBindingHelper(_binder);

			_notes.KeyDown += HandleNotesTextBoxKeyDown;

			NotifyWhenProjectIsSet();
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetWorkingLanguageFont(Font font)
		{
			this.SafeInvoke(() => { _notes.Font = UITextScaler.GetScaledWorkingLanguageFont(font); },
				$"{GetType().Name}.{nameof(SetWorkingLanguageFont)}", IgnoreAll);
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
				TabText = $"({_origTabText})";
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
		protected override void HandleStringsLocalized(object sender, EventArgs e)
		{
			_origTabText = TabText = LocalizationManager.GetString(
				"CommonToMultipleViews.NotesEditor.TabText", "Notes");

			base.HandleStringsLocalized(sender, e);
		}

		private void NotesEditor_Load(object sender, EventArgs e)
		{
			UpdateNotesTabLabel();
		}
	}
}
