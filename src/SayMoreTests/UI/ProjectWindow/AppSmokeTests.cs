using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Localization;
using SayMore;
using SayMore.Properties;
using SayMore.UI.ElementListScreen;
using SayMore.UI.LowLevelControls;
using Palaso.TestUtilities;
using SilUtils;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace SayMoreTests.UI.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class AppSmokeTests
	{
		private ProjectContext _projectContext;
		private ApplicationContainer _applicationContainer;
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
			Settings.Default.DefaultFolderForNewProjects = _projectsFolder.Path;
			Settings.Default.PreventDeleteElementSystemConfirmationMessage = true;

			_applicationContainer = new ApplicationContainer();
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			if (_projectContext != null)
				_projectContext.Dispose();

			_projectsFolder.Dispose();
			_projectsFolder = null;
		}

		/// ------------------------------------------------------------------------------------
		private void CopySampleProject()
		{
			var sampleDataPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(6);
			sampleDataPath += @"\..\..\SampleData";
			CopyDir(sampleDataPath, _projectsFolder.Path);
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
		private void SetupProjectWindow()
		{
			SetupProjectWindow(null);
		}

		/// ------------------------------------------------------------------------------------
		private void SetupProjectWindow(string prjFile)
		{
			prjFile = (prjFile ?? Directory.GetFiles(_projectsFolder.Path, "*.sprj", SearchOption.AllDirectories)[0]);
			_projectContext = _applicationContainer.CreateProjectContext(prjFile);
			_projectContext.ProjectWindow.Show();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Application_WalkThrough_DoesNotCrash()
		{
			CopySampleProject();
			SetupProjectWindow();

			ClickPeopleTab();
			WalkThroughElements("PersonEditor", "_peopleListPanel", "_personComponentFileGrid", "PersonListScreen");

			ClickEventTab();
			WalkThroughElements("EventEditor", "_eventsListPanel", "_eventComponentFileGrid", "EventsListScreen");

			_projectContext.ProjectWindow.Close();
			SetupProjectWindow();
			_projectContext.ProjectWindow.Close();
		}

		/// ------------------------------------------------------------------------------------
		private static void ClickEventTab()
		{
			var eventTabTester = new ControlTester("EventsViewTab", "ProjectWindow");
			eventTabTester.Click();
			Assert.IsTrue((bool)eventTabTester["Selected"]);
		}

		/// ------------------------------------------------------------------------------------
		private static void ClickPeopleTab()
		{
			var peopleTabTester = new ControlTester("PeopleViewTab", "ProjectWindow");
			peopleTabTester.Click();
			Assert.IsTrue((bool)peopleTabTester["Selected"]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Select each event or person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WalkThroughElements(string editorName, string listPanelName,
			string componentGridName, string screenName)
		{
			var idTextBoxTester = new TextBoxTester(editorName + "._tableLayout._id", "ProjectWindow");
			var listPanel = _projectContext.ProjectWindow.Controls.Find(listPanelName, true)[0] as ListPanel;
			var list = listPanel.ListControl as ElementGrid;

			for (int i = 0; i < list.RowCount; i++)
			{
				list.SelectElement(i);
				Assert.AreEqual(list.GetCurrentElement().Id, idTextBoxTester.Text);
				WalkThroughComponentFiles(componentGridName, screenName);
			}

			DeleteItems(listPanel, screenName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Select each component file on the event and people screen.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void WalkThroughComponentFiles(string componentGridName, string screenName)
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
		/// <summary>
		/// Click on each tab on the event or people screen.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void WalkThroughComponentEditorTabs(string screenName)
		{
			var screenTester = new ControlTester(screenName, "ProjectWindow");
			var tabCtrl = screenTester["SelectedComponentEditorsTabControl"] as TabControl;

			for (int i = 0; i < tabCtrl.TabCount; i++)
				tabCtrl.SelectTab(i);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes all the events, or all the people.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void DeleteItems(ListPanel listPanel, string screenName)
		{
			using (var modalFormTester = new ModalFormTester())
			{
				var list = listPanel.ListControl as ElementGrid;
				int maxDeleteAttempts = 100;
				var delButtonTester = new ButtonTester("_buttonDelete", "DeleteMessageBox");

				while (list.RowCount > 0)
				{
					list.SelectElement(0);
					modalFormTester.ExpectModal("DeleteMessageBox", delButtonTester.Click);
					listPanel.DeleteButton.PerformClick();

					if (--maxDeleteAttempts == 0)
						throw new Exception(string.Format("Deleting items from {0} failed.", screenName));
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Application_CreateProject_DoesNotCrash()
		{
			CreateProject();

			// Add new event.
			var idTextBoxTester = AddItem("EventEditor", "_eventsListPanel");

			// Add a component file to the event.
			var filePath = Path.Combine(_projectsFolder.Path, "dummyFile.png");
			(new Bitmap(1, 1)).Save(filePath);

			var componentGrid = _projectContext.ProjectWindow.Controls.Find(
				"_eventComponentFileGrid", true)[0] as ComponentFileGrid;

			// Can't add the file via clicking the add button because we cannot control the
			// open file dialog box unless this class were to derive from NUnitFormTest, but
			// that causes other problems.
			componentGrid.FilesAdded(new[] { filePath });
			Assert.AreEqual("dummyFile.png", componentGrid.Grid[1, 1].Value as string);

			var listPanel = GetListPanelByName("_eventsListPanel");
			var list = listPanel.ListControl as ElementGrid;

			// Rename the event.
			idTextBoxTester.Enter("RenamedEvent");
			idTextBoxTester.FireEvent("Validating", new CancelEventArgs());
			Assert.AreEqual("RenamedEvent", idTextBoxTester.Text);
			Assert.AreEqual("RenamedEvent", list.GetCurrentElement().Id);
			Assert.AreEqual("RenamedEvent.event", componentGrid.Grid[1, 0].Value as string);

			// Delete the event.
			DeleteItems(listPanel, "EventListScreen");
			Assert.AreEqual(0, list.RowCount);

			// Open the dialog to add files from a device, then cancel it.
			using (var modalFormTester = new ModalFormTester())
			{
				var cancelButtonTester = new ButtonTester("_cancelButton");
				modalFormTester.ExpectModal("NewEventsFromFilesDlg", cancelButtonTester.Click);
				var newButtonTester = new ButtonTester("_buttonNewFromFiles");
				newButtonTester.Click();
			}

			ClickPeopleTab();

			// Add a new person.
			listPanel = GetListPanelByName("_peopleListPanel");
			list = listPanel.ListControl as ElementGrid;
			idTextBoxTester = AddItem("PersonEditor", "_peopleListPanel");

			// Delete the person.
			DeleteItems(listPanel, "PersonListScreen");
			Assert.AreEqual(0, list.RowCount);
		}

		/// ------------------------------------------------------------------------------------
		private void CreateProject()
		{
			using (var modalFormTester = new ModalFormTester())
			{
				var createButtonTester = new ToolStripButtonTester("_buttonCreate");
				var nameTextBoxTester = new TextBoxTester("_nameTextBox");
				var okButtonTester = new ButtonTester("_buttonOK");

				modalFormTester.ExpectModal("WelcomeDialog", createButtonTester.Click);
				modalFormTester.ExpectModal("NewProjectDlg", () =>
				{
					nameTextBoxTester.Properties.Text = "Boring New Project Name";
					okButtonTester.Click();
				});

				using (var dlg = _applicationContainer.CreateWelcomeDialog())
				{
					dlg.ShowDialog();
					SetupProjectWindow(dlg.Model.ProjectSettingsFilePath);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an event or a person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private TextBoxTester AddItem(string editorName, string listPanelName)
		{
			var idTextBoxTester = new TextBoxTester(editorName + "._tableLayout._id", "ProjectWindow");
			var listPanel = GetListPanelByName(listPanelName);
			var list = listPanel.ListControl as ElementGrid;
			var origCount = list.RowCount;
			listPanel.NewButton.PerformClick();
			Assert.AreEqual(origCount + 1, list.RowCount);
			Assert.AreNotEqual(string.Empty, idTextBoxTester.Text);

			return idTextBoxTester;
		}

		/// ------------------------------------------------------------------------------------
		private ListPanel GetListPanelByName(string listPanelName)
		{
			return _projectContext.ProjectWindow.Controls.Find(listPanelName, true)[0] as ListPanel;
		}
	}
}
