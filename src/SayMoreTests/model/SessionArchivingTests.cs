using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using SIL.Reporting;
using SIL.TestUtilities;
using SIL.Archiving.IMDI;
using SayMore;
using SayMore.Model;
using SayMore.Model.Files;
using SIL.Archiving;
using SayMore.Properties;

namespace SayMoreTests.Utilities
{
	[TestFixture]
	public class SessionArchivingTests
	{
		private string _dummyProjectName;
		private TemporaryFolder _tmpFolder;
		private DummySession _session;
		private Mock<Person> _person;
		private Mock<PersonInformant> _personInformant;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			ErrorReport.IsOkToInteractWithUser = false;

			_dummyProjectName = "ArchiveHelperTestFolder";
			_tmpFolder = new TemporaryFolder(_dummyProjectName);

			CreateSessionAndMockedPerson();
		}

		/// ------------------------------------------------------------------------------------
		private void CreateSessionAndMockedPerson()
		{
			// Create a person
			var folder = Path.Combine(_tmpFolder.Path, Person.kFolderName);
			Directory.CreateDirectory(folder);
			folder = Path.Combine(folder, "ddo-person");
			Directory.CreateDirectory(folder);
			File.CreateText(Path.Combine(folder, "ddo-person.person")).Close();
			File.CreateText(Path.Combine(folder, "ddoPic.jpg")).Close();
			File.CreateText(Path.Combine(folder, "ddoVoice.wav")).Close();

			_person = new Mock<Person>();
			_person.Setup(p => p.FolderPath).Returns(Path.Combine(Path.Combine(_tmpFolder.Path, Person.kFolderName), "ddo-person"));
			_person.Setup(p => p.Id).Returns("ddo-person");

			_personInformant = new Mock<PersonInformant>();
			_personInformant.Setup(i => i.GetPersonByNameOrCode("ddo-person")).Returns(_person.Object);

			// Create a session
			var parentFolder = Path.Combine(_tmpFolder.Path, Session.kFolderName);
			Directory.CreateDirectory(parentFolder);
			folder = Path.Combine(parentFolder, "ddo-session");
			Directory.CreateDirectory(folder);
			File.CreateText(Path.Combine(folder, "ddo.session")).Close();
			File.CreateText(Path.Combine(folder, "ddo.mpg")).Close();
			File.CreateText(Path.Combine(folder, "ddo.mp3")).Close();
			File.CreateText(Path.Combine(folder, "ddo.pdf")).Close();
			_session = new DummySession(parentFolder, "ddo", _personInformant.Object);

			// create a project file
			var projFileName = _dummyProjectName + Settings.Default.ProjectFileExtension;
			File.CreateText(Path.Combine(_tmpFolder.Path, projFileName)).Close();
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_tmpFolder.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void SetFilesToArchive_RAMP_GetsCorrectListSizeIncludingSessionFile()
		{
			var model = new Mock<RampArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo",
				"ddo-session", "whatever", null, new Func<string, string, string>((a, b) =>a));
			model.Setup(s => s.AddFileGroup(string.Empty, It.Is<IEnumerable<string>>(e => e.Count() == 4), "Adding Files for Session 'ddo'"));
			model.Setup(s => s.AddFileGroup("ddo-person", It.Is<IEnumerable<string>>(e => e.Count() == 3), "Adding Files for Contributor 'ddo-person'"));
			_session.SetFilesToArchive(model.Object);
			model.VerifyAll();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void SetFilesToArchive_IMDI_GetsCorrectListSizeExcludingSessionFile()
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo",
				"ddo-session", "whatever", false, null, @"C:\my_imdi_folder");
			model.Setup(s => s.AddFileGroup(string.Empty, It.Is<IEnumerable<string>>(e => e.Count() == 3), "Adding Files for Session 'ddo'"));
			model.Setup(s => s.AddFileGroup("ddo-person", It.Is<IEnumerable<string>>(e => e.Count() == 2), "Adding Files for Contributor 'ddo-person'"));
			_session.SetFilesToArchive(model.Object);
			model.VerifyAll();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void SetFilesToArchive_GetsCorrectSessionAndPersonFiles()
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo", "ddo-session", "whatever", false, null, @"C:\my_imdi_folder");

			model.Setup(s => s.AddFileGroup(string.Empty,
				It.Is<IEnumerable<string>>(e => e.Select(Path.GetFileName).Union(new [] {"ddo.session", "ddo.mpg", "ddo.mp3", "ddo.pdf"}).Count() == 4),
				"Adding Files for Session 'ddo'"));

			model.Setup(s => s.AddFileGroup("ddo-person",
				It.Is<IEnumerable<string>>(e => e.Select(Path.GetFileName).Union(new[] { "ddoPic.jpg", "ddoVoice.wav" }).Count() == 2),
				"Adding Files for Contributor 'ddo-person'"));

