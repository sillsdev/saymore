using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AutoSegmentTester
{
	public partial class AutoSegmenterDlg : Form
	{
		private string[] _segments;
		private string _mediaFileName;

		/// ------------------------------------------------------------------------------------
		public AutoSegmenterDlg()
		{
			InitializeComponent();
			_buttonSegment.Enabled = false;
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

			if (_segments.Length == 0)
			{
				MessageBox.Show("No segments were generated.");
				return;
			}

			WriteSegmentsToFile();
			_pictureBoxBusyWheel.Visible = false;
			_labelGeneratingSegmentsMsg.Visible = false;
			Cursor = Cursors.Default;
		}

		/// ------------------------------------------------------------------------------------
		private void WriteSegmentsToFile()
		{
			_saveFileDlg.FileName = Path.GetFileNameWithoutExtension(_mediaFileName);
			if (_saveFileDlg.ShowDialog(this) != DialogResult.OK)
				return;

			for (int i = 0; i < _segments.Length; i++)
				_segments[i] = _segments[i] + "\t" + _segments[i];

			if (File.Exists(_saveFileDlg.FileName))
				File.Delete(_saveFileDlg.FileName);

			File.WriteAllLines(_saveFileDlg.FileName, _segments);
		}

		/// ------------------------------------------------------------------------------------
		private void GetSegments(object sender, DoWorkEventArgs e)
		{
			var prs = new Process();

			prs.StartInfo.CreateNoWindow = true;
			prs.StartInfo.UseShellExecute = false;
			prs.StartInfo.RedirectStandardOutput = true;
			prs.StartInfo.FileName = @"..\..\Lib\Ruby\ruby.exe";

			var script = @"..\..\Lib\segment.rb";

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

		/// ------------------------------------------------------------------------------------
		private void HandleCloseClick(object sender, EventArgs e)
		{
			Close();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWavFileLabelDragEnter(object sender, DragEventArgs e)
		{
			e.Effect = (GetFileBeingDragged(e) == null ?
				DragDropEffects.None : DragDropEffects.Copy);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWavFileLabelDragDrop(object sender, DragEventArgs e)
		{
			var file = GetFileBeingDragged(e);
			if (file != null)
			{
				_labelAudioFileName.Text = Path.GetFileName(file);
				_mediaFileName = file;
				_buttonSegment.Enabled = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		private string GetFileBeingDragged(DragEventArgs e)
		{
			if (!e.Data.GetFormats().Contains(DataFormats.FileDrop))
				return null;

			var files = e.Data.GetData(DataFormats.FileDrop) as string[];
			return (files != null && files.Length > 0 && files[0].ToLower().EndsWith(".wav") ?
				files[0] : null);
		}
	}
}
