using System;
using System.IO;
using System.Windows.Forms;

namespace SIL.Sponge.Dialogs
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
		private readonly string m_msg2TextForFormat;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFilesDlgFolderNotFoundMsg()
		{
			InitializeComponent();
			Dock = DockStyle.Fill;

			m_msg2TextForFormat = m_possibleProblemsMsg2Label.Text;

			m_problemOverviewMsgLabel.Text =
				string.Format(m_problemOverviewMsgLabel.Text, Application.ProductName);

			m_possibleProblemsMsg1Label.LeftMargin = m_problemOverviewMsgLabel.TextsXOffset;
			m_possibleProblemsMsg2Label.LeftMargin = m_problemOverviewMsgLabel.TextsXOffset;
			m_possibleProblemsMsg3Label.LeftMargin = m_problemOverviewMsgLabel.TextsXOffset;
			m_driveLetterHintMsgLabel.LeftMargin = m_problemOverviewMsgLabel.TextsXOffset + 30;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Extracts the drive letter from the specified path and displays it in one of the
		/// messages telling the user what may be the problem.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetDriveLetterFromPath(string path)
		{
			var driveLetter = (path == null ? string.Empty : Path.GetPathRoot(path));
			driveLetter = driveLetter.TrimEnd(Path.DirectorySeparatorChar, Path.VolumeSeparatorChar);
			m_possibleProblemsMsg2Label.Text = string.Format(m_msg2TextForFormat, driveLetter);
		}
	}
}
