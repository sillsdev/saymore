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
// File: SessionsVwTests.cs
// Responsibility: Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.IO;
using System.Windows.Forms;
using NUnit.Framework;
using SIL.Sponge.Controls;
using SIL.Sponge.Model;
using SilUtils;

namespace SIL.Sponge.Views
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SessionsVwTests : TestBase
	{
		private TestHostForm _frmHost;
		private SessionsVw _vw;
		private DataGridView _gridFiles;
		private ListPanel _lpSessions;
		private Label _lblNoSessionsMsg;
		private Label _lblEmptySessionMsg;
		private LinkLabel _lnkSessionPath;

		private readonly string[] _sessionIds = new[] { "cake", "donuts", "icecream", "pie" };

		#region Fixture/Test Setup/TearDown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fixture setup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_frmHost = new TestHostForm();

			ReflectionHelper.SetField(
				typeof(SessionFileInfoTemplateList), "s_list", new SessionFileInfoTemplateList());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create a test project before each test runs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override void TestSetup()
		{
			base.TestSetup();
			InitProject();

			_vw = new SessionsVw(_prj, ()=> new[] {"JOE", "SUE"});

			_lblNoSessionsMsg = ReflectionHelper.GetField(_vw, "lblNoSessionsMsg") as Label;
			_lblEmptySessionMsg = ReflectionHelper.GetField(_vw, "lblEmptySessionMsg") as Label;
			_lnkSessionPath = ReflectionHelper.GetField(_vw, "lnkSessionPath") as LinkLabel;
			_lpSessions = ReflectionHelper.GetField(_vw, "lpSessions") as ListPanel;
			_gridFiles = ReflectionHelper.GetField(_vw, "gridFiles") as DataGridView;

			_frmHost.Controls.Clear();
			_frmHost.Controls.Add(_vw);
			_frmHost.Show();

			Palaso.Reporting.ErrorReport.IsOkToInteractWithUser = false;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the specified tab in the sessions view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetTab(string tpg)
		{
			var tabPg = ReflectionHelper.GetField(_vw, tpg) as TabPage;
			var tabctrl = ReflectionHelper.GetField(_vw, "tabSessions") as TabControl;
			tabctrl.SelectedTab = tabPg;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a set of test sessions to the test project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddTestSessions()
		{
			foreach (string session in _sessionIds)
				_prj.AddSession(session);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the RefreshSessionList method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test][Ignore("Need to refactor with a view model")]
		public void RefreshSessionList()
		{
			SetTab("tpgFiles");

			ReflectionHelper.CallMethod(_vw, "RefreshSessionList");

			Assert.IsTrue(_lblNoSessionsMsg.Visible);
			Assert.AreEqual(0, _gridFiles.RowCount);
			AddTestSessions();
			ReflectionHelper.CallMethod(_vw, "RefreshSessionList");
			Assert.IsFalse(_lblNoSessionsMsg.Visible);
			Assert.AreEqual(0, _gridFiles.RowCount);
			Assert.AreEqual(_prj.Sessions.Count, _lpSessions.Items.Length);

			for (int i = 0; i < _prj.Sessions.Count; i++)
				Assert.AreEqual(_prj.Sessions[i], _lpSessions.Items[i]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the RefreshFileList method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[Ignore("Need to refactor with a view model")]
		public void RefreshFileList()
		{
			SetTab("tpgFiles");

			ReflectionHelper.CallMethod(_vw, "RefreshFileList");
			Assert.IsFalse(_lblEmptySessionMsg.Visible);
			Assert.IsFalse(_lnkSessionPath.Visible);
			Assert.IsFalse(_gridFiles.Visible);

			AddTestSessions();
			ReflectionHelper.CallMethod(_vw, "RefreshSessionList");
			_lpSessions.CurrentItem = _sessionIds[1];

			Assert.IsTrue(_lblEmptySessionMsg.Visible);
			Assert.IsTrue(_lnkSessionPath.Visible);
			Assert.IsFalse(_gridFiles.Visible);

			Assert.AreEqual(0, _gridFiles.RowCount);

			var session = _lpSessions.CurrentItem as Session;

			// Add a file to the session
			var file = Path.Combine(session.Folder, "fred.pdf");
			File.CreateText(file).Close();
			session.AddFiles(new[] { file });
			ReflectionHelper.CallMethod(_vw, "RefreshFileList");
			var currSessionFiles = ReflectionHelper.GetField(_vw, "_currSessionFiles") as SessionFile[];

			// Verify a bunch of stuff after adding a single file.
			Assert.AreEqual(1, currSessionFiles.Length);
			Assert.AreEqual("fred.pdf", currSessionFiles[0].FileName);
			Assert.AreEqual(1, _gridFiles.RowCount);
			Assert.IsFalse(_lblEmptySessionMsg.Visible);
			Assert.IsFalse(_lnkSessionPath.Visible);
			Assert.IsTrue(_gridFiles.Visible);

			// Add a couple more files to the session.
			var file1 = Path.Combine(session.Folder, "barney.mp3");
			var file2 = Path.Combine(session.Folder, "wilma.doc");
			File.CreateText(file1).Close();
			File.CreateText(file2).Close();
			session.AddFiles(new[] { file1, file2 });
			ReflectionHelper.CallMethod(_vw, "RefreshFileList");
			currSessionFiles = ReflectionHelper.GetField(_vw, "_currSessionFiles") as SessionFile[];

			// Verify stuff after adding another two files, including order of file names.
			Assert.AreEqual(3, currSessionFiles.Length);
			Assert.AreEqual("barney.mp3", currSessionFiles[0].FileName);
			Assert.AreEqual("fred.pdf", currSessionFiles[1].FileName);
			Assert.AreEqual("wilma.doc", currSessionFiles[2].FileName);
			Assert.AreEqual(3, _gridFiles.RowCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the FileListDragOver method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FileListDragOver()
		{
			SetTab("tpgFiles");

			const int leftMouse = 1;
			//const int shift = 4 + leftMouse;
			const int ctrl = 8 + leftMouse;

			// Create an array of temp. files in the temp. folder.
			var tmpFiles = new[] {Path.GetTempFileName(),
				Path.GetTempFileName(), Path.GetTempFileName() };

			try
			{
				// Make sure there is no current session specified in the side panel sessions list.
				Assert.IsNull(_lpSessions.CurrentItem);

				// Test when there is no current session selected.
				var dragObj = new DataObject(DataFormats.FileDrop, tmpFiles);
				var args = new DragEventArgs(dragObj, leftMouse, 0, 0, DragDropEffects.All, DragDropEffects.All);
				ReflectionHelper.CallMethod(_vw, "FileListDragOver", new[] { null, args });
				Assert.AreEqual(DragDropEffects.None, args.Effect);

				// Add some sessions, update the side panel sessions list and set the current session.

				AddTestSessions();
				ReflectionHelper.CallMethod(_vw, "RefreshSessionList");
				_lpSessions.SelectItem(_sessionIds[3], false);
				Assert.AreEqual(_sessionIds[3], _lpSessions.CurrentItem.ToString());

				// Test when KeyState invalid.
				args = new DragEventArgs(dragObj, 0, 0, 0, DragDropEffects.All, DragDropEffects.All);
				ReflectionHelper.CallMethod(_vw, "FileListDragOver", new[] { null, args });
				Assert.AreEqual(DragDropEffects.None, args.Effect);

				// Test when data format is invalid.
				dragObj = new DataObject(DataFormats.Bitmap, tmpFiles);
				args = new DragEventArgs(dragObj, leftMouse, 0, 0, DragDropEffects.All, DragDropEffects.All);
				ReflectionHelper.CallMethod(_vw, "FileListDragOver", new[] { null, args });
				Assert.AreEqual(DragDropEffects.None, args.Effect);

				// Test when all info. is valid and CTRL and SHIFT are not pressed.
				dragObj = new DataObject(DataFormats.FileDrop, tmpFiles);
				args = new DragEventArgs(dragObj, leftMouse, 0, 0, DragDropEffects.All, DragDropEffects.All);
				ReflectionHelper.CallMethod(_vw, "FileListDragOver", new[] { null, args });
				Assert.AreEqual(DragDropEffects.Copy, args.Effect);

				// Test when all info. is valid and CTRL is pressed (i.e. file copy).
				args = new DragEventArgs(dragObj, ctrl, 0, 0, DragDropEffects.All, DragDropEffects.All);
				ReflectionHelper.CallMethod(_vw, "FileListDragOver", new[] { null, args });
				Assert.AreEqual(DragDropEffects.Copy, args.Effect);

				// TODO: Uncomment when move is supported
				// Test when all info. is valid and Shift is pressed (i.e. file move).
				//args = new DragEventArgs(dragObj, shift, 0, 0, DragDropEffects.All, DragDropEffects.All);
				//ReflectionHelper.CallMethod(_vw, "FileListDragOver", new[] { null, args });
				//Assert.AreEqual(DragDropEffects.Copy, args.Effect);
			}
			finally
			{
				foreach (string file in tmpFiles)
				{
					try { File.Delete(file); }
					catch { }
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the FileListDragDrop method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[Ignore("Need to refactor with a view model")]
		public void FileListDragDrop()
		{
			SetTab("tpgFiles");

			// Add some sessions, update the side panel sessions list and set the current session.
			AddTestSessions();
			ReflectionHelper.CallMethod(_vw, "RefreshSessionList");
			_lpSessions.SelectItem(_sessionIds[0], false);
			var session = _lpSessions.CurrentItem as Session;
			Assert.AreEqual(_sessionIds[0], session.Id);

			// Create an array of temp. files in the temp. folder.
			var tmpFiles = new[] {Path.GetTempFileName(), Path.GetTempFileName() };

			try
			{
				// Test when there is no data to be dropped.
				var dragObj = new DataObject(DataFormats.FileDrop, null);
				var args = new DragEventArgs(dragObj, 0, 0, 0, DragDropEffects.All, DragDropEffects.Copy);
				ReflectionHelper.CallMethod(_vw, "FileListDragDrop", new[] { null, args });
				var currSessionFiles = ReflectionHelper.GetField(_vw, "_currSessionFiles") as SessionFile[];
				Assert.AreEqual(0, currSessionFiles.Length);

				// Test copying dropped files.
				dragObj = new DataObject(DataFormats.FileDrop, tmpFiles);
				args = new DragEventArgs(dragObj, 0, 0, 0, DragDropEffects.All, DragDropEffects.Copy);
				ReflectionHelper.CallMethod(_vw, "FileListDragDrop", new[] { null, args });
				currSessionFiles = ReflectionHelper.GetField(_vw, "_currSessionFiles") as SessionFile[];
				Assert.AreEqual(2, currSessionFiles.Length);
				Assert.AreEqual(Path.GetFileName(tmpFiles[0]), currSessionFiles[0].FileName);
				Assert.AreEqual(Path.GetFileName(tmpFiles[1]), currSessionFiles[1].FileName);
				Assert.IsTrue(File.Exists(tmpFiles[0]));
				Assert.IsTrue(File.Exists(tmpFiles[1]));

				// Delete the two files just copied and refersh the view.
				File.Delete(Path.Combine(session.Folder, currSessionFiles[0].FileName));
				File.Delete(Path.Combine(session.Folder, currSessionFiles[1].FileName));
				ReflectionHelper.CallMethod(_vw, "RefreshFileList");

				// TODO: Uncomment when move is supported
				// Test moving dropped files.
				//dragObj = new DataObject(DataFormats.FileDrop, tmpFiles);
				//args = new DragEventArgs(dragObj, 0, 0, 0, DragDropEffects.Move, DragDropEffects.All);
				//ReflectionHelper.CallMethod(_vw, "FileListDragDrop", new[] { null, args });
				//currSessionFiles = ReflectionHelper.GetField(_vw, "_currSessionFiles") as SessionFile[];
				//Assert.AreEqual(2, currSessionFiles.Length);
				//Assert.AreEqual(Path.GetFileName(tmpFiles[0]), currSessionFiles[0].FileName);
				//Assert.AreEqual(Path.GetFileName(tmpFiles[1]), currSessionFiles[1].FileName);
				//Assert.IsFalse(File.Exists(tmpFiles[0]));
				//Assert.IsFalse(File.Exists(tmpFiles[1]));
			}
			finally
			{
				foreach (string file in tmpFiles)
				{
					try { File.Delete(file); }
					catch { }
				}
			}
		}
	}
}
