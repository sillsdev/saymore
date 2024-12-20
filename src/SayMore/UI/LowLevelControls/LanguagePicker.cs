// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2024, SIL Global. All Rights Reserved.
// <copyright from='2024' to='2014' company='SIL Global'>
//		Copyright (c) 2024, SIL Global. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (https://sil.mit-license.org/)
// </copyright>
#endregion
// --------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SIL.IO;
using SIL.Reporting;
using static System.String;
using static SayMore.Program;

namespace SayMore.UI.LowLevelControls
{
	public partial class LanguagePicker : Form
	{
		private bool m_webBrowserReady;
		private string m_htmlTemplate;

		public string LanguageToFind { get; set; }
		public string Result { get; set; }

		public LanguagePicker()
		{
			InitializeComponent();
			//webViewLanguage.Source = new Uri($"file:///{path.Replace("\\", "/")}");
		}

		private async void OnLoad(object sender, EventArgs e)
		{
			var htmlPath = FileLocationUtilities.GetFileDistributedWithApplication("LanguageChooser.html");

			m_htmlTemplate = File.ReadAllText(htmlPath);
				//.Replace("??", "???");

			await InitializeWebBrowserAsync();
			m_webBrowserReady = true;
			SetHtmlDocument();
		}

		private async Task InitializeWebBrowserAsync()
		{
			try
			{
				await webViewLanguage.EnsureCoreWebView2Async(WebView2Environment);
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalException(e);
			}
		}

		private void SetHtmlDocument()
		{
			if (!m_webBrowserReady)
				return;

			InitializeWebBrowserUserInteractionSettings();

			var htmlContents = Format(m_htmlTemplate);

			try
			{
				//webViewLanguage.CoreWebView2.SetVirtualHostNameToFolderMapping(kTempResources,
				//	Path.GetDirectoryName(m_tempTxlLogoPath),
				//	CoreWebView2HostResourceAccessKind.Allow);
				webViewLanguage.NavigateToString(htmlContents);
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalException(e);
			}
		}

		public void InitializeWebBrowserUserInteractionSettings()
		{
#if DEBUG
			webViewLanguage.CoreWebView2.Settings.AreDevToolsEnabled = true;
#else
			webViewLanguage.CoreWebView2.Settings.AreDevToolsEnabled = false;
#endif
		}
	}
}
