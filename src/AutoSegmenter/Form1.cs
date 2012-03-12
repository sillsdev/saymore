using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Palaso.Reporting;

namespace AutoSegmenter
{
	public partial class Form1 : Form
	{
		/// ------------------------------------------------------------------------------------
		public Form1()
		{
			InitializeComponent();

			_buttonClose.Click += delegate { Close(); };
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBrowseButtonClicked(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				dlg.CheckFileExists = dlg.CheckPathExists = true;
				dlg.DefaultExt = ".wav";
				dlg.FileName = _textBoxAudioFile.Text;
				dlg.Filter = "Wave Audio (*.wav)|*.wav";
				dlg.RestoreDirectory = false;
				if (dlg.ShowDialog() == DialogResult.OK)
					_textBoxAudioFile.Text = dlg.FileName;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSegmentButtonClicked(object sender, EventArgs e)
		{
			int silenceThreshold;
			float clusterDuration;
			int clusterThreshold;
			float[] onsetDetectionValues;

			var audioFile = _textBoxAudioFile.Text;

			if (!File.Exists(audioFile))
			{
				System.Media.SystemSounds.Beep.Play();
				return;
			}

			try
			{
				silenceThreshold = int.Parse(_textBoxSilenceThreshold.Text);
				clusterDuration = float.Parse(_textBoxClusterDuration.Text);
				clusterThreshold = int.Parse(_textBoxClusterThreshold.Text);

				var odValues = _textBoxOnsetDectectionValues.Text.Split(
					new[] {','}, StringSplitOptions.RemoveEmptyEntries);

				onsetDetectionValues = new float[odValues.Length];
				for (int i = 0; i < odValues.Length; i++)
					onsetDetectionValues[i] = float.Parse(odValues[i]);
			}
			catch (Exception error)
			{
				System.Media.SystemSounds.Beep.Play();
				ErrorReport.NotifyUserOfProblem(error, "There was an error, obviously.");
				return;
			}

			DisplayOutput(AutoSegmenter.SegmentAudioFile(audioFile, silenceThreshold,
				clusterDuration, clusterThreshold, onsetDetectionValues));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Receives the output from the auto. segmentation process. The output is a
		/// dictionary of double values where the key is the value in seconds where a segment
		/// boundary was found by the segmenter and the value is a double value between zero
		/// and 1 indicating the probability of the segment value being good (I think that's
		/// what the probability means).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DisplayOutput(IDictionary<double, double> segmentValues)
		{
			_textBoxSegments.Clear();

			foreach (var item in segmentValues)
			{
				_textBoxSegments.AppendText(Math.Round(item.Key, 2,
					MidpointRounding.AwayFromZero).ToString("000.00"));

				_textBoxSegments.AppendText("       ");

				_textBoxSegments.AppendText(Math.Round(item.Value, 4,
					MidpointRounding.AwayFromZero).ToString("0.000"));

				_textBoxSegments.AppendText(Environment.NewLine);
			}
		}
	}
}
