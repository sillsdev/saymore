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
			_model.BeforeFileCopiedAction = eventCreatingMethod;
			_model.OnFinished += HandleCopyFinished;
			var copyControl = new CopyFilesControl(_model);
			copyControl.Dock = DockStyle.Fill;
			_tableLayout.Controls.Add(copyControl, 0, 0);
		}

		/// ------------------------------------------------------------------------------------
		void HandleCopyFinished(object sender, EventArgs e)
		{
			_buttonOK.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			_model.Start();
		}
	}
}
