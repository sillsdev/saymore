using System.Windows.Forms;
using L10NSharp;
using SayMore.Model.Files;
using SayMore.Transcription.Model;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class MissingMediaFileEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public MissingMediaFileEditor(ComponentFile file, string imageKey)
			: base(file, null, imageKey)
		{
			InitializeComponent();
			SetComponentFile(file);
			HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);
			txtMissingMediaFilePath.Text = file.PathToAnnotatedFile.Replace(
				AnnotationFileHelper.kAnnotationsEafFileSuffix, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnVisibleChanged(System.EventArgs e)
		{
			base.OnVisibleChanged(e);
			ReselectFilePathAndScrollIntoViewAsMuchAsPossible();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(System.EventArgs e)
		{
			base.OnSizeChanged(e);
			ReselectFilePathAndScrollIntoViewAsMuchAsPossible();
		}

		/// ------------------------------------------------------------------------------------
		private void ReselectFilePathAndScrollIntoViewAsMuchAsPossible()
		{
			txtMissingMediaFilePath.Select(0, 0);
			txtMissingMediaFilePath.Select(0, txtMissingMediaFilePath.Text.Length);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleHelpTopicLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Program.ShowHelpTopic("/Concepts/ELAN.htm");
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			base.HandleStringsLocalized();

			TabText = LocalizationManager.GetString("SessionsView.MissingMediaFileEditor.TabText", "Missing Media File");
			if (lblExplanation == null)
				return;

			lblExplanation.Text = LocalizationManager.GetString("SessionsView.MissingMediaFileEditor.lblExplanation",
				"This can happen if the media file is inadvertently deleted or renamed outside of SayMore. " +
				"It could also happen if a properly named ELAN file is added to a SayMore session but internally " +
				"refers to a media file that is not where SayMore expects to find it. If you have access to the media " +
				"file and would like to be able to annotate it in SayMore, please copy it to the above location.");

		}
	}
}
