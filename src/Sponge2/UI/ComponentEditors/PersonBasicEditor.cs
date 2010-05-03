using System.Windows.Forms;
using Sponge2.Model.Files;

namespace Sponge2.UI.ComponentEditors
{
	public partial class PersonBasicEditor : UserControl
	{
		public PersonBasicEditor(ComponentFile file)
		{
			InitializeComponent();
			Name = "Basic";
			_binder.SetComponentFile(file);
		}
	}
}
