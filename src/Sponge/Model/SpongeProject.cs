// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: SpongeProject.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using SIL.Localization;
using SIL.Sponge.ConfigTools;
using SilUtils;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates information for a single Sponge project.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("spongeproject")]
	public class SpongeProject : IDisposable
	{
		public event EventHandler ProjectChanged;
		private FileSystemWatcher _fileWatcher;

		#region Static methods/properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the specified project file. The file is a full path and file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SpongeProject Load(string prjFilePath)
		{
			Exception e;
			var prj = XmlSerializationHelper.DeserializeFromFile<SpongeProject>(prjFilePath, out e);
			if (e != null)
			{
				Utils.MsgBox(e.Message);
				return null;
			}

			var prjName = Path.GetDirectoryName(prjFilePath);
			int i = prjName.LastIndexOf(Path.DirectorySeparatorChar);
			prjName = (i >= 0 ? prjName.Substring(i + 1) : prjName);
			prj.Initialize(prjName, prjFilePath);
			return prj;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SpongeProject Create(IWin32Window parent)
		{
			var viewModel = new NewProjectDlgViewModel();

			using (var dlg = new NewProjectDlg(viewModel))
			{
				return (dlg.ShowDialog(parent) == DialogResult.OK ?
					Create(viewModel.NewProjectName) : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a Sponge project with the specified name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SpongeProject Create(string prjName)
		{
			var prj = new SpongeProject();
			prj.Initialize(prjName, null);
			return prj;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Since this class is designed to only be created through factories, prevent
		/// accidental direct construction
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private SpongeProject()
		{
			IsoCode = "X";//todo
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the parent folder for all project folders.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ProjectsFolder
		{
			get { return Path.Combine(Sponge.MainApplicationFolder, Sponge.ProjectFolderName); }
		}

		#endregion

		#region Initialization and Disposal
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SpongeProject"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Initialize(string prjName, string prjFileName)
		{
			Name = prjName;
			Folder = Path.Combine(ProjectsFolder, prjName);
			FileName = (prjFileName ?? prjName.Replace(" ", string.Empty));
			FileName = Path.ChangeExtension(FileName, Sponge.ProjectFileExtention);

			Sessions = new List<Session>();
			People = new List<Person>();

			if (!Directory.Exists(Folder))
				Directory.CreateDirectory(Folder);

			Save();

			InitializePeopleAndLanguages();
			InitializeSessions();

			_fileWatcher = new FileSystemWatcher(Folder);
			_fileWatcher.Renamed += HandleFileWatcherRename;
			_fileWatcher.Deleted += HandleFileWatcherEvent;
			_fileWatcher.Changed += HandleFileWatcherEvent;
			_fileWatcher.Created += HandleFileWatcherEvent;
			_fileWatcher.IncludeSubdirectories = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the people and languages list for the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializePeopleAndLanguages()
		{
			var path = Path.Combine(Folder, Sponge.PeopleFolderName);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			People = (from dir in Directory.GetDirectories(path)
					  orderby dir
					  select Person.Load(this, dir)).ToList();

			// Remove any null person objects.
			for (int i = People.Count - 1; i >= 0; i--)
			{
				if (People[i] == null)
					People.RemoveAt(i);
			}

			LanguageNames = new List<string>();

			foreach (var person in People)
			{
				AddLanguageNames(person.PrimaryLanguage);
				AddLanguageNames(person.OtherLangauge0);
				AddLanguageNames(person.OtherLangauge1);
				AddLanguageNames(person.OtherLangauge2);
				AddLanguageNames(person.OtherLangauge3);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the sessions for the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InitializeSessions()
		{
			if (!Directory.Exists(SessionsFolder))
				Directory.CreateDirectory(SessionsFolder);

			var x = (from folder in SessionNames
						orderby folder
						select Session.Load(this, Path.GetFileName(folder)));

			Sessions = (from session in x
					   where session != null	// sessions we couldn't load are null
					   select session).ToList();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or
		/// resetting unmanaged resources.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			_fileWatcher.Dispose();
			_fileWatcher = null;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified language to the list of all language names.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddLanguageNames(string langName)
		{
			if (!LanguageNames.Contains(langName))
				LanguageNames.Add(langName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			XmlSerializationHelper.SerializeToFile(FullFilePath, this);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether the file system is being monitored for
		/// changes to the project's files and folders.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool EnableFileWatching
		{
			get
			{
				return (_fileWatcher == null || _fileWatcher.SynchronizingObject == null ?
					false : _fileWatcher.EnableRaisingEvents);
			}
			set
			{
				if (_fileWatcher != null && _fileWatcher.SynchronizingObject != null)
					_fileWatcher.EnableRaisingEvents = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the file watcher's synchronizing object. Setting this to a control
		/// (e.g. the application's main window) created in the main thread keeps the file
		/// watcher from raising its events in a separate thread.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Control FileWatcherSynchronizingObject
		{
			get { return _fileWatcher.SynchronizingObject as Control; }
			set
			{
				_fileWatcher.SynchronizingObject = value;
				_fileWatcher.EnableRaisingEvents = (value != null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's sessions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<Session> Sessions { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's people.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<Person> People { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of all the names of the people in the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<string> PeopleNames
		{
			get
			{
				return (from x in People
						orderby x.FullName
						where !string.IsNullOrEmpty(x.FullName )
						select x.FullName).ToList();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string IsoCode { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of langauge names found in all the people records.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<string> LanguageNames { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Name { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Folder { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's file name and extension (not including its path).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FileName { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's path and file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FullFilePath
		{
			get { return Path.Combine(Folder, FileName); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the folder in which the project's people files are stored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string PeopleFolder
		{
			get { return Path.Combine(Folder, Sponge.PeopleFolderName); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the folder in which the project's session folders are stored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SessionsFolder
		{
			get { return Path.Combine(Folder, Sponge.SessionFolderName); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of sorted session folders (including their full path) in the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string[] SessionNames
		{
			get
			{
				return (from x in Directory.GetDirectories(SessionsFolder)
						orderby x
						select x).ToArray();
			}
		}

		#endregion

		#region Methods for watching the file system
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles a project file or folder changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFileWatcherEvent(object sender, FileSystemEventArgs e)
		{
			// We don't care when changes occur to our standoff markup files.
			if (e.Name.EndsWith(Sponge.SessionMetaDataFileExtension)
				|| e.Name.EndsWith(Sponge.SessionFileExtension)
				|| e.FullPath.ToLower() == SessionsFolder.ToLower()
				)
				return;

			EnableFileWatching = false;
			VerifySessionsPathExists();
			RefreshSessionList();

			if (ProjectChanged != null)
				ProjectChanged(this, EventArgs.Empty);

			EnableFileWatching = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles a project file or folder being renamed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFileWatcherRename(object sender, RenamedEventArgs e)
		{
			EnableFileWatching = false;
			VerifySessionsPathExists();
			RefreshSessionList();

			if (ProjectChanged != null)
				ProjectChanged(this, EventArgs.Empty);

			EnableFileWatching = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the list of sessions by looking in the file system for all the subfolders
		/// in the project's sessions folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshSessionList()
		{
			var sessionsFound = new HashSet<string>(Directory.GetDirectories(SessionsFolder));

			// Go through the existing sessions we have and remove
			// any that no longer have a sessions folder.
			for (int i = Sessions.Count - 1; i >= 0; i--)
			{
				if (!sessionsFound.Contains(Sessions[i].Folder))
					Sessions.RemoveAt(i);
			}

			// Make sure there is a session for each session folder found. For session
			// folders that do not have a corresponding session, a new one is added.
			foreach (string sessionName in sessionsFound)
			{
				var session = Sessions.FirstOrDefault(x => x.Folder == sessionName);
				if (session == null)
					AddSession(Path.GetFileName(sessionName));
			}
		}

		#endregion

		#region Methods for managing sessions
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the a session having the specified name. If a session with the specified name
		/// already exists, then it's returned. Otherwise the new session is returned. The
		/// list of sessions is kept sorted by Id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Session AddSession(string id)
		{
			var session = Sessions.FirstOrDefault(x => x.Id == id);
			if (session == null)
			{
				session = Session.Create(this, id);
				session.Save();
				Sessions.Add(session);
				Sessions.Sort((x, y) => x.Id.CompareTo(y.Id));
			}

			return session;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies the existence of the sessions path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void VerifySessionsPathExists()
		{
			if (!Directory.Exists(SessionsFolder))
			{
				var msg = LocalizationManager.LocalizeString("MissingSessionsPathMsg",
					"The sessions folder for the '{0}' project is missing. A new one will be " +
					"created for you.\n\nThis can happen if you have deleted or renamed the folder " +
					"using your operating system's file manager program.",
					"Message displayed when a project's sessions folder is found to be missing.",
					"Miscellaneous Strings");

				Utils.MsgBox(string.Format(msg, Name));
				Directory.CreateDirectory(SessionsFolder);
			}
		}

		#endregion

		#region Methods for managing the people list
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the first available, unique, unknown name for a person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetUniquePersonName()
		{
			const string fmt = "Unknown Name {0:D2}";
			int i = 1;
			while (true)
			{
				var name = string.Format(fmt, i++);
				var path = Path.Combine(PeopleFolder, name);
				if (!Directory.Exists(path))
					return name;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not a Person object exists for the specified name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Person GetPerson(string fullName)
		{
			if (fullName != null)
				fullName = fullName.Trim();

			return People.FirstOrDefault(x => x.FullName == fullName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified person to the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddPerson(Person person)
		{
			if (person != null)
			{
				if (person.FullName == null)
				{
					person.FullName = string.Empty;
				}
				person.Project = this;
				People.Add(person);
				People.Sort((x, y) => x.FullName.CompareTo(y.FullName));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the person from the project's internal list of people and removes its
		/// associated disk file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void DeletePerson(string toEuthanize)
		{
			if (toEuthanize == null)
				return;

			var person = People.FirstOrDefault(x => x.FullName == toEuthanize);
			if (person != null)
			{
				Directory.Delete(person.Folder, true);
				People.Remove(person);
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}

		public IEnumerable<string> GetPeopleNames()
		{
			return  from p in People select p.FullName;
		}
	}
}
