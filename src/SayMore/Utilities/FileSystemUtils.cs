using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using DesktopAnalytics;
using SIL.IO;
using SIL.Reporting;
using L10NSharp;
using Application = System.Windows.Forms.Application;
using DateTime = System.DateTime;
using FileInfo = System.IO.FileInfo;
using FileUtils = SIL.IO.FileUtils;
using static System.String;

namespace SayMore.Utilities
{
	public static class FileSystemUtils
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern uint GetShortPathName(
		   [MarshalAs(UnmanagedType.LPTStr)]string lpszLongPath,
		   [MarshalAs(UnmanagedType.LPTStr)]StringBuilder lpszShortPath,
		   uint cchBuffer);

        private static Regex s_regex8Dot3Filename;

		public delegate void FailedToGetShortNameHandler(string path, string failedActionDescription);
		public static event FailedToGetShortNameHandler FailedToGetShortName;

        static FileSystemUtils()
        {
            const string validChars = @"\x21-\x2D\x30-\x39\x3B-\x3E\x40-\x5B\x5D-\x7E";
            var eightDotThree = $"[{validChars}]" + "{1,8}" +
                $"(\\.[{validChars}]" + "{1,3})?";
            s_regex8Dot3Filename = new Regex(@"^([A-Za-z]:\\)?(" + eightDotThree + 
                @"\\)*" + eightDotThree + "$", RegexOptions.Compiled);
		}

        public enum WaitForReleaseResult
		{
			Free,
			ReadOnly,
			TimedOut,
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsAudioVideo(string path)
		{
			return (FileUtils.GetIsAudio(path) || FileUtils.GetIsVideo(path));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Waits for the lock on a file to be released. The method will give up after waiting
		/// for 10 seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WaitForReleaseResult WaitForFileRelease(string filePath)
		{
			return WaitForFileRelease(filePath, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Waits for the lock on a file to be released. The method will give up after waiting
		/// for 10 seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WaitForReleaseResult WaitForFileRelease(string filePath, bool reportErrorIfReadOnly)
		{
			return WaitForFileRelease(filePath, 10000, reportErrorIfReadOnly);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Waits up to the specified time for a lock on a file to be released.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WaitForReleaseResult WaitForFileRelease(string filePath,
			int millisecondsToWait, bool reportErrorIfReadOnly)
		{
			var timeout = DateTime.Now.AddMilliseconds(millisecondsToWait);

			// Now wait until the process lets go of the file.
			while (DateTime.Now < timeout)
			{
				if (!FileHelper.IsLocked(filePath))
					return WaitForReleaseResult.Free;

				try
				{
					FileInfo finfo = new FileInfo(filePath);
					if (finfo.IsReadOnly)
					{
						// No point in waiting. User isn't likely to change this in the next 10 seconds.
						if (reportErrorIfReadOnly)
						{
							var msg = LocalizationManager.GetString("CommonToMultipleViews.FileIsReadOnly",
								"SayMore is not able to write to the file \"{0}.\" It is read-only.");
							// ENHANCE: Should we offer to overwrite anyway?
							ErrorReport.ReportNonFatalMessageWithStackTrace(msg, filePath);
						}
						return WaitForReleaseResult.ReadOnly;
					}
				}
				catch
				{
					// Oh, well. We tried.
				}

				Application.DoEvents();
			}
			Analytics.Track("WaitForFileRelease Timeout", new Dictionary<string, string> {
				{"filePath", filePath} });
			return WaitForReleaseResult.TimedOut;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// There are times when the OS doesn't finish creating a directory when the program
		/// needs to begin writing files to the directory. This method will ensure the OS
		/// has finished creating the directory before returning. However, this method has
		/// its limits and if the OS is very slow to create the folder, it will give up and
		/// return false. If the directory already exists, then true is returned right away.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool CreateDirectory(string folder)
		{
			return CreateDirectory(folder, out _);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// There are times when the OS doesn't finish creating a directory when the program
		/// needs to begin writing files to the directory. This method will ensure the OS
		/// has finished creating the directory before returning. However, this method has
		/// its limits and if the OS is very slow to create the folder, it will give up and
		/// return false. If the directory already exists, then true is returned right away.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool CreateDirectory(string folder, out Exception error)
		{
			error = null;
			var testFile = Path.Combine(folder, "junk");

			if (Directory.Exists(folder))
				return true;

			int retryCount = 0;

			while (retryCount < 20)
			{
				try
				{
					Directory.CreateDirectory(folder);
					File.Create(testFile).Close();
					return true;
				}
				catch (Exception e)
				{
					Application.DoEvents();
					retryCount++;
					error = e;
				}
				finally
				{
					if (File.Exists(testFile))
						File.Delete(testFile);
				}
			}

			return false;
		}

        public static bool IsValidShortFileNamePath(string path) =>
            s_regex8Dot3Filename.IsMatch(path);

        public static string GetShortName(string path, Func<string> getDescriptionOfFailedAction = null)
		{
			var shortBuilder = new StringBuilder(300);
			var length = (int)GetShortPathName(path, shortBuilder, (uint)shortBuilder.Capacity);
			if (length > shortBuilder.Capacity)
			{
				shortBuilder = new StringBuilder(length);
				GetShortPathName(path, shortBuilder, (uint)shortBuilder.Capacity);
			}

			var result = shortBuilder.ToString();
			Logger.WriteEvent($"Short path obtained for {path} => {result}");
            if (result == path && getDescriptionOfFailedAction != null &&
                !IsValidShortFileNamePath(result))
            {
	            FailedToGetShortName?.Invoke(path, getDescriptionOfFailedAction());
            }
			return result;
		}

		public const string kTextFileExtension = ".txt";

		public static string LocalizedVersionOfTextFileDescriptor =>
			LocalizationManager.GetString("CommonToMultipleViews.TextFileDescriptor",
				"Text File ({0})");

		public const string kAllFilesFilter = "*.*";

		public static string LocalizedVersionOfAllFilesDescriptor =>
			LocalizationManager.GetString("CommonToMultipleViews.AllFilesDescriptor",
				"All Files ({0})");

		/// <summary>
		/// Gets the volume (i.e., drive letter), if any, from the given path. This does not
		/// include the directory separator character, but it does include the trailing volume
		/// separator character. If the path is null or empty or does not specify a volume, this
		/// returns the volume of the current working directory. In rare cases (permission or I/O
		/// exception), could return null.
		/// </summary>
		public static string GetVolume(string path)
		{
			try
			{
				var volume = Path.GetPathRoot(path).TrimEnd(Path.DirectorySeparatorChar);
				if (volume != Empty)
					return volume;
			}
			catch (Exception ex)
			{
				if (ex is IOException || ex is SecurityException)
					return null;
			}
			return GetVolume(Environment.CurrentDirectory);
		}

		public static void RobustDelete(string filePath)
		{
			if (!File.Exists(filePath)) return;

			try
			{
				RobustFile.Delete(filePath);
			}
			catch (IOException)
			{
				if (FileSyncHelper.PromptToStopSync(filePath) == FileSyncHelper.SyncClient.None)
					throw;

				// now that file syncing is disabled, try again
				RobustFile.Delete(filePath);
			}
		}
	}
}
