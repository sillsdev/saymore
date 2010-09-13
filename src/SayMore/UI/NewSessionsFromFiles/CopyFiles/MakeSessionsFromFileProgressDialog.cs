using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SayMore.UI.NewEventsFromFiles
{
	/// ----------------------------------------------------------------------------------------
	public partial class MakeEventsFromFileProgressDialog : Form
	{
		private readonly CopyFilesViewModel _model;

		/// ------------------------------------------------------------------------------------
		public MakeEventsFromFileProgressDialog(
			IEnumerable<KeyValuePair<string, string>> sourceDestinationPathPairs,
			Action<string> eventCreatingMethod)
		{
			InitializeComponent();
			_model = new CopyFilesViewModel(sourceDestinationPathPairs);
			_model.BeforeCopyingFileRaised = eventCreatingMethod;
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
