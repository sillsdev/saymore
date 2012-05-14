using System.IO;
using System.Windows.Forms;

namespace SayMore.UI.ProjectWindow
{
	public partial class FirstTimeRunDialog : Form
	{
		public FirstTimeRunDialog(string fileToDiaplay)
		{
			InitializeComponent();

			if  (!string.IsNullOrEmpty(fileToDiaplay) && File.Exists(fileToDiaplay))
				_browser.Navigate(fileToDiaplay);
			else
				_browser.DocumentText = "<HTML><b>First Time Run Information</b></HTML>";
		}
	}
}
