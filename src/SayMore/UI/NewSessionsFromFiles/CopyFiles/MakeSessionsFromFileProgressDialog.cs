using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SayMore.UI.NewSessionsFromFiles
{
	/// ----------------------------------------------------------------------------------------
	public partial class MakeSessionsFromFileProgressDialog : Form
	{
		private readonly CopyFilesViewModel _model;

		/// ------------------------------------------------------------------------------------
		public MakeSessionsFromFileProgressDialog(
			IEnumerable<KeyValuePair<string, string>> sourceDestinationPathPairs,
			Action<string> sessionCreatingMethod)
		{
			InitializeComponent();
			_model = new CopyFilesViewModel(sourceDestinationPathPairs);
			_model.BeforeCopyingFileRaised = sessionCreatingMethod;
			var copyControl = new CopyFilesControl(_model);
			copyControl.Dock = DockStyle.Fill;
			_tableLayout.Controls.Add(copyControl, 0, 0);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_buttonOK.Enabled = _model.Finished;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTimerTick(object sender, EventArgs e)
		{
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			_model.Start();
		}
	}
}
