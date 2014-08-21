using Palaso.Reporting;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class CarefulSpeechRecorderDlg : OralAnnotationRecorderBaseDlg
	{
		/// ------------------------------------------------------------------------------------
		public CarefulSpeechRecorderDlg(OralAnnotationRecorderDlgViewModel viewModel)
			: base(viewModel)
		{
			Logger.WriteEvent("CarefulSpeechRecorderDlg constructor. ComponentFile = {0}", viewModel.ComponentFile);
			InitializeComponent();
			Opacity = 0D;

			InitializeRecordingLabel(_labelCarefulSpeech);
		}
	}
}
