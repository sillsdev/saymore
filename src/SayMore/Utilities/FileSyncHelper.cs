using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;


namespace SayMore.Utilities
{
	public static class FileSyncHelper
	{
		private static bool _stoppedDropbox;
		private static bool _stoppedGoogleDrive;
		private static bool _stoppedOneDrive;

		private const string kDropboxProcess = "dropbox";
		private const string kGoogleDriveProcess = "googledrivesync";
		private const string kOneDriveProcess = "onedrive";

		internal static event Func<string, SyncClient> TestOverrideFileChecked;

		public enum SyncClient
		{
			None,
			Dropbox,
			GoogleDrive,
			OneDrive
		}

		/// <summary>
		/// Checks if the file is being synced, and prompts user to disable syncing if it is.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns>If a client is stopped, the client is returned, otherwise SyncClient.None</returns>
		public static SyncClient PromptToStopSync(string filePath)
		{
			if (TestOverrideFileChecked != null)
				return TestOverrideFileChecked(filePath);

			var client = IsSynched(filePath);

			var confirmationMsg = LocalizationManager.GetString("MainWindow.ConfirmStopFileSync",
				"It looks like this project is in a directory that is being synchronized with " +
				"{0}. This is known to cause problems due to files being locked. Would you like " +
				"to disable {0} temporarily until you close SayMore?",
				"Param: Known folder synchronization program (Dropbox, Google Drive, etc.)");

			string clientName;

			switch (client)
			{
				case SyncClient.Dropbox:
					clientName = "Dropbox";
					break;

				case SyncClient.GoogleDrive:
					clientName = "Google Drive";
					break;

				case SyncClient.OneDrive:
					clientName = "OneDrive";
					break;

				case SyncClient.None:
					return client;

				default:
					return SyncClient.None;
			}

			var msg = string.Format(confirmationMsg, clientName, Program.ProductName);
			if (DialogResult.No == MessageBox.Show(msg, Program.ProductName,
				    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
			{
				return SyncClient.None;
			}

			StopClient(client);
			return client;
		}

		/// <summary>
		/// Checks if the file is in a directory that is actively being synced (the sync program is running)
		/// </summary>
		/// <param name="filePath">Full path to the file being checked</param>
		/// <returns></returns>
		public static SyncClient IsSynched(string filePath)
		{
			var pathParts = filePath.ToLower().Split(new[] {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar},
				StringSplitOptions.RemoveEmptyEntries);

			// Dropbox
			if (IsClientActiveAndSynchingThisPath(pathParts, SyncClient.Dropbox))
				return SyncClient.Dropbox;

			// Google Drive
			if (IsClientActiveAndSynchingThisPath(pathParts, SyncClient.GoogleDrive))
				return SyncClient.GoogleDrive;

			// OneDrive
			if (IsClientActiveAndSynchingThisPath(pathParts, SyncClient.OneDrive))
				return SyncClient.OneDrive;

			return SyncClient.None;
		}

		/// <summary>
		/// Stops the selected client and remembers it for when SayMore exits
		/// </summary>
		/// <param name="client"></param>
		public static void StopClient(SyncClient client)
		{
			switch (client)
			{
				case SyncClient.Dropbox:
					if (!_stoppedDropbox)
					{
						KillProcess(kDropboxProcess);
						_stoppedDropbox = true;
					}
					break;

				case SyncClient.GoogleDrive:
					if (!_stoppedGoogleDrive)
					{
						KillProcess(kGoogleDriveProcess);
						_stoppedGoogleDrive = true;
					}
					break;

				case SyncClient.OneDrive:
					if (!_stoppedOneDrive)
					{
						StartStopOneDrive(true);
					}
					break;
			}
		}

		/// <summary>
		/// Restart the sync clients stopped by SayMore
		/// </summary>
		public static void RestartAllStoppedClients()
		{
			if (_stoppedDropbox)
			{
				var exeFile = GetProgramFilesPath(Path.Combine("Dropbox", "Client", "Dropbox.exe"));

				if (!string.IsNullOrEmpty(exeFile))
				{
					LaunchWithCmd(exeFile);
					_stoppedDropbox = false;
				}
			}

			if (_stoppedGoogleDrive)
			{
				var exeFile = GetProgramFilesPath(Path.Combine("Google", "Drive", "googledrivesync.exe"));

				if (!string.IsNullOrEmpty(exeFile))
				{
					LaunchWithCmd(exeFile);
					_stoppedGoogleDrive = false;
				}
			}

			if (_stoppedOneDrive)
			{
				StartStopOneDrive(false);
			}
		}

		/// <summary>
		/// Checks if the selected sync client is currently running
		/// </summary>
		/// <param name="client"></param>
		/// <returns>true id the client is running, false if not</returns>
		public static bool ClientIsRunning(SyncClient client)
		{
			string processNameLower;

			switch (client)
			{
				case SyncClient.None:
					return false;

				case SyncClient.Dropbox:
					processNameLower = kDropboxProcess;
					break;

				case SyncClient.GoogleDrive:
					processNameLower = kGoogleDriveProcess;
					break;

				case SyncClient.OneDrive:
					processNameLower = kOneDriveProcess;
					break;

				default:
					return false;
			}

			var processes = Process.GetProcesses();
			return processes.Any(p => p.ProcessName.ToLower() == processNameLower);
		}

		/// <summary>
		/// First checks if the file is in a potentially synced directory,
		/// then if it is, checks if the sync program is active.
		/// </summary>
		/// <param name="pathParts"></param>
		/// <param name="client"></param>
		/// <returns></returns>
		private static bool IsClientActiveAndSynchingThisPath(string[] pathParts, SyncClient client)
		{
			string dirNameLower;

			switch (client)
			{
				case SyncClient.None:
					return false;

				case SyncClient.Dropbox:
					dirNameLower = "dropbox";
					break;

				case SyncClient.GoogleDrive:
					dirNameLower = "google drive";
					break;

				case SyncClient.OneDrive:
					dirNameLower = "onedrive";
					break;

				default:
					return false;
			}

			if (Array.IndexOf(pathParts, dirNameLower) == -1) return false;

			return ClientIsRunning(client);
		}

		/// <summary>
		/// Kills all processes with the given name
		/// </summary>
		/// <param name="processNameLower"></param>
		private static void KillProcess(string processNameLower)
		{
			var processes = Process.GetProcesses().Where(p => p.ProcessName.ToLower() == processNameLower).ToArray();
			var ids = processes.Select(p => p.Id).ToArray();

			if (ids.Length == 0) return;

			var pidStr = string.Join(" /PID ", ids);

			// use the shell command 'taskkill' so it works on both 32 and 64 bit processes
			var info = new ProcessStartInfo("taskkill", "/PID " + pidStr + " /F /T")
			{
				CreateNoWindow = true, 
				WindowStyle = ProcessWindowStyle.Hidden
			};

			var proc = new Process {StartInfo = info};
			proc.Start();
			proc.WaitForExit();
		}

		/// <summary>
		/// Start the sync client
		/// </summary>
		/// <param name="exeName">Full path to the client executable file</param>
		/// <param name="switches">Command line switches for the exe file</param>
		private static void LaunchWithCmd(string exeName, string switches="")
		{
			// use 'cmd.exe' to start the process so it works with both 32 and 64 bit processes
			var arguments = "/C start \"\" \"" + exeName + "\"";

			if (!string.IsNullOrEmpty(switches))
				arguments += " " + switches;

			var info = new ProcessStartInfo("cmd.exe", arguments)
			{
				CreateNoWindow = true, 
				WindowStyle = ProcessWindowStyle.Hidden
			};

			var proc = new Process {StartInfo = info};
			proc.Start();
			proc.WaitForExit();
		}

		/// <summary>
		/// Looks for a file in 'Program Files' and 'Program Files (x86)'
		/// </summary>
		/// <param name="relativePath">The portion of the path after 'C:\Program Files\'</param>
		/// <returns>The full path to the file, or empty string on failure</returns>
		private static string GetProgramFilesPath(string relativePath)
		{
			var exeFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), relativePath);
			if (File.Exists(exeFile))
				return exeFile;

			// A 32-bit program running on 64-bit Windows always return 'Program Files (x86)' rather than just 'Program Files'.
			// However, Google Drive is installed in the 64 bit 'Program Files' directory.
			if (Environment.Is64BitOperatingSystem && exeFile.Contains(" (x86)"))
			{
				exeFile = exeFile.Replace(" (x86)", "");
				if (File.Exists(exeFile))
					return exeFile;
			}

			exeFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), relativePath);

			if (File.Exists(exeFile))
				return exeFile;

			return string.Empty;
		}

		private static void StartStopOneDrive(bool stop)
		{
			var exeFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "OneDrive", "OneDrive.exe");

			if (!File.Exists(exeFile)) return;

			var option = (stop ? "/shutdown" : "/background");

			LaunchWithCmd(exeFile, option);

			_stoppedOneDrive = stop;
		}
	}
}
