using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Localization;

namespace SayMore.Model.Files
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A FileCommand is something the user can do with a file, normally by using a
	/// context menu. E.g. rename, view in explorer, edit, etc.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FileCommand
	{
		/// ------------------------------------------------------------------------------------
		public FileCommand(string englishLabel, Action<string> action, string menuId)
		{
			EnglishLabel = englishLabel;
			Action = action;
			MenuId = menuId;
		}

		public Action<string> Action { get; private set; }
		public string MenuId { get; private set; }
		public string EnglishLabel { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open the file in the OS' file manager.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void HandleOpenInFileManager_Click(string path)
		{
			Process.Start("Explorer", "/select, \""+path+"\"");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open current session file in its associated application.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void HandleOpenInApp_Click(string path)
		{
			try
			{
				Process.Start(path);

				// I observed a case when an exception was thrown when the file specified by
				// path doesn't have an associated application. I can only assume the process
				// threw the exception after leaving this try/catch block. Therefore, we'll
				// sleep for a second in hopes that any exceptions the process may throw
				// get caught here.
				Thread.Sleep(1000);
			}
			catch (Exception error)
			{
				var msg = LocalizationManager.GetString(
					"CommonToMultipleViews.FileList.CannotOpenFileInApplicationErrorMsg",
					"There was a problem opening the file\r\n\r\n'{0}'\r\n\r\n" +
					"Make sure there is an application associated with this type of file.");

				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(error,  msg, path);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return EnglishLabel;
		}
	}
}