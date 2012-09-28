using System;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using Palaso.Progress;
using SayMore.Properties;
using SilTools;

namespace SayMore.UI.Utilities
{
	/// ----------------------------------------------------------------------------------------
	public partial class ArchivingDlg : Form
	{
		private readonly ArchivingDlgViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		public ArchivingDlg()
		{
			InitializeComponent();

			if (Settings.Default.ArchivingDialog == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.ArchivingDialog = FormSettings.Create(this);
			}

			_progressBar.Visible = false;
			_buttonLaunchRamp.Enabled = false;

			// Visual Studio's designer insists on putting long strings of text in the resource
			// file, even though the dialog's Localizable property is false. So, localized
			// controls having a lot of text in their Text property have to have it set this
			// way rather than in the designer. Otherwise, the code string scanner won't find
			// the control's text.
			_linkOverview.Text = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.OverviewText",
				"RAMP is a utility for entering metadata and uploading submissions to SIL's internal archive, " +
				"REAP. If you have access to this archive, this tool will help you use RAMP to archive your " +
				"SayMore sessions. It will gather up all the files and data related to a session and its " +
				"contributors, then launch RAMP so that you can fill out more information and do the actual submission. ",
				"The first occurance of the word 'RAMP' will be made a hyperlink to the RAMP website. " +
				"If the word 'RAMP' is not found, the text will not contain that hyperlink.",
				null, null, _linkOverview);

			_linkOverview.Font = Program.DialogFont;
			_linkOverview.Links.Clear();

			int i = _linkOverview.Text.IndexOf("RAMP");
			if (i >= 0)
				_linkOverview.Links.Add(i, 4, Settings.Default.RampWebSite);
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
				_buttonCreatePackage.Enabled = false;
				_progressBar.Visible = true;
				WaitCursor.Show();
				_buttonLaunchRamp.Enabled = model.CreatePackage();
				_buttonCreatePackage.Enabled = false;
				_progressBar.Visible = false;
				WaitCursor.Hide();
			};
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.ArchivingDialog.InitializeForm(this);
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			try
			{
				WaitCursor.Show();
				int maxProgBarValue;
				_buttonCreatePackage.Enabled = _viewModel.Initialize(out maxProgBarValue, () => _progressBar.Increment(1));
				_progressBar.Maximum = maxProgBarValue;
				WaitCursor.Hide();
			}
			catch
			{
				WaitCursor.Hide();
				throw;
			}
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
