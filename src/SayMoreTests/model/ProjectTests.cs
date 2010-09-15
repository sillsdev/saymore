using System;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;

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
			var sessionPath = CreateSessionFolder(prj);

			Assert.IsFalse(Directory.Exists(prj.EventsFolder));
			Assert.IsTrue(Directory.Exists(sessionPath));

			prj.RenameSessionsToEvents(prj.FolderPath);

			Assert.IsTrue(Directory.Exists(prj.EventsFolder));
			Assert.IsFalse(Directory.Exists(sessionPath));
		}

		[Test]
		public void RenameSessionsToEvents_FileRename_RenamesFileExtension()
		{
			var prj = CreateProject(_parentFolder);
			var sessionPath = CreateSessionFolder(prj);
			var sessionFile = CreateSessionFile(sessionPath);

			Assert.AreEqual(".session", Path.GetExtension(sessionFile));
			Assert.IsTrue(File.Exists(sessionFile));

			prj.RenameSessionsToEvents(prj.FolderPath);

			Assert.IsFalse(File.Exists(sessionFile));
			var eventFile = Path.ChangeExtension(sessionFile, "event");
			eventFile = Path.GetFileName(eventFile);
			Assert.IsTrue(File.Exists(Path.Combine(prj.EventsFolder, eventFile)));
		}

		[Test]
		public void RenameSessionsToEvents_TagRename_RenamesTagsInFile()
		{
			var prj = CreateProject(_parentFolder);
			var sessionPath = CreateSessionFolder(prj);
			var sessionFile = CreateSessionFile(sessionPath);

			var contents = XElement.Load(sessionFile);
			Assert.AreEqual("Session", contents.Name.LocalName);

			prj.RenameSessionsToEvents(prj.FolderPath);

			var eventFile = Path.ChangeExtension(sessionFile, "event");
			eventFile = Path.GetFileName(eventFile);
			eventFile = Path.Combine(prj.EventsFolder, eventFile);

			contents = XElement.Load(eventFile);
			Assert.AreEqual("Event", contents.Name.LocalName);
		}

		private static string CreateSessionFolder(Project prj)
		{
			var sessionPath = Path.Combine(prj.FolderPath, "Sessions");
			Directory.CreateDirectory(sessionPath);
			return sessionPath;
		}

		private static string CreateSessionFile(string sessionPath)
		{
			var sessionFile = Path.Combine(sessionPath, "foodoo.session");
			XElement session = new XElement("Session");
			session.Save(sessionFile);
			return sessionFile;
		}
	}
}
