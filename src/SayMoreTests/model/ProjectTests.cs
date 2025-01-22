using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Moq;
using NUnit.Framework;
using SIL.TestUtilities;
using SIL.Archiving.IMDI;
using SayMore;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMoreTests.Utilities;
using SIL.Archiving;
using System.Threading;

namespace SayMoreTests.Model
{
	[TestFixture]
	public class RenameEventsToSessionsTests
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
		public void RenameEventsToSessions_FolderRename_RenamesFolder()
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
		public void RenameEventsToSessions_FileRename_RenamesFileExtension()
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
		public void RenameEventsToSessions_TagRename_RenamesTagsInFile()
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

		#region Private helper methods
		private Project CreateProject(TemporaryFolder parent)
		{
			return new Project(parent.Combine("foo", "foo." + Project.ProjectSettingsFileExtension), null, null);
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
		#endregion
	}


	[TestFixture]
	public class ProjectTests
	{
		private TemporaryFolder _parentFolder;
		private Mock<PersonInformant> _personInformant;
		private ApplicationContainer _appContext;
		private ProjectContext _projectContext;
		private List<Session> _dummySessions;

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			Console.WriteLine($"{Environment.NewLine}ApplicationContainer.ProductVersion: " +
				$"{ApplicationContainer.ProductVersion}");
		}

		[SetUp]
		public void Setup()
		{
			_dummySessions = new List<Session>();
			_parentFolder = new TemporaryFolder("projectTest");


			_appContext = new ApplicationContainer();
			_projectContext = CreateProjectContext(_appContext);
		}

