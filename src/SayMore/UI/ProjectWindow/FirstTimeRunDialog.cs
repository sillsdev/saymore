using System.IO;
using System.Windows.Forms;

namespace SayMore.UI.ProjectWindow
{
	public partial class FirstTimeRunDialog : Form
	{
		public FirstTimeRunDialog(string fileToDisplay)
		{
			InitializeComponent();

			if  (!string.IsNullOrEmpty(fileToDisplay) && File.Exists(fileToDisplay))
				_browser.Navigate(fileToDisplay);
			else
				_browser.DocumentText = "<HTML><b>First Time Run Information</b></HTML>";
		}
	}
}
