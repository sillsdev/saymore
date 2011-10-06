using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
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
			_comboUILanguage.Font = SystemFonts.IconTitleFont;
			_comboUILanguage.SelectedItem = CultureInfo.GetCultureInfo(Program.GetUILanguageId());
			DialogResult = DialogResult.Cancel;
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
