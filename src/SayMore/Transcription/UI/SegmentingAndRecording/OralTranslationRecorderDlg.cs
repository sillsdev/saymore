using DesktopAnalytics;
using SIL.Reporting;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class OralTranslationRecorderDlg : OralAnnotationRecorderBaseDlg
	{
		/// ------------------------------------------------------------------------------------
		public OralTranslationRecorderDlg(OralAnnotationRecorderDlgViewModel viewModel)
			: base(viewModel)
		{
			Logger.WriteEvent("OralTranslationRecorderDlg constructor. ComponentFile = {0}", viewModel.ComponentFile);
			Analytics.Track(nameof(OralTranslationRecorderDlg));

			InitializeComponent();
			Opacity = 0D;

			InitializeRecordingLabel(_labelOralTranslation);
		}
	}
}