			_session.SetFilesToArchive(model.Object);
			model.VerifyAll();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void SetFilesToArchive_ParticipantFileDoesNotExist_DoesNotCrash()
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo", "ddo-session", "whatever", false, null, @"C:\my_imdi_folder");
			_session._participants = new[] { "ddo-person", "non-existant-person" };

			model.Setup(s => s.AddFileGroup(string.Empty,
				It.Is<IEnumerable<string>>(e => e.Select(Path.GetFileName).Union(new [] {"ddo.session", "ddo.mpg", "ddo.mp3", "ddo.pdf"}).Count() == 4),
				"Adding Files for Session 'ddo'"));

			model.Setup(s => s.AddFileGroup("ddo-person",
				It.Is<IEnumerable<string>>(e => e.Select(Path.GetFileName).Union(new[] { "ddoPic.jpg", "ddoVoice.wav" }).Count() == 2),
				"Adding Files for Contributor 'ddo-person'"));

			_session.SetFilesToArchive(model.Object);
			model.VerifyAll();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalDurationOfSourceMedia_ThreeSourceMediaFiles_ReturnsTotalTime()
		{
			var sourceRoleArray = new[] { ApplicationContainer.ComponentRoles.First(r => r.Id == ComponentRole.kSourceComponentRoleId) };

			var sourceMediaFile1 = new Mock<ComponentFile>();
			sourceMediaFile1.Setup(f => f.DurationSeconds).Returns(new TimeSpan(0, 50, 10));
			sourceMediaFile1.Setup(f => f.GetAssignedRoles()).Returns(sourceRoleArray);
			var sourceMediaFile2 = new Mock<ComponentFile>();
			sourceMediaFile2.Setup(f => f.DurationSeconds).Returns(new TimeSpan(3, 20, 3));
			sourceMediaFile2.Setup(f => f.GetAssignedRoles()).Returns(sourceRoleArray);
			var sourceMediaFile3 = new Mock<ComponentFile>();
			sourceMediaFile3.Setup(f => f.DurationSeconds).Returns(new TimeSpan(0, 4, 27));
			sourceMediaFile3.Setup(f => f.GetAssignedRoles()).Returns(sourceRoleArray);

			_session._mediaFiles = new[] { sourceMediaFile1.Object, sourceMediaFile2.Object, sourceMediaFile3.Object };
			Assert.AreEqual(new TimeSpan(4, 14, 40), _session.GetTotalDurationOfSourceMedia());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalDurationOfSourceMedia_TwoSourceMediaFilesAndOneNonSourceMediaFile_ReturnsTotalTime()
		{
			var sourceRoleArray = new[] { ApplicationContainer.ComponentRoles.First(r => r.Id == ComponentRole.kSourceComponentRoleId) };

			var sourceMediaFile1 = new Mock<ComponentFile>();
			sourceMediaFile1.Setup(f => f.DurationSeconds).Returns(new TimeSpan(0, 50, 10));
			sourceMediaFile1.Setup(f => f.GetAssignedRoles()).Returns(sourceRoleArray);
			var sourceMediaFile2 = new Mock<ComponentFile>();
			sourceMediaFile2.Setup(f => f.DurationSeconds).Returns(new TimeSpan(3, 20, 3));
			sourceMediaFile2.Setup(f => f.GetAssignedRoles()).Returns(new[]
				{
					ApplicationContainer.ComponentRoles.First(r => r.Id == ComponentRole.kConsentComponentRoleId),
					ApplicationContainer.ComponentRoles.First(r => r.Id == ComponentRole.kSourceComponentRoleId)
				});
			var sourceMediaFile3 = new Mock<ComponentFile>();
			sourceMediaFile3.Setup(f => f.DurationSeconds).Returns(new TimeSpan(0, 4, 27));
			sourceMediaFile3.Setup(f => f.GetAssignedRoles()).Returns(new[]
				{
					ApplicationContainer.ComponentRoles.First(r => r.Id == ComponentRole.kConsentComponentRoleId)
				});

			_session._mediaFiles = new[] { sourceMediaFile1.Object, sourceMediaFile2.Object, sourceMediaFile3.Object };
			Assert.AreEqual(new TimeSpan(4, 10, 13), _session.GetTotalDurationOfSourceMedia());
		}

		#region IMDI Archiving Tests

		[Test]
		public void GetProjectName_ReturnsCorrectProjectName()
		{
			var projname = _session.GetProjectName();
			Assert.AreEqual(_dummyProjectName, projname);
		}

		[Test]
		public void InitializeActor_AgeTest()
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo", "ddo-session", "whatever",
				false, null, @"C:\my_imdi_folder");
			var person = new Mock<Person>();
			person.Setup(p => p.MetaDataFile.GetStringValue("privacyProtection", "false")).Returns("false");
			person.Setup(p => p.MetaDataFile.GetStringValue("birthYear", string.Empty)).Returns(string.Empty);
			var actor = ArchivingHelper.InitializeActor(model.Object, person.Object, DateTime.MinValue);
			Assert.AreEqual("0", actor.Age);
			model.VerifyAll();
		}

		[Test]
		public void InitializeActor_Age68Test()
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo", "ddo-session", "whatever",
				false, null, @"C:\my_imdi_folder");
			var person = new Mock<Person>();
			person.Setup(p => p.MetaDataFile.GetStringValue("privacyProtection", "false")).Returns("false");
			person.Setup(p => p.MetaDataFile.GetStringValue("birthYear", string.Empty)).Returns("1950");
			var actor = ArchivingHelper.InitializeActor(model.Object, person.Object, new DateTime(2018, 1, 1));
			Assert.AreEqual("68", actor.Age);
			person.VerifyAll();
			model.VerifyAll();
		}

		[Test]
		public void GetOneLanguage_DefinedIsoTest()
		{
			var returnValue = ArchivingHelper.GetOneLanguage("eng");
			Assert.AreEqual("eng", returnValue.Iso3Code);
			Assert.AreEqual("English", returnValue.LanguageName);
			Assert.AreEqual("English", returnValue.EnglishName);
		}

		[Test]
		public void GetOneLanguage_DefinedNameTest()
		{
			var returnValue = ArchivingHelper.GetOneLanguage("English");
			Assert.AreEqual("eng", returnValue.Iso3Code);
			Assert.AreEqual("English", returnValue.LanguageName);
			Assert.AreEqual("English", returnValue.EnglishName);
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GetOneLanguage_UndefinedIsoTest()
		{
			var returnValue = ArchivingHelper.GetOneLanguage("tru");
			Assert.AreEqual("tru", returnValue.Iso3Code);
			Assert.AreEqual("Turoyo", returnValue.LanguageName);
			Assert.AreEqual("Turoyo", returnValue.EnglishName);
		}

		[Test]
		public void GetOneLanguage_DefaultIsoTest()
		{
			ArchivingHelper._defaultLanguage = new ArchivingLanguage("tru", "Turoyo", "Turoyo");
			var returnValue = ArchivingHelper.GetOneLanguage("tru");
			Assert.AreEqual("tru", returnValue.Iso3Code);
			Assert.AreEqual("Turoyo", returnValue.LanguageName);
			Assert.AreEqual("Turoyo", returnValue.EnglishName);
		}

		[Test]
		public void GetOneLanguage_DefaultNameTest()
		{
			ArchivingHelper._defaultLanguage = new ArchivingLanguage("tru", "Turoyo", "Turoyo");
			var returnValue = ArchivingHelper.GetOneLanguage("Turoyo");
			Assert.AreEqual("tru", returnValue.Iso3Code);
			Assert.AreEqual("Turoyo", returnValue.LanguageName);
			Assert.AreEqual("Turoyo", returnValue.EnglishName);
		}
		#endregion
	}

	public class DummySession : Session
	{
		public string[] _participants;
		private readonly Mock<ProjectElementComponentFile> _metaFile = new Mock<ProjectElementComponentFile>();
		public ComponentFile[] _mediaFiles;

		public DummySession(string parentFolder, string name, PersonInformant personInformant, params string [] actors) : base(parentFolder, name + "-session", null, new SessionFileType(() => null, () => null),
				MakeComponent, new XmlFileSerializer(null), (w, x, y, z) =>
					new ProjectElementComponentFile(w, x, y, z, FieldUpdater.CreateMinimalFieldUpdaterForTests(null)),
					ApplicationContainer.ComponentRoles, personInformant, null)
		{
			if (actors == null || actors.Length == 0)
				_participants = new[] {"ddo-person"};
			else
				_participants = actors;
			_metaFile.Setup(m => m.GetStringValue(SessionFileType.kTitleFieldName, null)).Returns(name);
		}

		public override IEnumerable<string> GetAllParticipants()
		{
			return _participants;
		}

		public override ProjectElementComponentFile MetaDataFile
		{
			get { return _metaFile.Object; }
		}

		public override ComponentFile[] GetComponentFiles()
		{
			lock (this)
			{
				// Return a copy of the list to guard against changes
				// on another thread (i.e., from the FileSystemWatcher)
				if (_componentFiles == null)
				{
					_componentFiles = new HashSet<ComponentFile>();

					// This is the actual person or session data
					_componentFiles.Add(MetaDataFile);

					foreach (ComponentFile componentFile in _mediaFiles)
						_componentFiles.Add(componentFile);
				}
				return _componentFiles.ToArray();
			}
		}

		private static ComponentFile MakeComponent(ProjectElement parentElement, string pathtoannotatedfile)
		{
			return ComponentFile.CreateMinimalComponentFileForTests(parentElement, pathtoannotatedfile);
		}
	}


}
