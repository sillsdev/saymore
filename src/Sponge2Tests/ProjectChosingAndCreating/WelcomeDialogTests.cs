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

using SilUtils;
using Sponge2.ProjectChoosingAndCreating;

namespace SpongeTests.ProjectChosingAndCreating
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class WelcomeDialogTests
	{
		private WelcomeDialogViewModel _viewModel;
		/*
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_viewModel = new WelcomeDialogViewModel();
			Utils.SuppressMsgBoxInteractions = true;
			Palaso.Reporting.ErrorReport.IsOkToInteractWithUser = false;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateNewProject_NonExistentFolder_Created()
		{
			using (var tmpFolder = new TemporaryFolder("basketball"))
			{
				var prjFolder = Path.Combine(tmpFolder.Path, "sonics");
				Assert.IsTrue(_viewModel.CreateNewProject(tmpFolder.Path, "sonics"));
				Assert.IsTrue(Directory.Exists(prjFolder));
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ProjectSettingsFilePath_AfterCreating_IsCorrect()
		{
			using (var tmpFolder = new TemporaryFolder("basketball"))
			{
				Assert.IsTrue(_viewModel.CreateNewProject(tmpFolder.Path, "sonics"));
				Assert.AreEqual(tmpFolder.Combine("sonics", "sonics.sprj"),
								_viewModel.ProjectSettingsFilePath);
			}
		}

//	NB: Dave, is is worth it to make it possible to create on top of an existing folder?
// seems like a a lot of work/testing for unclear benefit
//		[Test]
//		public void CreateNewProject_ExistingEmptyFolder_True()
//		{
//			using (var tmpFolder = new TemporaryFolder("tennis1"))
//			{
//				NewProjectDlgViewModel newProjectInfo = tmpFolder.Path;
//				Assert.IsTrue(_viewModel.CreateNewProject(newProjectInfo, newProjectInfo.ParentFolderPathForNewProject));
//			}
//		}

//		/// ------------------------------------------------------------------------------------
//		[Test]
//		public void CreateNewProject_ExistingEmptyFolder_ProjectPath_IsCorrect()
//		{
//			using (var tmpFolder = new TemporaryFolder("tennis3"))
//			{
//				NewProjectDlgViewModel newProjectInfo = tmpFolder.Path;
//				Assert.IsTrue(_viewModel.CreateNewProject(newProjectInfo, newProjectInfo.ParentFolderPathForNewProject));
//				Assert.AreEqual(Path.Combine(tmpFolder.Path, "tennis3.sprj"),
//					_viewModel.ProjectSettingsFilePath);
//			}
//		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateNewProject_FolderAlreadyExists_False()
		{
			using (new Palaso.Reporting.ErrorReport.NonFatalErrorReportExpected())
			{
				using (var parent = new TemporaryFolder("basketball"))
				{
					using (var projectFolder = new TemporaryFolder(parent, "sonics"))
					{
						Assert.IsFalse(_viewModel.CreateNewProject(parent.Path, "sonics"));
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateNewProject_FolderAlreadyContainsProject_ProjectPath_Null()
		{
			using (new Palaso.Reporting.ErrorReport.NonFatalErrorReportExpected())
			using (var parent = new TemporaryFolder("basketball"))
			{
				using (var projectFolder = new TemporaryFolder(parent,"sonics"))
				{
					_viewModel.CreateNewProject(parent.Path, "sonics");
					Assert.IsNull(_viewModel.ProjectSettingsFilePath);
				}
			}
		}
		 */
	}
}