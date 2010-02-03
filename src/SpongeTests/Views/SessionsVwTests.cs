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
using SilUtils;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SessionsVwTests
	{
		private const string kTestPrjName = "~~Moldy Sponge";

		private TestHostForm m_frmHost;
		private SpongeProject m_prj;
		private SessionsVw m_vw;
		private DataGridView m_gridFiles;
		private ListPanel m_lpSessions;
		private Label m_lblNoSessionsMsg;
		private Label m_lblEmptySessionMsg;
		private LinkLabel m_lnkSessionPath;
		private SessionFile[] m_currSessionFiles;

		private string[] m_sessionNames = new[] { "cake", "donuts", "icecream", "pie" };

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
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fixture tear down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void FixtureTearDown()
		{
			m_frmHost.Controls.Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create a test project before each test runs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_prj = ReflectionHelper.GetResult(typeof(SpongeProject),
				"Create", kTestPrjName) as SpongeProject;

			m_vw = new SessionsVw(m_prj);

			m_lblNoSessionsMsg = ReflectionHelper.GetField(m_vw, "lblNoSessionsMsg") as Label;
			m_lblEmptySessionMsg = ReflectionHelper.GetField(m_vw, "lblEmptySessionMsg") as Label;
			m_lnkSessionPath = ReflectionHelper.GetField(m_vw, "lnkSessionPath") as LinkLabel;
			m_lpSessions = ReflectionHelper.GetField(m_vw, "lpSessions") as ListPanel;
			m_gridFiles = ReflectionHelper.GetField(m_vw, "gridFiles") as DataGridView;
			m_currSessionFiles = ReflectionHelper.GetField(m_vw, "m_currSessionFiles") as SessionFile[];

			m_frmHost.Controls.Add(m_vw);
			m_frmHost.Show();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// After each test, remove the test project from the file system.
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

			//m_currSessionFiles = new SessionFile[3];
			//var path = m_prj.GetSessionFolder(m_sessionNames[3]);
			//var sessionFiles

			//var file = Path.Combine(path, "fred.pdf");
			//File.CreateText(file).Close();
			//m_prj.AddSessionFiles(
			//var sf = new SessionFile(file);
			//m_currSessionFiles[0] = sf;

			//file = Path.Combine(path, "barney.mp3");
			//File.CreateText(file).Close();
			//sf = new SessionFile(file);
			//m_currSessionFiles[1] = sf;

			//file = Path.Combine(path, "wilma.doc");
			//File.CreateText(file).Close();
			//sf = new SessionFile(file);
			//m_currSessionFiles[2] = sf;

			//ReflectionHelper.CallMethod(m_vw, "RefreshSessionList");
			//Assert.AreEqual(3, m_gridFiles.RowCount);

			//m_currSessionFiles = m_currProj.GetSessionFiles(lpSessions.CurrentItem);

			//lblEmptySessionMsg.Visible = (m_currSessionFiles != null && m_currSessionFiles.Length == 0);
			//lnkSessionPath.Visible = (m_currSessionFiles != null && m_currSessionFiles.Length == 0);
			//gridFiles.Visible = (m_currSessionFiles != null && m_currSessionFiles.Length > 0);

			//if (m_currSessionFiles != null)
			//{
			//    gridFiles.RowCount = m_currSessionFiles.Length;
			//    lnkSessionPath.Text = m_currProj.GetSessionFolder(lpSessions.CurrentItem);
			//}
		}
	}
}
