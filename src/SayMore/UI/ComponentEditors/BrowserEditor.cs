using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using L10NSharp;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Utilities;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class BrowserEditor : EditorBase
	{
		private HtmlElement _fileLink;

		/// ------------------------------------------------------------------------------------
		public BrowserEditor(ComponentFile file, string imageKey) : base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "Browser";
			SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		private static bool ShouldAllowShowingInBrowserControl(string filePath)
		{
			if (filePath == null) return false;

			var ext = Path.GetExtension(filePath).Trim(new[] {'.'});
			if (string.IsNullOrEmpty(ext)) return false;

			var allowed = Settings.Default.FileTypesToAllowLoadingInBrowserEditor.Split(new[] {';'});
			return allowed.Contains(ext);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			if (_fileLink != null)
			{
				_fileLink.Click += HandleFileLinkClick;
				_fileLink = null;
			}

			if (_browser == null || IsDisposed)
				return;

			DisplayFile(file.PathToAnnotatedFile);

			file.PreRenameAction = file.PreFileCommandAction = file.PreDeleteAction = (() =>
			{
				// This will unlock the file so it can be renamed.
				_browser.Navigate("about:blank");

				// The browser takes a moment to finish navigating and thus
				// to release the file pointed to by the previous URL.
				Cursor = Cursors.WaitCursor;
				FileSystemUtils.WaitForFileRelease(file.PathToAnnotatedFile);
				Cursor = Cursors.Default;
			});

			file.PostRenameAction = () => DisplayFile(file.PathToAnnotatedFile);
		}

		#region Methods for trying to display file in browser control
		/// ------------------------------------------------------------------------------------
		private void DisplayFile(string filePath)
		{
			if (ShouldAllowShowingInBrowserControl(filePath))
				TryToDisplayFileInBrowser(filePath);
			else
				DisplayInfoForFileNotShownInBrowser(filePath);
		}

		/// ------------------------------------------------------------------------------------
		private void TryToDisplayFileInBrowser(string filePath)
		{
			_browser.Tag = filePath;
			_browser.DocumentCompleted += HandleBrowserLoadCompleted;
			_browser.Navigate("about:blank");

			var msg = LocalizationManager.GetString("CommonToMultipleViews.GenericFileTypeViewer.FileNameMsg",
				"<HTML>SayMore attempted to load:<br /><br /><b>File:</b> {0}<br /><nobr><b>Folder:</b> {1}</nobr></HTML>");

			msg = msg.Replace("\n", "<br />");

			_browser.DocumentText = string.Format(msg,
				Path.GetFileName(filePath), Path.GetDirectoryName(filePath));
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBrowserLoadCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			_browser.DocumentCompleted -= HandleBrowserLoadCompleted;

			if (!string.IsNullOrEmpty(_browser.Tag as string))
				_browser.Navigate((string)_browser.Tag);
		}

		#endregion

		#region Methods for dealing with files we don't try showing in the browser control
		/// ------------------------------------------------------------------------------------
		private void DisplayInfoForFileNotShownInBrowser(string filePath)
		{
			var msg = LocalizationManager.GetString("CommonToMultipleViews.GenericFileTypeViewer.FileLinkMsg",
				"Open {0} in its associated program.");
			msg = msg.Replace("\n", "<br />");

			var html = string.Format("<HTML><BODY>{0}</BODY></HTML>", msg);
			html = string.Format(html, "<a href=\"file:///{0}\"><b>{1}</b></a>");

			_browser.Tag = filePath;
			_browser.DocumentCompleted += HandleDocumentCompleted;
			_browser.DocumentText = string.Format(html, filePath, Path.GetFileName(filePath));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// It turns out just sticking a link in HTML to a file in the local file system and
		/// setting the browser control's DocumentText to contain that link doesn't always
		/// work (as in clicking on the link does nothing). Apparently it works for some people
		/// though, as I discovered from various newsgroups, but not for others, including
		/// myself. I'm not sure if it's a browser security thing or what, but in order to
		/// avoid this variability, I just subscribe to the link's click event and fire off
		/// a process using the file in the link.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			_browser.DocumentCompleted -= HandleDocumentCompleted;
			var links = _browser.Document.Links;
			if (links.Count > 0)
			{
				_fileLink = links[0];
				_fileLink.Click += HandleFileLinkClick;
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleFileLinkClick(object sender, HtmlElementEventArgs e)
		{
			Process.Start(_browser.Tag as string);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString("CommonToMultipleViews.GenericFileTypeViewer.TabText", "View");
			if (_browser != null)
			{
				var filePath = _browser.Tag as string;
				if (filePath != null)
					DisplayFile(filePath);
			}
			base.HandleStringsLocalized();
		}
	}
}
