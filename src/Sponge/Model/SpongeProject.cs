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
			ProjectName = prjName;
			ProjectPath = Path.Combine(ProjectsFolder, prjName);
			ProjectFileName = (prjFileName ?? prjName.Replace(" ", string.Empty) + ".sprj");

			if (!Directory.Exists(ProjectPath))
				Directory.CreateDirectory(ProjectPath);

			if (!Directory.Exists(SessionsPath))
				Directory.CreateDirectory(SessionsPath);

			Save();

			Sessions = (from folders in Directory.GetDirectories(SessionsPath)
						orderby folders
						select Session.Create(this, Path.GetFileName(folders))).ToList();

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
		/// Saves the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			XmlSerializationHelper.SerializeToFile(FullProjectPath, this);
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
		/// Gets the sessions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<Session> Sessions { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectName { get; private set; }

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
		public string ProjectFileName { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's path and file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FullProjectPath
		{
			get { return Path.Combine(ProjectPath, ProjectFileName); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the folder in which all the project's sessions are saved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string SessionsPath
		{
			get { return Path.Combine(ProjectPath, "Sessions"); }
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
			if (!Directory.Exists(SessionsPath))
			{
				var msg = LocalizationManager.LocalizeString("MissingSessionsPathMsg",
					"The sessions folder for the '{0}' project is missing. A new one will be " +
					"created for you.\n\nThis can happen if you have deleted or renamed the folder " +
					"using your operating system's file manager program.",
					"Message displayed when a project's sessions folder is found to be missing.",
					"Miscellaneous Strings");

				Utils.MsgBox(string.Format(msg, ProjectName));
				Directory.CreateDirectory(SessionsPath);
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
			var sessionsFound = new HashSet<string>(Directory.GetDirectories(SessionsPath));

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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return ProjectName;
		}
	}
}
