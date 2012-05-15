using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Localization;

namespace SayMore.UI.ProjectWindow
{
	public partial class UILanguageDlg : Form
	{
		public string UILanguage { get; private set; }

		/// ------------------------------------------------------------------------------------
		public UILanguageDlg()
		{
			InitializeComponent();

			_labelLanguage.Font = Program.DialogFont;
			_linkIWantToLocalize.Font = Program.DialogFont;
			_linkHelpOnLocalizing.Font = Program.DialogFont;
			_comboUILanguage.Font = Program.DialogFont;
			_comboUILanguage.SelectedItem = CultureInfo.GetCultureInfo(LocalizationManager.UILanguageId);
			DialogResult = DialogResult.Cancel;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleIWantToLocalizeLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			LocalizationManager.ShowLocalizationDialogBox();
			_comboUILanguage.RefreshList();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.OK)
				UILanguage = ((CultureInfo)_comboUILanguage.SelectedItem).Name;

			base.OnFormClosing(e);
		}
	}
}
