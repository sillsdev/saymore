using System;
using System.Drawing;
using System.Windows.Forms;

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

			// Visual Studio's designer insists on putting long strings of text in the resource
			// file, even though the dialog's Localizable property is false. So, localized
			// controls having a lot of text in their Text property have to have it set this
			// way rather than in the designer. Otherwise, the code string scanner won't find
			// the control's text.
			_labelAudacityOverview.Text = Program.GetString(
				"DialogBoxes.Transcription.CreateAnnotationFileDlg.AudacityOverviewLabel",
				"Annotation requires that the media stream first be segmented into small pieces. " +
				"Currently, you need to use another program to specify the segment boundaries. " +
				"The easiest way is to use Audacity. Open your media file there, choose " +
				"\"Tracks:Create New:Label Track\". At each point where you want a segment, " +
				"choose \"Tracks:Add label at selection\". Finally, choose \"File:Export Labels...\", " +
				"then click this button and select the file you created.",
				null, null, null, _labelAudacityOverview);
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
			var caption = Program.GetString(
				"DialogBoxes.Transcription.CreateAnnotationFileDlg.AudacityLabelOpenFileDlg.Caption", "Select Audacity Label File");

			var filetype = Program.GetString("DialogBoxes.Transcription.CreateAnnotationFileDlg.AudacityLabelOpenFileDlg.FileTypeString",
				"Audacity Label File (*.txt)|*.txt");

			if (ShowOpenFileDialog(caption, filetype))
				Close();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLoadSegmentFileClick(object sender, EventArgs e)
		{
			var caption = Program.GetString(
				"DialogBoxes.Transcription.CreateAnnotationFileDlg.LoadSegmentFileDlgCaption", "Select Segment File");

			var filetype = Program.GetString("DialogBoxes.Transcription.CreateAnnotationFileDlg.ElanFileTypeString",
				"ELAN File (*.eaf)|*.eaf");

			if (ShowOpenFileDialog(caption, filetype))
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
				dlg.Filter = filter + "|" +
					Program.GetString("DialogBoxes.Transcription.CreateAnnotationFileDlg.AllFileTypeString", "All Files (*.*)|*.*");

				if (dlg.ShowDialog() != DialogResult.OK)
					return false;

				FileName = dlg.FileName;
				DialogResult = DialogResult.OK;
				return true;
			}
		}
	}
}
