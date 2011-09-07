using System;
using System.Drawing;
using System.Windows.Forms;
using Localization;

namespace SayMore.Transcription.UI
{
	public partial class CreateAnnotationFileDlg : Form
	{
		public string FileName { get; private set; }
		public bool AutoSegment { get; private set; }

		/// ------------------------------------------------------------------------------------
		public CreateAnnotationFileDlg(string mediaFilePath) : this()
		{
			_labelOverview.Text = string.Format(_labelOverview.Text, mediaFilePath);
		}

		/// ------------------------------------------------------------------------------------
		public CreateAnnotationFileDlg()
		{
			InitializeComponent();

			_labelOverview.Font = SystemFonts.IconTitleFont;
			_labelAnnoatationType1.Font = SystemFonts.IconTitleFont;
			_labelAnnoatationType2.Font = SystemFonts.IconTitleFont;
			_labelAudacityOverview.Font = SystemFonts.IconTitleFont;
			_labelElanOverview.Font = SystemFonts.IconTitleFont;

			_buttonAudacityHelp.Click += (s, e) =>
				Program.ShowHelpTopic("/Using_Tools/Events_tab/Create_Annotation_File.htm");

			_buttonELANFileHelp.Click += (s, e) =>
				Program.ShowHelpTopic("/Using_Tools/Events_tab/Create_Annotation_File.htm");
		}

		///// ------------------------------------------------------------------------------------
		//private void HandleAutoSegmentClick(object sender, EventArgs e)
		//{
		//    AutoSegment = true;
		//    DialogResult = DialogResult.OK;
		//    Close();
		//}

		/// ------------------------------------------------------------------------------------
		private void HandleLoadAudacityLabelFileClick(object sender, EventArgs e)
		{
			var caption = LocalizationManager.LocalizeString(
				"CreateAnnotationFileDlg.LoadAudacityLabelFileDlgCaption", "Select Audacity Label File");

			if (ShowOpenFileDialog(caption, "Audacity Label File (*.txt)|*.txt"))
				Close();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLoadSegmentFileClick(object sender, EventArgs e)
		{
			var caption = LocalizationManager.LocalizeString(
				"CreateAnnotationFileDlg.LoadSegmentFileDlgCaption", "Select Segment File");

			if (ShowOpenFileDialog(caption, "ELAN File (*.eaf)|*.eaf"))
				Close();
		}

		/// ------------------------------------------------------------------------------------
		private bool ShowOpenFileDialog(string caption, string filter)
		{
			using (var dlg = new OpenFileDialog())
			{
				dlg.Title = caption;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;
				dlg.Filter = filter + "|All Files (*.*)|*.*";

				if (dlg.ShowDialog() != DialogResult.OK)
					return false;

				FileName = dlg.FileName;
				DialogResult = DialogResult.OK;
				return true;
			}
		}
	}
}
