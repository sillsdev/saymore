using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
using SIL.Sponge.Model;

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
		private void txtSessionName_TextChanged(object sender, EventArgs e)
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
				txtSessionName.Text.Trim(), lblPath, validPathMsg, invalidPathMsg, toolTip);
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
			get { return txtSessionName.Text.Trim(); }
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
				if (lblPath.Height != sz.Height && lblPath.Bottom < btnOK.Top)
					lblPath.Height = sz.Height;
			}
		}
	}
}
