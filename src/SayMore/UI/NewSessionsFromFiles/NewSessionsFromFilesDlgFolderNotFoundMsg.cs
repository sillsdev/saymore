using System.IO;
using System.Windows.Forms;

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
			InitializeComponent();
			Dock = DockStyle.Fill;

			_msg2TextForFormat = _possibleProblemsMsg2Label.Text;

			_problemOverviewMsgLabel.Text =
				string.Format(_problemOverviewMsgLabel.Text, Application.ProductName);

			_possibleProblemsMsg1Label.LeftMargin = _problemOverviewMsgLabel.TextsXOffset;
			_possibleProblemsMsg2Label.LeftMargin = _problemOverviewMsgLabel.TextsXOffset;
			_possibleProblemsMsg3Label.LeftMargin = _problemOverviewMsgLabel.TextsXOffset;
			_driveLetterHintMsgLabel.LeftMargin = _problemOverviewMsgLabel.TextsXOffset + 30;
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
			_possibleProblemsMsg2Label.Text = string.Format(_msg2TextForFormat, driveLetter);
		}
	}
}
