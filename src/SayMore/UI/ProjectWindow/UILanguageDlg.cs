using System;
using System.Diagnostics;
using System.Drawing;
using L10NSharp;
using SayMore.Utilities;
using SIL.Reporting;
using System.Globalization;
using System.Windows.Forms;
using static System.Windows.Forms.DialogResult;

namespace SayMore.UI.ProjectWindow
{
	public partial class UILanguageDlg : Form
	{
		private float _baseLineFontSize;
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
			_numUpDownScale.Value = Properties.Settings.Default.UITextScale;
			
			DialogResult = Cancel;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			_baseLineFontSize = Font.Size;
			ScaleFonts();
		}
		
		/// ------------------------------------------------------------------------------------
		protected void ScaleFonts()
		{
			try
			{
				var scale = (int)_numUpDownScale.Value / 100f;
				var newFont = new Font(Font.FontFamily, _baseLineFontSize * scale, Font.Style);
				this.ApplyFontRecursively(newFont, _baseLineFontSize, scale);
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalException(e);
			}
		}

		private void HandleIWantToLocalizeLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://crowdin.com/project/saymore");
			_comboUILanguage.RefreshList();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (DialogResult == OK)
			{
				UILanguage = ((L10NCultureInfo)_comboUILanguage.SelectedItem).Name;
				Properties.Settings.Default.UITextScale = (int)_numUpDownScale.Value;
			}

			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void _btnReset_Click(object sender, EventArgs e)
		{
			_numUpDownScale.Value = 100;
		}

		/// ------------------------------------------------------------------------------------
		private void _numUpDownScale_ValueChanged(object sender, EventArgs e)
		{
			// This effectively gives a preview.
			ScaleFonts();
		}
	}
}
