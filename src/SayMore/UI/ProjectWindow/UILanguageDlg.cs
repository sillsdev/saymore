using System.Globalization;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;

namespace SayMore.UI.ProjectWindow
{
	public partial class UILanguageDlg : Form
	{
		private readonly ILocalizationManager _localizationManager;

		public delegate UILanguageDlg Factory(); //autofac uses this
		public string UILanguage { get; private set; }

		/// ------------------------------------------------------------------------------------
		public UILanguageDlg(ILocalizationManager localizationManager)
		{
			Logger.WriteEvent("UILanguageDlg constructor");

			_localizationManager = localizationManager;
			InitializeComponent();

			_labelLanguage.Font = Program.DialogFont;
			_linkIWantToLocalize.Font = Program.DialogFont;
			if (!localizationManager.CanCustomizeLocalizations)
			{
				_linkIWantToLocalize.Text = LocalizationManager.GetString(
					"DialogBoxes.UserInterfaceLanguageDlg.ViewLocalizationsLink",
					"View SayMore localizations...");
			}
			_linkHelpOnLocalizing.Font = Program.DialogFont;
			_comboUILanguage.Font = Program.DialogFont;
			_comboUILanguage.SelectedItem = CultureInfo.GetCultureInfo(LocalizationManager.UILanguageId);
			DialogResult = DialogResult.Cancel;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleIWantToLocalizeLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_localizationManager.ShowLocalizationDialogBox(false);
			_comboUILanguage.RefreshList();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.OK)
				UILanguage = ((L10NCultureInfo)_comboUILanguage.SelectedItem).Name;

			base.OnFormClosing(e);
		}
	}
}
