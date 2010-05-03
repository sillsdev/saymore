using System;
using System.Windows.Forms;
using Sponge2.Model;
using Sponge2.Model.Files;

namespace Sponge2.UI.ComponentEditors
{
	public partial class PersonBasicEditor : UserControl
	{
		private readonly BindingHelper _binder;

		public PersonBasicEditor(ComponentFile file)
		{
			InitializeComponent();
			Name = "Basic";
			_binder = new BindingHelper(file);
			_binder.BindTextBox(_fullName, "fullName");
			_binder.BindTextBox(_birthYear, "birthYear");
		}
	}
}
