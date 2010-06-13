using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using ThreadState = System.Threading.ThreadState;

namespace SayMore.Model.Files.DataGathering
{
	/// <summary>
	/// This is the base class for processes which live in the background,
	/// gathering data about the files in the collection so that this data
	/// is quickly accesible when needed.
	/// </summary>
	public abstract class BackgroundFileProcessor<T>: IDisposable where T: class
	{
		private Thread _workerThread;
		private string _rootDirectoryPath;
		private readonly IEnumerable<FileType> _fileTypes;
		private readonly Func<string, T> _fileDataFactory;
		private bool _restartRequested = true;
		protected Dictionary<string, T> _fileToDataDictionary = new Dictionary<string, T>();

		public BackgroundFileProcessor(string rootDirectoryPath, IEnumerable<FileType> fileTypes, Func<string, T> fileDataFactory)
		{
			_rootDirectoryPath = rootDirectoryPath;
			_fileTypes = fileTypes;
			_fileDataFactory = fileDataFactory;
			Status = "Not yet started";
		}

		protected virtual bool GetDoIncludeFile(string path)
		{
			return _fileTypes.Any(t => t.IsMatch(path));
		}

		public void Start()
		{
			_workerThread = new Thread(StartWorking);
			_workerThread.Priority = ThreadPriority.Lowest;
			_workerThread.Start();
		}

		private void StartWorking()
		{
			Status = "Working";

			//files added during the run are  handled with this stuff

			var watcher = new FileSystemWatcher(_rootDirectoryPath);
			watcher.Created += new FileSystemEventHandler(OnFileCreated);
			watcher.Renamed += new RenamedEventHandler(OnFileRenamed);

			//now just wait for file events that we should handle (on a different thread, in response to events)

			while (!ShouldStop)
			{
				if (_restartRequested)
				{
					_restartRequested = false;
					ProcessFiles();
				}
				Thread.Sleep(100);
			}
		}

		public T GetFileData(string filePath)
		{
			T stats;
			lock (((ICollection)_fileToDataDictionary).SyncRoot)
			{
				if (_fileToDataDictionary.TryGetValue(filePath.ToLower(), out stats))
					return stats;
				return null;
			}
		}

		private void CollectDataForFile(string path)
		{
			try
			{
				if (!GetDoIncludeFile(path))
				{
					return;
				}
				Debug.WriteLine("processing " + path);
				if (!ShouldStop)
				{
					var fileData = _fileDataFactory(path);

					lock (((ICollection)_fileToDataDictionary).SyncRoot)
					{
						if (_fileToDataDictionary.ContainsKey(path.ToLower()))
						{
							_fileToDataDictionary.Remove(path.ToLower());
						}
						_fileToDataDictionary.Add(path.ToLower(), fileData);
					}
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
#if  DEBUG
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(e, "Error gathering data");
#endif
				//nothing here is worth crashing over
			}
			InvokeNewDataAvailable();
		}

		public IEnumerable<T> GetAllStatistics()
		{
			lock (((ICollection)_fileToDataDictionary).SyncRoot)
			{
				//Give a copy (a list) because if we just give an enumerator, we'll get
				//an error if/when this collection is altered on another thread
				return _fileToDataDictionary.Values.ToList();
			}
		}

		public void ProcessFiles()
		{
			//now that the watcher is up and running, gather up all existing files
			lock (((ICollection)_fileToDataDictionary).SyncRoot)
			{
				_fileToDataDictionary.Clear();
			}
			var paths = new List<string>();
			paths.AddRange(Directory.GetFiles(_rootDirectoryPath, "*.*", SearchOption.AllDirectories));

			for (int i = 0; i < paths.Count; i++)
			{
				if (ShouldStop)
					break;
				Status = string.Format("Processing {0} of {1} files", 1 + i, paths.Count);
				if (ShouldStop)
					break;
				CollectDataForFile(paths[i]);
			}
			Status = "Up to date";
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
			//enhance: provide an enumeration for the use of tests (at least)
			get;
			private set;
		}

		public void Dispose()
		{
			if (_workerThread != null)
			{
				_workerThread.Abort();//will eventually lead to it stopping
			}
			_workerThread = null;
		}


		private void OnFileRenamed(object sender, RenamedEventArgs e)
		{
			try
			{
				lock (((ICollection)_fileToDataDictionary).SyncRoot)
				{
					if (_fileToDataDictionary.ContainsKey(e.OldFullPath.ToLower()))
					{
						_fileToDataDictionary.Add(e.FullPath.ToLower(), _fileToDataDictionary[e.OldFullPath.ToLower()]);
						_fileToDataDictionary.Remove(e.OldFullPath.ToLower());
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
			CollectDataForFile(e.FullPath);
			Status = "Up to date";

		}

		public event EventHandler NewDataAvailable;

		protected void InvokeNewDataAvailable()
		{
			EventHandler statistics = NewDataAvailable;
			if (statistics != null) statistics(this, null);
		}
	}

}