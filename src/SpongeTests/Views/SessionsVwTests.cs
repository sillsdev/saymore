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

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SessionsVwTests : TestBase
	{
		private TestHostForm m_frmHost;
		private SessionsVw m_vw;
		private DataGridView m_gridFiles;
		private ListPanel m_lpSessions;
		private Label m_lblNoSessionsMsg;
		private Label m_lblEmptySessionMsg;
		private LinkLabel m_lnkSessionPath;

		private readonly string[] m_sessionNames = new[] { "cake", "donuts", "icecream", "pie" };

		#region Fixture/Test Setup/TearDown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fixture setup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			m_frmHost = new TestHostForm();

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

			m_vw = new SessionsVw(m_prj);

			m_lblNoSessionsMsg = ReflectionHelper.GetField(m_vw, "lblNoSessionsMsg") as Label;
			m_lblEmptySessionMsg = ReflectionHelper.GetField(m_vw, "lblEmptySessionMsg") as Label;
			m_lnkSessionPath = ReflectionHelper.GetField(m_vw, "lnkSessionPath") as LinkLabel;
			m_lpSessions = ReflectionHelper.GetField(m_vw, "lpSessions") as ListPanel;
			m_gridFiles = ReflectionHelper.GetField(m_vw, "gridFiles") as DataGridView;

			m_frmHost.Controls.Clear();
			m_frmHost.Controls.Add(m_vw);
			m_frmHost.Show();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the specified tab in the sessions view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetTab(string tpg)
		{
			var tabPg = ReflectionHelper.GetField(m_vw, tpg) as TabPage;
			var tabctrl = ReflectionHelper.GetField(m_vw, "tabSessions") as TabControl;
			tabctrl.SelectedTab = tabPg;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a set of test sessions to the test project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddTestSessions()
		{
			foreach (string session in m_sessionNames)
				m_prj.AddSession(session);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the RefreshSessionList method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void RefreshSessionList()
		{
			SetTab("tpgFiles");

			ReflectionHelper.CallMethod(m_vw, "RefreshSessionList");

			Assert.IsTrue(m_lblNoSessionsMsg.Visible);
			Assert.AreEqual(0, m_gridFiles.RowCount);
			AddTestSessions();
			ReflectionHelper.CallMethod(m_vw, "RefreshSessionList");
			Assert.IsFalse(m_lblNoSessionsMsg.Visible);
			Assert.AreEqual(0, m_gridFiles.RowCount);
			Assert.AreEqual(m_prj.Sessions.Count, m_lpSessions.Items.Length);

			for (int i = 0; i < m_prj.Sessions.Count; i++)
				Assert.AreEqual(m_prj.Sessions[i], m_lpSessions.Items[i]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the RefreshFileList method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void RefreshFileList()
		{
			SetTab("tpgFiles");

			ReflectionHelper.CallMethod(m_vw, "RefreshFileList");
			Assert.IsFalse(m_lblEmptySessionMsg.Visible);
			Assert.IsFalse(m_lnkSessionPath.Visible);
			Assert.IsFalse(m_gridFiles.Visible);

			AddTestSessions();
			ReflectionHelper.CallMethod(m_vw, "RefreshSessionList");
			m_lpSessions.CurrentItem = m_sessionNames[1];

			Assert.IsTrue(m_lblEmptySessionMsg.Visible);
			Assert.IsTrue(m_lnkSessionPath.Visible);
			Assert.IsFalse(m_gridFiles.Visible);

			Assert.AreEqual(0, m_gridFiles.RowCount);

			var session = m_lpSessions.CurrentItem as Session;

			// Add a file to the session
			var file = Path.Combine(session.SessionPath, "fred.pdf");
			File.CreateText(file).Close();
			session.AddFiles(new[] { file });
			ReflectionHelper.CallMethod(m_vw, "RefreshFileList");
			var currSessionFiles = ReflectionHelper.GetField(m_vw, "m_currSessionFiles") as SessionFile[];

			// Verify a bunch of stuff after adding a single file.
			Assert.AreEqual(1, currSessionFiles.Length);
			Assert.AreEqual("fred.pdf", currSessionFiles[0].FileName);
			Assert.AreEqual(1, m_gridFiles.RowCount);
			Assert.IsFalse(m_lblEmptySessionMsg.Visible);
			Assert.IsFalse(m_lnkSessionPath.Visible);
			Assert.IsTrue(m_gridFiles.Visible);

			// Add a couple more files to the session.
			var file1 = Path.Combine(session.SessionPath, "barney.mp3");
			var file2 = Path.Combine(session.SessionPath, "wilma.doc");
			File.CreateText(file1).Close();
			File.CreateText(file2).Close();
			session.AddFiles(new[] { file1, file2 });
			ReflectionHelper.CallMethod(m_vw, "RefreshFileList");
			currSessionFiles = ReflectionHelper.GetField(m_vw, "m_currSessionFiles") as SessionFile[];

			// Verify stuff after adding another two files, including order of file names.
			Assert.AreEqual(3, currSessionFiles.Length);
			Assert.AreEqual("barney.mp3", currSessionFiles[0].FileName);
			Assert.AreEqual("fred.pdf", currSessionFiles[1].FileName);
			Assert.AreEqual("wilma.doc", currSessionFiles[2].FileName);
			Assert.AreEqual(3, m_gridFiles.RowCount);
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
				Assert.IsNull(m_lpSessions.CurrentItem);

				// Test when there is no current session selected.
				var dragObj = new DataObject(DataFormats.FileDrop, tmpFiles);
				var args = new DragEventArgs(dragObj, leftMouse, 0, 0, DragDropEffects.All, DragDropEffects.All);
				ReflectionHelper.CallMethod(m_vw, "FileListDragOver", new[] { null, args });
				Assert.AreEqual(DragDropEffects.None, args.Effect);

				// Add some sessions, update the side panel sessions list and set the current session.
				AddTestSessions();
				ReflectionHelper.CallMethod(m_vw, "RefreshSessionList");
				m_lpSessions.CurrentItem = m_sessionNames[3];
				Assert.AreEqual(m_sessionNames[3], m_lpSessions.CurrentItem.ToString());

				// Test when KeyState invalid.
				args = new DragEventArgs(dragObj, 0, 0, 0, DragDropEffects.All, DragDropEffects.All);
				ReflectionHelper.CallMethod(m_vw, "FileListDragOver", new[] { null, args });
				Assert.AreEqual(DragDropEffects.None, args.Effect);

				// Test when data format is invalid.
				dragObj = new DataObject(DataFormats.Bitmap, tmpFiles);
				args = new DragEventArgs(dragObj, leftMouse, 0, 0, DragDropEffects.All, DragDropEffects.All);
				ReflectionHelper.CallMethod(m_vw, "FileListDragOver", new[] { null, args });
				Assert.AreEqual(DragDropEffects.None, args.Effect);

				// Test when all info. is valid and CTRL and SHIFT are not pressed.
				dragObj = new DataObject(DataFormats.FileDrop, tmpFiles);
				args = new DragEventArgs(dragObj, leftMouse, 0, 0, DragDropEffects.All, DragDropEffects.All);
				ReflectionHelper.CallMethod(m_vw, "FileListDragOver", new[] { null, args });
				Assert.AreEqual(DragDropEffects.Copy, args.Effect);

				// Test when all info. is valid and CTRL is pressed (i.e. file copy).
				args = new DragEventArgs(dragObj, ctrl, 0, 0, DragDropEffects.All, DragDropEffects.All);
				ReflectionHelper.CallMethod(m_vw, "FileListDragOver", new[] { null, args });
				Assert.AreEqual(DragDropEffects.Copy, args.Effect);

				// TODO: Uncomment when move is supported
				// Test when all info. is valid and Shift is pressed (i.e. file move).
				//args = new DragEventArgs(dragObj, shift, 0, 0, DragDropEffects.All, DragDropEffects.All);
				//ReflectionHelper.CallMethod(m_vw, "FileListDragOver", new[] { null, args });
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
		public void FileListDragDrop()
		{
			SetTab("tpgFiles");

			// Add some sessions, update the side panel sessions list and set the current session.
			AddTestSessions();
			ReflectionHelper.CallMethod(m_vw, "RefreshSessionList");
			m_lpSessions.CurrentItem = m_sessionNames[0];
			var session = m_lpSessions.CurrentItem as Session;
			Assert.AreEqual(m_sessionNames[0], session.Name);

			// Create an array of temp. files in the temp. folder.
			var tmpFiles = new[] {Path.GetTempFileName(), Path.GetTempFileName() };

			try
			{
				// Test when there is no data to be dropped.
				var dragObj = new DataObject(DataFormats.FileDrop, null);
				var args = new DragEventArgs(dragObj, 0, 0, 0, DragDropEffects.Copy, DragDropEffects.All);
				ReflectionHelper.CallMethod(m_vw, "FileListDragDrop", new[] { null, args });
				var currSessionFiles = ReflectionHelper.GetField(m_vw, "m_currSessionFiles") as SessionFile[];
				Assert.AreEqual(0, currSessionFiles.Length);

				// Test when the allowed effects is not copy or move.
				dragObj = new DataObject(DataFormats.FileDrop, tmpFiles);
				args = new DragEventArgs(dragObj, 0, 0, 0, DragDropEffects.Link, DragDropEffects.All);
				ReflectionHelper.CallMethod(m_vw, "FileListDragDrop", new[] { null, args });
				currSessionFiles = ReflectionHelper.GetField(m_vw, "m_currSessionFiles") as SessionFile[];
				Assert.AreEqual(0, currSessionFiles.Length);

				// Test copying dropped files.
				dragObj = new DataObject(DataFormats.FileDrop, tmpFiles);
				args = new DragEventArgs(dragObj, 0, 0, 0, DragDropEffects.Copy, DragDropEffects.All);
				ReflectionHelper.CallMethod(m_vw, "FileListDragDrop", new[] { null, args });
				currSessionFiles = ReflectionHelper.GetField(m_vw, "m_currSessionFiles") as SessionFile[];
				Assert.AreEqual(2, currSessionFiles.Length);
				Assert.AreEqual(Path.GetFileName(tmpFiles[0]), currSessionFiles[0].FileName);
				Assert.AreEqual(Path.GetFileName(tmpFiles[1]), currSessionFiles[1].FileName);
				Assert.IsTrue(File.Exists(tmpFiles[0]));
				Assert.IsTrue(File.Exists(tmpFiles[1]));

				// Delete the two files just copied and refersh the view.
				File.Delete(Path.Combine(session.SessionPath, currSessionFiles[0].FileName));
				File.Delete(Path.Combine(session.SessionPath, currSessionFiles[1].FileName));
				ReflectionHelper.CallMethod(m_vw, "RefreshFileList");

				// TODO: Uncomment when move is supported
				// Test moving dropped files.
				//dragObj = new DataObject(DataFormats.FileDrop, tmpFiles);
				//args = new DragEventArgs(dragObj, 0, 0, 0, DragDropEffects.Move, DragDropEffects.All);
				//ReflectionHelper.CallMethod(m_vw, "FileListDragDrop", new[] { null, args });
				//currSessionFiles = ReflectionHelper.GetField(m_vw, "m_currSessionFiles") as SessionFile[];
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
