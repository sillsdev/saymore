using System;
using System.Drawing;
using System.Windows.Forms;
using SayMore.AudioUtils;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		public OralAnnotationDlg()
		{
			InitializeComponent();

			DoubleBuffered = true;

			_labelRecordingType.Font = new Font(SystemFonts.IconTitleFont.FontFamily,
				10f, FontStyle.Bold, GraphicsUnit.Point);

			_labelRecordingFormat.Font = SystemFonts.IconTitleFont;

			var bestFormat = WaveFileUtils.GetDefaultWaveFormat(1);

			_labelRecordingFormat.Text = string.Format(_labelRecordingFormat.Text,
				bestFormat.BitsPerSample, bestFormat.SampleRate);

			if (Settings.Default.OralAnnotationDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.OralAnnotationDlg = FormSettings.Create(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		public OralAnnotationDlg(string caption,
			OralAnnotationType annotationType, TimeOrderTier tier) : this()
		{
			_labelRecordingType.Text = caption;
			_oralAnnotationRecorder.Initialize(new OralAnnotationRecorderViewModel(annotationType, tier), caption);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			Settings.Default.OralAnnotationDlg.InitializeForm(this);
			base.OnShown(e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCloseClick(object sender, EventArgs e)
		{
			Close();
		}
	}
}
