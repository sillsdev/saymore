using System;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;
using SayMore.Properties;

namespace SayMoreTests.Model
{
	[TestFixture]
	public class ProjectTests
	{
		private TemporaryFolder _parentFolder;

		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("projectTest");
		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}


		[Test]
		public void Load_AfterSave_IsoPreserved()
		{
			{
				string settingsPath = _parentFolder.Combine("foo." + Project.ProjectSettingsFileExtension);
				var project = new Project(settingsPath);
				project.Iso639Code = "abc";
				project.Save();

				var project2 = new Project(settingsPath);
				Assert.AreEqual("abc",project2.Iso639Code);
			}
		}

		/*
		[Test, ExpectedException(typeof(FileNotFoundException))]
		public void FromSettingsFilePath_FileNotFound_Throws()
		{
			Project.FromSettingsFilePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates),
													  "foo." + Project.ProjectSettingsFileExtension));
		}

		[Test]
		public void FromSettingsFilePath_FileIsFound_GivesProject()
		{
			using (var parent = new Palaso.TestUtilities.TemporaryFolder("parent"))
			{
				var project = Project.CreateAtLocation(parent.Path, "foo", TODO);
				Assert.IsNotNull(Project.FromSettingsFilePath(project.SettingsFilePath));

			}
		}

		[Test]
		public void FromSettingsFilePath_SettingsPathNameDiffersFromDirectory_OK()
		{
			using (var parent = new Palaso.TestUtilities.TemporaryFolder("parent"))
			{
				var originalProject = Project.CreateAtLocation(parent.Path, "foo", TODO);

				//rename the settings, but not the directory, and reload
				string changedSettingsPath = originalProject.SettingsFilePath.Replace("foo." + Project.ProjectSettingsFileExtension, "baa." + Project.ProjectSettingsFileExtension);
				File.Move(originalProject.SettingsFilePath, changedSettingsPath);
				Project loadedProject = Project.FromSettingsFilePath(changedSettingsPath);
				Assert.IsNotNull(loadedProject);
				Assert.AreEqual(originalProject.FolderPath, loadedProject.FolderPath);
				Assert.AreEqual(changedSettingsPath, loadedProject.SettingsFilePath);
			}
		}

		[Test]
		public void Persistance_WritesAndReadsIso()
		{
			using (var parent = new Palaso.TestUtilities.TemporaryFolder("parent"))
			{
				var project = Project.CreateAtLocation(parent.Path, "foo", TODO);
				project.Iso639Code = "abc";
				project.Save();
				var sameProject = Project.FromSettingsFilePath(project.SettingsFilePath);
				Assert.AreEqual("abc", sameProject.Iso639Code);
			}
		}
		*/

		[Test, ExpectedException(typeof(ArgumentException))]
		[Ignore("Instantiating a project will create the necessary folders. Is this test of any use?")]
		public void Constructor_ParentFolderDoesNotExist_Throws()
		{
			var path = _parentFolder.Combine("NotThere", "foo", "foo." + Project.ProjectSettingsFileExtension);
			new Project(path);
		}

		[Test]
		public void Constructor_EverythingOk_CreatesFolderAndSettingsFile()
		{
				CreateProject(_parentFolder);
				Assert.IsTrue(File.Exists(_parentFolder.Combine("foo", "foo."+Project.ProjectSettingsFileExtension)));
		}

		private static Project CreateProject(TemporaryFolder parent)
		{
			return new Project(parent.Combine("foo","foo." + Project.ProjectSettingsFileExtension));
		}

		[Test]
		public void RenameSessionsToEvents_FolderRename_RenamesFolder()
		{
			var prj = CreateProject(_parentFolder);
			var eventPath = CreateEventFolder(prj);

			Assert.IsFalse(Directory.Exists(prj.SessionsFolder));
			Assert.IsTrue(Directory.Exists(eventPath));

			prj.RenameEventsToSessions(prj.FolderPath);

			Assert.IsTrue(Directory.Exists(prj.SessionsFolder));
			Assert.IsFalse(Directory.Exists(eventPath));
		}

		[Test]
		public void RenameSessionsToEvents_FileRename_RenamesFileExtension()
		{
			var prj = CreateProject(_parentFolder);
			var eventPath = CreateEventFolder(prj);
			var eventFile = CreateEventFile(eventPath);

			Assert.AreEqual(".event", Path.GetExtension(eventFile));
			Assert.IsTrue(File.Exists(eventFile));

			prj.RenameEventsToSessions(prj.FolderPath);

			Assert.IsFalse(File.Exists(eventFile));
			var sessionFile = Path.ChangeExtension(eventFile, Settings.Default.SessionFileExtension);
			sessionFile = Path.GetFileName(sessionFile);
			Assert.IsTrue(File.Exists(Path.Combine(prj.SessionsFolder, sessionFile)));
		}

		[Test]
		public void RenameSessionsToEvents_TagRename_RenamesTagsInFile()
		{
			var prj = CreateProject(_parentFolder);
			var eventPath = CreateEventFolder(prj);
			var eventFile = CreateEventFile(eventPath);

			var contents = XElement.Load(eventFile);
			Assert.AreEqual("Event", contents.Name.LocalName);

			prj.RenameEventsToSessions(prj.FolderPath);

			var sessionFile = Path.ChangeExtension(eventFile, Settings.Default.SessionFileExtension);
			sessionFile = Path.GetFileName(sessionFile);
			sessionFile = Path.Combine(prj.SessionsFolder, sessionFile);

			contents = XElement.Load(sessionFile);
			Assert.AreEqual("Session", contents.Name.LocalName);
		}

		private static string CreateEventFolder(Project prj)
		{
			var sessionPath = Path.Combine(prj.FolderPath, "Events");
			Directory.CreateDirectory(sessionPath);
			return sessionPath;
		}

		private static string CreateEventFile(string sessionPath)
		{
			var sessionFile = Path.Combine(sessionPath, "foodoo.event");
			XElement session = new XElement("Event");
			session.Save(sessionFile);
			return sessionFile;
		}
	}
}
