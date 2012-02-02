using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI.SegmentingAndRecording;
using SayMore.UI.ComponentEditors;
using SayMore.UI.MediaPlayer;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class StartAnnotatingEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public StartAnnotatingEditor(ComponentFile file) : base(file, null, null)
		{
			InitializeComponent();
			Name = "StartAnnotating";

			_buttonAudacityHelp.Click += (s, e) =>
				Program.ShowHelpTopic("/Using_Tools/Events_tab/Create_Annotation_File.htm");

			_buttonELANFileHelp.Click += (s, e) =>
				Program.ShowHelpTopic("/Using_Tools/Events_tab/Create_Annotation_File.htm");

			switch (Settings.Default.DefaultSegmentationMethod)
			{
				case 0: _radioButtonManual.Checked = true; break;
				case 1: _radioButtonCarefulSpeech.Checked = true; break;
				case 2: _radioButtonElan.Checked = true; break;
				case 3: _radioButtonAudacity.Checked = true; break;
				case 4: _radioButtonAutoSegmenter.Checked = true; break;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);

			_labelSegmentationMethod.Font = FontHelper.MakeFont(SystemFonts.IconTitleFont, 10, FontStyle.Bold);
			_labelIntroduction.Font = SystemFonts.IconTitleFont;
			_labelSegmentationMethodQuestion.Font = SystemFonts.IconTitleFont;
			_radioButtonManual.Font = SystemFonts.IconTitleFont;
			_radioButtonCarefulSpeech.Font = SystemFonts.IconTitleFont;
			_radioButtonElan.Font = SystemFonts.IconTitleFont;
			_radioButtonAudacity.Font = SystemFonts.IconTitleFont;
			_radioButtonAutoSegmenter.Font = SystemFonts.IconTitleFont;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsOKSToShow
		{
			get
			{
				return (_file != null && !_file.GetDoesHaveAnnotationFile() &&
					Path.GetExtension(_file.PathToAnnotatedFile).ToLower() == ".wav");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString("EventsView.Transcription.StartAnnotatingTab.TabText", "Start Annotating");
			base.HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGetStartedButtonClick(object sender, System.EventArgs e)
		{
			_buttonGetStarted.Enabled = false;
			MPlayerHelper.CleanUpMPlayerProcesses();
			string newAnnotationFile = null;

			if (_radioButtonManual.Checked)
			{
				Settings.Default.DefaultSegmentationMethod = 0;
				newAnnotationFile = ManuallySegmentFile();
			}
			else if (_radioButtonCarefulSpeech.Checked)
			{
				Settings.Default.DefaultSegmentationMethod = 1;
				newAnnotationFile = _file.RecordAnnotations(OralAnnotationType.Careful);
			}
			else if (_radioButtonElan.Checked)
			{
				var caption = LocalizationManager.GetString(
					"DialogBoxes.Transcription.CreateAnnotationFileDlg.LoadSegmentFileDlgCaption", "Select Segment File");

				var filetype = LocalizationManager.GetString("DialogBoxes.Transcription.CreateAnnotationFileDlg.ElanFileTypeString",
					"ELAN File (*.eaf)|*.eaf");

				newAnnotationFile = GetAudacityOrElanFile(caption, filetype);
				Settings.Default.DefaultSegmentationMethod = 2;
			}
			else if (_radioButtonAudacity.Checked)
			{
				var caption = LocalizationManager.GetString(
					"DialogBoxes.Transcription.CreateAnnotationFileDlg.AudacityLabelOpenFileDlg.Caption", "Select Audacity Label File");

				var filetype = LocalizationManager.GetString("DialogBoxes.Transcription.CreateAnnotationFileDlg.AudacityLabelOpenFileDlg.FileTypeString",
					"Audacity Label File (*.txt)|*.txt");

				newAnnotationFile = GetAudacityOrElanFile(caption, filetype);
				Settings.Default.DefaultSegmentationMethod = 3;
			}
			else if (_radioButtonAutoSegmenter.Checked)
			{
				Settings.Default.DefaultSegmentationMethod = 4;
			}

			if (newAnnotationFile != null && ComponentFileListRefreshAction != null)
				ComponentFileListRefreshAction(newAnnotationFile);

			_buttonGetStarted.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		private string ManuallySegmentFile()
		{
			using (var viewModel = new ManualSegmenterDlgViewModel(_file))
			using (var dlg = new ManualSegmenterDlg(viewModel))
			{
				return (dlg.ShowDialog(this) != DialogResult.OK || !viewModel.WereChangesMade ? null :
					AnnotationFileHelper.CreateFromSegments(_file.PathToAnnotatedFile,
					viewModel.GetSegments().ToArray()));
			}
		}

		/// ------------------------------------------------------------------------------------
		private string GetAudacityOrElanFile(string caption, string filter)
		{
			using (var dlg = new OpenFileDialog())
			{
				dlg.Title = caption;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;
				dlg.Filter = filter + "|" +
					LocalizationManager.GetString("DialogBoxes.Transcription.CreateAnnotationFileDlg.AllFileTypeString", "All Files (*.*)|*.*");

				return (dlg.ShowDialog() != DialogResult.OK ? null :
					AnnotationFileHelper.Create(dlg.FileName, _file.PathToAnnotatedFile));
			}
		}
	}
}
