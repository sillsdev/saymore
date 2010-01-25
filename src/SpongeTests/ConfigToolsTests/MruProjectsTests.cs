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
		private StringCollection m_prjFiles;
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
			m_prjFiles = new StringCollection();
			var prjFile = Path.Combine(Path.GetTempPath(), "~beans");
			Directory.CreateDirectory(prjFile);
			prjFile = Path.Combine(prjFile, "~beans.sprj");
			m_prjFiles.Add(prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~carrots");
			Directory.CreateDirectory(prjFile);
			prjFile = Path.Combine(prjFile, "~carrots.sprj");
			m_prjFiles.Add(prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~turnips");
			Directory.CreateDirectory(prjFile);
			prjFile = Path.Combine(prjFile, "~turnips.sprj");
			m_prjFiles.Add(prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~spinich");
			Directory.CreateDirectory(prjFile);
			prjFile = Path.Combine(prjFile, "~spinich.sprj");
			m_prjFiles.Add(prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~peas");
			Directory.CreateDirectory(prjFile);
			prjFile = Path.Combine(prjFile, "~peas.sprj");
			m_prjFiles.Add(prjFile);

			// Create dummy project files.
			foreach (string file in m_prjFiles)
				File.CreateText(file).Close();

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
			foreach (string file in m_prjFiles)
			{
				try
				{
					var folder = Path.GetDirectoryName(file);
					Directory.Delete(folder, true);
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

			foreach (string file in m_prjFiles)
				m_paths.Add(file);

			var prjFile = Path.Combine(Path.GetTempPath(), "~frog");
			prjFile = Path.Combine(prjFile, "~frog.sprj");
			m_paths.Insert(2, prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~lizard");
			prjFile = Path.Combine(prjFile, "~lizard.sprj");
			m_paths.Insert(2, prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~toad");
			prjFile = Path.Combine(prjFile, "~toad.sprj");
			m_paths.Insert(2, prjFile);

			Assert.AreEqual(m_prjFiles.Count + 3, m_paths.Count);
			ReflectionHelper.CallMethod(typeof(MruProjects), "RemoveStalePaths", null);
			Assert.AreEqual(m_prjFiles.Count, m_paths.Count);

			foreach (string file in m_prjFiles)
				Assert.IsTrue(m_paths.Contains(file));
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

			Assert.IsTrue(MruProjects.AddNewPath(m_prjFiles[0]));
			Assert.AreEqual(m_prjFiles[0], m_paths[0]);
			Assert.AreEqual(1, m_paths.Count);

			Assert.IsTrue(MruProjects.AddNewPath(m_prjFiles[1]));
			Assert.AreEqual(m_prjFiles[1], m_paths[0]);
			Assert.AreEqual(2, m_paths.Count);

			Assert.IsTrue(MruProjects.AddNewPath(m_prjFiles[2]));
			Assert.AreEqual(m_prjFiles[2], m_paths[0]);
			Assert.AreEqual(3, m_paths.Count);

			// Readd a path that already exists.
			Assert.IsTrue(MruProjects.AddNewPath(m_prjFiles[1]));
			Assert.AreEqual(m_prjFiles[1], m_paths[0]);
			Assert.AreEqual(3, m_paths.Count);

			m_paths.Clear();
			for (int i = 0; i < m_prjFiles.Count; i++)
			{
				Assert.IsTrue(MruProjects.AddNewPath(m_prjFiles[i]));
				Assert.IsTrue(m_paths.Count <= MruProjects.MaxMRUListSize);

				if (i < MruProjects.MaxMRUListSize)
				{
					Assert.AreEqual(i + 1, m_paths.Count);
					Assert.AreEqual(m_prjFiles[i], m_paths[0]);
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

			Assert.IsTrue(MruProjects.AddNewPath(m_prjFiles[0]));
			Assert.AreEqual(m_prjFiles[0], MruProjects.Latest);

			Assert.IsTrue(MruProjects.AddNewPath(m_prjFiles[1]));
			Assert.AreEqual(m_prjFiles[1], MruProjects.Latest);

			Assert.IsTrue(MruProjects.AddNewPath(m_prjFiles[2]));
			Assert.AreEqual(m_prjFiles[2], MruProjects.Latest);
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

			var prjFile = Path.Combine(Path.GetTempPath(), "~frog");
			prjFile = Path.Combine(prjFile, "~frog.sprj");
			m_prjFiles.Insert(0, prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~lizard");
			prjFile = Path.Combine(prjFile, "~lizard.sprj");
			m_prjFiles.Insert(0, prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~toad");
			prjFile = Path.Combine(prjFile, "~toad.sprj");
			m_prjFiles.Insert(0, prjFile);

			Assert.IsTrue(m_prjFiles.Count > MruProjects.MaxMRUListSize);
			ReflectionHelper.CallMethod(typeof(MruProjects), "LoadList", m_prjFiles);
			Assert.AreEqual(MruProjects.MaxMRUListSize, m_paths.Count);

			// Make sure only the first MaxMRUListSize valid paths in our collection were loaded.
			for (int i = 0; i < MruProjects.MaxMRUListSize; i++)
				Assert.AreEqual(m_prjFiles[i + 3], m_paths[i]);
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
			StringCollection files = new StringCollection();
			for (int i = 1; i < m_prjFiles.Count; i++)
				files.Add(m_prjFiles[0]);

			// Load the list in which all paths are the same.
			ReflectionHelper.CallMethod(typeof(MruProjects), "LoadList", files);
			Assert.AreEqual(1, m_paths.Count);
			Assert.AreEqual(files[0], m_paths[0]);
		}
	}
}
