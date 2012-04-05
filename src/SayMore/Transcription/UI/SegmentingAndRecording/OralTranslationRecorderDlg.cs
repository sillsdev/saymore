
using System.Windows.Forms;
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

			_labelOralTranslation.Margin = _labelOriginalRecording.Margin;
			_labelOralTranslation.TextAlign = _labelOriginalRecording.TextAlign;
			_labelOralTranslation.Anchor = _labelOriginalRecording.Anchor;
			_tableLayoutRecordAnnotations.RowStyles[0].SizeType = SizeType.AutoSize;
			_tableLayoutRecordAnnotations.Controls.Add(_labelOralTranslation, 0, 0);
		}

		/// ------------------------------------------------------------------------------------
		protected override string ReadyToRecordMessage
		{
			get
			{
				return LocalizationManager.GetString(
					"DialogBoxes.Transcription.OralTranslationRecorderDlg.ReadyToRecordAnnotationMsg",
					"Ready to Record\r\nOral Translation");
			}
		}
	}
}
