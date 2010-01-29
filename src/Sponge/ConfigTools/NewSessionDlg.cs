using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;

namespace SIL.Sponge.ConfigTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Dialog for allowing user to enter the name of a new session.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class NewSessionDlg : Form
	{
		private readonly string m_projectPath;
		private readonly HashSet<string> m_sessionFiles = new HashSet<string>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NewSessionDlg"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionDlg()
		{
			InitializeComponent();
			btnOK.Enabled = false;
			lblPath.Text = string.Empty;
			LocalizeItemDlg.StringsLocalized += SetLocationMsg;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NewSessionDlg"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionDlg(string prjPath) : this()
		{
			m_projectPath = prjPath;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the TextChanged event of the txtSessionName control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtName_TextChanged(object sender, EventArgs e)
		{
			SetLocationMsg();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the message under the project name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetLocationMsg()
		{
			var invalidPathMsg = LocalizationManager.LocalizeString(
				"NewSessionDlg.lblPath.InvalidPathMsg", "Unable to create a new session there.",
				"This text is displayed under the session name when it is invalid.", "Dialog Boxes");

			var validPathMsg = LocalizationManager.GetString(lblPath);

			btnOK.Enabled = PathValidator.ValidatePathEntry(m_projectPath,
				txtName.Text.Trim(), lblPath, validPathMsg, invalidPathMsg, toolTip);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			LocalizeItemDlg.StringsLocalized -= SetLocationMsg;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the path to new project directory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NewSessionName
		{
			get { return txtName.Text.Trim(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the files added to the session.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] SessionFiles
		{
			get { return m_sessionFiles.ToArray(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make the label underneath the name text box only as tall as necessary.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lblPath_TextChanged(object sender, EventArgs e)
		{
			using (var g = lblPath.CreateGraphics())
			{
				var sz = new Size(lblPath.Width, 0);
				sz = TextRenderer.MeasureText(g, lblPath.Text, lblPath.Font, sz, TextFormatFlags.WordBreak);
				if (lblPath.Height != sz.Height && lblPath.Bottom < btnCopyFiles.Top)
					lblPath.Height = sz.Height;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the Click event of the btnCopyFiles control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCopyFiles_Click(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				var caption = LocalizationManager.LocalizeString(
					"NewSessionDlg.OpenFileDlgCaption", "Copy Files into Session", "Dialog Boxes");

				dlg.Title = caption;
				dlg.Filter = Sponge.OFDlgAllFileTypeText + "|*.*";
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = true;
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					foreach (string file in dlg.FileNames)
						m_sessionFiles.Add(file);
				}
			}
		}
	}
}
