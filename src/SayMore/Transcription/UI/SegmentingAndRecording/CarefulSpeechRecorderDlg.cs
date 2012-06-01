using SayMore.Transcription.Model;

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

			InitializeRecordingLabel(_labelCarefulSpeech);
		}

		/// ------------------------------------------------------------------------------------
		public virtual OralAnnotationType AnnotationType
		{
			get { return Model.OralAnnotationType.CarefulSpeech; }
		}
	}
}
