using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.UI.Archiving;
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

			if (Settings.Default.OralAnnotationDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.OralAnnotationDlg = FormSettings.Create(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		public OralAnnotationDlg(string caption, string annotationAffix, TimeOrderTier tier) : this()
		{
			_labelRecordingType.Text = caption;
			_oralAnnotationRecorder.Initialize(new OralAnnotationRecorderViewModel(annotationAffix, tier));
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
