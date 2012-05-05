using System;
using System.Windows.Forms;
using SayMore.Utilities.LowLevelControls;

namespace SayMore.Utilities
{
	/// ----------------------------------------------------------------------------------------
	public partial class ProgressDlg : Form
	{
		private readonly IProgressViewModel _model;

		/// ------------------------------------------------------------------------------------
		public ProgressDlg(IProgressViewModel model, string caption) : this(model, caption, false)
		{
		}

		/// ------------------------------------------------------------------------------------
		public ProgressDlg(IProgressViewModel model, string caption, bool showCancel)
		{
			_model = model;
			InitializeComponent();
			_model.OnFinished += HandleFinished;
			var copyControl = new ProgressControl(_model);
			copyControl.Dock = DockStyle.Fill;
			_tableLayout.Controls.Add(copyControl, 0, 0);
			_tableLayout.SetColumnSpan(copyControl, 3);
			Text = caption;

			if (showCancel)
			{
				_buttonCancel.Visible = true;
				_buttonOK.Visible = false;
				_buttonCancel.Click += delegate { _model.Cancel(); };
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			_model.Start();
		}

		/// ------------------------------------------------------------------------------------
		void HandleFinished(object sender, ProgressFinishedArgs e)
		{
			if (e.ProgressCanceled)
				Close();

			_buttonOK.Enabled = true;
			_buttonCancel.Visible = false;
			_buttonOK.Visible = true;
		}
	}
}
