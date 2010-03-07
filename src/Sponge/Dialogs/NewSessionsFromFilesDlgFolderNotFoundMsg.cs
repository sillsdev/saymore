using System.Windows.Forms;

namespace SIL.Sponge.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class NewSessionsFromFilesDlgFolderNotFoundMsg : UserControl
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFilesDlgFolderNotFoundMsg()
		{
			InitializeComponent();
			Dock = DockStyle.Fill;

			m_overviewMessageLabel.Text
				= string.Format(m_overviewMessageLabel.Text, Application.ProductName);
		}
	}
}
