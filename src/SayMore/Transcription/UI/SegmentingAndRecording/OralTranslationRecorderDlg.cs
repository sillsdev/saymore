
using Localization;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class OralTranslationRecorderDlg : OralAnnotationRecorderBaseDlg
	{
		/// ------------------------------------------------------------------------------------
		public OralTranslationRecorderDlg(OralAnnotationRecorderDlgViewModel viewModel)
			: base(viewModel)
		{
			InitializeComponent();
			Opacity = 0D;
		}

		/// ------------------------------------------------------------------------------------
		protected override string ReadyToRecordMessage
		{
			get
			{
				return LocalizationManager.GetString(
					"DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.ReadyToRecordAnnotationMsg.OralTranslation",
					"Ready to Record\r\nOral Translation");
			}
		}
	}
}
