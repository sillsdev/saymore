
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
		}
	}
}
