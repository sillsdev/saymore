using System.IO;
using System.Windows.Forms;
using Localization;
using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class BrowserEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public BrowserEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Browser";
			SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			if (_browser == null)
				return;

			_browser.Tag = file.PathToAnnotatedFile;
			_browser.DocumentCompleted += HandleBrowserLoadCompleted;
			_browser.Navigate("about:blank");

			var msg = LocalizationManager.LocalizeString("BrowserEditor.FileNameMsg",
				"<HTML>An attempt was made to load:<br /><br /><b>File:</b> {0}<br /><nobr><b>Folder:</b> {1}</nobr></HTML>");

			msg = msg.Replace("\n", "<br />");

			_browser.DocumentText = string.Format(msg,
				Path.GetFileName(file.PathToAnnotatedFile),
				Path.GetDirectoryName(file.PathToAnnotatedFile));

			file.PreRenameAction = (() =>
			{
				// This will unlock the file so it can be renamed.
				_browser.Navigate("about:blank");

				// I don't like doing this, but the browser takes a moment to finish
				// navigating and thus to release the file pointed to by the previous URL.
				Application.DoEvents();
			});
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBrowserLoadCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			_browser.DocumentCompleted -= HandleBrowserLoadCompleted;

			if (!string.IsNullOrEmpty(_browser.Tag as string))
				_browser.Navigate((string)_browser.Tag);
		}
	}
}
