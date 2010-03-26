using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIL.Sponge.Dialogs.NewSessionsFromFiles.CopyFiles
{
	public partial class MakeSessionsFromFileProgressDialog : Form
	{
		private CopyFilesViewModel _model;

		public MakeSessionsFromFileProgressDialog(IEnumerable<KeyValuePair<string, string>> sourceDestinationPathPairs,
			Action<string, string> sessionCreatingMethod)
		{
			InitializeComponent();
			_model = new CopyFilesViewModel(sourceDestinationPathPairs);
			_model.BeforeCopyingFileRaised = sessionCreatingMethod;
			var copyControl = new CopyFilesControl(_model);
			Controls.Add(copyControl);
		}

		private void UpdateDisplay()
		{
			_okButton.Enabled = _model.Finished;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			UpdateDisplay();
		}

		private void MakeSessionsFromFileProgressDialog_Load(object sender, EventArgs e)
		{
			_model.Start();
		}

		private void _okButton_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}
	}
}
