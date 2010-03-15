using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
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
		private string _selectedFolder;
		private NewSessionsFromFilesDlg _dlg;

		#region Construction, initialization and disposal.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NewSessionsFromFileDlgViewModel"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFileDlgViewModel()
		{
			Files = new List<NewSessionFile>();
			SelectedFolder = Settings.Default.NewSessionsFromFilesLastFolder;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			Application.Idle -= HandleApplicationIdle;
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
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Monitor the selected folder in case it disappears or reappears (e.g. when the user
		/// plug-in or unplugs the device containing the folder).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleApplicationIdle(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(SelectedFolder) || _dlg == null)
				return;

			if ((!Directory.Exists(SelectedFolder) && !_dlg.IsSelectedFolderMissing) ||
				(Directory.Exists(SelectedFolder) && _dlg.IsSelectedFolderMissing))
			{
				LoadFilesFromFolder(SelectedFolder);
				_dlg.UpdateDisplay();
			}
		}

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
			using (var dlg = new FolderBrowserDialog())
			{
				dlg.Description = LocalizationManager.LocalizeString(
					"NewSessionsFromFilesDlg.FolderBrowserDlgDescription",
					"Choose a Folder Medial Files.", "Dialog Boxes");

				if (SelectedFolder != null && Directory.Exists(SelectedFolder))
					dlg.SelectedPath = SelectedFolder;

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					SelectedFolder = dlg.SelectedPath;
					if (_dlg != null)
						_dlg.UpdateDisplay();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of files from the audio and video files found in the specified
		/// folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadFilesFromFolder(string folder)
		{
			Files.Clear();

			if (folder == null || !Directory.Exists(folder))
				return;

			var validExtensions = (Settings.Default.AudioFileExtensions +
				Settings.Default.VideoFileExtensions).ToLower();

			foreach (var file in Directory.GetFiles(folder))
			{
				if (validExtensions.Contains(Path.GetExtension(file.ToLower())))
					Files.Add(new NewSessionFile(file));
			}

			Files.Sort((x, y) => x.FileName.CompareTo(y.FileName));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CreateSessions()
		{
			using (var statusDlg = new StatusDlg())
				statusDlg.ShowAndProcess(GetStatusMessageForFile, CreateSingleSession, Files);
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
		/// Performs the actual work of creating a session from the specified file. New
		///	session names are the name of the file without the extension. The Date of the
		/// session is the file's DateModified date.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool CreateSingleSession(object obj)
		{
			var file = obj as NewSessionFile;

			if (file == null || !file.Selected)
				return false;

			var sessionId = Path.GetFileNameWithoutExtension(file.FileName);
			var session = Session.Create(MainWnd.CurrentProject, sessionId);
			session.Date = DateTime.Parse(file.DateModified);
			session.Save();
			session.AddFile(file.FullFilePath);

			if (FirstNewSessionAdded == null)
				FirstNewSessionAdded = sessionId;

			return true;
		}
	}

	#endregion
}
