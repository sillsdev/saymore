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
		private readonly PathValidator _pathValidator;

		/// ------------------------------------------------------------------------------------
		public CustomComponentFileRenamingDialog()
		{
			InitializeComponent();

			_labelPrefix.Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold);
			_labelExtension.Font = _labelPrefix.Font;
			_textBox.Font = SystemFonts.MessageBoxFont;
			_labelMessage.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 8f, GraphicsUnit.Point);
			_labelMessage.Height = _labelMessage.PreferredHeight;

			_pathValidator = new PathValidator(_labelMessage, null)
			{
				InvalidMessage = "A file by that name already exists or the name is invalid."
			};
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
		/// <summary>
		/// New file path, including the file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NewFilePath
		{
			get { return Path.Combine(Path.GetDirectoryName(_origFilePath), NewFileName); }
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
			var validMsg = (_textBox.Text.Trim() == string.Empty ?
				string.Empty : string.Format("New file: {0}", NewFilePath));

			_buttonOK.Enabled = _pathValidator.IsPathValid(
				Path.GetDirectoryName(_origFilePath), NewFileName, validMsg);
		}
	}
}
