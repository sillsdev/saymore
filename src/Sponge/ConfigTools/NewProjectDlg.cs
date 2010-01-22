using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;

namespace SIL.Sponge.ConfigTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class NewProjectDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NewProjectDlg"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewProjectDlg()
		{
			InitializeComponent();
			btnOK.Enabled = false;
			lblPath.Text = string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the TextChanged event of the txtProjectName control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void txtProjectName_TextChanged(object sender, EventArgs e)
		{
			btnOK.Enabled = NameLooksOk;
			if (btnOK.Enabled)
			{
				string[] dirs = PathOfNewProject.Split(Path.DirectorySeparatorChar);
				if (dirs.Length > 1)
				{
					string root = Path.Combine(dirs[dirs.Length - 3], dirs[dirs.Length - 2]);
					lblPath.Text = string.Format("Project will be created in: {0}",
					Path.Combine(root, dirs[dirs.Length - 1]));
				}

				lblPath.Invalidate();
			}
			else
			{
				lblPath.Text = (txtProjectName.Text.Length > 0 ?
					"Unable to create a new project there." : string.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the name is OK.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool NameLooksOk
		{
			get
			{
				//http://regexlib.com/Search.aspx?k=file+name
				//Regex legalFilePattern = new Regex(@"(.*?)");
				//   if (!(legalFilePattern.IsMatch(_textProjectName.Text)))
				//   {
				//   return false;
				//   }

				if (txtProjectName.Text.Trim().Length < 1)
					return false;

				if (txtProjectName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
					return false;

				if (Directory.Exists(PathOfNewProject) || File.Exists(PathOfNewProject))
					return false;

				return true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the path to new project directory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string PathOfNewProject
		{
			get { return Path.Combine(SpongeProject.MainProjectsFolder, txtProjectName.Text); }
		}

		//protected void btnOK_Click(object sender, EventArgs e)
		//{
		//DialogResult = DialogResult.OK;
		//Close();
		//}

		//protected void btnCancel_Click(object sender, EventArgs e)
		//{
		//DialogResult = DialogResult.Cancel;
		//Close();
		//}
	}
}