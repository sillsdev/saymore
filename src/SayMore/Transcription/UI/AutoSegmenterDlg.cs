using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Palaso.IO;
using Palaso.Progress.LogBox;
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
			var script = FileLocator.GetFileDistributedWithApplication("AutoSegmenter", "segment.rb");

			var args = string.Format("\"{0}\" \"{1}\" {2} {3} 0 {4}", script,
									 _mediaFileName, (int) _upDnSilenceThreshold.Value,
									 (float) _upDownClusterDuration.Value, (float) _upDnOnsetAlgorithmThreshold.Value);

			var result =
				Palaso.CommandLineProcessing.CommandLineRunner.Run(
					FileLocator.GetFileDistributedWithApplication("AutoSegmenter", "Ruby", "ruby.exe"),
					args, null, 100, new NullProgress());


			var output = result.StandardOutput.Replace("\r", string.Empty);
			var pairs = new List<KeyValuePair<float, float>>();
			foreach (var line in output.Split('\n'))
			{
				string[] parts = line.Split(',');
				if (parts.Length == 2 && !string.IsNullOrEmpty(parts[0]) && !string.IsNullOrEmpty(parts[1]))
					pairs.Add(new KeyValuePair<float, float>(float.Parse(parts[0]), float.Parse(parts[1])));
			}
			_segments = (from s in ChooseSegments(pairs) select s.ToString()).ToArray();
		}


		private IEnumerable<float> ChooseSegments(List<KeyValuePair<float, float>> pairs)
		{
			var p = pairs.ToArray();
			float last = 0.0f;
			for (int i = 0; i < p.Length; i++)
			{
				float time = p[i].Key;
				float probability = p[i].Value; //TODO: this would be really helpful, we're just being dumb at the moment
				if (time - last > 2)
				{
					last = time;
					Debug.WriteLine(string.Format("{0} {1}*", time, probability));
					yield return time;
				}
				else
				{
					Debug.WriteLine(string.Format("{0} {1}", time, probability));
				}
			}
		}
	}
}
