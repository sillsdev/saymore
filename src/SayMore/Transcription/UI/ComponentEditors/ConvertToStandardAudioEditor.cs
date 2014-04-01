using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DesktopAnalytics;
using L10NSharp;
using Palaso.Reporting;
using Palaso.UI.WindowsForms;
using SayMore.Media.Audio;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;
namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class ConvertToStandardAudioEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public ConvertToStandardAudioEditor(ComponentFile file) : base(file, null, null)
		{
			InitializeComponent();
			Name = "StartAnnotating";

			_tableLayoutConvert.Dock = DockStyle.Top;
			_pictureInfo.Image = SystemIcons.Information.ToBitmap();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			_labelConvertHeading.Font = FontHelper.MakeFont(Program.DialogFont, 10, FontStyle.Bold);
			_labelConvertIntroduction.Font = Program.DialogFont;
			_labelSourceFileName.Font = Program.DialogFont;
			_labelSourceFileNameValue.Font = FontHelper.MakeFont(Program.DialogFont, FontStyle.Bold);
			_labelStandardAudioFileName.Font = Program.DialogFont;
			_labelStandardAudioFileNameValue.Font = _labelSourceFileNameValue.Font;

			SetComponentFile(_file);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			_labelConvertIntroduction.Text = GetIntroMessage();
			_labelSourceFileNameValue.Text = Path.GetFileName(_file.PathToAnnotatedFile);
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
					"SessionsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio._labelIntroduction.ForVideo",
					"In order to annotate, SayMore needs to convert this video to WAV PCM audio. " +
					"During the conversion process, a standard audio file will be created from the " +
					"source and added to the session's file list. The name of the new audio file will " +
					"be the same as the name of the source with the suffix \"{0}\" added to the end. The source " +
					"file will remain unchanged in the session's file list.", null, _labelConvertIntroduction);

				return string.Format(text, suffix);
			}

			var encoding = AudioUtils.GetAudioEncoding(_file.PathToAnnotatedFile);

			text = LocalizationManager.GetString(
				"SessionsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio._labelIntroduction.ForAudio",
				"The format of this audio file is '{0}'. In order to annotate, SayMore needs to convert " +
				"it to WAV PCM, which is a good, standard choice for archiving. During the conversion " +
				"process, a standard audio file will be created from the source and added to the " +
				"session's file list. The name of the new audio file will be the same as the source with " +
				"the suffix \"{1}\" added to the end. The source file will remain unchanged in the " +
				"session's file list.", null, _labelConvertIntroduction);

			return string.Format(text, encoding, suffix);
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsOKToShow
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
				"SessionsView.Transcription.StartAnnotatingTab.TabText", "Start Annotating");

			base.HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleConvertButtonClick(object sender, EventArgs e)
		{
			Analytics.Track("Convert to standard audio", new Dictionary<string, string> {
				{"fileExtension", Path.GetExtension(_file.PathToAnnotatedFile) }});

			_buttonConvert.Enabled = true;

			var error = AudioUtils.ConvertToStandardPCM(_file.PathToAnnotatedFile,
				_file.GetSuggestedPathToStandardAudioFile(), this,
				AudioUtils.GetConvertingToStandardPcmAudioMsg());

			if (error == null)
			{
				if (ComponentFileListRefreshAction != null)
				{
					ComponentFileListRefreshAction(
						_file.GetSuggestedPathToStandardAudioFile(), typeof(StartAnnotatingEditor));
				}

				return;
			}

			ErrorReport.NotifyUserOfProblem(error,
				AudioUtils.GetConvertingToStandardPcmAudioErrorMsg(), _file.PathToAnnotatedFile);
		}
	}
}
