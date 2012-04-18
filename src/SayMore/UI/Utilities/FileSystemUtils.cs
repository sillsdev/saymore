using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SayMore.UI.Utilities
{
	public class FileSystemUtils
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Waits for the lock on a file to be released. The method will give up after waiting
		/// for 10 seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WaitForFileRelease(string filePath)
		{
			WaitForFileRelease(filePath, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Waits for the lock on a file to be released. The method will give up after waiting
		/// for 10 seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WaitForFileRelease(string filePath, bool fileOpenedByThisProcess)
		{
			WaitForFileRelease(filePath, fileOpenedByThisProcess, 10000);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Waits up to the specified time for a lock on a file to be released.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WaitForFileRelease(string filePath, int millisecondsToWait)
		{
			WaitForFileRelease(filePath, false, millisecondsToWait);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Waits up to the specified time for a lock on a file to be released.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WaitForFileRelease(string filePath, bool fileOpenedByThisProcess,
			int millisecondsToWait)
		{
			var timeout = DateTime.Now.AddMilliseconds(millisecondsToWait);

			// Now wait until the process lets go of the file.
			while (true)
			{
				try
				{
					if (fileOpenedByThisProcess)
						Application.DoEvents();
					else
						Thread.Sleep(100);

					if (!IsFileLocked(filePath) || DateTime.Now >= timeout)
						return;
				}
				catch
				{
					if (DateTime.Now >= timeout)
						return;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public static bool IsFileLocked(string filePath)
		{
			if (filePath == null || !File.Exists(filePath))
				return false;

			try
			{
				File.OpenWrite(filePath).Close();
				return false;
			}
			catch
			{
				return true;
			}
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
	}
}
