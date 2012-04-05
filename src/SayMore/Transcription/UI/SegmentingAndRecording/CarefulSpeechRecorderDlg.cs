
using System.Windows.Forms;
using Localization;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class CarefulSpeechRecorderDlg : OralAnnotationRecorderBaseDlg
	{
		/// ------------------------------------------------------------------------------------
		public CarefulSpeechRecorderDlg(OralAnnotationRecorderDlgViewModel viewModel)
			: base(viewModel)
		{
			InitializeComponent();
			Opacity = 0D;

			_labelCarefulSpeech.Margin = _labelOriginalRecording.Margin;
			_labelCarefulSpeech.TextAlign = _labelOriginalRecording.TextAlign;
			_labelCarefulSpeech.Anchor = _labelOriginalRecording.Anchor;
			_tableLayoutRecordAnnotations.RowStyles[0].SizeType = SizeType.AutoSize;
			_tableLayoutRecordAnnotations.Controls.Add(_labelCarefulSpeech, 0, 0);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);
			_labelCarefulSpeech.Font = _labelOriginalRecording.Font;
		}

		/// ------------------------------------------------------------------------------------
		protected override string ReadyToRecordMessage
		{
			get
			{
				return LocalizationManager.GetString(
					"DialogBoxes.Transcription.CarefulSpeechRecorderDlg.ReadyToRecordAnnotationMsg",
					"Ready to Record\r\nCareful Speech");
			}
		}
	}
}
