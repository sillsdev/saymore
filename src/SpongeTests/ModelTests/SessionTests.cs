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
	public class SessionTests
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
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddFiles method when the session's folder is missing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFiles_WhenSessionFolderMissing()
		{
			var session = Session.Create(m_prj, "wallace");
			Directory.Delete(session.SessionPath, true);
			Assert.IsFalse(session.AddFiles(new[] { string.Empty }));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddFiles method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFiles()
		{
			var session = Session.Create(m_prj, "gromit");

			var file1 = Path.Combine(m_prj.ProjectPath, "wrong.junk");
			var file2 = Path.Combine(m_prj.ProjectPath, "trousers.junk");
			File.CreateText(file1).Close();
			File.CreateText(file2).Close();

			Assert.IsTrue(session.AddFiles(new[] { file1, file2 }));
			Assert.IsTrue(File.Exists(Path.Combine(session.SessionPath, file1)));
			Assert.IsTrue(File.Exists(Path.Combine(session.SessionPath, file2)));
		}
	}
}
