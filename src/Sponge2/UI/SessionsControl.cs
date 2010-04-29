using System;
using System.Windows.Forms;
using Sponge2.Model;

namespace Sponge2.UI
{
	public partial class SessionsControl : UserControl
	{
		private readonly SessionsViewModel _model;

		public SessionsControl(SessionsViewModel presentationModel)
		{
			_model = presentationModel;
			InitializeComponent();
			//label1.Text = _model.TestLabel;
		}

		private void SessionsControl_Load(object sender, EventArgs e)
		{
			UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			textBox1.Text = "";
			foreach (Session  session in _model.Sessions)
			{
				textBox1.Text += session.InfoForPrototype + Environment.NewLine;
			}
		}
	}
}
