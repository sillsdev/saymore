using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sponge2.Model.Files;

namespace Sponge2.UI.ComponentEditors
{
	public partial class SessionBasicEditor : UserControl
	{
		private readonly BindingHelper _binder;

		public SessionBasicEditor(ComponentFile file)
		{
			InitializeComponent();
			Name = "Basic";
			_binder = new BindingHelper(file);
			_binder.BindTextBox(_id, "id");
			_binder.BindTextBox(_title, "title");
		}
	}
}
