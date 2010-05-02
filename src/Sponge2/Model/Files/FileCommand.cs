using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using SilUtils;

namespace Sponge2.Model.Files
{
	/// <summary>
	/// A FileCommand is something the user can do with a file, normally by using a
	/// context menu. E.g. rename, view in explorer, edit, etc.
	/// </summary>
	public class FileCommand
	{
		public FileCommand(string englishLabel, Action<string> action)
		{
			EnglishLabel = englishLabel;
			Action = action;
		}

		public Action<string> Action { get; private set; }

		public string EnglishLabel { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open the file's folder in the OS' file manager.
		/// ENHANCE: After opening
		/// the file manager, it would be nice to select the file itself, but that
		/// appears to be harder to accomplish, so I leave that for a future exercise.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void HandleOpenInFileManager_Click(string path)
		{
			Process.Start(Path.GetDirectoryName(path));
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
			}
			catch (Exception error)
			{
				Palaso.Reporting.ErrorReport.ReportNonFatalException(error);
			}
		}
	}
}