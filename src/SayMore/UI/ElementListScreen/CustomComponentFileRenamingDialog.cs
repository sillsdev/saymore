using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SayMore.UI.ProjectChoosingAndCreating.NewProjectDialog;

namespace SayMore.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	public partial class CustomComponentFileRenamingDialog : Form
	{
		private readonly string _origFilePath;

		/// ------------------------------------------------------------------------------------
		public CustomComponentFileRenamingDialog()
		{
			InitializeComponent();

			_labelPrefix.Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold);
			_labelExtension.Font = _labelPrefix.Font;
			_textBox.Font = SystemFonts.MessageBoxFont;
			_labelMessage.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 8f, GraphicsUnit.Point);
			_labelMessage.Height = _labelMessage.PreferredHeight;
		}

		/// ------------------------------------------------------------------------------------
		public CustomComponentFileRenamingDialog(string id, string origFilePath) : this()
		{
			_labelPrefix.Text = id + "_";
			_labelExtension.Text = Path.GetExtension(origFilePath);
			_origFilePath = origFilePath;
			HandleTextBoxTextChanged(null, null);
		}

		/// ------------------------------------------------------------------------------------
		public string NewFileName
		{
			get { return _labelPrefix.Text + _textBox.Text.Trim() + _labelExtension.Text; }
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTableLayoutSizeChanged(object sender, EventArgs e)
		{
			if (!IsHandleCreated)
				CreateHandle();

			Height = _tableLayout.Height + Padding.Top + Padding.Bottom + (Height - ClientSize.Height);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTextBoxTextChanged(object sender, EventArgs e)
		{
			var newPath = Path.GetDirectoryName(_origFilePath);
			var validMsg = (_textBox.Text.Trim() == string.Empty ? string.Empty :
				string.Format("New file: {0}", Path.Combine(newPath, NewFileName)));

			_buttonOK.Enabled = PathValidator.ValidatePathEntry(Path.GetDirectoryName(_origFilePath),
				NewFileName, _labelMessage, validMsg, "A file by that name already exists.", null);
		}
	}
}
