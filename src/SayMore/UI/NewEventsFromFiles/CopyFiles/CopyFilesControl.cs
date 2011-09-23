using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using SayMore.Properties;

namespace SayMore.UI.NewEventsFromFiles
{
	/// ----------------------------------------------------------------------------------------
	public partial class CopyFilesControl : UserControl
	{
		private readonly CopyFilesViewModel _model;

		/// ------------------------------------------------------------------------------------
		public CopyFilesControl(CopyFilesViewModel model)
		{
			InitializeComponent();
			_labelStatus.Font = new Font(SystemFonts.IconTitleFont, FontStyle.Bold);
			_model = model;
			_model.OnFinished += HandleCopyFinished;
			_model.OnUpdateProgress += HandleCopyProgressUpdate;
			_model.OnUpdateStatus += HandleCopyStatusUpdate;
			_progressBar.Maximum = 100;
		}

		/// ------------------------------------------------------------------------------------
		void HandleCopyStatusUpdate(object sender, EventArgs e)
		{
			_labelStatus.Text = _model.StatusString;
		}

		/// ------------------------------------------------------------------------------------
		void HandleCopyProgressUpdate(object sender, EventArgs e)
		{
			_progressBar.Value = _model.TotalPercentage;
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
					Invoke(new EventHandler(delegate { _progressBar.ForeColor = Color.Red; }));
				else
					_progressBar.ForeColor = Color.Red;

				//enhance...  play an error sound.
			}
		}
	}
}
