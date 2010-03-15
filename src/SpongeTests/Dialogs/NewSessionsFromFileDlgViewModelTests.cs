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
// File: NewSessionsFromFileDlgViewModelTests.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Linq;
using System.IO;
using NUnit.Framework;
using Palaso.TestUtilities;
using SIL.Sponge.Model;
using SilUtils;

namespace SIL.Sponge.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Test for the NewSessionsFromFileDlgViewModel class
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class NewSessionsFromFileDlgViewModelTests : TestBase
	{
		private NewSessionsFromFileDlgViewModel _viewModel;
		private TemporaryFolder _tmpFolder;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public override void TestSetup()
		{
			base.TestSetup();

			SessionFileBase.PreventGettingMediaFileDurationsUsingDirectX = true;
			_viewModel = new NewSessionsFromFileDlgViewModel();
			_tmpFolder = new TemporaryFolder("~~SpongeTestFolder~~");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public override void TestTearDown()
		{
			base.TestTearDown();
			_tmpFolder.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that LoadFilesFromFolder only loads audio or video files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadFilesFromFolder_VerifyFileTypesLoaded()
		{
			File.CreateText(_tmpFolder.Combine("hamster.pdf")).Close();
			File.CreateText(_tmpFolder.Combine("dog.wav")).Close();
			File.CreateText(_tmpFolder.Combine("cat.mpg")).Close();

			ReflectionHelper.CallMethod(_viewModel, "LoadFilesFromFolder", _tmpFolder.FolderPath);

			Assert.AreEqual(2, _viewModel.Files.Count);
			Assert.IsNotNull(_viewModel.Files.First(x => x.FileName == "dog.wav"));
			Assert.IsNotNull(_viewModel.Files.First(x => x.FileName == "cat.mpg"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that LoadFilesFromFolder clears the Files list when sent a null folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadFilesFromFolder_FolderArgumentIsNull()
		{
			var path = _tmpFolder.Combine("bird.doc");
			File.CreateText(path).Close();
			_viewModel.Files.Add(new NewSessionFile(path));
			Assert.AreEqual(1, _viewModel.Files.Count);
			ReflectionHelper.CallMethod(_viewModel, "LoadFilesFromFolder", new object[] { null });
			Assert.AreEqual(0, _viewModel.Files.Count);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that LoadFilesFromFolder clears the Files list when sent a folder whose
		/// name is empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadFilesFromFolder_FolderArgumentIsEmpty()
		{
			var path = _tmpFolder.Combine("bird.doc");
			File.CreateText(path).Close();
			_viewModel.Files.Add(new NewSessionFile(path));
			Assert.AreEqual(1, _viewModel.Files.Count);
			ReflectionHelper.CallMethod(_viewModel, "LoadFilesFromFolder", string.Empty);
			Assert.AreEqual(0, _viewModel.Files.Count);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that LoadFilesFromFolder finishes with a sorted list of files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadFilesFromFolder_VerifyFileListSorted()
		{
			File.CreateText(_tmpFolder.Combine("z.wma")).Close();
			File.CreateText(_tmpFolder.Combine("y.wav")).Close();
			File.CreateText(_tmpFolder.Combine("y.mp3")).Close();

			ReflectionHelper.CallMethod(_viewModel, "LoadFilesFromFolder", _tmpFolder.FolderPath);

			Assert.AreEqual(3, _viewModel.Files.Count);
			Assert.AreEqual("y.mp3", _viewModel.Files[0].FileName);
			Assert.AreEqual("y.wav", _viewModel.Files[1].FileName);
			Assert.AreEqual("z.wma", _viewModel.Files[2].FileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that SelectedFolder property loads files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SelectedFolder()
		{
			File.CreateText(_tmpFolder.Combine("dog.wav")).Close();
			File.CreateText(_tmpFolder.Combine("cat.mpg")).Close();

			_viewModel.SelectedFolder = _tmpFolder.FolderPath;

			Assert.AreEqual(2, _viewModel.Files.Count);
			Assert.AreEqual("cat.mpg", _viewModel.Files[0].FileName);
			Assert.AreEqual("dog.wav", _viewModel.Files[1].FileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the Refresh method updates the files list when a file is added.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Refresh()
		{
			File.CreateText(_tmpFolder.Combine("dog.wav")).Close();
			_viewModel.SelectedFolder = _tmpFolder.FolderPath;
			Assert.AreEqual(1, _viewModel.Files.Count);
			Assert.AreEqual("dog.wav", _viewModel.Files[0].FileName);

			File.CreateText(_tmpFolder.Combine("cat.mpg")).Close();
			_viewModel.Refresh();
			Assert.AreEqual(2, _viewModel.Files.Count);
			Assert.AreEqual("cat.mpg", _viewModel.Files[0].FileName);
			Assert.AreEqual("dog.wav", _viewModel.Files[1].FileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the NumberOfSelectedFiles property indicates that files are selected
		/// by default when they are added.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void NumberOfSelectedFiles_SelectedByDefault()
		{
			Assert.AreEqual(0, _viewModel.NumberOfSelectedFiles);

			File.CreateText(_tmpFolder.Combine("dog.wav")).Close();
			File.CreateText(_tmpFolder.Combine("cat.wav")).Close();
			_viewModel.SelectedFolder = _tmpFolder.FolderPath;
			Assert.AreEqual(2, _viewModel.NumberOfSelectedFiles);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the NumberOfSelectedFiles property after adding two files and
		/// unselecting one, then both.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void NumberOfSelectedFiles_ProperCountAfterUnselect()
		{
			File.CreateText(_tmpFolder.Combine("dog.wav")).Close();
			File.CreateText(_tmpFolder.Combine("cat.wav")).Close();
			_viewModel.SelectedFolder = _tmpFolder.FolderPath;
			Assert.AreEqual(2, _viewModel.NumberOfSelectedFiles);
			_viewModel.Files[1].Selected = false;
			Assert.AreEqual(1, _viewModel.NumberOfSelectedFiles);
			_viewModel.Files[0].Selected = false;
			Assert.AreEqual(0, _viewModel.NumberOfSelectedFiles);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the AnyFilesSelected property after adding two files and
		/// unselecting one, then both.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AnyFilesSelected()
		{
			Assert.IsFalse(_viewModel.AnyFilesSelected);
			File.CreateText(_tmpFolder.Combine("dog.wav")).Close();
			File.CreateText(_tmpFolder.Combine("cat.wav")).Close();
			_viewModel.SelectedFolder = _tmpFolder.FolderPath;
			Assert.IsTrue(_viewModel.AnyFilesSelected);
			_viewModel.Files[1].Selected = false;
			Assert.IsTrue(_viewModel.AnyFilesSelected);
			_viewModel.Files[0].Selected = false;
			Assert.IsFalse(_viewModel.AnyFilesSelected);
			_viewModel.Files[0].Selected = true;
			Assert.IsTrue(_viewModel.AnyFilesSelected);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that passing a null object to the CreateSingleSession method does nothing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleSession_NullFileIsIgnored()
		{
			Assert.IsFalse(ReflectionHelper.GetBoolResult(
				_viewModel, "CreateSingleSession", new object[] { null }));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that passing an unselected file object to the CreateSingleSession method
		/// does nothing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleSession_UnselectedFileIsIgnored()
		{
			var path = _tmpFolder.Combine("dog.wav");
			File.CreateText(path).Close();
			var nsf = new NewSessionFile(path);
			nsf.Selected = false;
			Assert.IsFalse(ReflectionHelper.GetBoolResult(_viewModel, "CreateSingleSession", nsf));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that passing some selected files to the CreateSingleSession method
		/// sets the FirstNewSessionAdded property correclty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleSession_SetsFirstNewSessionAddedProperly()
		{
			InitProject();

			Assert.IsNull(_viewModel.FirstNewSessionAdded);

			var path = _tmpFolder.Combine("lizard.mov");
			File.CreateText(path).Close();
			var nsf  = new NewSessionFile(path);
			Assert.IsTrue(ReflectionHelper.GetBoolResult(_viewModel, "CreateSingleSession", nsf));

			path = _tmpFolder.Combine("skunk.wma");
			File.CreateText(path).Close();
			nsf = new NewSessionFile(path);
			Assert.IsTrue(ReflectionHelper.GetBoolResult(_viewModel, "CreateSingleSession", nsf));

			Assert.AreEqual("lizard", _viewModel.FirstNewSessionAdded);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that passing a selected file object to the CreateSingleSession method
		/// creates a new session with the correct data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleSession()
		{
			InitProject();

			var path = _tmpFolder.Combine("rabbit.ogg");
			File.CreateText(path).Close();
			var nsf = new NewSessionFile(path);
			var date = File.GetLastWriteTime(path);
			Assert.IsTrue(ReflectionHelper.GetBoolResult(_viewModel, "CreateSingleSession", nsf));

			var session = Session.Create(_prj, "rabbit");
			Assert.AreEqual("rabbit", session.Id);
			Assert.AreEqual(date.ToString(), session.Date.ToString());
			Assert.AreEqual(1, session.Files.Length);
			Assert.AreEqual(Path.Combine(session.Folder, "rabbit.ogg"), session.Files[0]);
		}
	}
}
