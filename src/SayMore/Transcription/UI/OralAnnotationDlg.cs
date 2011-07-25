using System.Windows.Forms;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		public OralAnnotationDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public OralAnnotationDlg(TimeOrderTier tier) : this()
		{
			_oralAnnotationRecorder.Initialize(new OralAnnotationRecorderViewModel(tier));
		}
	}
}
