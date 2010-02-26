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
		/// Creates a session having the specified name and for the specified project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Session Create(SpongeProject prj, string name)
		{
			var session = new Session(prj, name);

			if (!Directory.Exists(session.FullPath))
				Directory.CreateDirectory(session.FullPath);

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
			FullPath = Path.Combine(prj.SessionsFolder, Name);
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
		public string FullPath { get; private set; }

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
				if (!Directory.Exists(FullPath))
					return null;

				return (from x in Directory.GetFiles(FullPath, "*.*")
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

			if (!Directory.Exists(FullPath))
				return false;

			bool fileWatchingState = Project.EnableFileWatching;
			Project.EnableFileWatching = false;

			foreach (string filePath in sessionFiles)
			{
				if (File.Exists(filePath) && Path.GetDirectoryName(filePath) != FullPath)
				{
					var file = Path.GetFileName(filePath);

					// TODO: Deal with file when it already exists in session folder.
					File.Copy(filePath, Path.Combine(FullPath, file));
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
