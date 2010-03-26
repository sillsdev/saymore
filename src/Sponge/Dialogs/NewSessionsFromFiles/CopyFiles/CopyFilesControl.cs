using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using SIL.Sponge.Properties;

namespace SIL.Sponge.Dialogs.NewSessionsFromFiles.CopyFiles
{
	public partial class CopyFilesControl : UserControl
	{
		private readonly CopyFilesViewModel _model;

		public CopyFilesControl(CopyFilesViewModel model)
		{
			_model = model;
			model.OnFinished += new EventHandler(OnFinished);
			InitializeComponent();
		}

		void OnFinished(object sender, EventArgs e)
		{
			if (sender == null)
			{
				using (var player = new SoundPlayer(Resources.finished))
				{
					player.Play();
				}
			}
			else
			{
				if(InvokeRequired)
				{
					var d = new EventHandler(delegate { _progressBar.ForeColor = Color.Red; });
					Invoke(d);
					return;
				}
				_progressBar.ForeColor = Color.Red;
				//enhance...  play an error sound.
			}
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			_progressBar.Value = _model.TotalPercentage;
			_statusLabel.Text = _model.StatusString;
		}

		private void _progressBar_Click(object sender, EventArgs e)
		{

		}
	}
}
