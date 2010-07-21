using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using NUnit.Extensions.Forms;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;
using SayMore.UI.LowLevelControls;
using SayMore.UI.ProjectWindow;
using SIL.Localization;
using SilUtils;

namespace SayMoreTests.UI.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class AppSmokeTests : NUnitFormTest
	{
		private static ProjectContext _projectContext;
		private static ApplicationContainer _applicationContainer;
		private TemporaryFolder _projectsFolder;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			Palaso.Reporting.ErrorReport.IsOkToInteractWithUser = false;
			LocalizationManager.Enabled = false;

			_projectsFolder = new TemporaryFolder("SayMoreSmokeTest");
			PortableSettingsProvider.SettingsFileFolder = _projectsFolder.Combine("Settings");
			Settings.Default.MRUList = MruFiles.Initialize(Settings.Default.MRUList, 4);

			var sampleDataPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(6);
			sampleDataPath += @"\..\..\SampleData";
			CopyDir(sampleDataPath, _projectsFolder.Path);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public override void TearDown()
		{
			if (_projectContext != null)
				_projectContext.Dispose();

			_projectsFolder.Dispose();
			_projectsFolder = null;

			base.TearDown();
		}

		/// ------------------------------------------------------------------------------------
		private static void CopyDir(string src, string dst)
		{
			if (!Directory.Exists(dst))
				Directory.CreateDirectory(dst);

			foreach (var entry in Directory.GetFileSystemEntries(src, "*.*"))
			{
				var dstName = Path.Combine(dst, Path.GetFileName(entry));

				if (Directory.Exists(entry))
					CopyDir(entry, dstName);
				else
					File.Copy(entry, dstName, true);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Application_WalkThrough_DoesNotCrash()
		{
			SetupProjectWindow();

			var peopleTabTester = new ControlTester("PeopleViewTab", "ProjectWindow");
			peopleTabTester.Click();
			Assert.IsTrue((bool)peopleTabTester["Selected"]);
			WalkThroughElements("PersonEditor", "_peopleListPanel", "_personComponentFileGrid", "PersonListScreen");

			var sessionTabTester = new ControlTester("SessionsViewTab", "ProjectWindow");
			sessionTabTester.Click();
			Assert.IsTrue((bool)sessionTabTester["Selected"]);
			WalkThroughElements("SessionEditor", "_sessionsListPanel", "_sessionComponentFileGrid", "SessionScreen");

			_projectContext.ProjectWindow.Close();
			SetupProjectWindow();
			_projectContext.ProjectWindow.Close();
		}

		/// ------------------------------------------------------------------------------------
		private void SetupProjectWindow()
		{
			var prjFiles = Directory.GetFiles(_projectsFolder.Path, "*.sprj", SearchOption.AllDirectories);
			_applicationContainer = new ApplicationContainer();
			_projectContext = _applicationContainer.CreateProjectContext(prjFiles[0]);
			_projectContext.ProjectWindow.Show();
		}

		/// ------------------------------------------------------------------------------------
		private void WalkThroughElements(string editorName, string listPanelName,
			string componentGridName, string screenName)
		{
			var idTextBoxTester = new TextBoxTester(editorName + "._tableLayout._id", "ProjectWindow");
			var listPanel = _projectContext.ProjectWindow.Controls.Find(listPanelName, true)[0] as ListPanel;
			var lvItems = listPanel.ListView;

			for (int i = 0; i < lvItems.Items.Count; i++)
			{
				lvItems.SelectedItems.Clear();
				lvItems.FocusedItem = lvItems.Items[i];
				lvItems.Items[i].Selected = true;
				Assert.AreEqual(lvItems.Items[i].Text, idTextBoxTester.Text);
				WalkThroughComponentFiles(componentGridName, screenName);
			}

			DeleteItems(listPanel, screenName);
		}

		/// ------------------------------------------------------------------------------------
		private void WalkThroughComponentFiles(string componentGridName, string screenName)
		{
			var gridTester = new ControlTester(componentGridName, "ProjectWindow");
			var grid = gridTester["Grid"] as DataGridView;

			foreach (DataGridViewRow row in grid.Rows)
			{
				grid.CurrentCell = grid[0, row.Index];
				WalkThroughComponentEditorTabs(screenName);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void WalkThroughComponentEditorTabs(string screenName)
		{
			var screenTester = new ControlTester(screenName, "ProjectWindow");
			var tabCtrl = screenTester["SelectedComponentEditorsTabControl"] as TabControl;

			for (int i = 0; i < tabCtrl.TabCount; i++)
				tabCtrl.SelectTab(i);
		}

		/// ------------------------------------------------------------------------------------
		private void DeleteItems(ListPanel listPanel, string screenName)
		{
			var lvItems = listPanel.ListView;

			using (var modalFormTester = new ModalFormTester())
			{
				int maxDeleteAttempts = 100;
				var delButtonTester = new ButtonTester("_buttonDelete");

				while (lvItems.Items.Count > 0)
				{
					lvItems.SelectedItems.Clear();
					lvItems.FocusedItem = lvItems.Items[0];
					lvItems.Items[0].Selected = true;
					modalFormTester.ExpectModal("DeleteMessageBox", delButtonTester.Click);
					listPanel.DeleteButton.PerformClick();

					if (--maxDeleteAttempts == 0)
						throw new Exception(string.Format("Deleting items from {0} failed.", screenName));
				}
			}
		}
	}
}
