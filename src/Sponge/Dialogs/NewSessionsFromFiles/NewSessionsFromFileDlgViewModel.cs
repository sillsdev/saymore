using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
using SIL.Sponge.Dialogs.NewSessionsFromFiles.CopyFiles;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;
using SilUtils;

namespace SIL.Sponge.Dialogs
{
	#region NewSessionFile class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates all the file information displayed in the NewSessionsFromFileDlg.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class NewSessionFile : SessionFileBase
	{
		public bool Selected { get; set; }
		public NewSessionFile(string fullFilePath) : base(fullFilePath)
		{
			Selected = true;
		}
	}

	#endregion

	#region NewSessionsFromFileDlgModel class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class NewSessionsFromFileDlgViewModel : IDisposable
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used by
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool WaitForAsyncFileLoadingToFinish { get; set; }

		private string _selectedFolder;
		private NewSessionsFromFilesDlg _dlg;
		private readonly BackgroundWorker _fileLoaderWorker;
		private readonly FileSystemWatcher _fileWatcher;

		#region Construction, initialization and disposal.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NewSessionsFromFileDlgViewModel"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFileDlgViewModel()
		{
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

			Files = new List<NewSessionFile>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			Application.Idle -= HandleApplicationIdle;
			_fileWatcher.Dispose();
			_fileLoaderWorker.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the dialog for which this class is the view model.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFilesDlg Dialog
		{
			get { return _dlg; }
			set
			{
				Application.Idle -= HandleApplicationIdle;
				_dlg = value;
				_fileWatcher.SynchronizingObject = _dlg;
				EnableFileWatchingIfAble();
				if (_dlg != null)
					Application.Idle += HandleApplicationIdle;
			}
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the Id of the first of the newly added sessions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FirstNewSessionAdded { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of potential session files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<NewSessionFile> Files { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not any files are selected from which a
		/// session will be created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AnyFilesSelected
		{
			get { return Files.Any(x => x.Selected); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not all files are selected.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AllFilesSelected
		{
			get { return Files.TrueForAll(x => x.Selected); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating how many files are selected from which sessions will be
		/// created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int NumberOfSelectedFiles
		{
			get { return Files.Count(x => x.Selected); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the selected folder from which new sessions will be created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SelectedFolder
		{
			get { return _selectedFolder; }
			set
			{
				_selectedFolder = value;
				LoadFilesFromFolder(value);
				Settings.Default.NewSessionsFromFilesLastFolder = value;
				Settings.Default.Save();
				EnableFileWatchingIfAble();
			}
		}

		#endregion

		#region Methods for watching the file system
		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void EnableFileWatchingIfAble()
		{
			if (_dlg == null || !Directory.Exists(_selectedFolder))
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
		void HandleApplicationIdle(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(SelectedFolder) || _dlg == null || _fileLoaderWorker.IsBusy)
				return;

			if ((!Directory.Exists(SelectedFolder) && !_dlg.IsMissingFolderMessageVisible) ||
				(Directory.Exists(SelectedFolder) && _dlg.IsMissingFolderMessageVisible))
			{
				LoadFilesFromFolder(SelectedFolder);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFileWatcherDeleteOrCreatedEvent(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType == WatcherChangeTypes.Deleted)
				RemoveFile(e.FullPath);
			else if (e.ChangeType == WatcherChangeTypes.Created)
				AddFile(e.FullPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RemoveFile(string fullPath)
		{
			var nsf = Files.FirstOrDefault(x => x.FullFilePath == fullPath);
			if (nsf != null)
				Files.Remove(nsf);

			if (_dlg != null)
				_dlg.UpdateDisplay();
			}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddFile(string fullPath)
		{
			if (!File.Exists(fullPath) || Files.Any(x => x.FullFilePath == fullPath))
				return;

			Files.Add(new NewSessionFile(fullPath));
			Files.Sort((x, y) => x.FileName.CompareTo(y.FileName));

			if (_dlg != null)
				_dlg.UpdateDisplay();
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
		private void RenameFile(string newFullPath, string oldFullPath)
		{
			var nsf = Files.FirstOrDefault(x => x.FullFilePath == oldFullPath);
			if (nsf == null)
				return;

			nsf.FullFilePath = newFullPath;
			Files.Sort((x, y) => x.FileName.CompareTo(y.FileName));

			if (_dlg != null)
				_dlg.UpdateDisplay();
		}

		#endregion

		#region Misc. public methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the specified property value for the file at the specified index in the
		/// files list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public object GetPropertyValueForFile(int fileIndex, string property)
		{
			return (fileIndex < 0 || fileIndex >= Files.Count ? null :
				ReflectionHelper.GetProperty(Files[fileIndex], property));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// An empty string is returned if fileIndex is out of range.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetFullFilePath(int fileIndex)
		{
			return (fileIndex < 0 || fileIndex >= Files.Count ?
				string.Empty : Files[fileIndex].FullFilePath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ToggleFilesSelectedState(int fileIndex)
		{
			if (fileIndex >= 0 && fileIndex < Files.Count)
			{
				Files[fileIndex].Selected = !Files[fileIndex].Selected;
				if (_dlg != null)
					_dlg.UpdateDisplay();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Selects or deselects all the files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SelectAllFiles(bool select)
		{
			foreach (var file in Files)
				file.Selected = select;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LetUserChangeSelectedFolder()
		{
			bool interuptedLoad = false;
			if (_fileLoaderWorker.IsBusy && !_fileLoaderWorker.CancellationPending)
			{
				_fileLoaderWorker.CancelAsync();
				interuptedLoad = true;
			}

			using (var dlg = new FolderBrowserDialog())
			{
				dlg.Description = LocalizationManager.LocalizeString(
					"NewSessionsFromFilesDlg.FolderBrowserDlgDescription",
					"Choose a Folder of Medial Files.", "Dialog Boxes");

				if (SelectedFolder != null && Directory.Exists(SelectedFolder))
					dlg.SelectedPath = SelectedFolder;

				if (dlg.ShowDialog() == DialogResult.OK)
					SelectedFolder = dlg.SelectedPath;
				else if (interuptedLoad)
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
			Files.Clear();

			if (_dlg != null)
				_dlg.UpdateDisplay();

			if (folder == null || !Directory.Exists(folder))
				return;

			var fileList = Directory.GetFiles(folder);
			if (_dlg != null)
				_dlg.InitializeProgressIndicatorForFileLoading(fileList.Length);

			_fileLoaderWorker.RunWorkerAsync(fileList);

			if (WaitForAsyncFileLoadingToFinish)
				while (_fileLoaderWorker.IsBusy) { Application.DoEvents(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleFileLoaderDoWork(object sender, DoWorkEventArgs e)
		{
			var fileList = e.Argument as string[];
			if (fileList == null)
				return;

			var validExtensions = (Settings.Default.AudioFileExtensions +
				Settings.Default.VideoFileExtensions).ToLower();

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
				if (validExtensions.Contains(Path.GetExtension(file.ToLower())))
				{
					var sessionFile = new NewSessionFile(file);
					sessionFile.Selected = (new FileInfo(file).Attributes & FileAttributes.Archive)>0;
					Files.Add(sessionFile);
				}
			}
				catch (Exception)
				{
					if (Directory.Exists(Path.GetDirectoryName(file)))
						throw;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleFileLoaderProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (_dlg != null)
				_dlg.UpdateFileLoadingProgress();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CancelLoad()
		{
			if (!_fileLoaderWorker.CancellationPending)
				_fileLoaderWorker.CancelAsync();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleFileLoaderComplete(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
				Files.Clear();
			else
			{
			Files.Sort((x, y) => x.FileName.CompareTo(y.FileName));
				_fileWatcher.Path = _selectedFolder;
			}

			if (_dlg != null)
				_dlg.FileLoadingProgressComplele();

			EnableFileWatchingIfAble();
		}

		#endregion

		#region Methods for creating sessions from selected files
		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<KeyValuePair<string, string>> GetSourceAndDestinationPairs()
		{
			var pathpairs = new List<KeyValuePair<string, string>>();
			foreach (var source in Files)
			{
				if (source.Selected)
				{
					string destPath = Path.Combine(MainWnd.CurrentProject.SessionsFolder,
												   Path.GetFileNameWithoutExtension(source.FullFilePath));
					destPath = Path.Combine(destPath,
												   Path.GetFileName(source.FullFilePath));
					pathpairs.Add(new KeyValuePair<string, string>(source.FullFilePath, destPath));
				}
			}
			return pathpairs;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called by the background worker in the status dialog when it's about to process
		/// one of the items in its list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetStatusMessageForFile(object obj)
		{
			var file = obj as NewSessionFile;

			if (file == null || !file.Selected)
				return null;

			var sessionId = Path.GetFileNameWithoutExtension(file.FileName);
			return string.Format("Creating session '{0}'", sessionId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a session which will "receive" the specified file. New
		///	session names are the name of the file without the extension. The Date of the
		/// session is the file's DateModified date.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CreateSingleSession(string sourcePath)
		{
			var file = new NewSessionFile(sourcePath);

			var sessionId = Path.GetFileNameWithoutExtension(file.FileName);
			var session = Session.Create(MainWnd.CurrentProject, sessionId);//todo: remove static
			session.Date = File.GetLastWriteTime(file.FullFilePath);
			session.Save();

			if (FirstNewSessionAdded == null)
				FirstNewSessionAdded = sessionId;
		}

		#endregion
	}

	#endregion
}
