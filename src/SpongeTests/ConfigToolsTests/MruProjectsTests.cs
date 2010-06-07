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
		private StringCollection _prjFiles;
		private string _settingsFilePath;
		private List<string> _paths;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs before each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_settingsFilePath = Path.Combine(Path.GetTempPath(), "~mrutestsettingsfile~.settings");
			SpongeSettingsProvider.SettingsFileFolder = _settingsFilePath;

			MruProjects.Paths = new string[] { };

			// Use a string collection because that's what the settings provider uses.
			_prjFiles = new StringCollection();
			var prjFile = Path.Combine(Path.GetTempPath(), "~beans");
			Directory.CreateDirectory(prjFile);
			prjFile = Path.Combine(prjFile, "~beans.sprj");
			_prjFiles.Add(prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~carrots");
			Directory.CreateDirectory(prjFile);
			prjFile = Path.Combine(prjFile, "~carrots.sprj");
			_prjFiles.Add(prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~turnips");
			Directory.CreateDirectory(prjFile);
			prjFile = Path.Combine(prjFile, "~turnips.sprj");
			_prjFiles.Add(prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~spinich");
			Directory.CreateDirectory(prjFile);
			prjFile = Path.Combine(prjFile, "~spinich.sprj");
			_prjFiles.Add(prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~peas");
			Directory.CreateDirectory(prjFile);
			prjFile = Path.Combine(prjFile, "~peas.sprj");
			_prjFiles.Add(prjFile);

			// Create dummy project files.
			foreach (string file in _prjFiles)
				File.CreateText(file).Close();

			// Get the private static variable that stores the paths.
			_paths = ReflectionHelper.GetField(typeof(MruProjects), "s_paths") as List<string>;
			Assert.IsNotNull(_paths);
			Assert.AreEqual(0, _paths.Count);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs after each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TestTearDown()
		{
			foreach (string file in _prjFiles)
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
				File.Delete(_settingsFilePath);
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
			Assert.IsNotNull(_paths);
			Assert.AreEqual(0, _paths.Count);

			foreach (string file in _prjFiles)
				_paths.Add(file);

			var prjFile = Path.Combine(Path.GetTempPath(), "~frog");
			prjFile = Path.Combine(prjFile, "~frog.sprj");
			_paths.Insert(2, prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~lizard");
			prjFile = Path.Combine(prjFile, "~lizard.sprj");
			_paths.Insert(2, prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~toad");
			prjFile = Path.Combine(prjFile, "~toad.sprj");
			_paths.Insert(2, prjFile);

			Assert.AreEqual(_prjFiles.Count + 3, _paths.Count);
			ReflectionHelper.CallMethod(typeof(MruProjects), "RemoveStalePaths", null);
			Assert.AreEqual(_prjFiles.Count, _paths.Count);

			foreach (string file in _prjFiles)
				Assert.IsTrue(_paths.Contains(file));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddNewPath method when a null path is passed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
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

			Assert.IsTrue(MruProjects.AddNewPath(_prjFiles[0]));
			Assert.AreEqual(_prjFiles[0], _paths[0]);
			Assert.AreEqual(1, _paths.Count);

			Assert.IsTrue(MruProjects.AddNewPath(_prjFiles[1]));
			Assert.AreEqual(_prjFiles[1], _paths[0]);
			Assert.AreEqual(2, _paths.Count);

			Assert.IsTrue(MruProjects.AddNewPath(_prjFiles[2]));
			Assert.AreEqual(_prjFiles[2], _paths[0]);
			Assert.AreEqual(3, _paths.Count);

			// Readd a path that already exists.
			Assert.IsTrue(MruProjects.AddNewPath(_prjFiles[1]));
			Assert.AreEqual(_prjFiles[1], _paths[0]);
			Assert.AreEqual(3, _paths.Count);

			_paths.Clear();
			for (int i = 0; i < _prjFiles.Count; i++)
			{
				Assert.IsTrue(MruProjects.AddNewPath(_prjFiles[i]));
				Assert.IsTrue(_paths.Count <= MruProjects.MaxMRUListSize);

				if (i < MruProjects.MaxMRUListSize)
				{
					Assert.AreEqual(i + 1, _paths.Count);
					Assert.AreEqual(_prjFiles[i], _paths[0]);
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

			Assert.IsTrue(MruProjects.AddNewPath(_prjFiles[0]));
			Assert.AreEqual(_prjFiles[0], MruProjects.Latest);

			Assert.IsTrue(MruProjects.AddNewPath(_prjFiles[1]));
			Assert.AreEqual(_prjFiles[1], MruProjects.Latest);

			Assert.IsTrue(MruProjects.AddNewPath(_prjFiles[2]));
			Assert.AreEqual(_prjFiles[2], MruProjects.Latest);
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
			Assert.AreEqual(0, _paths.Count);

			// Load some non existant paths into our collection of paths.

			var prjFile = Path.Combine(Path.GetTempPath(), "~frog");
			prjFile = Path.Combine(prjFile, "~frog.sprj");
			_prjFiles.Insert(0, prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~lizard");
			prjFile = Path.Combine(prjFile, "~lizard.sprj");
			_prjFiles.Insert(0, prjFile);

			prjFile = Path.Combine(Path.GetTempPath(), "~toad");
			prjFile = Path.Combine(prjFile, "~toad.sprj");
			_prjFiles.Insert(0, prjFile);

			Assert.IsTrue(_prjFiles.Count > MruProjects.MaxMRUListSize);
			ReflectionHelper.CallMethod(typeof(MruProjects), "LoadList", _prjFiles);
			Assert.AreEqual(MruProjects.MaxMRUListSize, _paths.Count);

			// Make sure only the first MaxMRUListSize valid paths in our collection were loaded.
			for (int i = 0; i < MruProjects.MaxMRUListSize; i++)
				Assert.AreEqual(_prjFiles[i + 3], _paths[i]);
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
			for (int i = 1; i < _prjFiles.Count; i++)
				files.Add(_prjFiles[0]);

			// Load the list in which all paths are the same.
			ReflectionHelper.CallMethod(typeof(MruProjects), "LoadList", files);
			Assert.AreEqual(1, _paths.Count);
			Assert.AreEqual(files[0], _paths[0]);
		}
	}
}
