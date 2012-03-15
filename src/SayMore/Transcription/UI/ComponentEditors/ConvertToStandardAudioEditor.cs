using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Localization;
using Palaso.Reporting;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class ConvertToStandardAudioEditor : EditorBase
	{
		private readonly ConvertToStandardAudioEditorViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		public ConvertToStandardAudioEditor(ComponentFile file) : base(file, null, null)
		{
			_viewModel = new ConvertToStandardAudioEditorViewModel();

			InitializeComponent();
			Name = "StartAnnotating";

			_tableLayoutConvert.Dock = DockStyle.Top;
			_pictureInfo.Image = SystemIcons.Information.ToBitmap();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			_labelConvertHeading.Font = FontHelper.MakeFont(SystemFonts.IconTitleFont, 10, FontStyle.Bold);
			_labelConvertIntroduction.Font = SystemFonts.IconTitleFont;
			_labelOriginalFileName.Font = SystemFonts.IconTitleFont;
			_labelOriginalFileNameValue.Font = FontHelper.MakeFont(SystemFonts.IconTitleFont, FontStyle.Bold);
			_labelStandardAudioFileName.Font = SystemFonts.IconTitleFont;
			_labelStandardAudioFileNameValue.Font = _labelOriginalFileNameValue.Font;
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			_labelConvertIntroduction.Text = GetIntroMessage();
			_labelOriginalFileNameValue.Text = Path.GetFileName(_file.PathToAnnotatedFile);
			_labelStandardAudioFileNameValue.Text = Path.GetFileName(_file.GetSuggestedPathToStandardAudioFile());
		}

		/// ------------------------------------------------------------------------------------
		private string GetIntroMessage()
		{
			string text;
			var suffix = Path.GetFileNameWithoutExtension(Settings.Default.StandardAudioFileSuffix);

			if (_file.FileType.IsVideo)
			{
				text = LocalizationManager.GetString(
					"EventsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio._labelIntroduction.ForVideo",
					"In order to annotate, SayMore needs to convert this video into to WAV PCM audio. " +
					"During the conversion process, a standard audio file will be created from the " +
					"original and added to the event's file list. The name of the new audio file will " +
					"be that of the original with the suffix \"{0}\" added to the end. The original " +
					"file will remain unchanged in the event's file list.", null, _labelConvertIntroduction);

				return string.Format(text, suffix);
			}

			var encoding = _viewModel.GetAudioFileEncoding(_file.PathToAnnotatedFile);

			text = LocalizationManager.GetString(
				"EventsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio._labelIntroduction.ForAudio",
				"The format of this audio file is '{0}'. In order to annotate, SayMore needs to convert " +
				"it to WAV PCM, which is a good, standard choice for archiving. During the conversion " +
				"process, a standard audio file will be created from the original and added to the " +
				"event's file list. The name of the new audio file will be that of the original with " +
				"the suffix \"{1}\" added to the end. The original file will remain unchanged in the " +
				"event's file list.", null, _labelConvertIntroduction);

			return string.Format(text, encoding, suffix);
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsOKSToShow
		{
			get
			{
				return (_file != null && !_file.GetDoesHaveAnnotationFile() &&
					_file.GetNeedsConvertingToStandardAudio() &&
					!File.Exists(_file.GetSuggestedPathToStandardAudioFile()));
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString(
				"EventsView.Transcription.StartAnnotatingTab.TabText", "Start Annotating");

			base.HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleConvertButtonClick(object sender, EventArgs e)
		{
			_buttonConvert.Enabled = true;

			var msg = LocalizationManager.GetString(
				"EventsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio.ConvertingMsg",
				"Converting...");

			var error = _viewModel.Convert(this, _file, msg);
			if (error == null)
			{
				if (ComponentFileListRefreshAction != null)
				{
					ComponentFileListRefreshAction(
						_file.GetSuggestedPathToStandardAudioFile(), typeof(StartAnnotatingEditor));
				}

				return;
			}

			msg = LocalizationManager.GetString(
				"EventsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio.ConversionErrorMsg",
				"There was an error trying to create a standard audio file from:\r\n\r\n{0}");

			ErrorReport.NotifyUserOfProblem(error, msg, _file.PathToAnnotatedFile);
		}
	}
}
