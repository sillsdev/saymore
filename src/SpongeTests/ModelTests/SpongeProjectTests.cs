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
// File: SpongeProjectTests.cs
// Responsibility: Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.IO;
using NUnit.Framework;
using SilUtils;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SpongeProjectTests
	{
		private const string kTestPrjName = "~~Moldy Sponge";
		private const string kTestPrjFileName = "~~MoldySponge.sprj";
		private const string kTestSessionName = "~~Fungus";

		private SpongeProject m_prj;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs before each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_prj = ReflectionHelper.GetResult(typeof(SpongeProject),
				"Create", kTestPrjName) as SpongeProject;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs after each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TestTearDown()
		{
			try
			{
				Directory.Delete(m_prj.ProjectPath, true);
			}
			catch { }

			Assert.IsFalse(Directory.Exists(m_prj.ProjectPath));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the private Create method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Create()
		{
			VerifyProject(m_prj, kTestPrjName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Load method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Load()
		{
			VerifyProject(m_prj, kTestPrjName);

			m_prj = SpongeProject.Load(m_prj.FullProjectPath);
			VerifyProject(m_prj, kTestPrjName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies the existence of the specified project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void VerifyProject(SpongeProject prj, string expectedPrjName)
		{
			Assert.IsNotNull(prj);
			var expectedPath = Path.Combine(SpongeProject.ProjectsFolder, expectedPrjName);
			Assert.AreEqual(expectedPath, prj.ProjectPath);
			Assert.IsTrue(Directory.Exists(prj.ProjectPath));

			expectedPath = Path.Combine(prj.ProjectPath, "Sessions");
			Assert.AreEqual(expectedPath, prj.SessionsPath);
			Assert.IsTrue(Directory.Exists(prj.SessionsPath));

			expectedPath = Path.Combine(prj.ProjectPath, kTestPrjFileName);
			Assert.AreEqual(expectedPath, prj.FullProjectPath);
			Assert.IsTrue(File.Exists(prj.FullProjectPath));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddSession method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddSession()
		{
			Assert.AreEqual(0, m_prj.Sessions.Count);
			Assert.IsTrue(m_prj.AddSession(kTestSessionName));
			Assert.AreEqual(1, m_prj.Sessions.Count);
			Assert.AreEqual(kTestSessionName, m_prj.Sessions[0]);

			// Now add a session that already exists.
			Assert.IsFalse(m_prj.AddSession(kTestSessionName));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetSessionFolder method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSessionFolder()
		{
			Assert.IsNull(m_prj.GetSessionFolder(null));
			Assert.IsNull(m_prj.GetSessionFolder(string.Empty));
			Assert.IsNull(m_prj.GetSessionFolder(kTestSessionName));

			m_prj.AddSession(kTestSessionName);
			var path = Path.Combine(m_prj.SessionsPath, kTestSessionName);
			Assert.AreEqual(path, m_prj.GetSessionFolder(kTestSessionName));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetSessionFiles method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSessionFiles()
		{
			Assert.IsNull(m_prj.GetSessionFiles(null));
			Assert.IsNull(m_prj.GetSessionFiles(string.Empty));

			m_prj.AddSession(kTestSessionName);
			Assert.AreEqual(0, m_prj.GetSessionFiles(kTestSessionName).Length);

			// Create a few files in the session folder. Must use CreateText so we can
			// close the files because Create leaves them locked too long.
			var path = m_prj.GetSessionFolder(kTestSessionName);
			File.CreateText(Path.Combine(path, "yak.pdf")).Close();
			File.CreateText(Path.Combine(path, "hippo.wav")).Close();
			File.CreateText(Path.Combine(path, "meerkat.wma")).Close();

			// Get session's files and make sure they're returned sorted.
			var files = m_prj.GetSessionFiles(kTestSessionName);
			Assert.AreEqual(3, files.Length);
			Assert.AreEqual(files[0], Path.Combine(path, "hippo.wav"));
			Assert.AreEqual(files[1], Path.Combine(path, "meerkat.wma"));
			Assert.AreEqual(files[2], Path.Combine(path, "yak.pdf"));
		}
	}
}
