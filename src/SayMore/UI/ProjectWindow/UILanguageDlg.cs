using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace SayMore.UI.ProjectWindow
{
	public partial class UILanguageDlg : Form
	{
		public string UILanguage { get; private set; }

		/// ------------------------------------------------------------------------------------
		public UILanguageDlg()
		{
			InitializeComponent();

			_labelLanguage.Font = SystemFonts.IconTitleFont;
			_linkIWantToLocalize.Font = SystemFonts.IconTitleFont;
			_linkHelpOnLocalizing.Font = SystemFonts.IconTitleFont;
			_comboUILanguage.Font = SystemFonts.IconTitleFont;
			_comboUILanguage.SelectedItem = CultureInfo.GetCultureInfo(Program.GetUILanguageId());
			DialogResult = DialogResult.Cancel;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleIWantToLocalizeLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Program.ShowLocalizationDialogBox();
			_comboUILanguage.RefreshList();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.OK)
				UILanguage = ((CultureInfo)_comboUILanguage.SelectedItem).TwoLetterISOLanguageName;

			base.OnFormClosing(e);
		}
	}
}
