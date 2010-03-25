using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using SIL.Sponge.Properties;

namespace SIL.Sponge.Dialogs.NewSessionsFromFiles.CopyFiles
{
	public partial class CopyFilesView : UserControl
	{
		private readonly CopyFilesViewModel _model;

		public CopyFilesView(CopyFilesViewModel model)
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
				//enhance...  play an error sound.
			}
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			_progressBar.Value = _model.TotalPercentage;
			_statusLabel.Text = _model.StatusString;
		}
	}
}
