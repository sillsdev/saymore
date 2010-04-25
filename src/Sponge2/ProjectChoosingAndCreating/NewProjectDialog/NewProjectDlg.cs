using System;
using System.Windows.Forms;

namespace Sponge2.ProjectChoosingAndCreating.NewProjectDialog
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Dialog for allowing user to enter the name of a new project.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class NewProjectDlg : Form
	{
		private readonly NewProjectDlgViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewProjectDlg()
		{
			InitializeComponent();
			_OKButton.Enabled = false;
			_newProjectPathLabel.Text = string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewProjectDlg(NewProjectDlgViewModel viewModel) : this()
		{
			_viewModel = viewModel;
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
		protected void txtName_TextChanged(object sender, EventArgs e)
		{
			_OKButton.Enabled = _viewModel.IsNewProjectNameValid(
				_newNameTextBox.Text.Trim(), _newProjectPathLabel);
		}
	}
}