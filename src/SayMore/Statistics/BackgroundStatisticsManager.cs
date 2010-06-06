using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using ThreadState = System.Threading.ThreadState;

namespace SayMore.Statistics
{
	public interface IProvideFileStatistics
	{
		FileStatistics GetStatistics(string filePath);
	}

	public class BackgroundStatisticsManager :IDisposable, IProvideFileStatistics
	{
		private Thread _workerThread;
		private string _rootPath;
		private Dictionary<string, FileStatistics> _statistics=new Dictionary<string, FileStatistics>();
		private bool _restartRequested = true;

		public event EventHandler NewStatistics;

		private void InvokeNewStatistics()
		{
			EventHandler statistics = NewStatistics;
			if (statistics != null) statistics(this, null);
		}

		public BackgroundStatisticsManager(string rootPath)
		{
			_rootPath = rootPath;
		}

		public void Start()
		{
			_workerThread = new Thread(StartWorking);
			_workerThread.Priority = ThreadPriority.Lowest;
			_workerThread.Start();
		}

		public FileStatistics GetStatistics(string filePath)
		{
			FileStatistics stats;
			lock (((ICollection)_statistics).SyncRoot)
			{
				if (_statistics.TryGetValue(filePath.ToLower(), out stats))
					return stats;
				return null;
			}
		}

		private void StartWorking()
		{
			Status = "Working";

			//files added during the run are  handled with this stuff

			var watcher = new FileSystemWatcher(_rootPath);
			watcher.Created+=new FileSystemEventHandler(OnFileCreated);
			watcher.Renamed+=new RenamedEventHandler(OnFileRenamed);

			//now just wait for file events that we should handle (on a different thread, in response to events)

			while (!ShouldStop)
			{
				if(_restartRequested )
				{
					_restartRequested = false;
					ProcessFiles();
				}
				Thread.Sleep(100);
			}
		}

		public void Restart()
		{
			_restartRequested = true;
		}

		protected bool ShouldStop
		{
			get
			{
				return (Thread.CurrentThread.ThreadState &
						(ThreadState.StopRequested | ThreadState.AbortRequested | ThreadState.Stopped | ThreadState.Aborted)) > 0;
			}
		}

		public string Status
		{
			get; private set;
		}

		private void OnFileRenamed(object sender, RenamedEventArgs e)
		{
			try
			{
				lock (((ICollection)_statistics).SyncRoot)
				{
					if (_statistics.ContainsKey(e.OldFullPath.ToLower()))
					{
						_statistics.Add(e.FullPath.ToLower(), _statistics[e.OldFullPath.ToLower()]);
						_statistics.Remove(e.OldFullPath.ToLower());
					}
				}
			}
			catch (Exception)
			{
				//nothing here is worth crashing over
			}
		}

		private void OnFileCreated(object sender, FileSystemEventArgs e)
		{
			Status = "Working";
			AddStatisticsForFile(e.FullPath);
			Status = "Up to date";

		}



		private void AddStatisticsForFile(string path)
		{
			try
			{
				Debug.WriteLine("processing " + path);
				if (!ShouldStop)
				{
					var statistics = new FileStatistics(path);

					lock (((ICollection) _statistics).SyncRoot)
					{
						if (_statistics.ContainsKey(path.ToLower()))
						{
							_statistics.Remove(path.ToLower());
						}
						_statistics.Add(path.ToLower(), statistics);
					}
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				//nothing here is worth crashing over
			}
			InvokeNewStatistics();
		}


		public void Dispose()
		{
			if(_workerThread!=null)
			{
				_workerThread.Abort();//will eventually lead to it stopping
			}
			_workerThread = null;
		}

		public IEnumerable<FileStatistics> GetAllStatistics()
		{
			lock (((ICollection)_statistics).SyncRoot)
			{
				//Give a copy (a list) because if we just give an enumerator, we'll get
				//an error if/when this collection is altered on another thread
				return _statistics.Values.ToList();
			}
		}

		public void ProcessFiles()
		{
			//now that the watcher is up and running, gather up all existing files
			lock (((ICollection) _statistics).SyncRoot)
			{
				_statistics.Clear();
			}
			string[] sessionAndPeopleDirectoryPaths = Directory.GetDirectories(_rootPath);
			var elementDirectoryPaths = new List<string>();
			foreach (var directory in sessionAndPeopleDirectoryPaths)
			{
				elementDirectoryPaths.AddRange(Directory.GetDirectories(directory));
			}

			for (int i = 0; i < elementDirectoryPaths.ToArray().Length; i++)
			{
				if (ShouldStop)
					break;
				Status = string.Format("Processing {0} of {1} sessions", 1 + i, elementDirectoryPaths.ToArray().Length);
				foreach (var path in Directory.GetFiles(elementDirectoryPaths[i]))
				{
					if (ShouldStop)
						break;
					AddStatisticsForFile(path);
				}
			}
			Status = "Up to date";
		}
	}
}