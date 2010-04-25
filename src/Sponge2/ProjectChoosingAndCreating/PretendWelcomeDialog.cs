using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIL.Sponge
{
	public partial class PretendWelcomeDialog : Form
	{
		public PretendWelcomeDialog()
		{
			InitializeComponent();
		}
		public string ProjectPath { get; private set; }

		private void button1_Click(object sender, EventArgs e)
		{
			using(var dlg  = new FolderBrowserDialog())
			{
				dlg.RootFolder = Environment.SpecialFolder.MyDocuments;

				if(DialogResult.OK == dlg.ShowDialog())
				{
					ProjectPath = dlg.SelectedPath;
					DialogResult = System.Windows.Forms.DialogResult.OK;
					this.Close();
				}
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{

		}
	}
}
