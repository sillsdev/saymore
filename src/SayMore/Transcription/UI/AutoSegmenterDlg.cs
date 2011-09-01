using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Palaso.IO;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	public partial class AutoSegmenterDlg : Form
	{
		private string[] _segments;
		private string _annotationFileName;
		private readonly string _mediaFileName;

		/// ------------------------------------------------------------------------------------
		public static string CreateAnnotationFile(string mediaFileName)
		{
			using (var dlg = new AutoSegmenterDlg(mediaFileName))
			{
				dlg.ShowDialog();
				return dlg._annotationFileName;
			}
		}

		/// ------------------------------------------------------------------------------------
		public AutoSegmenterDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public AutoSegmenterDlg(string mediaFileName) : this()
		{
			_mediaFileName = mediaFileName;
			_labelAudioFileName.Text = Path.GetFileName(_mediaFileName);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSegmentButtonClick(object sender, EventArgs e)
		{
			_segments = null;

			_pictureBoxBusyWheel.Visible = true;
			_labelGeneratingSegmentsMsg.Visible = true;
			Cursor = Cursors.WaitCursor;

			var worker = new BackgroundWorker();
			worker.DoWork += GetSegments;
			worker.RunWorkerAsync();

			while (worker.IsBusy)
				Application.DoEvents();

			if (_segments != null)
				_annotationFileName = AnnotationFileHelper.CreateFromSegments(_mediaFileName, _segments);

			Close();
		}

		/// ------------------------------------------------------------------------------------
		private void GetSegments(object sender, DoWorkEventArgs e)
		{
			var prs = new Process();

			prs.StartInfo.CreateNoWindow = true;
			prs.StartInfo.UseShellExecute = false;
			prs.StartInfo.RedirectStandardOutput = true;
			prs.StartInfo.FileName = FileLocator.GetFileDistributedWithApplication("AutoSegmenter", "Ruby", "ruby.exe");

			var script = FileLocator.GetFileDistributedWithApplication("AutoSegmenter", "segment.rb");

			prs.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" {2} {3} 0 {4}", script,
				_mediaFileName, (int)_upDnSilenceThreshold.Value,
				(float)_upDownClusterDuration.Value, (float)_upDnOnsetAlgorithmThreshold.Value);

			if (prs.Start())
			{
				var output = prs.StandardOutput.ReadToEnd().Replace("\r", string.Empty);
				prs.WaitForExit();
				_segments = output.Split('\n').Select(line => line.Split(',')[0]).ToArray();
			}
		}
	}
}
