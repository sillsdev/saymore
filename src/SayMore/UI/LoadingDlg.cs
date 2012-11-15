using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using Palaso.Reporting;
using SayMore.Utilities;

namespace SayMore.UI
{
	public partial class LoadingDlg : Form
	{
		public BackgroundWorker BackgroundWorker { get; set; }
		public string GenericErrorMessage { get; set; }
		private Image _originalGif;

		private Exception _exception;

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

			if (_labelLoading.Right - 20 > Right)
				Width += ((_labelLoading.Right + 20) - Right);
		}

		/// ------------------------------------------------------------------------------------
		public void Show(Control parent)
		{
			if (parent == null || parent.Width < Width || parent.Height < Height)
				StartPosition = FormStartPosition.CenterScreen;
			else
			{
				// Center the loading dialog within the bounds of the specified control.
				StartPosition = FormStartPosition.Manual;
				var pt = parent.PointToScreen(new Point(0, 0));
				pt.X += (parent.Width - Width) / 2;
				pt.Y += (parent.Height - Height) / 2;
				Location = pt;
				_labelLoading.MaximumSize = new Size(parent.Height / 2 - (Height - _labelLoading.Height),
					parent.Width / 2 - (Width - _labelLoading.Width));
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
				}
				_originalGif = _pictureLoading.Image;
				BackgroundWorker.RunWorkerCompleted += HandleBackgroundWorkerCompleted;
				BackgroundWorker.RunWorkerAsync(this);

				ShowDialog(parent);
			}
			else
				Show();
		}

		/// ------------------------------------------------------------------------------------
		public void SetState(string msg, Exception e)
		{
			if (InvokeRequired)
				Invoke(new Action(() => SetState(msg, e)));
			else
			{
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
				UsageReporter.ReportException(false, GenericErrorMessage, _exception, _labelLoading.Text);
			}
			else if (e.Error != null)
			{
				ErrorReport.NotifyUserOfProblem(GenericErrorMessage, e.Error);
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
	}
}
