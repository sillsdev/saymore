using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sponge2
{
	public partial class SessionsControl : UserControl
	{
		private readonly SessionsPM _model;

		public SessionsControl(SessionsPM presentationModel)
		{
			_model = presentationModel;
			InitializeComponent();
			label1.Text = _model.TestLabel;
		}
	}
}
