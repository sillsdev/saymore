using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SIL.Sponge.Properties;

namespace SIL.Sponge.ConfigTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a class to manage the list of most recently used project paths.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public static class MruProjects
	{
		public const int MaxMRUListSize = 4;

		private static readonly List<string> s_paths = new List<string>(/*MaxMRUListSize*/);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="MruProjects"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Initialize(ICollection collection)
		{
			if (collection != null)
				LoadList(collection);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of project paths.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string[] Paths
		{
			get
			{
				RemoveStalePaths();
				return s_paths.ToArray();
			}
			set { LoadList(value); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of paths from the specified collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void LoadList(ICollection values)
		{
			s_paths.Clear();
			if (values == null)
				return;

			int i = 0;
			foreach (object val in values)
			{
				if (i++ == MaxMRUListSize)
					break;

				string path = val as string;
				if (path != null && !s_paths.Contains(path))
					s_paths.Add(path);
			}

			RemoveStalePaths();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the path to the most recently used (i.e. opened) project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string Latest
		{
			get { return (s_paths.Count == 0 ? null : s_paths[s_paths.Count - 1]); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes non existant paths from the MRU list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void RemoveStalePaths()
		{
			if (s_paths.Count > 0)
			{
				for (int i = s_paths.Count - 1; i >= 0; i--)
				{
					if (!Directory.Exists(s_paths[i]))
						s_paths.RemoveAt(i);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds path to top of list of most recently used files if it exists (returns false
		/// if it doesn't exist)
		/// </summary>
		/// <returns>true if successful, false if given file does not exist</returns>
		/// ------------------------------------------------------------------------------------
		public static bool AddNewPath(string path)
		{
			if (path == null)
				throw new ArgumentNullException("path");

			if (!Directory.Exists(path))
				return false;

			// Remove the path from the list if it exists already.
			s_paths.Remove(path);

			// Make sure inserting a new path at the beginning will not exceed our max.
			if (s_paths.Count >= MaxMRUListSize)
				s_paths.RemoveAt(s_paths.Count - 1);

			s_paths.Insert(0, path);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the MRU list to the settings file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Save()
		{
			var collection = new System.Collections.Specialized.StringCollection();
			foreach (string path in s_paths)
				collection.Add(path);

			Settings.Default.MRUList = (collection.Count == 0 ? null : collection);
			Settings.Default.Save();
		}
	}
}