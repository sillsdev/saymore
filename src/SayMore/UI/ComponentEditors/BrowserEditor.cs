using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Utilities;
using SIL.Reporting;

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

			// REVIEW (TomB): Having just now fixed this so that localizing this is easier and
			// less error-prone, I find that it is apparently impossible to get this HTML to be
			// displayed. Looked back 10 years in the history and since this code was added on
			// 6/21/2010, despite lots of editing, the basic logic represented here is unchanged.
			// It's entirely possible that changing/differening browser behavior is the reason
			// for it, so I'm going to play it safe and leave it here, but I'm guessing it can
			// go away (and almost certainly isn't worth localizing). I am wrapping it in a try-
			// catch block so that if there are future localization gaffs that would cause format
			// errors they will be silently ignored. Can't see bringing down SayMore for an error
			// that might occur in useless code.
			try
			{
				var msgPreamble = string.Format(LocalizationManager.GetString(
						"CommonToMultipleViews.GenericFileTypeViewer.FileNameMsgPreamble",
						"{0} attempted to load:", "Param is \"SayMore\" (program name)"),
					Program.ProductName);
				var fileLabel = LocalizationManager.GetString(
					"CommonToMultipleViews.GenericFileTypeViewer.FileLabel", "File:");
				var folderLabel = LocalizationManager.GetString(
					"CommonToMultipleViews.GenericFileTypeViewer.FolderLabel", "Folder:");

				var msg = "<HTML>" + msgPreamble + "<br /><br /><b>" +
					fileLabel + "</b> {0}<br /><nobr><b>" +
					folderLabel + "</b> {1}</nobr></HTML>";

				msg = msg.Replace("\n", "<br />");

				_browser.DocumentText = string.Format(msg,
					Path.GetFileName(filePath), Path.GetDirectoryName(filePath));
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
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
			var filePath = (string)_browser.Tag;
			Logger.WriteEvent("Browser link clicked. Opening file: " + filePath);
			try
			{
				Process.Start(filePath);
			}
			catch (Exception ex)
			{
				ErrorReport.NotifyUserOfProblem(ex,
					LocalizationManager.GetString("CommonToMultipleViews.GenericFileTypeViewer.FailedToOpen",
					"Failed to open file: {0}"), filePath);
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized(ILocalizationManager lm)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
			{
				TabText = LocalizationManager.GetString(
					"CommonToMultipleViews.GenericFileTypeViewer.TabText", "View");
				if (_browser?.Tag is string filePath)
					DisplayFile(filePath);
			}

			base.HandleStringsLocalized(lm);
		}
	}
}
