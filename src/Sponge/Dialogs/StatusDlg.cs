using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SIL.Sponge.Utilities;

namespace SIL.Sponge.Dialogs
{
	public partial class StatusDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public StatusDlg()
		{
			InitializeComponent();

			_messageLabel.Text = string.Empty;
			_OKButton.Enabled = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OKEnabled
		{
			get { return _OKButton.Enabled; }
			set { _OKButton.Enabled = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Message
		{
			get { return _messageLabel.Text; }
			set { _messageLabel.Text = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw a border around the dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			var rc = ClientRectangle;
			rc.Inflate(-1, -1);
			SpongeColors.PaintBorder(e.Graphics, SpongeColors.BarBorder, rc, BorderSides.All);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ShowAndProcess(Func<object, string> getStatusMessageMethod,
			Func<object, bool> processMethod, IEnumerable listToProcess)
		{
			using (var worker = new BackgroundWorker())
			{
				worker.WorkerReportsProgress = true;
				worker.WorkerSupportsCancellation = false;
				worker.DoWork += HandlerWorkerDoWork;
				worker.ProgressChanged += HandlerWorkerProgressChanged;
				worker.RunWorkerCompleted += HandlerWorkerRunWorkerCompleted;
				var args = new object[] { getStatusMessageMethod, processMethod, listToProcess };
				worker.RunWorkerAsync(args);
				ShowDialog(ActiveForm);
			}
		}

		#region Background worker event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Iterates through a list of objects and calls the specified processing delegate
		/// for each item in the list. Before each item is processed, the item is passed to
		/// a delegate (if one is specified) that returns a message. If a message is returned,
		/// then the dialog's status message is updated accordingly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void HandlerWorkerDoWork(object sender, DoWorkEventArgs e)
		{
			var args = e.Argument as object[];
			if (args.Length != 3)
				throw new ArgumentException("Invalid number of arguments for background worker.");

			var getStatusMessageMethod = args[0] as Func<object, string>;

			var processMethod = args[1] as Func<object, bool>;
			if (processMethod == null)
				throw new ArgumentException("Second argument to background worker must be of type Func<object, bool>.");

			var list = args[2] as IEnumerable;
			if (list == null)
				throw new ArgumentException("Third argument to background worker must be of type IEnumerable.");

			int i = 0;
			foreach (var item in list)
			{
				if (getStatusMessageMethod != null)
				{
					var msg = getStatusMessageMethod(item);
					if (msg != null)
						((BackgroundWorker)sender).ReportProgress(0, msg);
				}

				if (processMethod(item))
					i++;
			}

			e.Result = i;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the ProgressChanged event of the worker control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlerWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			_messageLabel.Text = e.UserState as string;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the RunWorkerCompleted event of the worker control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlerWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			var worker = sender as BackgroundWorker;
			worker.DoWork -= HandlerWorkerDoWork;
			worker.ProgressChanged -= HandlerWorkerProgressChanged;
			worker.RunWorkerCompleted -= HandlerWorkerRunWorkerCompleted;
			_OKButton.Enabled = true;
			_OKButton.Refresh();
			_messageLabel.Text = string.Format("Finished creating {0} new sessions.", e.Result);
		}

		#endregion
	}
}
