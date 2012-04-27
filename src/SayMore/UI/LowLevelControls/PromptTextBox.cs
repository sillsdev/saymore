using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SayMore.Utilities.LowLevelControls
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
				Font = new Font(SystemFonts.MenuFont,
					(SystemFonts.MenuFont.FontFamily.IsStyleAvailable(FontStyle.Italic) ?
					FontStyle.Italic : SystemFonts.MenuFont.Style))
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
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string PromptText
		{
			get { return _labelPrompt.Text; }
			set
			{
				_labelPrompt.Text = value;
				ManagePromptDisplay();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ManagePromptDisplay()
		{
			var promptVisible = (Text.Length == 0 && !Focused && !string.IsNullOrEmpty(_labelPrompt.Text));
			if (_labelPrompt.Visible != promptVisible)
				_labelPrompt.Visible = promptVisible;
		}
	}
}
