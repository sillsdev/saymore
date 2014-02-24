using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using L10NSharp;
using Palaso.IO;
using Palaso.Reporting;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.ElementListScreen;
using Palaso.Extensions;

namespace SayMore.UI.NewSessionsFromFiles
{
	#region NewSessionsFromFileDlgViewModel class
	/// ----------------------------------------------------------------------------------------
	public class NewSessionsFromFileDlgViewModel : IDisposable
	{
		private readonly NewComponentFile.NewComponentFileFactory _newComponentFileFactory;

		public delegate NewSessionsFromFileDlgViewModel Factory(ElementListViewModel<Session> sessionPresentationModel);

		public bool WaitForAsyncFileLoadingToFinish { get; set; }

		protected ElementListViewModel<Session> SessionPresentationModel { get; private set; }

		private string _selectedFolder;
		private NewSessionsFromFilesDlg _dlg;
		private readonly BackgroundWorker _fileLoaderWorker;
		private readonly FileSystemWatcher _fileWatcher;

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

			Files = new List<NewComponentFile>();
		}

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
		public List<NewComponentFile> Files { get; private set; }

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
		public int NumberOfNewSessions
		{
			get
			{
				// SP-789: Do not add duplicate sessions
				//return Files.Count(x => x.Selected);

				var newCount = 0;
				HashSet<string> sessions = new HashSet<string>();

				foreach (var element in SessionPresentationModel.Elements)
					sessions.Add(element.Id);

				foreach (var sessName in Files.Where(f => f.Selected)
					.Select(file => Path.GetFileNameWithoutExtension(file.FileName))
					.Where(sessName => !string.IsNullOrEmpty(sessName))
					.Where(sessName => !sessions.Contains(sessName)))
				{
					newCount++;
					sessions.Add(sessName);
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
			get { return _selectedFolder; }
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
		private void HandleFileWatcherDeleteOrCreatedEvent(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType == WatcherChangeTypes.Deleted)
				RemoveFile(e.FullPath);
			else if (e.ChangeType == WatcherChangeTypes.Created)
				AddFile(e.FullPath);
		}

		/// ------------------------------------------------------------------------------------
		private void RemoveFile(string path)
		{
			var componentFile = Files.FirstOrDefault(x => x.PathToAnnotatedFile == path);
			if (componentFile != null)
				Files.Remove(componentFile);

			if (_dlg != null)
				_dlg.UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void AddFile(string path)
		{
			if (!File.Exists(path) || Files.Any(x => x.PathToAnnotatedFile == path))
				return;

			Files.Add(_newComponentFileFactory(path));
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
		private void RenameFile(string newPath, string oldPath)
		{
			var componentFile = Files.FirstOrDefault(x => x.PathToAnnotatedFile == oldPath);
			if (componentFile == null)
				return;

			componentFile.Rename(newPath);
			Files.Sort((x, y) => x.FileName.CompareTo(y.FileName));

			if (_dlg != null)
				_dlg.UpdateDisplay();
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
			return (fileIndex < 0 || fileIndex >= Files.Count ?
				string.Empty : Files[fileIndex].PathToAnnotatedFile);
		}

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
				dlg.Description = LocalizationManager.GetString("DialogBoxes.NewSessionsFromFilesDlg.FolderBrowserDlgDescription",
					"Choose a Folder of Media Files.");

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
		void HandleFileLoaderDoWork(object sender, DoWorkEventArgs e)
		{
			var fileList = e.Argument as string[];
			if (fileList == null)
				return;

			var validExtensions = new List<string>();
			validExtensions.AddRange(FileUtils.AudioFileExtensions.Cast<string>().Select(x => x.ToLower()));
			validExtensions.AddRange(FileUtils.VideoFileExtensions.Cast<string>().Select(x => x.ToLower()));

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
		void HandleFileLoaderProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (_dlg != null)
				_dlg.UpdateFileLoadingProgress();
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
		public IEnumerable<KeyValuePair<string, string>> GetAllSourceAndDestinationPairs()
		{
			var sessionsPath = SessionPresentationModel.PathToSessionsFolder;
			var sourceRole = ApplicationContainer.ComponentRoles.First(r => r.Id == ComponentRole.kSourceComponentRoleId);

			foreach (var source in Files.Where(file => file.Selected))
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

			// SP-789: Do not add duplicate sessions
			if (SessionPresentationModel.Elements.Any(e => e.Id == id))
			{
				// this is a duplicate
				Debug.Print(id);
				return;
			}

			var newSession = SessionPresentationModel.CreateNewElementWithId(id);

			string msg;
			var date = File.GetLastWriteTime(sourcePath);
			newSession.MetaDataFile.SetStringValue(SessionFileType.kDateFieldName, date.ToISO8601DateOnlyString(), out msg);
			newSession.MetaDataFile.SetStringValue(SessionFileType.kStatusFieldName, Session.Status.Incoming.ToString(), out msg);
			newSession.MetaDataFile.Save();

			if (FirstNewSessionAdded == null)
				FirstNewSessionAdded = newSession.Id;
		}

		#endregion
	}

	#endregion
}
