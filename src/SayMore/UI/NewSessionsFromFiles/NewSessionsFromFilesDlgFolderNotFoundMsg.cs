using System.IO;
using System.Windows.Forms;
using Palaso.Reporting;

namespace SayMore.UI.NewSessionsFromFiles
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This control contains the text of the message displayed in the NewSessionsFromFilesDlg
	/// when that dialog box is unable to find the location where files were found the last
	/// time the user created sessions from files.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class NewSessionsFromFilesDlgFolderNotFoundMsg : UserControl
	{
		private readonly string _msg2TextForFormat;

		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFilesDlgFolderNotFoundMsg()
		{
			Logger.WriteEvent("NewSessionsFromFilesDlgFolderNotFoundMsg constructor");

			InitializeComponent();
			Dock = DockStyle.Fill;

			_msg2TextForFormat = _labelPossibleProblemsMsg2.Text;

			_labelPossibleProblemsMsg1.LeftMargin = _labelProblemOverviewMsg.TextsXOffset;
			_labelPossibleProblemsMsg2.LeftMargin = _labelProblemOverviewMsg.TextsXOffset;
			_labelPossibleProblemsMsg3.LeftMargin = _labelProblemOverviewMsg.TextsXOffset;
			_labelDriveLetterHintMsg.LeftMargin = _labelProblemOverviewMsg.TextsXOffset + 30;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Extracts the drive letter from the specified path and displays it in one of the
		/// messages telling the user what may be the problem.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetDriveLetterFromPath(string path)
		{
			var driveLetter = (string.IsNullOrEmpty(path) ? string.Empty : Path.GetPathRoot(path));
			driveLetter = driveLetter.TrimEnd(Path.DirectorySeparatorChar, Path.VolumeSeparatorChar);
			_labelPossibleProblemsMsg2.Text = string.Format(_msg2TextForFormat, driveLetter);
		}
	}
}
