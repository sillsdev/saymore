using System.Windows.Forms;
using Sponge2.Model;
using Sponge2.Model.Files;

namespace Sponge2.UI.ComponentEditors
{
	public partial class SimpleFileInfoControl : UserControl
	{

		public SimpleFileInfoControl(ComponentFile file)
		{
			InitializeComponent();
			Name = "info";
			textBox1.Text = file.FileName;
		}
	}
}
