using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SIL.Windows.Forms;

namespace SayMore.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// TextBox class that displays a faintly visible prompt when the text box contains no text.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PromptTextBox : TextBox
	{
		private readonly Label _labelPrompt;

		/// ------------------------------------------------------------------------------------
		public PromptTextBox()
		{
			_labelPrompt = new Label
			{
				Text = "Put Prompt Here",
				Visible = true,
				AutoSize = false,
				Dock = DockStyle.Fill,
				TextAlign = ContentAlignment.MiddleLeft,
				BackColor = Color.Transparent,
				ForeColor = SystemColors.GrayText,
				Font = FontHelper.MakeFont(Program.DialogFont, FontStyle.Italic)
			};

			_labelPrompt.Click += HandlePromptLabelClick;

			Controls.Add(_labelPrompt);
		}

		/// ------------------------------------------------------------------------------------
		void HandlePromptLabelClick(object sender, EventArgs e)
		{
			Focus();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			ManagePromptDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			ManagePromptDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			ManagePromptDisplay();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Supplies the prompt text, re-evaluated every time the prompt might be (re)shown,
		/// so it reflects a UI language change made after this was set.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Func<string> _getPromptText;

		[Browsable(false)]
		public Func<string> GetPromptText
		{
			get { return _getPromptText; }
			set
			{
				_getPromptText = value;
				ManagePromptDisplay();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Re-evaluates whether the prompt should be shown right now (based on current text
		/// and focus state) and refreshes its text from GetPromptText if set. Useful when
		/// something outside the normal Enter/Leave/TextChanged path may have desynced the
		/// prompt's visibility from whether this control is actually focused - e.g. a parent
		/// container being disabled while this control had focus, which silently steals focus
		/// away without raising Leave.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshPromptDisplay()
		{
			ManagePromptDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void ManagePromptDisplay()
		{
			if (GetPromptText != null)
				_labelPrompt.Text = GetPromptText();

			_labelPrompt.Visible = Text.Length == 0 && !Focused &&
				!string.IsNullOrEmpty(_labelPrompt.Text);
		}
	}
}
