using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Palaso.Reporting;
using SayMore.Utilities;

namespace SayMore.Media
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
		public static ExternalProcess StartProcessToMonitor(string exePath,
			IEnumerable<string> args, DataReceivedEventHandler outputDataHandler,
			DataReceivedEventHandler errorDataHandler, string processFailedToStartErrorMsg)
		{
			return StartProcessToMonitor(exePath, BuildCommandLine(args),
				outputDataHandler, errorDataHandler, processFailedToStartErrorMsg);
		}

		/// ------------------------------------------------------------------------------------
		public static ExternalProcess StartProcessToMonitor(string exePath, string args,
			DataReceivedEventHandler outputDataHandler, DataReceivedEventHandler errorDataHandler,
			string processFailedToStartErrorMsg)
		{
			if (outputDataHandler == null)
				throw new ArgumentNullException("outputDataHandler");

			if (errorDataHandler == null)
				throw new ArgumentNullException("errorDataHandler");

			var prs = new ExternalProcess(exePath);
			prs.StartInfo.RedirectStandardInput = true;
			prs.StartInfo.RedirectStandardError = true;
			prs.OutputDataReceived += outputDataHandler;
			prs.ErrorDataReceived += errorDataHandler;
			prs.StartInfo.Arguments = args;

			if (!prs.StartProcess())
			{
				prs = null;
				if (processFailedToStartErrorMsg != null)
					Palaso.Reporting.ErrorReport.NotifyUserOfProblem(processFailedToStartErrorMsg);
			}

			prs.StandardInput.AutoFlush = true;
			prs.BeginOutputReadLine();
			prs.BeginErrorReadLine();

			return prs;
		}

		/// ------------------------------------------------------------------------------------
		private static string BuildCommandLine(IEnumerable<string> args)
		{
			var bldr = new StringBuilder();
			foreach (var arg in args)
				bldr.AppendFormat("{0} ", arg);

			return bldr.ToString();
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
				catch (Exception e)
				{
					Logger.WriteEvent("Handled exception in ExternalProcess.StartProcess:\r\n{0}", e.ToString());
				}

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
			lock (s_processIds)
			{
				HashSet<int> processIds;
				if (!s_processIds.TryGetValue(processExePath, out processIds))
					return;

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
