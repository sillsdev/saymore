using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using L10NSharp;
using SIL.IO;
using SIL.Reporting;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.ElementListScreen;
using SIL.Extensions;

namespace SayMore.UI.NewSessionsFromFiles
{
	#region NewSessionsFromFileDlgViewModel class
	/// ----------------------------------------------------------------------------------------
	public class NewSessionsFromFileDlgViewModel : IDisposable
	{
		private readonly NewComponentFile.NewComponentFileFactory _newComponentFileFactory;

		public delegate NewSessionsFromFileDlgViewModel Factory(ElementListViewModel<Session> sessionPresentationModel);

		private ElementListViewModel<Session> SessionPresentationModel { get; }

		private string _selectedFolder;
		private readonly BackgroundWorker _fileLoaderWorker;
		private readonly FileSystemWatcher _fileWatcher;
		protected readonly List<NewComponentFile> m_files;
		private bool m_folderWasMissingOnLastLoadAttempt;

		public delegate void FilesChangedHandler(ReadOnlyCollection<NewComponentFile> files, bool isFolderMissing);

		public event FilesChangedHandler FilesChanged;

		public delegate void FileLoadingStartedHandler(int numberOfFilesToLoad);

		public event FileLoadingStartedHandler FileLoadingStarted;

		public delegate void FilesLoadedHandler(int numberOfFilesLoaded);

		public event FilesLoadedHandler FilesLoaded;

		public event FilesChangedHandler FileLoadingCompleted;

