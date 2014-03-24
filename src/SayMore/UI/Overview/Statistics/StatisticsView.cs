using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using L10NSharp.UI;

namespace SayMore.UI.Overview.Statistics
{
	public partial class StatisticsView : UserControl
	{
		private readonly StatisticsViewModel _model;
		private bool _updateDisplayNeeded;

		/// ------------------------------------------------------------------------------------
		public StatisticsView(StatisticsViewModel model)
		{
			_model = model;
			InitializeComponent();

			_panelWorking.BorderStyle = BorderStyle.None;
		}

		// SP-788: "Cannot access a disposed object" when changing UI language
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);

			LocalizeItemDlg.StringsLocalized -= UpdateDisplay;

			if (_refreshTimer != null)
			{
				_refreshTimer.Dispose();
				_refreshTimer = null;
			}

			if (_webBrowser != null)
			{
				_webBrowser.Dispose();
				_webBrowser = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void InitializeView()
		{
			_model.FinishedGatheringStatisticsForAllFiles += HandleFinishedGatheringStatisticsForAllFiles;

			if (!_model.IsDataUpToDate)
				UpdateStatusDisplay();
			else
			{
				_model.NewStatisticsAvailable += HandleNewDataAvailable;
				UpdateDisplay();
			}

			LocalizeItemDlg.StringsLocalized += UpdateDisplay;
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateDisplay()
		{
			_updateDisplayNeeded = false;

			if (_webBrowser.DocumentStream != null)
				_webBrowser.DocumentStream.Dispose();

			_webBrowser.DocumentStream = new MemoryStream(Encoding.UTF8.GetBytes(_model.HTMLString));

			if (_webBrowser.Document != null)
				_webBrowser.Document.Encoding = "utf-8";

			UpdateStatusDisplay();
			_model.NewStatisticsAvailable -= HandleNewDataAvailable;
			_model.NewStatisticsAvailable += HandleNewDataAvailable;
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateStatusDisplay()
		{
			var showRefreshButton = _model.IsDataUpToDate;
			_buttonRefresh.Visible = showRefreshButton;
			_panelWorking.Visible = !showRefreshButton;
		}

		/// ------------------------------------------------------------------------------------
		public void HandleCopyToClipboardClick(object sender, EventArgs e)
		{
			if (_webBrowser.Document == null || _model.IsBusy)
				return;

			var header =
				"Version:0.9" + Environment.NewLine +
				"StartHTML:00000000" + Environment.NewLine +
				"EndHTML:00000001" + Environment.NewLine +
				"StartFragment:00000002" + Environment.NewLine +
				"EndFragment:00000003" + Environment.NewLine +
				"<!--StartFragment-->";

			int start = Encoding.UTF8.GetByteCount(header);
			header = header.Replace("00000000", start.ToString("D8"));
			header = header.Replace("00000002", start.ToString("D8"));

			int end = start + Encoding.UTF8.GetByteCount(_webBrowser.DocumentText);
			header = header.Replace("00000001", end.ToString("D8"));
			header = header.Replace("00000003", end.ToString("D8"));

			Clipboard.SetData(DataFormats.Html, header + _webBrowser.DocumentText + "<!--EndFragment-->");
		}

		/// ------------------------------------------------------------------------------------
		public void HandlePrintButtonClicked(object sender, EventArgs e)
		{
			if (_webBrowser.Document == null || _model.IsBusy)
				return;

#if !__MonoCS__
			const string regKeyPath = @"Software\Microsoft\Internet Explorer\PageSetup";

			var regKey = Registry.CurrentUser.OpenSubKey(regKeyPath, true);
			var isIEPageSetupSetToPrintingBkgndColor =
				regKey != null && ((string)regKey.GetValue("Print_Background", "no")).ToLowerInvariant() == "yes";

			if (!isIEPageSetupSetToPrintingBkgndColor)
				if (regKey != null) regKey.SetValue("Print_Background", "yes", RegistryValueKind.String);
#endif

			_webBrowser.ShowPrintDialog();

#if !__MonoCS__
			if (!isIEPageSetupSetToPrintingBkgndColor)
				if (regKey != null) regKey.SetValue("Print_Background", "no", RegistryValueKind.String);

			if (regKey != null) regKey.Close();
#endif
		}

		/// ------------------------------------------------------------------------------------
		public void HandleSaveButtonClicked(object sender, EventArgs e)
		{
			if (_webBrowser.Document == null || _model.IsBusy)
				return;

			// I could use the browser's ShowSaveAsDialog method, but that
			// doesn't give me as much control over the dialog's settings.
			using (var dlg = new System.Windows.Forms.SaveFileDialog())
			{
				dlg.DefaultExt = "html";
				dlg.Filter = @"HTML File (*.html)|*.html|All Files (*.*)|*.*";
				dlg.FileName = Path.ChangeExtension(_webBrowser.DocumentTitle, "html");
				dlg.OverwritePrompt = true;
				if (dlg.ShowDialog() == DialogResult.OK)
					File.WriteAllText(dlg.FileName, _webBrowser.DocumentText);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRefreshButtonClicked(object sender, EventArgs e)
		{
			if (_model.IsBusy)
				return;

			_model.NewStatisticsAvailable -= HandleNewDataAvailable;
			_model.Refresh();
			_webBrowser.DocumentText = string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTimerTick(object sender, EventArgs e)
		{
			//SP-866: Program crashes or freezes after progress tab is displayed.
			try
			{
				_refreshTimer.Enabled = false;
				_refreshTimer.Interval = IsReallyVisible() ? 3000 : 1000;

				if (_refreshTimer.Interval > 1000)
				{
					UpdateStatusDisplay();

					if (_updateDisplayNeeded)
						UpdateDisplay();
				}
			}
			finally
			{
				_refreshTimer.Enabled = true;
			}

		}

		/// <summary>SP-866: Program crashes or freezes after progress tab is displayed.</summary>
		private bool IsReallyVisible()
		{
			if (Program.ProjectWindow == null) return false;
			if (Program.ProjectWindow.SelectedTabIndex() != 0) return false;

			// is the progress tab selected?
			var pos = Program.ProjectWindow.PointToClient(_toolStripActions.PointToScreen(Point.Empty));
			if ((pos.X < 0) || (pos.Y < 0)) return false;

			return true;
		}

		/// ------------------------------------------------------------------------------------
		void HandleFinishedGatheringStatisticsForAllFiles(object sender, EventArgs e)
		{
			// Can't actually call UpdateDisplay from here because this event is fired from
			// a background (data gathering) thread and updating the browser control on the
			// background thread is a no-no. UpdateDisplay will be called when the timer
			// tick fires.
			_updateDisplayNeeded = true;
		}

		/// ----------------------------------------------------------------------------------
		void HandleNewDataAvailable(object sender, EventArgs e)
		{
			// Can't actually call UpdateDisplay from here because this event is fired from
			// a background (data gathering) thread and updating the browser control on the
			// background thread is a no-no. UpdateDisplay will be called when the timer
			// tick fires.
			_updateDisplayNeeded = true;
		}
	}
}
