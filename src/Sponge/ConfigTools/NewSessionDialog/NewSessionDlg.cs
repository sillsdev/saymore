using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
		private readonly NewSessionDlgViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionDlg()
		{
			InitializeComponent();
			_OKButton.Enabled = false;
			_newSessionPathLabel.Text = string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionDlg(NewSessionDlgViewModel viewModel) : this()
		{
			_viewModel = viewModel;
			_idTextBox.Text = _viewModel.DefaultNewSessionId;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);

			// Do this to update the message label if we've just come back
			// from the localization dialog box.
			txtName_TextChanged(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtName_TextChanged(object sender, EventArgs e)
		{
			_OKButton.Enabled = _viewModel.IsNewSessionIdValid(_idTextBox.Text.Trim(), _newSessionPathLabel);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCopyFiles_Click(object sender, EventArgs e)
		{
			_viewModel.LetUserChooseSessionFiles();
		}
	}
}
