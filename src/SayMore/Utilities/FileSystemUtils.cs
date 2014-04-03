using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DesktopAnalytics;
using Palaso.IO;
using Palaso.Reporting;
using L10NSharp;

namespace SayMore.Utilities
{
	public class FileSystemUtils
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern uint GetShortPathName(
		   [MarshalAs(UnmanagedType.LPTStr)]string lpszLongPath,
		   [MarshalAs(UnmanagedType.LPTStr)]StringBuilder lpszShortPath,
		   uint cchBuffer);

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
				if (!FileUtils.IsFileLocked(filePath))
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
		/// it's limits and if the OS is very slow to create the folder, it will give up and
		/// return false. If the directory already exists, then true is returned right away.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool CreateDirectory(string folder)
		{
			Exception error;
			return CreateDirectory(folder, out error);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// There are times when the OS doesn't finish creating a directory when the program
		/// needs to begin writing files to the directory. This method will ensure the OS
		/// has finished creating the directory before returning. However, this method has
		/// it's limits and if the OS is very slow to create the folder, it will give up and
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// There are times when the OS doesn't finish removing a directory when the program
		/// needs to, for example, recreate the directory. This method will ensure the OS
		/// has finished removing the directory before returning. However, this method has
		/// it's limits and if the OS is very slow to remove the folder, it will give up and
		/// return false. If the directory already does not exist, then true is returned
		/// right away.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool RemoveDirectory(string folder)
		{
			Exception error;
			return RemoveDirectory(folder, out error);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// There are times when the OS doesn't finish removing a directory when the program
		/// needs to, for example, recreate the directory. This method will ensure the OS
		/// has finished removing the directory before returning. However, this method has
		/// it's limits and if the OS is very slow to remove the folder, it will give up and
		/// return false. If the directory already does not exist, then true is returned
		/// right away.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool RemoveDirectory(string folder, out Exception error)
		{
			error = null;
			var testFile = Path.Combine(folder, "junk");

			if (!Directory.Exists(folder))
				return true;

			int retryCount = 0;

			while (retryCount < 20)
			{
				try
				{
					Directory.Delete(folder, true);
					try { File.Create(testFile).Close(); }
					catch { return true; }
					Thread.Sleep(200);
					retryCount++;
				}
				catch (Exception e)
				{
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

		public static string GetShortName(string path)
		{
			var shortBuilder = new StringBuilder(300);
			GetShortPathName(path, shortBuilder, (uint)shortBuilder.Capacity);
			return shortBuilder.ToString();
		}
	}
}
