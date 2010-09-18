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
			_statusLabel.Font = new Font(SystemFonts.IconTitleFont, FontStyle.Bold);
			_model = model;
			model.OnFinished += HandleCopyFinished;
		}

		/// ------------------------------------------------------------------------------------
		void HandleCopyFinished(object sender, EventArgs e)
		{
			if (sender == null)
			{
				using (var player = new SoundPlayer(Resources.Finished))
					player.Play();
			}
			else
			{
				if (InvokeRequired)
				{
					Invoke(new EventHandler(delegate { _progressBar.ForeColor = Color.Red; }));
					return;
				}

				_progressBar.ForeColor = Color.Red;
				//enhance...  play an error sound.
			}
		}

		/// ------------------------------------------------------------------------------------
		private void _timer_Tick(object sender, EventArgs e)
		{
			_progressBar.Value = _model.TotalPercentage;
			_statusLabel.Text = _model.StatusString;
		}
	}
}