		[TearDown]
		public void TearDown()
		{
			_projectContext.Dispose();
			_projectContext = null;
			_appContext.Dispose();
			_appContext = null;
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		private ElementRepository<Session> GetSessionRepo(string projectDirectory, string elementGroupName, FileType type)
		{
			var repo = new Mock<ElementRepository<Session>>(projectDirectory, elementGroupName, type,
				_projectContext.ResolveForTests<ElementRepository<Session>.ElementFactory>());
			repo.Setup(r => r.AllItems).Returns(_dummySessions);
			return repo.Object;
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

		[Test]
		[Ignore("Instantiating a project will create the necessary folders. Is this test of any use?")]
		public void Constructor_ParentFolderDoesNotExist_Throws()
		{
			var path = _parentFolder.Combine("NotThere", "foo", "foo." + Project.ProjectSettingsFileExtension);

			Assert.Throws<ArgumentException>(() => new Project(path, _projectContext.ResolveForTests<ElementRepository<Session>.Factory>(),
				_projectContext.ResolveForTests<SessionFileType>()));
		}

		[Test, Apartment(ApartmentState.STA)]
		public void Constructor_EverythingOk_CreatesFolderAndSettingsFile()
		{
			CreateProject(_parentFolder);
			Assert.IsTrue(File.Exists(_parentFolder.Combine("foo", "foo." + Project.ProjectSettingsFileExtension)));
		}

		#region SetFilesToArchive Tests
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void SetFilesToArchive_GetsCorrectSessionAndPersonFiles()
		{
			var prj = CreateProject(_parentFolder);
			string person1 = CreateMockedPerson("ddo");
			string person2 = CreateMockedPerson("tlb");
			string person3 = CreateMockedPerson("tws");
			_dummySessions.Add(CreateDummySession("The Frog Dance", person1, person2));
			_dummySessions.Add(CreateDummySession("Underwater Marriage", person2));
			_dummySessions.Add(CreateDummySession("Why Rice Can't Fly", person1, person3));
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "foo",
				"foo", true, null, @"c:\my_imdi_folder");

			// session files
			model.Setup(s => s.AddFileGroup(_dummySessions[0].Id, It.Is<IEnumerable<string>>(e => e.Count() == 3), $"Adding Files for Session '{_dummySessions[0].Title}'"));
			model.Setup(s => s.AddFileGroup(_dummySessions[1].Id, It.Is<IEnumerable<string>>(e => e.Count() == 3), $"Adding Files for Session '{_dummySessions[1].Title}'"));
			model.Setup(s => s.AddFileGroup(_dummySessions[2].Id, It.Is<IEnumerable<string>>(e => e.Count() == 3), $"Adding Files for Session '{_dummySessions[2].Title}'"));

			// contributor files
			model.Setup(s => s.AddFileGroup("\n" + person1, It.Is<HashSet<string>>(e => e.Count == 2), "Adding Files for Contributors..."));
			model.Setup(s => s.AddFileGroup("\n" + person2, It.Is<HashSet<string>>(e => e.Count == 2), "Adding Files for Contributors..."));
			model.Setup(s => s.AddFileGroup("\n" + person3, It.Is<HashSet<string>>(e => e.Count == 2), "Adding Files for Contributors..."));

			prj.SetFilesToArchive(model.Object, default);
			model.VerifyAll();
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void SetFilesToArchiveOnRAMP_MockAProjectWithThreePeople_TestNumberOfFilesForEachPerson()
		{
			var prj = CreateProject(_parentFolder);
			string person1 = CreateMockedPerson("ddo");
			string person2 = CreateMockedPerson("tlb");
			string person3 = CreateMockedPerson("tws");
			_dummySessions.Add(CreateDummySession("The Frog Dance", person1, person2));
			_dummySessions.Add(CreateDummySession("Underwater Marriage", person2));
			_dummySessions.Add(CreateDummySession("Why Rice Can't Fly", person1, person3));
			var model = new Mock<RampArchivingDlgViewModel>(MockBehavior.Default, "SayMore", "ddo",
				"ddo-session", null, new Func<string, string, string>((a, b) => a));

			// session files
			model.Setup(s => s.AddFileGroup(_dummySessions[0].Id, It.Is<IEnumerable<string>>(e => e.Count() == 3), $"Adding Files for Session '{_dummySessions[0].Title}'"));
			model.Setup(s => s.AddFileGroup(_dummySessions[1].Id, It.Is<IEnumerable<string>>(e => e.Count() == 3), $"Adding Files for Session '{_dummySessions[1].Title}'"));
			model.Setup(s => s.AddFileGroup(_dummySessions[2].Id, It.Is<IEnumerable<string>>(e => e.Count() == 3), $"Adding Files for Session '{_dummySessions[2].Title}'"));

			// contributor files
			model.Setup(s => s.AddFileGroup("\n" + person1, It.Is<HashSet<string>>(e => e.Count == 2), "Adding Files for Contributors..."));
			model.Setup(s => s.AddFileGroup("\n" + person2, It.Is<HashSet<string>>(e => e.Count == 2), "Adding Files for Contributors..."));
			model.Setup(s => s.AddFileGroup("\n" + person3, It.Is<HashSet<string>>(e => e.Count == 2), "Adding Files for Contributors..."));

			model.Setup(f => f.AddFileGroup(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
				.Callback((string groupId, IEnumerable<string> files, string msg) => TrackFiles(groupId, files, msg));

			prj.SetFilesToArchive(model.Object, default);
			Assert.AreEqual(7, _fileList.Count);
			Assert.AreEqual(2, (from f in _fileList where f.Contains(person1) select f).Count());
			Assert.AreEqual(2, (from f in _fileList where f.Contains(person2) select f).Count());
			Assert.AreEqual(2, (from f in _fileList where f.Contains(person3) select f).Count());
			_fileList.Clear();
		}

		private readonly List<string> _fileList = new List<string>();

		private void TrackFiles(string groupId, IEnumerable<string> files, string msg)
		{
			_fileList.AddRange(files);
		}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//[Category("SkipOnTeamCity")]
		//public void SetFilesToArchive_ParticipantFileDoNotExist_DoesNotCrash()
		//{
		//    var model = new Mock<ArchivingDlgViewModel>(MockBehavior.Loose, "SayMore", "ddo", "ddo-session", null);
		//    _session._participants = new[] { "ddo-person", "non-existant-person" };
		//    _session.SetFilesToArchive(model.Object);
		//	model.VerifyAll();
		//}
		#endregion

		[Test, Apartment(ApartmentState.STA)]
		public void SetContributorsListToSession_ContributorsInMetaAndSession_AllContributorsInSession()
		{
			WriteXmlResource("Qustan059.session");  // Two contributors: Iskender Demirel, Lahdo Agirman
			WriteXmlResource("Qustan059.WAV.meta"); // One contributor: Greg Trihus
			WriteXmlResource("Qustan059_Source.mp4.meta"); // Two contributors: Eliyo Acar, Lahdo Agirman
			WriteXmlResource("Qustan059-1.pdf.meta");   // One contributor: Eliyo Acar
			ProjectContext.SetContributorsListToSession(Path.GetDirectoryName(SessionScenarioPath));
			var sessionXml = new XmlDocument();
			using (var xmlReader = XmlReader.Create(Path.Combine(SessionScenarioPath, "Qustan059.session")))
			{
				sessionXml.Load(xmlReader);
			}
			Assert.AreEqual(4, sessionXml.SelectNodes("//contributor").Count, "Expecting four contributors: Eliyo Acar, Greg Trihus, Iskender Demirel, Lahdo Agirman");
			Assert.AreEqual(4, sessionXml.SelectSingleNode("//participants").InnerText.Split(';').Length, "Expecting four participants (with roles): Iskender Demirel, Lahdo Agirman, Greg Trihus, Eliyo Acar");
		}

		[Test, Apartment(ApartmentState.STA)]
		public void SetContributorsListToSession_LegacyFile_AddsContributors_KeepsParticipants()
		{
			WriteXmlResource("OldSession.session");  // Two contributors: Iskender Demirel, Lahdo Agirman
			ProjectContext.SetContributorsListToSession(Path.GetDirectoryName(SessionScenarioPath));
			var sessionXml = new XmlDocument();
			using (var xmlReader = XmlReader.Create(Path.Combine(SessionScenarioPath, "OldSession.session")))
			{
				sessionXml.Load(xmlReader);
			}
			var participants = sessionXml.SelectSingleNode("//participants").InnerText.Split(';').Select(x => x.Trim());
			Assert.AreEqual(2, participants.Count(), "Expecting two participants (without roles): Iskender Demirel, Lahdo Agirman");
			Assert.That(participants, Has.Member("Iskender Demirel"));
			Assert.That(participants, Has.Member("Lahdo Agirman"));
			var contributors = sessionXml.SelectNodes("//contributions/contributor").Cast<XmlElement>();
			Assert.AreEqual(2, contributors.Count(), "Expecting two contributors: Iskender Demirel, Lahdo Agirman");
			var iskender = contributors.FirstOrDefault(n =>
				n.GetElementsByTagName("name").Cast<XmlElement>().First().InnerText == "Iskender Demirel");
			Assert.That(iskender, Is.Not.Null);
			// This verifies the current behavior, making sure it doesn't change unexpectedly.
			// However, it's by no means certain that this is the most helpful role to assign
			// someone who was previously a participant of unknown role. If 'unknown' at some
			// point becomes a valid role we migth want to switch to it; or if we make it OK
			// not to specify a role.
			Assert.That(iskender.GetElementsByTagName("role").Cast<XmlElement>().First().InnerText, Is.EqualTo("participant"));
		}

		[Test, Apartment(ApartmentState.STA)]
		public void SetContributorsListToSession_MergesParticpantsAndContributors()
		{
			// File has two contributors: Iskender Demirel, Lahdo Agirman; two participants, Iskender Demirel and Joe
			// Iskender has a different role in the two lists. I don't ever expect this to happen, because the only
			// program ever to write roles into participants was a beta of SayMore which kept the two consistent.
			// For this reason I have not made the code smart enough to create an additional contributor with a different
			// role in this case; however, the test does verify that the contributor version wins and the presence of a
			// role in participants doesn't mess things up. For the same reason I have not made the code use
			// role information from the participants at all, even when adding to contributors.
			WriteXmlResource("MixedSession.session");
			ProjectContext.SetContributorsListToSession(Path.GetDirectoryName(SessionScenarioPath));
			var sessionXml = new XmlDocument();
			using (var xmlReader = XmlReader.Create(Path.Combine(SessionScenarioPath, "MixedSession.session")))
			{
				sessionXml.Load(xmlReader);
			}
			var participants = sessionXml.SelectSingleNode("//participants").InnerText.Split(';').Select(x => x.Trim());
			Assert.AreEqual(3, participants.Count(), "Expecting two participants (without roles): Iskender Demirel, Lahdo Agirman, Joe");
			Assert.That(participants, Has.Member("Iskender Demirel"));
			Assert.That(participants, Has.Member("Lahdo Agirman"));
			Assert.That(participants, Has.Member("Joe"));
			var contributors = sessionXml.SelectNodes("//contributions/contributor").Cast<XmlElement>();
			Assert.AreEqual(3, contributors.Count(), "Expecting three contributors: Iskender Demirel, Lahdo Agirman, Joe");
			var iskender = contributors.FirstOrDefault(n =>
				n.GetElementsByTagName("name").Cast<XmlElement>().First().InnerText == "Iskender Demirel");
			Assert.That(iskender, Is.Not.Null);
			Assert.That(iskender.GetElementsByTagName("role").Cast<XmlElement>().First().InnerText, Is.EqualTo("consultant"), "Should have kept the role from existing contributor");
			var joe = contributors.FirstOrDefault(n =>
				n.GetElementsByTagName("name").Cast<XmlElement>().First().InnerText == "Joe");
			Assert.That(joe, Is.Not.Null);
			Assert.That(joe.GetElementsByTagName("role").Cast<XmlElement>().First().InnerText, Is.EqualTo("participant"), "Should have supplied a default role for Joe");
		}

		#region Private helper methods
		private ProjectContext CreateProjectContext(ApplicationContainer appContext)
		{
			return appContext.CreateProjectContext(_parentFolder.Combine("foo", "foo." + Project.ProjectSettingsFileExtension));
		}

		private Project CreateProject(TemporaryFolder parent)
		{
			return new Project(parent.Combine("foo", "foo." + Project.ProjectSettingsFileExtension),
				GetSessionRepo, _projectContext.ResolveForTests<SessionFileType>());
		}

		/// ------------------------------------------------------------------------------------
		private string CreateMockedPerson(string name)
		{
			var folder = Path.Combine(_parentFolder.Path, "foo", Person.kFolderName);
			Directory.CreateDirectory(folder);
			var personName = name + "-person";
			folder = Path.Combine(folder, personName);
			Directory.CreateDirectory(folder);
			File.CreateText(Path.Combine(folder, name + "Pic.jpg")).Close();
			File.CreateText(Path.Combine(folder, name + "Voice.wav")).Close();

			var person = new Mock<Person>();
			person.Setup(p => p.FolderPath).Returns(Path.Combine(Path.Combine(_parentFolder.Path, "foo", Person.kFolderName), personName));
			person.Setup(p => p.Id).Returns(personName);

			if (_personInformant == null)
				_personInformant = new Mock<PersonInformant>(MockBehavior.Loose);
			_personInformant.Setup(i => i.GetPersonByNameOrCode(personName)).Returns(person.Object);
			return personName;
		}


		/// ------------------------------------------------------------------------------------
		private DummySession CreateDummySession(string name, params string[] actors)
		{
			// Create a session
			var parentFolder = Path.Combine(_parentFolder.Path, "foo", Session.kFolderName);
			Directory.CreateDirectory(parentFolder);
			var sessionName = name + "-session";
			var folder = Path.Combine(parentFolder, sessionName);
			Directory.CreateDirectory(folder);
			//File.CreateText(Path.Combine(folder, sessionName + ".session")).Close();
			File.CreateText(Path.Combine(folder, name + ".mpg")).Close();
			File.CreateText(Path.Combine(folder, name + ".mp3")).Close();
			File.CreateText(Path.Combine(folder, name + ".pdf")).Close();
			var session = new DummySession(parentFolder, name, _personInformant.Object, actors);

			return session;
		}

		/// ------------------------------------------------------------------------------------
		private static string SessionScenarioPath = null;
		private void WriteXmlResource(string file)
		{
			if (SessionScenarioPath == null)
			{
				SessionScenarioPath = Path.Combine(_parentFolder.Path, "sessions", "ContributorsScenario");
			}

			if (!Directory.Exists(SessionScenarioPath))
					Directory.CreateDirectory(SessionScenarioPath);

			using (var sessionData =
				new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SayMoreTests.model.Data." + file)))
			{
				using (var sessionFile = new StreamWriter(Path.Combine(SessionScenarioPath, file)))
				{
					sessionFile.Write(sessionData.ReadToEnd());
				}
			}
		}

		#endregion
	}
}
