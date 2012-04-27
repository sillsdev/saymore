using System;
using System.Windows.Forms;

namespace SayMore.Utilities.ProjectWindow
{
	/// <summary>
	/// Just shows a simple dialog containing the html pointed to by the Path property
	/// </summary>
	public partial class ShowHtmlDialog : Form
	{
		public ShowHtmlDialog()
		{
			InitializeComponent();
		}

		public string Path { get; set; }

		private void ShowHtmlDialog_Load(object sender, EventArgs e)
		{
			_browser.Navigate(Path);
		}

		private void HandleClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
