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
// File: WelcomeDialogTests.cs
// Responsibility: Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.IO;
using NUnit.Framework;
using Palaso.TestUtilities;
using SIL.Sponge.ConfigTools;
using SilUtils;

namespace SpongeTests
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class WelcomeDialogTests
	{
		private WelcomeDialogViewManager _viewManager;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_viewManager = new WelcomeDialogViewManager();
			Utils.SuppressMsgBoxInteractions = true;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateNewProject_NonExistentFolder_Created()
		{
			using (var tmpFolder = new TemporaryFolder("basketball"))
			{
				var prjFolder = Path.Combine(tmpFolder.FolderPath, "sonics");
				Assert.IsTrue(_viewManager.CreateNewProject(prjFolder));
				Assert.IsTrue(Directory.Exists(prjFolder));
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateNewProject_NonExistentFolder_ProjectFilePath_IsCorrect()
		{
			using (var tmpFolder = new TemporaryFolder("basketball"))
			{
				Assert.IsTrue(_viewManager.CreateNewProject(tmpFolder.FolderPath));
				Assert.AreEqual(Path.Combine(tmpFolder.FolderPath, "basketball.sprj"),
					_viewManager.ProjectPath);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateNewProject_ExistingEmptyFolder_True()
		{
			using (var tmpFolder = new TemporaryFolder("tennis1"))
				Assert.IsTrue(_viewManager.CreateNewProject(tmpFolder.FolderPath));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateNewProject_ExistingEmptyFolder_ProjectPath_IsCorrect()
		{
			using (var tmpFolder = new TemporaryFolder("tennis3"))
			{
				Assert.IsTrue(_viewManager.CreateNewProject(tmpFolder.FolderPath));
				Assert.AreEqual(Path.Combine(tmpFolder.FolderPath, "tennis3.sprj"),
					_viewManager.ProjectPath);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateNewProject_FolderAlreadyContainsProject_False()
		{
			using (var tmpFolder = new TemporaryFolder("baseball"))
			{
				var tmpFile = Path.Combine(tmpFolder.FolderPath, "baseball.sprj");
				File.CreateText(tmpFile).Close();
				Assert.IsFalse(_viewManager.CreateNewProject(tmpFolder.FolderPath));
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateNewProject_FolderAlreadyContainsProject_ProjectPath_Null()
		{
			using (var tmpFolder = new TemporaryFolder("baseball"))
			{
				var tmpFile = Path.Combine(tmpFolder.FolderPath, "baseball.sprj");
				File.CreateText(tmpFile).Close();
				Assert.IsFalse(_viewManager.CreateNewProject(tmpFolder.FolderPath));
				Assert.IsNull(_viewManager.ProjectPath);
			}
		}
	}
}