		#region Construction, initialization and disposal.
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFileDlgViewModel(ElementListViewModel<Session> sessionPresentationModel,
			NewComponentFile.NewComponentFileFactory newComponentFileFactory)
		{
			_newComponentFileFactory = newComponentFileFactory;
			SessionPresentationModel = sessionPresentationModel;

			_fileWatcher = new FileSystemWatcher();
			_fileWatcher.EnableRaisingEvents = false;
			_fileWatcher.IncludeSubdirectories = false;
			_fileWatcher.Renamed += HandleFileWatcherRenameEvent;
			_fileWatcher.Deleted += HandleFileWatcherDeleteOrCreatedEvent;
			_fileWatcher.Created += HandleFileWatcherDeleteOrCreatedEvent;

			_fileLoaderWorker = new BackgroundWorker();
			_fileLoaderWorker.WorkerReportsProgress = true;
			_fileLoaderWorker.WorkerSupportsCancellation = true;
			_fileLoaderWorker.ProgressChanged += HandleFileLoaderProgressChanged;
			_fileLoaderWorker.RunWorkerCompleted += HandleFileLoaderComplete;
			_fileLoaderWorker.DoWork += HandleFileLoaderDoWork;

			m_files = new List<NewComponentFile>();
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			Application.Idle -= HandleApplicationIdle;
			_fileWatcher.Dispose();
			_fileWatcher.SynchronizingObject = null;
			_fileLoaderWorker.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the dialog for which this class is the view model.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Initialize(ISynchronizeInvoke syncObject)
		{
			Application.Idle -= HandleApplicationIdle; // just in case this gets called more than once
			_fileWatcher.SynchronizingObject = syncObject;
			EnableFileWatchingIfAble();
			if (syncObject != null)
				Application.Idle += HandleApplicationIdle;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the ID of the first of the newly added sessions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FirstNewSessionAdded { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating how many files are selected from which sessions will be
		/// created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int NumberOfNewSessions
		{
			get
			{
				// SP-789: Do not add duplicate sessions
				//return Files.Count(x => x.Selected);

				var newCount = 0;
				var sessions = new HashSet<string>();

				foreach (var element in SessionPresentationModel.Elements)
					sessions.Add(element.Id);

				lock (m_files)
				{
					foreach (var sessionName in m_files.Where(f => f.Selected)
						.Select(file => Path.GetFileNameWithoutExtension(file.FileName))
						.Where(name => !string.IsNullOrEmpty(name) && !sessions.Contains(name)))
					{
						newCount++;
						sessions.Add(sessionName);
					}
				}

				return newCount;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the selected folder from which new sessions will be created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SelectedFolder
		{
			get => _selectedFolder;
			set
			{
				_selectedFolder = value;
				LoadFilesFromFolder(value);
				Settings.Default.NewSessionsFromFilesLastFolder = value;
				EnableFileWatchingIfAble();
			}
		}

		#endregion

		#region Methods for watching the file system
		/// ------------------------------------------------------------------------------------
		private void EnableFileWatchingIfAble()
		{
			if (_fileWatcher.SynchronizingObject == null || !Directory.Exists(_selectedFolder))
				_fileWatcher.EnableRaisingEvents = false;
			else
			{
				_fileWatcher.Path = _selectedFolder;
				_fileWatcher.EnableRaisingEvents = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Monitor the selected folder in case it disappears or reappears (e.g. when the user
		/// plug-in or unplugs the device containing the folder).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleApplicationIdle(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(SelectedFolder) || _fileWatcher.SynchronizingObject == null || _fileLoaderWorker.IsBusy)
				return;

			if ((!Directory.Exists(SelectedFolder) && !m_folderWasMissingOnLastLoadAttempt) ||
				(Directory.Exists(SelectedFolder) && m_folderWasMissingOnLastLoadAttempt))
			{
				LoadFilesFromFolder(SelectedFolder);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFileWatcherDeleteOrCreatedEvent(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType == WatcherChangeTypes.Deleted)
				RemoveFile(e.FullPath);
			else if (e.ChangeType == WatcherChangeTypes.Created)
				AddFile(e.FullPath);
		}

		/// ------------------------------------------------------------------------------------
		/// Must only be called from context where m_files is locked!
		/// ------------------------------------------------------------------------------------
		private void RaiseFilesChangedEvent(bool fileLoadingCompleted = false)
		{
			var files = new ReadOnlyCollection<NewComponentFile>(m_files);
			m_folderWasMissingOnLastLoadAttempt = (!string.IsNullOrEmpty(SelectedFolder) &&
													!Directory.Exists(SelectedFolder));
			if (fileLoadingCompleted)
			{
				if (FileLoadingCompleted != null)
					FileLoadingCompleted(files, m_folderWasMissingOnLastLoadAttempt);
			}
			else
			{
				if (FilesChanged != null)
					FilesChanged(files, m_folderWasMissingOnLastLoadAttempt);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void RemoveFile(string path)
		{
			lock (m_files)
			{
				var componentFile = m_files.FirstOrDefault(x => x.PathToAnnotatedFile == path);
				if (componentFile != null)
					m_files.Remove(componentFile);

				RaiseFilesChangedEvent();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void AddFile(string path)
		{
			lock (m_files)
			{
				if (!File.Exists(path) || m_files.Any(x => x.PathToAnnotatedFile == path))
					return;

				m_files.Add(_newComponentFileFactory(path));
				SortFiles();

				RaiseFilesChangedEvent();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles a project file or folder being renamed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFileWatcherRenameEvent(object sender, RenamedEventArgs e)
		{
			RenameFile(e.FullPath, e.OldFullPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Finds the file in the file list having the specified old path and renames its
		/// file to the specified new path, finishing by sorting the file list accordingly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RenameFile(string newPath, string oldPath)
		{
			lock (m_files)
			{
				var componentFile = m_files.FirstOrDefault(x => x.PathToAnnotatedFile == oldPath);
				if (componentFile == null)
					return;

				componentFile.Rename(newPath);
				SortFiles();

				RaiseFilesChangedEvent();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// Must only be called from context where m_files is locked!
		/// ------------------------------------------------------------------------------------
		private void SortFiles()
		{
			m_files.Sort((x, y) => String.Compare(x.FileName, y.FileName, StringComparison.Ordinal));
		}
		#endregion

		#region Misc. public methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// An empty string is returned if fileIndex is out of range.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetFullFilePath(int fileIndex)
		{
			lock (m_files)
			{
				return (fileIndex < 0 || fileIndex >= m_files.Count
					? string.Empty : m_files[fileIndex].PathToAnnotatedFile);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void ToggleFilesSelectedState(int fileIndex)
		{
			lock (m_files)
			{
				if (fileIndex >= 0 && fileIndex < m_files.Count)
				{
					m_files[fileIndex].Selected = !m_files[fileIndex].Selected;
					RaiseFilesChangedEvent();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Selects or deselects all the files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SelectAllFiles(bool select)
		{
			lock (m_files)
			{
				foreach (var file in m_files)
					file.Selected = select;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void LetUserChangeSelectedFolder()
		{
			bool interruptedLoad = false;
			if (_fileLoaderWorker.IsBusy && !_fileLoaderWorker.CancellationPending)
			{
				_fileLoaderWorker.CancelAsync();
				interruptedLoad = true;
			}

			using (var dlg = new FolderBrowserDialog())
			{
				dlg.Description = LocalizationManager.GetString("DialogBoxes.NewSessionsFromFilesDlg.FolderBrowserDlgDescription",
					"Choose a Folder of Media Files.");

				if (SelectedFolder != null && Directory.Exists(SelectedFolder))
					dlg.SelectedPath = SelectedFolder;

				if (dlg.ShowDialog() == DialogResult.OK)
					SelectedFolder = dlg.SelectedPath;
				else if (interruptedLoad)
					SelectedFolder = _selectedFolder;
			}
		}

		#endregion

		#region Methods for loading (in a new process thread) files from selected folder
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of files from the audio and video files found in the specified
		/// folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadFilesFromFolder(string folder)
		{
			if (_fileLoaderWorker.IsBusy && !_fileLoaderWorker.CancellationPending)
				_fileLoaderWorker.CancelAsync();

			_fileWatcher.EnableRaisingEvents = false;

			lock (m_files)
			{
				m_files.Clear();

				RaiseFilesChangedEvent();
			}

			if (folder == null || !Directory.Exists(folder))
				return;

			var fileList = Directory.GetFiles(folder);
			if (FileLoadingStarted != null)
				FileLoadingStarted(fileList.Length);

			_fileLoaderWorker.RunWorkerAsync(fileList);
		}

		/// ------------------------------------------------------------------------------------
		void HandleFileLoaderDoWork(object sender, DoWorkEventArgs e)
		{
			var fileList = e.Argument as string[];
			if (fileList == null)
				return;

			var validExtensions = new List<string>();
			validExtensions.AddRange(FileUtils.AudioFileExtensions.Cast<string>().Select(x => x.ToLower()));
			validExtensions.AddRange(FileUtils.VideoFileExtensions.Cast<string>().Select(x => x.ToLower()));

			lock (m_files)
			{
				foreach (var file in fileList)
				{
					// If the file's path no longer exists, it probably means the user disconnected the
					// storage device (e.g. recorder) before reading all its contents.
					if (_fileLoaderWorker.CancellationPending || !Directory.Exists(Path.GetDirectoryName(file)))
					{
						e.Cancel = true;
						return;
					}

					_fileLoaderWorker.ReportProgress(0);

					try
					{
						if (validExtensions.Contains(Path.GetExtension(file).ToLower()))
						{
							var sessionFile = _newComponentFileFactory(file);
							sessionFile.Selected = (new FileInfo(file).Attributes & FileAttributes.Archive) > 0;
							m_files.Add(sessionFile);
						}
					}
					catch (Exception)
					{
						if (Directory.Exists(Path.GetDirectoryName(file)))
							throw;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleFileLoaderProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (FilesLoaded != null)
				FilesLoaded(1);
		}

		/// ------------------------------------------------------------------------------------
		public void CancelLoad()
		{
			if (!_fileLoaderWorker.CancellationPending)
				_fileLoaderWorker.CancelAsync();
		}

		/// ------------------------------------------------------------------------------------
		void HandleFileLoaderComplete(object sender, RunWorkerCompletedEventArgs e)
		{
			lock (m_files)
			{
				if (e.Cancelled)
					m_files.Clear();
				else
				{
					SortFiles();
					_fileWatcher.Path = _selectedFolder;
				}

				RaiseFilesChangedEvent(true);
			}

			EnableFileWatchingIfAble();
		}

		#endregion

		#region Methods for creating sessions from selected files
		/// ------------------------------------------------------------------------------------
		public IEnumerable<KeyValuePair<string, string>> GetAllSourceAndDestinationPairs()
		{
			var sessionsPath = SessionPresentationModel.PathToSessionsFolder;
			var sourceRole = ApplicationContainer.ComponentRoles.First(r => r.Id == ComponentRole.kSourceComponentRoleId);

			lock (m_files)
			{
				foreach (var source in m_files.Where(file => file.Selected))
				{
					var srcFile = source.PathToAnnotatedFile;
					var sessionName = Path.GetFileNameWithoutExtension(srcFile);
					if (sessionName == null)
						continue;
					var destPath = Path.Combine(Path.Combine(sessionsPath, sessionName),
						sourceRole.GetCanoncialName(sessionName, Path.GetFileName(srcFile)));
					yield return new KeyValuePair<string, string>(srcFile, destPath);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list source/destination file path pairs where the destination file is sure
		/// not to exist. If any of the destination files already exist, a message box will
		/// be displayed, telling the user of that fact.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<KeyValuePair<string, string>> GetUniqueSourceAndDestinationPairs()
		{
			var pairs = GetAllSourceAndDestinationPairs();

			var existing = (from kvp in GetAllSourceAndDestinationPairs()
						   where File.Exists(kvp.Value)
						   select kvp.Key).ToList();

			if (existing.Count > 0)
			{
				// Remove the pairs for which sessions already exist.
				pairs = pairs.Where(kvp => !existing.Contains(kvp.Key));

				// Build a string showing all the session Ids that will be skipped.
				var bldr = new StringBuilder();
				foreach (var file in existing)
					bldr.AppendLine(Path.GetFileNameWithoutExtension(file));

				var msg = LocalizationManager.GetString("DialogBoxes.NewSessionsFromFilesDlg.SessionsAlreadyExistMsg",
					"Sessions already exist for the following Ids. These will be skipped.\n\n{0}");

				ErrorReport.NotifyUserOfProblem(msg, bldr);
			}

			return pairs;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a session to "receive" the specified file. New session names are the name of
		/// the file without the extension. The Date of the session is the file's DateModified
		/// date.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CreateSingleSession(string sourcePath)
		{
			var id = Path.GetFileNameWithoutExtension(sourcePath);
			Debug.Assert(id != null);

			// SP-789: Do not add duplicate sessions
			if (SessionPresentationModel.Elements.Any(e => e.Id == id))
			{
				// this is a duplicate
				Debug.Print(id);
				return;
			}

			var newSession = SessionPresentationModel.CreateNewElementWithId(id);

			var date = File.GetLastWriteTime(sourcePath);
			newSession.MetaDataFile.SetStringValue(SessionFileType.kDateFieldName, date.ToISO8601TimeFormatDateOnlyString());
			newSession.MetaDataFile.SetStringValue(SessionFileType.kStatusFieldName, nameof(Session.Status.Incoming));
			newSession.MetaDataFile.Save();

			if (FirstNewSessionAdded == null)
				FirstNewSessionAdded = newSession.Id;
		}

		#endregion

		public void SetFileSelectionState(int index, bool value)
		{
			lock (m_files)
			{
				if (index < m_files.Count)
					m_files[index].Selected = value;
				else
				{
					// If not, maybe the collection is changing, so just log the evidence.
					Logger.WriteEvent("Attempted to set file selection state to {0} for file at index {1}, but collection only contained {2} files",
						value, index, m_files.Count);
				}
			}
		}
	}

	#endregion
}
