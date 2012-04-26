
using System.Collections.Generic;
using System.Diagnostics;
using SayMore.UI.Utilities;

namespace SayMore.Media.UI
{
	/// ----------------------------------------------------------------------------------------
	public class ExternalProcess : Process
	{
		public string FileOpenedByProcess { get; set; }

		private static readonly Dictionary<string, HashSet<int>> s_processIds =
			new Dictionary<string, HashSet<int>>();

		/// ------------------------------------------------------------------------------------
		public ExternalProcess(string processesExePath)
		{
			StartInfo.CreateNoWindow = true;
			StartInfo.UseShellExecute = false;
			StartInfo.RedirectStandardOutput = true;
			StartInfo.FileName = processesExePath;
		}

		/// ------------------------------------------------------------------------------------
		public bool StartProcess()
		{
			CleanUpProcesses(StartInfo.FileName);

			HashSet<int> processIds;
			if (!s_processIds.TryGetValue(StartInfo.FileName, out processIds))
				s_processIds[StartInfo.FileName] = new HashSet<int>();

			if (Start())
			{
				// Sometimes the program will crash when trying to set the PriorityClass,
				// claiming the process has already exited. Hence the try/catch. Hmm...
				try
				{
					PriorityClass = ProcessPriorityClass.High;
					s_processIds[StartInfo.FileName].Add(Id);
				}
				catch { }

				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public static void CleanUpAllProcesses()
		{
			foreach (var processExePath in s_processIds.Keys)
				CleanUpProcesses(processExePath);
		}

		/// ------------------------------------------------------------------------------------
		public static void CleanUpProcesses(string processExePath)
		{
			HashSet<int> processIds;
			if (!s_processIds.TryGetValue(processExePath, out processIds))
				return;

			lock (s_processIds)
			{
				foreach (int id in processIds)
				{
					try
					{
						var prs = GetProcessById(id);
						prs.Kill();
						prs.Close();
					}
					catch { }
				}

				processIds.Clear();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void KillAndWaitForFileRelease()
		{
			if (!HasExited)
				Kill();

			if (FileOpenedByProcess != null)
				FileSystemUtils.WaitForFileRelease(FileOpenedByProcess);
		}

		/// ------------------------------------------------------------------------------------
		public void KillProcess()
		{
			if (!HasExited)
				Kill();
		}
	}
}
