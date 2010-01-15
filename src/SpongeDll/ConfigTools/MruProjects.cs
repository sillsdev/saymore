using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SIL.Sponge.ConfigTools
{
	[Serializable]
	[XmlRoot("RecentlyUsedFiles")]
	public class MruProjects
	{
		public static MruProjects CreateOne
		{
			get { return new MruProjects(); }
		}

		private readonly List<string> m_paths;

		public MruProjects()
		{
			m_paths = new List<string>();
		}

		[XmlElement("Path")]
		public string[] Paths
		{
			get
			{
				List<string> paths = new List<string>(GetNonStalePaths());
				return paths.ToArray();
			}
			set
			{
				m_paths.Clear();
				if (value != null)
				{
					foreach (string path in value)
					{
						if (!m_paths.Contains(path))
							m_paths.Add(path);
					}
				}
			}
		}

		[XmlIgnore]
		public string Latest
		{
			get
			{
				foreach (string path in GetNonStalePaths())
					return path;

				return null;
			}
		}

		private IEnumerable<string> GetNonStalePaths()
		{
			foreach (string path in m_paths)
			{
				if (File.Exists(path))
					yield return path;
			}
		}

		/// <summary>
		/// Adds path to top of list of most recently used files if it exists (returns false if it doesn't exist)
		/// </summary>
		/// <param name="path"></param>
		/// <returns>true if successful, false if given file does not exist</returns>
		public bool AddNewPath(string path)
		{
			if (path == null)
				throw new ArgumentNullException("path");

			if (!File.Exists(path))
				return false;

			m_paths.Remove(path);
			m_paths.Insert(0, path);
			return true;
		}
	}
}