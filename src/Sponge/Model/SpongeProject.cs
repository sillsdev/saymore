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
using SIL.Localize.LocalizationUtils;
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
		private FileSystemWatcher m_fileWatcher;

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
			using (var dlg = new NewProjectDlg())
			{
				return (dlg.ShowDialog(parent) == DialogResult.OK ?
					Create(dlg.NewProjectName) : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a Sponge project with the specified name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static SpongeProject Create(string prjName)
		{
			var prj = new SpongeProject();
			prj.Initialize(prjName, null);
			return prj;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the parent folder for all project folders.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ProjectsFolder
		{
			get { return Path.Combine(Sponge.MainAppSettingsFolder, "Projects"); }
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
			ProjectPath = Path.Combine(ProjectsFolder, prjName);
			FileName = (prjFileName ?? prjName.Replace(" ", string.Empty) + ".sprj");

			if (!Directory.Exists(ProjectPath))
				Directory.CreateDirectory(ProjectPath);

			Session.InitializeSessionFolder(ProjectPath);
			Person.InitializePeopleFolder(ProjectPath);

			Save();

			Sessions = (from folder in Session.Sessions
						orderby folder
						select Session.Create(this, Path.GetFileName(folder))).ToList();

			People = (from file in Person.PeopleFiles
					  orderby file
					  select Person.CreateFromFile(file)).ToList();

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

			m_fileWatcher = new FileSystemWatcher(ProjectPath);
			m_fileWatcher.Renamed += HandleFileWatcherRename;
			m_fileWatcher.Deleted += HandleFileWatcherEvent;
			m_fileWatcher.Changed += HandleFileWatcherEvent;
			m_fileWatcher.Created += HandleFileWatcherEvent;
			m_fileWatcher.IncludeSubdirectories = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or
		/// resetting unmanaged resources.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			m_fileWatcher.Dispose();
			m_fileWatcher = null;
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
			XmlSerializationHelper.SerializeToFile(FullPath, this);
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
				return (m_fileWatcher.SynchronizingObject == null ?
					false : m_fileWatcher.EnableRaisingEvents);
			}
			set
			{
				if (m_fileWatcher.SynchronizingObject != null)
					m_fileWatcher.EnableRaisingEvents = value;
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
			get { return m_fileWatcher.SynchronizingObject as Control; }
			set
			{
				m_fileWatcher.SynchronizingObject = value;
				m_fileWatcher.EnableRaisingEvents = (value != null);
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
		/// Gets the project's path (not including the project file name).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectPath { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's file name (not including its path).
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
		public string FullPath
		{
			get { return Path.Combine(ProjectPath, FileName); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of sorted session names.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string[] SessionNames
		{
			get
			{
				return (from session in Sessions
						orderby session.Name
						select session.Name).ToArray();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies the existence of the sessions path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void VerifySessionsPathExists()
		{
			if (!Directory.Exists(Session.SessionsPath))
			{
				var msg = LocalizationManager.LocalizeString("MissingSessionsPathMsg",
					"The sessions folder for the '{0}' project is missing. A new one will be " +
					"created for you.\n\nThis can happen if you have deleted or renamed the folder " +
					"using your operating system's file manager program.",
					"Message displayed when a project's sessions folder is found to be missing.",
					"Miscellaneous Strings");

				Utils.MsgBox(string.Format(msg, Name));
				Directory.CreateDirectory(Session.SessionsPath);
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
			if (e.Name.EndsWith(SessionFile.SessionFileExtension))
				return;

			EnableFileWatching = false;
			VerifySessionsPathExists();
			UpdateSessions();

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
			UpdateSessions();

			if (ProjectChanged != null)
				ProjectChanged(this, EventArgs.Empty);

			EnableFileWatching = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the list of sessions after the file system watcher is notified of a
		/// change in the file system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateSessions()
		{
			var sessionsFound = new HashSet<string>(Directory.GetDirectories(Session.SessionsPath));

			// Go through the existing sessions we have and remove
			// any that no longer have a sessions folder.
			for (int i = Sessions.Count - 1; i >= 0; i--)
			{
				if (!sessionsFound.Contains(Sessions[i].SessionPath))
					Sessions.RemoveAt(i);
			}

			// Make sure there is a session for each session folder found. For session
			// folders that do not have a corresponding session, a new one is added.
			foreach (string sessionName in sessionsFound)
			{
				var session = Sessions.FirstOrDefault(x => x.SessionPath == sessionName);
				if (session == null)
					AddSession(Path.GetFileName(sessionName));
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the a session having the specified name. If a session with the specified name
		/// already exists, then it's returned. Otherwise the new session is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Session AddSession(string sessionName)
		{
			var session = Sessions.FirstOrDefault(x => x.Name == sessionName);
			if (session == null)
			{
				session = Session.Create(this, sessionName);
				Sessions.Add(session);
				Sessions.Sort((x, y) => x.Name.CompareTo(y.Name));
			}

			return session;
		}

		#region Methods for managing the people list
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not a Person object exists for the specified name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool PersonExists(string fullName)
		{
			if (fullName == null)
				return false;

			fullName = fullName.Trim();
			return (People.FirstOrDefault(x => x.FullName == fullName) != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified person to the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AddPerson(Person person, bool saveToFile)
		{
			if (person == null)
				return false;

			if (PersonExists(person.FullName))
			{
				var msg = LocalizationManager.LocalizeString("AddingDuplicatePersonMsg",
					"The person '{0}' already exists and you may not add another person with the same name.",
					"Miscellaneous Strings");

				Utils.MsgBox(string.Format(msg, person.FullName));
				return false;
			}

			if (saveToFile && person.CanSave)
				person.Save();

			People.Add(person);
			People.Sort((x, y) => x.FullName.CompareTo(y.FullName));
			return true;
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
				File.Delete(Path.Combine(Person.PeoplesPath, person.FileName));
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
	}
}
