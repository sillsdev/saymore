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
// File: Session.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates information about a single session.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Session
	{
		#region static methods and properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the folder in which sessions are stored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string SessionsPath { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the people folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void InitializeSessionFolder(string prjPath)
		{
			SessionsPath = Path.Combine(prjPath, Sponge.SessionFolderName);

			if (!Directory.Exists(SessionsPath))
				Directory.CreateDirectory(SessionsPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets all the session names for a project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string[] Sessions
		{
			get { return Directory.GetDirectories(SessionsPath); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a session having the specified name and for the specified project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Session Create(SpongeProject prj, string name)
		{
			var session = new Session(prj, name);

			if (!Directory.Exists(session.SessionPath))
				Directory.CreateDirectory(session.SessionPath);

			return session;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Session"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Session(SpongeProject prj, string name)
		{
			Project = prj;
			Name = name;
			SessionPath = Path.Combine(SessionsPath, Name);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the session's name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Name { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the session's owning project
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SpongeProject Project { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the session's folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SessionPath { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full paths to all the session files found in the session's folder.
		/// If the folder for the session does not exist, then null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] Files
		{
			get
			{
				if (!Directory.Exists(SessionPath))
					return null;

				return (from x in Directory.GetFiles(SessionPath, "*.*")
						where !x.EndsWith("." + Sponge.SessionFileExtension)
						orderby x
						select x).ToArray();
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified files to the session folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AddFiles(string[] sessionFiles)
		{
			Debug.Assert(sessionFiles != null);

			if (!Directory.Exists(SessionPath))
				return false;

			bool fileWatchingState = Project.EnableFileWatching;
			Project.EnableFileWatching = false;

			foreach (string filePath in sessionFiles)
			{
				if (File.Exists(filePath) && Path.GetDirectoryName(filePath) != SessionPath)
				{
					var file = Path.GetFileName(filePath);

					// TODO: Deal with file when it already exists in session folder.
					File.Copy(filePath, Path.Combine(SessionPath, file));
				}
			}

			Project.EnableFileWatching = fileWatchingState;
			return true;
		}

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
