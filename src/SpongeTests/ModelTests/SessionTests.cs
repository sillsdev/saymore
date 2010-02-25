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
// File: SessionTests.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.IO;
using NUnit.Framework;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests for the Session class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SessionTests : TestBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs before each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override void TestSetup()
		{
			base.TestSetup();
			InitProject();
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
