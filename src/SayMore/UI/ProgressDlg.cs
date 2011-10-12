using System;
using System.Windows.Forms;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class ProgressDlg : Form
	{
		private readonly IProgressViewModel _model;

		/// ------------------------------------------------------------------------------------
		public ProgressDlg(IProgressViewModel model, string caption)
		{
			_model = model;
			InitializeComponent();
			_model.OnFinished += HandleCopyFinished;
			var copyControl = new ProgressControl(_model);
			copyControl.Dock = DockStyle.Fill;
			_tableLayout.Controls.Add(copyControl, 0, 0);
			Text = caption;
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
