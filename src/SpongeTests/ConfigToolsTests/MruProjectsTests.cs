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
// File: MruProjectsTests.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using NUnit.Framework;
using SilUtils;

namespace SIL.Sponge.ConfigTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class MruProjectsTests
	{
		private StringCollection m_prjFolders;
		private string m_settingsFilePath;
		private List<string> m_paths;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs before each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_settingsFilePath = Path.Combine(Path.GetTempPath(), "~mrutestsettingsfile~.settings");
			PortableSettingsProvider.SettingsFilePath = m_settingsFilePath;

			MruProjects.Paths = new string[] { };

			// Use a string collection because that's what the settings provider uses.
			m_prjFolders = new StringCollection();
			m_prjFolders.Add(Path.Combine(Path.GetTempPath(), "~beans"));
			m_prjFolders.Add(Path.Combine(Path.GetTempPath(), "~carrots"));
			m_prjFolders.Add(Path.Combine(Path.GetTempPath(), "~turnips"));
			m_prjFolders.Add(Path.Combine(Path.GetTempPath(), "~spinich"));
			m_prjFolders.Add(Path.Combine(Path.GetTempPath(), "~peas"));

			Directory.CreateDirectory(m_prjFolders[0]);
			Directory.CreateDirectory(m_prjFolders[1]);
			Directory.CreateDirectory(m_prjFolders[2]);
			Directory.CreateDirectory(m_prjFolders[3]);
			Directory.CreateDirectory(m_prjFolders[4]);

			// Get the private static variable that stores the paths.
			m_paths = ReflectionHelper.GetField(typeof(MruProjects), "s_paths") as List<string>;
			Assert.IsNotNull(m_paths);
			Assert.AreEqual(0, m_paths.Count);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs after each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TestTearDown()
		{
			foreach (string folder in m_prjFolders)
			{
				try
				{
					Directory.Delete(Path.Combine(Path.GetTempPath(), folder));
				}
				catch { }
			}

			try
			{
				File.Delete(m_settingsFilePath);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the RemoveStalePaths method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveStalePaths()
		{
			Assert.IsNotNull(m_paths);
			Assert.AreEqual(0, m_paths.Count);

			foreach (string folder in m_prjFolders)
				m_paths.Add(folder);

			m_paths.Insert(2, Path.Combine(Path.GetTempPath(), "~frog"));
			m_paths.Insert(4, Path.Combine(Path.GetTempPath(), "~lizard"));
			m_paths.Insert(0, Path.Combine(Path.GetTempPath(), "~toad"));
			Assert.AreEqual(m_prjFolders.Count + 3, m_paths.Count);

			ReflectionHelper.CallMethod(typeof(MruProjects), "RemoveStalePaths", null);
			Assert.AreEqual(m_prjFolders.Count, m_paths.Count);

			foreach (string folder in m_prjFolders)
				Assert.IsTrue(m_paths.Contains(folder));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddNewPath method when a null path is passed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddNewNullPath()
		{
			MruProjects.AddNewPath(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddNewPath method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddNewPath()
		{
			// Test a path that does not exists.
			Assert.IsFalse(MruProjects.AddNewPath(Path.Combine(Path.GetTempPath(), "@#$%badpath")));

			Assert.IsTrue(MruProjects.AddNewPath(m_prjFolders[0]));
			Assert.AreEqual(m_prjFolders[0], m_paths[0]);
			Assert.AreEqual(1, m_paths.Count);

			Assert.IsTrue(MruProjects.AddNewPath(m_prjFolders[1]));
			Assert.AreEqual(m_prjFolders[1], m_paths[0]);
			Assert.AreEqual(2, m_paths.Count);

			Assert.IsTrue(MruProjects.AddNewPath(m_prjFolders[2]));
			Assert.AreEqual(m_prjFolders[2], m_paths[0]);
			Assert.AreEqual(3, m_paths.Count);

			// Readd a path that already exists.
			Assert.IsTrue(MruProjects.AddNewPath(m_prjFolders[1]));
			Assert.AreEqual(m_prjFolders[1], m_paths[0]);
			Assert.AreEqual(3, m_paths.Count);

			m_paths.Clear();
			for (int i = 0; i < m_prjFolders.Count; i++)
			{
				Assert.IsTrue(MruProjects.AddNewPath(m_prjFolders[i]));
				Assert.IsTrue(m_paths.Count <= MruProjects.MaxMRUListSize);

				if (i < MruProjects.MaxMRUListSize)
				{
					Assert.AreEqual(i + 1, m_paths.Count);
					Assert.AreEqual(m_prjFolders[i], m_paths[0]);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Latest property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Latest()
		{
			Assert.IsNull(MruProjects.Latest);

			Assert.IsTrue(MruProjects.AddNewPath(m_prjFolders[0]));
			Assert.AreEqual(m_prjFolders[0], MruProjects.Latest);

			Assert.IsTrue(MruProjects.AddNewPath(m_prjFolders[1]));
			Assert.AreEqual(m_prjFolders[1], MruProjects.Latest);

			Assert.IsTrue(MruProjects.AddNewPath(m_prjFolders[2]));
			Assert.AreEqual(m_prjFolders[2], MruProjects.Latest);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the LoadList method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadList()
		{
			ReflectionHelper.CallMethod(typeof(MruProjects), "LoadList", new object[] { null });
			Assert.AreEqual(0, m_paths.Count);

			// Load some non existant paths into our collection of paths.
			m_prjFolders.Insert(0, Path.Combine(Path.GetTempPath(), "~frog"));
			m_prjFolders.Insert(0, Path.Combine(Path.GetTempPath(), "~lizard"));
			m_prjFolders.Insert(0, Path.Combine(Path.GetTempPath(), "~toad"));
			Assert.IsTrue(m_prjFolders.Count > MruProjects.MaxMRUListSize);

			ReflectionHelper.CallMethod(typeof(MruProjects), "LoadList", m_prjFolders);
			Assert.AreEqual(MruProjects.MaxMRUListSize, m_paths.Count);

			// Make sure only the first MaxMRUListSize valid paths in our collection were loaded.
			for (int i = 0; i < MruProjects.MaxMRUListSize; i++)
				Assert.AreEqual(m_prjFolders[i + 3], m_paths[i]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the LoadList method when sending a list in which all the paths are the same.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadListOfSamePaths()
		{
			// Now make every path in our collection the same and verify that
			// loading that list will result in only one path getting loaded.
			for (int i = 1; i < m_prjFolders.Count; i++)
				m_prjFolders[i] = m_prjFolders[0];

			// Load the list in which all paths are the same.
			ReflectionHelper.CallMethod(typeof(MruProjects), "LoadList", m_prjFolders);
			Assert.AreEqual(1, m_paths.Count);
			Assert.AreEqual(m_prjFolders[0], m_paths[0]);
		}
	}
}
