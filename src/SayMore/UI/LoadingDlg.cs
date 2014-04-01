using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DesktopAnalytics;
using L10NSharp;
using Palaso.Reporting;
using Palaso.UI.WindowsForms.Miscellaneous;
using SayMore.Utilities;

namespace SayMore.UI
{
	public partial class LoadingDlg : Form
	{
		public BackgroundWorker BackgroundWorker { get; set; }
		public string GenericErrorMessage { get; set; }
		private Image _originalGif;
		private Control _parent;
		private Exception _exception;
		private const int kMarginInParent = 36;

		/// ------------------------------------------------------------------------------------
		public LoadingDlg()
		{
			InitializeComponent();
			_linkCancel.Font = Program.DialogFont;
			BackColor = Color.White;
			_linkCancel.Visible = false;
		}

		/// ------------------------------------------------------------------------------------
		public LoadingDlg(string message) : this()
		{
			if (message != null)
				_labelLoading.Text = message;
		}

		/// ------------------------------------------------------------------------------------
		public void Show(Control parent)
		{
			_parent = parent;
			if (parent == null || parent.Width < Width + kMarginInParent || parent.Height < Height + kMarginInParent)
				StartPosition = FormStartPosition.CenterScreen;
			else
			{
				StartPosition = FormStartPosition.Manual;
				_labelLoading.MaximumSize = new Size(parent.Width - kMarginInParent - (Width - _labelLoading.Width),
					parent.Height - kMarginInParent - (Height - _labelLoading.Height));
			}

			if (BackgroundWorker != null)
			{
				if (BackgroundWorker.WorkerSupportsCancellation)
				{
					_linkCancel.Visible = true;
					_linkCancel.LinkClicked += delegate
					{
						BackgroundWorker.CancelAsync();
						_labelLoading.Text = LocalizationManager.GetString("DialogBoxes.ProgressDlg.CancellingMsg", "Canceling...");
						_linkCancel.Hide();
					};
					CancelButton = _linkCancel;
				}
				else
					_tableLayoutPanel.SetColumnSpan(_labelLoading, 2);

				_originalGif = _pictureLoading.Image;
				BackgroundWorker.RunWorkerCompleted += HandleBackgroundWorkerCompleted;
				BackgroundWorker.RunWorkerAsync(this);

				ShowDialog(parent);
			}
			else
			{
				_tableLayoutPanel.SetRowSpan(_labelLoading, 2);
				base.Show(parent);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetState(string msg, Exception e)
		{
			if (InvokeRequired)
				Invoke(new Action(() => SetState(msg, e)));
			else
			{
				if (BackgroundWorker != null && BackgroundWorker.CancellationPending)
					return;
				_labelLoading.Text = msg;
				_exception = e;
				_pictureLoading.Image = (e == null ? _originalGif : Properties.Resources.kimidWarning);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				DialogResult = DialogResult.Cancel;
				Analytics.Track("User cancelled operation", new Dictionary<string, string> {
					{"_labelLoading.Text", _labelLoading.Text} });
				Analytics.ReportException(_exception);
			}
			else if (e.Error != null)
			{
				WaitCursor.Hide();
				ErrorReport.NotifyUserOfProblem(e.Error, GenericErrorMessage);
				DialogResult = DialogResult.Abort;
			}
			else if (e.Result == null || (e.Result is bool && !(bool)e.Result))
				DialogResult = DialogResult.No;
			else
				DialogResult = DialogResult.OK;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var rc = ClientRectangle;
			rc.Width--;
			rc.Height--;

			using (var pen = new Pen(AppColors.BarBorder))
				e.Graphics.DrawRectangle(pen, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadingDlg_Resize(object sender, EventArgs e)
		{
			if (_parent == null || StartPosition == FormStartPosition.CenterScreen)
				return;
			// Center the loading dialog within the bounds of the parent.
			var pt = _parent.PointToScreen(new Point(0, 0));
			pt.X += (_parent.Width - Width) / 2;
			pt.Y += (_parent.Height - Height) / 2;
			Location = pt;
		}
	}
}
