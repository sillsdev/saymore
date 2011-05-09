using System;
using System.Windows.Forms;
using Palaso.Progress;
using SayMore.Properties;

namespace SayMore.UI.Archiving
{
	/// ----------------------------------------------------------------------------------------
	public partial class ArchivingDlg : Form
	{
		private readonly ArchivingDlgViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		public ArchivingDlg()
		{
			InitializeComponent();

			_progressBar.Visible = false;
			_linkOverview.Font = Program.DialogFont;
			_linkOverview.Links.Clear();
			_linkOverview.Links.Add(0, 4, Settings.Default.RampWebSite);
			_buttonLaunchRamp.Enabled = false;
		}

		/// ------------------------------------------------------------------------------------
		public ArchivingDlg(ArchivingDlgViewModel model) : this()
		{
			_viewModel = model;

			model.LogBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			model.LogBox.Margin = new Padding(0, 5, 0, 5);
			model.LogBox.ReportErrorLinkClicked += delegate { Close(); };
			_tableLayoutPanel.Controls.Add(model.LogBox, 0, 1);
			_tableLayoutPanel.SetColumnSpan(model.LogBox, 3);

			_buttonLaunchRamp.Click += (s, e) => model.CallRAMP();

			_buttonCancel.MouseLeave += delegate
			{
				if (model.IsBusy)
					WaitCursor.Show();
			};

			_buttonCancel.MouseEnter += delegate
			{
				if (model.IsBusy)
					WaitCursor.Hide();
			};

			_buttonCancel.Click += delegate
			{
				model.Cancel();
				WaitCursor.Hide();
			};

			_buttonCreatePackage.Click += delegate
			{
				Focus();
				_progressBar.Visible = true;
				WaitCursor.Show();
				_buttonLaunchRamp.Enabled = model.CreatePackage();
				_buttonCreatePackage.Enabled = false;
				_progressBar.Visible = false;
				WaitCursor.Hide();
			};
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			WaitCursor.Show();

			_buttonCreatePackage.Enabled = _viewModel.Initialize(
				max => _progressBar.Invoke(new Action(() => { _progressBar.Value = 0; _progressBar.Maximum = max; })),
				() => _progressBar.Increment(1));

			WaitCursor.Hide();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRampLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var tgt = e.Link.LinkData as string;

			if (!string.IsNullOrEmpty(tgt))
				System.Diagnostics.Process.Start(tgt);
		}
	}
}
