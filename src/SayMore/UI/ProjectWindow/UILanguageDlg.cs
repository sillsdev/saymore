using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;

namespace SayMore.UI.ProjectWindow
{
	public partial class UILanguageDlg : Form
	{
		public delegate UILanguageDlg Factory(); //autofac uses this
		public string UILanguage { get; private set; }

		public UILanguageDlg()
		{
			Logger.WriteEvent("UILanguageDlg constructor");
			InitializeComponent();
			_labelLanguage.Font = Program.DialogFont;
			_linkIWantToLocalize.Font = Program.DialogFont;
			_comboUILanguage.Font = Program.DialogFont;
			_comboUILanguage.SelectedItem = CultureInfo.GetCultureInfo(LocalizationManager.UILanguageId);
			DialogResult = DialogResult.Cancel;
		}

		private void HandleIWantToLocalizeLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://crowdin.com/project/saymore");
			_comboUILanguage.RefreshList();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.OK)
				UILanguage = ((L10NCultureInfo)_comboUILanguage.SelectedItem).Name;

			base.OnFormClosing(e);
		}
	}
}
