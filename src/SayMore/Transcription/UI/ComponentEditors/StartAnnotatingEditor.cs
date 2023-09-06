using System;
using System.Drawing;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;
using SIL.Windows.Forms;
using SayMore.Media.Audio;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Media;
using SayMore.UI.ComponentEditors;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class StartAnnotatingEditor : EditorBase
	{
		private readonly Project _project;

		/// ------------------------------------------------------------------------------------
		public StartAnnotatingEditor(ComponentFile file, Project project) :
			base(file, null, null)
		{
			_project = project;
			Logger.WriteEvent("OralAnnotationEditor constructor. file = {0}", file);
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

			PopulateAudacityLabelTierItems();

			_labelSegmentationMethod.Font = FontHelper.MakeFont(Program.DialogFont, 10, FontStyle.Bold);
			_labelIntroduction.Font = Program.DialogFont;
			_labelSegmentationMethodQuestion.Font = Program.DialogFont;
			_radioButtonManual.Font = Program.DialogFont;
			_radioButtonCarefulSpeech.Font = Program.DialogFont;
			_radioButtonElan.Font = Program.DialogFont;
			_radioButtonAudacity.Font = Program.DialogFont;
			_labelAudacityLabelTier.Font = Program.DialogFont;
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
		protected override void HandleStringsLocalized(ILocalizationManager lm)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
			{
				TabText = CommonUIStrings.StartAnnotatingTabText;

                if (_cboAudacityLabelTier != null)
                {
                    var selectedIndex = _cboAudacityLabelTier.SelectedIndex;
                    _cboAudacityLabelTier.Items.Clear();
                    PopulateAudacityLabelTierItems();
                    _cboAudacityLabelTier.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
                }
            }

			base.HandleStringsLocalized(lm);
		}

        private void PopulateAudacityLabelTierItems()
        {
            _cboAudacityLabelTier.Items.AddRange(new [] {
                CommonUIStrings.TranscriptionTierDisplayName,
                CommonUIStrings.TranslationTierDisplayName });
            _cboAudacityLabelTier.Size = _cboAudacityLabelTier.PreferredSize;
			if (_radioButtonAudacity.Checked)
                _cboAudacityLabelTier.SelectedIndex = 0;
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
				newAnnotationFile = ManualSegmenterDlg.ShowDialog(_file, this, -1);
			}
			else if (_radioButtonCarefulSpeech.Checked)
			{
				Settings.Default.DefaultSegmentationMethod = 1;
				newAnnotationFile = (!AudioUtils.GetCanRecordAudio()) ? null :
					_file.RecordAnnotations(FindForm(), AudioRecordingType.Careful);
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

				newAnnotationFile = GetAudacityOrElanFile(caption, filetype,
                    _cboAudacityLabelTier.SelectedIndex == 0 ? TierType.Transcription :
                        TierType.FreeTranslation);
				Settings.Default.DefaultSegmentationMethod = 3;
			}
			else if (_radioButtonAutoSegmenter.Checked)
			{
				var segmenter = new AutoSegmenter(_file, _project);
				newAnnotationFile = segmenter.Run();
				Settings.Default.DefaultSegmentationMethod = 4;
			}

			if (newAnnotationFile != null && ComponentFileListRefreshAction != null)
				ComponentFileListRefreshAction(newAnnotationFile, null);

			_buttonGetStarted.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <param name="tierType">If creating from an Audacity file, the tier type specifies
		/// which tier the label text goes into.</param>
        /// ------------------------------------------------------------------------------------
		private string GetAudacityOrElanFile(string caption, string filter,
            TierType tierType = default)
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
					AnnotationFileHelper.CreateFileFromFile(dlg.FileName, _file.PathToAnnotatedFile, tierType));
			}
		}

        private void _radioButtonAudacity_CheckedChanged(object sender, EventArgs e)
        {
            _labelAudacityLabelTier.Enabled = _cboAudacityLabelTier.Enabled =
                _radioButtonAudacity.Checked;

            if (_radioButtonAudacity.Checked && _cboAudacityLabelTier.Items.Count > 0 &&
                _cboAudacityLabelTier.SelectedIndex < 0)
            {
                _cboAudacityLabelTier.SelectedIndex = 0;
            }
        }
    }
}
