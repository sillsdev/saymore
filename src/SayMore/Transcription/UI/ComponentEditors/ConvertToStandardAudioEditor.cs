using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Localization;
using Palaso.Reporting;
using SayMore.AudioUtils;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI;
using SayMore.UI.ComponentEditors;
using SilTools;

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

			var text = LocalizationManager.GetString(
				"EventsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio._labelIntroduction",
				"This media file is not in a format that can be annotated and needs to be " +
				"converted. During the conversion process, a standard audio file will be created " +
				"from the original and added to the event's file list. The name of the new audio " +
				"file will be that of the original with the suffix \"{0}\" added to the end. The " +
				"original file will remain unchanged in the event's file list.",
				null, _labelConvertIntroduction);

			_labelConvertIntroduction.Text = string.Format(text,
				Path.GetFileNameWithoutExtension(Settings.Default.StandardAudioFileSuffix));

			_labelOriginalFileNameValue.Text = Path.GetFileName(_file.PathToAnnotatedFile);
			_labelStandardAudioFileNameValue.Text = Path.GetFileName(_file.GetSuggestedPathToStandardAudioFile());
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

			var pathToStdAudioFile = _file.GetSuggestedPathToStandardAudioFile();
			var mediaInfo = new MediaFileInfo(_file.PathToAnnotatedFile);
			var format = WaveFileUtils.GetDefaultWaveFormat(mediaInfo.Channels);
			Exception error;

			var msg = LocalizationManager.GetString(
				"EventsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio.ConvertingMsg",
				"Converting...");

			using (var dlg = new LoadingDlg(msg))
			{
				dlg.Show(this);

				WaveFileUtils.GetPlainPcmStream(_file.PathToAnnotatedFile,
					pathToStdAudioFile, format, out error);

				dlg.Close();
			}

			if (error == null && File.Exists(pathToStdAudioFile))
			{
				if (ComponentFileListRefreshAction != null)
					ComponentFileListRefreshAction(pathToStdAudioFile, typeof(StartAnnotatingEditor));

				return;
			}

			msg = LocalizationManager.GetString(
				"EventsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio.ConversionErrorMsg",
				"There was an error trying to create a standard audio file from:\r\n\r\n{0}");

			ErrorReport.NotifyUserOfProblem(error, msg, _file.PathToAnnotatedFile);

			if (File.Exists(pathToStdAudioFile))
				File.Delete(pathToStdAudioFile);
		}
	}
}
