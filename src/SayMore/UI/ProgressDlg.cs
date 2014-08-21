using System;
using System.Windows.Forms;
using Palaso.Reporting;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI
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
			Logger.WriteEvent("ProgressDlg constructor. caption = {0}", caption);

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
				_buttonCancel.Click += delegate
				{
					Logger.WriteEvent("Cancelled {0}", Text);
					_model.Cancel();
				};
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
