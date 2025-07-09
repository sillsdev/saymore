using DesktopAnalytics;
using SIL.Reporting;

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
			Analytics.Track(nameof(CarefulSpeechRecorderDlg));

			InitializeComponent();
			Opacity = 0D;

			InitializeRecordingLabel(_labelCarefulSpeech);
		}
	}
}
