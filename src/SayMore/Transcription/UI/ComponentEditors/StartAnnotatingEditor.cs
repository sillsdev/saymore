using System;
using System.Drawing;
using System.Windows.Forms;
using L10NSharp;
using Palaso.UI.WindowsForms;
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
					_file.GetCanHaveAnnotationFile());
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
			ExternalProcess.CleanUpAllProcesses();
		}
	}
}
