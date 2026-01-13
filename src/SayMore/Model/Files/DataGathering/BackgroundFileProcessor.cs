using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using L10NSharp;
using SIL.Reporting;
using ThreadState = System.Threading.ThreadState;

namespace SayMore.Model.Files.DataGathering
{
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
	/// is quickly accessible when needed.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public abstract class BackgroundFileProcessor<T> : IDisposable where T : class
	{
		public const string kNotYetStartedStatus = "Not yet started";
		public const string kWorkingStatus = "Working";
		public const string kUpToDataStatus = "Up to date";

		private Thread _workerThread;
		public string RootDirectoryPath { get; protected set; }
		protected readonly IEnumerable<FileType> _typesOfFilesToProcess;
		protected readonly Func<string, T> _fileDataFactory;
		protected bool _restartRequested = true;
		protected Dictionary<string, T> _fileToDataDictionary = new();
		private readonly Queue<FileSystemEventArgs> _pendingFileEvents;
		private volatile int _suspendEventProcessingCount;
		private readonly object _lockObj = new();
		private readonly object _lockSuspendObj = new();

		public event EventHandler NewDataAvailable;
		public event EventHandler FinishedProcessingAllFiles;

		/// ------------------------------------------------------------------------------------
		protected BackgroundFileProcessor(string rootDirectoryPath,
			IEnumerable<FileType> typesOfFilesToProcess, Func<string, T> fileDataFactory)
		{
			RootDirectoryPath = rootDirectoryPath;
			_typesOfFilesToProcess = typesOfFilesToProcess;
			_fileDataFactory = fileDataFactory;
			Status = kNotYetStartedStatus;
			_pendingFileEvents = new Queue<FileSystemEventArgs>();
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			_workerThread?.Abort(); //will eventually lead to it stopping
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
			lock (_lockSuspendObj)
			{
				_suspendEventProcessingCount++;
			}
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
			lock (_lockSuspendObj)
			{
				if (_suspendEventProcessingCount > 0)
					_suspendEventProcessingCount--;
			}

			if (processAllPendingEventsNow && _suspendEventProcessingCount == 0)
			{
				lock (_lockObj)
				{
					while (_pendingFileEvents.Count > 0)
						ProcessFileEvent(_pendingFileEvents.Dequeue());
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool GetDoIncludeFile(string path)
		{
			var fileName = Path.GetFileName(path);
			return fileName != null && !fileName.StartsWith(".") &&
			       _typesOfFilesToProcess.Any(t => t.IsMatch(path));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual ThreadPriority ThreadPriority => ThreadPriority.Lowest;

		/// ------------------------------------------------------------------------------------
		public virtual void Start()
		{
			_workerThread = new Thread(StartWorking);
			_workerThread.Name = GetType().Name;
			_workerThread.Priority = ThreadPriority;
			_workerThread.TrySetApartmentState(ApartmentState.STA);//needed in case we eventually show an error & need to talk to email.
			_workerThread.Start();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnNewDataAvailable(T fileData)
		{
			NewDataAvailable?.Invoke(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private void StartWorking()
		{
			try
			{
				Status = kWorkingStatus; //NB: this helps simplify unit tests, if go to the busy state before returning

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
							Status = kWorkingStatus;
							ProcessAllFiles();
						}

						lock (_lockObj)
						{
							if (_pendingFileEvents.Count > 0 && _suspendEventProcessingCount == 0)
								ProcessFileEvent(_pendingFileEvents.Dequeue());
						}

// ReSharper disable once ConditionIsAlwaysTrueOrFalse
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
				ErrorReport.NotifyUserOfProblem(error, "Background file watching failed.");
			}
		}

		/// ------------------------------------------------------------------------------------
		private void QueueFileEvent(object sender, FileSystemEventArgs e)
		{
			lock (_lockObj)
			{
				if (!_pendingFileEvents.Any(fsea =>
					fsea.FullPath == e.FullPath && fsea.ChangeType == e.ChangeType))
				{
					Debug.WriteLine(GetType().Name + ": " + e.ChangeType + ": " + e.Name);
					_pendingFileEvents.Enqueue(e);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ProcessFileEvent(FileSystemEventArgs fileEvent)
		{
			try
			{
				if (fileEvent is RenamedEventArgs e)
				{
					lock (((ICollection)_fileToDataDictionary).SyncRoot)
					{
						if (_fileToDataDictionary.TryGetValue(e.OldFullPath, out var fileData))
						{
							_fileToDataDictionary.Remove(e.OldFullPath);
							_fileToDataDictionary[e.FullPath] = fileData;
						}
					}
				}
				else if (GetDoIncludeFile(fileEvent.FullPath))
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
				Logger.WriteEvent("Handled Exception in {0}.ProcessingFileEvent:\r\n{1}", GetType().Name, e.ToString());
#if  DEBUG
				ErrorReport.NotifyUserOfProblem(e, "Error gathering data");
#endif
				//nothing here is worth crashing over
			}
		}

		/// ------------------------------------------------------------------------------------
		public T GetFileData(string filePath)
		{
			lock (((ICollection)_fileToDataDictionary).SyncRoot)
			{
				if (_fileToDataDictionary.TryGetValue(filePath, out var stats))
					return stats;

				if (GetDoIncludeFile(filePath))
				{
					CollectDataForFile(filePath);
					return _fileToDataDictionary.TryGetValue(filePath, out stats) ? stats : null;
				}
				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void CollectDataForFile(string path)
		{
			T fileData = null;

			var priorStatus = Status;
			Status = kWorkingStatus;

			try
			{
				var actualPath = GetActualPath(path);

				if (!ShouldStop && File.Exists(actualPath))
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
				// REVIEW: What specific types of (common?) exceptions were we hoping to ignore here. This represents a lot of code with
				// a lot of possible errors that could be ignored.

				Debug.WriteLine(e.Message);

				// To try to solve SP-898 and SP-915, I'm ensuring that we DO report any ArgumentException or OutOfMemoryException.
				// If this starts catching stuff we'd really rather ignore, this part of the try-catch block can maybe be moved into
				// ComponentFile.GetSmallIconAndFileType to catch errors in the call to Icon.ToBitmap.
				Logger.WriteEvent("Exception caught in {0}.CollectDataForFile. path = {1}\r\nException: {2}", GetType(), path, e.ToString());
				if (e is ArgumentException || e is OutOfMemoryException)
				{
					ErrorReport.NotifyUserOfProblem(new ShowOncePerSessionBasedOnExactMessagePolicy(), e,
						string.Format(LocalizationManager.GetString("MainWindow.AutoCompleteValueGathererError",
						"An error of type {0} occurred trying to gather information from file: {1}",
						"Parameter 0 is an exception type; parameter 1 is a file name"), e.GetType(), path));
				}
				else
				{
#if  DEBUG
					ErrorReport.NotifyUserOfProblem(e, "Error gathering data");
#endif
				}
				//nothing here is worth crashing over
			}

			OnNewDataAvailable(fileData);
			Status = priorStatus;
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
				return _fileToDataDictionary.Values.Where(v => v != null).ToList();
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
			// Now that the watcher is up and running, gather up all existing files
			lock (((ICollection)_fileToDataDictionary).SyncRoot)
			{
				_fileToDataDictionary.Clear();
			}

			ProcessAllFiles(RootDirectoryPath, true);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void ProcessAllFiles(string topLevelFolder, bool searchSubFolders)
		{
			Status = kWorkingStatus;

			//var paths = new List<string>();
			//paths.AddRange(Directory.GetFiles(topLevelFolder, "*.*",
			//    searchSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
			var paths = WalkDirectoryTree(topLevelFolder, (searchSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));

			for (int i = 0; i < paths.Count; i++)
			{
				if (ShouldStop)
					break;

				while (true)
				{
					int suspendCount;
					lock (_lockObj)
					{
						suspendCount = _suspendEventProcessingCount;
					}

					if (suspendCount == 0)
						break;

					Thread.Sleep(100);
				}

				if (GetDoIncludeFile(paths[i]))
				{
					Status = string.Format("{0}: Processing {1} of {2} files",
						kWorkingStatus, 1 + i, paths.Count);

					CollectDataForFile(paths[i]);
				}
			}

			Status = kUpToDataStatus;

			FinishedProcessingAllFiles?.Invoke(this, EventArgs.Empty);
		}

		private static List<string> WalkDirectoryTree(string topLevelFolder, SearchOption searchOption)
		{
			IEnumerable<string> files = null;
			var returnVal = new List<string>();
			// First, process all the files directly under this folder
			try
			{
				// SP-879: Crash reading .DS_Store file on macOS
				files = Directory.GetFiles(topLevelFolder, "*.*").Where(name =>
				{
					var fileName = Path.GetFileName(name);
					return fileName != null && !fileName.StartsWith(".");
				});
				returnVal.AddRange(files);
			}
			catch (UnauthorizedAccessException)
			{
				// You may decide to do something different here.
				Debug.Print("Access denied: " + topLevelFolder);
			}
			catch (DirectoryNotFoundException)
			{
				// You may decide to do something different here.
				Debug.Print("Directory not found: " + topLevelFolder);
			}

			if (files != null && searchOption == SearchOption.AllDirectories)
			{
				// Now find all the subdirectories under this directory.
				var dirs = Directory.GetDirectories(topLevelFolder);
				foreach (var dir in dirs)
				{
					// Recursive call for each subdirectory.
					returnVal.AddRange(WalkDirectoryTree(dir, searchOption));
				}
			}

			return returnVal;
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
		public bool Busy => Status.StartsWith(kWorkingStatus);

		/// ------------------------------------------------------------------------------------
		public bool DataUpToDate => Status == kUpToDataStatus;

		/// ------------------------------------------------------------------------------------
		public string Status
		{
			//enhance: provide an enumeration for the use of tests (at least)
			get;
			private set;
		}
	}
}