using System;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using SayMore.Media.Audio;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Media;
using SayMore.UI.ComponentEditors;
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

			_buttonManualSegmentationHelp.Click += (s, e) =>
				Program.ShowHelpTopic("/Using_Tools/Sessions_tab/Use_Manual_Segmentation_Tool.htm");
			_buttonCarefulSpeechToolHelp.Click += (s, e) =>
				Program.ShowHelpTopic("/Using_Tools/Sessions_tab/Use_Careful_Speech_Tool.htm");
			_buttonELANFileHelp.Click += (s, e) => Program.ShowHelpTopic("/Concepts/ELAN.htm");
			_buttonAudacityHelp.Click += (s, e) => Program.ShowHelpTopic("/Concepts/Audacity.htm");
			_buttonAutoSegmenterHelp.Click += (s, e) =>
				Program.ShowHelpTopic("/Using_Tools/Sessions_tab/Use_Auto_Segmenter_Tool.htm");

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
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			_labelSegmentationMethod.Font = FontHelper.MakeFont(Program.DialogFont, 10, FontStyle.Bold);
			_labelIntroduction.Font = Program.DialogFont;
			_labelSegmentationMethodQuestion.Font = Program.DialogFont;
			_radioButtonManual.Font = Program.DialogFont;
			_radioButtonCarefulSpeech.Font = Program.DialogFont;
			_radioButtonElan.Font = Program.DialogFont;
			_radioButtonAudacity.Font = Program.DialogFont;
			_radioButtonAutoSegmenter.Font = Program.DialogFont;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsOKToShow
		{
			get
			{
				return (_file != null &&
					!_file.GetDoesHaveAnnotationFile() &&
					_file.GetCanHaveAnnotationFile() &&
					AudioUtils.GetIsFileStandardPcm(_file.PathToAnnotatedFile));
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString(
				"SessionsView.Transcription.StartAnnotatingTab.TabText", "Start Annotating");

			base.HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGetStartedButtonClick(object sender, EventArgs e)
		{
			_buttonGetStarted.Enabled = false;
			ExternalProcess.CleanUpAllProcesses();
			string newAnnotationFile = null;

			if (_radioButtonManual.Checked)
			{
				Settings.Default.DefaultSegmentationMethod = 0;
				newAnnotationFile = ManualSegmenterDlg.ShowDialog(_file, this);
			}
			else if (_radioButtonCarefulSpeech.Checked)
			{
				Settings.Default.DefaultSegmentationMethod = 1;
				newAnnotationFile = _file.RecordAnnotations(FindForm(), AudioRecordingType.Careful);
			}
			else if (_radioButtonElan.Checked)
			{
				var caption = LocalizationManager.GetString(
					"DialogBoxes.Transcription.CreateAnnotationFileDlg.LoadSegmentFileDlgCaption", "Select Segment File");

				var filetype = LocalizationManager.GetString(
					"DialogBoxes.Transcription.CreateAnnotationFileDlg.ElanFileTypeString",
					"ELAN File (*.eaf)|*.eaf");

				newAnnotationFile = GetAudacityOrElanFile(caption, filetype);
				Settings.Default.DefaultSegmentationMethod = 2;
			}
			else if (_radioButtonAudacity.Checked)
			{
				var caption = LocalizationManager.GetString(
					"DialogBoxes.Transcription.CreateAnnotationFileDlg.AudacityLabelOpenFileDlg.Caption",
					"Select Audacity Label File");

				var filetype = LocalizationManager.GetString(
					"DialogBoxes.Transcription.CreateAnnotationFileDlg.AudacityLabelOpenFileDlg.FileTypeString",
					"Audacity Label File (*.txt)|*.txt");

				newAnnotationFile = GetAudacityOrElanFile(caption, filetype);
				Settings.Default.DefaultSegmentationMethod = 3;
			}
			else if (_radioButtonAutoSegmenter.Checked)
			{
				Settings.Default.DefaultSegmentationMethod = 4;
			}

			if (newAnnotationFile != null && ComponentFileListRefreshAction != null)
				ComponentFileListRefreshAction(newAnnotationFile, null);

			_buttonGetStarted.Enabled = true;
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
				dlg.Filter = filter + "|" + LocalizationManager.GetString(
					"DialogBoxes.Transcription.CreateAnnotationFileDlg.AllFileTypeString",
					"All Files (*.*)|*.*");

				return (dlg.ShowDialog() != DialogResult.OK ? null :
					AnnotationFileHelper.CreateFileFromFile(dlg.FileName, _file.PathToAnnotatedFile));
			}
		}
	}
}
