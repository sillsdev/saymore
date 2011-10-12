using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using SayMore.Properties;

namespace SayMore.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	public interface IProgressViewModel
	{
		int CurrentProgressValue { get; }
		int MaximumProgressValue { get; }
		string StatusString { get; }
		void Start();
		event EventHandler OnFinished;
		event EventHandler OnUpdateProgress;
		event EventHandler OnUpdateStatus;
	}

	/// ----------------------------------------------------------------------------------------
	public partial class ProgressControl : UserControl
	{
		private readonly IProgressViewModel _model;

		/// ------------------------------------------------------------------------------------
		public ProgressControl(IProgressViewModel model)
		{
			InitializeComponent();
			_labelStatus.Font = new Font(SystemFonts.IconTitleFont, FontStyle.Bold);
			_model = model;
			_model.OnFinished += HandleCopyFinished;
			_model.OnUpdateProgress += HandleCopyProgressUpdate;
			_model.OnUpdateStatus += HandleCopyStatusUpdate;
			_progressBar.Maximum = _model.MaximumProgressValue;
		}

		/// ------------------------------------------------------------------------------------
		void HandleCopyStatusUpdate(object sender, EventArgs e)
		{
			if (InvokeRequired)
				Invoke((Action)(() => _labelStatus.Text = _model.StatusString));
			else
				_labelStatus.Text = _model.StatusString;
		}

		/// ------------------------------------------------------------------------------------
		void HandleCopyProgressUpdate(object sender, EventArgs e)
		{
			if (InvokeRequired)
				Invoke((Action)(() => _progressBar.Value = _model.CurrentProgressValue));
			else
				_progressBar.Value = _model.CurrentProgressValue;
		}

		/// ------------------------------------------------------------------------------------
		void HandleCopyFinished(object sender, EventArgs e)
		{
			_progressBar.Value = _progressBar.Maximum;
			_labelStatus.Text = _model.StatusString;

			if (sender == null)
			{
				using (var player = new SoundPlayer(Resources.Finished))
					player.Play();
			}
			else
			{
				if (InvokeRequired)
					Invoke((Action)(() => _labelStatus.ForeColor = Color.Red));
				else
					_labelStatus.ForeColor = Color.Red;

				//enhance...  play an error sound.
			}
		}
	}
}
