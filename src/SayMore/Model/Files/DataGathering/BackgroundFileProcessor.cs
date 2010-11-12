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
	/// ----------------------------------------------------------------------------------------
	public interface ISingleListDataGatherer
	{
		event EventHandler NewDataAvailable;
		IEnumerable<string> GetValues();
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Gives lists of data, indexed by a key into a dictionary
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IMultiListDataProvider
	{
		event EventHandler NewDataAvailable;
		Dictionary<string, IEnumerable<string>> GetValueLists(bool includeUnattestedFactoryChoices);
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This is the base class for processes which live in the background,
	/// gathering data about the files in the collection so that this data
	/// is quickly accesible when needed.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public abstract class BackgroundFileProcessor<T> : IDisposable where T : class
	{
		private Thread _workerThread;
		public string RootDirectoryPath { get; protected set; }
		protected readonly IEnumerable<FileType> _typesOfFilesToProcess;
		protected readonly Func<string, T> _fileDataFactory;
		protected bool _restartRequested = true;
		protected Dictionary<string, T> _fileToDataDictionary = new Dictionary<string, T>();
		private readonly Queue<FileSystemEventArgs> _pendingFileEvents;
		private volatile bool _suspendEventProcessing;

		public event EventHandler NewDataAvailable;

		/// ------------------------------------------------------------------------------------
		public BackgroundFileProcessor(string rootDirectoryPath,
			IEnumerable<FileType> typesOfFilesToProcess, Func<string, T> fileDataFactory)
		{
			RootDirectoryPath = rootDirectoryPath;
			_typesOfFilesToProcess = typesOfFilesToProcess;
			_fileDataFactory = fileDataFactory;
			Status = "Not yet started";
			_pendingFileEvents = new Queue<FileSystemEventArgs>();
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_workerThread != null)
				_workerThread.Abort(); //will eventually lead to it stopping

			_workerThread = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides a means for threads outside the background processing thread to suspend
		/// the background process while still allowing it to enqueue events to be processed
		/// when ResumeProcessing is called.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void SuspendProcessing()
		{
			_suspendEventProcessing = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides a means for threads outside the background processing thread to resume
		/// the background process after SuspendProcessing has been called.
		/// If processAllPendingEventsNow is true, then all pending events are forced to be
		/// processed without delay.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void ResumeProcessing(bool processAllPendingEventsNow)
		{
			if (processAllPendingEventsNow)
			{
				lock (_pendingFileEvents)
				{
					while (_pendingFileEvents.Count > 0)
						ProcessFileEvent(_pendingFileEvents.Dequeue());
				}
			}

			_suspendEventProcessing = false;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool GetDoIncludeFile(string path)
		{
			return (_typesOfFilesToProcess.Any(t => t.IsMatch(path)));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual ThreadPriority ThreadPriority
		{
			get { return ThreadPriority.Lowest; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Start()
		{
			_workerThread = new Thread(StartWorking);
			_workerThread.Name = GetType().Name;
			_workerThread.Priority = ThreadPriority;
			_workerThread.Start();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnNewDataAvailable(T fileData)
		{
			if (NewDataAvailable != null)
				NewDataAvailable(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private void StartWorking()
		{
			try
			{
				Status = "Working"; //NB: this helps simplify unit tests, if go to the busy state before returning

				using (var watcher = new FileSystemWatcher(RootDirectoryPath))
				{
					watcher.Created += QueueFileEvent;
					watcher.Renamed += QueueFileEvent;
					watcher.Changed += QueueFileEvent;
					watcher.IncludeSubdirectories = true;
					watcher.EnableRaisingEvents = true;

					//now just wait for file events that we should handle (on a different thread, in response to events)

					while (!ShouldStop)
					{
						if (_restartRequested)
						{
							_restartRequested = false;
							ProcessAllFiles();
						}
						lock (_pendingFileEvents)
						{
							if (_pendingFileEvents.Count > 0 && !_suspendEventProcessing)
							{
								ProcessFileEvent(_pendingFileEvents.Dequeue());
							}
						}

						if (!ShouldStop)
							Thread.Sleep(100);
					}
				}

			}
			catch (ThreadAbortException)
			{
				//this is fine, it happens when we quit
			}
			catch (Exception error)
			{
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(error, "Background file watching failed.");
			}
		}

		/// ------------------------------------------------------------------------------------
		private void QueueFileEvent(object sender, FileSystemEventArgs e)
		{
			lock (_pendingFileEvents)
			{
				Debug.WriteLine(GetType().Name + ": " + e.ChangeType + ": " + e.Name);

				// REVIEW: Except for possibly rename events, could we ignore adding events
				// to the queue if the file for the event already has a pending event?
				_pendingFileEvents.Enqueue(e);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ProcessFileEvent(FileSystemEventArgs fileEvent)
		{
			try
			{
				if (fileEvent is RenamedEventArgs)
				{
					var e = fileEvent as RenamedEventArgs;
					lock (((ICollection)_fileToDataDictionary).SyncRoot)
					{
						T fileData;
						if (_fileToDataDictionary.TryGetValue(e.OldFullPath, out fileData))
						{
							_fileToDataDictionary.Remove(e.OldFullPath);
							_fileToDataDictionary[e.FullPath] = fileData;
						}
					}
				}
				else
				{
					Debug.WriteLine(GetType().Name + " Collecting " + fileEvent.ChangeType + ": " + fileEvent.Name);
					CollectDataForFile(fileEvent.FullPath);
				}
			}
			catch (ThreadAbortException)
			{
				//this is fine, it happens when we quit
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
#if  DEBUG
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(e, "Error gathering data");
#endif
				//nothing here is worth crashing over
			}
		}

		/// ------------------------------------------------------------------------------------
		public T GetFileData(string filePath)
		{
			lock (((ICollection)_fileToDataDictionary).SyncRoot)
			{
				T stats;
				return (_fileToDataDictionary.TryGetValue(filePath, out stats) ? stats : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected void CollectDataForFile(string path)
		{
			T fileData = null;

			Status = "Working";

			try
			{
				if (!GetDoIncludeFile(path))
				{
					return;
				}

				Debug.WriteLine("processing " + path);
				var actualPath = GetActualPath(path);

				if (!ShouldStop)
				{
					fileData = _fileDataFactory(actualPath);

					lock (((ICollection)_fileToDataDictionary).SyncRoot)
					{
						_fileToDataDictionary[actualPath] = fileData;
					}
				}

			}
			catch (ThreadAbortException)
			{
				//this is fine, it happens when we quit
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
#if  DEBUG
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(e, "Error gathering data");
#endif
				//nothing here is worth crashing over
			}

			OnNewDataAvailable(fileData);
			Status = "Up to date";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// subclass can override this to, for example, use the path of a sidecar file
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string GetActualPath(string path)
		{
			return path;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<T> GetAllFileData()
		{
			lock (((ICollection)_fileToDataDictionary).SyncRoot)
			{
				//Give a copy (a list) because if we just give an enumerator, we'll get
				//an error if/when this collection is altered on another thread
				return _fileToDataDictionary.Values.ToList();
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual void ProcessAllFilesInFolder(string folder)
		{
			ProcessAllFiles(folder, false);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void ProcessAllFiles()
		{
			//now that the watcher is up and running, gather up all existing files
			lock (((ICollection)_fileToDataDictionary).SyncRoot)
			{
				_fileToDataDictionary.Clear();
			}

			ProcessAllFiles(RootDirectoryPath, true);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void ProcessAllFiles(string topLevelFolder, bool searchSubFolders)
		{
			Status = "Working";

			var paths = new List<string>();
			paths.AddRange(Directory.GetFiles(topLevelFolder, "*.*",
				searchSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));

			for (int i = 0; i < paths.Count; i++)
			{
				if (ShouldStop)
					break;

				Status = string.Format("Processing {0} of {1} files", 1 + i, paths.Count);
				CollectDataForFile(paths[i]);
			}

			Status = "Up to date";
		}

		/// ------------------------------------------------------------------------------------
		public void Restart()
		{
			_restartRequested = true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool ShouldStop
		{
			get
			{
				const ThreadState stopStates = ThreadState.StopRequested |
					ThreadState.AbortRequested | ThreadState.Stopped | ThreadState.Aborted;

				return ((Thread.CurrentThread.ThreadState & stopStates) > 0);
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool Busy
		{
			get { return Status == "Working"; }
		}

		/// ------------------------------------------------------------------------------------
		public string Status
		{
			//enhance: provide an enumeration for the use of tests (at least)
			get;
			private set;
		}
	}
}