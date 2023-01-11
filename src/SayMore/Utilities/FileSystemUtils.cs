using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DesktopAnalytics;
using SIL.IO;
using SIL.Reporting;
using L10NSharp;

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
                string volume;
				try
                {
                    volume = Path.GetPathRoot(path);
					if (volume.Last() == Path.DirectorySeparatorChar)
                        volume = volume.Remove(volume.Length - 1);
                }
				catch (Exception)
                {
                    volume = "";
                }

                if (volume.Length == 0)
                    volume = "???";

                ErrorReport.NotifyUserOfProblem(new ShowOncePerSessionBasedOnExactMessagePolicy(), 
                    LocalizationManager.GetString("CommonToMultipleViews.UnableToObtainShortName",
                    "{0} was unable to obtain a \"short name\" for this file:\r\n{1}\r\nYou (or " +
                    "a system administrator) can use {2} to enable creation of short \"8.3\" " +
                    "file names for the file system volume ({3}) where this file is located.",
                    "Param 0: \"SayMore\" (product name); " +
                    "Param 1: file path; " +
                    "Param 2: \"fsutil 8dot3name\" (a Microsoft Windows utility); " +
                    "Param 3: A system volume (e.g. \"D:\"") +
                    Environment.NewLine + getDescriptionOfFailedAction(),
                    Program.ProductName, path, "fsutil 8dot3name", volume);
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

				// now that file synching is disabled, try again
				RobustFile.Delete(filePath);
			}
		}
	}
}
