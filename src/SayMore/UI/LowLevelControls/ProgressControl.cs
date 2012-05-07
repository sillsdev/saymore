using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using SayMore.Properties;

namespace SayMore.Utilities.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	public interface IProgressViewModel
	{
		int CurrentProgressValue { get; }
		int MaximumProgressValue { get; }
		string StatusString { get; }
		void Cancel();
		void Start();
		event EventHandler<ProgressFinishedArgs> OnFinished;
		event EventHandler OnUpdateProgress;
		event EventHandler OnUpdateStatus;
	}

	/// ----------------------------------------------------------------------------------------
	public partial class ProgressControl : UserControl
	{
		private IProgressViewModel _model;

		/// ------------------------------------------------------------------------------------
		public ProgressControl()
		{
			InitializeComponent();
			_labelStatus.Font = new Font(Program.DialogFont, FontStyle.Bold);
		}

		/// ------------------------------------------------------------------------------------
		public ProgressControl(IProgressViewModel model) : this()
		{
			Initialize(model);
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize(IProgressViewModel model)
		{
			_model = model;
			_model.OnFinished += HandleFinished;
			_model.OnUpdateProgress += HandleProgressUpdate;
			_model.OnUpdateStatus += HandleStatusUpdate;
			_progressBar.Maximum = _model.MaximumProgressValue;
			_labelStatus.ForeColor = ForeColor;
		}

		/// ------------------------------------------------------------------------------------
		public void SetStatusMessage(string message)
		{
			_labelStatus.Text = message;
		}

		/// ------------------------------------------------------------------------------------
		void HandleStatusUpdate(object sender, EventArgs e)
		{
			if (InvokeRequired)
				Invoke((Action)(() => _labelStatus.Text = _model.StatusString));
			else
				_labelStatus.Text = _model.StatusString;
		}

		/// ------------------------------------------------------------------------------------
		void HandleProgressUpdate(object sender, EventArgs e)
		{
			if (InvokeRequired)
				Invoke((Action)(() => _progressBar.Value = Math.Min(_progressBar.Maximum, _model.CurrentProgressValue)));
			else
				_progressBar.Value = Math.Min(_progressBar.Maximum, _model.CurrentProgressValue);
		}

		/// ------------------------------------------------------------------------------------
		void HandleFinished(object sender, ProgressFinishedArgs e)
		{
			_labelStatus.Text = _model.StatusString;

			if (e.Exception == null && !e.ProgressCanceled)
			{
				_progressBar.Value = _progressBar.Maximum;
				using (var player = new SoundPlayer(Resources.Finished))
					player.Play();
			}
			else
			{
				_progressBar.Value = 0;

				if (InvokeRequired)
					Invoke((Action)(() => _labelStatus.ForeColor = Color.Red));
				else
					_labelStatus.ForeColor = Color.Red;

				if (!e.ProgressCanceled)
					SystemSounds.Exclamation.Play();
			}
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class ProgressFinishedArgs : EventArgs
	{
		public bool ProgressCanceled { get; private set; }
		public Exception Exception { get; private set; }

		public ProgressFinishedArgs(bool canceled, Exception error)
		{
			ProgressCanceled = canceled;
			Exception = error;
		}
	}
}
